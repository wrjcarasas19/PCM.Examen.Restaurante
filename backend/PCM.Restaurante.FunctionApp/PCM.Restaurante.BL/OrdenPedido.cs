using Npgsql;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace PCM.Restaurante.BL
{
    public class OrdenPedido : Base
    {
        public async ValueTask<List<BE.OrdenPedido>> ListarAsync(Guid ordenId)
        {
            using (var conexion = await ObtenerConexionAsync())
            {
                return await new DA.OrdenPedido(conexion).ListarPorOrdenAsync(ordenId);
            }
        }
        public async ValueTask<List<BE.OrdenPedido>> InsertarAsync(BE.OrdenPedido beOrdenPedido)
        {
            var daOrden = default(DA.Orden);
            var daOrdenPedido = default(DA.OrdenPedido);
            var beOrden = default(BE.Orden);
            var transaccion = default(NpgsqlTransaction);

            if (beOrdenPedido.ProductoId <= 0) throw new UserException($"La propiedad {nameof(BE.OrdenPedido.ProductoId)} no es valida", HttpStatusCode.BadRequest);
            else if (beOrdenPedido.Cantidad <= 0) throw new UserException($"La propiedad {nameof(BE.OrdenPedido.Cantidad)} no es valida", HttpStatusCode.BadRequest);

            using (var conexion = await ObtenerConexionAsync())
            {
                daOrden = new DA.Orden(conexion);
                beOrden = await daOrden.ObtenerPorIdAsync(beOrdenPedido.OrdenId);

                if (beOrden == null) throw new UserException("La orden indicada no existe", HttpStatusCode.NotFound);
                else if (beOrden.EstadoValor != BE.OrdenEstado.EnProceso) throw new UserException("La orden indicada no se encuentra en proceso", HttpStatusCode.Conflict);

                transaccion = conexion.BeginTransaction();

                daOrdenPedido = new DA.OrdenPedido(conexion);
                if (!await daOrdenPedido.InsertarAsync(beOrdenPedido)) throw new UserException("El producto indicado no existe", HttpStatusCode.NotFound);

                await daOrden.ActualizarAsync(beOrden);

                await transaccion.CommitAsync();

                return await daOrdenPedido.ListarPorOrdenAsync(beOrden.OrdenId);
            }

        }
        public async ValueTask<List<BE.OrdenPedido>> ActualizarAsync(BE.OrdenPedido beOrdenPedido)
        {
            var daOrden = default(DA.Orden);
            var daOrdenPedido = default(DA.OrdenPedido);
            var beOrden = default(BE.Orden);
            var transaccion = default(NpgsqlTransaction);

            if (beOrdenPedido.ProductoId <= 0) throw new UserException($"La propiedad {nameof(BE.OrdenPedido.ProductoId)} no es valida", HttpStatusCode.BadRequest);
            else if (beOrdenPedido.Cantidad <= 0) throw new UserException($"La propiedad {nameof(BE.OrdenPedido.Cantidad)} no es valida", HttpStatusCode.BadRequest);

            using (var conexion = await ObtenerConexionAsync())
            {
                daOrden = new DA.Orden(conexion);
                beOrden = await daOrden.ObtenerPorIdAsync(beOrdenPedido.OrdenId);

                if (beOrden == null) throw new UserException("La orden indicada no existe", HttpStatusCode.NotFound);
                else if (beOrden.EstadoValor != BE.OrdenEstado.EnProceso) throw new UserException("La orden indicada no se encuentra en proceso", HttpStatusCode.Conflict);

                transaccion = conexion.BeginTransaction();

                daOrdenPedido = new DA.OrdenPedido(conexion);
                if (!await daOrdenPedido.ActualizarAsync(beOrdenPedido)) throw new UserException("El pedido indicado no existe", HttpStatusCode.NotFound);

                await daOrden.ActualizarAsync(beOrden);

                await transaccion.CommitAsync();

                return await daOrdenPedido.ListarPorOrdenAsync(beOrden.OrdenId);
            }

        }
        public async ValueTask<List<BE.OrdenPedido>> EliminarAsync(BE.OrdenPedido beOrdenPedido)
        {
            var daOrden = default(DA.Orden);
            var daOrdenPedido = default(DA.OrdenPedido);
            var beOrden = default(BE.Orden);
            var transaccion = default(NpgsqlTransaction);

            if (beOrdenPedido.ProductoId <= 0) throw new UserException($"La propiedad {nameof(BE.OrdenPedido.ProductoId)} no es valida", HttpStatusCode.BadRequest);
            
            using (var conexion = await ObtenerConexionAsync())
            {
                daOrden = new DA.Orden(conexion);
                beOrden = await daOrden.ObtenerPorIdAsync(beOrdenPedido.OrdenId);

                if (beOrden == null) throw new UserException("La orden indicada no existe", HttpStatusCode.NotFound);
                else if (beOrden.EstadoValor != BE.OrdenEstado.EnProceso) throw new UserException("La orden indicada no se encuentra en proceso", HttpStatusCode.Conflict);

                transaccion = conexion.BeginTransaction();

                daOrdenPedido = new DA.OrdenPedido(conexion);
                if (!await daOrdenPedido.EliminarAsync(beOrdenPedido)) throw new UserException("El pedido indicado no existe", HttpStatusCode.NotFound);

                await daOrden.ActualizarAsync(beOrden);

                await transaccion.CommitAsync();

                return await daOrdenPedido.ListarPorOrdenAsync(beOrden.OrdenId);
            }

        }
    }
}
