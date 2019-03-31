using Prism.Events;

namespace BookOrganizer.UI.WPF.Events
{
    public class OpenItemMatchingSelectedSeriesIdEvent<Guid> : PubSubEvent<Guid>
    {
    }
}
