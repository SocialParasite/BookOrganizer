using BookOrganizer.Data.SqlServer;
using BookOrganizer.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Configuration;
using System.IO;
using System.Linq;

namespace BookOrganizer.TestConsoleUI
{
    class Program
    {
        public static IConfigurationRoot Configuration { get; private set; }

        static void Main(string[] args)
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("connectionString.json");

            Configuration = builder.Build();

            string connection = Configuration.GetConnectionString("BookOrganizerDbDEV");

            using (var context = new BookOrganizerDbContext())
            {
                var books = context.Set<Book>()
                    .Include(b => b.Publisher)
                    .Include(b => b.Language)
                    .Include(b => b.AuthorsLink)
                    .ThenInclude(a => a.Author)
                    .Include(b => b.BookSeries)
                    .ThenInclude(s => s.BooksInSeries)
                    .Include(b => b.BookSeries)
                    .ThenInclude(s => s.SeriesReadOrder)
                    .Include(b => b.ReadDates)
                    .ToList();

                var bookOne = books.First();
                Console.WriteLine(bookOne.ReadDates.Count());
                //bookOne.ReadDates.Add(new BooksReadDate { ReadDate = DateTime.Today });
                var rd = bookOne.ReadDates.First(d => d.ReadDate == DateTime.Today);
                Console.WriteLine(rd.ReadDate);
                bookOne.ReadDates.Remove(rd);
                //context.Update(bookOne);
                Console.WriteLine(bookOne.Title);
                Console.WriteLine(bookOne.ReadDates.Count());
                context.SaveChanges();
            }
        }
    }
}
