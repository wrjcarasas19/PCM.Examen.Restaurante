using System.Collections.Generic;
using System.Threading.Tasks;

namespace PCM.Restaurante.BL
{
    public class ProductoEtiqueta : Base
    {
        public async ValueTask<List<BE.ProductoEtiqueta>> ListarAsync()
        {
            using (var conexion = await ObtenerConexionAsync())
            {
                return await new DA.ProductoEtiqueta(conexion).ListarAsync();
            }
        }
    }
}
