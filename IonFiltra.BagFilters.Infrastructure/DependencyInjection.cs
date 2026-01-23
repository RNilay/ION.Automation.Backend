using IonFiltra.BagFilters.Application.Interfaces;
using IonFiltra.BagFilters.Application.Interfaces.Bagfilters.BagfilterMaster;
using IonFiltra.BagFilters.Application.Interfaces.Bagfilters.Sections.Bag_Selection;
using IonFiltra.BagFilters.Application.Interfaces.Enquiry;
using IonFiltra.BagFilters.Application.Interfaces.GenericView;
using IonFiltra.BagFilters.Application.Interfaces.MasterData.Master_Definition;
using IonFiltra.BagFilters.Application.Interfaces.Report;
using IonFiltra.BagFilters.Application.Interfaces.Users.User;
using IonFiltra.BagFilters.Application.Services.Assignment;
using IonFiltra.BagFilters.Application.Services.BagfilterDatabase.WithCanopy;
using IonFiltra.BagFilters.Application.Services.BagfilterDatabase.WithoutCanopy;
using IonFiltra.BagFilters.Application.Services.Bagfilters.BagfilterInputs;
using IonFiltra.BagFilters.Application.Services.Bagfilters.BagfilterMasterEntity;
using IonFiltra.BagFilters.Application.Services.Bagfilters.Sections.Access_Group;
using IonFiltra.BagFilters.Application.Services.Bagfilters.Sections.Bag_Selection;
using IonFiltra.BagFilters.Application.Services.Bagfilters.Sections.Cage_Inputs;
using IonFiltra.BagFilters.Application.Services.Bagfilters.Sections.Capsule_Inputs;
using IonFiltra.BagFilters.Application.Services.Bagfilters.Sections.Casing_Inputs;
using IonFiltra.BagFilters.Application.Services.Bagfilters.Sections.DamperSize;
using IonFiltra.BagFilters.Application.Services.Bagfilters.Sections.EV;
using IonFiltra.BagFilters.Application.Services.Bagfilters.Sections.Hopper_Trough;
using IonFiltra.BagFilters.Application.Services.Bagfilters.Sections.Painting;
using IonFiltra.BagFilters.Application.Services.Bagfilters.Sections.Process_Info;
using IonFiltra.BagFilters.Application.Services.Bagfilters.Sections.Roof_Door;
using IonFiltra.BagFilters.Application.Services.Bagfilters.Sections.Structure_Inputs;
using IonFiltra.BagFilters.Application.Services.Bagfilters.Sections.Support_Structure;
using IonFiltra.BagFilters.Application.Services.Bagfilters.Sections.Weight_Summary;
using IonFiltra.BagFilters.Application.Services.BOM.Bill_Of_Material;
using IonFiltra.BagFilters.Application.Services.BOM.Cage_Cost;
using IonFiltra.BagFilters.Application.Services.BOM.Damper_Cost;
using IonFiltra.BagFilters.Application.Services.BOM.Painting_Cost;
using IonFiltra.BagFilters.Application.Services.BOM.PaintingRates;
using IonFiltra.BagFilters.Application.Services.BOM.Rates;
using IonFiltra.BagFilters.Application.Services.BOM.Transp_Cost;
using IonFiltra.BagFilters.Application.Services.EnquiryService;
using IonFiltra.BagFilters.Application.Services.GenericView;
using IonFiltra.BagFilters.Application.Services.MasterData.BoughtOutItems;
using IonFiltra.BagFilters.Application.Services.MasterData.DPTData;
using IonFiltra.BagFilters.Application.Services.MasterData.FilterBagData;
using IonFiltra.BagFilters.Application.Services.MasterData.Master_Definition;
using IonFiltra.BagFilters.Application.Services.MasterData.SolenoidValveData;
using IonFiltra.BagFilters.Application.Services.MasterData.TimerData;
using IonFiltra.BagFilters.Application.Services.Report;
using IonFiltra.BagFilters.Application.Services.SkyCiv;
using IonFiltra.BagFilters.Application.Services.Users.User;
using IonFiltra.BagFilters.Application.Services.Users.UserRoles;
using IonFiltra.BagFilters.Core.Interfaces.Bagfilters.BagfilterMasters;
using IonFiltra.BagFilters.Core.Interfaces.EnquiryRep;
using IonFiltra.BagFilters.Core.Interfaces.GenericView;
using IonFiltra.BagFilters.Core.Interfaces.MasterData.Master_Definition;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.Assignment;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.BagfilterDatabase.WithCanopy;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.BagfilterDatabase.WithoutCanopy;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.Bagfilters.BagfilterInputs;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.Bagfilters.Sections.Access_Group;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.Bagfilters.Sections.Bag_Selection;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.Bagfilters.Sections.Cage_Inputs;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.Bagfilters.Sections.Capsule_Inputs;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.Bagfilters.Sections.Casing_Inputs;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.Bagfilters.Sections.DamperSize;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.Bagfilters.Sections.EV;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.Bagfilters.Sections.Hopper_Trough;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.Bagfilters.Sections.Painting;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.Bagfilters.Sections.Process_Info;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.Bagfilters.Sections.Roof_Door;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.Bagfilters.Sections.Structure_Inputs;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.Bagfilters.Sections.Support_Structure;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.Bagfilters.Sections.Weight_Summary;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.BOM.Bill_Of_Material;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.BOM.Cage_Cost;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.BOM.Damper_Cost;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.BOM.Painting_Cost;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.BOM.PaintingRates;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.BOM.Rates;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.BOM.Transp_Cost;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.MasterData.BoughtOutItems;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.MasterData.DPTData;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.MasterData.FilterBagData;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.MasterData.SolenoidValveData;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.MasterData.TimerData;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.Users.UserRoles;
using IonFiltra.BagFilters.Core.Interfaces.SkyCiv;
using IonFiltra.BagFilters.Core.Interfaces.Users.User;
using IonFiltra.BagFilters.Infrastructure.Data;
using IonFiltra.BagFilters.Infrastructure.EnquiryRepo;
using IonFiltra.BagFilters.Infrastructure.Repositories.Assignment;
using IonFiltra.BagFilters.Infrastructure.Repositories.BagfilterDatabase.WithCanopy;
using IonFiltra.BagFilters.Infrastructure.Repositories.BagfilterDatabase.WithoutCanopy;
using IonFiltra.BagFilters.Infrastructure.Repositories.Bagfilters.BagfilterInputs;
using IonFiltra.BagFilters.Infrastructure.Repositories.Bagfilters.BagfilterMasters;
using IonFiltra.BagFilters.Infrastructure.Repositories.Bagfilters.Sections.Access_Group;
using IonFiltra.BagFilters.Infrastructure.Repositories.Bagfilters.Sections.Bag_Selection;
using IonFiltra.BagFilters.Infrastructure.Repositories.Bagfilters.Sections.Cage_Inputs;
using IonFiltra.BagFilters.Infrastructure.Repositories.Bagfilters.Sections.Capsule_Inputs;
using IonFiltra.BagFilters.Infrastructure.Repositories.Bagfilters.Sections.Casing_Inputs;
using IonFiltra.BagFilters.Infrastructure.Repositories.Bagfilters.Sections.DamperSize;
using IonFiltra.BagFilters.Infrastructure.Repositories.Bagfilters.Sections.EV;
using IonFiltra.BagFilters.Infrastructure.Repositories.Bagfilters.Sections.Hopper_Trough;
using IonFiltra.BagFilters.Infrastructure.Repositories.Bagfilters.Sections.Painting;
using IonFiltra.BagFilters.Infrastructure.Repositories.Bagfilters.Sections.Process_Info;
using IonFiltra.BagFilters.Infrastructure.Repositories.Bagfilters.Sections.Roof_Door;
using IonFiltra.BagFilters.Infrastructure.Repositories.Bagfilters.Sections.Structure_Inputs;
using IonFiltra.BagFilters.Infrastructure.Repositories.Bagfilters.Sections.Support_Structure;
using IonFiltra.BagFilters.Infrastructure.Repositories.Bagfilters.Sections.Weight_Summary;
using IonFiltra.BagFilters.Infrastructure.Repositories.BOM.Bill_Of_Material;
using IonFiltra.BagFilters.Infrastructure.Repositories.BOM.Cage_Cost;
using IonFiltra.BagFilters.Infrastructure.Repositories.BOM.Damper_Cost;
using IonFiltra.BagFilters.Infrastructure.Repositories.BOM.Painting_Cost;
using IonFiltra.BagFilters.Infrastructure.Repositories.BOM.PaintingRates;
using IonFiltra.BagFilters.Infrastructure.Repositories.BOM.Rates;
using IonFiltra.BagFilters.Infrastructure.Repositories.BOM.Transp_Cost;
using IonFiltra.BagFilters.Infrastructure.Repositories.GenericView;
using IonFiltra.BagFilters.Infrastructure.Repositories.MasterData.BoughtOutItems;
using IonFiltra.BagFilters.Infrastructure.Repositories.MasterData.DPTData;
using IonFiltra.BagFilters.Infrastructure.Repositories.MasterData.FilterBagData;
using IonFiltra.BagFilters.Infrastructure.Repositories.MasterData.Master_Definition;
using IonFiltra.BagFilters.Infrastructure.Repositories.MasterData.SolenoidValveData;
using IonFiltra.BagFilters.Infrastructure.Repositories.MasterData.TimerData;
using IonFiltra.BagFilters.Infrastructure.Repositories.SkyCiv;
using IonFiltra.BagFilters.Infrastructure.Repositories.Users.User;
using IonFiltra.BagFilters.Infrastructure.Repositories.Users.UserRoles;
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

            services.AddScoped<IOtpRepository, OtpRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserMfaRepository, UserMfaRepository>();
            services.AddScoped<IUserMfaService, UserMfaService>();



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

            services.AddScoped<IPaintingAreaService, PaintingAreaService>();
            services.AddScoped<IPaintingAreaRepository, PaintingAreaRepository>();

            //Bill of Material
            services.AddScoped<IBillOfMaterialService, BillOfMaterialService>();
            services.AddScoped<IBillOfMaterialRepository, BillOfMaterialRepository>();

            services.AddScoped<IPaintingCostService, PaintingCostService>();
            services.AddScoped<IPaintingCostRepository, PaintingCostRepository>();

            // SkyCiv Services
            // In Program.cs or DependencyInjection
            services.AddScoped<ISkyCivAnalysisService, SkyCivAnalysisService>();
            services.AddScoped<IAnalysisSessionRepository, AnalysisSessionRepository>();

            // Ion filtra Data base for Bagfilters
            services.AddScoped<IIFI_Bagfilter_Database_Without_CanopyService, IFI_Bagfilter_Database_Without_CanopyService>();
            services.AddScoped<IIFI_Bagfilter_Database_Without_CanopyRepository, IFI_Bagfilter_Database_Without_CanopyRepository>();

            services.AddScoped<IIFI_Bagfilter_Database_With_CanopyService, IFI_Bagfilter_Database_With_CanopyService>();
            services.AddScoped<IIFI_Bagfilter_Database_With_CanopyRepository, IFI_Bagfilter_Database_With_CanopyRepository>();

            //Generic View
            services.AddScoped<IGenericViewRepository, GenericViewRepository>();
            services.AddScoped<IGenericViewService, GenericViewService>();

            //Report Services
            services.AddScoped<IReportService, ReportService>();

            //BOM Rates
            services.AddScoped<IBillOfMaterialRatesService, BillOfMaterialRatesService>();
            services.AddScoped<IBillOfMaterialRatesRepository, BillOfMaterialRatesRepository>();

            services.AddScoped<IPaintingCostConfigService, PaintingCostConfigService>();
            services.AddScoped<IPaintingCostConfigRepository, PaintingCostConfigRepository>();

            services.AddScoped<IBoughtOutItemSelectionService, BoughtOutItemSelectionService>();
            services.AddScoped<IBoughtOutItemSelectionRepository, BoughtOutItemSelectionRepository>();

            //master data tables

            services.AddScoped<IMasterDefinitionsRepository, MasterDefinitionsRepository>();
            services.AddScoped<IMasterDefinitionsService, MasterDefinitionsService>();


            services.AddScoped<IFilterBagService, FilterBagService>();
            services.AddScoped<IFilterBagRepository, FilterBagRepository>();

            services.AddScoped<ITimerEntityService, TimerEntityService>();
            services.AddScoped<ITimerEntityRepository, TimerEntityRepository>();

            services.AddScoped<ISolenoidValveService, SolenoidValveService>();
            services.AddScoped<ISolenoidValveRepository, SolenoidValveRepository>();

            services.AddScoped<IDPTEntityService, DPTEntityService>();
            services.AddScoped<IDPTEntityRepository, DPTEntityRepository>();

            services.AddScoped<ITransportationCostEntityService, TransportationCostEntityService>();
            services.AddScoped<ITransportationCostEntityRepository, TransportationCostEntityRepository>();

            services.AddScoped<IDamperCostEntityService, DamperCostEntityService>();
            services.AddScoped<IDamperCostEntityRepository, DamperCostEntityRepository>();

            services.AddScoped<ICageCostEntityService, CageCostEntityService>();
            services.AddScoped<ICageCostEntityRepository, CageCostEntityRepository>();

            services.AddScoped<IDamperSizeInputsService, DamperSizeInputsService>();
            services.AddScoped<IDamperSizeInputsRepository, DamperSizeInputsRepository>();

            services.AddScoped<IExplosionVentEntityService, ExplosionVentEntityService>();
            services.AddScoped<IExplosionVentEntityRepository, ExplosionVentEntityRepository>();

            services.AddScoped<IApplicationRolesService, ApplicationRolesService>();
            services.AddScoped<IApplicationRolesRepository, ApplicationRolesRepository>();

            return services;
        }
    }
}
