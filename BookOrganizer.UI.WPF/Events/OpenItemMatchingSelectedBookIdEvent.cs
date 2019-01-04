using Prism.Events;

namespace BookOrganizer.UI.WPF.Events
{
    public class OpenItemMatchingSelectedBookIdEvent<Guid> : PubSubEvent<Guid>
    {
    }
}
