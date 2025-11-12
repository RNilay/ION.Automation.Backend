using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using IonFiltra.BagFilters.Application.DTOs.Report;
using IonFiltra.BagFilters.Application.Heplers.Report;
using IonFiltra.BagFilters.Application.Interfaces.Report;
using IonFiltra.BagFilters.Application.Models.Report;
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

        //private Task ExpandRepeatRowsAsync(ReportTemplateModelDto reportTemplate, Dictionary<string, object> inputValues)
        //{
        //    if (reportTemplate.Rows == null) return Task.CompletedTask;

        //    foreach (var row in reportTemplate.Rows)
        //    {
        //        if (row.Type?.Equals("table", StringComparison.OrdinalIgnoreCase) != true)
        //            continue;

        //        var repeat = row.RepeatRow;
        //        if (repeat == null) continue;

        //        if (!inputValues.TryGetValue(repeat.SourceKey, out var sourceObj) || sourceObj == null)
        //            continue;

        //        // Normalize the source to a List<Dictionary<string, object>>
        //        var sourceList = ConvertToListOfDicts(sourceObj);
        //        if (sourceList == null || sourceList.Count == 0)
        //        {
        //            // nothing to expand
        //            row.Rows = row.Rows; // no-op but explicit
        //            continue;
        //        }

        //        // Validate template index
        //        var templateIdx = Math.Clamp(repeat.TemplateRowIndex, 0, (row.Rows?.Count ?? 1) - 1);
        //        var templateRow = row.Rows?[templateIdx] ?? new TableRow { RowData = repeat.Columns.Select(_ => "").ToList() };

        //        var expandedRows = new List<TableRow>();
        //        var sumMap = new Dictionary<string, double>(StringComparer.OrdinalIgnoreCase);

        //        foreach (var item in sourceList)
        //        {
        //            var newRow = new TableRow
        //            {
        //                RowStyle = templateRow.RowStyle ?? new ReportRowStyle(),
        //                RowData = new List<string>()
        //            };

        //            foreach (var colKey in repeat.Columns)
        //            {
        //                if (item.TryGetValue(colKey, out var val) && val != null)
        //                {
        //                    string s = val switch
        //                    {
        //                        double d => d.ToString("0.##"),
        //                        float f => f.ToString("0.##"),
        //                        decimal m => m.ToString("0.##"),
        //                        int i => i.ToString(),
        //                        long l => l.ToString(),
        //                        _ => val.ToString() ?? ""
        //                    };

        //                    newRow.RowData.Add(s);

        //                    if (double.TryParse(s, out var dbl))
        //                    {
        //                        if (!sumMap.ContainsKey(colKey)) sumMap[colKey] = 0;
        //                        sumMap[colKey] += dbl;
        //                    }
        //                }
        //                else
        //                {
        //                    newRow.RowData.Add("N/A");
        //                }
        //            }

        //            expandedRows.Add(newRow);
        //        }

        //        // Splice expanded rows into row.Rows (keep before/after template rows)
        //        var before = (row.Rows ?? new List<TableRow>()).Take(templateIdx).ToList();
        //        var after = (row.Rows ?? new List<TableRow>()).Skip(templateIdx + 1).ToList();

        //        var finalRows = new List<TableRow>();
        //        finalRows.AddRange(before);
        //        finalRows.AddRange(expandedRows);
        //        finalRows.AddRange(after);

        //        row.Rows = finalRows;

        //        // Push sums into inputValues as "__sum:Key"
        //        foreach (var kv in sumMap)
        //        {
        //            inputValues[$"__sum:{kv.Key}"] = kv.Value;
        //        }
        //    }

        //    return Task.CompletedTask;
        //}

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


        private Task ExpandRepeatRowsAsync(ReportTemplateModelDto reportTemplate, Dictionary<string, object> inputValues)
        {
            if (reportTemplate.Rows == null) return Task.CompletedTask;

            foreach (var row in reportTemplate.Rows)
            {
                if (row.Type?.Equals("table", StringComparison.OrdinalIgnoreCase) != true)
                    continue;

                var repeat = row.RepeatRow;
                if (repeat == null) continue;

                if (!inputValues.TryGetValue(repeat.SourceKey, out var sourceObj) || sourceObj == null)
                    continue;

                // Normalize the source to a List<Dictionary<string, object>>
                var sourceList = ConvertToListOfDicts(sourceObj);
                if (sourceList == null || sourceList.Count == 0)
                {
                    // nothing to expand
                    row.Rows = row.Rows; // no-op but explicit
                    continue;
                }

                // Validate template index
                var templateIdx = Math.Clamp(repeat.TemplateRowIndex, 0, (row.Rows?.Count ?? 1) - 1);
                var templateRow = row.Rows?[templateIdx] ?? new TableRow { RowData = repeat.Columns.Select(_ => "").ToList() };

                var expandedRows = new List<TableRow>();
                var sumMap = new Dictionary<string, double>(StringComparer.OrdinalIgnoreCase);

                // Helper to format a single object value to string
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

                // Helper: quick placeholder replacement for fallback (simple {Key} substitution)
                static string EvaluatePlaceholdersSync(string templateText, Dictionary<string, object> item)
                {
                    if (string.IsNullOrWhiteSpace(templateText)) return templateText ?? "";

                    var text = templateText;
                    var matches = Regex.Matches(text, @"\{(.*?)\}");
                    foreach (Match m in matches)
                    {
                        var inner = m.Groups[1].Value;
                        if (item.TryGetValue(inner, out var val) && val != null)
                        {
                            text = text.Replace(m.Value, FormatValue(val));
                        }
                        else
                        {
                            text = text.Replace(m.Value, "N/A");
                        }
                    }

                    return text;
                }

                foreach (var item in sourceList)
                {
                    // CASE A: BLOCK expansion if templateRow is marked as "__BLOCK__" and has BlockRows
                    if (templateRow != null
                        && templateRow.RowData != null
                        && templateRow.RowData.Count > 0
                        && templateRow.RowData[0] == "__BLOCK__"
                        && templateRow.BlockRows != null
                        && templateRow.BlockRows.Count > 0)
                    {
                        foreach (var block in templateRow.BlockRows)
                        {
                            if (block == null)
                                continue;

                            if (block.IsHeader)
                            {
                                var headerStyle = CloneRowStyle(templateRow.RowStyle);

                                // apply inline css from block definition (overrides cloned style)
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

                                // override with row-specific inline css if provided
                                if (!string.IsNullOrWhiteSpace(block.InlineCss))
                                    rowStyle.InlineCss = block.InlineCss;

                                var r = new TableRow
                                {
                                    RowStyle = rowStyle,
                                    RowData = new List<string> { block.Label ?? "", valueStr }
                                };

                                expandedRows.Add(r);
                            }

                        }

                        // optional spacer after each record block
                        expandedRows.Add(new TableRow
                        {
                            RowStyle = new ReportRowStyle(),
                            RowData = new List<string> { "", "" }
                        });
                    }
                    else
                    {
                        // CASE B: original behavior — treat templateRow as a single-row template and replace placeholders for each repeat column
                        var newRow = new TableRow
                        {
                            RowStyle = CloneRowStyle(templateRow.RowStyle),
                            RowData = new List<string>()
                        };


                        foreach (var colKey in repeat.Columns)
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

                        expandedRows.Add(newRow);
                    }
                }

                // Splice expanded rows into row.Rows (keep before/after template rows)
                var before = (row.Rows ?? new List<TableRow>()).Take(templateIdx).ToList();
                var after = (row.Rows ?? new List<TableRow>()).Skip(templateIdx + 1).ToList();

                var finalRows = new List<TableRow>();
                finalRows.AddRange(before);
                finalRows.AddRange(expandedRows);
                finalRows.AddRange(after);

                row.Rows = finalRows;

                // Push sums into inputValues as "__sum:Key"
                foreach (var kv in sumMap)
                {
                    inputValues[$"__sum:{kv.Key}"] = kv.Value;
                }
            }

            return Task.CompletedTask;
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

        /// <summary>
        /// Replaces placeholders and evaluates inline formulas.
        /// Supports:
        ///  - {VariableName}
        ///  - {formula:inline: ... }  (NCalc expression)
        ///  - {formula:Name} - not implemented here, returns "N/A" (you can add evaluator if needed)
        /// </summary>
        //private async Task<string> EvaluateAllEmbeddedPlaceholdersAsync(string input, Dictionary<string, object> inputValues)
        //{
        //    if (string.IsNullOrWhiteSpace(input))
        //        return input ?? "";

        //    var text = input;
        //    var matches = PlaceholderRegex.Matches(text);

        //    // Evaluate matches left-to-right
        //    foreach (Match match in matches)
        //    {
        //        var placeholder = match.Value;
        //        try
        //        {
        //            if (placeholder.StartsWith("{formula:inline:", StringComparison.OrdinalIgnoreCase))
        //            {
        //                // remove markers
        //                var expr = placeholder.Substring("{formula:inline:".Length);
        //                if (expr.EndsWith("}")) expr = expr[..^1];

        //                //var result = EvaluateInlineFormula(expr, inputValues);
        //                //text = text.Replace(placeholder, result);
        //            }
        //            else if (placeholder.StartsWith("{formula:", StringComparison.OrdinalIgnoreCase))
        //            {
        //                // non-inline formula reference: by default we don't have named formulas here
        //                // return N/A or you can hook to your formula evaluator service
        //                text = text.Replace(placeholder, "N/A");
        //            }
        //            if (placeholder.StartsWith("__sum:", StringComparison.OrdinalIgnoreCase))
        //            {
        //                // variableName is like "__sum:Bagfilter"
        //                if (inputValues.TryGetValue(placeholder, out var sumVal))
        //                    text = text.Replace(placeholder, (sumVal)?.ToString() ?? "0");
        //                else
        //                    text = text.Replace(placeholder, "0");
        //            }
        //            else
        //            {
        //                var variableName = placeholder.Trim('{', '}');
        //                var variableValue = GetVariableValueAsString(variableName, inputValues);
        //                text = text.Replace(placeholder, variableValue);
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            _logger?.LogWarning(ex, "Error evaluating placeholder {Placeholder} in {Input}", placeholder, input);
        //            text = text.Replace(placeholder, "N/A");
        //        }
        //    }

        //    return await Task.FromResult(text);
        //}


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


        //private string EvaluateInlineFormula(string formulaExpression, Dictionary<string, object> inputValues)
        //{
        //    try
        //    {
        //        var expr = new Expression(formulaExpression, EvaluateOptions.IgnoreCase);

        //        // pass parameters
        //        foreach (var kvp in inputValues)
        //        {
        //            // NCalc accepts primitive types; if non-primitive, let it be (ToString fallback)
        //            expr.Parameters[kvp.Key] = kvp.Value ?? 0;
        //        }

        //        var result = expr.Evaluate();
        //        return result?.ToString() ?? "N/A";
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger?.LogError(ex, "Inline formula evaluation failed for '{Formula}'", formulaExpression);
        //        return "N/A";
        //    }
        //}

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










        public async Task<byte[]> GeneratePdfBytesAsync(TemplateRoot header, TemplateRoot footer, List<Dictionary<string, object>> rows, byte[] companyIcon = null, ReportTemplate reportTemplate = null)
        {
            // 🔹 Step 2: PDF Setup
            QuestPDF.Settings.License = LicenseType.Community;
            var memoryStream = new MemoryStream();
            //var document = new GenericReportDocument(header, footer, rows, companyIcon);
            var document = new GenericReportDocument(header, footer, rows, companyIcon, reportTemplate);
            return document.GeneratePdf();
        }

        // helper to load and render templates from file paths with provided values
        public static async Task<(TemplateRoot header, TemplateRoot footer)> GetHeaderFooterRenderedAsync(
            string headerFilePath,
            string footerFilePath,
            IDictionary<string, object> values)
        {
            var header = await TemplateRenderer.LoadTemplateFromFileAsync(headerFilePath);
            var footer = await TemplateRenderer.LoadTemplateFromFileAsync(footerFilePath);

            header = TemplateRenderer.RenderTemplate(header, values);
            footer = TemplateRenderer.RenderTemplate(footer, values);

            return (header, footer);
        }
    }
}
