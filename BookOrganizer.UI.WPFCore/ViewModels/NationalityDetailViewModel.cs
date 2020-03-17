using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using BookOrganizer.Domain;
using BookOrganizer.Domain.Services;
using BookOrganizer.UI.WPFCore.DialogServiceManager;
using BookOrganizer.UI.WPFCore.Wrappers;
using Prism.Commands;
using Prism.Events;
using Serilog;

namespace BookOrganizer.UI.WPFCore.ViewModels
{
    public class NationalityDetailViewModel : BaseDetailViewModel<Nationality, NationalityWrapper>
    {
        private NationalityWrapper selectedItem;
        private readonly INationalityLookupDataService nationalityLookupDataService;

        public NationalityDetailViewModel(IEventAggregator eventAggregator,
            ILogger logger,
            IDomainService<Nationality> domainService,
            INationalityLookupDataService nationalityLookupDataService,
            IDialogService dialogService)
            : base(eventAggregator, logger, domainService, dialogService)
        {
            this.nationalityLookupDataService = nationalityLookupDataService ?? throw new ArgumentNullException(nameof(nationalityLookupDataService));

            ChangeEditedNationalityCommand = new DelegateCommand<Guid?>(OnChangeEditedNationalityExecute);
            SaveItemCommand = new DelegateCommand(base.SaveItemExecute, base.SaveItemCanExecute)
                .ObservesProperty(() => SelectedItem.Name);

            SelectedItem = CreateWrapper(domainService.CreateItem());

            Nations = new ObservableCollection<LookupItem>();

            UserMode = (!UserMode.Item1, DetailViewState.EditMode, Brushes.LightGray, !UserMode.Item4).ToTuple();
        }

        public ICommand ChangeEditedNationalityCommand { get; }

        public override NationalityWrapper SelectedItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value ?? throw new ArgumentNullException(nameof(SelectedItem));
                OnPropertyChanged();
            }
        }

        public ObservableCollection<LookupItem> Nations { get; set; }

        protected override string CreateChangeMessage(DatabaseOperation operation)
        {
            return $"{operation.ToString()}: {SelectedItem.Name}.";
        }

        public async override Task LoadAsync(Guid id)
        {
            var nationality = await domainService.Repository.GetSelectedAsync(id) ?? new Nationality();

            SelectedItem = CreateWrapper(nationality);

            SelectedItem.PropertyChanged += (s, e) =>
            {
                if (!HasChanges)
                {
                    HasChanges = domainService.Repository.HasChanges();
                }
                if (e.PropertyName == nameof(SelectedItem.HasErrors))
                {
                    ((DelegateCommand)SaveItemCommand).RaiseCanExecuteChanged();
                }
                if (e.PropertyName == nameof(SelectedItem.Name)
                    || e.PropertyName == nameof(SelectedItem.Name))
                {
                    TabTitle = SelectedItem.Name;
                }
            };
            ((DelegateCommand)SaveItemCommand).RaiseCanExecuteChanged();

            Id = id;

            if (Id != default)
            {
                TabTitle = SelectedItem.Name;
            }
            else
            {
                SelectedItem.Name = SelectedItem.Model.Name;
            }

            await InitializeFormatCollection();

            async Task InitializeFormatCollection()
            {
                if (!Nations.Any())
                {
                    Nations.Clear();

                    foreach (var item in await GetNationalityList())
                    {
                        Nations.Add(item);
                    }
                }
            }
        }

        private async Task<IEnumerable<LookupItem>> GetNationalityList()
            => await nationalityLookupDataService.GetNationalityLookupAsync(nameof(NationalityDetailViewModel));

        private async void OnChangeEditedNationalityExecute(Guid? nationalityId)
        {
            if (this.domainService.Repository.HasChanges())
            {
                var dialog = new OkCancelViewModel("Close the view?", "You have made changes. Changing editable nationality will loose all unsaved changes. Are you sure you still want to switch?");
                var result = dialogService.OpenDialog(dialog);

                if (result == DialogResult.No)
                {
                    return;
                }
            }

            domainService.Repository.ResetTracking(SelectedItem.Model);
            HasChanges = domainService.Repository.HasChanges();

            await LoadAsync((Guid)nationalityId);
        }

        public override NationalityWrapper CreateWrapper(Nationality entity)
        {
            var wrapper = new NationalityWrapper(entity);
            return wrapper;
        }
    }
}
