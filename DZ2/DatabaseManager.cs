using Microsoft.Data.Sqlite;

namespace FinalHomework2;

/// <summary>
/// Управление базой данных SQLite
/// </summary>
public class DatabaseManager
{
    private readonly string _connectionString;

    public DatabaseManager(string dbPath)
    {
        _connectionString = $"Data Source={dbPath}";
    }

    public void InitializeDatabase()
    {
        CreateTables();
    }

    private void CreateTables()
    {
        using var conn = new SqliteConnection(_connectionString);
        conn.Open();
        var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            CREATE TABLE IF NOT EXISTS server (
                server_id INTEGER PRIMARY KEY AUTOINCREMENT,
                server_name TEXT NOT NULL
            );
            CREATE TABLE IF NOT EXISTS database (
                db_id INTEGER PRIMARY KEY AUTOINCREMENT,
                server_id INTEGER NOT NULL,
                db_name TEXT NOT NULL,
                size_gb INTEGER NOT NULL,
                FOREIGN KEY (server_id) REFERENCES server(server_id)
            );";
        cmd.ExecuteNonQuery();
    }

    public void ImportFromCsv(string serverPath, string databasePath)
    {
        ImportServers(serverPath);
        ImportDatabases(databasePath);
    }

    private void ImportServers(string path)
    {
        if (!File.Exists(path)) return;

        using var conn = new SqliteConnection(_connectionString);
        conn.Open();

        var check = conn.CreateCommand();
        check.CommandText = "SELECT COUNT(*) FROM server";
        if ((long)check.ExecuteScalar()! > 0) return;

        var lines = File.ReadAllLines(path);
        for (int i = 1; i < lines.Length; i++)
        {
            var parts = lines[i].Split(';');
            if (parts.Length < 2) continue;

            var cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO server (server_id, server_name) VALUES (@id, @name)";
            cmd.Parameters.AddWithValue("@id", int.Parse(parts[0]));
            cmd.Parameters.AddWithValue("@name", parts[1]);
            cmd.ExecuteNonQuery();
        }
    }

    private void ImportDatabases(string path)
    {
        if (!File.Exists(path)) return;

        using var conn = new SqliteConnection(_connectionString);
        conn.Open();

        var check = conn.CreateCommand();
        check.CommandText = "SELECT COUNT(*) FROM database";
        if ((long)check.ExecuteScalar()! > 0) return;

        var lines = File.ReadAllLines(path);
        for (int i = 1; i < lines.Length; i++)
        {
            var parts = lines[i].Split(';');
            if (parts.Length < 4) continue;

            var cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO database (db_id, server_id, db_name, size_gb) VALUES (@id, @sid, @name, @size)";
            cmd.Parameters.AddWithValue("@id", int.Parse(parts[0]));
            cmd.Parameters.AddWithValue("@sid", int.Parse(parts[1]));
            cmd.Parameters.AddWithValue("@name", parts[2]);
            cmd.Parameters.AddWithValue("@size", int.Parse(parts[3]));
            cmd.ExecuteNonQuery();
        }
    }

    public List<Server> GetAllServers()
    {
        var result = new List<Server>();
        using var conn = new SqliteConnection(_connectionString);
        conn.Open();
        var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT server_id, server_name FROM server ORDER BY server_id";
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            result.Add(new Server(reader.GetInt32(0), reader.GetString(1)));
        }
        return result;
    }

    public List<Database> GetAllDatabases()
    {
        var result = new List<Database>();
        using var conn = new SqliteConnection(_connectionString);
        conn.Open();
        var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT db_id, server_id, db_name, size_gb FROM database ORDER BY db_id";
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            result.Add(new Database(reader.GetInt32(0), reader.GetInt32(1), reader.GetString(2), reader.GetInt32(3)));
        }
        return result;
    }

    public Database? GetDatabaseById(int id)
    {
        using var conn = new SqliteConnection(_connectionString);
        conn.Open();
        var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT db_id, server_id, db_name, size_gb FROM database WHERE db_id = @id";
        cmd.Parameters.AddWithValue("@id", id);
        using var reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            return new Database(reader.GetInt32(0), reader.GetInt32(1), reader.GetString(2), reader.GetInt32(3));
        }
        return null;
    }

    public void AddDatabase(Database db)
    {
        using var conn = new SqliteConnection(_connectionString);
        conn.Open();
        var cmd = conn.CreateCommand();
        cmd.CommandText = "INSERT INTO database (server_id, db_name, size_gb) VALUES (@sid, @name, @size)";
        cmd.Parameters.AddWithValue("@sid", db.ServerId);
        cmd.Parameters.AddWithValue("@name", db.Name);
        cmd.Parameters.AddWithValue("@size", db.SizeGb);
        cmd.ExecuteNonQuery();
    }

    public void UpdateDatabase(Database db)
    {
        using var conn = new SqliteConnection(_connectionString);
        conn.Open();
        var cmd = conn.CreateCommand();
        cmd.CommandText = "UPDATE database SET server_id = @sid, db_name = @name, size_gb = @size WHERE db_id = @id";
        cmd.Parameters.AddWithValue("@id", db.Id);
        cmd.Parameters.AddWithValue("@sid", db.ServerId);
        cmd.Parameters.AddWithValue("@name", db.Name);
        cmd.Parameters.AddWithValue("@size", db.SizeGb);
        cmd.ExecuteNonQuery();
    }

    public void DeleteDatabase(int id)
    {
        using var conn = new SqliteConnection(_connectionString);
        conn.Open();
        var cmd = conn.CreateCommand();
        cmd.CommandText = "DELETE FROM database WHERE db_id = @id";
        cmd.Parameters.AddWithValue("@id", id);
        cmd.ExecuteNonQuery();
    }

    public (string[] columns, List<string[]> rows) ExecuteQuery(string sql)
    {
        using var conn = new SqliteConnection(_connectionString);
        conn.Open();
        var cmd = conn.CreateCommand();
        cmd.CommandText = sql;
        using var reader = cmd.ExecuteReader();

        var cols = new string[reader.FieldCount];
        for (int i = 0; i < reader.FieldCount; i++) cols[i] = reader.GetName(i);

        var rows = new List<string[]>();
        while (reader.Read())
        {
            var row = new string[reader.FieldCount];
            for (int i = 0; i < reader.FieldCount; i++) row[i] = reader.GetValue(i)?.ToString() ?? "";
            rows.Add(row);
        }
        return (cols, rows);
    }
}