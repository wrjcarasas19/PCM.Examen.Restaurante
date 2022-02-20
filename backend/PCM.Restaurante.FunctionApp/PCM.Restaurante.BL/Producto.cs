using System.Collections.Generic;
using System.Threading.Tasks;

namespace PCM.Restaurante.BL
{
    public class Producto : Base
    {
        public async ValueTask<List<BE.Producto>> ListarAsync()
        {
            using (var conexion = await ObtenerConexionAsync())
            {
                return await new DA.Producto(conexion).ListarAsync();
            }
        }
    }
}
