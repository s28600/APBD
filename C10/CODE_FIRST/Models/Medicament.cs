using System.ComponentModel.DataAnnotations;

namespace CODE_FIRST.Models;

public class Medicament
{
    [Key]
    public int IdMedicament { get; set; }
    
    [MaxLength(100)]
    [Required]
    public string Name { get; set; }
    
    [MaxLength(100)]
    [Required]
    public string Description { get; set; }
    
    [MaxLength(100)]
    [Required]
    public string Type { get; set; }
}