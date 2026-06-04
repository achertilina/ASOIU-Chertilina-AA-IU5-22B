using Microsoft.EntityFrameworkCore;
using Homework3.Models;
using Homework3.Data;
namespace Homework3.Forms;
public partial class ServersForm : Form
{
    private AppDbContext _context;
    private DataGridView dgv;
    private TextBox txtName;
    private Button btnAdd, btnEdit, btnDelete;
    private int? _selectedId;
    public ServersForm(AppDbContext context)
    {
        _context = context;
        this.Text = "Управление серверами";
        this.Size = new Size(600, 500);
        this.StartPosition = FormStartPosition.CenterScreen;
        dgv = new DataGridView { Location = new Point(10, 10), Size = new Size(560, 300), SelectionMode = DataGridViewSelectionMode.FullRowSelect, AllowUserToAddRows = false };
        dgv.SelectionChanged += (s, e) => { if (dgv.SelectedRows.Count > 0) SelectRow(); };
        var lblName = new Label { Text = "Название сервера:", Location = new Point(10, 330), Size = new Size(120, 25) };
        txtName = new TextBox { Location = new Point(130, 330), Size = new Size(200, 25) };
        btnAdd = new Button { Text = "Добавить", Location = new Point(10, 370), Size = new Size(100, 35), BackColor = Color.LightGreen };
        btnEdit = new Button { Text = "Редактировать", Location = new Point(120, 370), Size = new Size(100, 35), Enabled = false };
        btnDelete = new Button { Text = "Удалить", Location = new Point(230, 370), Size = new Size(100, 35), Enabled = false, BackColor = Color.LightCoral };
        btnAdd.Click += (s, e) => Add();
        btnEdit.Click += (s, e) => Edit();
        btnDelete.Click += (s, e) => Delete();
        this.Controls.Add(dgv);
        this.Controls.Add(lblName);
        this.Controls.Add(txtName);
        this.Controls.Add(btnAdd);
        this.Controls.Add(btnEdit);
        this.Controls.Add(btnDelete);
        LoadData();
    }
    private void LoadData()
    {
        _context.ChangeTracker.Clear();
        var servers = _context.Servers.OrderBy(s => s.Name).ToList();
        dgv.DataSource = null;
        dgv.DataSource = servers.Select(s => new { s.Id, s.Name }).ToList();
        dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
    }
    private void SelectRow()
    {
        if (dgv.SelectedRows.Count > 0)
        {
            _selectedId = (int)dgv.SelectedRows[0].Cells["Id"].Value;
            var server = _context.Servers.Find(_selectedId);
            txtName.Text = server?.Name;
            btnEdit.Enabled = true;
            btnDelete.Enabled = true;
        }
    }
    private void Add()
    {
        if (string.IsNullOrWhiteSpace(txtName.Text)) { MessageBox.Show("Введите название!"); return; }
        _context.Servers.Add(new Server { Name = txtName.Text });
        _context.SaveChanges();
        LoadData();
        txtName.Clear();
        MessageBox.Show("Сервер добавлен!");
    }
    private void Edit()
    {
        if (_selectedId == null) return;
        var server = _context.Servers.Find(_selectedId);
        if (server != null) { server.Name = txtName.Text; _context.SaveChanges(); LoadData(); txtName.Clear(); btnEdit.Enabled = false; btnDelete.Enabled = false; _selectedId = null; MessageBox.Show("Сервер обновлён!"); }
    }
    private void Delete()
    {
        if (_selectedId == null) return;
        if (_context.Databases.Any(d => d.ServerId == _selectedId)) { MessageBox.Show("Нельзя удалить! Есть связанные БД!"); return; }
        if (MessageBox.Show("Удалить?", "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
        { _context.Servers.Remove(_context.Servers.Find(_selectedId)); _context.SaveChanges(); LoadData(); txtName.Clear(); btnEdit.Enabled = false; btnDelete.Enabled = false; _selectedId = null; MessageBox.Show("Сервер удалён!"); }
    }
}
