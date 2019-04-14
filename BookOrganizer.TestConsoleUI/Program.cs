using BookOrganizer.Data.SqlServer;
using BookOrganizer.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;

namespace BookOrganizer.TestConsoleUI
{
    class Program
    {
        public static IConfigurationRoot Configuration { get; private set; }
        public static Dictionary<int, string> ReadOrderOfBooks { get; set; }

        static void Main(string[] args)
        {
            ReadOrderOfBooks = new Dictionary<int, string>();

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

                var bookOne = books.Find(b => b.Title == "The Crimson Campaign"); //books.First();
                Console.WriteLine(bookOne.ReadDates.Count());
                //bookOne.ReadDates.Add(new BooksReadDate { ReadDate = DateTime.Today });
                //var rd = bookOne.ReadDates.First(d => d.ReadDate == DateTime.Today);
                //Console.WriteLine(rd.ReadDate);
                //bookOne.ReadDates.Remove(rd);
                //context.Update(bookOne);
                Console.WriteLine(bookOne.Title);
                Console.WriteLine(bookOne.ReadDates.Count());
                Console.WriteLine($"Series: {bookOne.BookSeries.Name}, book count in series: {bookOne.BookSeries.NumberOfBooks}");

                //foreach (var book in bookOne.BookSeries.BooksInSeries.OrderBy(b => b.BookSeries.SeriesReadOrder))
                //{
                //    //Console.WriteLine(book.Title);
                //    var readOrder = book.BookSeries.SeriesReadOrder.Where(s => s.BookId == book.Id).Select(o => o.Instalment);
                //    //Console.WriteLine($"TEST: {readOrder.SingleOrDefault()}");
                //    ReadOrderOfBooks.Add(readOrder.SingleOrDefault(), book.Title);
                //}

                ReadOrderOfBooks = GetSeriesInOrder(bookOne);
                foreach (var orderedSeries in ReadOrderOfBooks)
                {
                    Console.WriteLine($"{orderedSeries.Key}. {orderedSeries.Value}");
                }
                //context.SaveChanges();


            }

        }
        public static Dictionary<int, string> GetSeriesInOrder(Book bookInSeries)
        {
            Dictionary<int, string> orderedList = new Dictionary<int, string>();

            foreach (var book in bookInSeries.BookSeries.BooksInSeries.OrderBy(b => b.BookSeries.SeriesReadOrder))
            {
                //Console.WriteLine(book.Title);
                var readOrder = book.BookSeries.SeriesReadOrder.Where(s => s.BookId == book.Id).Select(o => o.Instalment);
                //Console.WriteLine($"TEST: {readOrder.SingleOrDefault()}");
                //ReadOrderOfBooks.Add(readOrder.SingleOrDefault(), book.Title);
                orderedList.Add(readOrder.SingleOrDefault(), book.Title);
            }
            return orderedList;
            //foreach (var orderedSeries in ReadOrderOfBooks)
            //{
            //    Console.WriteLine($"{orderedSeries.Key}. {orderedSeries.Value}");
            //}
        }
    }
}
