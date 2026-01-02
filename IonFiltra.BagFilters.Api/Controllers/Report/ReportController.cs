using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using IonFiltra.BagFilters.Api.Controllers.Assignment;
using IonFiltra.BagFilters.Application.DTOs.Report;

using IonFiltra.BagFilters.Application.Interfaces.GenericView;
using IonFiltra.BagFilters.Application.Interfaces.Report;
using IonFiltra.BagFilters.Application.Services.GenericView;
using IonFiltra.BagFilters.Application.Services.Report;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace IonFiltra.BagFilters.Api.Controllers.Report
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _pdfService;
        private readonly IGenericViewService _viewService;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<ReportController> _logger;

        public ReportController(IReportService pdfService, IGenericViewService genericViewService, IWebHostEnvironment env, ILogger<ReportController> logger)
        {
            _pdfService = pdfService;
            _viewService = genericViewService;
            _env = env;
            _logger = logger;
        }

        
        


        [HttpPost("generate-pdf")]
        public async Task<IActionResult> GenerateProjectPdf([FromBody] ProjectReportRequest request)
        {
            try
            {
                // Create service (preferably inject via DI)
                var templates = await _pdfService.GetAllTemplatesFromFolderAsync();
                // defensive: ensure not null
                templates ??= new List<ReportTemplateModelDto>();

                // order by Order (ascending). fallback to Title if multiple have same Order
                templates = templates
                    .OrderBy(t => t.Order)                  // primary: explicit order
                    .ThenBy(t => t.Title, StringComparer.OrdinalIgnoreCase) // secondary: stable deterministic
                    .ToList();

                // 2) Apply Summary / Detailed logic
                if (string.Equals(request.ReportType, "Summary", StringComparison.OrdinalIgnoreCase))
                {
                    templates = templates
                        .Where(t =>
                            !(t.Order == 2 ||
                              string.Equals(t.ReportName, "Bag Filter Details", StringComparison.OrdinalIgnoreCase)))
                        .ToList();
                }
                // Detailed => keep all templates

                var evaluatedTemplates = new List<EvaluatedReportTemplateDto>();
                var dictValues = new Dictionary<string, object>
                {
                    { "Id", request.EnquiryId },
                };

              

                // For each template load data - you said use "enquiry" as table name for now
                foreach (var template in templates)
                {
                    // YOU: fetch data for this report. For now use a sample dict:
                    /// Build parameters for this template only
                    var dataParams = new Dictionary<string, object>
                    {
                        ["EnquiryId"] = request.EnquiryId
                    };

                    // 👉 Only Bagfilter Details gets the extra Process_Volume
                    bool isBagfilterDetails =
                        template.ReportName?.Equals("Bag Filter Details", StringComparison.OrdinalIgnoreCase) == true
                        || template.Order == 2
                        || template.EntityDbName?.Equals("vw_BagfilterDetails", StringComparison.OrdinalIgnoreCase) == true;

                    if (isBagfilterDetails && request.ProcessVolumeM3h.HasValue)
                    {
                        dataParams["Process_Volume_M3h"] = request.ProcessVolumeM3h.Value;
                    }


                    var headerDict = _viewService.GetViewDataWithParam("enquiry", dictValues);
                   
                    var bagfilterMasterData = _viewService.GetViewDataWithParam("bagfiltermaster", dataParams);
                    var reportInputData = await _viewService.GetViewDataWithParam(template.EntityDbName, dataParams);


                    var headerValues = headerDict.Result.FirstOrDefault() ?? new Dictionary<string, object>();
                    
                    // Defensive conversion: always end up with a List<Dictionary<string,object>>
                    var listData = reportInputData?.ToList() ?? new List<Dictionary<string, object>>();

                    // Build row values
                    var rowValues = new Dictionary<string, object>();
                    if (listData.Count > 0)
                    {
                        foreach (var kvp in listData[0])
                            rowValues[kvp.Key] = kvp.Value;
                    }

                    //var rowValues = new Dictionary<string, object>();

                    //bool isGroupedTemplate =
                    //    template.EntityDbName.Equals("vw_BillOfMaterialDetails", StringComparison.OrdinalIgnoreCase)
                    //    || template.EntityDbName.Equals("vw_TransportationCostDetails", StringComparison.OrdinalIgnoreCase)
                    //    || template.EntityDbName.Equals("vw_PaintingCostDetails", StringComparison.OrdinalIgnoreCase);

                    //if (!isGroupedTemplate && listData.Count > 0)
                    //{
                    //    foreach (var kvp in listData[0])
                    //        rowValues[kvp.Key] = kvp.Value;
                    //}


                    var masterDataList = bagfilterMasterData?.Result.ToList() ?? new List<Dictionary<string, object>>();
                    // Merge master data
                    if (masterDataList.Count > 0)
                    {
                        foreach (var kvp in masterDataList[0])
                            rowValues[kvp.Key] = kvp.Value;
                    }

                    // for repeating rows add list_values
                    //rowValues["list_values"] = listData;

                    // SPECIAL HANDLING ONLY FOR BILL OF MATERIAL TEMPLATE
                    if (string.Equals(template.EntityDbName, "vw_BillOfMaterialDetails",
                                      StringComparison.OrdinalIgnoreCase) || string.Equals(template.Title, "Bill Of Material", StringComparison.OrdinalIgnoreCase))
                    {
                        // group BOM rows by BagfilterMasterId (and/or Process_Volume_M3h)
                        var bomGroups = listData
                            .GroupBy(d => d["Process_Volume_M3h"])
                            .Select(g =>
                            {
                                var first = g.First();

                                var groupDict = new Dictionary<string, object>();

                                // carry over keys we need on header table
                                if (first.TryGetValue("EnquiryId", out var enq)) groupDict["EnquiryId"] = enq;
                                if (first.TryGetValue("BagfilterMasterId", out var bm)) groupDict["BagfilterMasterId"] = bm;
                                if (first.TryGetValue("Process_Volume_M3h", out var pv)) groupDict["Process_Volume_M3h"] = pv;
                                if (first.TryGetValue("Qty", out var qty)) groupDict["Qty"] = qty;
                                if (first.TryGetValue("Enquiry_RequiredBagFilters", out var rb))
                                    groupDict["Enquiry_RequiredBagFilters"] = rb;
                                if (first.TryGetValue("Item", out var item)) groupDict["Item"] = item;
                                if (first.TryGetValue("Material", out var material)) groupDict["Material"] = material;
                                if (first.TryGetValue("Weight", out var weight)) groupDict["Weight"] = weight;
                                if (first.TryGetValue("Units", out var units)) groupDict["Units"] = units;
                                if (first.TryGetValue("Rate", out var rate)) groupDict["Rate"] = rate;
                                if (first.TryGetValue("Cost", out var cost)) groupDict["Cost"] = cost;

                                // put ALL rows for this group into BomRows
                                groupDict["BomRows"] = g.ToList();

                                return groupDict;
                            })
                            .ToList();

                        // this is what the group in the template will iterate
                        rowValues["bom_groups"] = bomGroups;
                    }
                    else if (
                    string.Equals(template.EntityDbName, "vw_TransportationCostDetails",
                                  StringComparison.OrdinalIgnoreCase)
                    || string.Equals(template.Title, "Transportation Cost", StringComparison.OrdinalIgnoreCase)
)
                    {
                        var transportGroups = listData
                            .GroupBy(d => d["Process_Volume_M3h"])
                            .Select(g =>
                            {
                                var first = g.First();

                                var groupDict = new Dictionary<string, object>();

                                // Header fields (MUST match template placeholders)
                                if (first.TryGetValue("EnquiryId", out var enq))
                                    groupDict["EnquiryId"] = enq;

                                if (first.TryGetValue("BagfilterMasterId", out var bm))
                                    groupDict["BagfilterMasterId"] = bm;

                                if (first.TryGetValue("Process_Volume_M3h", out var pv))
                                    groupDict["Process_Volume_M3h"] = pv;

                                if (first.TryGetValue("Qty", out var qty))
                                    groupDict["Qty"] = qty;

                                if (first.TryGetValue("Enquiry_RequiredBagFilters", out var rb))
                                    groupDict["Enquiry_RequiredBagFilters"] = rb;

                                // 🚨 THIS IS THE KEY PART
                                groupDict["TransportationRows"] = g
                                    .Select(r => new Dictionary<string, object>
                                    {
                                        ["Parameter"] = r.GetValueOrDefault("Parameter"),
                                        ["Value"] = r.GetValueOrDefault("Value"),
                                        ["Unit"] = r.GetValueOrDefault("Unit")
                                    })
                                    .ToList();

                                return groupDict;
                            })
                            .ToList();

                        rowValues["transport_groups"] = transportGroups;
                    }
                    else if (
                        string.Equals(template.EntityDbName, "vw_DamperCostDetails",
                                      StringComparison.OrdinalIgnoreCase)
                        || string.Equals(template.Title, "Damper Cost",
                                         StringComparison.OrdinalIgnoreCase))

                    {
                        var damperGroups = listData
                            .GroupBy(d => d["Process_Volume_M3h"])
                            .Select(g =>
                            {
                                var first = g.First();
                                var groupDict = new Dictionary<string, object>();

                                groupDict["EnquiryId"] = first["EnquiryId"];
                                groupDict["BagfilterMasterId"] = first["BagfilterMasterId"];
                                groupDict["Process_Volume_M3h"] = first["Process_Volume_M3h"];
                                groupDict["Qty"] = first["Qty"];
                                groupDict["Enquiry_RequiredBagFilters"] = first["Enquiry_RequiredBagFilters"];

                                groupDict["DamperRows"] = g
                                    .Select(r => new Dictionary<string, object>
                                    {
                                        ["Parameter"] = r["Parameter"],
                                        ["Value"] = r["Value"],
                                        ["Unit"] = r["Unit"]
                                    })
                                    .ToList();

                                return groupDict;
                            })
                            .ToList();

                        rowValues["damper_groups"] = damperGroups;
                    }
                    else if (
                    string.Equals(template.EntityDbName, "vw_CageCostDetails",
                                  StringComparison.OrdinalIgnoreCase)
                    || string.Equals(template.Title, "Cage Costing",
                                     StringComparison.OrdinalIgnoreCase)
)
                    {
                        var cageGroups = listData
                            .GroupBy(d => d["Process_Volume_M3h"])
                            .Select(g =>
                            {
                                var first = g.First();
                                var groupDict = new Dictionary<string, object>();

                                groupDict["EnquiryId"] = first["EnquiryId"];
                                groupDict["BagfilterMasterId"] = first["BagfilterMasterId"];
                                groupDict["Process_Volume_M3h"] = first["Process_Volume_M3h"];
                                groupDict["Qty"] = first["Qty"];
                                groupDict["Enquiry_RequiredBagFilters"] = first["Enquiry_RequiredBagFilters"];

                                groupDict["CageRows"] = g
                                    .Select(r => new Dictionary<string, object>
                                    {
                                        ["Parameter"] = r["Parameter"],
                                        ["Value"] = r["Value"],
                                        ["Unit"] = r["Unit"]
                                    })
                                    .ToList();

                                return groupDict;
                            })
                            .ToList();

                        rowValues["cage_groups"] = cageGroups;
                    }

                    else if (string.Equals(template.EntityDbName, "vw_PaintingCostDetails",
                                      StringComparison.OrdinalIgnoreCase) || string.Equals(template.Title, "Painting Cost", StringComparison.OrdinalIgnoreCase))
                    {
                       

                        var paintingGroups = new List<Dictionary<string, object>>();

                        foreach (var rec in listData)
                        {
                            var group = new Dictionary<string, object>();

                            // header fields for the small Process Volume table
                            if (rec.TryGetValue("EnquiryId", out var enq)) group["EnquiryId"] = enq;
                            if (rec.TryGetValue("BagfilterMasterId", out var bm)) group["BagfilterMasterId"] = bm;
                            if (rec.TryGetValue("Process_Volume_M3h", out var pv)) group["Process_Volume_M3h"] = pv;
                            if (rec.TryGetValue("Qty", out var qty)) group["Qty"] = qty;
                            if (rec.TryGetValue("Enquiry_RequiredBagFilters", out var rb)) group["Enquiry_RequiredBagFilters"] = rb;

                            // parse PaintingTableJson for this volume
                            var paintingJson = rec.TryGetValue("PaintingTableJson", out var ptj)
                                ? ptj?.ToString()
                                : null;

                            List<Dictionary<string, object>> paintingRows = new();

                            if (!string.IsNullOrWhiteSpace(paintingJson))
                            {
                                var list = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(paintingJson);
                                if (list != null)
                                    paintingRows = list;
                            }

                            group["PaintingRows"] = paintingRows;

                            paintingGroups.Add(group);
                        }

                        rowValues["painting_groups"] = paintingGroups;
                    }
                    else
                    {
                        // normal templates still use list_values
                        rowValues["list_values"] = listData;
                    }


                    var processed = await _pdfService.PrepareTemplateWithValuesAsync(template, rowValues, headerValues);

                    evaluatedTemplates.Add(new EvaluatedReportTemplateDto
                    {
                        ReportName = processed.ReportName,
                        ReportTitle = processed.Title,
                        Template = processed,
                        HeaderInputDict = headerValues,
                        ValuesDict = rowValues
                    });
                }

                // 🔹 Step 2: PDF Setup
                QuestPDF.Settings.License = LicenseType.Community;
                var memoryStream = new MemoryStream();
                var logoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/company.png");

                try
                {
                    // 🔹 Step 3: Create PDF
                    var document = Document.Create(container =>
                    {
                        foreach (var evaluated in evaluatedTemplates)
                        {
                            var processedTemplate = evaluated.Template;
                            var currentHeaderDict = evaluated.HeaderInputDict;

                            container.Page(page =>
                            {
                                page.Margin(20);
                                page.DefaultTextStyle(TextStyle.Default.FontSize(11).FontFamily("Arial"));

                                // 🧱 Page Header
                                page.Header().Element(header =>
                                {
                                    header.Border(1)
                                          .Background("#E5E5EA")
                                          .Padding(10)
                                          .Column(col =>
                                          {
                                              // Report Title
                                              col.Item().AlignCenter().Text(evaluated.ReportTitle ?? "")
                                                  .FontSize(14).Bold().FontColor(Colors.Black);

                                              col.Item().PaddingTop(3);
                                              col.Item().LineHorizontal(1).LineColor(Colors.Black);

                                              col.Item().PaddingTop(5).Row(row =>
                                              {
                                                  // Left Column
                                                  row.RelativeItem().Column(left =>
                                                  {
                                                      left.Item().PaddingBottom(5).Text(text =>
                                                      {
                                                          text.Span("Client Name: ").SemiBold();
                                                          text.Span(currentHeaderDict?.GetValueOrDefault("Customer")?.ToString() ?? "-");
                                                      });
                                                      left.Item().PaddingBottom(5).Text(text =>
                                                      {
                                                          text.Span("Enquiry Id: ").SemiBold();
                                                          text.Span(currentHeaderDict?.GetValueOrDefault("EnquiryId")?.ToString() ?? "-");
                                                      });
                                                      
                                                  });

                                                  // Right Column
                                                  row.RelativeItem().Column(right =>
                                                  {
                                                      right.Item().PaddingBottom(5).Text(text =>
                                                      {
                                                          text.Span("Date: ").SemiBold();
                                                          text.Span(DateTime.Today.ToString("dd-MMM-yyyy"));
                                                      });
                                    
                                                  });
                                              });
                                          });
                                });

                                // 🧾 Page Content
                                page.Content().Element(body =>
                                {
                                    body.PaddingTop(10).Column(col =>
                                    {
                                        col.Spacing(10);

                                        foreach (var row in processedTemplate.Rows)
                                        {
                                            // 1️⃣ Page break rows – used by Bagfilter Details group
                                            if (row.Type?.Equals("pagebreak", StringComparison.OrdinalIgnoreCase) == true)
                                            {
                                                // This will end the current page and start a new one
                                                col.Item().PageBreak();
                                                continue;
                                            }
                                            if (row.Type == "heading")
                                            {
                                                col.Item().Text(row.Text).FontSize(14).Bold();
                                            }
                                            else if (row.Type == "text")
                                            {
                                                col.Item().Text(row.Text);
                                            }
                                            else if (row.Type == "table" && row.Rows != null && row.LayoutColumns != null)
                                            {

                                                var imageRows = new List<string>(); // move this here
                                                var columnCount = row.LayoutColumns.Count;

                                                // 1️⃣ Table rendering (excluding base64 rows)
                                                col.Item().Table(table =>
                                                {
                                                    // Define column widths
                                                    table.ColumnsDefinition(columns =>
                                                    {
                                                        for (int i = 0; i < row.LayoutColumns.Count; i++)
                                                        {
                                                            var width = (row.ColumnWidths != null && i < row.ColumnWidths.Count && row.ColumnWidths[i] > 0)
                                                                       ? row.ColumnWidths[i]
                                                                       : 1; // fallback to width = 1 if invalid or missing

                                                            columns.RelativeColumn((float)width);
                                                        }
                                                    });

                                                    // Header row
                                                    var firstRow = row.Rows.FirstOrDefault();
                                                    if (firstRow != null)
                                                    {
                                                        table.Header(header =>
                                                        {
                                                            foreach (var headerCell in firstRow.RowData)
                                                            {
                                                                var safeText = (!string.IsNullOrWhiteSpace(headerCell) && headerCell.StartsWith("data:image"))
                                                                                ? "[Image]"
                                                                                : headerCell ?? "";
                                                                var background = ExtractBackgroundColor(firstRow.RowStyle.InlineCss);

                                                                header.Cell().Element(cell =>
                                                                    cell.Background(background ?? "#F9C043")
                                                                        .Border(1)
                                                                        .Padding(5)
                                                                        .AlignMiddle()
                                                                        .AlignCenter()
                                                                        .DefaultTextStyle(x => x.FontColor(Colors.White).SemiBold())
                                                                        //.Text(safeText)
                                                                        .Text(text =>
                                                                        {
                                                                            foreach (var part in ParseHtmlWithSubSup(safeText))
                                                                            {
                                                                                if (part.IsLineBreak)
                                                                                {
                                                                                    text.Line("");
                                                                                    continue;
                                                                                }

                                                                                var span = text.Span(part.Text);

                                                                                if (part.IsSub) span.Subscript();
                                                                                if (part.IsSup) span.Superscript();
                                                                                if (part.IsBold) span.Bold();
                                                                                if (part.IsItalic) span.Italic();

                                                                                if (!part.IsSub && !part.IsSup)
                                                                                    span.FontSize(10);
                                                                            }
                                                                        })
                                                                );
                                                            }
                                                        });
                                                    }

                                                    // Table body rows
                                                    foreach (var tableRow in row.Rows.Skip(1))
                                                    {
                                                        var base64Cell = tableRow.RowData.FirstOrDefault(cell =>
                                                            !string.IsNullOrWhiteSpace(cell) &&
                                                            cell.StartsWith("data:image", StringComparison.OrdinalIgnoreCase));

                                                        if (base64Cell != null)
                                                        {
                                                            imageRows.Add(base64Cell); // collect for later
                                                            continue;
                                                        }

                                                        //heading like data rows
                                                        if (tableRow.RowData.Count(cell => !string.IsNullOrWhiteSpace(cell)) == 1)
                                                        {
                                                            // This is a heading-like row: span across all columns
                                                            table.Cell().ColumnSpan((uint)row.LayoutColumns.Count).Element(cell =>
                                                            {
                                                                var rowStyle = tableRow.RowStyle; // fallback
                                                                var bgColor = ExtractBackgroundColor(rowStyle.InlineCss);
                                                                var textColor = ExtractTextColor(rowStyle.InlineCss);
                                                                var isBolder = ExtractFontWeightBold(rowStyle.InlineCss);


                                                                var defaultTextStyle = TextStyle.Default.FontSize(10).FontColor(textColor);

                                                                if (rowStyle.Bold || isBolder)
                                                                    defaultTextStyle = defaultTextStyle.Bold();

                                                                cell.Background(bgColor)
                                                                    .Border(1)
                                                                    .Padding(5)
                                                                    .AlignTop()
                                                                    .DefaultTextStyle(defaultTextStyle)
                                                                    .Text(text =>
                                                                    {
                                                                        foreach (var part in ParseHtmlWithSubSup(tableRow.RowData[0] ?? ""))
                                                                        {
                                                                            if (part.IsLineBreak)
                                                                            {
                                                                                text.Line("");
                                                                                continue;
                                                                            }

                                                                            var span = text.Span(part.Text);

                                                                            if (part.IsSub)
                                                                                span.Subscript();
                                                                            if (part.IsSup)
                                                                                span.Superscript();
                                                                            if (part.IsBold)
                                                                                span.Bold();
                                                                            if (part.IsItalic)
                                                                                span.Italic();

                                                                            if (!part.IsSub && !part.IsSup)
                                                                                span.FontSize(10);
                                                                        }
                                                                    });
                                                            });
                                                        }
                                                        else
                                                        { //normal data rows
                                                            foreach (var cellValue in tableRow.RowData)
                                                            {
                                                                table.Cell().Element(cell =>
                                                                {
                                                                    var rowStyle = tableRow.RowStyle; // fallback
                                                                    var bgColor = ExtractBackgroundColor(rowStyle.InlineCss);
                                                                    var textColor = ExtractTextColor(rowStyle.InlineCss);
                                                                    var isBolder = ExtractFontWeightBold(rowStyle.InlineCss);
                                                                  

                                                                    var defaultTextStyle = TextStyle.Default.FontSize(10).FontColor(textColor);

                                                                    if (rowStyle.Bold || isBolder)
                                                                        defaultTextStyle = defaultTextStyle.Bold();

                                                                    

                                                                    cell.Background(bgColor)
                                                                        .Border(1)
                                                                        .Padding(5)
                                                                        .AlignTop()
                                                                        .DefaultTextStyle(defaultTextStyle)
                                                                        .Text(text =>
                                                                        {
                                                                            foreach (var part in ParseHtmlWithSubSup(cellValue ?? ""))
                                                                            {
                                                                                if (part.IsLineBreak)
                                                                                {
                                                                                    text.Line("");
                                                                                    continue;
                                                                                }

                                                                                var span = text.Span(part.Text);

                                                                                if (part.IsSub)
                                                                                    span.Subscript();
                                                                                if (part.IsSup)
                                                                                    span.Superscript();
                                                                                if (part.IsBold)
                                                                                    span.Bold();
                                                                                if (part.IsItalic)
                                                                                    span.Italic();

                                                                                if (!part.IsSub && !part.IsSup)
                                                                                    span.FontSize(10);
                                                                            }
                                                                        });
                                                                });
                                                            }

                                                        }

                                                    }
                                                });

                                                // 2️⃣ Render base64 images on new page — OUTSIDE Table()
                                                foreach (var base64 in imageRows)
                                                {

                                                    col.Item().Element(imageContainer =>
                                                    {
                                                        try
                                                        {
                                                            var base64Data = base64.Substring(base64.IndexOf(",") + 1);
                                                            var imageBytes = Convert.FromBase64String(base64Data);

                                                            imageContainer
                                                                .Padding(10)
                                                                .Border(1)
                                                                .AlignCenter()
                                                                .Image(imageBytes)
                                                                .FitWidth()
                                                                .WithCompressionQuality(ImageCompressionQuality.Best); // Optional
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            _logger.LogWarning(ex, "Failed to render standalone base64 image");
                                                            imageContainer.Text("[Image Error]").FontSize(10).WrapAnywhere();
                                                        }
                                                    });
                                                }

                                            }

                                        }
                                    });
                                });

                                // 📄 Footer with logo and page number
                                page.Footer().Element(footer =>
                                {
                                    footer.PaddingTop(20).Row(row =>
                                    {
                                        row.ConstantItem(100).AlignLeft().AlignMiddle().Image(logoPath, ImageScaling.FitWidth);
                                        row.RelativeItem().AlignRight().AlignMiddle().Text(text =>
                                        {
                                            text.Span("Page ");
                                            text.CurrentPageNumber();
                                        });
                                    });
                                });
                            });
                        }
                    });

                    // 🔄 Generate and return PDF
                    document.GeneratePdf(memoryStream);
                    memoryStream.Position = 0;

                    return File(memoryStream.ToArray(), "application/pdf", "ProjectReport.pdf");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "PDF generation failed at rendering time");
                    return StatusCode(500, "PDF generation failed: " + ex.Message);
                }

               
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to generate PDF");
                return StatusCode(500, "Failed to generate PDF: " + ex.Message);
            }
        }



        public class ProjectReportRequest
        {
            public int EnquiryId { get; set; }

            // "Detailed" or "Summary"
            public string ReportType { get; set; } = "Detailed";

            // Optional process volume in m³/h
            public double? ProcessVolumeM3h { get; set; }
        }

        public class EvaluatedReportTemplateDto
        {
            public string? ReportName { get; set; }
            public string? ReportTitle { get; set; }
            public string? EntityDbName { get; set; }

            public ReportTemplateModelDto Template { get; set; } = default!;
            public Dictionary<string, object>? HeaderInputDict { get; set; } // ✅ Add this
            public Dictionary<string, object>? ValuesDict { get; set; } // ✅ Add this

        }

        public class TextPart
        {
            public string Text { get; set; } = "";
            public bool IsSub { get; set; }
            public bool IsSup { get; set; }
            public bool IsLineBreak { get; set; }
            public bool IsBold { get; set; }
            public bool IsItalic { get; set; }
        }

        public static string ExtractBackgroundColor(string? inlineCss)
        {
            if (string.IsNullOrEmpty(inlineCss))
                return Colors.White; // fallback

            var match = Regex.Match(inlineCss, @"background-color\s*:\s*(#[0-9A-Fa-f]{6})");
            return match.Success ? match.Groups[1].Value : Colors.White;
        }

        public static string ExtractTextColor(string? inlineCss)
        {
            if (string.IsNullOrWhiteSpace(inlineCss))
                return Colors.Black;

            // Split into declarations, e.g. ["background-color:#92D050", " font-weight:bold", " color: red"]
            var parts = inlineCss.Split(';', StringSplitOptions.RemoveEmptyEntries);

            foreach (var part in parts)
            {
                var kv = part.Split(':', 2, StringSplitOptions.RemoveEmptyEntries);
                if (kv.Length != 2) continue;

                var key = kv[0].Trim().ToLowerInvariant();
                var value = kv[1].Trim();

                // only match exact "color" key (not background-color etc.)
                if (key != "color") continue;

                // normalize value (strip trailing ; if any, trim)
                value = value.Trim().TrimEnd(';').Trim();

                // If it's a hex color (with or without #), normalize to #RRGGBB
                var hexMatch = Regex.Match(value, @"^#?([0-9A-Fa-f]{3}|[0-9A-Fa-f]{6})$");
                if (hexMatch.Success)
                {
                    var hex = hexMatch.Groups[1].Value;
                    if (hex.Length == 3)
                    {
                        // expand short hex e.g. "FAB" -> "FFAABB"
                        var r = hex[0].ToString();
                        var g = hex[1].ToString();
                        var b = hex[2].ToString();
                        return $"#{r}{r}{g}{g}{b}{b}";
                    }
                    return $"#{hex}";
                }

                // Named colors mapping - extend as needed
                var name = value.ToLowerInvariant();
                return name switch
                {
                    "red" => Colors.Red.Medium,
                    "blue" => Colors.Blue.Medium,
                    "green" => Colors.Green.Medium,
                    "yellow" => Colors.Yellow.Medium,
                    "orange" => Colors.Orange.Medium,
                    "gray" or "grey" => Colors.Grey.Medium,
                    "black" => Colors.Black,
                    "white" => Colors.White,
                    "transparent" => Colors.Transparent,
                    _ => Colors.Black // fallback for unknown names
                };
            }

            // no explicit color declaration found -> fallback
            return Colors.Black;
        }



        public static bool ExtractFontWeightBold(string? inlineCss)
        {
            if (string.IsNullOrWhiteSpace(inlineCss))
                return false;

            // Matches:
            // - font-weight: bold;
            // - font-weight: bolder;
            // - font-weight: 600;
            // - font-weight: 700;
            // Ignores case and whitespace
            var match = Regex.Match(inlineCss, @"font-weight\s*:\s*(bold(er)?|[6-9]00)", RegexOptions.IgnoreCase);
            return match.Success;
        }

        public static (float Top, float Right, float Bottom, float Left) ExtractPadding(string? inlineCss)
        {
            if (string.IsNullOrWhiteSpace(inlineCss))
                return (0, 0, 0, 0);

            // Match common patterns like:
            // padding: 5px;
            // padding: 5px 10px;
            // padding: 5px 10px 15px;
            // padding: 5px 10px 15px 20px;
            var match = Regex.Match(inlineCss, @"padding\s*:\s*([\d\.]+)px(?:\s+([\d\.]+)px)?(?:\s+([\d\.]+)px)?(?:\s+([\d\.]+)px)?", RegexOptions.IgnoreCase);
            if (match.Success)
            {
                float p1 = float.TryParse(match.Groups[1].Value, out var v1) ? v1 : 0;
                float p2 = float.TryParse(match.Groups[2].Value, out var v2) ? v2 : p1;
                float p3 = float.TryParse(match.Groups[3].Value, out var v3) ? v3 : p1;
                float p4 = float.TryParse(match.Groups[4].Value, out var v4) ? v4 : p2;

                // CSS padding shorthand interpretation:
                // 1 value: all sides same
                // 2 values: top/bottom = first, left/right = second
                // 3 values: top, left/right, bottom
                // 4 values: top, right, bottom, left
                switch (match.Groups.Cast<Group>().Count(g => g.Success))
                {
                    case 2: return (p1, p1, p1, p1);
                    case 3: return (p1, p2, p1, p2);
                    case 4: return (p1, p2, p3, p2);
                    default: return (p1, p2, p3, p4);
                }
            }

            // Match individual side paddings: padding-top / padding-bottom / padding-left / padding-right
            float top = ExtractSidePadding(inlineCss, "top");
            float right = ExtractSidePadding(inlineCss, "right");
            float bottom = ExtractSidePadding(inlineCss, "bottom");
            float left = ExtractSidePadding(inlineCss, "left");

            return (top, right, bottom, left);
        }

        private static float ExtractSidePadding(string inlineCss, string side)
        {
            var match = Regex.Match(inlineCss, $@"padding-{side}\s*:\s*([\d\.]+)px", RegexOptions.IgnoreCase);
            return match.Success && float.TryParse(match.Groups[1].Value, out var v) ? v : 0;
        }



        public static List<TextPart> ParseHtmlWithSubSup(string input)
        {
            var parts = new List<TextPart>();

            if (string.IsNullOrWhiteSpace(input))
                return parts;

            // Remove <span> tags
            input = Regex.Replace(input, @"<\/?span[^>]*>", "", RegexOptions.IgnoreCase);

            // Fix malformed sub/sup tags (e.g., <sup>...</sub>)
            input = Regex.Replace(input, @"<sup>([^<]+)<\/sub>", "<sup>$1</sup>", RegexOptions.IgnoreCase);
            input = Regex.Replace(input, @"<sub>([^<]+)<\/sup>", "<sub>$1</sub>", RegexOptions.IgnoreCase);

            // Replace <br> with placeholder
            input = Regex.Replace(input, @"<br\s*\/?>", "[[BR]]", RegexOptions.IgnoreCase);

            // Replace supported tags with placeholders
            input = input.Replace("<b>", "[[B]]").Replace("</b>", "[[/B]]")
                         .Replace("<i>", "[[I]]").Replace("</i>", "[[/I]]")
                         .Replace("<sub>", "[[SUB]]").Replace("</sub>", "[[/SUB]]")
                         .Replace("<sup>", "[[SUP]]").Replace("</sup>", "[[/SUP]]");

            var tokens = Regex.Split(input, @"(\[\[/?[A-Z]+\]\])");
            var tagStack = new Stack<string>();
            var buffer = new StringBuilder();

            void FlushBuffer()
            {
                if (buffer.Length == 0)
                    return;

                parts.Add(new TextPart
                {
                    Text = buffer.ToString(),
                    IsBold = tagStack.Contains("B"),
                    IsItalic = tagStack.Contains("I"),
                    IsSub = tagStack.Contains("SUB"),
                    IsSup = tagStack.Contains("SUP")
                });

                buffer.Clear();
            }

            foreach (var token in tokens)
            {
                if (string.IsNullOrWhiteSpace(token))
                    continue;

                if (token == "[[BR]]")
                {
                    FlushBuffer();
                    parts.Add(new TextPart { IsLineBreak = true });
                }
                else if (Regex.IsMatch(token, @"\[\[([A-Z]+)\]\]")) // Opening tag
                {
                    FlushBuffer();
                    tagStack.Push(token.Trim('[', ']'));
                }
                else if (Regex.IsMatch(token, @"\[\[/([A-Z]+)\]\]")) // Closing tag
                {
                    FlushBuffer();
                    var closingTag = token.Trim('[', ']', '/');
                    if (tagStack.Contains(closingTag))
                    {
                        var tempStack = new Stack<string>();
                        while (tagStack.Count > 0)
                        {
                            var popped = tagStack.Pop();
                            if (popped == closingTag) break;
                            tempStack.Push(popped);
                        }
                        while (tempStack.Count > 0)
                            tagStack.Push(tempStack.Pop());
                    }
                }
                else
                {
                    buffer.Append(token);
                }
            }

            FlushBuffer();
            return parts;
        }


    }
}
