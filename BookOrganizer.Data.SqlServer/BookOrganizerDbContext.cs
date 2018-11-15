using Microsoft.EntityFrameworkCore;
using System.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using BookOrganizer.Domain;
using Microsoft.Extensions.Configuration;
using System.IO;

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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (connectionString is null)
            {
                IConfigurationBuilder builder = new ConfigurationBuilder().AddJsonFile("connectionString.json");
                //.SetBasePath(Directory.GetCurrentDirectory())
                //.AddJsonFile("connectionString.json");

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
