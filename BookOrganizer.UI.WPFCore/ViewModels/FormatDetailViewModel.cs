using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using BookOrganizer.Domain;
using BookOrganizer.Domain.Services;
using BookOrganizer.UI.WPFCore.Wrappers;
using Prism.Commands;
using Prism.Events;
using Serilog;

namespace BookOrganizer.UI.WPFCore.ViewModels
{
    public class FormatDetailViewModel : BaseDetailViewModel<Format, FormatWrapper>
    {
        private FormatWrapper selectedItem;
        private readonly IFormatLookupDataService formatLookupDataService;

        public FormatDetailViewModel(IEventAggregator eventAggregator,
            ILogger logger,
            IDomainService<Format> domainService,
            IFormatLookupDataService formatLookupDataService)
            : base(eventAggregator, logger, domainService)
        {
            this.formatLookupDataService = formatLookupDataService ?? throw new ArgumentNullException(nameof(formatLookupDataService));

            ChangeEditedFormatCommand = new DelegateCommand<Guid?>(OnChangeEditedFormatExecute);

            SelectedItem = new FormatWrapper(domainService.CreateItem());

            UserMode = (!UserMode.Item1, DetailViewState.EditMode, Brushes.LightGray, !UserMode.Item4).ToTuple();

            Formats = new ObservableCollection<LookupItem>();
        }

        public ICommand ChangeEditedFormatCommand { get; }

        public override FormatWrapper SelectedItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value ?? throw new ArgumentNullException(nameof(SelectedItem));
                OnPropertyChanged();
            }
        }

        public ObservableCollection<LookupItem> Formats { get; set; }

        public async override Task LoadAsync(Guid id)
        {
            var format = await domainService.Repository.GetSelectedAsync(id) ?? new Format();

            SelectedItem = CreateWrapper(format);

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
                if (!Formats.Any())
                {
                    Formats.Clear();

                    foreach (var item in await GetFormatList())
                    {
                        Formats.Add(item);
                    }
                }
            }
        }

        private async Task<IEnumerable<LookupItem>> GetFormatList()
            => await formatLookupDataService.GetFormatLookupAsync(nameof(FormatDetailViewModel));

        private async void OnChangeEditedFormatExecute(Guid? formatId)
        {
            if (this.domainService.Repository.HasChanges())
            {
                //var result = await metroDialogService
                //   .ShowOkCancelDialogAsync(
                //   "You have made changes. Changing editable format will loose all unsaved changes. Are you sure you still want to switch?",
                //   "Close the view?");

                //if (result == MessageDialogResult.Canceled)
                //{
                //    return;
                //}
            }

            domainService.Repository.ResetTracking(SelectedItem.Model);
            HasChanges = domainService.Repository.HasChanges();

            await LoadAsync((Guid)formatId);
        }

        public override FormatWrapper CreateWrapper(Format entity)
        {
            var wrapper = new FormatWrapper(entity);
            return wrapper;
        }
    }
}
