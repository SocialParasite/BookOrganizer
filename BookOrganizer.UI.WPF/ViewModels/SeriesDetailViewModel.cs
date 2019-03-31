using BookOrganizer.Domain;
using BookOrganizer.UI.WPF.Repositories;
using BookOrganizer.UI.WPF.Services;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookOrganizer.UI.WPF.ViewModels
{
    public class SeriesDetailViewModel : BaseDetailViewModel<Series>, ISeriesDetailViewModel
    {
        private string name;

        public SeriesDetailViewModel(IEventAggregator eventAggregator, IMetroDialogService metroDialogService,
            IRepository<Series> seriesRepo)
            : base(eventAggregator, metroDialogService)
        {
            Repository = seriesRepo ?? throw new ArgumentNullException(nameof(seriesRepo));

            //AddSeriesPictureCommand
        }

        public string Name
        {
            get { return name; }
            set { name = value; OnPropertyChanged(); TabTitle = value; SelectedItem.Name = value; }
        }

        public async override Task LoadAsync(Guid id)
        {
            SelectedItem = await Repository.GetSelectedAsync(id) ?? null;

            Id = id;

            if (Id != Guid.Parse("00000000-0000-0000-0000-000000000000"))
            {
                TabTitle = SelectedItem.Name;
                Name = SelectedItem.Name;
            }
            else
                this.SwitchEditableStateExecute();

            //SetDefaultSeriesPictureIfNoneSet();

            //void SetDefaultSeriesPictureIfNoneSet()
            //{
            //    if (SelectedItem.PicturePath is null)
            //        SelectedItem.PicturePath = FileExplorerService.GetImagePath();
            //}
        }
    }
}
