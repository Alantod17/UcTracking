﻿// <auto-generated />
using System;
using AwesomeDi.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AwesomeDi.Api.Migrations
{
    [DbContext(typeof(_DbContext.AwesomeDiContext))]
    [Migration("20210126092743_DropFileCreationDateTime")]
    partial class DropFileCreationDateTime
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.2");

            modelBuilder.Entity("AwesomeDi.Api.Models.Configuration", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<DateTime?>("CreatedUtcDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("DefaultThumbnailFilePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ModifiedUtcDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("ThumbnailFolderPath")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Configuration");
                });

            modelBuilder.Entity("AwesomeDi.Api.Models.FileEntry", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<DateTime?>("CreatedUtcDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("FilePath")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("LastWriteUtcDateTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ModifiedUtcDateTime")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("FileEntry");
                });

            modelBuilder.Entity("AwesomeDi.Api.Models.FileEntryEncryptionLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<DateTime?>("CreatedUtcDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("EncryptedFilePath")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("FileEntryId")
                        .HasColumnType("int");

                    b.Property<DateTime>("FileLastWriteUtcDateTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ModifiedUtcDateTime")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("FileEntryId");

                    b.ToTable("FileEntryEncryptionLog");
                });

            modelBuilder.Entity("AwesomeDi.Api.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("CreatedUtcDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime?>("ModifiedUtcDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("User");
                });

            modelBuilder.Entity("AwesomeDi.Api.Models.UserToken", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("AccessToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("AccessTokenExpireUtcDateTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("CreatedUtcDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("DeviceId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ModifiedUtcDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("RefreshToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("RefreshTokenExpireUtcDateTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserToken");
                });

            modelBuilder.Entity("AwesomeDi.Api.Models.FileEntryEncryptionLog", b =>
                {
                    b.HasOne("AwesomeDi.Api.Models.FileEntry", "FileEntry")
                        .WithMany("FileEntryEncryptionLogList")
                        .HasForeignKey("FileEntryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FileEntry");
                });

            modelBuilder.Entity("AwesomeDi.Api.Models.UserToken", b =>
                {
                    b.HasOne("AwesomeDi.Api.Models.User", "User")
                        .WithMany("UserTokenList")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("AwesomeDi.Api.Models.FileEntry", b =>
                {
                    b.Navigation("FileEntryEncryptionLogList");
                });

            modelBuilder.Entity("AwesomeDi.Api.Models.User", b =>
                {
                    b.Navigation("UserTokenList");
                });
#pragma warning restore 612, 618
        }
    }
}
