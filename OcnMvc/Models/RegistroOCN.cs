using System.ComponentModel.DataAnnotations;

namespace OcnMvc.Models;

public class RegistroOCN
{
    [Display(Name = "Id")]
    public int IdRegistroOCN { get; set; }

    // Llave foránea → ocn.zona
    [Required(ErrorMessage = "Selecciona la zona.")]
    [Display(Name = "Zona")]
    public int IdZona { get; set; }

    // Llave foránea → ocn.problematica
    [Required(ErrorMessage = "Selecciona la problemática.")]
    [Display(Name = "Problemática")]
    public int IdProblematica { get; set; }

    [StringLength(500, ErrorMessage = "La descripción admite máximo {1} caracteres.")]
    [Display(Name = "Descripción")]
    public string? Descripcion { get; set; }

    // En la base tiene valor por omisión sysdatetime(); aquí se propone la fecha actual.
    [Required(ErrorMessage = "La fecha de registro es obligatoria.")]
    [Display(Name = "Fecha de registro")]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
    public DateTime FechaRegistro { get; set; } = DateTime.Now;

    public bool Activo { get; set; } = true;

    // Propiedades de navegación: EF las llena con .Include() (no son columnas).
    [Display(Name = "Zona")]
    public Zona? Zona { get; set; }

    [Display(Name = "Problemática")]
    public Problematica? Problematica { get; set; }
}
