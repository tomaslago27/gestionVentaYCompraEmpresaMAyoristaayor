using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_Empresa_Mayorista.Data;
using Proyecto_Empresa_Mayorista.Models;

namespace Proyecto_Empresa_Mayorista.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DetalleCompraController : ControllerBase
    {
        private readonly EmpresaDbContext _context;

        public DetalleCompraController(EmpresaDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DetalleCompra>>> GetDetallesCompra()
        {
            return await _context.DetallesCompra.Include(d => d.Compra).Include(d => d.Producto).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DetalleCompra>> GetDetalleCompra(int id)
        {
            var detalle = await _context.DetallesCompra.Include(d => d.Compra).Include(d => d.Producto).FirstOrDefaultAsync(d => d.Id == id);
            if (detalle == null)
                return NotFound();
            return detalle;
        }

        [HttpPost]
        public async Task<ActionResult<DetalleCompra>> PostDetalleCompra(DetalleCompra detalle)
        {
            _context.DetallesCompra.Add(detalle);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetDetallesCompra), new { id = detalle.Id }, detalle);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutDetalleCompra(int id, DetalleCompra detalle)
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
                if (!_context.DetallesCompra.Any(e => e.Id == id))
                    return NotFound();
                else
                    throw;
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDetalleCompra(int id)
        {
            var detalle = await _context.DetallesCompra.FindAsync(id);
            if (detalle == null)
                return NotFound();
            _context.DetallesCompra.Remove(detalle);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}