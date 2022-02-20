using Npgsql;
using System;
using System.Threading.Tasks;

namespace PCM.Restaurante.DA
{
    public class Orden : Base
    {
        public Orden(NpgsqlConnection conexion) : base(conexion)
        {
        }

        public async ValueTask<BE.Orden> ObtenerPorIdAsync(Guid ordenId)
        {
            var beOrden = default(BE.Orden);
            var comando = ObtenerComandoQuery(@"
                select * from public.orden where orden_id=@orden_id;
            ");

            comando.Parameters.Add(new NpgsqlParameter("@orden_id", ordenId));

            using (var lector = await comando.ExecuteReaderAsync())
            {
                if (lector.HasRows)
                {
                    var posOrdenId = lector.GetOrdinal("orden_id");
                    var posFechaCreacion = lector.GetOrdinal("fecha_creacion");
                    var posMesaId = lector.GetOrdinal("mesa_id");
                    var posTotal = lector.GetOrdinal("total");
                    var posContadorPedidos = lector.GetOrdinal("contador_pedidos");
                    var posEstado = lector.GetOrdinal("estado");

                    if (await lector.ReadAsync())
                    {
                        beOrden = new BE.Orden()
                        {
                            OrdenId = lector.GetGuid(posOrdenId),
                            FechaCreacion = lector.GetDateTime(posFechaCreacion),
                            MesaId = lector.GetInt32(posMesaId),
                            Total = lector.GetDecimal(posTotal),
                            ContadorPedidos = lector.GetInt32(posContadorPedidos),
                            Estado = lector.GetInt32(posEstado)
                        };
                    }
                }
            }

            return beOrden;
        }
        public async ValueTask<bool> InsertarAsync(BE.Orden beOrden)
        {
            var comando = ObtenerComandoQuery(@"
                insert into public.orden
                    (orden_id, fecha_creacion, mesa_id, total, contador_pedidos, estado)
                values
                    (@orden_id, @fecha_creacion, @mesa_id, @total, @contador_pedidos, @estado);
            ");

            comando.Parameters.Add(new NpgsqlParameter("@orden_id", beOrden.OrdenId));
            comando.Parameters.Add(new NpgsqlParameter("@fecha_creacion", beOrden.FechaCreacion));
            comando.Parameters.Add(new NpgsqlParameter("@mesa_id", beOrden.MesaId));
            comando.Parameters.Add(new NpgsqlParameter("@total", beOrden.Total));
            comando.Parameters.Add(new NpgsqlParameter("@contador_pedidos", beOrden.ContadorPedidos));
            comando.Parameters.Add(new NpgsqlParameter("@estado", beOrden.Estado));

            return (await comando.ExecuteNonQueryAsync()) > 0;
        }
        public async ValueTask<bool> ActualizarAsync(BE.Orden beOrden)
        {
            var comando = ObtenerComandoQuery(@"
                update public.orden as o set
                    total = p.total,
                    contador_pedidos = p.contador,
                    estado = @estado
                from
                    (select sum(cantidad * precio_unitario) as total, sum(cantidad) as contador from public.orden_pedido where orden_id = @orden_id) as p
                where
                    o.orden_id = @orden_id;
            ");

            comando.Parameters.Add(new NpgsqlParameter("@estado", beOrden.Estado));
            comando.Parameters.Add(new NpgsqlParameter("@orden_id", beOrden.OrdenId));

            return (await comando.ExecuteNonQueryAsync()) > 0;
        }
    }
}
