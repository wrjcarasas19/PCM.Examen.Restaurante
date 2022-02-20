using Npgsql;

namespace PCM.Restaurante.DA
{
    public class Base
    {
        public NpgsqlConnection Conexion { get; private set; }

        public NpgsqlCommand ObtenerComandoQuery(string consulta)
        {
            var comando = Conexion.CreateCommand();

            comando.CommandType = System.Data.CommandType.Text;
            comando.CommandText = consulta;

            return comando;
        }
        
        public Base(NpgsqlConnection conexion)
        {
            Conexion = conexion;
        }
    }
}
