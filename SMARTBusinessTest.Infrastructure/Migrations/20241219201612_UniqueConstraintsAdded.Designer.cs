﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SMARTBusinessTest.Infrastructure;

#nullable disable

namespace SMARTBusinessTest.Infrastructure.Migrations
{
    [DbContext(typeof(EquipmentContractsDbContext))]
    [Migration("20241219201612_UniqueConstraintsAdded")]
    partial class UniqueConstraintsAdded
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.36")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("SMARTBusinessTest.Domain.Entities.EquipmentUnit", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("Unit_Id");

                    b.Property<int>("Amount")
                        .HasColumnType("int")
                        .HasColumnName("Unit_Amount");

                    b.Property<Guid>("ContractId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("Unit_ContractId");

                    b.Property<Guid>("EquipmentId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("Unit_EquipmentId");

                    b.Property<int>("TotalArea")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ContractId");

                    b.HasIndex("EquipmentId");

                    b.ToTable("Unit");
                });

            modelBuilder.Entity("SMARTBusinessTest.Domain.Entities.PlacementContract", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("Contract_Id");

                    b.Property<Guid>("FacilityId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("Contract_FacilityId");

                    b.Property<int>("TotalEquipmentArea")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("FacilityId");

                    b.ToTable("Contract");
                });

            modelBuilder.Entity("SMARTBusinessTest.Domain.Entities.ProcessEquipment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("Equipment_Id");

                    b.Property<int>("Area")
                        .HasColumnType("int")
                        .HasColumnName("Equipment_Area");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("Equipment_Code");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("Equipment_Name");

                    b.HasKey("Id");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Equipment");
                });

            modelBuilder.Entity("SMARTBusinessTest.Domain.Entities.ProductionFacility", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("Facility_Id");

                    b.Property<int>("Area")
                        .HasColumnType("int")
                        .HasColumnName("Facility_Area");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("Facility_Code");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("Facility_Name");

                    b.HasKey("Id");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Facility");
                });

            modelBuilder.Entity("SMARTBusinessTest.Domain.Entities.EquipmentUnit", b =>
                {
                    b.HasOne("SMARTBusinessTest.Domain.Entities.PlacementContract", "Contract")
                        .WithMany("EquipmentUnits")
                        .HasForeignKey("ContractId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SMARTBusinessTest.Domain.Entities.ProcessEquipment", "Equipment")
                        .WithMany("Units")
                        .HasForeignKey("EquipmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Contract");

                    b.Navigation("Equipment");
                });

            modelBuilder.Entity("SMARTBusinessTest.Domain.Entities.PlacementContract", b =>
                {
                    b.HasOne("SMARTBusinessTest.Domain.Entities.ProductionFacility", "ProductionFacility")
                        .WithMany("Contracts")
                        .HasForeignKey("FacilityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ProductionFacility");
                });

            modelBuilder.Entity("SMARTBusinessTest.Domain.Entities.PlacementContract", b =>
                {
                    b.Navigation("EquipmentUnits");
                });

            modelBuilder.Entity("SMARTBusinessTest.Domain.Entities.ProcessEquipment", b =>
                {
                    b.Navigation("Units");
                });

            modelBuilder.Entity("SMARTBusinessTest.Domain.Entities.ProductionFacility", b =>
                {
                    b.Navigation("Contracts");
                });
#pragma warning restore 612, 618
        }
    }
}
