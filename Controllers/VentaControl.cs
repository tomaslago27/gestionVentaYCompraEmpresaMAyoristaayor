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
        // Consulta SQL equivalente:
        // SELECT v.*, c.*, dv.*, p.*
        // FROM Ventas v
        // INNER JOIN Clientes c ON v.ClienteId = c.Id
        // INNER JOIN DetallesVenta dv ON dv.VentaId = v.Id
        // INNER JOIN Producto p ON dv.ProductoId = p.Id


        [HttpGet("{id}")]
        public async Task<ActionResult<Venta>> GetVenta(int id)
        {

            var venta = await _context.Ventas.Include(v => v.Cliente)
                .Include(v => v.Detalles)
                    .ThenInclude(dv => dv.Producto).FirstOrDefaultAsync(v => v.Id == id); if (venta == null)
                return NotFound();
            return venta;
        }
        // Consulta SQL equivalente:
        // SELECT v.*, c.*, dv.*, p.*
        // FROM Ventas v
        // INNER JOIN Clientes c ON v.ClienteId = c.Id
        // INNER JOIN DetallesVenta dv ON dv.VentaId = v.Id
        // INNER JOIN Producto p ON dv.ProductoId = p.Id
        // WHERE v.Id = {id}

        [HttpPost]
        public async Task<ActionResult<Venta>> PostVenta([FromBody] NuevaVentaDto nuevaVenta)
        {
            // Verifica que existan el cliente y el producto
            var cliente = await _context.Clientes.FindAsync(nuevaVenta.ClienteId);
            if (cliente == null)
                return BadRequest("Cliente o producto no encontrado.");

            // Crea la venta
            var venta = new Venta
            {
                ClienteId = cliente.Id,
                Fecha = DateTime.Now,
                Detalles = new List<DetalleVenta>
                {

                }
            };
            foreach (var detalle in nuevaVenta.Detalles)
            {
                var producto = await _context.Producto.FindAsync(detalle.ProductoId);
                if (producto == null)
                    return BadRequest($"Producto con ID {detalle.ProductoId} no encontrado.");

                venta.Detalles.Add(new DetalleVenta
                {
                    ProductoId = detalle.ProductoId,
                    Cantidad = detalle.Cantidad,
                    PrecioUnitario = producto.Precio
                });
                venta.Total += producto.Precio * detalle.Cantidad;
                producto.Stock -= detalle.Cantidad;
                _context.Producto.Update(producto);
            }
            _context.Ventas.Add(venta);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetVenta), new { id = venta.Id }, new { venta.Id });
        }

        // DTO para recibir los datos mínimos de la venta
        public class NuevaVentaDto
        {
            public int ClienteId { get; set; }
            public int ProductoId { get; set; }
            public int Cantidad { get; set; }
            public List<DetalleVentaDto> Detalles { get; set; }
        }

        public class DetalleVentaDto
        {
            public int ProductoId { get; set; }
            public int Cantidad { get; set; }
            public decimal PrecioUnitario { get; set; }
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
        }
        [HttpGet("venta-cliente-articulo")]
        public IActionResult ObtenerVentaDeClienteYProducto(int idCliente, int idArticulo)
        {
            // Filtra las ventas que pertenecen al cliente especificado por idCliente
            var ventas = _context.Ventas
                .Where(v => v.ClienteId == idCliente)
                .Include(v => v.Cliente)
                .Include(v => v.Detalles)
                    .ThenInclude(dv => dv.Producto)
                .SelectMany(v => v.Detalles
                    .Where(dv => dv.ProductoId == idArticulo)
                    .Select(dv => new
                    {
                        VentaId = v.Id,
                        Cliente = v.Cliente,
                        Fecha = v.Fecha,
                        Total = v.Total,
                        Detalles = v.Detalles
                            .Where(det => det.ProductoId == idArticulo)
                            .Select(det => new
                            {
                                det.Id,
                                det.Cantidad,
                                det.PrecioUnitario,
                                Producto = new
                                {
                                    det.Producto.Id,
                                    det.Producto.Nombre,
                                    det.Producto.Descripcion,
                                    det.Producto.Tipo,
                                    det.Producto.Precio,
                                    det.Producto.Stock
                                },
                                Subtotal = det.Cantidad * det.PrecioUnitario
                            }).ToList()
                    }))
                .ToList();
            if (ventas.Count == 0)
            {
                return NotFound("No se encontraron ventas para ese cliente y artículo.");
            }

            return Ok(ventas);
        }






        // Consulta SQL equivalente:
        // SELECT v.Id, c.Nombre AS Cliente, p.Nombre AS Articulo, dv.Cantidad, dv.PrecioUnitario,
        //        (dv.Cantidad * dv.PrecioUnitario) AS Total, v.Fecha
        // FROM Ventas v
        // INNER JOIN Clientes c ON v.ClienteId = c.Id
        // INNER JOIN DetallesVenta dv ON dv.VentaId = v.Id
        // INNER JOIN Producto p ON dv.ProductoId = p.Id
        // WHERE v.ClienteId = @idCliente AND dv.ProductoId = @idArticulo

        [HttpGet("ventas-por-cliente")]
        public IActionResult ObtenerVentasPorCliente(int idCliente)
        {
            var ventas = _context.Ventas
            .Where(v => v.ClienteId == idCliente)
            .Include(v => v.Cliente)
            .Include(v => v.Detalles)
            .ThenInclude(dv => dv.Producto)
            .Select(v => new
            {
            v.Id,
            Cliente = v.Cliente.Nombre,
            Fecha = v.Fecha,
            Total = v.Total,
            Detalles = v.Detalles.Select(dv => new
            {
                Articulo = dv.Producto.Nombre,
                dv.Cantidad,
                dv.PrecioUnitario,
                Subtotal = dv.Cantidad * dv.PrecioUnitario
            }).ToList()
            })
            .ToList();

            if (ventas.Count == 0)
            {
            return NotFound("No se encontraron ventas para ese cliente.");
            }

            return Ok(ventas);
        }
        // Consulta SQL equivalente:
        // SELECT v.Id, c.Nombre AS Cliente, v.Fecha, v.Total,
        //        p.Nombre AS Articulo, dv.Cantidad, dv.PrecioUnitario,
        //        (dv.Cantidad * dv.PrecioUnitario) AS Subtotal
        // FROM Ventas v
        // INNER JOIN Clientes c ON v.ClienteId = c.Id
        // INNER JOIN DetallesVenta dv ON dv.VentaId = v.Id
        // INNER JOIN Producto p ON dv.ProductoId = p.Id
        // WHERE v.ClienteId = @idCliente

    }
}

/*
    Esta clase define un controlador API para gestionar ventas en una aplicación ASP.NET Core.
    La clase se llama VentaController y hereda de ControllerBase.
    Incluye métodos para obtener la lista de ventas y agregar una nueva venta a la base de datos.
    Utiliza Entity Framework Core para interactuar con la base de datos a través del DbContext EmpresaDbContext.
*/
