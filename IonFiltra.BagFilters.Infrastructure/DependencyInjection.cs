using IonFiltra.BagFilters.Application.Interfaces;
using IonFiltra.BagFilters.Application.Interfaces.Bagfilters.BagfilterMaster;
using IonFiltra.BagFilters.Application.Interfaces.Enquiry;
using IonFiltra.BagFilters.Application.Services.Assignment;
using IonFiltra.BagFilters.Application.Services.BagfilterDatabase.WithoutCanopy;
using IonFiltra.BagFilters.Application.Services.Bagfilters.BagfilterInputs;
using IonFiltra.BagFilters.Application.Services.Bagfilters.BagfilterMasterEntity;
using IonFiltra.BagFilters.Application.Services.EnquiryService;
using IonFiltra.BagFilters.Application.Services.SkyCiv;
using IonFiltra.BagFilters.Core.Interfaces.Bagfilters.BagfilterMasters;
using IonFiltra.BagFilters.Core.Interfaces.EnquiryRep;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.Assignment;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.BagfilterDatabase.WithoutCanopy;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.Bagfilters.BagfilterInputs;
using IonFiltra.BagFilters.Core.Interfaces.SkyCiv;
using IonFiltra.BagFilters.Infrastructure.Data;
using IonFiltra.BagFilters.Infrastructure.EnquiryRepo;
using IonFiltra.BagFilters.Infrastructure.Repositories.Assignment;
using IonFiltra.BagFilters.Infrastructure.Repositories.BagfilterDatabase.WithoutCanopy;
using IonFiltra.BagFilters.Infrastructure.Repositories.Bagfilters.BagfilterInputs;
using IonFiltra.BagFilters.Infrastructure.Repositories.Bagfilters.BagfilterMasters;
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



            // SkyCiv Services

            // In Program.cs or DependencyInjection
            services.AddScoped<ISkyCivAnalysisService, SkyCivAnalysisService>();
            services.AddScoped<IAnalysisSessionRepository, AnalysisSessionRepository>();

            // Ion filtra Data base for Bagfilters
            services.AddScoped<IIFI_Bagfilter_Database_Without_CanopyService, IFI_Bagfilter_Database_Without_CanopyService>();
            services.AddScoped<IIFI_Bagfilter_Database_Without_CanopyRepository, IFI_Bagfilter_Database_Without_CanopyRepository>();

            return services;
        }
    }
}
