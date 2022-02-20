using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace PCM.Restaurante.FunctionApp
{
    public class Producto : Base
    {
        [FunctionName("ProductoObtener")]
        public async Task<IActionResult> ProductoObtenerRun([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "v1/producto")] HttpRequest req, ILogger log)
        {
            return await EncapsuladorAsync(log, async () =>
            {
                return ObtenerRespuestaJson(await new BL.Producto().ListarAsync());
            });
        }
    }
}
