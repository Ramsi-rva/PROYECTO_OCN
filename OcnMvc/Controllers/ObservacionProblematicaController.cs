using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OcnMvc.Data;
using OcnMvc.Models;

namespace OcnMvc.Controllers;

// Controlador de la tabla ocn.observacionproblematica
public class ObservacionProblematicaController : Controller
{
    private readonly OcnContext _context;

    public ObservacionProblematicaController(OcnContext context)
    {
        _context = context;
    }

    // Muestra la lista de registros
    public async Task<IActionResult> Index()
    {
        var lista = _context.ObservacionProblematica
            .Include(x => x.Observacion)
            .Include(x => x.Problematica)
            .Include(x => x.Magnitud)
            .Include(x => x.Gravedad)
            .Include(x => x.Frecuencia)
            .AsQueryable();
        return View(await lista.ToListAsync());
    }

    // Muestra el detalle de un registro
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var item = await _context.ObservacionProblematica
            .Include(x => x.Observacion)
            .Include(x => x.Problematica)
            .Include(x => x.Magnitud)
            .Include(x => x.Gravedad)
            .Include(x => x.Frecuencia)
            .FirstOrDefaultAsync(m => m.IdObservacionProblematica == id);
        if (item == null) return NotFound();
        return View(item);
    }

    // Llena las listas desplegables de las llaves foráneas
    private void CargarListas(ObservacionProblematica? item = null)
    {
        ViewData["IdObservacion"] = new SelectList(_context.Observacion, "IdObservacion", "NotaContexto", item?.IdObservacion);
        ViewData["IdProblematica"] = new SelectList(_context.Problematica.Where(x => x.Activo), "IdProblematica", "Nombre", item?.IdProblematica);
        ViewData["IdMagnitud"] = new SelectList(_context.CatMagnitud.Where(x => x.Activo), "IdMagnitud", "Nombre", item?.IdMagnitud);
        ViewData["IdGravedad"] = new SelectList(_context.CatGravedad.Where(x => x.Activo), "IdGravedad", "Nombre", item?.IdGravedad);
        ViewData["IdFrecuencia"] = new SelectList(_context.CatFrecuencia.Where(x => x.Activo), "IdFrecuencia", "Nombre", item?.IdFrecuencia);
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
    public async Task<IActionResult> Create([Bind("IdObservacion,IdProblematica,IdMagnitud,IdGravedad,IdFrecuencia,NivelConfianza")] ObservacionProblematica item)
    {
        if (ModelState.IsValid)
        {
            try
            {
                _context.Add(item);
                await _context.SaveChangesAsync();
                TempData["Mensaje"] = "Registro creado.";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                // error de la base: llave repetida, referencia inválida, etc.
                ModelState.AddModelError(string.Empty, "No se pudo guardar: " + (ex.InnerException?.Message ?? ex.Message));
            }
        }
        CargarListas(item);
        return View(item);
    }

    // Muestra el formulario para modificar
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var item = await _context.ObservacionProblematica.FirstOrDefaultAsync(m => m.IdObservacionProblematica == id);
        if (item == null) return NotFound();
        CargarListas(item);
        return View(item);
    }

    // Guarda los cambios del registro
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("IdObservacionProblematica,IdObservacion,IdProblematica,IdMagnitud,IdGravedad,IdFrecuencia,NivelConfianza")] ObservacionProblematica item)
    {
        if (id != item.IdObservacionProblematica) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(item);
                await _context.SaveChangesAsync();
                TempData["Mensaje"] = "Registro actualizado.";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.ObservacionProblematica.AnyAsync(e => e.IdObservacionProblematica == id)) return NotFound();
                throw;
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError(string.Empty, "No se pudo guardar: " + (ex.InnerException?.Message ?? ex.Message));
            }
        }
        CargarListas(item);
        return View(item);
    }

    // Pide confirmación para eliminar
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var item = await _context.ObservacionProblematica
            .Include(x => x.Observacion)
            .Include(x => x.Problematica)
            .Include(x => x.Magnitud)
            .Include(x => x.Gravedad)
            .Include(x => x.Frecuencia)
            .FirstOrDefaultAsync(m => m.IdObservacionProblematica == id);
        if (item == null) return NotFound();
        return View(item);
    }

    // Elimina el registro
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var item = await _context.ObservacionProblematica.FirstOrDefaultAsync(m => m.IdObservacionProblematica == id);
        if (item != null)
        {
            try
            {
                _context.Remove(item);
                await _context.SaveChangesAsync();
                TempData["Mensaje"] = "Registro eliminado.";
            }
            catch (DbUpdateException ex)
            {
                TempData["Error"] = "No se pudo eliminar: " + (ex.InnerException?.Message ?? ex.Message);
            }
        }
        return RedirectToAction(nameof(Index));
    }
}
