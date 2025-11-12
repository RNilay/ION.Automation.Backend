using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using IonFiltra.BagFilters.Application.Models.Report;

namespace IonFiltra.BagFilters.Application.Helpers
{
    public static class ReportTemplateLoader
    {
        private static readonly Regex PlaceholderRegex = new Regex(@"\{\{\s*(\w+)\s*\}\}", RegexOptions.Compiled);

        // Loads the report template JSON and renders placeholders (title/header/style) using provided values.
        // It also normalizes column.Field by trimming braces (so "{{Name}}" -> "Name").
        public static async Task<ReportTemplate> LoadAndRenderAsync(string filePath, IDictionary<string, object> values)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentNullException(nameof(filePath));
            if (!File.Exists(filePath))
                throw new FileNotFoundException("Report template not found", filePath);

            var json = await File.ReadAllTextAsync(filePath);
            var opts = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReadCommentHandling = JsonCommentHandling.Skip,
                AllowTrailingCommas = true
            };

            var template = JsonSerializer.Deserialize<ReportTemplate>(json, opts);
            if (template == null) return null;

            // Render section titles and column headers (replace placeholders)
            if (template.Sections != null)
            {
                foreach (var section in template.Sections)
                {
                    section.Title = ReplacePlaceholders(section.Title, values);

                    if (section.Columns != null)
                    {
                        foreach (var col in section.Columns)
                        {
                            // header might contain placeholders (replace them)
                            col.Header = ReplacePlaceholders(col.Field, values);

                            // normalize Field to remove any braces if present, we do not replace it with runtime data here
                            if (!string.IsNullOrWhiteSpace(col.Field))
                                col.Field = TrimBraces(col.Field);
                        }
                    }

                    // style values might contain placeholders as strings (optional)
                    if (section.Style != null)
                    {
                        var keys = new List<string>(section.Style.Keys);
                        foreach (var k in keys)
                        {
                            if (section.Style[k] is string s)
                            {
                                section.Style[k] = ReplacePlaceholders(s, values);
                            }
                        }
                    }
                }
            }

            // layout may contain placeholders too
            if (template.Layout != null)
            {
                var layoutKeys = new List<string>(template.Layout.Keys);
                foreach (var k in layoutKeys)
                {
                    if (template.Layout[k] is string s)
                        template.Layout[k] = ReplacePlaceholders(s, values);
                }
            }

            return template;
        }

        // Replaces {{Key}} with values[Key].ToString() if present; leaves input unchanged otherwise.
        //private static string ReplacePlaceholders(string input, IDictionary<string, object> values)
        //{
        //    if (string.IsNullOrEmpty(input) || values == null) return input;

        //    return PlaceholderRegex.Replace(input, match =>
        //    {
        //        var key = match.Groups[1].Value;
        //        if (values.TryGetValue(key, out var val) && val != null)
        //            return val.ToString();
        //        return string.Empty;
        //    });
        //}

        private static string ReplacePlaceholders(string input, IDictionary<string, object> values)
        {
            if (string.IsNullOrEmpty(input) || values == null)
                return input;

            return PlaceholderRegex.Replace(input, match =>
            {
                var key = match.Groups[1].Value;
                object val = null;
                string valueToReplace = string.Empty;

                try
                {
                    if (values.TryGetValue(key, out val) && val != null)
                        valueToReplace = val.ToString() ?? string.Empty;
                    else
                        valueToReplace = string.Empty;

                    // 🪵 Debug and Logging
                    Debug.WriteLine($"[TemplateRenderer] Placeholder: {key} -> '{valueToReplace}'");

                    // You can also log to a file or console:
                    // Console.WriteLine($"[TemplateRenderer] Placeholder: {key} -> '{valueToReplace}'");

                    // 💡 Set a breakpoint here if you want to inspect keys dynamically
                    // Debugger.Break(); // uncomment only while debugging!
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"[TemplateRenderer] ERROR replacing key '{key}': {ex.Message}");
                    valueToReplace = string.Empty;
                }

                return valueToReplace;
            });
        }

        // Trim braces if field is like {{Name}} or {Name}; otherwise return as-is.
        private static string TrimBraces(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return s;
            var t = s.Trim();
            if (t.StartsWith("{{") && t.EndsWith("}}")) return t.Substring(2, t.Length - 4).Trim();
            if (t.StartsWith("{") && t.EndsWith("}")) return t.Substring(1, t.Length - 2).Trim();
            return t;
        }
    }
}
