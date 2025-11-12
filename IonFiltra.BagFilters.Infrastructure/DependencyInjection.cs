using IonFiltra.BagFilters.Application.Interfaces;
using IonFiltra.BagFilters.Application.Interfaces.Bagfilters.BagfilterMaster;
using IonFiltra.BagFilters.Application.Interfaces.Bagfilters.Sections.Bag_Selection;
using IonFiltra.BagFilters.Application.Interfaces.Enquiry;
using IonFiltra.BagFilters.Application.Interfaces.GenericView;
using IonFiltra.BagFilters.Application.Interfaces.Report;
using IonFiltra.BagFilters.Application.Services.Assignment;
using IonFiltra.BagFilters.Application.Services.BagfilterDatabase.WithoutCanopy;
using IonFiltra.BagFilters.Application.Services.Bagfilters.BagfilterInputs;
using IonFiltra.BagFilters.Application.Services.Bagfilters.BagfilterMasterEntity;
using IonFiltra.BagFilters.Application.Services.Bagfilters.Sections.Access_Group;
using IonFiltra.BagFilters.Application.Services.Bagfilters.Sections.Bag_Selection;
using IonFiltra.BagFilters.Application.Services.Bagfilters.Sections.Cage_Inputs;
using IonFiltra.BagFilters.Application.Services.Bagfilters.Sections.Capsule_Inputs;
using IonFiltra.BagFilters.Application.Services.Bagfilters.Sections.Casing_Inputs;
using IonFiltra.BagFilters.Application.Services.Bagfilters.Sections.Hopper_Trough;
using IonFiltra.BagFilters.Application.Services.Bagfilters.Sections.Process_Info;
using IonFiltra.BagFilters.Application.Services.Bagfilters.Sections.Roof_Door;
using IonFiltra.BagFilters.Application.Services.Bagfilters.Sections.Structure_Inputs;
using IonFiltra.BagFilters.Application.Services.Bagfilters.Sections.Support_Structure;
using IonFiltra.BagFilters.Application.Services.Bagfilters.Sections.Weight_Summary;
using IonFiltra.BagFilters.Application.Services.EnquiryService;
using IonFiltra.BagFilters.Application.Services.GenericView;
using IonFiltra.BagFilters.Application.Services.Report;
using IonFiltra.BagFilters.Application.Services.SkyCiv;
using IonFiltra.BagFilters.Core.Interfaces.Bagfilters.BagfilterMasters;
using IonFiltra.BagFilters.Core.Interfaces.EnquiryRep;
using IonFiltra.BagFilters.Core.Interfaces.GenericView;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.Assignment;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.BagfilterDatabase.WithoutCanopy;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.Bagfilters.BagfilterInputs;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.Bagfilters.Sections.Access_Group;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.Bagfilters.Sections.Bag_Selection;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.Bagfilters.Sections.Cage_Inputs;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.Bagfilters.Sections.Capsule_Inputs;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.Bagfilters.Sections.Casing_Inputs;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.Bagfilters.Sections.Hopper_Trough;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.Bagfilters.Sections.Process_Info;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.Bagfilters.Sections.Roof_Door;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.Bagfilters.Sections.Structure_Inputs;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.Bagfilters.Sections.Support_Structure;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.Bagfilters.Sections.Weight_Summary;
using IonFiltra.BagFilters.Core.Interfaces.SkyCiv;
using IonFiltra.BagFilters.Infrastructure.Data;
using IonFiltra.BagFilters.Infrastructure.EnquiryRepo;
using IonFiltra.BagFilters.Infrastructure.Repositories.Assignment;
using IonFiltra.BagFilters.Infrastructure.Repositories.BagfilterDatabase.WithoutCanopy;
using IonFiltra.BagFilters.Infrastructure.Repositories.Bagfilters.BagfilterInputs;
using IonFiltra.BagFilters.Infrastructure.Repositories.Bagfilters.BagfilterMasters;
using IonFiltra.BagFilters.Infrastructure.Repositories.Bagfilters.Sections.Access_Group;
using IonFiltra.BagFilters.Infrastructure.Repositories.Bagfilters.Sections.Bag_Selection;
using IonFiltra.BagFilters.Infrastructure.Repositories.Bagfilters.Sections.Cage_Inputs;
using IonFiltra.BagFilters.Infrastructure.Repositories.Bagfilters.Sections.Capsule_Inputs;
using IonFiltra.BagFilters.Infrastructure.Repositories.Bagfilters.Sections.Casing_Inputs;
using IonFiltra.BagFilters.Infrastructure.Repositories.Bagfilters.Sections.Hopper_Trough;
using IonFiltra.BagFilters.Infrastructure.Repositories.Bagfilters.Sections.Process_Info;
using IonFiltra.BagFilters.Infrastructure.Repositories.Bagfilters.Sections.Roof_Door;
using IonFiltra.BagFilters.Infrastructure.Repositories.Bagfilters.Sections.Structure_Inputs;
using IonFiltra.BagFilters.Infrastructure.Repositories.Bagfilters.Sections.Support_Structure;
using IonFiltra.BagFilters.Infrastructure.Repositories.Bagfilters.Sections.Weight_Summary;
using IonFiltra.BagFilters.Infrastructure.Repositories.GenericView;
using IonFiltra.BagFilters.Infrastructure.Repositories.SkyCiv;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure; // ✅ for MySqlServerVersion

