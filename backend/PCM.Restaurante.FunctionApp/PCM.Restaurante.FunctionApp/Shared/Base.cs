using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PCM.Restaurante.BL;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace PCM.Restaurante.FunctionApp
{
    public class Base
    {
        public async ValueTask<T> ObtenerObjetoAsync<T>(HttpRequest req)
        {
            try
            {
                return await Utf8Json.JsonSerializer.DeserializeAsync<T>(req.Body);
            }
            catch
            {
                throw new UserException("El objeto enviado no es valido", System.Net.HttpStatusCode.BadRequest);
            }
        }
        public IActionResult ObtenerRespuestaJson<T>(T valor)
        {
            return new ContentResult()
            {
                Content = Utf8Json.JsonSerializer.ToJsonString(valor),
                StatusCode = 200,
                ContentType = "application/json"
            };
        }
        public async ValueTask<IActionResult> EncapsuladorAsync(ILogger log, Func<ValueTask<IActionResult>> accion, [CallerMemberName] string metodo = "")
        {
            try
            {
                return await accion();
            }
            catch (UserException ux)
            {
                return new ContentResult() { Content = ux.Message, StatusCode = (int)ux.Codigo, ContentType = "text/html" };
            }
            catch (Exception ex)
            {
                log.LogError(ex, metodo);

                //TODO: Se debe remover el mensaje de la excepción en producción
                return new ContentResult() { Content = ex.Message, StatusCode = 500, ContentType = "text/html" };
            }
        }
    }
}
