using System;
using System.Threading.Tasks;

namespace BookOrganizer.UI.WPFCore.ViewModels
{
    public interface IDetailViewModel
    {
        Task LoadAsync(Guid id);
        Guid Id { get; }
    }
}
