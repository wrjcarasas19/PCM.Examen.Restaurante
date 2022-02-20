using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PCM.Restaurante.DA
{
    public class OrdenPedido : Base
    {
        public OrdenPedido(NpgsqlConnection conexion) : base(conexion)
        {
        }

        public async ValueTask<List<BE.OrdenPedido>> ListarPorOrdenAsync(Guid ordenId)
        {
            var lbeOrdenPedido = new List<BE.OrdenPedido>();
            var comando = ObtenerComandoQuery(@"
                select * from public.orden_pedido where orden_id = @orden_id;
            ");

            comando.Parameters.Add(new NpgsqlParameter("@orden_id", ordenId));

            using (var lector = await comando.ExecuteReaderAsync())
            {
                if (lector.HasRows)
                {
                    var posOrdenId = lector.GetOrdinal("orden_id");
                    var posProductoId = lector.GetOrdinal("producto_id");
                    var posCantidad = lector.GetOrdinal("cantidad");
                    var posPrecioUnitario = lector.GetOrdinal("precio_unitario");

                    while (await lector.ReadAsync())
                    {
                        lbeOrdenPedido.Add(new BE.OrdenPedido()
                        {
                            OrdenId = lector.GetGuid(posOrdenId),
                            ProductoId = lector.GetInt32(posProductoId),
                            Cantidad = lector.GetDecimal(posCantidad),
                            PrecioUnitario = lector.GetDecimal(posPrecioUnitario)
                        });
                    }
                }
            }

            return lbeOrdenPedido;
        }
        public async ValueTask<bool> InsertarAsync(BE.OrdenPedido beOrdenPedido)
        {
            var comando = ObtenerComandoQuery(@"
                insert into public.orden_pedido
                select @orden_id, producto_id, @cantidad, precio from producto where producto_id = @producto_id;
            ");

            comando.Parameters.Add(new NpgsqlParameter("@orden_id", beOrdenPedido.OrdenId));
            comando.Parameters.Add(new NpgsqlParameter("@producto_id", beOrdenPedido.ProductoId));
            comando.Parameters.Add(new NpgsqlParameter("@cantidad", beOrdenPedido.Cantidad));

            return (await comando.ExecuteNonQueryAsync()) > 0;
        }
        public async ValueTask<bool> ActualizarAsync(BE.OrdenPedido beOrdenPedido)
        {
            var comando = ObtenerComandoQuery(@"
                update public.orden_pedido set
                    cantidad = @cantidad
                where
                    orden_id = @orden_id and
                    producto_id = @producto_id;
            ");

            comando.Parameters.Add(new NpgsqlParameter("@cantidad", beOrdenPedido.Cantidad));
            comando.Parameters.Add(new NpgsqlParameter("@orden_id", beOrdenPedido.OrdenId));
            comando.Parameters.Add(new NpgsqlParameter("@producto_id", beOrdenPedido.ProductoId));

            return (await comando.ExecuteNonQueryAsync()) > 0;
        }
        public async ValueTask<bool> EliminarAsync(BE.OrdenPedido beOrdenPedido)
        {
            var comando = ObtenerComandoQuery(@"
                delete from public.orden_pedido where
                    orden_id = @orden_id and
                    producto_id = @producto_id;
            ");

            comando.Parameters.Add(new NpgsqlParameter("@orden_id", beOrdenPedido.OrdenId));
            comando.Parameters.Add(new NpgsqlParameter("@producto_id", beOrdenPedido.ProductoId));

            return (await comando.ExecuteNonQueryAsync()) > 0;
        }
    }
}
