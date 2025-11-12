using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using IonFiltra.BagFilters.Application.Models.Report;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace IonFiltra.BagFilters.Application.Heplers.Report
{
    public static class ReportTemplateRenderer
    {
        private static string SafeParseColor(string colorValue, string defaultColor)
        {
            if (string.IsNullOrWhiteSpace(colorValue))
                return defaultColor;

            colorValue = colorValue.Trim();

            // ✅ QuestPDF older versions expect color strings (not Color structs)
            // So we just return the validated hex string
            if (colorValue.StartsWith("#") &&
                (colorValue.Length == 7 || colorValue.Length == 9)) // #RRGGBB or #AARRGGBB
                return colorValue;

            // handle named colors like "red", "grey", etc. by mapping to QuestPDF built-ins
            return colorValue.ToLowerInvariant() switch
            {
                "red" => Colors.Red.Medium,
                "blue" => Colors.Blue.Medium,
                "green" => Colors.Green.Medium,
                "grey" or "gray" => Colors.Grey.Lighten3,
                "white" => Colors.White,
                "black" => Colors.Black,
                _ => defaultColor
            };
        }

        public static ReportTemplate LoadTemplate(string filePath)
        {
            var json = File.ReadAllText(filePath);
            var opts = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReadCommentHandling = JsonCommentHandling.Skip,
                AllowTrailingCommas = true
            };
            return JsonSerializer.Deserialize<ReportTemplate>(json, opts);
        }

        public static void RenderReportBody(IContainer container, ReportTemplate template, List<Dictionary<string, object>> dataRows)
        {
            var padding = ConvertToInt(template?.Layout?.GetValueOrDefault("padding"), 12);
            container.Padding(padding).Column(col =>
            {
                if (template?.Sections == null) return;

                foreach (var section in template.Sections)
                {
                    switch ((section.Type ?? "").ToLowerInvariant())
                    {
                        case "table":
                            col.Item().Element(c => RenderTableSection(c, section, dataRows));
                            break;
                        default:
                            if (!string.IsNullOrEmpty(section.Title))
                                col.Item().Text(section.Title).FontSize(12).SemiBold();
                            break;
                    }
                }
            });
        }

        private static void RenderTableSection(IContainer container, ReportSection section, List<Dictionary<string, object>> dataRows)
        {
            var columns = section.Columns ?? new List<ReportColumn>();
            var style = section.Style ?? new Dictionary<string, object>();
            int headerFontSize = ConvertToInt(style.GetValueOrDefault("fontSizeHeader"), 10);
            int cellFontSize = ConvertToInt(style.GetValueOrDefault("fontSizeCell"), 9);
            var headerBg = style.GetValueOrDefault("headerBackground")?.ToString();
            bool rowStriping = string.Equals(style.GetValueOrDefault("rowStriping")?.ToString(), "true", StringComparison.OrdinalIgnoreCase);

            container.Column(col =>
            {
                if (!string.IsNullOrEmpty(section.Title))
                    col.Item().PaddingBottom(6).Text(section.Title).FontSize(headerFontSize + 2).SemiBold();


                col.Item().Table(table =>
                {
                    // define columns
                    table.ColumnsDefinition(cd =>
                    {
                        foreach (var colDef in columns)
                        {
                            if (!string.IsNullOrWhiteSpace(colDef.Width) && colDef.Width.EndsWith("%")
                                && int.TryParse(colDef.Width.TrimEnd('%'), out var p) && p > 0)
                            {
                                cd.RelativeColumn(p);
                            }
                            else if (int.TryParse(colDef.Width, out var n) && n > 0)
                            {
                                cd.RelativeColumn(n);
                            }
                            else
                            {
                                cd.RelativeColumn();
                            }
                        }
                    });

                    
                    // Header row - chain directly (no local 'cell' variable)
                    table.Header(h =>
                    {
                        foreach (var colDef in columns)
                        {
                            // chain calls directly to avoid accidental reuse of a container
                            var headerText = colDef.Header ?? TrimBraces(colDef.Field);
                            if (!string.IsNullOrWhiteSpace(headerBg))
                            {
                                // use SafeParseColor which returns a string or known Colors.* token
                                h.Cell()
                                 .Padding(6)
                                 .Background(SafeParseColor(headerBg, Colors.Grey.Lighten3))
                                 .Text(headerText)
                                 .FontSize(headerFontSize)
                                 .SemiBold();
                            }
                            else
                            {
                                h.Cell()
                                 .Padding(6)
                                 .Text(headerText)
                                 .FontSize(headerFontSize)
                                 .SemiBold();
                            }
                        }
                    });


                    
                    // body rows
                    for (int r = 0; r < (dataRows?.Count ?? 0); r++)
                    {
                        var row = dataRows[r];
                        bool even = r % 2 == 0;

                        foreach (var colDef in columns)
                        {
                            // create locals to avoid closure issues inside lambdas
                            var fieldName = TrimBraces(colDef.Field);
                            var align = (colDef.Align ?? "").ToLowerInvariant();
                            var cellValue = string.Empty;

                            if (string.Equals(fieldName, "__serial", StringComparison.OrdinalIgnoreCase))
                            {
                                cellValue = (r + 1).ToString();
                            }
                            else if (TryGetRowValue(row, fieldName, out var val))
                            {
                                cellValue = val?.ToString() ?? string.Empty;
                            }

                            // chain cell creation directly and use Element with local copies
                            var cellContainer = table.Cell().Padding(6);

                            // optional striped background
                            if (rowStriping && !even)
                                cellContainer = cellContainer.Background(Colors.Grey.Lighten4);

                            // draw text element inside the cell
                            cellContainer.Element(c =>
                            {
                                var txt = c.Text(cellValue).FontSize(cellFontSize);
                                if (align == "right")
                                    txt.AlignRight();
                                else if (align == "center")
                                    txt.AlignCenter();
                                txt.WrapAnywhere();
                            });

                        }
                    }

                });
            });
        }

        // strip {{ and }} if present
        private static string TrimBraces(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return s;
            var t = s.Trim();
            if (t.StartsWith("{{") && t.EndsWith("}}"))
                return t.Substring(2, t.Length - 4).Trim();
            if (t.StartsWith("{") && t.EndsWith("}"))
                return t.Substring(1, t.Length - 2).Trim();
            return t;
        }

        private static bool TryGetRowValue(Dictionary<string, object> row, string key, out object value)
        {
            value = null;
            if (row == null || string.IsNullOrWhiteSpace(key)) return false;
            if (row.TryGetValue(key, out value)) return true;
            foreach (var kv in row)
                if (string.Equals(kv.Key, key, StringComparison.OrdinalIgnoreCase))
                {
                    value = kv.Value;
                    return true;
                }
            return false;
        }

        private static int ConvertToInt(object o, int defaultValue)
        {
            if (o == null) return defaultValue;
            if (o is int i) return i;
            return int.TryParse(o.ToString(), out var v) ? v : defaultValue;
        }
    }

    internal static class DictExtensions
    {
        public static object GetValueOrDefault(this Dictionary<string, object> dict, string key)
        {
            if (dict == null) return null;
            return dict.TryGetValue(key, out var v) ? v : null;
        }
    }


}
