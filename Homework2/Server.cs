namespace Homework2;

class Server
{
    public int Id { get; set; }
    public string Name { get; set; }
    public Server(int id, string name) { Id = id; Name = name; }
    public Server() : this(0, "") { }
    public override string ToString() => $"[{Id}] {Name}";
}