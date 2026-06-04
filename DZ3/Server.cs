using System.ComponentModel.DataAnnotations;
namespace Homework3.Models;
public class Server
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } = "";
    public virtual ICollection<Database> Databases { get; set; } = new List<Database>();
    public override string ToString() => Name;
}
