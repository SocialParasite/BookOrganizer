using Prism.Events;

namespace BookOrganizer.UI.WPF.Events
{
    public class OpenItemMatchingSelectedAuthorIdEvent<Guid> : PubSubEvent<Guid>
    {
    }
}
