using BookOrganizer.Data.SqlServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookOrganizer.UI.WPF.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly BookOrganizerDbContext context;

        public MainViewModel(BookOrganizerDbContext context)
        {
            this.context = context;
        }
    }
}
