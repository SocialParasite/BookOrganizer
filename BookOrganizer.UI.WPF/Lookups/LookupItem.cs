using System;

namespace BookOrganizer.UI.WPF.Lookups
{
    public class LookupItem
    {
        public Guid Id { get; set; }
        public string DisplayMember { get; set; }
        public string Picture { get; set; }
        public string ViewModelName { get; set; }
    }
}
