using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace BookOrganizer.Domain
{
    public class BaseDomainEntity
    {
        //private string displayMember;

        //[NotMapped]
        //public string DisplayMember
        //{
        //    get { return displayMember; }
        //    set { displayMember = value; OnPropertyChanged(); }
        //}

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName]string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
