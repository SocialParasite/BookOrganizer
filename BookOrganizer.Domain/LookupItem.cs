﻿using System;
using BookOrganizer.Domain.Enums;

namespace BookOrganizer.Domain
{
    public class LookupItem
    {
        public Guid Id { get; set; }
        public string DisplayMember { get; set; }
        public string Picture { get; set; }
        public string ViewModelName { get; set; }
        public BookStatus ItemStatus { get; set; }
    }
}
