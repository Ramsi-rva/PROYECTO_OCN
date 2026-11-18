using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OcnMvc.Data;
using OcnMvc.Models;

namespace OcnMvc.Controllers;

// Controlador de la tabla ocn.fuente
public class FuenteController : Controller
{
    private readonly OcnContext _context;

    public FuenteController(OcnContext context)
    {
        _context = context;
    }

    // Muestra la lista de registros
    public async Task<IActionResult> Index()
    {
        var lista = _context.Fuente
            .Include(x => x.TipoFuente)
            .Include(x => x.Organizacion)
            .AsQueryable();
        lista = lista.Where(x => x.Activo);
        return View(await lista.ToListAsync());
    }

    // Muestra el detalle de un registro
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var item = await _context.Fuente
            .Include(x => x.TipoFuente)
            .Include(x => x.Organizacion)
            .FirstOrDefaultAsync(m => m.IdFuente == id);
        if (item == null) return NotFound();
        return View(item);
    }

    // Llena las listas desplegables de las llaves foráneas
    private void CargarListas(Fuente? item = null)
    {
        ViewData["IdTipoFuente"] = new SelectList(_context.CatTipoFuente.Where(x => x.Activo), "IdTipoFuente", "Nombre", item?.IdTipoFuente);
        ViewData["IdOrganizacion"] = new SelectList(_context.Organizacion.Where(x => x.Activo), "IdOrganizacion", "Nombre", item?.IdOrganizacion);
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
    public async Task<IActionResult> Create([Bind("IdTipoFuente,IdOrganizacion,Descripcion,Activo")] Fuente item)
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

        var item = await _context.Fuente.FirstOrDefaultAsync(m => m.IdFuente == id);
        if (item == null) return NotFound();
        CargarListas(item);
        return View(item);
    }

    // Guarda los cambios del registro
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("IdFuente,IdTipoFuente,IdOrganizacion,Descripcion,Activo")] Fuente item)
    {
        if (id != item.IdFuente) return NotFound();

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
                if (!await _context.Fuente.AnyAsync(e => e.IdFuente == id)) return NotFound();
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

        var item = await _context.Fuente
            .Include(x => x.TipoFuente)
            .Include(x => x.Organizacion)
            .FirstOrDefaultAsync(m => m.IdFuente == id);
        if (item == null) return NotFound();
        return View(item);
    }

    // Da de baja el registro
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var item = await _context.Fuente.FirstOrDefaultAsync(m => m.IdFuente == id);
        if (item != null)
        {
            // solo se marca como inactivo, no se borra de la base
            item.Activo = false;
            await _context.SaveChangesAsync();
            TempData["Mensaje"] = "Registro dado de baja.";
        }
        return RedirectToAction(nameof(Index));
    }
}
