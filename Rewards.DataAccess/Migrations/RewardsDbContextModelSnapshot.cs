﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Rewards.DataAccess;

#nullable disable

namespace Rewards.DataAccess.Migrations
{
    [DbContext(typeof(RewardsDbContext))]
    partial class RewardsDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Rewards.DataAccess.Models.Campaign", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AdminId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ValidFrom")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ValidTo")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Campaigns");
                });

            modelBuilder.Entity("Rewards.DataAccess.Models.PurchaseRecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CampaignId")
                        .HasColumnType("int");

                    b.Property<int>("CustomerId")
                        .HasColumnType("int");

                    b.Property<string>("CustomerName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CampaignId");

                    b.ToTable("PurchaseRecords");
                });

            modelBuilder.Entity("Rewards.DataAccess.Models.Reward", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AgentId")
                        .HasColumnType("int");

                    b.Property<int>("CampaignId")
                        .HasColumnType("int");

                    b.Property<int>("CustomerId")
                        .HasColumnType("int");

                    b.Property<int>("DiscountPercentage")
                        .HasColumnType("int");

                    b.Property<DateTime>("ValidFrom")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ValidTo")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("createdAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("CampaignId");

                    b.ToTable("Rewards");
                });

            modelBuilder.Entity("Rewards.DataAccess.Models.PurchaseRecord", b =>
                {
                    b.HasOne("Rewards.DataAccess.Models.Campaign", null)
                        .WithMany("PurchaseRecords")
                        .HasForeignKey("CampaignId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Rewards.DataAccess.Models.Reward", b =>
                {
                    b.HasOne("Rewards.DataAccess.Models.Campaign", "Campaign")
                        .WithMany("Rewards")
                        .HasForeignKey("CampaignId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Campaign");
                });

            modelBuilder.Entity("Rewards.DataAccess.Models.Campaign", b =>
                {
                    b.Navigation("PurchaseRecords");

                    b.Navigation("Rewards");
                });
#pragma warning restore 612, 618
        }
    }
}
