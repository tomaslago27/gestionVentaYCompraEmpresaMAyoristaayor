namespace Proyecto_Empresa_Mayorista.Models
{
    public class DetalleCompra
    {
        public int Id { get; set; }
        public int Id_Compra { get; set; }
        public int Id_Producto { get; set; }
        public decimal PrecioUnitario { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioTotal { get; set; }
        public Compra Compra { get; set; }
        public Producto Producto { get; set; }
    }
}