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
    [Migration("20181117114527_BooksFormats")]
    partial class BooksFormats
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BookOrganizer.Domain.Author", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("Authors");
                });

            modelBuilder.Entity("BookOrganizer.Domain.Book", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("BookCoverPicture");

                    b.Property<string>("Description");

                    b.Property<string>("ISBN")
                        .HasMaxLength(13);

                    b.Property<bool>("IsRead");

                    b.Property<Guid?>("LanguageId");

                    b.Property<int>("PageCount");

                    b.Property<Guid?>("PublisherId");

                    b.Property<int>("ReleaseYear");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(256);

                    b.HasKey("Id");

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

                    b.Property<Guid?>("BookId");

                    b.Property<DateTime>("ReadDate");

                    b.HasKey("Id");

                    b.HasIndex("BookId");

                    b.ToTable("BooksReadDate");
                });

            modelBuilder.Entity("BookOrganizer.Domain.Format", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Abbreveation")
                        .IsRequired()
                        .HasMaxLength(16);

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

            modelBuilder.Entity("BookOrganizer.Domain.Publisher", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.HasKey("Id");

                    b.ToTable("Publishers");
                });

            modelBuilder.Entity("BookOrganizer.Domain.Book", b =>
                {
                    b.HasOne("BookOrganizer.Domain.Language", "Language")
                        .WithMany()
                        .HasForeignKey("LanguageId");

                    b.HasOne("BookOrganizer.Domain.Publisher", "Publisher")
                        .WithMany("Books")
                        .HasForeignKey("PublisherId");
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
                    b.HasOne("BookOrganizer.Domain.Book", "Book")
                        .WithMany("ReadDates")
                        .HasForeignKey("BookId");
                });
#pragma warning restore 612, 618
        }
    }
}
