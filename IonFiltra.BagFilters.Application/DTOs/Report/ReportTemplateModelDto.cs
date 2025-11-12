using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IonFiltra.BagFilters.Application.DTOs.Report
{
    public class ReportTemplateModelDto
    {
        public int Id { get; set; }
        public string ReportName { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Logo { get; set; } = string.Empty;
        public LayoutConfig Layout { get; set; } = new LayoutConfig();
        public HeaderFooterConfig Header { get; set; } = new HeaderFooterConfig();
        public HeaderFooterConfig Footer { get; set; } = new HeaderFooterConfig();
        public List<ReportRow> Rows { get; set; } = new();
        public ReportStyles Styles { get; set; } = new ReportStyles();
        public string? EntityDbName { get; set; }
    }

    public class LayoutConfig
    {
        public int Columns { get; set; } = 3;  // Default to 3-column layout
        public List<string> ColumnWidths { get; set; } = new() { "33%", "33%", "34%" };

        public string ReportWidth { get; set; } = "210mm"; // A4 width (default 210mm)
    }

    public class HeaderFooterConfig
    {
        public string Text { get; set; } = string.Empty;

        public string[] TextRows { get; set; } = Array.Empty<string>();
        public string Alignment { get; set; } = "center";  // left, center, right
        public int FontSize { get; set; } = 14;
        public string Color { get; set; } = "#000000";
    }



    public class ReportRow
    {
        public string Type { get; set; } = "text";  // text, heading, table, image
        public string HeadingType { get; set; } = "";
        public string? Text { get; set; }  // Used for headings or text
        public ReportRowStyle Style { get; set; } = new();
        public List<string>? LayoutColumns { get; set; }  // Column headers for tables

        public List<double>? ColumnWidths { get; set; }
        public List<TableRow>? Rows { get; set; }  // Now stores a list of TableRow objects
        public string? Src { get; set; }  // Image source
        public string? Caption { get; set; }  // Image caption
        public RepeatRowDefinition? RepeatRow { get; set; } // NEW
    }

    // Define a new TableRow class for individual row styling
    public class TableRow
    {
        public List<string> RowData { get; set; } = new();
        public ReportRowStyle RowStyle { get; set; } = new();
        // NEW (instructions for renderer): if >1, some cells should use column span.
        public int ColumnSpan { get; set; } = 1;
        public List<BlockRowItem>? BlockRows { get; set; } = null;
    }

    public class ReportRowStyle
    {
        public string FontFamily { get; set; } = "Calibri";
        public int FontSize { get; set; } = 12;
        public string Color { get; set; } = "#000000";
        public bool Bold { get; set; } = false;
        public bool Italic { get; set; } = false;
        public string Alignment { get; set; } = "left";  // left, center, right
        public string InlineCss { get; set; } = "";
    }

    public class ReportStyles
    {
        public string FontFamily { get; set; } = "Arial";
        public int FontSize { get; set; } = 12;
        public string TextColor { get; set; } = "#000000";
        public string BackgroundColor { get; set; } = "#FFFFFF";
    }

    public class RepeatRowDefinition
    {
        public string SourceKey { get; set; } = "list_values";
        public int TemplateRowIndex { get; set; } = 1;
        public List<string> Columns { get; set; } = new();
    }

    public class BlockRowItem
    {
        public string Label { get; set; } = "";
        public string? ValueKey { get; set; } = null; // null => header row (no value cell)
        public bool IsHeader { get; set; } = false;
        public bool IsFooter { get; set; } = false;
        public string? InlineCss { get; set; } = null;
        public int ColSpan { get; set; } = 1; // if 2 => span both columns
    }



}
