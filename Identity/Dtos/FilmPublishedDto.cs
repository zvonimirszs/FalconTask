using System.ComponentModel.DataAnnotations;

namespace Identity.Dtos;

public class FilmPublishedDto
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
    public string Event { get; set; }

}