using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace PCM.Restaurante.FunctionApp
{
    public class Mesa : Base
    {
        [FunctionName("MesaObtener")]
        public async Task<IActionResult> MesaObtenerRun([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "v1/mesa")] HttpRequest req, ILogger log)
        {
            return await EncapsuladorAsync(log, async () =>
            {
                var mesaId = 0;

                if (int.TryParse(req.Query["mesaId"], out mesaId)) return ObtenerRespuestaJson(await new BL.Mesa().SeleccionarAsync(mesaId));
                else return ObtenerRespuestaJson(await new BL.Mesa().ListarAsync());
            });
        }
    }
}
