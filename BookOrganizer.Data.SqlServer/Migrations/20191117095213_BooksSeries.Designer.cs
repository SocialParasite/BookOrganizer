﻿// <auto-generated />
using System;
using BookOrganizer.Data.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BookOrganizer.Data.SqlServer.Migrations
{
    [DbContext(typeof(BookOrganizerDbContext))]
    [Migration("20191117095213_BooksSeries")]
    partial class BooksSeries
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BookOrganizer.Domain.Author", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Biography");

                    b.Property<DateTime?>("DateOfBirth");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("MugShotPath");

                    b.Property<Guid?>("NationalityId");

                    b.HasKey("Id");

                    b.HasIndex("NationalityId");

                    b.ToTable("Authors");
                });

            modelBuilder.Entity("BookOrganizer.Domain.Book", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("BookCoverPicturePath");

                    b.Property<Guid?>("BookSeriesId");

                    b.Property<string>("Description");

                    b.Property<string>("ISBN")
                        .HasMaxLength(13);

                    b.Property<bool>("IsRead");

                    b.Property<Guid>("LanguageId");

                    b.Property<int>("PageCount");

                    b.Property<Guid>("PublisherId");

                    b.Property<int>("ReleaseYear");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(256);

                    b.Property<int>("WordCount");

                    b.HasKey("Id");

                    b.HasIndex("BookSeriesId");

                    b.HasIndex("LanguageId");

                    b.HasIndex("PublisherId");

                    b.ToTable("Books");
                });

            modelBuilder.Entity("BookOrganizer.Domain.BookAuthors", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("AuthorId");

                    b.Property<Guid>("BookId");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("BookId");

                    b.ToTable("BookAuthors");
                });

            modelBuilder.Entity("BookOrganizer.Domain.BookGenres", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("BookId");

                    b.Property<Guid>("GenreId");

                    b.HasKey("Id");

                    b.HasIndex("BookId");

                    b.HasIndex("GenreId");

                    b.ToTable("BookGenres");
                });

            modelBuilder.Entity("BookOrganizer.Domain.BooksFormats", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("BookId");

                    b.Property<Guid>("FormatId");

                    b.HasKey("Id");

                    b.HasIndex("BookId");

                    b.HasIndex("FormatId");

                    b.ToTable("BooksFormats");
                });

            modelBuilder.Entity("BookOrganizer.Domain.BooksReadDate", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("BookId");

                    b.Property<DateTime>("ReadDate");

                    b.HasKey("Id");

                    b.HasIndex("BookId");

                    b.ToTable("BooksReadDate");
                });

            modelBuilder.Entity("BookOrganizer.Domain.BooksSeries", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("BookId");

                    b.Property<Guid>("SeriesId");

                    b.HasKey("Id");

                    b.HasIndex("BookId");

                    b.HasIndex("SeriesId");

                    b.ToTable("BooksSeries");
                });

            modelBuilder.Entity("BookOrganizer.Domain.Format", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(32);

                    b.HasKey("Id");

                    b.ToTable("Formats");
                });

            modelBuilder.Entity("BookOrganizer.Domain.Genre", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(32);

                    b.HasKey("Id");

                    b.ToTable("Genres");
                });

            modelBuilder.Entity("BookOrganizer.Domain.Language", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("LanguageName")
                        .IsRequired()
                        .HasMaxLength(32);

                    b.HasKey("Id");

                    b.ToTable("Languages");
                });

            modelBuilder.Entity("BookOrganizer.Domain.Nationality", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(32);

                    b.HasKey("Id");

                    b.ToTable("Nationalities");
                });

            modelBuilder.Entity("BookOrganizer.Domain.Publisher", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("LogoPath");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.HasKey("Id");

                    b.ToTable("Publishers");
                });

            modelBuilder.Entity("BookOrganizer.Domain.Series", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256);

                    b.Property<int>("NumberOfBooks");

                    b.Property<string>("PicturePath");

                    b.HasKey("Id");

                    b.ToTable("Series");
                });

            modelBuilder.Entity("BookOrganizer.Domain.SeriesReadOrder", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("BookId");

                    b.Property<int>("Instalment");

                    b.Property<Guid>("SeriesId");

                    b.HasKey("Id");

                    b.HasIndex("BookId");

                    b.HasIndex("SeriesId");

                    b.ToTable("SeriesReadOrder");
                });

            modelBuilder.Entity("BookOrganizer.Domain.Settings", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("StoragePath");

                    b.HasKey("Id");

                    b.ToTable("Settings");
                });

            modelBuilder.Entity("BookOrganizer.Domain.Author", b =>
                {
                    b.HasOne("BookOrganizer.Domain.Nationality", "Nationality")
                        .WithMany("Authors")
                        .HasForeignKey("NationalityId");
                });

            modelBuilder.Entity("BookOrganizer.Domain.Book", b =>
                {
                    b.HasOne("BookOrganizer.Domain.Series", "BookSeries")
                        .WithMany("BooksInSeries")
                        .HasForeignKey("BookSeriesId");

                    b.HasOne("BookOrganizer.Domain.Language", "Language")
                        .WithMany()
                        .HasForeignKey("LanguageId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BookOrganizer.Domain.Publisher", "Publisher")
                        .WithMany("Books")
                        .HasForeignKey("PublisherId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BookOrganizer.Domain.BookAuthors", b =>
                {
                    b.HasOne("BookOrganizer.Domain.Author", "Author")
                        .WithMany("BooksLink")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BookOrganizer.Domain.Book", "Book")
                        .WithMany("AuthorsLink")
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BookOrganizer.Domain.BookGenres", b =>
                {
                    b.HasOne("BookOrganizer.Domain.Book", "Book")
                        .WithMany("GenreLink")
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BookOrganizer.Domain.Genre", "Genre")
                        .WithMany("BookLink")
                        .HasForeignKey("GenreId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BookOrganizer.Domain.BooksFormats", b =>
                {
                    b.HasOne("BookOrganizer.Domain.Book", "Book")
                        .WithMany("FormatLink")
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BookOrganizer.Domain.Format", "Format")
                        .WithMany("BookLink")
                        .HasForeignKey("FormatId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BookOrganizer.Domain.BooksReadDate", b =>
                {
                    b.HasOne("BookOrganizer.Domain.Book")
                        .WithMany("ReadDates")
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BookOrganizer.Domain.BooksSeries", b =>
                {
                    b.HasOne("BookOrganizer.Domain.Book", "Book")
                        .WithMany("BooksSeries")
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BookOrganizer.Domain.Series", "Series")
                        .WithMany()
                        .HasForeignKey("SeriesId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BookOrganizer.Domain.SeriesReadOrder", b =>
                {
                    b.HasOne("BookOrganizer.Domain.Book", "Book")
                        .WithMany()
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BookOrganizer.Domain.Series", "Series")
                        .WithMany("SeriesReadOrder")
                        .HasForeignKey("SeriesId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
