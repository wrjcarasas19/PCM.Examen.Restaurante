using Npgsql;
using System;
using System.Net;
using System.Threading.Tasks;

namespace PCM.Restaurante.BL
{
    public class Orden : Base
    {
        protected async ValueTask<BE.Orden> ActualizarFinalizarAsync(BE.Orden beOrden)
        {
            var daMesa = default(DA.Mesa);
            var daOrden = default(DA.Orden);
            var beMesa = default(BE.Mesa);
            var transaccion = default(NpgsqlTransaction);

            using (var conexion = await ObtenerConexionAsync())
            {
                daOrden = new DA.Orden(conexion);
                beOrden = await daOrden.ObtenerPorIdAsync(beOrden.OrdenId);

                if (beOrden == null) throw new UserException("La orden indicada no existe", HttpStatusCode.NotFound);
                else if (beOrden.EstadoValor != BE.OrdenEstado.EnProceso) throw new UserException("La orden indicada no se encuentra en proceso", HttpStatusCode.Conflict);

                daMesa = new DA.Mesa(conexion);
                beMesa = await daMesa.ObtenerPorIdAsync(beOrden.MesaId);

                if (beMesa == null) throw new UserException("La mesa indicada no existe", HttpStatusCode.NotFound);
                else if (beMesa.EstadoValor != BE.MesaEstado.Ocupada) throw new UserException("La mesa indicada no se encuentra ocupada", HttpStatusCode.Conflict);

                transaccion = conexion.BeginTransaction();

                beOrden.EstadoValor = BE.OrdenEstado.Finalizada;
                await daOrden.ActualizarAsync(beOrden);

                beMesa.OrdenId = null;
                beMesa.EstadoValor = BE.MesaEstado.Disponible;
                await daMesa.ActualizarAsync(beMesa);

                await transaccion.CommitAsync();
            }

            return beOrden;
        }

        public async ValueTask<BE.Orden> InsertarAsync(BE.Orden beOrden)
        {
            var daMesa = default(DA.Mesa);
            var daOrden = default(DA.Orden);
            var beMesa = default(BE.Mesa);
            var transaccion = default(NpgsqlTransaction);

            if (beOrden == null) throw new UserException("El objeto enviado esta nulo", HttpStatusCode.BadRequest);
            else if (beOrden.MesaId <= 0) throw new UserException($"La propiedad {nameof(BE.Orden.MesaId)} no puede estar vacía o nula", HttpStatusCode.BadRequest);

            using (var conexion = await ObtenerConexionAsync())
            {
                daMesa = new DA.Mesa(conexion);
                beMesa = await daMesa.ObtenerPorIdAsync(beOrden.MesaId);

                if (beMesa == null) throw new UserException("La mesa indicada no existe", HttpStatusCode.NotFound);
                else if (beMesa.EstadoValor != BE.MesaEstado.Disponible) throw new UserException("La mesa indicada no se encuentra disponible", HttpStatusCode.Conflict);

                transaccion = conexion.BeginTransaction();

                beOrden.OrdenId = Guid.NewGuid();
                beOrden.FechaCreacion = DateTime.Now;
                beOrden.EstadoValor = BE.OrdenEstado.EnProceso;
                beOrden.Total = 0M;
                beOrden.ContadorPedidos = 0;

                daOrden = new DA.Orden(conexion);
                await daOrden.InsertarAsync(beOrden);

                beMesa.OrdenId = beOrden.OrdenId;
                beMesa.EstadoValor = BE.MesaEstado.Ocupada;
                await daMesa.ActualizarAsync(beMesa);

                await transaccion.CommitAsync();
            }

            return beOrden;
        }
        public async ValueTask<BE.Orden> ActualizarAsync(BE.Orden beOrden)
        {
            if (beOrden == null) throw new UserException("El objeto enviado esta nulo", HttpStatusCode.BadRequest);

            switch (beOrden.EstadoValor)
            {
                case BE.OrdenEstado.Finalizar:
                    return await ActualizarFinalizarAsync(beOrden);
                default:
                    throw new UserException($"La propiedad {nameof(BE.Orden.Estado)} es no es valida", HttpStatusCode.BadRequest);
            }
        }
    }
}
