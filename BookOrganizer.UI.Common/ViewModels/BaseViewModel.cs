using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookOrganizer.DA;
using BookOrganizer.Domain;
using BookOrganizer.UI.Common.Extensions;

namespace BookOrganizer.UI.Common.ViewModels
{
    public abstract class BaseViewModel<T> where T : class, IIdentifiable
    {
        private List<LookupItem> entityCollection;
        private string searchString;

        public IEnumerable<LookupItem> Items;
        //public IRepository<T> Repository;

        public List<LookupItem> EntityCollection
        {
            get => entityCollection;
            set
            {
                entityCollection = value;
                FilteredEntityCollection = entityCollection.FromListToList();
                //OnPropertyChanged();
            }
        }

        private List<LookupItem> filteredEntityCollection;

        public List<LookupItem> FilteredEntityCollection
        {
            get => filteredEntityCollection;
            set
            {
                filteredEntityCollection = value;
                //OnPropertyChanged();
            }
        }

        public string SearchString
        {
            get { return searchString; }
            set
            {
                searchString = value;
                //OnPropertyChanged();
                UpdateFilteredEntityCollection();
            }
        }

        private void UpdateFilteredEntityCollection()
        {
            FilteredEntityCollection.Clear();
            FilteredEntityCollection = EntityCollection.Where(w => w.DisplayMember
                                                       .IndexOf(SearchString, StringComparison.OrdinalIgnoreCase) != -1)
                                                       .ToList();
        }

        public abstract Task InitializeRepositoryAsync();
    }
}
