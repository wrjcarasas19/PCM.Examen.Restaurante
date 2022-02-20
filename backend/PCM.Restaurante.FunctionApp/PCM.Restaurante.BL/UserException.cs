using System;
using System.Net;

namespace PCM.Restaurante.BL
{
    public class UserException : Exception
    {
        public HttpStatusCode Codigo { get; set; }

        public UserException(string mensaje, HttpStatusCode codigo) : base(mensaje)
        {
            Codigo = codigo;
        }
    }
}
