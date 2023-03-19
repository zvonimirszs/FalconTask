using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace DataService.Models;

public class Direktor
{
    [Key]
    [Required]
    public int Id { get; set; }
    [Required]
    public string Ime { get; set; }
    [Required]
    public string Prezime { get; set; }
    public string Adresa { get; set; }
}

