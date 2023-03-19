using DataService.Models;
using System.ComponentModel.DataAnnotations;

namespace DataService.Dtos;

public class FilmReadDto
{
    [Key]
    [Required]
    public int Id { get; set; }
    [Required]   
    public string Naziv { get; set; }
    public string Opis { get; set; }
    public double Budzet { get; set; }
    public DateTime? PocetakSnimanja { get; set; }
    public DateTime? KrajSnimanja { get; set; }
    [Required] 
    public Direktor Direktor { get; set; }
    public ICollection<Zanr> Zanrovi { get; set; } = new List<Zanr>();
    public ICollection<Glumac> Glumci { get; set; } = new List<Glumac>();

}