using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IonFiltra.BagFilters.Application.DTOs.Report;
using IonFiltra.BagFilters.Application.Models.Report;

namespace IonFiltra.BagFilters.Application.Interfaces.Report
{
    public interface IReportService
    {
        Task<List<ReportTemplateModelDto>> GetAllTemplatesFromFolderAsync();

        Task<ReportTemplateModelDto> PrepareTemplateWithValuesAsync(
            ReportTemplateModelDto template,
            Dictionary<string, object> rowValues,
            Dictionary<string, object> headerValues);


        Task<byte[]> GeneratePdfBytesAsync(TemplateRoot header, TemplateRoot footer, List<Dictionary<string, object>> rows, byte[] companyIcon = null, ReportTemplate reportTemplate = null);
    }
}
