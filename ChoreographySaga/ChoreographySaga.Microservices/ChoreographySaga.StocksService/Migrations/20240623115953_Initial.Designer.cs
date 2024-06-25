﻿// <auto-generated />
using System;
using ChoreographySaga.StocksService.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ChoreographySaga.StocksService.Migrations
{
    [DbContext(typeof(StocksDbContext))]
    [Migration("20240623115953_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ChoreographySaga.StocksService.Persistence.Entities.Product", b =>
                {
                    b.Property<Guid>("ProductUuid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<decimal>("AvailableQuantity")
                        .HasColumnType("numeric");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("UnitPrice")
                        .HasColumnType("numeric");

                    b.HasKey("ProductUuid");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("ChoreographySaga.StocksService.Persistence.Entities.ReservedProduct", b =>
                {
                    b.Property<Guid>("ReservationUuid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("OrderUuid")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ProductUuid")
                        .HasColumnType("uuid");

                    b.Property<decimal>("Quantity")
                        .HasColumnType("numeric");

                    b.HasKey("ReservationUuid");

                    b.ToTable("ReservedProducts");
                });
#pragma warning restore 612, 618
        }
    }
}
