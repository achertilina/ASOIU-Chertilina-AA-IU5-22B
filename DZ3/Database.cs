using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Homework3.Models;
public class Database
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } = "";
    [Required]
    public int SizeGb { get; set; }
    public int ServerId { get; set; }
    [ForeignKey("ServerId")]
    public virtual Server? Server { get; set; }
}