namespace IonFiltra.BagFilters.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
        {
            var serverVersion = new MySqlServerVersion(new Version(8, 0, 43)); // ✅ adjust to your MySQL version

            // Register DbContext
            services.AddDbContext<AppDbContext>(options =>
                options.UseMySql(connectionString, serverVersion));

            // Register DbContextFactory
            services.AddDbContextFactory<AppDbContext>(options =>
                options.UseMySql(connectionString, serverVersion),
                ServiceLifetime.Scoped);

            // Register helpers & repositories
            services.AddScoped<TransactionHelper>();
            services.AddScoped<IEnquiryService, EnquiryService>();
            services.AddScoped<IEnquiryRepository, EnquiryRepository>();

            services.AddScoped<IAssignmentEntityService, AssignmentEntityService>();
            services.AddScoped<IAssignmentEntityRepository, AssignmentEntityRepository>();

            services.AddScoped<IBagfilterMasterService, BagfilterMasterService>();
            services.AddScoped<IBagfilterMasterRepository, BagfilterMasterRepository>();

            services.AddScoped<IBagfilterInputService, BagfilterInputService>();
            services.AddScoped<IBagfilterInputRepository, BagfilterInputRepository>();

            //Sections Tables
            services.AddScoped<IWeightSummaryService, WeightSummaryService>();
            services.AddScoped<IWeightSummaryRepository, WeightSummaryRepository>();

            services.AddScoped<IProcessInfoService, ProcessInfoService>();
            services.AddScoped<IProcessInfoRepository, ProcessInfoRepository>();

            services.AddScoped<ICageInputsService, CageInputsService>();
            services.AddScoped<ICageInputsRepository, CageInputsRepository>();

            services.AddScoped<IBagSelectionService, BagSelectionService>();
            services.AddScoped<IBagSelectionRepository, BagSelectionRepository>();

            services.AddScoped<IStructureInputsService, StructureInputsService>();
            services.AddScoped<IStructureInputsRepository, StructureInputsRepository>();

            services.AddScoped<ICapsuleInputsService, CapsuleInputsService>();
            services.AddScoped<ICapsuleInputsRepository, CapsuleInputsRepository>();

            services.AddScoped<ICasingInputsService, CasingInputsService>();
            services.AddScoped<ICasingInputsRepository, CasingInputsRepository>();

            services.AddScoped<IHopperInputsService, HopperInputsService>();
            services.AddScoped<IHopperInputsRepository, HopperInputsRepository>();

            services.AddScoped<ISupportStructureService, SupportStructureService>();
            services.AddScoped<ISupportStructureRepository, SupportStructureRepository>();

            services.AddScoped<IAccessGroupService, AccessGroupService>();
            services.AddScoped<IAccessGroupRepository, AccessGroupRepository>();

            services.AddScoped<IRoofDoorService, RoofDoorService>();
            services.AddScoped<IRoofDoorRepository, RoofDoorRepository>();

            // SkyCiv Services
            // In Program.cs or DependencyInjection
            services.AddScoped<ISkyCivAnalysisService, SkyCivAnalysisService>();
            services.AddScoped<IAnalysisSessionRepository, AnalysisSessionRepository>();

            // Ion filtra Data base for Bagfilters
            services.AddScoped<IIFI_Bagfilter_Database_Without_CanopyService, IFI_Bagfilter_Database_Without_CanopyService>();
            services.AddScoped<IIFI_Bagfilter_Database_Without_CanopyRepository, IFI_Bagfilter_Database_Without_CanopyRepository>();

            //Generic View
            services.AddScoped<IGenericViewRepository, GenericViewRepository>();
            services.AddScoped<IGenericViewService, GenericViewService>();

            //Report Services
            services.AddScoped<IReportService, ReportService>();

            return services;
        }
    }
}
