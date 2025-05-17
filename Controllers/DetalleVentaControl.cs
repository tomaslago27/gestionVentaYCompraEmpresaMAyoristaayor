using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_Empresa_Mayorista.Data;
using Proyecto_Empresa_Mayorista.Models;

namespace Proyecto_Empresa_Mayorista.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DetalleVentaController : ControllerBase
    {
        private readonly EmpresaDbContext _context;

        public DetalleVentaController(EmpresaDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DetalleVenta>>> GetDetallesVenta()
        {
            return await _context.DetallesVenta.Include(d => d.Venta).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DetalleVenta>> GetDetalleVenta(int id)
        {
            var detalle = await _context.DetallesVenta.Include(d => d.Venta).FirstOrDefaultAsync(d => d.Id == id);
            if (detalle == null)
                return NotFound();
            return detalle;
        }

        [HttpPost]
        public async Task<ActionResult<DetalleVenta>> PostDetalleVenta(DetalleVenta detalle)
        {
            _context.DetallesVenta.Add(detalle);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetDetallesVenta), new { id = detalle.Id }, detalle);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutDetalleVenta(int id, DetalleVenta detalle)
        {
            if (id != detalle.Id)
                return BadRequest();
            _context.Entry(detalle).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.DetallesVenta.Any(e => e.Id == id))
                    return NotFound();
                else
                    throw;
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDetalleVenta(int id)
        {
            var detalle = await _context.DetallesVenta.FindAsync(id);
            if (detalle == null)
                return NotFound();
            _context.DetallesVenta.Remove(detalle);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
/*
    Esta clase define un controlador API para gestionar los detalles de ventas en una aplicación ASP.NET Core.
    La clase se llama DetalleVentaController y hereda de ControllerBase.
    Incluye métodos para obtener la lista de detalles de venta y agregar un nuevo detalle a la base de datos.
    Utiliza Entity Framework Core para interactuar con la base de datos a través del DbContext EmpresaDbContext.
*/