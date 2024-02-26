using System.ComponentModel.DataAnnotations;

namespace WebShop.Models;

public class Category
{
    [Key]
    public int Id { get; set; } // Primary key
    [Required]
    public string Name { get; set; }
    public int DisplayOrder { get; set; }
}
