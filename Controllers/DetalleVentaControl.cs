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

        [HttpPost]
        public async Task<ActionResult<DetalleVenta>> PostDetalleVenta(DetalleVenta detalle)
        {
            _context.DetallesVenta.Add(detalle);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetDetallesVenta), new { id = detalle.Id }, detalle);
        }
    }
}
/*
    Esta clase define un controlador API para gestionar los detalles de ventas en una aplicación ASP.NET Core.
    La clase se llama DetalleVentaController y hereda de ControllerBase.
    Incluye métodos para obtener la lista de detalles de venta y agregar un nuevo detalle a la base de datos.
    Utiliza Entity Framework Core para interactuar con la base de datos a través del DbContext EmpresaDbContext.
*/