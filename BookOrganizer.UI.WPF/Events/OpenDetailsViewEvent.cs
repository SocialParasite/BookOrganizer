using Prism.Events;
using System;

namespace BookOrganizer.UI.WPF.Events
{
    public class OpenDetailsViewEvent<T> : PubSubEvent<Guid> where T : class
    {
    }

    public class OpenDetailViewEvent : PubSubEvent<OpenDetailViewEventArgs>
    {
    }
    public class OpenDetailViewEventArgs
    {
        public Guid Id { get; set; }
        public string ViewModelName { get; set; }
    }
}
