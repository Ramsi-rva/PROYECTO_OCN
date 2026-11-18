using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OcnMvc.Data;
using OcnMvc.Models;

namespace OcnMvc.Controllers;

// Controlador de la tabla ocn.modeloponderacion
public class ModeloPonderacionController : Controller
{
    private readonly OcnContext _context;

    public ModeloPonderacionController(OcnContext context)
    {
        _context = context;
    }

    // Muestra la lista de registros
    public async Task<IActionResult> Index()
    {
        var lista = _context.ModeloPonderacion
            .Include(x => x.Modelo)
            .AsQueryable();
        return View(await lista.ToListAsync());
    }

    // Muestra el detalle de un registro
    public async Task<IActionResult> Details(int? idModelo, string? variable)
    {
        if (idModelo == null || variable == null) return NotFound();

        var item = await _context.ModeloPonderacion
            .Include(x => x.Modelo)
            .FirstOrDefaultAsync(m => m.IdModelo == idModelo && m.Variable == variable);
        if (item == null) return NotFound();
        return View(item);
    }

    // Llena las listas desplegables de las llaves foráneas
    private void CargarListas(ModeloPonderacion? item = null)
    {
        ViewData["IdModelo"] = new SelectList(_context.ModeloPriorizacion.Where(x => x.Activo), "IdModelo", "Nombre", item?.IdModelo);
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
    public async Task<IActionResult> Create([Bind("IdModelo,Variable,Peso")] ModeloPonderacion item)
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
    public async Task<IActionResult> Edit(int? idModelo, string? variable)
    {
        if (idModelo == null || variable == null) return NotFound();

        var item = await _context.ModeloPonderacion.FirstOrDefaultAsync(m => m.IdModelo == idModelo && m.Variable == variable);
        if (item == null) return NotFound();
        CargarListas(item);
        return View(item);
    }

    // Guarda los cambios del registro
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int idModelo, string variable, [Bind("IdModelo,Variable,Peso")] ModeloPonderacion item)
    {
        if (idModelo != item.IdModelo || variable != item.Variable) return NotFound();

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
                if (!await _context.ModeloPonderacion.AnyAsync(e => e.IdModelo == idModelo && e.Variable == variable)) return NotFound();
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
    public async Task<IActionResult> Delete(int? idModelo, string? variable)
    {
        if (idModelo == null || variable == null) return NotFound();

        var item = await _context.ModeloPonderacion
            .Include(x => x.Modelo)
            .FirstOrDefaultAsync(m => m.IdModelo == idModelo && m.Variable == variable);
        if (item == null) return NotFound();
        return View(item);
    }

    // Elimina el registro
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int idModelo, string variable)
    {
        var item = await _context.ModeloPonderacion.FirstOrDefaultAsync(m => m.IdModelo == idModelo && m.Variable == variable);
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
