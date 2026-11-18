using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OcnMvc.Data;
using OcnMvc.Models;

namespace OcnMvc.Controllers;

// Controlador de la tabla ocn.registroocn
public class RegistrosOCNController : Controller
{
    private readonly OcnDbContext _context;

    public RegistrosOCNController(OcnDbContext context)
    {
        _context = context;
    }

    // Muestra la lista de registros
    public async Task<IActionResult> Index()
    {
        var lista = _context.RegistroOCN
            .Include(x => x.Zona)
            .Include(x => x.Problematica)
            .AsQueryable();
        lista = lista.Where(x => x.Activo);
        return View(await lista.ToListAsync());
    }

    // Muestra el detalle de un registro
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var item = await _context.RegistroOCN
            .Include(x => x.Zona)
            .Include(x => x.Problematica)
            .FirstOrDefaultAsync(m => m.IdRegistroOCN == id);
        if (item == null) return NotFound();
        return View(item);
    }

    // Llena las listas desplegables de las llaves foráneas
    private void CargarListas(RegistroOCN? item = null)
    {
        ViewData["IdZona"] = new SelectList(_context.Zona.Where(x => x.Activo), "IdZona", "Nombre", item?.IdZona);
        ViewData["IdProblematica"] = new SelectList(_context.Problematica.Where(x => x.Activo), "IdProblematica", "Nombre", item?.IdProblematica);
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
    public async Task<IActionResult> Create([Bind("IdZona,IdProblematica,Descripcion,FechaRegistro,Activo")] RegistroOCN item)
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

        var item = await _context.RegistroOCN.FirstOrDefaultAsync(m => m.IdRegistroOCN == id);
        if (item == null) return NotFound();
        CargarListas(item);
        return View(item);
    }

    // Guarda los cambios del registro
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("IdRegistroOCN,IdZona,IdProblematica,Descripcion,FechaRegistro,Activo")] RegistroOCN item)
    {
        if (id != item.IdRegistroOCN) return NotFound();

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
                if (!await _context.RegistroOCN.AnyAsync(e => e.IdRegistroOCN == id)) return NotFound();
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

        var item = await _context.RegistroOCN
            .Include(x => x.Zona)
            .Include(x => x.Problematica)
            .FirstOrDefaultAsync(m => m.IdRegistroOCN == id);
        if (item == null) return NotFound();
        return View(item);
    }

    // Da de baja el registro
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var item = await _context.RegistroOCN.FirstOrDefaultAsync(m => m.IdRegistroOCN == id);
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
