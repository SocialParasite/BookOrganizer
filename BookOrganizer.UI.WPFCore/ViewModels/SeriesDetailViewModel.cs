using System;
using System.Threading.Tasks;

namespace BookOrganizer.UI.WPFCore.ViewModels
{
    public class SeriesDetailViewModel : IDetailViewModel
    {
        public Guid Id => throw new NotImplementedException();

        public Task LoadAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}