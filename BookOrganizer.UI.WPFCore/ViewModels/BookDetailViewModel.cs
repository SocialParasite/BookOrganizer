using BookOrganizer.DA;
using BookOrganizer.Domain;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookOrganizer.UI.WPFCore.ViewModels
{
    public class BookDetailViewModel : IDetailViewModel
    {
        public Guid Id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Task LoadAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
