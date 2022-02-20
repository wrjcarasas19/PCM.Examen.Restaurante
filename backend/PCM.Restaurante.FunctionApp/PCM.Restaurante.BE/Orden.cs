using System;
using System.Runtime.Serialization;

namespace PCM.Restaurante.BE
{
    public class Orden
    {
        public Guid OrdenId { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int MesaId { get; set; }
        public decimal Total { get; set; }
        public int ContadorPedidos { get; set; }
        public int Estado
        {
            get { return (int)EstadoValor; }
            set { EstadoValor = (OrdenEstado)value; }
        }
        public string EstadoTexto
        {
            get { return EstadoValor.ToString(); }
        }

        [IgnoreDataMember]
        public OrdenEstado EstadoValor { get; set; }
    }
}
