using System;

namespace FinalHomework2;

/// <summary>
/// База данных (основная таблица, сторона «много»)
/// </summary>
public class Database
{
    public int Id { get; set; }
    public int ServerId { get; set; }
    public string Name { get; set; } = "";

    private int _sizeGb;

    public int SizeGb
    {
        get => _sizeGb;
        set
        {
            if (value < 0)
                throw new ArgumentException("Размер базы данных не может быть отрицательным!");
            _sizeGb = value;
        }
    }

    public Database() { }

    public Database(int id, int serverId, string name, int sizeGb)
    {
        Id = id;
        ServerId = serverId;
        Name = name;
        SizeGb = sizeGb;
    }

    public override string ToString() => $"[{Id}] {Name}, сервер #{ServerId}, размер: {SizeGb} ГБ";
}