using BookOrganizer.Domain;
using BookOrganizer.UI.WPF.Lookups;
using BookOrganizer.UI.WPF.Repositories;
using BookOrganizer.UI.WPF.Services;
using BookOrganizer.UI.WPF.ViewModels;
using BookOrganizer.UI.WPFTests.Extensions;
using FluentAssertions;
using Moq;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BookOrganizer.UI.WPFTests
{
    public class LanguageDetailViewModelTests
    {
        private Mock<IEventAggregator> eventAggregatorMock;
        private Mock<IMetroDialogService> metroDialogServiceMock;
        private Mock<IRepository<Language>> languagesRepoMock;
        private Mock<ILanguageLookupDataService> languageLookupServiceMock;
        private LanguageDetailViewModel viewModel;

        public LanguageDetailViewModelTests()
        {
            eventAggregatorMock = new Mock<IEventAggregator>();
            metroDialogServiceMock = new Mock<IMetroDialogService>();
            languagesRepoMock = new Mock<IRepository<Language>>();
            languageLookupServiceMock = new Mock<ILanguageLookupDataService>();

            viewModel = new LanguageDetailViewModel(eventAggregatorMock.Object,
                metroDialogServiceMock.Object,
                languagesRepoMock.Object,
                languageLookupServiceMock.Object);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void WhenLanguageNameIsSetNullOrEmpty_ThrowsArgumentOutOfRangeException(string name)
        {
            Action action = () => viewModel.LanguageName = name;

            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void TryingToSetLanguageNameLongerThan32Characters_ThrowsArgumentOutOfRangeException()
        {
            Action action = ()
                => viewModel.LanguageName = "Spicy jalapeno bacon ipsum dolor amet.";

            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void LanguageName_ShouldRaise_PropertyChangedEvent()
        {
            var raised = viewModel.IsPropertyChangedRaised(() =>
            {
                viewModel.LanguageName = "Latin";
            }, nameof(viewModel.LanguageName));

            raised.Should().BeTrue();
        }


        [Fact]
        public void NewLanguagesId_ShouldHaveDefaultValue()
        {
            viewModel.SelectedItem.Should().BeOfType<Language>();
            viewModel.SelectedItem.Id.Should().Equals(default(Guid));
        }

        [Fact]
        public void NewLanguagesLanguagesCollection_ShouldBeEmpty()
        {
            viewModel.Languages.Should().BeEmpty();
        }
    }
}
