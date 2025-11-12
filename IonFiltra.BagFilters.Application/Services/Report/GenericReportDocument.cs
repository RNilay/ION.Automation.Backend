using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IonFiltra.BagFilters.Application.Heplers.Report;
using IonFiltra.BagFilters.Application.Models.Report;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace IonFiltra.BagFilters.Application.Services.Report
{
    // QuestPDF Document implementation
    public class GenericReportDocument : IDocument
    {
        private readonly TemplateRoot _header;
        private readonly TemplateRoot _footer;
        private readonly List<Dictionary<string, object>> _rows;
        private readonly byte[] _companyIcon;
        private readonly ReportTemplate _reportTemplate;

        public GenericReportDocument(TemplateRoot header, TemplateRoot footer, List<Dictionary<string, object>> rows, byte[] companyIcon = null, ReportTemplate reportTemplate = null)
        {
            _header = header;
            _footer = footer;
            _rows = rows ?? new List<Dictionary<string, object>>();
            _companyIcon = companyIcon;
            _reportTemplate = reportTemplate;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Margin(20);

                // Header
                page.Header().Element(ComposeHeader);

                // Content
                page.Content().Element(ComposeContent);

                // Footer
                page.Footer().Element(ComposeFooter);
            });
        }

        private void ComposeHeader(IContainer container)
        {
            if (_header == null) return;

            container.Padding(8).Column(col =>
            {
                foreach (var row in _header.Rows)
                {
                    col.Item().Row(r =>
                    {
                        foreach (var colDef in row.Columns)
                        {
                            var width = ParseWidth(colDef.Width);

                            r.RelativeColumn(width).PaddingRight(4).AlignMiddle().Element(x =>
                            {
                                var content = colDef.Content;
                                // content["type"] should exist
                                if (!content.TryGetValue("type", out var ct)) return;

                                var type = ct?.ToString();
                                switch (type)
                                {
                                    case "text":
                                        {
                                            var text = content.TryGetValue("value", out var v) ? v?.ToString() : string.Empty;
                                            var style = content.TryGetValue("style", out var s) ? s as JsonObjectLike : null;
                                            // Simple mapping: center alignment + font size + bold
                                            int fontSize = 12;
                                            if (style?.Get("fontSize") is not null && int.TryParse(style.Get("fontSize").ToString(), out var parsed))
                                                fontSize = parsed;

                                            var textElement = x.AlignCenter().Text(text).FontSize(fontSize);
                                            if (ParseBool(style?.Get("fontWeight") == "bold"))
                                                textElement.SemiBold();

                                            break;
                                        }
                                    case "labelValue":
                                        {
                                            var label = content.TryGetValue("label", out var l) ? l?.ToString() : "";
                                            var value = content.TryGetValue("value", out var val) ? val?.ToString() : "";
                                            x.Column(c =>
                                            {
                                                c.Item().Row(r2 =>
                                                {
                                                    r2.ConstantColumn(120).Text(label).FontSize(10).SemiBold();
                                                    r2.RelativeColumn().Text(value).FontSize(10);
                                                });
                                            });
                                            break;
                                        }
                                    case "labelValueRight":
                                        {
                                            var label = content.TryGetValue("label", out var l2) ? l2?.ToString() : "";
                                            var value = content.TryGetValue("value", out var v2) ? v2?.ToString() : "";
                                            x.Column(c =>
                                            {
                                                c.Item().Row(r2 =>
                                                {
                                                    r2.RelativeColumn().AlignLeft().Text(label).FontSize(10).SemiBold();
                                                    r2.RelativeColumn().AlignRight().Text(value).FontSize(10);
                                                });
                                            });
                                            break;
                                        }
                                }
                            });
                        }
                    });
                }
            });
        }

        //private void ComposeContent(IContainer container)
        //{
        //    container.PaddingVertical(8).Column(col =>
        //    {
        //        if (_rows.Count == 0)
        //        {
        //            col.Item().Text("No data").Italic();
        //            return;
        //        }

        //        // Build header list from first row keys (or compute union of keys if needed)
        //        var headers = new List<string>(_rows[0].Keys);

        //        col.Item().Table(table =>
        //        {
        //            // Define columns (one relative column per header)
        //            table.ColumnsDefinition(columns =>
        //            {
        //                foreach (var _ in headers)
        //                {
        //                    columns.RelativeColumn();
        //                }
        //            });

        //            // Header row
        //            table.Header(header =>
        //            {
        //                foreach (var h in headers)
        //                {
        //                    header.Cell().Background(Colors.Grey.Lighten3).Padding(6).Text(h).SemiBold();
        //                }
        //            });

        //            // Body rows: for each row, emit one cell per header
        //            for (int rowIndex = 0; rowIndex < _rows.Count; rowIndex++)
        //            {
        //                var row = _rows[rowIndex];
        //                bool isEven = (rowIndex % 2 == 0);

        //                foreach (var h in headers)
        //                {
        //                    var textValue = row.TryGetValue(h, out var v) ? (v?.ToString() ?? "") : "";

        //                    var cell = table.Cell().Padding(6);

        //                    // alternating background color
        //                    if (!isEven)
        //                        cell.Background(Colors.Grey.Lighten4);

        //                    // basic alignment and wrapping
        //                    cell.Element(c => c.Text(textValue).FontSize(10).WrapAnywhere());
        //                }
        //            }
        //        });
        //    });
        //}

        private void ComposeContent(IContainer container)
        {
            // If a report template is provided, render based on it.
            if (_reportTemplate != null)
            {
                // Use the ReportTemplateRenderer (if you implemented it) to generate the body.
                // If you haven't created a separate renderer, you can implement similar logic inline.
                ReportTemplateRenderer.RenderReportBody(container, _reportTemplate, _rows);
                return;
            }

            // fallback to previous auto-table behavior if no template is provided
            container.PaddingVertical(8).Column(col =>
            {
                if (_rows.Count == 0)
                {
                    col.Item().Text("No data").Italic();
                    return;
                }

                var headers = new List<string>(_rows[0].Keys);

                col.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        foreach (var _ in headers)
                            columns.RelativeColumn();
                    });

                    table.Header(header =>
                    {
                        foreach (var h in headers)
                            header.Cell().Background(Colors.Grey.Lighten3).Padding(6).Text(h).SemiBold();
                    });

                    for (int rowIndex = 0; rowIndex < _rows.Count; rowIndex++)
                    {
                        var row = _rows[rowIndex];
                        bool isEven = (rowIndex % 2 == 0);

                        foreach (var h in headers)
                        {
                            var textValue = row.TryGetValue(h, out var v) ? (v?.ToString() ?? "") : "";

                            var cell = table.Cell().Padding(6);

                            if (!isEven)
                                cell.Background(Colors.Grey.Lighten4);

                            cell.Element(c => c.Text(textValue).FontSize(10).WrapAnywhere());
                        }
                    }
                });
            });
        }


        private void ComposeFooter(IContainer container)
        {
            if (_footer == null) return;

            container.Padding(8).Row(row =>
            {
                foreach (var r in _footer.Rows)
                {
                    foreach (var colDef in r.Columns)
                    {
                        var width = ParseWidth(colDef.Width);
                        row.RelativeColumn(width).AlignMiddle().Padding(2).Element(x =>
                        {
                            var content = colDef.Content;
                            if (!content.TryGetValue("type", out var ct)) return;
                            var type = ct?.ToString();
                            switch (type)
                            {
                                case "image":
                                    {
                                        var value = content.TryGetValue("value", out var v) ? v?.ToString() : null;
                                        // if company icon byte[] provided use that, else attempt base64 from template
                                        if (_companyIcon != null && _companyIcon.Length > 0)
                                        {
                                            x.Image(_companyIcon);
                                        }
                                        else if (!string.IsNullOrWhiteSpace(value))
                                        {
                                            try
                                            {
                                                var base64 = value.StartsWith("data:") ? value.Split(',')[1] : value;
                                                var bytes = Convert.FromBase64String(base64);
                                                x.Image(bytes).FitArea();
                                            }
                                            catch
                                            {
                                                // ignore image errors
                                            }
                                        }
                                        break;
                                    }
                                case "pageNumber":
                                    {
                                        // QuestPDF supports page numbers via container.PageNumber/TotalPages
                                        x.AlignRight().Text(txt =>
                                        {
                                            // insert inline page numbers
                                            txt.Span($"Page ").FontSize(9);
                                            txt.CurrentPageNumber();
                                            txt.Span(" of ").FontSize(9);
                                            txt.TotalPages();
                                        });
                                        break;
                                    }
                                case "spacer":
                                    {
                                        // nothing to draw
                                        break;
                                    }
                            }
                        });
                    }
                }
            });
        }

        // Utility helpers for parsing simple style-ish values
        private static int ParseWidth(string widthToken)
        {
            if (string.IsNullOrWhiteSpace(widthToken)) return 1;
            if (widthToken.EndsWith("%"))
            {
                // convert percent to relative scale: e.g., 50% -> 50
                if (int.TryParse(widthToken.TrimEnd('%'), out var p)) return p;
            }
            if (int.TryParse(widthToken, out var v)) return v;
            return 1;
        }

        private static int ParseInt(object value, int defaultValue)
        {
            if (value == null) return defaultValue;
            return int.TryParse(value.ToString(), out var result) ? result : defaultValue;
        }


        private static bool ParseBool(object val)
        {
            if (val == null) return false;
            if (val is bool b) return b;
            return string.Equals(val.ToString(), "true", StringComparison.OrdinalIgnoreCase);
        }
    }

    // small helper type to make style access easier (optional)
    internal class JsonObjectLike
    {
        private readonly Dictionary<string, object> _inner;
        public JsonObjectLike(Dictionary<string, object> inner) => _inner = inner ?? new Dictionary<string, object>();
        public object Get(string key) => _inner.TryGetValue(key, out var v) ? v : null;
    }
}
