﻿using BookOrganizer.Domain;
using BookOrganizer.DA;
using BookOrganizer.UI.WPF.Services;
using Prism.Commands;
using Prism.Events;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookOrganizer.UI.WPF.ViewModels
{
    public class PublisherDetailViewModel : BaseDetailViewModel<Publisher>
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

        public ICommand AddPublisherLogoCommand { get; }

        [Required]
        [MinLength(1, ErrorMessage = "Publishers name should be at minimum 1 character long.")]
        [MaxLength(64, ErrorMessage = "Publishers name should be maximum of 64 characters long.")]
        public string Name
        {
            get { return name; }
            set
            {
                ValidatePropertyInternal(nameof(Name), value);
                name = value;
                OnPropertyChanged();
                TabTitle = value;
                ((DelegateCommand)SaveItemCommand).RaiseCanExecuteChanged();
                SelectedItem.Name = value;
            }
        }

        public async override Task LoadAsync(Guid id)
        {
            SelectedItem = await Repository.GetSelectedAsync(id) ?? new Publisher();

            Id = id;

            if (Id != default)
            {
                TabTitle = SelectedItem.Name;
                Name = SelectedItem.Name;
            }
            else
                this.SwitchEditableStateExecute();

            SetDefaultPublisherLogoIfNoneSet();

            SelectedItem.PropertyChanged += (s, e) =>
            {
                SetChangeTracker();
            };

            void SetDefaultPublisherLogoIfNoneSet()
            {
                if (SelectedItem.LogoPath is null)
                    SelectedItem.LogoPath = FileExplorerService.GetImagePath();
            }
        }

        private async void OnAddPublisherLogoExecute()
        {
            SelectedItem.LogoPath = FileExplorerService.BrowsePicture() ?? SelectedItem.LogoPath;
            await LoadAsync(this.Id);
        }
    }
}
