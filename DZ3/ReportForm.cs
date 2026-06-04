using Microsoft.EntityFrameworkCore;
using Homework3.Data;
namespace Homework3.Forms;
public partial class ReportForm : Form
{
    private AppDbContext _context;
    private TabControl tabControl;
    public ReportForm(AppDbContext context)
    {
        _context = context;
        this.Text = "Отчёты";
        this.Size = new Size(850, 600);
        this.StartPosition = FormStartPosition.CenterScreen;
        tabControl = new TabControl { Dock = DockStyle.Fill };
        tabControl.TabPages.Add("Полный список БД");
        tabControl.TabPages.Add("Количество БД по серверам");
        tabControl.TabPages.Add("Средний размер БД по серверам");
        this.Controls.Add(tabControl);
        LoadReports();
    }
    private void LoadReports()
    {
        var report1 = _context.Databases.Include(d => d.Server).OrderBy(d => d.Name).Select(d => new { d.Name, Сервер = d.Server != null ? d.Server.Name : "", Размер_ГБ = d.SizeGb }).ToList();
        var dgv1 = new DataGridView { Dock = DockStyle.Fill, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };
        dgv1.DataSource = report1;
        tabControl.TabPages[0].Controls.Add(dgv1);
        var report2 = _context.Databases.Include(d => d.Server).GroupBy(d => d.Server != null ? d.Server.Name : "Без сервера").Select(g => new { Сервер = g.Key, Количество = g.Count() }).OrderBy(r => r.Сервер).ToList();
        var dgv2 = new DataGridView { Dock = DockStyle.Fill, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };
        dgv2.DataSource = report2;
        tabControl.TabPages[1].Controls.Add(dgv2);
        var report3 = _context.Databases.Include(d => d.Server).GroupBy(d => d.Server != null ? d.Server.Name : "Без сервера").Select(g => new { Сервер = g.Key, Средний_размер_ГБ = g.Average(d => d.SizeGb) }).OrderByDescending(r => r.Средний_размер_ГБ).ToList();
        var dgv3 = new DataGridView { Dock = DockStyle.Fill, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };
        dgv3.DataSource = report3;
        tabControl.TabPages[2].Controls.Add(dgv3);
    }
}
