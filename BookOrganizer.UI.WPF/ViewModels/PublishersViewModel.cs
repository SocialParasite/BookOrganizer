﻿using System;
using System.Linq;
using System.Threading.Tasks;
using BookOrganizer.Domain;
using Prism.Events;

namespace BookOrganizer.UI.WPF.ViewModels
{
    public class PublishersViewModel : BaseViewModel<Publisher>
    {
        private readonly IPublisherLookupDataService publisherLookupDataService;

        public PublishersViewModel(IEventAggregator eventAggregator,
                              IPublisherLookupDataService publisherLookupDataService)
            : base(eventAggregator)
        {
            this.publisherLookupDataService = publisherLookupDataService
                ?? throw new ArgumentNullException(nameof(publisherLookupDataService));

            Init();
        }

        public Task Init()
        {
            return InitializeRepositoryAsync();
        }

        public override async Task InitializeRepositoryAsync()
        {
            Items = await publisherLookupDataService.GetPublisherLookupAsync(nameof(PublisherDetailViewModel));

            EntityCollection = Items.OrderBy(p => p.DisplayMember).ToList();
        }
    }
}
