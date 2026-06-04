using System.Text;

namespace Homework2;

class ReportBuilder
{
    private DatabaseManager _db;
    private string _sql = "", _title = "";
    private string[] _headers = Array.Empty<string>();
    private int[] _widths = Array.Empty<int>();
    private bool _numbered = false;

    public ReportBuilder(DatabaseManager db) => _db = db;
    public ReportBuilder Query(string sql) { _sql = sql; return this; }
    public ReportBuilder Title(string title) { _title = title; return this; }
    public ReportBuilder Header(params string[] cols) { _headers = cols; return this; }
    public ReportBuilder ColumnWidths(params int[] widths) { _widths = widths; return this; }
    public ReportBuilder Numbered() { _numbered = true; return this; }

    public string Build()
    {
        var (cols, rows) = _db.ExecuteQuery(_sql);
        var sb = new StringBuilder();
        if (_title.Length > 0) sb.AppendLine($"\n=== {_title} ===");
        string[] headers = _headers.Length > 0 ? _headers : cols;
        int colCount = headers.Length;
        int[] widths = _widths.Length >= colCount ? _widths : Enumerable.Repeat(20, colCount).ToArray();
        int numWidth = _numbered ? 6 : 0;

        if (_numbered) sb.Append("№".PadRight(numWidth));
        for (int i = 0; i < colCount; i++) sb.Append(headers[i].PadRight(widths[i]));
        sb.AppendLine();
        int total = numWidth + widths.Sum();
        sb.AppendLine(new string('-', total));
        for (int r = 0; r < rows.Count; r++)
        {
            if (_numbered) sb.Append((r + 1).ToString().PadRight(numWidth));
            for (int c = 0; c < rows[r].Length && c < colCount; c++) sb.Append(rows[r][c].PadRight(widths[c]));
            sb.AppendLine();
        }
        return sb.ToString();
    }

    public void Print() => Console.WriteLine(Build());
}