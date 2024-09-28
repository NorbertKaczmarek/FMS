﻿// <auto-generated />
using System;
using FMS.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FMS.API.Migrations
{
    [DbContext(typeof(FMSDbContext))]
    partial class FMSDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("FMS.API.Entities.Flight", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTimeOffset>("DataWylotu")
                        .HasColumnType("datetime");

                    b.Property<string>("MiejscePrzylotu")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("MiejsceWylotu")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("NumerLotu")
                        .HasColumnType("int");

                    b.Property<string>("TypSamolotu")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Flights");
                });

            modelBuilder.Entity("FMS.API.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .HasColumnType("longtext");

                    b.Property<string>("FullName")
                        .HasColumnType("longtext");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
