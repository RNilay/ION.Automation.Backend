using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using IonFiltra.BagFilters.Core.Common;
using IonFiltra.BagFilters.Core.Entities.Assignment;
using IonFiltra.BagFilters.Core.Entities.Bagfilters.BagfilterInputs;
using IonFiltra.BagFilters.Core.Entities.Bagfilters.BagfilterMasterEntity;
using IonFiltra.BagFilters.Core.Entities.EnquiryEntity;
using Microsoft.EntityFrameworkCore;


namespace IonFiltra.BagFilters.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
        {
        }


        public DbSet<Enquiry> Enquirys { get; set; }
        public DbSet<AssignmentEntity> AssignmentEntitys { get; set; }
        public DbSet<BagfilterMaster> BagfilterMasters { get; set; }
        public DbSet<BagfilterInput> BagfilterInputs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Fluent API configs

            modelBuilder.Entity<Enquiry>(entity =>
            {
                entity.ToTable("Enquiry", GlobalConstants.IONFILTRA_SCHEMA);
                entity.HasKey(u => u.Id);
                entity.Property(u => u.EnquiryId).IsRequired();
            });

            modelBuilder.Entity<AssignmentEntity>(entity =>
            {
                entity.ToTable("AssignmentEntity", GlobalConstants.IONFILTRA_SCHEMA);
                entity.HasKey(u => u.Id);
                entity.Property(u => u.EnquiryId).IsRequired();
            });

            modelBuilder.Entity<BagfilterMaster>(entity =>
            {
                entity.ToTable("BagfilterMaster", GlobalConstants.IONFILTRA_SCHEMA);
                entity.HasKey(u => u.BagfilterMasterId);
                entity.Property(u => u.AssignmentId);
            });

            modelBuilder.Entity<BagfilterInput>(entity =>
            {
                entity.ToTable("BagfilterInput", GlobalConstants.IONFILTRA_SCHEMA);
                entity.HasKey(u => u.BagfilterInputId);
                entity.Property(u => u.BagfilterMasterId).IsRequired();
            });
        }
    }
}
