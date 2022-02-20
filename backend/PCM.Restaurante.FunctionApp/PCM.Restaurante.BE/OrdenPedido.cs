using System;

namespace PCM.Restaurante.BE
{
    public class OrdenPedido
    {
        public Guid OrdenId { get; set; }
        public int ProductoId { get; set; }
        public decimal Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }

        public Producto Producto { get; set; }
    }
}
