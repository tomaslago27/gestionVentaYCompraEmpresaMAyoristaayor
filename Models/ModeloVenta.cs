namespace Proyecto_Empresa_Mayorista.Models
{
    public class Venta
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }
        public List<DetalleVenta> Detalles { get; set; }
    }
}
/*
    Esta clase define un modelo de datos para una venta en una aplicación de ventas.
    La clase se llama Venta y contiene propiedades para el ID de la venta, la fecha,
    el ID del cliente y una lista de detalles de la venta.
    La propiedad Cliente es una referencia a un objeto Cliente, lo que permite
    establecer una relación entre la venta y el cliente asociado.
*/