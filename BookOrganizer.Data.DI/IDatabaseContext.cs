using BookOrganizer.Domain;
using Microsoft.EntityFrameworkCore;
using System;

namespace BookOrganizer.Data.DI
{
    public interface IDatabaseContext : IDisposable
    {
        //DbSet<TEntity> Set<TEntity>() where TEntity : class;
        DbSet<Book> Books { get; set; }
        DbSet<Author> Authors { get; set; }
        DbSet<Language> Languages { get; set; }
        DbSet<Publisher> Publishers { get; set; }
        DbSet<Genre> Genres { get; set; }
        DbSet<Format> Formats { get; set; }
        DbSet<Nationality> Nationalities { get; set; }
        DbSet<Series> Series { get; set; }
        DbSet<SeriesReadOrder> SeriesReadOrder { get; set; }
        DbSet<Settings> Settings { get; set; }
    }
}
