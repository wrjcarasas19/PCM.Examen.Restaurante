using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PCM.Restaurante.DA
{
    public class Producto : Base
    {
        public Producto(NpgsqlConnection conexion) : base(conexion)
        {
        }

        public async ValueTask<List<BE.Producto>> ListarAsync()
        {
            var lbeProducto = new List<BE.Producto>();
            var comando = ObtenerComandoQuery(@"
                select * from public.producto;
            ");

            using (var lector = await comando.ExecuteReaderAsync())
            {
                if (lector.HasRows)
                {
                    var posProductoId = lector.GetOrdinal("producto_id");
                    var posNombre = lector.GetOrdinal("nombre");
                    var posPrecio = lector.GetOrdinal("precio");
                    var posStock = lector.GetOrdinal("stock");
                    var posDisponible = lector.GetOrdinal("disponible");
                    var posEtiquetas = lector.GetOrdinal("etiquetas");

                    while (await lector.ReadAsync())
                    {
                        lbeProducto.Add(new BE.Producto()
                        {
                            ProductoId = lector.GetInt32(posProductoId),
                            Nombre = lector.GetString(posNombre),
                            Precio = lector.GetDecimal(posPrecio),
                            Stock = lector.IsDBNull(posStock) ? default : lector.GetDecimal(posStock),
                            Disponible = lector.GetBoolean(posDisponible),
                            Etiquetas = lector.GetString(posEtiquetas)
                                .Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries)
                                .Select(x => x.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries))
                                .Select(x => new BE.ProductoEtiqueta() { Nombre = x[0], Valor = x[1] })
                                .ToList()
                        });
                    }
                }
            }

            return lbeProducto;
        }
    }
}
