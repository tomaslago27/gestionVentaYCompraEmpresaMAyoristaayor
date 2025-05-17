namespace Proyecto_Empresa_Mayorista.Models
{
    public class Compra
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }
        public string Proveedor { get; set; }
    }
}