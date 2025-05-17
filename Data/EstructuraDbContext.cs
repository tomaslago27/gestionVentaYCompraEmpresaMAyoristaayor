using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Proyecto_Empresa_Mayorista.Models;

namespace Proyecto_Empresa_Mayorista.Data
{
    public class EmpresaDbContext : DbContext
    {
        public EmpresaDbContext(DbContextOptions<EmpresaDbContext> options)
            : base(options) { }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Compra> Compra { get; set; }
        public DbSet<DetalleCompra> DetallesCompra { get; set; }
        public DbSet<Venta> Ventas { get; set; }
        public DbSet<DetalleVenta> DetallesVenta { get; set; }
        public DbSet<Producto> Producto { get; set; }

        
    }
}
/*
    Esta clase define un DbContext para una aplicación con Entity Framework Core.
    La clase se llama EmpresaDbContext y hereda de DbContext.
    Incluye DbSet para las entidades Cliente, Venta y DetalleVenta,
    lo que permite realizar operaciones CRUD sobre estas tablas en la base de datos.
*/