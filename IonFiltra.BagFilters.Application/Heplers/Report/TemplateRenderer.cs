using System.Text.Json;
using System.Text.RegularExpressions;
using IonFiltra.BagFilters.Application.Models.Report;

namespace IonFiltra.BagFilters.Application.Heplers.Report
{
    public static class TemplateRenderer
    {
        private static readonly Regex PlaceholderRegex = new Regex(@"\{\{\s*(\w+)\s*\}\}", RegexOptions.Compiled);

        // Load JSON from a file path and return deserialized TemplateRoot
        public static async Task<TemplateRoot> LoadTemplateFromFileAsync(string filePath)
        {
            var json = await File.ReadAllTextAsync(filePath);
            return JsonSerializer.Deserialize<TemplateRoot>(json);
        }

        // Render - replace placeholders in all string values inside the TemplateRoot's nested dictionaries
        public static TemplateRoot RenderTemplate(TemplateRoot template, IDictionary<string, object> values)
        {
            if (template == null) return null;

            foreach (var row in template.Rows)
            {
                foreach (var col in row.Columns)
                {
                    ReplacePlaceholdersRecursive(col.Content, values);
                }
            }

            return template;
        }

        private static void ReplacePlaceholdersRecursive(Dictionary<string, object> node, IDictionary<string, object> values)
        {
            var keys = new List<string>(node.Keys);
            foreach (var key in keys)
            {
                var val = node[key];

                if (val is JsonElement je)
                {
                    // convert JsonElement to primitive where possible
                    node[key] = JeToObject(je);
                    val = node[key];
                }

                if (val is string s)
                {
                    node[key] = ReplacePlaceholdersInString(s, values);
                }
                else if (val is Dictionary<string, object> innerDict)
                {
                    ReplacePlaceholdersRecursive(innerDict, values);
                }
                else if (val is JsonElement[] || val is object[])
                {
                    // seldom used - if arrays exist, you can extend here.
                }
                else if (val is Dictionary<string, JsonElement> dictJe)
                {
                    // guard for cases where deserializer produces JsonElement nested
                    var converted = new Dictionary<string, object>();
                    foreach (var kv in dictJe)
                        converted[kv.Key] = JeToObject(kv.Value);
                    ReplacePlaceholdersRecursive(converted, values);
                    node[key] = converted;
                }
            }
        }

        private static string ReplacePlaceholdersInString(string input, IDictionary<string, object> values)
        {
            return PlaceholderRegex.Replace(input, match =>
            {
                var key = match.Groups[1].Value;
                if (values != null && values.TryGetValue(key, out var val) && val != null)
                {
                    return val.ToString();
                }
                return string.Empty;
            });
        }

        private static object JeToObject(JsonElement je)
        {
            switch (je.ValueKind)
            {
                case JsonValueKind.String: return je.GetString();
                case JsonValueKind.Number:
                    if (je.TryGetInt64(out var l)) return l;
                    if (je.TryGetDouble(out var d)) return d;
                    return je.GetRawText();
                case JsonValueKind.True: return true;
                case JsonValueKind.False: return false;
                case JsonValueKind.Null: return null;
                default: return je.GetRawText();
            }
        }
    }
}
