using System.ComponentModel.DataAnnotations;

namespace APBD6.Models;

public class Animal
{
    [Required]
    public int idAnimal { get; set; }
    [Required]
    public string name { get; set; }
    public string description { get; set; }
    [Required]
    public string category { get; set; }
    [Required]
    public string area { get; set; }
}