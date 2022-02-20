using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using PCM.Restaurante.BL;

namespace PCM.Restaurante.FunctionApp
{
    public class OrdenPedido : Base
    {
        [FunctionName("OrdenPedidoObtener")]
        public async Task<IActionResult> OrdenPedidoObtenerRun([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "v1/orden/pedido")] HttpRequest req, ILogger log)
        {
            return await EncapsuladorAsync(log, async () =>
            {
                var ordenId = Guid.NewGuid();

                if (!Guid.TryParse(req.Query["ordenId"], out ordenId)) throw new UserException($"La propiedad {nameof(BE.OrdenPedido.OrdenId)} no es valida", System.Net.HttpStatusCode.BadRequest);

                return ObtenerRespuestaJson(await new BL.OrdenPedido().ListarAsync(ordenId));
            });
        }

        [FunctionName("OrdenPedidoInsertar")]
        public async Task<IActionResult> OrdenPedidoInsertarRun([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "v1/orden/pedido")] HttpRequest req, ILogger log)
        {
            return await EncapsuladorAsync(log, async () =>
            {
                var beOrdenPedido = await ObtenerObjetoAsync<BE.OrdenPedido>(req);

                return ObtenerRespuestaJson(await new BL.OrdenPedido().InsertarAsync(beOrdenPedido));
            });
        }

        [FunctionName("OrdenPedidoActualizar")]
        public async Task<IActionResult> OrdenPedidoActualizarRun([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "v1/orden/pedido")] HttpRequest req, ILogger log)
        {
            return await EncapsuladorAsync(log, async () =>
            {
                var beOrdenPedido = await ObtenerObjetoAsync<BE.OrdenPedido>(req);

                return ObtenerRespuestaJson(await new BL.OrdenPedido().ActualizarAsync(beOrdenPedido));
            });
        }

        [FunctionName("OrdenPedidoEliminar")]
        public async Task<IActionResult> OrdenPedidoEliminarRun([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "v1/orden/pedido")] HttpRequest req, ILogger log)
        {
            return await EncapsuladorAsync(log, async () =>
            {
                var beOrdenPedido = await ObtenerObjetoAsync<BE.OrdenPedido>(req);

                return ObtenerRespuestaJson(await new BL.OrdenPedido().EliminarAsync(beOrdenPedido));
            });
        }
    }
}
