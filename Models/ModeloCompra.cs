namespace Proyecto_Empresa_Mayorista.Models
{
    public class Compra
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public decimal GastoTotal { get; set; }
        public string Proveedor { get; set; }
    }
}