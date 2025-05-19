
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

        [HttpGet("{id}")]
        public async Task<ActionResult<Cliente>> GetCliente(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);

            if (cliente == null)
                return NotFound();

            return cliente;
        }

        [HttpPost]
        public async Task<ActionResult<Cliente>> PostCliente(Cliente cliente)
        {
            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCliente), new { id = cliente.Id }, cliente);
        }
         [HttpPut]
         public async Task<IActionResult> PutCliente([FromQuery] int id, [FromBody] Cliente cliente)
    {
       var clienteExiste = await _context.Clientes.FindAsync(id);
        if (clienteExiste == null)
            return NotFound();

        clienteExiste.Nombre = cliente.Nombre;
        clienteExiste.Apellido = cliente.Apellido;
        clienteExiste.DNI = cliente.DNI;
        clienteExiste.Domicilio = cliente.Domicilio;
        clienteExiste.Localidad = cliente.Localidad;
        clienteExiste.Telefono = cliente.Telefono;
        clienteExiste.Email = cliente.Email;
        await _context.SaveChangesAsync();

        return Ok(clienteExiste);
    }

         [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCliente(int id)
    {
        var cliente = await _context.Clientes.FindAsync(id);
        if (cliente == null)
            return NotFound();

        _context.Clientes.Remove(cliente);
        await _context.SaveChangesAsync();

        return NoContent();
    } 
    }
}
/*
    Esta clase define un controlador API para gestionar clientes en una aplicación ASP.NET Core.
    La clase se llama ClienteController y hereda de ControllerBase.
    Incluye métodos para obtener la lista de clientes y agregar un nuevo cliente a la base de datos.
    Utiliza Entity Framework Core para interactuar con la base de datos a través del DbContext EmpresaDbContext.
cambio
*/
