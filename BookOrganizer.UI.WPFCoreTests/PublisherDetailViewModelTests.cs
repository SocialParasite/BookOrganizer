﻿using System;
using System.Windows.Media;
using BookOrganizer.Domain;
using BookOrganizer.Domain.Services;
using BookOrganizer.UI.WPFCore;
using BookOrganizer.UI.WPFCore.ViewModels;
using BookOrganizer.UI.WPFCore.Wrappers;
using FluentAssertions;
using Moq;
using Prism.Events;
using Serilog;
using Xunit;

namespace BookOrganizer.UI.WPFCoreTests
{
    public class PublisherDetailViewModelTests
    {
        private PublisherDetailViewModel viewModel;

        public PublisherDetailViewModelTests()
        {
            var eventAggregatorMock = new Mock<IEventAggregator>();
            var loggerMock = new Mock<ILogger>();
            var repositoryMock = new Mock<IRepository<Publisher>>();

            var publisherService = new PublisherService(repositoryMock.Object);

            viewModel = new PublisherDetailViewModel(eventAggregatorMock.Object,
                loggerMock.Object,
                publisherService);
        }

        [Fact]
        public void NewPublishersId_ShouldHaveDefaultValue()
        {
            viewModel.SelectedItem.Should().BeOfType<PublisherWrapper>();
            viewModel.SelectedItem.Id.Should().Equals(default(Guid));
        }

        [Fact]
        public async void OpeningAPublisherDetailViewWithANewPublisher_ShouldByDefaultOpenAsEditable()
        {
            await viewModel.LoadAsync(default(Guid));
            viewModel.UserMode.Item1.Should().BeFalse();
            viewModel.UserMode.Item2.Should().Equals(DetailViewState.EditMode);
            viewModel.UserMode.Item3.Should().Equals(Brushes.LightGreen);
            viewModel.UserMode.Item4.Should().BeTrue();
        }

        [Fact]
        public async void NewPublisherLogoPath_ShouldBeSetToPlaceholderImage()
        {
            await viewModel.LoadAsync(default);
            viewModel.SelectedItem.LogoPath.Should().EndWith("placeholder.png");
        }
    }
}
