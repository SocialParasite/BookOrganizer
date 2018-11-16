using Microsoft.EntityFrameworkCore;
using System.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using BookOrganizer.Domain;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookOrganizer.Data.SqlServer
{
    public class BookOrganizerDbContext : DbContext
    {
        private string connectionString;
        public static IConfigurationRoot Configuration { get; private set; }

        public BookOrganizerDbContext() { }

        public BookOrganizerDbContext(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Genre> Genres { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new BookAuthorConfig());
            modelBuilder.ApplyConfiguration(new BookGenreConfig());
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (connectionString is null)
            {
                IConfigurationBuilder builder = new ConfigurationBuilder().AddJsonFile("connectionString.json");

                Configuration = builder.Build();

                connectionString = Configuration.GetConnectionString("BookOrganizerDbDEV");
                optionsBuilder.UseSqlServer(connectionString);
            }
            else
                optionsBuilder.UseSqlServer(connectionString);

            base.OnConfiguring(optionsBuilder);
        }
    }
}
