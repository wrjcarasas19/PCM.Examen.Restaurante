using Npgsql;
using System.Threading.Tasks;

namespace PCM.Restaurante.BL
{
    public class Base
    {
        public async ValueTask<NpgsqlConnection> ObtenerConexionAsync()
        {
            var conexion = new NpgsqlConnection(Constantes.CADENA_CONEXION_BD);

            await conexion.OpenAsync();

            return conexion;
        }
    }
}
