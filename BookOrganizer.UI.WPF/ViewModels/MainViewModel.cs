using BookOrganizer.Data.SqlServer;
using Prism.Commands;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace BookOrganizer.UI.WPF.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly BookOrganizerDbContext context;
        private IDetailViewModel _selectedDetailViewModel;

        public MainViewModel(BookOrganizerDbContext context)
        {
            this.context = context;

            DetailViewModels = new ObservableCollection<IDetailViewModel>();

            CreateNewBookDetailViewCommand = new DelegateCommand(OnCreateNewBookDetailViewExecute);

        }

        private void OnCreateNewBookDetailViewExecute()
        {
            DetailViewModels.Add(new BookDetailViewModel());
            SelectedDetailViewModel = DetailViewModels.First();
        }

        public ICommand CreateNewBookDetailViewCommand { get; set; }

        public ObservableCollection<IDetailViewModel> DetailViewModels { get; set; }

        public IDetailViewModel SelectedDetailViewModel
        {
            get { return _selectedDetailViewModel; }
            set
            {
                _selectedDetailViewModel = value;
                OnPropertyChanged();
            }
        }


    }
}
