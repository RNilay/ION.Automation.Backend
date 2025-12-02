using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using IonFiltra.BagFilters.Application.DTOs.Report;

using IonFiltra.BagFilters.Application.Interfaces.Report;

using Microsoft.Extensions.Logging;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace IonFiltra.BagFilters.Application.Services.Report
{
    public class ReportService : IReportService
    {

        private readonly string _templatesFolder;
        private readonly ILogger<ReportService> _logger;
        private static readonly Regex PlaceholderRegex = new(@"\{.*?\}", RegexOptions.Compiled);

        public ReportService(ILogger<ReportService> logger)
        {
            // Templates folder relative to content root
            _templatesFolder = Path.Combine(Directory.GetCurrentDirectory(), "Templates");
            _logger = logger;
        }



        /// <summary>
        /// Loads all json templates from the Templates folder and deserializes them.
        /// </summary>
        public async Task<List<ReportTemplateModelDto>> GetAllTemplatesFromFolderAsync()
        {
            if (!Directory.Exists(_templatesFolder))
                Directory.CreateDirectory(_templatesFolder);

            var files = Directory.GetFiles(_templatesFolder, "*.json", SearchOption.TopDirectoryOnly);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReadCommentHandling = JsonCommentHandling.Skip,
                AllowTrailingCommas = true
            };

            var list = new List<ReportTemplateModelDto>();

            foreach (var file in files)
            {
                try
                {
                    var json = await File.ReadAllTextAsync(file, Encoding.UTF8);
                    var dto = JsonSerializer.Deserialize<ReportTemplateModelDto>(json, options);
                    if (dto != null) list.Add(dto);
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex, "Failed to read template file {File}", file);
                }
            }

            return list;
        }


        private ReportRowStyle CloneRowStyle(ReportRowStyle? src)
        {
            if (src == null) return new ReportRowStyle();

            return new ReportRowStyle
            {
                FontFamily = src.FontFamily,
                FontSize = src.FontSize,
                Color = src.Color,
                Bold = src.Bold,
                Italic = src.Italic,
                Alignment = src.Alignment,
                InlineCss = src.InlineCss ?? ""
            };
        }

        static string FormatValue(object? val)
        {
            if (val == null) return "N/A";
            return val switch
            {
                double d => d.ToString("0.##"),
                float f => f.ToString("0.##"),
                decimal m => m.ToString("0.##"),
                int i => i.ToString(),
                long l => l.ToString(),
                bool b => b ? "True" : "False",
                _ => val.ToString() ?? "N/A"
            };
        }

        private Task ExpandRepeatRowsAsync(ReportTemplateModelDto reportTemplate, Dictionary<string, object> inputValues)
        {
            if (reportTemplate.Rows == null) return Task.CompletedTask;

            // local helpers (kept mostly as your original implementations)
           

            static string EvaluatePlaceholdersSync(string templateText, Dictionary<string, object> item)
            {
                if (string.IsNullOrWhiteSpace(templateText)) return templateText ?? "";
                var text = templateText;
                var matches = Regex.Matches(text, @"\{(.*?)\}");
                foreach (Match m in matches)
                {
                    var inner = m.Groups[1].Value;
                    if (item.TryGetValue(inner, out var val) && val != null)
                        text = text.Replace(m.Value, FormatValue(val));
                    else
                        text = text.Replace(m.Value, "N/A");
                }
                return text;
            }

            // Deep-clone helper (simple JSON round-trip). Use your own clone if you have one.
            T DeepClone<T>(T src)
            {
                var json = JsonSerializer.Serialize(src);
                return JsonSerializer.Deserialize<T>(json)!;
            }

            // Helper to expand a single table row template (no RepeatRow) using a single item dict
            List<TableRow> ExpandTableTemplateForItem(ReportRow tableRowTemplate, Dictionary<string, object> item)
            {
                var outRows = new List<TableRow>();

                // For each TableRow inside the table template
                foreach (var tr in tableRowTemplate.Rows ?? new List<TableRow>())
                {
                    // If tr is a BLOCK template
                    if (tr?.RowData != null && tr.RowData.Count > 0 && tr.RowData[0] == "__BLOCK__" && tr.BlockRows != null)
                    {
                        // Expand block rows into TableRow entries using the single item
                        foreach (var block in tr.BlockRows)
                        {
                            if (block == null) continue;

                            // Header-like block
                            if (block.IsHeader)
                            {
                                var headerStyle = CloneRowStyle(tr.RowStyle);
                                if (!string.IsNullOrWhiteSpace(block.InlineCss))
                                    headerStyle.InlineCss = block.InlineCss;

                                var headerRow = new TableRow
                                {
                                    RowStyle = headerStyle,
                                    RowData = new List<string> { block.Label ?? "", "" },
                                    ColumnSpan = Math.Max(1, block.ColSpan)
                                };

                                outRows.Add(headerRow);
                            }
                            else if (block.IsSpacer)
                            {
                                var spacerStyle = CloneRowStyle(tr.RowStyle) ?? new ReportRowStyle();
                                if (!string.IsNullOrWhiteSpace(block.InlineCss))
                                    spacerStyle.InlineCss = block.InlineCss;

                                outRows.Add(new TableRow
                                {
                                    RowStyle = spacerStyle,
                                    RowData = new List<string> { "", "" },   // two columns => blank row
                                    ColumnSpan = Math.Max(1, block.ColSpan)
                                });

                                continue;
                            }
                            else
                            {
                                // normal label/value
                                var valueStr = "N/A";
                                if (!string.IsNullOrEmpty(block.ValueKey) && item.TryGetValue(block.ValueKey, out var v) && v != null)
                                    valueStr = FormatValue(v);

                                var rowStyle = CloneRowStyle(tr.RowStyle);

                                if (block.InlineCss == "{#CALC}")
                                    rowStyle.InlineCss = "background-color:#C0C0C0;";
                                else if (block.InlineCss == "{#USER}")
                                    rowStyle.InlineCss = "background-color:#FFFFFF;";
                                else if (!string.IsNullOrWhiteSpace(block.InlineCss))
                                    rowStyle.InlineCss = block.InlineCss;

                                var r = new TableRow
                                {
                                    RowStyle = rowStyle,
                                    RowData = new List<string> { block.Label ?? "", valueStr }
                                };

                                outRows.Add(r);


                                // ✅ Only for Bagfilter Details and only after Hopper Height row
                                if (reportTemplate.ReportName == "Bagfilter Details" &&
                                    string.Equals(block.ValueKey, "Hopper_Height", StringComparison.OrdinalIgnoreCase))
                                {
                                    // Peek Hopper_Type from the current record
                                    string? hopperType = null;
                                    if (item.TryGetValue("Hopper_Type", out var hopperTypeObj) && hopperTypeObj != null)
                                        hopperType = hopperTypeObj.ToString();

                                    // Build extra rows with images based on Hopper_Type
                                    var imageRows = BuildHopperImageRows(hopperType);
                                    if (imageRows.Count > 0)
                                    {
                                        outRows.AddRange(imageRows);
                                    }
                                }

                            }
                        }

                        // spacer after block
                        outRows.Add(new TableRow
                        {
                            RowStyle = new ReportRowStyle(),
                            RowData = new List<string> { "", "" }
                        });
                    }
                    else
                    {
                        // Normal static row — replace placeholders inside each cell using the item
                        var newRow = new TableRow
                        {
                            RowStyle = CloneRowStyle(tr?.RowStyle ?? new ReportRowStyle()),
                            RowData = new List<string>()
                        };

                        foreach (var cell in tr?.RowData ?? new List<string>())
                        {
                            newRow.RowData.Add(EvaluatePlaceholdersSync(cell ?? "", item));
                        }

                        outRows.Add(newRow);
                    }
                }

                return outRows;
            }

            // Main logic: build a new Rows list to replace reportTemplate.Rows at the end.
            var newRowsList = new List<ReportRow>();

            // Iterate original rows and either copy them, expand tables with RepeatRow, or expand groups.
            foreach (var row in reportTemplate.Rows)
            {
                if (row == null)
                    continue;

                //// ------ CASE: group row (new) ------
                //if (row.Type?.Equals("group", StringComparison.OrdinalIgnoreCase) == true && row.RepeatRow != null)
                //{
                //    // get the source list for the group
                //    if (!inputValues.TryGetValue(row.RepeatRow.SourceKey, out var srcObj) || srcObj == null)
                //        continue;

                //    var sourceList = ConvertToListOfDicts(srcObj);
                //    if (sourceList == null || sourceList.Count == 0)
                //        continue;

                //    // For each record in the source list, render the group's ChildRows (in order)
                //    for (int idx = 0; idx < sourceList.Count; idx++)
                //    {
                //        var record = sourceList[idx];
                //        // child rows are ReportRow-like (tables typically)
                //        foreach (var child in row.ChildRows ?? new List<ReportRow>())
                //        {
                //            // clone child so we don't mutate the template
                //            var clonedChild = DeepClone(child);

                //            // If child is a table with its own RepeatRow (rare for group usage), you may decide how to handle.
                //            // For our group pattern, child tables are plain templates (no RepeatRow). We'll expand placeholders / blocks using the single record.
                //            if (clonedChild.Type?.Equals("table", StringComparison.OrdinalIgnoreCase) == true)
                //            {
                //                // Create a new ReportRow to hold the expanded table result
                //                var expandedTable = DeepClone(clonedChild); // clone structure
                //                expandedTable.Rows = ExpandTableTemplateForItem(clonedChild, record);
                //                newRowsList.Add(expandedTable);
                //            }
                //            else
                //            {
                //                // For non-table child types, do a simple placeholders pass on any string fields you expect (Title, Text etc.)
                //                var cloned = DeepClone(clonedChild);
                //                // Example: if child has Header/TextRows or Footer rows, you'd evaluate placeholders similarly.
                //                newRowsList.Add(cloned);
                //            }
                //        }

                //        // 🔹 add pagebreak only between records, not after the last one
                //        if (idx < sourceList.Count - 1)
                //        {
                //            // and **optionally** only for this specific template:
                //            // if (reportTemplate.ReportName == "Bagfilter Details")
                //            newRowsList.Add(new ReportRow { Type = "pagebreak" });
                //        }

                //        //// Insert optional pagebreak separator between records.
                //        //// The renderer must know how to treat Type == "pagebreak". If you already support this, uncomment.
                //        //var pageBreakRow = new ReportRow { Type = "pagebreak" };
                //        //newRowsList.Add(pageBreakRow);
                //    }

                //    // Done with group — continue to next original row (we replaced the group with expanded children)
                //    continue;
                //}

                // ------ CASE: group row (new) ------
                if (row.Type?.Equals("group", StringComparison.OrdinalIgnoreCase) == true
                    && row.RepeatRow != null)
                {
                    // get the source list for the group
                    if (!inputValues.TryGetValue(row.RepeatRow.SourceKey, out var srcObj) || srcObj == null)
                        continue;

                    var sourceList = ConvertToListOfDicts(srcObj);
                    if (sourceList == null || sourceList.Count == 0)
                        continue;

                    bool isBagfilterDetails =
                        reportTemplate.ReportName?.Equals("Bag Filter Details", StringComparison.OrdinalIgnoreCase) == true;

                    bool isBillOfMaterial =
                        reportTemplate.ReportName?.Equals("Bill Of Material", StringComparison.OrdinalIgnoreCase) == true;

                    bool isPaintingCost =
                        reportTemplate.ReportName?.Equals("Painting Cost", StringComparison.OrdinalIgnoreCase) == true;


                    // For each record in the source list, render the group's ChildRows (in order)
                    for (int idx = 0; idx < sourceList.Count; idx++)
                    {
                        var record = sourceList[idx];

                        foreach (var child in row.ChildRows ?? new List<ReportRow>())
                        {
                            var clonedChild = DeepClone(child);

                            // 1) TABLE child rows
                            if (clonedChild.Type?.Equals("table", StringComparison.OrdinalIgnoreCase) == true)
                            {
                                // --- 1a) Bag Filter Details: big __BLOCK__ table should IGNORE its RepeatRow ---
                                bool isBagfilterBlockTable =
                                    isBagfilterDetails &&
                                    (clonedChild.RepeatRow == null ||
                                     string.Equals(clonedChild.RepeatRow.SourceKey, "list_values", StringComparison.OrdinalIgnoreCase));

                                if (clonedChild.RepeatRow == null || isBagfilterBlockTable)
                                {
                                    // Old behaviour: expand using single record + __BLOCK__
                                    var expandedTable = DeepClone(clonedChild);
                                    expandedTable.Rows = ExpandTableTemplateForItem(clonedChild, record);
                                    newRowsList.Add(expandedTable);
                                }
                                // --- 1b) Bill Of Material: nested RepeatRow using record["BomRows"] ---
                                else if (isBillOfMaterial
                                         && clonedChild.RepeatRow != null
                                         && string.Equals(clonedChild.RepeatRow.SourceKey, "BomRows",
                                                          StringComparison.OrdinalIgnoreCase))
                                {
                                    var nestedRepeat = clonedChild.RepeatRow;

                                    if (record.TryGetValue(nestedRepeat.SourceKey, out var nestedObj) && nestedObj != null)
                                    {
                                        var nestedList = ConvertToListOfDicts(nestedObj);
                                        var expandedRows = ExpandNestedRepeatTable(clonedChild, nestedRepeat, nestedList);

                                        var expandedTable = DeepClone(clonedChild);
                                        expandedTable.Rows = expandedRows;
                                        // IMPORTANT: we don't want this inner RepeatRow to run again later
                                        expandedTable.RepeatRow = null;

                                        newRowsList.Add(expandedTable);
                                    }
                                    else
                                    {
                                        // Fallback: just treat it as a static table with placeholders
                                        var expandedTable = DeepClone(clonedChild);
                                        expandedTable.Rows = ExpandTableTemplateForItem(clonedChild, record);
                                        expandedTable.RepeatRow = null;
                                        newRowsList.Add(expandedTable);
                                    }
                                }// --- 1c) Painting Cost: nested RepeatRow using record["PaintingRows"] ---
                                else if (isPaintingCost
                                         && clonedChild.RepeatRow != null
                                         && string.Equals(clonedChild.RepeatRow.SourceKey, "PaintingRows",
                                                          StringComparison.OrdinalIgnoreCase))
                                {
                                    var nestedRepeat = clonedChild.RepeatRow;

                                    if (record.TryGetValue(nestedRepeat.SourceKey, out var nestedObj) && nestedObj != null)
                                    {
                                        var nestedList = ConvertToListOfDicts(nestedObj);

                                        var originalRows = clonedChild.Rows ?? new List<TableRow>();
                                        var templateIdx = Math.Clamp(nestedRepeat.TemplateRowIndex, 0,
                                                                     Math.Max(0, originalRows.Count - 1));

                                        var templateRow = originalRows.ElementAtOrDefault(templateIdx)
                                                         ?? new TableRow
                                                         {
                                                             RowData = nestedRepeat.Columns?.Select(_ => "").ToList()
                                                                       ?? new List<string>()
                                                         };

                                        var expandedRows = new List<TableRow>();
                                        var sumMap = new Dictionary<string, double>(StringComparer.OrdinalIgnoreCase);

                                        foreach (var item in nestedList)
                                        {
                                            var newRow = new TableRow
                                            {
                                                RowData = new List<string>()
                                            };

                                            foreach (var colKey in nestedRepeat.Columns ?? Enumerable.Empty<string>())
                                            {
                                                if (item.TryGetValue(colKey, out var val) && val != null)
                                                {
                                                    string s = FormatValue(val);
                                                    newRow.RowData.Add(s);

                                                    if (double.TryParse(s, out var dbl))
                                                    {
                                                        if (!sumMap.ContainsKey(colKey)) sumMap[colKey] = 0;
                                                        sumMap[colKey] += dbl;
                                                    }
                                                }
                                                else
                                                {
                                                    // Painting Cost: show "-" instead of "N/A"
                                                    newRow.RowData.Add("-");
                                                }
                                            }

                                            // apply Painting Cost row styling (material / subtotal / footer)
                                            var rowStyle = CloneRowStyle(templateRow.RowStyle) ?? new ReportRowStyle();
                                            var rowType = item.TryGetValue("rowType", out var rtObj)
                                                ? rtObj?.ToString()
                                                : null;

                                            if (string.Equals(rowType, "material", StringComparison.OrdinalIgnoreCase))
                                            {
                                                rowStyle.InlineCss = "background-color:#c7fad6;";
                                            }
                                            else if (string.Equals(rowType, "subtotal", StringComparison.OrdinalIgnoreCase) ||
                                                     string.Equals(rowType, "footer", StringComparison.OrdinalIgnoreCase))
                                            {
                                                rowStyle.InlineCss = "background-color:#f7d8e9;font-weight:bold;";
                                            }

                                            newRow.RowStyle = rowStyle;

                                            expandedRows.Add(newRow);
                                        }

                                        // splice expanded rows between header/footer
                                        var before = originalRows.Take(templateIdx).ToList();
                                        var after = originalRows.Skip(templateIdx + 1).ToList();

                                        var finalRows = new List<TableRow>();
                                        finalRows.AddRange(before);
                                        finalRows.AddRange(expandedRows);
                                        finalRows.AddRange(after);

                                        var expandedTable = DeepClone(clonedChild);
                                        expandedTable.Rows = finalRows;
                                        expandedTable.RepeatRow = null;

                                        newRowsList.Add(expandedTable);
                                    }
                                    else
                                    {
                                        // fallback if PaintingRows missing
                                        var expandedTable = DeepClone(clonedChild);
                                        expandedTable.Rows = ExpandTableTemplateForItem(clonedChild, record);
                                        expandedTable.RepeatRow = null;
                                        newRowsList.Add(expandedTable);
                                    }
                                }
                                // --- 1d) Any other future group tables: simple placeholder expansion ---

                                else
                                {
                                    var expandedTable = DeepClone(clonedChild);
                                    expandedTable.Rows = ExpandTableTemplateForItem(clonedChild, record);
                                    expandedTable.RepeatRow = null;
                                    newRowsList.Add(expandedTable);
                                }
                            }
                            // 2) Non-table child rows (heading, text, etc.)
                            else
                            {
                                var cloned = DeepClone(clonedChild);
                                newRowsList.Add(cloned);
                            }
                        }

                        // Page break between group records, not after the last one
                        if (idx < sourceList.Count - 1)
                        {
                            newRowsList.Add(new ReportRow { Type = "pagebreak" });
                        }
                    }

                    // group handled, go to next original row
                    continue;
                }



                // ------ CASE: normal table with RepeatRow (original behavior) ------
                if (row.Type?.Equals("table", StringComparison.OrdinalIgnoreCase) == true && row.RepeatRow != null)
                {
                    var repeat = row.RepeatRow;

                    if (!inputValues.TryGetValue(repeat.SourceKey, out var sourceObj) || sourceObj == null)
                    {
                        // no data => keep row as-is
                        newRowsList.Add(row);
                        continue;
                    }

                    var sourceList = ConvertToListOfDicts(sourceObj);
                    if (sourceList == null || sourceList.Count == 0)
                    {
                        newRowsList.Add(row);
                        continue;
                    }

                    // Use the existing expansion logic you had, but produce a new ReportRow with expanded row.Rows
                    var originalRows = row.Rows ?? new List<TableRow>();
                    var templateIdx = Math.Clamp(repeat.TemplateRowIndex, 0, Math.Max(0, originalRows.Count - 1));
                    var templateRow = originalRows.ElementAtOrDefault(templateIdx) ?? new TableRow { RowData = repeat.Columns?.Select(_ => "").ToList() ?? new List<string>() };

                    var expandedRows = new List<TableRow>();
                    var sumMap = new Dictionary<string, double>(StringComparer.OrdinalIgnoreCase);

                    // iterate each source item and expand (CASE A/CALC-BLOCK or CASE B normal)
                    foreach (var item in sourceList)
                    {
                        // CASE A: BLOCK expansion
                        if (templateRow != null
                            && templateRow.RowData != null
                            && templateRow.RowData.Count > 0
                            && templateRow.RowData[0] == "__BLOCK__"
                            && templateRow.BlockRows != null
                            && templateRow.BlockRows.Count > 0)
                        {
                            foreach (var block in templateRow.BlockRows)
                            {
                                if (block == null) continue;

                                

                                if (block.IsHeader)
                                {
                                    var headerStyle = CloneRowStyle(templateRow.RowStyle);
                                    if (!string.IsNullOrWhiteSpace(block.InlineCss))
                                        headerStyle.InlineCss = block.InlineCss;

                                    var headerRow = new TableRow
                                    {
                                        RowStyle = headerStyle,
                                        RowData = new List<string> { block.Label ?? "", "" },
                                        ColumnSpan = Math.Max(1, block.ColSpan)
                                    };
                                    expandedRows.Add(headerRow);
                                }
                                else
                                {
                                    var valueStr = "N/A";
                                    if (!string.IsNullOrEmpty(block.ValueKey) && item.TryGetValue(block.ValueKey, out var v) && v != null)
                                        valueStr = FormatValue(v);

                                    var rowStyle = CloneRowStyle(templateRow.RowStyle);

                                    if (block.InlineCss == "{#CALC}")
                                        rowStyle.InlineCss = "background-color:#C0C0C0;";
                                    else if (block.InlineCss == "{#USER}")
                                        rowStyle.InlineCss = "background-color:#FFFFFF;";
                                    else if (!string.IsNullOrWhiteSpace(block.InlineCss))
                                        rowStyle.InlineCss = block.InlineCss;

                                    var r = new TableRow
                                    {
                                        RowStyle = rowStyle,
                                        RowData = new List<string> { block.Label ?? "", valueStr }
                                    };

                                    expandedRows.Add(r);

                                }
                            }

                            // spacer after each expanded record
                            expandedRows.Add(new TableRow
                            {
                                RowStyle = new ReportRowStyle(),
                                RowData = new List<string> { "", "" }
                            });
                        }
                        else
                        {
                            // CASE B: single-row template replaced column-wise
                            //var newRow = new TableRow
                            //{
                            //    RowStyle = CloneRowStyle(templateRow.RowStyle),
                            //    RowData = new List<string>()
                            //};

                            //foreach (var colKey in repeat.Columns ?? Enumerable.Empty<string>())
                            //{
                            //    if (item.TryGetValue(colKey, out var val) && val != null)
                            //    {
                            //        string s = FormatValue(val);
                            //        newRow.RowData.Add(s);

                            //        if (double.TryParse(s, out var dbl))
                            //        {
                            //            if (!sumMap.ContainsKey(colKey)) sumMap[colKey] = 0;
                            //            sumMap[colKey] += dbl;
                            //        }
                            //    }
                            //    else
                            //    {
                            //        newRow.RowData.Add("N/A");
                            //    }
                            //}

                            //expandedRows.Add(newRow);

                            // CASE B: single-row template replaced column-wise
                            var isPaintingCost =
                                reportTemplate.ReportName?.Equals("Painting Cost",
                                    StringComparison.OrdinalIgnoreCase) == true;

                            var newRow = new TableRow
                            {
                                RowData = new List<string>()
                            };

                            foreach (var colKey in repeat.Columns ?? Enumerable.Empty<string>())
                            {
                                if (item.TryGetValue(colKey, out var val) && val != null)
                                {
                                    string s = FormatValue(val);
                                    newRow.RowData.Add(s);

                                    // keep the generic summation as-is
                                    if (double.TryParse(s, out var dbl))
                                    {
                                        if (!sumMap.ContainsKey(colKey)) sumMap[colKey] = 0;
                                        sumMap[colKey] += dbl;
                                    }
                                }
                                else
                                {
                                    // For Painting Cost we want "-" instead of "N/A"
                                    var fallback = isPaintingCost ? "-" : "N/A";
                                    newRow.RowData.Add(fallback);
                                }
                            }

                            // --- row-level styling (only Painting Cost) ---
                            if (isPaintingCost)
                            {
                                var rowStyle = CloneRowStyle(templateRow.RowStyle) ?? new ReportRowStyle();

                                var rowType = item.TryGetValue("rowType", out var rtObj)
                                    ? rtObj?.ToString()
                                    : null;

                                // Material rows -> light green background
                                if (string.Equals(rowType, "material", StringComparison.OrdinalIgnoreCase))
                                {
                                    rowStyle.InlineCss = "background-color:#c7fad6;"; // like your UI
                                }
                                // Subtotal + footer rows -> pink and bold
                                else if (string.Equals(rowType, "subtotal", StringComparison.OrdinalIgnoreCase) ||
                                         string.Equals(rowType, "footer", StringComparison.OrdinalIgnoreCase))
                                {
                                    rowStyle.InlineCss = "background-color:#f7d8e9;font-weight:bold;";
                                }

                                newRow.RowStyle = rowStyle;
                            }
                            else
                            {
                                // All other reports behave exactly as before
                                newRow.RowStyle = CloneRowStyle(templateRow.RowStyle);
                            }

                            expandedRows.Add(newRow);

                        }
                    } // foreach item

                    // --- IMPORTANT: splice expandedRows into the original template rows, preserving headers/footers ---
                    var before = originalRows.Take(templateIdx).ToList();
                    var after = originalRows.Skip(templateIdx + 1).ToList();

                    var finalRows = new List<TableRow>();
                    finalRows.AddRange(before);       // keep header/static rows above template row
                    finalRows.AddRange(expandedRows); // insert generated rows
                    finalRows.AddRange(after);        // keep trailing rows (totals / spacers) below template row

                    // Clone the row (so we don't mutate the original template) and set the spliced rows
                    var clonedTableRow = DeepClone(row);
                    clonedTableRow.Rows = finalRows;

                    // push sums to inputValues
                    foreach (var kv in sumMap)
                        inputValues[$"__sum:{kv.Key}"] = kv.Value;

                    newRowsList.Add(clonedTableRow);
                    continue;
                }

                // ------ CASE: any other row (no repeat) — copy as-is ------
                newRowsList.Add(row);
            } // foreach original row

            // Replace the template rows with newRowsList (preserving other metadata)
            reportTemplate.Rows = newRowsList;

            return Task.CompletedTask;
        }


        // Adjust path logic as per your project structure
        private List<TableRow> BuildHopperImageRows(string? hopperType)
        {
            var rows = new List<TableRow>();

            if (string.IsNullOrWhiteSpace(hopperType))
                return rows;

            // Example: map hopperType -> image filenames
            // "Hopper" -> hopper_1.png, hopper_2.png
            // "Trough" -> trough_1.png, trough_2.png
            var imageFiles = hopperType.Equals("Pyramid", StringComparison.OrdinalIgnoreCase)
                ? new[] { "hopper_pyramidal_1.png", "hopper_pyramidal_2.png" }
                : hopperType.Equals("Trough", StringComparison.OrdinalIgnoreCase)
                    ? new[] { "hopper_trough.png" }
                    : Array.Empty<string>();

            foreach (var fileName in imageFiles)
            {
                var base64 = TryLoadImageAsBase64(fileName);
                if (string.IsNullOrEmpty(base64))
                    continue;

                // 2-column table: left empty, right contains base64 image
                rows.Add(new TableRow
                {
                    RowStyle = new ReportRowStyle
                    {
                        // If you want to control bg or padding for image row
                        InlineCss = "background-color:#FFFFFF; padding-top:4px; padding-bottom:4px;"
                    },
                    RowData = new List<string>
            {
                "",                // Description column empty
                base64             // Value column has data:image...
            }
                });
            }

            return rows;
        }

        private string? TryLoadImageAsBase64(string fileName)
        {
            try
            {
                // Example: /wwwroot/images/bagfilter/...
                var root = Directory.GetCurrentDirectory();
                var fullPath = Path.Combine(root, "wwwroot", "images", "bagfilter", fileName);

                if (!File.Exists(fullPath))
                    return null;

                var bytes = File.ReadAllBytes(fullPath);
                var base64 = Convert.ToBase64String(bytes);

                // Let the existing renderer treat this as an image
                return $"data:image/png;base64,{base64}";
            }
            catch (Exception ex)
            {
                _logger?.LogWarning(ex, "Failed to load hopper image {FileName}", fileName);
                return null;
            }
        }

        // Expand a table that has RepeatRow, but where the source list
        // comes from a *nested* collection (e.g. record["BomRows"])
        List<TableRow> ExpandNestedRepeatTable(
            ReportRow tableRow,
            RepeatRowDefinition repeat,
            List<Dictionary<string, object>> sourceList)
        {
            var originalRows = tableRow.Rows ?? new List<TableRow>();
            var templateIdx = Math.Clamp(repeat.TemplateRowIndex, 0, Math.Max(0, originalRows.Count - 1));

            var templateRow = originalRows.ElementAtOrDefault(templateIdx)
                             ?? new TableRow
                             {
                                 RowData = repeat.Columns?.Select(_ => "").ToList()
                                           ?? new List<string>()
                             };

            var expandedRows = new List<TableRow>();
            var sumMap = new Dictionary<string, double>(StringComparer.OrdinalIgnoreCase);

            foreach (var item in sourceList)
            {
                // You can support __BLOCK__ here if ever needed for nested tables.
                // Bill Of Material doesn't use __BLOCK__, so we only do the
                // simple column-wise expansion.
                var newRow = new TableRow
                {
                    RowStyle = CloneRowStyle(templateRow.RowStyle),
                    RowData = new List<string>()
                };

                foreach (var colKey in repeat.Columns ?? Enumerable.Empty<string>())
                {
                    if (item.TryGetValue(colKey, out var val) && val != null)
                    {
                        string s = FormatValue(val);
                        newRow.RowData.Add(s);

                        if (double.TryParse(s, out var dbl))
                        {
                            if (!sumMap.ContainsKey(colKey)) sumMap[colKey] = 0;
                            sumMap[colKey] += dbl;
                        }
                    }
                    else
                    {
                        newRow.RowData.Add("N/A");
                    }
                }

               
                if (item.TryGetValue("Item", out var itemVal) &&
                    itemVal?.ToString()?.Trim().Equals("Total", StringComparison.OrdinalIgnoreCase) == true)
                {
                    // Make the row bold + highlight background if needed
                    newRow.RowStyle ??= new ReportRowStyle();
                    newRow.RowStyle.InlineCss = "font-weight:bold; background-color:#E8EBF5;";
                }
              


                expandedRows.Add(newRow);
            }

            // splice into original: keep header rows before, footer rows after
            var before = originalRows.Take(templateIdx).ToList();
            var after = originalRows.Skip(templateIdx + 1).ToList();

            var finalRows = new List<TableRow>();
            finalRows.AddRange(before);
            finalRows.AddRange(expandedRows);
            finalRows.AddRange(after);

            return finalRows;
        }


        private ReportRowStyle CreateCellStyleFromToken(string? token, ReportRowStyle? baseStyle)
        {
            var style = CloneRowStyle(baseStyle);
            if (string.IsNullOrWhiteSpace(token)) return style;

            if (token == "{#CALC}")
                style.InlineCss = "background-color:#FFF9E6; color:#8A4F00;"; // example
            else if (token == "{#USER}")
                style.InlineCss = "background-color:#E6F7FF; color:#003366;";
            else
                style.InlineCss = token; // assume full css provided

            return style;
        }



        // Helper: convert multiple possible shapes to List<Dictionary<string, object>>
        private List<Dictionary<string, object>>? ConvertToListOfDicts(object sourceObj)
        {
            // Case 1: already the right type
            if (sourceObj is List<Dictionary<string, object>> listOfDicts1)
                return listOfDicts1;

            if (sourceObj is IEnumerable<Dictionary<string, object>> enumerableDicts)
                return enumerableDicts.ToList();

            // Case 2: IEnumerable<object> (e.g., List<object> where items are JsonElement or IDictionary)
            if (sourceObj is IEnumerable<object> objEnum)
            {
                var result = new List<Dictionary<string, object>>();
                foreach (var item in objEnum)
                {
                    if (item is Dictionary<string, object> dict)
                    {
                        result.Add(dict);
                        continue;
                    }

                    // If it's JsonElement (from System.Text.Json when deserializing object)
                    if (item is System.Text.Json.JsonElement je && je.ValueKind == System.Text.Json.JsonValueKind.Object)
                    {
                        var dictFromJe = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
                        foreach (var prop in je.EnumerateObject())
                        {
                            dictFromJe[prop.Name] = JsonElementToObject(prop.Value);
                        }
                        result.Add(dictFromJe);
                        continue;
                    }

                    // If it's IDictionary<string, object> (some serializers)
                    if (item is IDictionary<string, object> idict)
                    {
                        result.Add(idict.ToDictionary(k => k.Key, v => v.Value));
                        continue;
                    }

                    // last attempt: try convert via JsonSerializer
                    try
                    {
                        var json = System.Text.Json.JsonSerializer.Serialize(item);
                        var dict2 = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(json);
                        if (dict2 != null) result.Add(dict2);
                    }
                    catch
                    {
                        // ignore individual conversion failure
                    }
                }

                return result;
            }

            // Case 3: single object that is a JsonElement array/object
            if (sourceObj is System.Text.Json.JsonElement jeRoot)
            {
                if (jeRoot.ValueKind == System.Text.Json.JsonValueKind.Array)
                {
                    var result = new List<Dictionary<string, object>>();
                    foreach (var child in jeRoot.EnumerateArray())
                    {
                        if (child.ValueKind == System.Text.Json.JsonValueKind.Object)
                        {
                            var d = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
                            foreach (var prop in child.EnumerateObject())
                                d[prop.Name] = JsonElementToObject(prop.Value);
                            result.Add(d);
                        }
                    }
                    return result;
                }
                else if (jeRoot.ValueKind == System.Text.Json.JsonValueKind.Object)
                {
                    var d = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
                    foreach (var prop in jeRoot.EnumerateObject())
                        d[prop.Name] = JsonElementToObject(prop.Value);
                    return new List<Dictionary<string, object>> { d };
                }
            }

            return null;
        }

        private object? JsonElementToObject(System.Text.Json.JsonElement element)
        {
            return element.ValueKind switch
            {
                System.Text.Json.JsonValueKind.String => element.GetString(),
                System.Text.Json.JsonValueKind.Number => element.TryGetInt64(out var l) ? (object)l :
                                                      element.TryGetDouble(out var d) ? (object)d : element.GetRawText(),
                System.Text.Json.JsonValueKind.True => true,
                System.Text.Json.JsonValueKind.False => false,
                System.Text.Json.JsonValueKind.Null => null,
                System.Text.Json.JsonValueKind.Object => element.ToString(), // Could deserialize recursively if needed
                System.Text.Json.JsonValueKind.Array => element.ToString(),
                _ => element.ToString()
            };
        }



        /// <summary>
        /// Prepare a single template with header and row values.
        /// Values dictionaries should contain the keys used inside placeholders.
        /// </summary>
        public async Task<ReportTemplateModelDto> PrepareTemplateWithValuesAsync(
            ReportTemplateModelDto template,
            Dictionary<string, object> rowValues,
            Dictionary<string, object> headerValues)
        {
            rowValues ??= new Dictionary<string, object>();
            headerValues ??= new Dictionary<string, object>();

            // 1. Header placeholders
            await ProcessHeaderPlaceholdersAsync(template, headerValues);

            // 2. Expand any RepeatRow table definitions into concrete rows and compute aggregates
            await ExpandRepeatRowsAsync(template, rowValues);

            // 3. Replace placeholders in rows (now includes expanded rows and aggregate values)
            await ProcessRowPlaceholdersAsync(template, rowValues);

            return template;
        }


        private async Task ProcessHeaderPlaceholdersAsync(ReportTemplateModelDto template, Dictionary<string, object> headerValues)
        {

            async Task<string> ReplacePlaceholdersAsync(string input)
            {
                return await EvaluateAllEmbeddedPlaceholdersAsync(input, headerValues);
            }

            if (!string.IsNullOrWhiteSpace(template.Title))
            {
                // ensure title value available as placeholder for other header fields
                headerValues["Title"] = template.Title.ToUpperInvariant();
                template.Title = await ReplacePlaceholdersAsync(template.Title);
            }

            if (template.Header?.TextRows != null && template.Header.TextRows.Length > 0)
            {
                for (int i = 0; i < template.Header.TextRows.Length; i++)
                {
                    template.Header.TextRows[i] = await ReplacePlaceholdersAsync(template.Header.TextRows[i]);
                }
            }

            if (template.Footer?.TextRows != null && template.Footer.TextRows.Length > 0)
            {
                for (int i = 0; i < template.Footer.TextRows.Length; i++)
                {
                    var line = template.Footer.TextRows[i];

                    // support a {Logo} placeholder replacement
                    if (!string.IsNullOrEmpty(template.Logo))
                    {
                        line = line.Replace("{Logo}", template.Logo);
                    }

                    template.Footer.TextRows[i] = await ReplacePlaceholdersAsync(line);
                }
            }
        }

        private async Task ProcessRowPlaceholdersAsync(ReportTemplateModelDto reportTemplate, Dictionary<string, object> inputValues)
        {
            if (reportTemplate.Rows == null) return;


            for (int r = 0; r < reportTemplate.Rows.Count; r++)
            {
                var row = reportTemplate.Rows[r];

                if (row == null) continue;

                if (row.Type?.Equals("table", StringComparison.OrdinalIgnoreCase) == true && row.Rows != null)
                {
                    for (int rowIndex = 0; rowIndex < row.Rows.Count; rowIndex++)
                    {
                        var tableRow = row.Rows[rowIndex];
                        for (int colIndex = 0; colIndex < tableRow.RowData.Count; colIndex++)
                        {
                            tableRow.RowData[colIndex] = await EvaluateAllEmbeddedPlaceholdersAsync(tableRow.RowData[colIndex], inputValues);
                        }
                    }
                }
                else if (row.Type?.Equals("text", StringComparison.OrdinalIgnoreCase) == true ||
                         row.Type?.Equals("heading", StringComparison.OrdinalIgnoreCase) == true)
                {
                    if (!string.IsNullOrEmpty(row.Text))
                    {
                        row.Text = await EvaluateAllEmbeddedPlaceholdersAsync(row.Text, inputValues);
                    }
                }
                else if (row.Type?.Equals("image", StringComparison.OrdinalIgnoreCase) == true)
                {
                    if (!string.IsNullOrEmpty(row.Caption))
                    {
                        row.Caption = await EvaluateAllEmbeddedPlaceholdersAsync(row.Caption, inputValues);
                    }

                    if (!string.IsNullOrEmpty(row.Src))
                    {
                        row.Src = await EvaluateAllEmbeddedPlaceholdersAsync(row.Src, inputValues);
                    }
                }
            }
        }

        

        private async Task<string> EvaluateAllEmbeddedPlaceholdersAsync(string input, Dictionary<string, object> inputValues)
        {
            if (string.IsNullOrWhiteSpace(input))
                return input ?? "";

            string text = input;
            var matches = PlaceholderRegex.Matches(text);

            // Note: replace matches left-to-right. Because we mutate text we capture current snapshot.
            foreach (Match match in matches)
            {
                var placeholder = match.Value; // includes braces, e.g., "{DesignAvgFlow}"
                try
                {
                    // inner content without braces
                    var inner = placeholder.Trim('{', '}').Trim();

                    // 1) inline formula: {formula:inline: ... }
                    if (inner.StartsWith("formula:inline:", StringComparison.OrdinalIgnoreCase))
                    {
                        var expr = inner.Substring("formula:inline:".Length).Trim();
                        //var result = EvaluateInlineFormula(expr, inputValues);
                        //text = text.Replace(placeholder, result);
                        continue;
                    }

                    // 2) named formula: {formula:Name} -> placeholder for future formula engine
                    if (inner.StartsWith("formula:", StringComparison.OrdinalIgnoreCase))
                    {
                        // If you have a formula evaluator, hook here. For now fallback:
                        text = text.Replace(placeholder, "N/A");
                        continue;
                    }

                    // 3) sum aggregation placeholder: __sum:FieldName
                    if (inner.StartsWith("__sum:", StringComparison.OrdinalIgnoreCase))
                    {
                        var key = "__sum:" + inner.Substring("__sum:".Length);
                        if (inputValues.TryGetValue(key, out var sumVal) && sumVal != null)
                        {
                            text = text.Replace(placeholder, sumVal.ToString() ?? "0");
                        }
                        else
                        {
                            text = text.Replace(placeholder, "0");
                        }
                        continue;
                    }

                    // 4) normal variable replacement {VariableName}
                    var variableName = inner;
                    var variableValue = GetVariableValueAsString(variableName, inputValues);
                    text = text.Replace(placeholder, variableValue);
                }
                catch (Exception ex)
                {
                    _logger?.LogWarning(ex, "Error evaluating placeholder {Placeholder} in {Input}", match.Value, input);
                    text = text.Replace(match.Value, "N/A");
                }
            }

            return await Task.FromResult(text);
        }
        
        private string GetVariableValueAsString(string variableName, Dictionary<string, object> inputValues)
        {
            if (inputValues != null && inputValues.TryGetValue(variableName, out var value))
            {
                if (value == null) return "N/A";

                return value switch
                {
                    double d => d.ToString("0.##"),
                    float f => f.ToString("0.##"),
                    decimal m => m.ToString("0.##"),
                    int i => i.ToString(),
                    long l => l.ToString(),
                    string s => s,
                    _ => value.ToString()
                };
            }

            // fallback: not found
            _logger?.LogDebug("Variable '{VarName}' missing in supplied values", variableName);
            return "N/A";
        }

    }
}
