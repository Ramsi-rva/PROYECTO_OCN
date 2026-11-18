using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OcnMvc.Data;
using OcnMvc.Models;

namespace OcnMvc.Controllers;

// Controlador de la tabla ocn.evidencia
public class EvidenciaController : Controller
{
    private readonly OcnContext _context;

    public EvidenciaController(OcnContext context)
    {
        _context = context;
    }

    // Muestra la lista de registros
    public async Task<IActionResult> Index()
    {
        var lista = _context.Evidencia
            .Include(x => x.ObservacionProblematica)
            .Include(x => x.TipoEvidencia)
            .Include(x => x.Fuente)
            .AsQueryable();
        return View(await lista.ToListAsync());
    }

    // Muestra el detalle de un registro
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var item = await _context.Evidencia
            .Include(x => x.ObservacionProblematica)
            .Include(x => x.TipoEvidencia)
            .Include(x => x.Fuente)
            .FirstOrDefaultAsync(m => m.IdEvidencia == id);
        if (item == null) return NotFound();
        return View(item);
    }

    // Llena las listas desplegables de las llaves foráneas
    private void CargarListas(Evidencia? item = null)
    {
        ViewData["IdObservacionProblematica"] = new SelectList(_context.ObservacionProblematica, "IdObservacionProblematica", "IdObservacionProblematica", item?.IdObservacionProblematica);
        ViewData["IdTipoEvidencia"] = new SelectList(_context.CatTipoEvidencia.Where(x => x.Activo), "IdTipoEvidencia", "Nombre", item?.IdTipoEvidencia);
        ViewData["IdFuente"] = new SelectList(_context.Fuente.Where(x => x.Activo), "IdFuente", "Descripcion", item?.IdFuente);
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
    public async Task<IActionResult> Create([Bind("IdObservacionProblematica,IdTipoEvidencia,IdFuente,NumeroMenciones,DescripcionBreve,FechaRegistro")] Evidencia item)
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

        var item = await _context.Evidencia.FirstOrDefaultAsync(m => m.IdEvidencia == id);
        if (item == null) return NotFound();
        CargarListas(item);
        return View(item);
    }

    // Guarda los cambios del registro
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("IdEvidencia,IdObservacionProblematica,IdTipoEvidencia,IdFuente,NumeroMenciones,DescripcionBreve,FechaRegistro")] Evidencia item)
    {
        if (id != item.IdEvidencia) return NotFound();

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
                if (!await _context.Evidencia.AnyAsync(e => e.IdEvidencia == id)) return NotFound();
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

        var item = await _context.Evidencia
            .Include(x => x.ObservacionProblematica)
            .Include(x => x.TipoEvidencia)
            .Include(x => x.Fuente)
            .FirstOrDefaultAsync(m => m.IdEvidencia == id);
        if (item == null) return NotFound();
        return View(item);
    }

    // Elimina el registro
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var item = await _context.Evidencia.FirstOrDefaultAsync(m => m.IdEvidencia == id);
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
