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
    [Migration("20181115170336_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BookOrganizer.Domain.Book", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("BookCoverPicture");

                    b.Property<string>("Description");

                    b.Property<string>("ISBN")
                        .HasMaxLength(13);

                    b.Property<bool>("IsRead");

                    b.Property<int>("PageCount");

                    b.Property<int>("ReleaseYear");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.ToTable("Books");
                });
#pragma warning restore 612, 618
        }
    }
}
