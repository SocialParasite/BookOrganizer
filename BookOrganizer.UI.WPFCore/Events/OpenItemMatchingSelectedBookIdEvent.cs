using Prism.Events;

namespace BookOrganizer.UI.WPFCore.Events
{
    public class OpenItemMatchingSelectedBookIdEvent<Guid> : PubSubEvent<Guid>
    {
    }
}
