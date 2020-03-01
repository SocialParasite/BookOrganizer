using BookOrganizer.Domain;

namespace BookOrganizer.UI.WPFCore.Wrappers
{
    public class SeriesWrapper : BaseWrapper<Series>
    {
        public SeriesWrapper(Series model) : base(model) { }

        public string Name
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public int NumberOfBooks
        {
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }

        public string PicturePath
        {
            get { return GetValue<string>(); }
            set
            {
                SetValue(DomainHelpers.SetPicturePath(value, "SeriesPictures"));
            }
        }

        public string Description
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }
    }
}
