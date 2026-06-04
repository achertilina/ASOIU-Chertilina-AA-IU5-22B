using System.Text;

namespace FinalHomework2;

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;

        string dbPath = "databases.db";
        string serverCsv = Path.Combine(AppContext.BaseDirectory, "Server.csv");
        string databaseCsv = Path.Combine(AppContext.BaseDirectory, "Database.csv");

        var db = new DatabaseManager(dbPath);
        db.InitializeDatabase();
        db.ImportFromCsv(serverCsv, databaseCsv);

        string choice;
        do
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════════════════╗");
            Console.WriteLine("║         УПРАВЛЕНИЕ БАЗАМИ ДАННЫХ             ║");
            Console.WriteLine("╠══════════════════════════════════════════════╣");
            Console.WriteLine("║ 1 — Показать все серверы                     ║");
            Console.WriteLine("║ 2 — Показать все базы данных                 ║");
            Console.WriteLine("║ 3 — Добавить базу данных                     ║");
            Console.WriteLine("║ 4 — Редактировать базу данных                ║");
            Console.WriteLine("║ 5 — Удалить базу данных                      ║");
            Console.WriteLine("║ 6 — Отчёты                                   ║");
            Console.WriteLine("║ 0 — Выход                                    ║");
            Console.WriteLine("╚══════════════════════════════════════════════╝");
            Console.Write("Ваш выбор: ");
            choice = Console.ReadLine()?.Trim() ?? "";

            switch (choice)
            {
                case "1": ShowServers(db); break;
                case "2": ShowDatabases(db); break;
                case "3": AddDatabase(db); break;
                case "4": EditDatabase(db); break;
                case "5": DeleteDatabase(db); break;
                case "6": ReportsMenu(db); break;
                case "0": Console.WriteLine("До свидания!"); break;
                default: Console.WriteLine("Неверный выбор!"); break;
            }
            if (choice != "0") { Console.WriteLine("\nНажмите любую клавишу..."); Console.ReadKey(); }
        } while (choice != "0");
    }

    static void ShowServers(DatabaseManager db)
    {
        Console.WriteLine("\n═══════════════ ВСЕ СЕРВЕРЫ ═══════════════");
        foreach (var s in db.GetAllServers()) Console.WriteLine($"  {s}");
        Console.WriteLine($"────────────────────────────────────────────");
        Console.WriteLine($"Итого: {db.GetAllServers().Count}");
    }

    static void ShowDatabases(DatabaseManager db)
    {
        Console.WriteLine("\n═══════════════ ВСЕ БАЗЫ ДАННЫХ ═══════════════");
        foreach (var d in db.GetAllDatabases()) Console.WriteLine($"  {d}");
        Console.WriteLine($"──────────────────────────────────────────────");
        Console.WriteLine($"Итого: {db.GetAllDatabases().Count}");
    }

    static void AddDatabase(DatabaseManager db)
    {
        Console.WriteLine("\n═══════════════ ДОБАВЛЕНИЕ БАЗЫ ДАННЫХ ═══════════════");
        Console.WriteLine("Доступные серверы:");
        foreach (var s in db.GetAllServers()) Console.WriteLine($"  {s}");

        Console.Write("\nID сервера: ");
        if (!int.TryParse(Console.ReadLine(), out int sid)) { Console.WriteLine("Ошибка!"); return; }

        Console.Write("Название: ");
        string name = Console.ReadLine()?.Trim() ?? "";
        if (string.IsNullOrEmpty(name)) { Console.WriteLine("Ошибка!"); return; }

        Console.Write("Размер (ГБ): ");
        if (!int.TryParse(Console.ReadLine(), out int size)) { Console.WriteLine("Ошибка!"); return; }

        try
        {
            db.AddDatabase(new Database(0, sid, name, size));
            Console.WriteLine("✅ Добавлено!");
        }
        catch (Exception ex) { Console.WriteLine($"Ошибка: {ex.Message}"); }
    }

    static void EditDatabase(DatabaseManager db)
    {
        Console.Write("\nВведите ID базы данных: ");
        if (!int.TryParse(Console.ReadLine(), out int id)) { Console.WriteLine("Ошибка!"); return; }

        var d = db.GetDatabaseById(id);
        if (d == null) { Console.WriteLine("Не найдено!"); return; }

        Console.WriteLine($"Текущие данные: {d}");
        Console.WriteLine("(Enter - оставить без изменений)");

        Console.Write($"Название [{d.Name}]: ");
        string input = Console.ReadLine()?.Trim() ?? "";
        if (!string.IsNullOrEmpty(input)) d.Name = input;

        Console.Write($"ID сервера [{d.ServerId}]: ");
        input = Console.ReadLine()?.Trim() ?? "";
        if (!string.IsNullOrEmpty(input) && int.TryParse(input, out int sid)) d.ServerId = sid;

        Console.Write($"Размер [{d.SizeGb}]: ");
        input = Console.ReadLine()?.Trim() ?? "";
        if (!string.IsNullOrEmpty(input) && int.TryParse(input, out int size))
        {
            try { d.SizeGb = size; }
            catch (Exception ex) { Console.WriteLine($"Ошибка: {ex.Message}"); return; }
        }

        db.UpdateDatabase(d);
        Console.WriteLine("✅ Обновлено!");
    }

    static void DeleteDatabase(DatabaseManager db)
    {
        Console.Write("\nВведите ID базы данных: ");
        if (!int.TryParse(Console.ReadLine(), out int id)) { Console.WriteLine("Ошибка!"); return; }

        var d = db.GetDatabaseById(id);
        if (d == null) { Console.WriteLine("Не найдено!"); return; }

        Console.Write($"Удалить '{d.Name}'? (да/нет): ");
        if (Console.ReadLine()?.Trim().ToLower() == "да")
        {
            db.DeleteDatabase(id);
            Console.WriteLine("✅ Удалено!");
        }
    }

    static void ReportsMenu(DatabaseManager db)
    {
        Console.WriteLine("\n═══════════════════ ОТЧЁТЫ ═══════════════════");
        Console.WriteLine("1 - Полный список БД с серверами");
        Console.WriteLine("2 - Количество БД по серверам");
        Console.WriteLine("3 - Средний размер БД по серверам");
        Console.WriteLine("0 - Назад");
        Console.Write("Выбор: ");
        string choice = Console.ReadLine()?.Trim() ?? "";

        switch (choice)
        {
            case "1":
                new ReportBuilder(db)
                    .Query("SELECT d.db_name, s.server_name, d.size_gb FROM database d JOIN server s ON d.server_id = s.server_id ORDER BY d.db_name")
                    .Title("БАЗЫ ДАННЫХ ПО СЕРВЕРАМ")
                    .Header("Название", "Сервер", "ГБ")
                    .ColumnWidths(25, 22, 10)
                    .Numbered()
                    .Print();
                break;
            case "2":
                new ReportBuilder(db)
                    .Query("SELECT s.server_name, COUNT(*) FROM database d JOIN server s ON d.server_id = s.server_id GROUP BY s.server_name ORDER BY s.server_name")
                    .Title("КОЛИЧЕСТВО БД ПО СЕРВЕРАМ")
                    .Header("Сервер", "Кол-во")
                    .ColumnWidths(25, 10)
                    .Numbered()
                    .Print();
                break;
            case "3":
                new ReportBuilder(db)
                    .Query("SELECT s.server_name, ROUND(AVG(d.size_gb),1) FROM database d JOIN server s ON d.server_id = s.server_id GROUP BY s.server_name ORDER BY AVG(d.size_gb) DESC")
                    .Title("СРЕДНИЙ РАЗМЕР БД ПО СЕРВЕРАМ")
                    .Header("Сервер", "Средний (ГБ)")
                    .ColumnWidths(25, 15)
                    .Numbered()
                    .Print();
                break;
        }
    }
}