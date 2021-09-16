﻿// <auto-generated />
using System;
using ImageSourcesStorage.DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ImageSourcesStorage.DataAccessLayer.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.9")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ImageSourcesStorage.DataAccessLayer.Models.Boards", b =>
                {
                    b.Property<Guid>("BoardId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("BoardId");

                    b.HasIndex("UserId");

                    b.ToTable("Boards");
                });

            modelBuilder.Entity("ImageSourcesStorage.DataAccessLayer.Models.PinBoard", b =>
                {
                    b.Property<Guid>("PinBoardId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("BoardId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("PinId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("PinBoardId");

                    b.ToTable("PinBoards");
                });

            modelBuilder.Entity("ImageSourcesStorage.DataAccessLayer.Models.Users", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Score")
                        .HasColumnType("int");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ImageSourcesStorage.DataAccessLayer.Pins", b =>
                {
                    b.Property<Guid>("PinId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("BoardId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("PinId");

                    b.HasIndex("BoardId");

                    b.HasIndex("UserId");

                    b.ToTable("Pins");
                });

            modelBuilder.Entity("ImageSourcesStorage.DataAccessLayer.Models.Boards", b =>
                {
                    b.HasOne("ImageSourcesStorage.DataAccessLayer.Models.Users", "Owner")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("ImageSourcesStorage.DataAccessLayer.Pins", b =>
                {
                    b.HasOne("ImageSourcesStorage.DataAccessLayer.Models.Boards", null)
                        .WithMany("Pins")
                        .HasForeignKey("BoardId");

                    b.HasOne("ImageSourcesStorage.DataAccessLayer.Models.Users", "Owner")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("ImageSourcesStorage.DataAccessLayer.Models.Boards", b =>
                {
                    b.Navigation("Pins");
                });
#pragma warning restore 612, 618
        }
    }
}
