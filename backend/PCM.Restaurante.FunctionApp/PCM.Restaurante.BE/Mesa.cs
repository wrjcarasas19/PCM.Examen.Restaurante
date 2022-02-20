using System;
using System.Runtime.Serialization;

namespace PCM.Restaurante.BE
{
    public class Mesa
    {
        public int MesaId { get; set; }
        public int Capacidad { get; set; }
        public Guid? OrdenId { get; set; }
        public int Estado
        {
            get { return (int)EstadoValor; }
            set { EstadoValor = (MesaEstado)value; }
        }
        public string EstadoTexto
        {
            get { return EstadoValor.ToString(); }
        }

        public Orden Orden { get; set; }

        [IgnoreDataMember]
        public MesaEstado EstadoValor { get; set; }
    }
}
