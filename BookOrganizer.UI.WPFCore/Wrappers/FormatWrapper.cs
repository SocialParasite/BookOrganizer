using BookOrganizer.Domain;

namespace BookOrganizer.UI.WPFCore.Wrappers
{
    public class FormatWrapper : BaseWrapper<Format>
    {
        public FormatWrapper(Format model) : base(model) { }

        public string Name
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }
    }
}
