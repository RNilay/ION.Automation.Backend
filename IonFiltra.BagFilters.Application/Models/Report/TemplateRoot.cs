using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace IonFiltra.BagFilters.Application.Models.Report
{
    // Root templates
    public class TemplateRoot
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("layout")]
        public Dictionary<string, object> Layout { get; set; }

        [JsonPropertyName("rows")]
        public List<TemplateRow> Rows { get; set; }
    }

    public class TemplateRow
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("columns")]
        public List<TemplateColumn> Columns { get; set; }
    }

    public class TemplateColumn
    {
        [JsonPropertyName("width")]
        public string Width { get; set; }

        [JsonPropertyName("content")]
        public Dictionary<string, object> Content { get; set; }
    }

    // Represents the main report body template (like report_template.json)
    public class ReportTemplate
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }  // e.g., "report"

        [JsonPropertyName("layout")]
        public Dictionary<string, object> Layout { get; set; }

        [JsonPropertyName("sections")]
        public List<ReportSection> Sections { get; set; }
    }

    // Represents a single section (like "table", "textblock", etc.)
    public class ReportSection
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }  // e.g., "table"

        [JsonPropertyName("id")]
        public string Id { get; set; }  // optional unique ID

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("columns")]
        public List<ReportColumn> Columns { get; set; }

        [JsonPropertyName("style")]
        public Dictionary<string, object> Style { get; set; }
    }

    // Represents a column definition inside a table section
    public class ReportColumn
    {
        [JsonPropertyName("header")]
        public string Header { get; set; }

        [JsonPropertyName("field")]
        public string Field { get; set; }

        [JsonPropertyName("width")]
        public string Width { get; set; }

        [JsonPropertyName("align")]
        public string Align { get; set; }
    }
}
