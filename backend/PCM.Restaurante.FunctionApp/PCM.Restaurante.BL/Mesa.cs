using System.Collections.Generic;
using System.Threading.Tasks;

namespace PCM.Restaurante.BL
{
    public class Mesa : Base
    {
        public async ValueTask<BE.Mesa> SeleccionarAsync(int mesaId)
        {
            using (var conexion = await ObtenerConexionAsync())
            {
                return await new DA.Mesa(conexion).ObtenerPorIdAsync(mesaId);
            }
        }
        public async ValueTask<List<BE.Mesa>> ListarAsync()
        {
            using (var conexion = await ObtenerConexionAsync())
            {
                return await new DA.Mesa(conexion).ListarAsync();
            }
        }
    }
}
