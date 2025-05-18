using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_Empresa_Mayorista.Data;
using Proyecto_Empresa_Mayorista.Models;

namespace Proyecto_Empresa_Mayorista.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentaController : ControllerBase
    {
        private readonly EmpresaDbContext _context;

        public VentaController(EmpresaDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Venta>>> GetVentas()
        {
            return await _context.Ventas
                .Include(v => v.Cliente)
                .Include(v => v.Detalles)
                    .ThenInclude(dv => dv.Producto)
                .ToListAsync();

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Venta>> GetVenta(int id)
        {
            var venta = await _context.Ventas.Include(v => v.Cliente).FirstOrDefaultAsync(v => v.Id == id);
            if (venta == null)
                return NotFound();
            return venta;
        }

        [HttpPost]
        public async Task<ActionResult<Venta>> PostVenta(Venta venta)
        {
            _context.Ventas.Add(venta);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetVenta), new { id = venta.Id }, venta);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutVenta(int id, Venta venta)
        {
            if (id != venta.Id)
                return BadRequest();
            _context.Entry(venta).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Ventas.Any(e => e.Id == id))
                    return NotFound();
                else
                    throw;
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVenta(int id)
        {
            var venta = await _context.Ventas.FindAsync(id);
            if (venta == null)
                return NotFound();
            _context.Ventas.Remove(venta);
            await _context.SaveChangesAsync();
            return NoContent();
        }        [HttpGet("venta-cliente-articulo")]
        public IActionResult ObtenerVentaDeClienteYProducto(int idCliente, int idArticulo)
        {
            // Filtra las ventas que pertenecen al cliente especificado por idCliente
            var ventas = _context.Ventas
                .Where(v => v.ClienteId == idCliente)
                // Realiza un join entre las ventas y los detalles de venta usando el Id de la venta
                .Join(_context.DetallesVenta,
                    v => v.Id,                  // Clave de la entidad Venta
                    dv => dv.VentaId,           // Clave de la entidad DetalleVenta
                    (v, dv) => new { v, dv })   // Crea un objeto anónimo con la venta y su detalle
                // Filtra los resultados para obtener solo los detalles del artículo especificado por idArticulo
                .Where(joined => joined.dv.ProductoId == idArticulo)
                // Proyecta los datos seleccionados en un nuevo objeto anónimo con la información relevante
                .Select(joined => new
                {
                    joined.v.Id,                                 // Id de la venta
                    Cliente = joined.v.Cliente.Nombre,           // Nombre del cliente
                    Articulo = joined.dv.Producto.Nombre,        // Nombre del artículo/producto
                    joined.dv.Cantidad,                          // Cantidad vendida
                    joined.dv.PrecioUnitario,                    // Precio unitario del artículo
                    Total = joined.dv.Cantidad * joined.dv.PrecioUnitario, // Total de la línea (cantidad * precio)
                    Fecha = joined.v.Fecha                       // Fecha de la venta
                })
                .ToList(); // Convierte el resultado a una lista
    if (ventas.Count == 0)
    {
        return NotFound("No se encontraron ventas para ese cliente y artículo.");
    }

    return Ok(ventas);
}

    }
}

/*
    Esta clase define un controlador API para gestionar ventas en una aplicación ASP.NET Core.
    La clase se llama VentaController y hereda de ControllerBase.
    Incluye métodos para obtener la lista de ventas y agregar una nueva venta a la base de datos.
    Utiliza Entity Framework Core para interactuar con la base de datos a través del DbContext EmpresaDbContext.
*/