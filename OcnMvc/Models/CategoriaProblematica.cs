using System.ComponentModel.DataAnnotations;

namespace OcnMvc.Models;

public class CategoriaProblematica
{
    [Display(Name = "Id")]
    public int IdCategoria { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100, ErrorMessage = "El nombre admite máximo {1} caracteres.")]
    public string Nombre { get; set; } = string.Empty;

    public bool Activo { get; set; } = true;
}
