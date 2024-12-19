using Microsoft.EntityFrameworkCore;
using SMARTBusinessTest.Domain.Entities;
using System.Xml;

namespace SMARTBusinessTest.Infrastructure
{
    public class EquipmentContractsDbContext : DbContext
    {
        public EquipmentContractsDbContext(DbContextOptions<EquipmentContractsDbContext> options) 
            : base(options) 
        { 
        }

        public DbSet<ProductionFacility> ProductionFacilities { get; set; }
        public DbSet<ProcessEquipment> ProcessEquipment { get; set; }
        public DbSet<PlacementContract> PlacementContract { get; set; }
        public DbSet<EquipmentUnit> EquipmentUnits { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductionFacility>()
                .HasKey(pf => pf.Id);
            modelBuilder.Entity<ProductionFacility>(pf =>
            {
                pf.Property(x => x.Area).IsRequired();
                pf.Property(x => x.Name).IsRequired();
                pf.Property(x => x.Code).IsRequired().HasMaxLength(50);
            });

            modelBuilder.Entity<ProcessEquipment>()
                .HasKey(pe => pe.Id);
            modelBuilder.Entity<ProcessEquipment>(pe =>
            {
                pe.Property(x => x.Area).IsRequired();
                pe.Property(x => x.Name).IsRequired();
                pe.Property(x => x.Code).IsRequired().HasMaxLength(50);
            });

            modelBuilder.Entity<EquipmentUnit>(pe =>
            {
                pe.Property(x => x.Amount).IsRequired();
                pe.Property(x => x.TotalArea).IsRequired();
            });

            modelBuilder.Entity<ProductionFacility>()
              .HasIndex(e => e.Code)
              .IsUnique();
            modelBuilder.Entity<ProductionFacility>()
              .HasIndex(e => e.Name)
              .IsUnique();

            modelBuilder.Entity<ProcessEquipment>()
              .HasIndex(e => e.Code)
              .IsUnique();
            modelBuilder.Entity<ProcessEquipment>()
              .HasIndex(e => e.Name)
              .IsUnique();

            modelBuilder.Entity<PlacementContract>()
                .HasKey(pc => pc.Id);
            modelBuilder.Entity<PlacementContract>()
                .HasOne(pc => pc.ProductionFacility)
                .WithMany(pf => pf.Contracts)
                .HasForeignKey(pc => pc.FacilityId);

            modelBuilder.Entity<EquipmentUnit>()
                .HasKey(eu => eu.Id);
            modelBuilder.Entity<EquipmentUnit>()
                .HasOne(eu => eu.Contract)
                .WithMany(pc => pc.EquipmentUnits)
                .HasForeignKey(eu => eu.ContractId);
            modelBuilder.Entity<EquipmentUnit>()
                .HasOne(eu => eu.Equipment)
                .WithMany(pe => pe.Units)
                .HasForeignKey(eu => eu.EquipmentId);
        }
    }
}
