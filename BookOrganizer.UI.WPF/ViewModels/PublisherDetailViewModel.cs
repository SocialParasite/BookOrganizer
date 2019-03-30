﻿using BookOrganizer.Domain;
using BookOrganizer.UI.WPF.Repositories;
using BookOrganizer.UI.WPF.Services;
using Prism.Commands;
using Prism.Events;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookOrganizer.UI.WPF.ViewModels
{
    public class PublisherDetailViewModel : BaseDetailViewModel<Publisher>, IPublisherDetailViewModel
    {
        private string name;

        public PublisherDetailViewModel(IEventAggregator eventAggregator,
            IMetroDialogService metroDialogService,
            IRepository<Publisher> publisherRepo)
            : base(eventAggregator, metroDialogService)
        {
            Repository = publisherRepo ?? throw new ArgumentNullException(nameof(publisherRepo));

            AddPublisherLogoCommand = new DelegateCommand(OnAddPublisherLogoExecute);

            SelectedItem = new Publisher();
        }

        private void OnAddPublisherLogoExecute()
            => SelectedItem.LogoPath = FileExplorerService.BrowsePicture() ?? SelectedItem.LogoPath;

        public ICommand AddPublisherLogoCommand { get; }

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

            SetDefaultPublisherLogoIfNoneSet();

            void SetDefaultPublisherLogoIfNoneSet()
            {
                if (SelectedItem.LogoPath is null)
                    zzTemp();
            }
        }

        private void zzTemp()
        {
            SelectedItem.LogoPath =
                $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase).Substring(6)}\\placeholder.png";
        }
    }
}
