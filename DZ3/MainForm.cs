using Homework3.Data;
namespace Homework3.Forms;
public partial class MainForm : Form
{
    private AppDbContext _context;
    public MainForm()
    {
        _context = new AppDbContext();
        _context.Database.EnsureCreated();
        this.Text = "Управление серверами и базами данных";
        this.Size = new Size(500, 350);
        this.StartPosition = FormStartPosition.CenterScreen;
        var btnServers = new Button { Text = "📁 Серверы", Location = new Point(100, 50), Size = new Size(280, 45), Font = new Font("Segoe UI", 10, FontStyle.Bold) };
        var btnDatabases = new Button { Text = "💾 Базы данных", Location = new Point(100, 120), Size = new Size(280, 45), Font = new Font("Segoe UI", 10, FontStyle.Bold) };
        var btnReport = new Button { Text = "📊 Отчёты", Location = new Point(100, 190), Size = new Size(280, 45), Font = new Font("Segoe UI", 10, FontStyle.Bold) };
        btnServers.Click += (s, e) => { new ServersForm(_context).ShowDialog(); };
        btnDatabases.Click += (s, e) => { new DatabasesForm(_context).ShowDialog(); };
        btnReport.Click += (s, e) => { new ReportForm(_context).ShowDialog(); };
        this.Controls.Add(btnServers);
        this.Controls.Add(btnDatabases);
        this.Controls.Add(btnReport);
    }
}
