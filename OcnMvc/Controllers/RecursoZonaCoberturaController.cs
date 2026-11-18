using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OcnMvc.Data;
using OcnMvc.Models;

namespace OcnMvc.Controllers;

// Controlador de la tabla ocn.recursozonacobertura
public class RecursoZonaCoberturaController : Controller
{
    private readonly OcnContext _context;

    public RecursoZonaCoberturaController(OcnContext context)
    {
        _context = context;
    }

    // Muestra la lista de registros
    public async Task<IActionResult> Index()
    {
        var lista = _context.RecursoZonaCobertura
            .Include(x => x.Recurso)
            .Include(x => x.Zona)
            .Include(x => x.TipoCobertura)
            .AsQueryable();
        return View(await lista.ToListAsync());
    }

    // Muestra el detalle de un registro
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var item = await _context.RecursoZonaCobertura
            .Include(x => x.Recurso)
            .Include(x => x.Zona)
            .Include(x => x.TipoCobertura)
            .FirstOrDefaultAsync(m => m.IdRecursoZonaCobertura == id);
        if (item == null) return NotFound();
        return View(item);
    }

    // Llena las listas desplegables de las llaves foráneas
    private void CargarListas(RecursoZonaCobertura? item = null)
    {
        ViewData["IdRecurso"] = new SelectList(_context.Recurso.Where(x => x.Activo), "IdRecurso", "Nombre", item?.IdRecurso);
        ViewData["IdZona"] = new SelectList(_context.Zona.Where(x => x.Activo), "IdZona", "Nombre", item?.IdZona);
        ViewData["IdTipoCobertura"] = new SelectList(_context.CatTipoCobertura.Where(x => x.Activo), "IdTipoCobertura", "Nombre", item?.IdTipoCobertura);
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
    public async Task<IActionResult> Create([Bind("IdRecurso,IdZona,IdTipoCobertura,FechaInicio,FechaFin")] RecursoZonaCobertura item)
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

        var item = await _context.RecursoZonaCobertura.FirstOrDefaultAsync(m => m.IdRecursoZonaCobertura == id);
        if (item == null) return NotFound();
        CargarListas(item);
        return View(item);
    }

    // Guarda los cambios del registro
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("IdRecursoZonaCobertura,IdRecurso,IdZona,IdTipoCobertura,FechaInicio,FechaFin")] RecursoZonaCobertura item)
    {
        if (id != item.IdRecursoZonaCobertura) return NotFound();

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
                if (!await _context.RecursoZonaCobertura.AnyAsync(e => e.IdRecursoZonaCobertura == id)) return NotFound();
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

        var item = await _context.RecursoZonaCobertura
            .Include(x => x.Recurso)
            .Include(x => x.Zona)
            .Include(x => x.TipoCobertura)
            .FirstOrDefaultAsync(m => m.IdRecursoZonaCobertura == id);
        if (item == null) return NotFound();
        return View(item);
    }

    // Elimina el registro
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var item = await _context.RecursoZonaCobertura.FirstOrDefaultAsync(m => m.IdRecursoZonaCobertura == id);
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
