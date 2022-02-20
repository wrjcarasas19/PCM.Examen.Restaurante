using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PCM.Restaurante.DA
{
    public class Mesa : Base
    {
        public Mesa(NpgsqlConnection conexion) : base(conexion)
        {
        }

        public async ValueTask<BE.Mesa> ObtenerPorIdAsync(int mesaId)
        {
            var beMesa = default(BE.Mesa);
            var comando = ObtenerComandoQuery(@"
                select 
                    m.*,
                    o.fecha_creacion,
                    o.total,
                    o.contador_pedidos 
                from 
                    public.mesa m left join 
                    public.orden o on m.orden_id = o.orden_id
                where
                    m.mesa_id = @mesa_id
                order by
                    mesa_id;
            ");

            comando.Parameters.Add(new NpgsqlParameter("@mesa_id", mesaId));

            using (var lector = await comando.ExecuteReaderAsync())
            {
                if (lector.HasRows)
                {
                    var posMesaId = lector.GetOrdinal("mesa_id");
                    var posCapacidad = lector.GetOrdinal("capacidad");
                    var posOrdenId = lector.GetOrdinal("orden_id");
                    var posEstado = lector.GetOrdinal("estado");
                    var posFechaCreacion = lector.GetOrdinal("fecha_creacion");
                    var posTotal = lector.GetOrdinal("total");
                    var posContadorPedidos = lector.GetOrdinal("contador_pedidos");

                    if (await lector.ReadAsync())
                    {
                        beMesa = new BE.Mesa()
                        {
                            MesaId = lector.GetInt32(posMesaId),
                            Capacidad = lector.GetInt32(posCapacidad),
                            OrdenId = lector.IsDBNull(posOrdenId) ? default(Guid?) : lector.GetGuid(posOrdenId),
                            Estado = lector.GetInt32(posEstado)
                        };

                        if (beMesa.OrdenId.HasValue)
                        {
                            beMesa.Orden = new BE.Orden()
                            {
                                OrdenId = lector.GetGuid(posOrdenId),
                                FechaCreacion = lector.GetDateTime(posFechaCreacion),
                                MesaId = lector.GetInt32(posMesaId),
                                Total = lector.GetDecimal(posTotal),
                                ContadorPedidos = lector.GetInt32(posContadorPedidos),
                                EstadoValor = BE.OrdenEstado.EnProceso
                            };
                        }
                    }
                }
            }

            return beMesa;
        }
        public async ValueTask<List<BE.Mesa>> ListarAsync()
        {
            var lbeMesa = new List<BE.Mesa>();
            var beMesa = default(BE.Mesa);
            var comando = ObtenerComandoQuery(@"
                select 
                    m.*,
                    o.fecha_creacion,
                    o.total,
                    o.contador_pedidos 
                from 
                    public.mesa m left join 
                    public.orden o on m.orden_id = o.orden_id;
            ");

            using (var lector = await comando.ExecuteReaderAsync())
            {
                if (lector.HasRows)
                {
                    var posMesaId = lector.GetOrdinal("mesa_id");
                    var posCapacidad = lector.GetOrdinal("capacidad");
                    var posOrdenId = lector.GetOrdinal("orden_id");
                    var posEstado = lector.GetOrdinal("estado");
                    var posFechaCreacion = lector.GetOrdinal("fecha_creacion");
                    var posTotal = lector.GetOrdinal("total");
                    var posContadorPedidos = lector.GetOrdinal("contador_pedidos");

                    while (await lector.ReadAsync())
                    {
                        beMesa = new BE.Mesa()
                        {
                            MesaId = lector.GetInt32(posMesaId),
                            Capacidad = lector.GetInt32(posCapacidad),
                            OrdenId = lector.IsDBNull(posOrdenId) ? default(Guid?) : lector.GetGuid(posOrdenId),
                            Estado = lector.GetInt32(posEstado)
                        };

                        if (beMesa.OrdenId.HasValue)
                        {
                            beMesa.Orden = new BE.Orden()
                            {
                                OrdenId = lector.GetGuid(posOrdenId),
                                FechaCreacion = lector.GetDateTime(posFechaCreacion),
                                MesaId = lector.GetInt32(posMesaId),
                                Total = lector.GetDecimal(posTotal),
                                ContadorPedidos = lector.GetInt32(posContadorPedidos),
                                EstadoValor = BE.OrdenEstado.EnProceso
                            };
                        }

                        lbeMesa.Add(beMesa);
                    }
                }
            }

            return lbeMesa;
        }
        public async ValueTask<bool> ActualizarAsync(BE.Mesa beMesa)
        {
            var comando = ObtenerComandoQuery(@"
                update public.mesa set
                    orden_id = @orden_id,
                    estado = @estado
                where
                    mesa_id = @mesa_id;
            ");

            comando.Parameters.Add(new NpgsqlParameter("@orden_id", beMesa.OrdenId.HasValue ? (object)beMesa.OrdenId.Value : DBNull.Value));
            comando.Parameters.Add(new NpgsqlParameter("@estado", beMesa.Estado));
            comando.Parameters.Add(new NpgsqlParameter("@mesa_id", beMesa.MesaId));

            return (await comando.ExecuteNonQueryAsync()) > 0;
        }
    }
}
