using System.ComponentModel.DataAnnotations;

namespace DataService.Dtos;

public class FilmCreateDto
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
    public int DirektorId { get; set; }
    public ICollection<int> Zanrovi { get; set; } = new List<int>();
    public ICollection<int> Glumci { get; set; } = new List<int>();

}