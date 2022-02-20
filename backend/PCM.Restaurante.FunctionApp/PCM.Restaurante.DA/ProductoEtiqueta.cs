using Npgsql;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PCM.Restaurante.DA
{
    public class ProductoEtiqueta : Base
    {
        public ProductoEtiqueta(NpgsqlConnection conexion) : base(conexion)
        {
        }

        public async ValueTask<List<BE.ProductoEtiqueta>> ListarAsync()
        {
            var lbeProductoEtiqueta = new List<BE.ProductoEtiqueta>();
            var comando = ObtenerComandoQuery(@"
                select * from public.producto_etiqueta;
            ");

            using (var lector = await comando.ExecuteReaderAsync())
            {
                if (lector.HasRows)
                {
                    var posNombre = lector.GetOrdinal("nombre");
                    var posValor = lector.GetOrdinal("valor");

                    while(await lector.ReadAsync())
                    {
                        lbeProductoEtiqueta.Add(new BE.ProductoEtiqueta()
                        {
                            Nombre = lector.GetString(posNombre),
                            Valor = lector.GetString(posValor)
                        });
                    }
                }
            }

            return lbeProductoEtiqueta;
        }
    }
}
