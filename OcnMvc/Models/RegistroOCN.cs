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

    // Escala del 1 (muy baja) al 5 (muy alta). En la base: tinyint con check 1-5.
    [Required(ErrorMessage = "Selecciona la intensidad.")]
    [Range(1, 5, ErrorMessage = "La intensidad debe estar entre {1} y {2}.")]
    [Display(Name = "Intensidad")]
    public byte Intensidad { get; set; } = 1;

    // Valores permitidos por el check de la base: Ocasional, Frecuente, Constante, Primera Aparición.
    [Required(ErrorMessage = "Selecciona la frecuencia.")]
    [RegularExpression("^(Ocasional|Frecuente|Constante|Primera Aparición)$", ErrorMessage = "Frecuencia no válida.")]
    [Display(Name = "Frecuencia")]
    public string Frecuencia { get; set; } = "Ocasional";

    // Opciones para las listas desplegables de las vistas.
    public static readonly byte[] Intensidades = { 1, 2, 3, 4, 5 };
    public static readonly string[] Frecuencias = { "Ocasional", "Frecuente", "Constante", "Primera Aparición" };

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
