using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BookOrganizer.Domain
{
    public class BaseDomainEntity
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName]string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
