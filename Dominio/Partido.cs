using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Partido
    {
        public int Id { get; set; }
        public string Fecha { get; set; }
        public int TipoPartidoId { get; set; }
        public int Duracion { get; set; }
        public string Jugador1Nombre { get; set; }
        public int PuntosJugador1 { get; set; }
        public string Jugador2Nombre { get; set; }
        public int PuntosJugador2 { get; set; }
    }
}
