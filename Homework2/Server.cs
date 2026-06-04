namespace FinalHomework2;

/// <summary>
/// Сервер (справочная таблица, сторона «один»)
/// </summary>
public class Server
{
    public int Id { get; set; }
    public string Name { get; set; } = "";

    public Server() { }

    public Server(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public override string ToString() => $"[{Id}] {Name}";
}