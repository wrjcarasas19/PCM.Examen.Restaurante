using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace PCM.Restaurante.FunctionApp
{
    public class ProductoEtiqueta : Base
    {
        [FunctionName("ProductoEtiquetaObtener")]
        public async Task<IActionResult> ProductoEtiquetaObtenerRun([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "v1/producto/etiqueta")] HttpRequest req, ILogger log)
        {
            return await EncapsuladorAsync(log, async () =>
            {
                return ObtenerRespuestaJson(await new BL.ProductoEtiqueta().ListarAsync());
            });
        }

    }
}
