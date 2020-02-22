using System;
using System.Threading.Tasks;

namespace BookOrganizer.UI.WPFCore.ViewModels
{
    public interface IDetailViewModel
    {
        Guid Id { get; set; }
        Task LoadAsync(Guid id);
    }
}
