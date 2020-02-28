using BookOrganizer.Domain;

namespace BookOrganizer.UI.WPFCore.Wrappers
{
    public class PublisherWrapper : BaseWrapper<Publisher>
    {
        public PublisherWrapper(Publisher model) : base(model) { }

        public string Name
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public string LogoPath
        {
            get { return GetValue<string>(); }
            set
            {
                SetValue(DomainHelpers.SetPicturePath(value, "PublisherLogos"));
            }
        }

        public string Description
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }
    }
}
