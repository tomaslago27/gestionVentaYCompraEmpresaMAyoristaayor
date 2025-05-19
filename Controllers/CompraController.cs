using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_Empresa_Mayorista.Data;
using Proyecto_Empresa_Mayorista.Models;

namespace Proyecto_Empresa_Mayorista.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompraController : ControllerBase
    {
        private readonly EmpresaDbContext _context;

        public CompraController(EmpresaDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        
        public async Task<ActionResult<IEnumerable<Compra>>> GetCompras()
        {
            return await _context.Compra.Include(c => c.Detalles).ThenInclude(dv => dv.Producto).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Compra>> GetCompra(int id)
        {
            var compra = await _context.Compra.FindAsync(id);
            if (compra == null)
                return NotFound();
            return compra;
        }

        [HttpPost]
        public async Task<ActionResult<Compra>> PostCompra([FromBody] NuevaCompraDto compraDto)
        {
            var nuevaCompra = new Compra
            {
                Fecha = DateTime.Now,
                Proveedor = compraDto.Proveedor,
                Detalles = new List<DetalleCompra>(),
                Total = 0
            };

            foreach (var detalleDto in compraDto.Detalles)
            {
                var producto = await _context.Producto.FindAsync(detalleDto.ProductoId);
                
                if (producto == null)
                    return BadRequest($"Producto con ID {detalleDto.ProductoId} no encontrado.");

                            if (producto.Precio != detalleDto.PrecioUnitario)
                            {
                                producto.Precio = detalleDto.PrecioUnitario;
                                _context.Producto.Update(producto);
                            }
                
 
                nuevaCompra.Detalles.Add(new DetalleCompra
                {
                    ProductoId = detalleDto.ProductoId,
                    Cantidad = detalleDto.Cantidad,
                    PrecioUnitario = detalleDto.PrecioUnitario,
                    PrecioTotal = detalleDto.PrecioUnitario * detalleDto.Cantidad
                });

                // Aumentar stock
                producto.Stock += detalleDto.Cantidad;
                _context.Producto.Update(producto);

                // Actualizar total compra
                nuevaCompra.Total += detalleDto.PrecioUnitario * detalleDto.Cantidad;
            }
            _context.Compra.Add(nuevaCompra);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCompras), new { id = nuevaCompra.Id }, new { nuevaCompra.Id });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCompra(int id, Compra compra)
        {
            if (id != compra.Id)
                return BadRequest();
            _context.Entry(compra).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Compra.Any(e => e.Id == id))
                    return NotFound();
                else
                    throw;
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompra(int id)
        {
            var compra = await _context.Compra.FindAsync(id);
            if (compra == null)
                return NotFound();
            _context.Compra.Remove(compra);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }

    public class NuevaCompraDto
    {
        public string Proveedor { get; set; } = string.Empty;
        public List<DetalleCompraDto> Detalles { get; set; } = new List<DetalleCompraDto>();
    }

    public class DetalleCompraDto
    {
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
    }
}

