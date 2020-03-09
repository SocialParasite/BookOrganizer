using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using BookOrganizer.Domain;
using BookOrganizer.Domain.Services;
using BookOrganizer.UI.WPFCore.Services;
using BookOrganizer.UI.WPFCore.Wrappers;
using Prism.Commands;
using Prism.Events;
using Serilog;

namespace BookOrganizer.UI.WPFCore.ViewModels
{
    public class PublisherDetailViewModel : BaseDetailViewModel<Publisher, PublisherWrapper>
    {
        private PublisherWrapper selectedItem;

        public PublisherDetailViewModel(IEventAggregator eventAggregator,
                                     ILogger logger,
                                     IDomainService<Publisher> domainService)
            : base(eventAggregator, logger, domainService)
        {
            AddPublisherLogoCommand = new DelegateCommand(OnAddPublisherLogoExecute);
            SaveItemCommand = new DelegateCommand(SaveItemExecute, SaveItemCanExecute)
                .ObservesProperty(() => SelectedItem.Name);
            
            SelectedItem = new PublisherWrapper(domainService.CreateItem());
        }

        public ICommand AddPublisherLogoCommand { get; }

        public override PublisherWrapper SelectedItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value ?? throw new ArgumentNullException(nameof(SelectedItem));
                OnPropertyChanged();
            }
        }

        private void OnAddPublisherLogoExecute()
        {
            SelectedItem.LogoPath = FileExplorerService.BrowsePicture() ?? SelectedItem.LogoPath;
        }

        public async override Task LoadAsync(Guid id)
        {
            try
            {
                var publisher = await domainService.Repository.GetSelectedAsync(id) ?? new Publisher();

                SelectedItem = CreateWrapper(publisher);

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
                    if (e.PropertyName == nameof(SelectedItem.Name))
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
                    this.SwitchEditableStateExecute();
                    SelectedItem.Name = "";
                }

                SetDefaultPublisherLogoIfNoneSet();

                void SetDefaultPublisherLogoIfNoneSet()
                {
                    if (SelectedItem.LogoPath is null)
                        SelectedItem.LogoPath = FileExplorerService.GetImagePath();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                logger.Error("Message: {Message}\n\n Stack trace: {StackTrace}\n\n", ex.Message, ex.StackTrace);
            }
        }
        
        public override PublisherWrapper CreateWrapper(Publisher entity)
        {
            var wrapper = new PublisherWrapper(entity);
            return wrapper;
        }
    }
}
