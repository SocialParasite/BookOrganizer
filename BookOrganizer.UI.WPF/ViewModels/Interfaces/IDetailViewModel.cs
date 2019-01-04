using System;
using System.Threading.Tasks;

namespace BookOrganizer.UI.WPF.ViewModels
{
    public interface IDetailViewModel
    {
        Task LoadAsync(Guid id);
        //bool HasChanges { get; }
        Guid Id { get; }
    }
}
