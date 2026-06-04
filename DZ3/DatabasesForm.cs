using Microsoft.EntityFrameworkCore;
using Homework3.Models;
using Homework3.Data;
namespace Homework3.Forms;
public partial class DatabasesForm : Form
{
    private AppDbContext _context;
    private DataGridView dgv;
    private ComboBox cmbServer;
    private TextBox txtName, txtSize;
    private Button btnAdd, btnEdit, btnDelete;
    private int? _selectedId;
    public DatabasesForm(AppDbContext context)
    {
        _context = context;
        this.Text = "Управление базами данных";
        this.Size = new Size(750, 600);
        this.StartPosition = FormStartPosition.CenterScreen;
        dgv = new DataGridView { Location = new Point(10, 10), Size = new Size(710, 320), SelectionMode = DataGridViewSelectionMode.FullRowSelect, AllowUserToAddRows = false };
        dgv.SelectionChanged += (s, e) => { if (dgv.SelectedRows.Count > 0) SelectRow(); };
        var lblServer = new Label { Text = "Сервер:", Location = new Point(10, 350), Size = new Size(80, 25) };
        cmbServer = new ComboBox { Location = new Point(100, 350), Size = new Size(200, 25), DropDownStyle = ComboBoxStyle.DropDownList };
        var lblName = new Label { Text = "Название БД:", Location = new Point(10, 385), Size = new Size(80, 25) };
        txtName = new TextBox { Location = new Point(100, 385), Size = new Size(200, 25) };
        var lblSize = new Label { Text = "Размер (ГБ):", Location = new Point(10, 420), Size = new Size(80, 25) };
        txtSize = new TextBox { Location = new Point(100, 420), Size = new Size(200, 25) };
        btnAdd = new Button { Text = "Добавить", Location = new Point(10, 460), Size = new Size(100, 40), BackColor = Color.LightGreen };
        btnEdit = new Button { Text = "Редактировать", Location = new Point(120, 460), Size = new Size(110, 40), Enabled = false };
        btnDelete = new Button { Text = "Удалить", Location = new Point(240, 460), Size = new Size(100, 40), Enabled = false, BackColor = Color.LightCoral };
        btnAdd.Click += (s, e) => Add();
        btnEdit.Click += (s, e) => Edit();
        btnDelete.Click += (s, e) => Delete();
        this.Controls.Add(dgv);
        this.Controls.Add(lblServer);
        this.Controls.Add(cmbServer);
        this.Controls.Add(lblName);
        this.Controls.Add(txtName);
        this.Controls.Add(lblSize);
        this.Controls.Add(txtSize);
        this.Controls.Add(btnAdd);
        this.Controls.Add(btnEdit);
        this.Controls.Add(btnDelete);
        LoadServers();
        LoadData();
    }
    private void LoadServers() { var servers = _context.Servers.OrderBy(s => s.Name).ToList(); cmbServer.DataSource = servers; cmbServer.DisplayMember = "Name"; cmbServer.ValueMember = "Id"; }
    private void LoadData() { _context.ChangeTracker.Clear(); var databases = _context.Databases.Include(d => d.Server).OrderBy(d => d.Name).ToList(); dgv.DataSource = null; dgv.DataSource = databases.Select(d => new { d.Id, Сервер = d.Server?.Name ?? "", d.Name, Размер_ГБ = d.SizeGb }).ToList(); dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; }
    private void SelectRow() { if (dgv.SelectedRows.Count > 0) { _selectedId = (int)dgv.SelectedRows[0].Cells["Id"].Value; var db = _context.Databases.Include(d => d.Server).FirstOrDefault(d => d.Id == _selectedId); if (db != null) { cmbServer.SelectedValue = db.ServerId; txtName.Text = db.Name; txtSize.Text = db.SizeGb.ToString(); btnEdit.Enabled = true; btnDelete.Enabled = true; } } }
    private void Add() { if (cmbServer.SelectedItem == null || string.IsNullOrWhiteSpace(txtName.Text)) { MessageBox.Show("Заполните поля!"); return; } if (!int.TryParse(txtSize.Text, out int size) || size < 0) { MessageBox.Show("Размер >= 0!"); return; } _context.Databases.Add(new Database { ServerId = (int)cmbServer.SelectedValue, Name = txtName.Text, SizeGb = size }); _context.SaveChanges(); LoadData(); ClearForm(); MessageBox.Show("БД добавлена!"); }
    private void Edit() { if (_selectedId == null) return; var db = _context.Databases.Find(_selectedId); if (db != null) { db.ServerId = (int)cmbServer.SelectedValue; db.Name = txtName.Text; db.SizeGb = int.Parse(txtSize.Text); _context.SaveChanges(); LoadData(); ClearForm(); btnEdit.Enabled = false; btnDelete.Enabled = false; _selectedId = null; MessageBox.Show("БД обновлена!"); } }
    private void Delete() { if (_selectedId == null) return; if (MessageBox.Show("Удалить?", "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.Yes) { var db = _context.Databases.Find(_selectedId); if (db != null) { _context.Databases.Remove(db); _context.SaveChanges(); LoadData(); ClearForm(); btnEdit.Enabled = false; btnDelete.Enabled = false; _selectedId = null; MessageBox.Show("БД удалена!"); } } }
    private void ClearForm() { cmbServer.SelectedIndex = -1; txtName.Clear(); txtSize.Clear(); }
}
