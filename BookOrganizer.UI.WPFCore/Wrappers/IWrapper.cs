using System;
using BookOrganizer.Domain;

namespace BookOrganizer.UI.WPFCore.Wrappers
{
    public interface IWrapper<T> where T : class, IIdentifiable
    {
        T Model { get; set; }
        Guid Id { get; set; }
    }
}
