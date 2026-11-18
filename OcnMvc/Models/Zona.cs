using System.ComponentModel.DataAnnotations;

namespace OcnMvc.Models;

public class Zona
{
    [Display(Name = "Id")]
    public int IdZona { get; set; }

    [Required(ErrorMessage = "El código de zona es obligatorio.")]
    [StringLength(30, ErrorMessage = "El código admite máximo {1} caracteres.")]
    [Display(Name = "Código de zona")]
    public string CodigoZona { get; set; } = string.Empty;

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(150, ErrorMessage = "El nombre admite máximo {1} caracteres.")]
    public string Nombre { get; set; } = string.Empty;

    // Llave foránea → ocn.divisionterritorial
    [Required(ErrorMessage = "Selecciona la división territorial.")]
    [Display(Name = "División territorial")]
    public int IdDivision { get; set; }

    // Llave foránea → ocn.cattipozona
    [Required(ErrorMessage = "Selecciona el tipo de zona.")]
    [Display(Name = "Tipo de zona")]
    public int IdTipoZona { get; set; }

    [StringLength(500, ErrorMessage = "La descripción admite máximo {1} caracteres.")]
    [Display(Name = "Descripción general")]
    public string? DescripcionGeneral { get; set; }

    [StringLength(200, ErrorMessage = "La referencia admite máximo {1} caracteres.")]
    [Display(Name = "Referencia geográfica aprox.")]
    public string? ReferenciaGeograficaAprox { get; set; }

    public bool Activo { get; set; } = true;

    // Propiedades de navegación: EF las llena con .Include() (no son columnas).
    [Display(Name = "División territorial")]
    public DivisionTerritorial? Division { get; set; }

    [Display(Name = "Tipo de zona")]
    public CatTipoZona? TipoZona { get; set; }
}
