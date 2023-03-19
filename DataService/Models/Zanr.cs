using System.ComponentModel.DataAnnotations;

namespace DataService.Models;

public class Zanr
{
    [Key]
    [Required]
    public int Id { get; set; }
    [Required]
    public string Naziv { get; set; }
    public string Opis { get; set; }
}