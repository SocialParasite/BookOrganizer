using BookOrganizer.Domain;

namespace BookOrganizer.UI.WPFCore.Wrappers
{
    public class NationalityWrapper : BaseWrapper<Nationality>
    {
        public NationalityWrapper(Nationality model) : base(model) { }

        public string Name
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }
    }
}
