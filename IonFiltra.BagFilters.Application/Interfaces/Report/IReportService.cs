using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IonFiltra.BagFilters.Application.DTOs.Report;


namespace IonFiltra.BagFilters.Application.Interfaces.Report
{
    public interface IReportService
    {
        Task<List<ReportTemplateModelDto>> GetAllTemplatesFromFolderAsync();

        Task<ReportTemplateModelDto> PrepareTemplateWithValuesAsync(
            ReportTemplateModelDto template,
            Dictionary<string, object> rowValues,
            Dictionary<string, object> headerValues);


        Task<EvaluatedReportTemplateDto> EvaluateTemplateAsync(
                ReportTemplateModelDto template,
                double? processVolume, int enquiryId,
        HashSet<string> nonVolumeDependentReports, List<Dictionary<string, object>> headerDict,
    List<Dictionary<string, object>> bagfilterMasterData);
    }
}
