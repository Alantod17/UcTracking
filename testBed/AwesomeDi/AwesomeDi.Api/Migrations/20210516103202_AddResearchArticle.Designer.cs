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
    [Migration("20210516103202_AddResearchArticle")]
    partial class AddResearchArticle
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

                    b.Property<string>("Extension")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FilePath")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Height")
                        .HasColumnType("int");

                    b.Property<DateTime>("LastWriteUtcDateTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ModifiedUtcDateTime")
                        .HasColumnType("datetime2");

                    b.Property<int?>("Size")
                        .HasColumnType("int");

                    b.Property<int?>("Width")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("LastWriteUtcDateTime");

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

            modelBuilder.Entity("AwesomeDi.Api.Models.ResearchArticle", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Abstract")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ArticleNo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Author")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Booktitle")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("CreatedUtcDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Doi")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EntryKey")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EntryType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Isbn")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Issn")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IssueDate")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Journal")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Keywords")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Location")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ModifiedUtcDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Month")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Number")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Numpages")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Pages")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Publisher")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Series")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Url")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Volume")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Year")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ResearchArticle");
                });

            modelBuilder.Entity("AwesomeDi.Api.Models.SharesiesConfiguration", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<DateTime?>("CreatedUtcDateTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ModifiedUtcDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("SharesiesConfiguration");
                });

            modelBuilder.Entity("AwesomeDi.Api.Models.SharesiesInstrument", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<DateTime?>("CreatedUtcDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("ExchangeCountry")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("InstrumentType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ModifiedUtcDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RiskRating")
                        .HasColumnType("int");

                    b.Property<string>("SharesiesId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Symbol")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("SharesiesInstrument");
                });

            modelBuilder.Entity("AwesomeDi.Api.Models.SharesiesInstrumentCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<DateTime?>("CreatedUtcDateTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ModifiedUtcDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("SharesiesInstrumentCategory");
                });

            modelBuilder.Entity("AwesomeDi.Api.Models.SharesiesInstrumentComparisonPrice", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<DateTime?>("CreatedUtcDateTime")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("Max")
                        .HasPrecision(18, 6)
                        .HasColumnType("decimal(18,6)");

                    b.Property<decimal>("Min")
                        .HasPrecision(18, 6)
                        .HasColumnType("decimal(18,6)");

                    b.Property<DateTime?>("ModifiedUtcDateTime")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("Percent")
                        .HasPrecision(18, 6)
                        .HasColumnType("decimal(18,6)");

                    b.Property<int>("SharesiesInstrumentId")
                        .HasColumnType("int");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<decimal>("Value")
                        .HasPrecision(18, 6)
                        .HasColumnType("decimal(18,6)");

                    b.HasKey("Id");

                    b.HasIndex("SharesiesInstrumentId");

                    b.ToTable("SharesiesInstrumentComparisonPrice");
                });

            modelBuilder.Entity("AwesomeDi.Api.Models.SharesiesInstrumentPriceHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<DateTime?>("CreatedUtcDateTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ModifiedUtcDateTime")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("Price")
                        .HasPrecision(18, 6)
                        .HasColumnType("decimal(18,6)");

                    b.Property<int>("SharesiesInstrumentId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SharesiesInstrumentId");

                    b.ToTable("SharesiesInstrumentPriceHistory");
                });

            modelBuilder.Entity("AwesomeDi.Api.Models.SharesiesInstrumentXCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<DateTime?>("CreatedUtcDateTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ModifiedUtcDateTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("SharesiesInstrumentCategoryId")
                        .HasColumnType("int");

                    b.Property<int>("SharesiesInstrumentId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SharesiesInstrumentCategoryId");

                    b.HasIndex("SharesiesInstrumentId");

                    b.ToTable("SharesiesInstrumentXCategory");
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

            modelBuilder.Entity("AwesomeDi.Api.Models.SharesiesInstrumentComparisonPrice", b =>
                {
                    b.HasOne("AwesomeDi.Api.Models.SharesiesInstrument", "SharesiesInstrument")
                        .WithMany("SharesiesInstrumentComparisonPriceList")
                        .HasForeignKey("SharesiesInstrumentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SharesiesInstrument");
                });

            modelBuilder.Entity("AwesomeDi.Api.Models.SharesiesInstrumentPriceHistory", b =>
                {
                    b.HasOne("AwesomeDi.Api.Models.SharesiesInstrument", "SharesiesInstrument")
                        .WithMany("SharesiesInstrumentPriceHistoryList")
                        .HasForeignKey("SharesiesInstrumentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SharesiesInstrument");
                });

            modelBuilder.Entity("AwesomeDi.Api.Models.SharesiesInstrumentXCategory", b =>
                {
                    b.HasOne("AwesomeDi.Api.Models.SharesiesInstrumentCategory", "SharesiesInstrumentCategory")
                        .WithMany("SharesiesInstrumentXCategoryList")
                        .HasForeignKey("SharesiesInstrumentCategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AwesomeDi.Api.Models.SharesiesInstrument", "SharesiesInstrument")
                        .WithMany("SharesiesInstrumentXCategoryList")
                        .HasForeignKey("SharesiesInstrumentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SharesiesInstrument");

                    b.Navigation("SharesiesInstrumentCategory");
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

            modelBuilder.Entity("AwesomeDi.Api.Models.SharesiesInstrument", b =>
                {
                    b.Navigation("SharesiesInstrumentComparisonPriceList");

                    b.Navigation("SharesiesInstrumentPriceHistoryList");

                    b.Navigation("SharesiesInstrumentXCategoryList");
                });

            modelBuilder.Entity("AwesomeDi.Api.Models.SharesiesInstrumentCategory", b =>
                {
                    b.Navigation("SharesiesInstrumentXCategoryList");
                });

            modelBuilder.Entity("AwesomeDi.Api.Models.User", b =>
                {
                    b.Navigation("UserTokenList");
                });
#pragma warning restore 612, 618
        }
    }
}
