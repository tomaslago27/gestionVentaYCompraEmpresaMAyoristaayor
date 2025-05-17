using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_Empresa_Mayorista.Data;
using Proyecto_Empresa_Mayorista.Models;

namespace Proyecto_Empresa_Mayorista.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly EmpresaDbContext _context;

        public ClienteController(EmpresaDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetClientes()
        {
            return await _context.Clientes.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Cliente>> PostCliente(Cliente cliente)
        {
            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetClientes), new { id = cliente.Id }, cliente);
        }
    }
}
/*
    Esta clase define un controlador API para gestionar clientes en una aplicación ASP.NET Core.
    La clase se llama ClienteController y hereda de ControllerBase.
    Incluye métodos para obtener la lista de clientes y agregar un nuevo cliente a la base de datos.
    Utiliza Entity Framework Core para interactuar con la base de datos a través del DbContext EmpresaDbContext.
*/
