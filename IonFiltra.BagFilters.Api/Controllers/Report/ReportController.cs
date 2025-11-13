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

                var evaluatedTemplates = new List<EvaluatedReportTemplateDto>();
                var dictValues = new Dictionary<string, object>
                {
                    { "Id", request.EnquiryId },
                };
                // For each template load data - you said use "enquiry" as table name for now
                foreach (var template in templates)
                {
                    // YOU: fetch data for this report. For now use a sample dict:

                    var headerDict = _viewService.GetViewDataWithParam("enquiry", dictValues);
                    var bagfilterMasterData = _viewService.GetViewDataWithParam("bagfiltermaster", new Dictionary<string, object> { ["EnquiryId"] = request.EnquiryId });
                    var reportInputData = await _viewService.GetViewDataWithParam(template.EntityDbName, new Dictionary<string, object> { ["EnquiryId"] = request.EnquiryId });
                    var headerValues = headerDict.Result.FirstOrDefault() ?? new Dictionary<string, object>();
                    //var rowInputDict = reportInputData.FirstOrDefault() ?? new Dictionary<string, object>();
                    // Defensive conversion: always end up with a List<Dictionary<string,object>>
                    var listData = reportInputData?.ToList() ?? new List<Dictionary<string, object>>();

                    // Build row values
                    var rowValues = new Dictionary<string, object>();
                    if (listData.Count > 0)
                    {
                        foreach (var kvp in listData[0])
                            rowValues[kvp.Key] = kvp.Value;
                    }

                    var masterDataList = bagfilterMasterData?.Result.ToList() ?? new List<Dictionary<string, object>>();
                    // Merge master data
                    if (masterDataList.Count > 0)
                    {
                        foreach (var kvp in masterDataList[0])
                            rowValues[kvp.Key] = kvp.Value;
                    }

                    // for repeating rows add list_values
                    rowValues["list_values"] = listData;

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
                                            if (row.Type == "heading")
                                            {
                                                col.Item().Text(row.Text).FontSize(16).Bold();
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
                                                                        .Text(safeText)
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
