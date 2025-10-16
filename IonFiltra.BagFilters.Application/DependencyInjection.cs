using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IonFiltra.BagFilters.Application.Interfaces.Enquiry;
using IonFiltra.BagFilters.Application.Services.EnquiryService;

using Microsoft.Extensions.DependencyInjection;

namespace IonFiltra.BagFilters.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
          

            //services.AddScoped<TransactionHelper>();
            //services.AddScoped<IEnquiryService, EnquiryService>();
            //services.AddScoped<IEnquiryRepository, EnquiryRepository>();

            return services;
        }
    }
}
