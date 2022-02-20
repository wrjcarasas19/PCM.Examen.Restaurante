using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace PCM.Restaurante.FunctionApp
{
    public class Orden : Base
    {
        [FunctionName("OrdenInsertar")]
        public async Task<IActionResult> OrdenInsertarRun([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "v1/orden")] HttpRequest req, ILogger log)
        {
            return await EncapsuladorAsync(log, async () =>
            {
                var beOrden = await ObtenerObjetoAsync<BE.Orden>(req);

                return ObtenerRespuestaJson(await new BL.Orden().InsertarAsync(beOrden));
            });
        }
        
        [FunctionName("OrdenActualizar")]
        public async Task<IActionResult> OrdenActualizarRun([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "v1/orden")] HttpRequest req, ILogger log)
        {
            return await EncapsuladorAsync(log, async () =>
            {
                var beOrden = await ObtenerObjetoAsync<BE.Orden>(req);

                return ObtenerRespuestaJson(await new BL.Orden().ActualizarAsync(beOrden));
            });
        }
    }
}
