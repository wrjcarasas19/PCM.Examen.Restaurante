using System;

namespace PCM.Restaurante.BL
{
    public static class Constantes
    {
#if DEBUG
        public static string CADENA_CONEXION_BD
        {
            get { return "Server=pgsql-pcm-examen.postgres.database.azure.com;Database=restaurante;Port=5432;User Id=m2ZoZdf65HTh87yS@pgsql-pcm-examen;Password=V49SrRYPagjnxAPU;"; }
        }
#else
        public static string CADENA_CONEXION_BD
        {
            get { return Environment.GetEnvironmentVariable(nameof(CADENA_CONEXION_BD), EnvironmentVariableTarget.Process) ?? throw new ArgumentNullException(nameof(CADENA_CONEXION_BD)); }
        }
#endif

    }
}
