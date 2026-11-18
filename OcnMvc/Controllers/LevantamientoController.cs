using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OcnMvc.Data;
using OcnMvc.Models;

namespace OcnMvc.Controllers;

// Controlador de la tabla ocn.levantamiento
public class LevantamientoController : Controller
{
    private readonly OcnContext _context;

    public LevantamientoController(OcnContext context)
    {
        _context = context;
    }

    // Muestra la lista de registros
    public async Task<IActionResult> Index()
    {
        var lista = _context.Levantamiento
            .Include(x => x.Zona)
            .Include(x => x.Periodo)
            .Include(x => x.Metodo)
            .AsQueryable();
        return View(await lista.ToListAsync());
    }

    // Muestra el detalle de un registro
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var item = await _context.Levantamiento
            .Include(x => x.Zona)
            .Include(x => x.Periodo)
            .Include(x => x.Metodo)
            .FirstOrDefaultAsync(m => m.IdLevantamiento == id);
        if (item == null) return NotFound();
        return View(item);
    }

    // Llena las listas desplegables de las llaves foráneas
    private void CargarListas(Levantamiento? item = null)
    {
        ViewData["IdZona"] = new SelectList(_context.Zona.Where(x => x.Activo), "IdZona", "Nombre", item?.IdZona);
        ViewData["IdPeriodo"] = new SelectList(_context.Periodo.Where(x => x.Activo), "IdPeriodo", "Nombre", item?.IdPeriodo);
        ViewData["IdMetodo"] = new SelectList(_context.CatMetodo.Where(x => x.Activo), "IdMetodo", "Nombre", item?.IdMetodo);
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
    public async Task<IActionResult> Create([Bind("CodigoLevantamiento,IdZona,IdPeriodo,IdMetodo,FechaLevantamiento,NumeroParticipantesAprox,ObservacionesMetodologicas,FechaCaptura")] Levantamiento item)
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

        var item = await _context.Levantamiento.FirstOrDefaultAsync(m => m.IdLevantamiento == id);
        if (item == null) return NotFound();
        CargarListas(item);
        return View(item);
    }

    // Guarda los cambios del registro
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("IdLevantamiento,CodigoLevantamiento,IdZona,IdPeriodo,IdMetodo,FechaLevantamiento,NumeroParticipantesAprox,ObservacionesMetodologicas,FechaCaptura")] Levantamiento item)
    {
        if (id != item.IdLevantamiento) return NotFound();

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
                if (!await _context.Levantamiento.AnyAsync(e => e.IdLevantamiento == id)) return NotFound();
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

        var item = await _context.Levantamiento
            .Include(x => x.Zona)
            .Include(x => x.Periodo)
            .Include(x => x.Metodo)
            .FirstOrDefaultAsync(m => m.IdLevantamiento == id);
        if (item == null) return NotFound();
        return View(item);
    }

    // Elimina el registro
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var item = await _context.Levantamiento.FirstOrDefaultAsync(m => m.IdLevantamiento == id);
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
