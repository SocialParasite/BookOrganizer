using BookOrganizer.Domain;
using BookOrganizer.UI.WPF.Enums;
using BookOrganizer.UI.WPF.Repositories;
using BookOrganizer.UI.WPF.Services;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace BookOrganizer.UI.WPF.ViewModels
{
    public class LanguageDetailViewModel : BaseDetailViewModel<Language>, ILanguageDetailViewModel
    {
        private string languageName;

        public LanguageDetailViewModel(IEventAggregator eventAggregator,
            IMetroDialogService metroDialogService,
            IRepository<Language> languageRepository)
            : base(eventAggregator, metroDialogService)
        {
            Repository = languageRepository;

            SelectedItem = new Language();

            UserMode = (!UserMode.Item1, DetailViewState.EditMode, Brushes.LightGray, !UserMode.Item4).ToTuple();
        }

        [Required]
        [MinLength(1, ErrorMessage = "Language name should be at minimum 1 character long.")]
        [MaxLength(32, ErrorMessage = "Language name should be maximum of 32 characters long.")]
        public string LanguageName
        {
            get => languageName;
            set
            {
                ValidatePropertyInternal(nameof(LanguageName), value);
                languageName = value;
                OnPropertyChanged();
                TabTitle = value;
                ((DelegateCommand)SaveItemCommand).RaiseCanExecuteChanged();
                SelectedItem.LanguageName = value;
            }
        }
        public async override Task LoadAsync(Guid id)
        {
            SelectedItem = await Repository.GetSelectedAsync(id) ?? null;

            Id = id;

            if (Id != Guid.Parse("00000000-0000-0000-0000-000000000000"))
            {
                LanguageName = SelectedItem.LanguageName;

                TabTitle = LanguageName;
            }
        }
    }
}
