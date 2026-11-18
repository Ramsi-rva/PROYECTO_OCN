using System.ComponentModel.DataAnnotations;

namespace OcnMvc.Models;

public class Problematica
{
    [Display(Name = "Id")]
    public int IdProblematica { get; set; }

    // Llave foránea → ocn.categoriaproblematica
    [Required(ErrorMessage = "Selecciona la categoría.")]
    [Display(Name = "Categoría")]
    public int IdCategoria { get; set; }

    // Llave foránea opcional → ocn.problematica (la misma tabla)
    [Display(Name = "Problemática padre")]
    public int? IdProblematicaPadre { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(200, ErrorMessage = "El nombre admite máximo {1} caracteres.")]
    public string Nombre { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "La descripción admite máximo {1} caracteres.")]
    [Display(Name = "Descripción")]
    public string? Descripcion { get; set; }

    public bool Activo { get; set; } = true;

    // Propiedades de navegación: EF las llena con .Include() (no son columnas).
    [Display(Name = "Categoría")]
    public CategoriaProblematica? Categoria { get; set; }

    [Display(Name = "Problemática padre")]
    public Problematica? ProblematicaPadre { get; set; }
}
