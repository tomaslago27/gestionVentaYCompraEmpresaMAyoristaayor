using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_Empresa_Mayorista.Data;
using Proyecto_Empresa_Mayorista.Models;

namespace Proyecto_Empresa_Mayorista.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        private readonly EmpresaDbContext _context;

        public ProductoController(EmpresaDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Producto>>> GetProductos()
        {
            return await _context.Producto.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Producto>> GetProducto(int id)
        {
            var producto = await _context.Producto.FindAsync(id);
            if (producto == null)
                return NotFound();
            return producto;
        }

        [HttpPost]
        public async Task<ActionResult<Producto>> PostProducto(Producto producto)
        {
            _context.Producto.Add(producto);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetProductos), new { id = producto.Id }, producto);
        }
[HttpPut]
        public async Task<IActionResult> PutProducto([FromBody] Producto producto)
        {
            var productoExiste = await _context.Producto.FindAsync(producto.Id);
            if (productoExiste == null)
                return NotFound();

            // Update only the properties you want to allow to change
            productoExiste.Nombre = producto.Nombre;
            productoExiste.Precio = producto.Precio;
            productoExiste.Stock = producto.Stock;
            // Add other properties as needed

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Producto.Any(e => e.Id == producto.Id))
                    return NotFound();
                else
                    throw;
            }
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProducto(int id)
        {
            var producto = await _context.Producto.FindAsync(id);
            if (producto == null)
                return NotFound();
            _context.Producto.Remove(producto);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}