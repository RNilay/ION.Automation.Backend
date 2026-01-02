using IonFiltra.BagFilters.Core.Common;
using IonFiltra.BagFilters.Core.Entities.Assignment;
using IonFiltra.BagFilters.Core.Entities.BagfilterDatabase.WithCanopy;
using IonFiltra.BagFilters.Core.Entities.BagfilterDatabase.WithoutCanopy;
using IonFiltra.BagFilters.Core.Entities.Bagfilters.BagfilterInputs;
using IonFiltra.BagFilters.Core.Entities.Bagfilters.BagfilterMasterEntity;
using IonFiltra.BagFilters.Core.Entities.Bagfilters.Sections.Access_Group;
using IonFiltra.BagFilters.Core.Entities.Bagfilters.Sections.Bag_Selection;
using IonFiltra.BagFilters.Core.Entities.Bagfilters.Sections.Cage_Inputs;
using IonFiltra.BagFilters.Core.Entities.Bagfilters.Sections.Capsule_Inputs;
using IonFiltra.BagFilters.Core.Entities.Bagfilters.Sections.Casing_Inputs;
using IonFiltra.BagFilters.Core.Entities.Bagfilters.Sections.Hopper_Trough;
using IonFiltra.BagFilters.Core.Entities.Bagfilters.Sections.Painting;
using IonFiltra.BagFilters.Core.Entities.Bagfilters.Sections.Process_Info;
using IonFiltra.BagFilters.Core.Entities.Bagfilters.Sections.Roof_Door;
using IonFiltra.BagFilters.Core.Entities.Bagfilters.Sections.Structure_Inputs;
using IonFiltra.BagFilters.Core.Entities.Bagfilters.Sections.Support_Structure;
using IonFiltra.BagFilters.Core.Entities.Bagfilters.Sections.Weight_Summary;
using IonFiltra.BagFilters.Core.Entities.BOM.Bill_Of_Material;
using IonFiltra.BagFilters.Core.Entities.BOM.Cage_Cost;
using IonFiltra.BagFilters.Core.Entities.BOM.Damper_Cost;
using IonFiltra.BagFilters.Core.Entities.BOM.Painting_Cost;
using IonFiltra.BagFilters.Core.Entities.BOM.PaintingRates;
using IonFiltra.BagFilters.Core.Entities.BOM.Rates;
using IonFiltra.BagFilters.Core.Entities.BOM.Transp_Cost;
using IonFiltra.BagFilters.Core.Entities.EnquiryEntity;
using IonFiltra.BagFilters.Core.Entities.MasterData.BoughtOutItems;
using IonFiltra.BagFilters.Core.Entities.MasterData.DPTData;
using IonFiltra.BagFilters.Core.Entities.MasterData.FilterBagData;
using IonFiltra.BagFilters.Core.Entities.MasterData.Master_Definition;
using IonFiltra.BagFilters.Core.Entities.MasterData.SolenoidValveData;
using IonFiltra.BagFilters.Core.Entities.MasterData.TimerData;
using IonFiltra.BagFilters.Core.Entities.SkyCivEntities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;


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


        public DbSet<AnalysisSession> AnalysisSessions { get; set; }
        public DbSet<AnalysisResult> AnalysisResults { get; set; }

        //Sections tables
        public DbSet<WeightSummary> WeightSummarys { get; set; }
        public DbSet<ProcessInfo> ProcessInfos { get; set; }
        public DbSet<CageInputs> CageInputss { get; set; }
        public DbSet<BagSelection> BagSelections { get; set; }
        public DbSet<StructureInputs> StructureInputss { get; set; }
        public DbSet<CapsuleInputs> CapsuleInputss { get; set; }
        public DbSet<CasingInputs> CasingInputss { get; set; }
        public DbSet<HopperInputs> HopperInputss { get; set; }
        public DbSet<SupportStructure> SupportStructures { get; set; }
        public DbSet<AccessGroup> AccessGroups { get; set; }
        public DbSet<RoofDoor> RoofDoors { get; set; }
        public DbSet<PaintingArea> PaintingAreas { get; set; }

        //Bill Of Material Table
        public DbSet<BillOfMaterial> BillOfMaterials { get; set; }
        public DbSet<PaintingCost> PaintingCosts { get; set; }


        // Database from ion filtra for bag filters
        public DbSet<IFI_Bagfilter_Database_Without_Canopy> IFI_Bagfilter_Database_Without_Canopys { get; set; }
        public DbSet<IFI_Bagfilter_Database_With_Canopy> IFI_Bagfilter_Database_With_Canopys { get; set; }

        //BOM Rates
        public DbSet<BillOfMaterialRates> BillOfMaterialRatess { get; set; }

        public DbSet<PaintingCostConfig> PaintingCostConfigs { get; set; }


        public DbSet<BoughtOutItemSelection> BoughtOutItemSelections { get; set; }

        //Master Data tables
        public DbSet<MasterDefinitions> MasterDefinitions { get; set; }
        public DbSet<FilterBag> FilterBags { get; set; }
        public DbSet<TimerEntity> TimerEntitys { get; set; }

        public DbSet<SolenoidValve> SolenoidValves { get; set; }

        public DbSet<DPTEntity> DPTEntitys { get; set; }

        public DbSet<TransportationCostEntity> TransportationCostEntitys { get; set; }

        public DbSet<DamperCostEntity> DamperCostEntitys { get; set; }

        public DbSet<CageCostEntity> CageCostEntitys { get; set; }


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

            //Section tables
            modelBuilder.Entity<WeightSummary>(entity =>
            {
                entity.ToTable("WeightSummary", GlobalConstants.IONFILTRA_SCHEMA);
                entity.HasKey(u => u.Id);
                entity.Property(u => u.EnquiryId).IsRequired();
            });

            modelBuilder.Entity<ProcessInfo>(entity =>
            {
                entity.ToTable("ProcessInfo", GlobalConstants.IONFILTRA_SCHEMA);
                entity.HasKey(u => u.Id);
                entity.Property(u => u.EnquiryId).IsRequired();
            });

            modelBuilder.Entity<CageInputs>(entity =>
            {
                entity.ToTable("CageInputs", GlobalConstants.IONFILTRA_SCHEMA);
                entity.HasKey(u => u.Id);
                entity.Property(u => u.EnquiryId).IsRequired();
            });

            modelBuilder.Entity<BagSelection>(entity =>
            {
                entity.ToTable("BagSelection", GlobalConstants.IONFILTRA_SCHEMA);
                entity.HasKey(u => u.Id);
                entity.Property(u => u.EnquiryId).IsRequired();
            });

            modelBuilder.Entity<StructureInputs>(entity =>
            {
                entity.ToTable("StructureInputs", GlobalConstants.IONFILTRA_SCHEMA);
                entity.HasKey(u => u.Id);
                entity.Property(u => u.EnquiryId).IsRequired();
            });

            modelBuilder.Entity<CapsuleInputs>(entity =>
            {
                entity.ToTable("CapsuleInputs", GlobalConstants.IONFILTRA_SCHEMA);
                entity.HasKey(u => u.Id);
                entity.Property(u => u.EnquiryId).IsRequired();
            });

            modelBuilder.Entity<CasingInputs>(entity =>
            {
                entity.ToTable("CasingInputs", GlobalConstants.IONFILTRA_SCHEMA);
                entity.HasKey(u => u.Id);
                entity.Property(u => u.EnquiryId).IsRequired();
            });

            modelBuilder.Entity<HopperInputs>(entity =>
            {
                entity.ToTable("HopperInputs", GlobalConstants.IONFILTRA_SCHEMA);
                entity.HasKey(u => u.Id);
                entity.Property(u => u.EnquiryId).IsRequired();
            });

            modelBuilder.Entity<SupportStructure>(entity =>
            {
                entity.ToTable("SupportStructure", GlobalConstants.IONFILTRA_SCHEMA);
                entity.HasKey(u => u.Id);
                entity.Property(u => u.EnquiryId).IsRequired();
            });

            modelBuilder.Entity<AccessGroup>(entity =>
            {
                entity.ToTable("AccessGroup", GlobalConstants.IONFILTRA_SCHEMA);
                entity.HasKey(u => u.Id);
                entity.Property(u => u.EnquiryId).IsRequired();
            });

            modelBuilder.Entity<RoofDoor>(entity =>
            {
                entity.ToTable("RoofDoor", GlobalConstants.IONFILTRA_SCHEMA);
                entity.HasKey(u => u.Id);
                entity.Property(u => u.EnquiryId).IsRequired();
            });

            modelBuilder.Entity<PaintingArea>(entity =>
            {
                entity.ToTable("PaintingArea", GlobalConstants.IONFILTRA_SCHEMA);
                entity.HasKey(u => u.Id);
                entity.Property(u => u.EnquiryId).IsRequired();
            });

            // Bill of Material
            modelBuilder.Entity<BillOfMaterial>(entity =>
            {
                entity.ToTable("BillOfMaterial", GlobalConstants.IONFILTRA_SCHEMA);
                entity.HasKey(u => u.Id);
                entity.Property(u => u.EnquiryId).IsRequired();
            });

            modelBuilder.Entity<PaintingCost>(entity =>
            {
                entity.ToTable("PaintingCost", GlobalConstants.IONFILTRA_SCHEMA);
                entity.HasKey(u => u.Id);
                entity.Property(u => u.EnquiryId).IsRequired();
            });

            // Ion filtra database for bagfilters
            modelBuilder.Entity<IFI_Bagfilter_Database_Without_Canopy>(entity =>
            {
                entity.ToTable("IFI_Bagfilter_Database_Without_Canopy", GlobalConstants.IONFILTRA_SCHEMA);
                entity.HasKey(u => u.Id);
            });

            modelBuilder.Entity<IFI_Bagfilter_Database_With_Canopy>(entity =>
            {
                entity.ToTable("IFI_Bagfilter_Database_With_Canopy", GlobalConstants.IONFILTRA_SCHEMA);
                entity.HasKey(u => u.Id);
            });

            // BOM Rates
            modelBuilder.Entity<BillOfMaterialRates>(entity =>
            {
                entity.ToTable("BillOfMaterialRates", GlobalConstants.IONFILTRA_SCHEMA);
                entity.HasKey(u => u.Id);
            });

            modelBuilder.Entity<PaintingCostConfig>(entity =>
            {
                entity.ToTable("PaintingCostConfig", GlobalConstants.IONFILTRA_SCHEMA);
                entity.HasKey(u => u.Id);
            });


            modelBuilder.Entity<BoughtOutItemSelection>(entity =>
            {
                entity.ToTable("BoughtOutItemSelection", GlobalConstants.IONFILTRA_SCHEMA);
                entity.HasKey(u => u.Id);
                entity.Property(u => u.EnquiryId).IsRequired();
            });

            //master data tables

            modelBuilder.Entity<MasterDefinitions>(entity =>
            {
                entity.ToTable("MasterDefinitions", GlobalConstants.IONFILTRA_SCHEMA);
                entity.HasKey(u => u.Id);
                entity.Property(u => u.MasterKey).IsRequired();
            });

            modelBuilder.Entity<FilterBag>(entity =>
            {
                entity.ToTable("FilterBag", GlobalConstants.IONFILTRA_SCHEMA);
                entity.HasKey(u => u.Id);
            });

            modelBuilder.Entity<TimerEntity>(entity =>
            {
                entity.ToTable("TimerEntity", GlobalConstants.IONFILTRA_SCHEMA);
                entity.HasKey(u => u.Id);
            });

            modelBuilder.Entity<SolenoidValve>(entity =>
            {
                entity.ToTable("SolenoidValve", GlobalConstants.IONFILTRA_SCHEMA);
                entity.HasKey(u => u.Id);
            });

            modelBuilder.Entity<DPTEntity>(entity =>
            {
                entity.ToTable("DPTEntity", GlobalConstants.IONFILTRA_SCHEMA);
                entity.HasKey(u => u.Id);
            });

            modelBuilder.Entity<TransportationCostEntity>(entity =>
            {
                entity.ToTable("TransportationCostEntity", GlobalConstants.IONFILTRA_SCHEMA);
                entity.HasKey(u => u.Id);
                entity.Property(u => u.EnquiryId).IsRequired();
            });

            modelBuilder.Entity<DamperCostEntity>(entity =>
            {
                entity.ToTable("DamperCostEntity", GlobalConstants.IONFILTRA_SCHEMA);
                entity.HasKey(u => u.Id);
                entity.Property(u => u.EnquiryId).IsRequired();
            });

            modelBuilder.Entity<CageCostEntity>(entity =>
            {
                entity.ToTable("CageCostEntity", GlobalConstants.IONFILTRA_SCHEMA);
                entity.HasKey(u => u.Id);
                entity.Property(u => u.EnquiryId).IsRequired();
            });
        }
    }
}
