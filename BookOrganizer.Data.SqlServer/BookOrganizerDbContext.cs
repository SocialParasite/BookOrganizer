﻿using BookOrganizer.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BookOrganizer.Data.SqlServer
{
    public class BookOrganizerDbContext : DbContext
    {
        public string connectionString;
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
        public DbSet<Format> Formats { get; set; }
        public DbSet<Nationality> Nationalities { get; set; }
        public DbSet<Series> Series { get; set; }
        public DbSet<SeriesReadOrder> SeriesReadOrder { get; set; }
        public DbSet<Settings> Settings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new BookAuthorConfig());
            modelBuilder.ApplyConfiguration(new BookGenreConfig());
            modelBuilder.ApplyConfiguration(new BookFormatConfig());
            modelBuilder.ApplyConfiguration(new BookSeriesConfig());

            modelBuilder.Query<AnnualBookStatisticsReport>();
            modelBuilder.Query<AnnualBookStatisticsInRangeReport>();
            modelBuilder.Query<MonthlyReadsReport>();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (connectionString is null)
            {
                connectionString = ConnectivityService.GetConnectionString();
            }
            optionsBuilder.UseSqlServer(connectionString);

            base.OnConfiguring(optionsBuilder);
        }
    }
}
