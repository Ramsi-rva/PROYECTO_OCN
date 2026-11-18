using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using OcnMvc.Datos;
using OcnMvc.Models;

namespace OcnMvc.Controllers;

// Controlador de la tabla ocn.registroocn
public class RegistrosOCNController : Controller
{
    private readonly RegistroOCNDatos _datos;
    private readonly ZonaDatos _zonas;
    private readonly ProblematicaDatos _problematicas;

    public RegistrosOCNController(RegistroOCNDatos datos, ZonaDatos zonas, ProblematicaDatos problematicas)
    {
        _datos = datos;
        _zonas = zonas;
        _problematicas = problematicas;
    }

    // Muestra la lista de registros
    public IActionResult Index()
    {
        return View(_datos.Listar());
    }

    // Muestra el detalle de un registro
    public IActionResult Details(int? id)
    {
        if (id == null) return NotFound();

        var item = _datos.Buscar(id.Value);
        if (item == null) return NotFound();
        return View(item);
    }

    // Llena las listas desplegables de las llaves foráneas
    private void CargarListas(RegistroOCN? item = null)
    {
        ViewData["IdZona"] = new SelectList(_zonas.Listar(), "IdZona", "Nombre", item?.IdZona);
        ViewData["IdProblematica"] = new SelectList(_problematicas.Listar(), "IdProblematica", "Nombre", item?.IdProblematica);
        ViewData["Intensidad"] = new SelectList(RegistroOCN.Intensidades, item?.Intensidad ?? 1);
        ViewData["Frecuencia"] = new SelectList(RegistroOCN.Frecuencias, item?.Frecuencia ?? "Ocasional");
    }

    // Muestra el formulario para agregar
    public IActionResult Create()
    {
        CargarListas();
        return View();
    }

    // Guarda el registro nuevo
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create([Bind("IdZona,IdProblematica,Descripcion,Intensidad,Frecuencia,FechaRegistro")] RegistroOCN item)
    {
        if (ModelState.IsValid)
        {
            try
            {
                _datos.Insertar(item);
                TempData["Mensaje"] = "Registro creado.";
                return RedirectToAction(nameof(Index));
            }
            catch (SqlException ex) when (EsErrorDeNegocio(ex))
            {
                // Mensaje que lanzó el procedimiento almacenado (throw)
                ModelState.AddModelError(string.Empty, ex.Message);
            }
        }
        CargarListas(item);
        return View(item);
    }

    // Muestra el formulario para editar
    public IActionResult Edit(int? id)
    {
        if (id == null) return NotFound();

        var item = _datos.Buscar(id.Value);
        if (item == null) return NotFound();
        CargarListas(item);
        return View(item);
    }

    // Guarda los cambios del registro
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, [Bind("IdRegistroOCN,IdZona,IdProblematica,Descripcion,Intensidad,Frecuencia,FechaRegistro")] RegistroOCN item)
    {
        if (id != item.IdRegistroOCN) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _datos.Actualizar(item);
                TempData["Mensaje"] = "Registro actualizado.";
                return RedirectToAction(nameof(Index));
            }
            catch (SqlException ex) when (EsErrorDeNegocio(ex))
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
        }
        CargarListas(item);
        return View(item);
    }

    // Muestra la confirmación para eliminar
    public IActionResult Delete(int? id)
    {
        if (id == null) return NotFound();

        var item = _datos.Buscar(id.Value);
        if (item == null) return NotFound();
        return View(item);
    }

    // Da de baja el registro (borrado lógico)
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        try
        {
            _datos.Eliminar(id);
            TempData["Mensaje"] = "Registro dado de baja.";
        }
        catch (SqlException ex) when (EsErrorDeNegocio(ex))
        {
            TempData["Mensaje"] = ex.Message;
        }
        return RedirectToAction(nameof(Index));
    }

    // Los errores 50001 a 50004 los lanzan los procedimientos con throw
    private static bool EsErrorDeNegocio(SqlException ex)
    {
        return ex.Number >= 50001 && ex.Number <= 50004;
    }
}
