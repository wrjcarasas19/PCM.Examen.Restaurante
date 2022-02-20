using System.Collections.Generic;
using System.Text;

namespace PCM.Restaurante.BE
{
    public class Producto
    {
        public int ProductoId { get; set; }
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public decimal? Stock { get; set; }
        public bool Disponible { get; set; }
        public List<BE.ProductoEtiqueta> Etiquetas { get; set; }
    }
}
