using Prism.Events;
using System;

namespace BookOrganizer.UI.WPFCore.Events
{
    public class CloseDetailsViewEvent : PubSubEvent<CloseDetailsViewEventArgs> { }

    public class CloseDetailsViewEventArgs
    {
        public Guid Id { get; set; }
        public string ViewModelName { get; set; }
    }
}
