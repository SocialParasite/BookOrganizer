using System.Threading.Tasks;

namespace BookOrganizer.UI.WPFCore.ViewModels
{
    public interface IItemLists : ISelectedViewModel
    {
        Task InitializeRepositoryAsync();
    }
}
