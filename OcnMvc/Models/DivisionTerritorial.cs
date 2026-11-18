using System.ComponentModel.DataAnnotations;

namespace OcnMvc.Models;

public class DivisionTerritorial
{
    [Display(Name = "Id")]
    public int IdDivision { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(150, ErrorMessage = "El nombre admite máximo {1} caracteres.")]
    public string Nombre { get; set; } = string.Empty;

    [StringLength(60, ErrorMessage = "El tipo admite máximo {1} caracteres.")]
    public string? Tipo { get; set; }

    public bool Activo { get; set; } = true;
}
