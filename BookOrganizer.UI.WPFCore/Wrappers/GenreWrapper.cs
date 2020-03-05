using BookOrganizer.Domain;

namespace BookOrganizer.UI.WPFCore.Wrappers
{
    public class GenreWrapper : BaseWrapper<Genre>
    {
        public GenreWrapper(Genre model) : base(model) { }

        public string Name
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }
    }
}
