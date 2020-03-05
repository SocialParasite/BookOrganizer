using BookOrganizer.Domain;

namespace BookOrganizer.UI.WPFCore.Wrappers
{
    public class LanguageWrapper : BaseWrapper<Language>
    {
        public LanguageWrapper(Language model) : base(model) { }

        public string LanguageName
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }
    }
}
