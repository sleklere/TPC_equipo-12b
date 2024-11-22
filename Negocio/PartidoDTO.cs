using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class PartidoDTO
    {
        public int PartidoId { get; set; }
        public DateTime Fecha { get; set; }
        public int Jugador1Id{ get; set; }
        public string NombreJugador1 { get; set; }
        public string ApellidoJugador1 { get; set; }
        public int PuntosJugador1 { get; set; }
        public int Jugador2Id{ get; set; }
        public string NombreJugador2 { get; set; }
        public string ApellidoJugador2 { get; set; }
        public int PuntosJugador2 { get; set; }
        public bool EsGanador { get; set; }
        public string NombreLiga { get; set; }
        public string NombreTorneo{ get; set; }
        public int GanadorId { get; set; }
        public int TipoPartidoId { get; set; }
    }
}
