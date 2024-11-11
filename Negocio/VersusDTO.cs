using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class VersusDTO
    {
        public int Jugador1Id { get; set; }
        public string Jugador1Nombre { get; set; }
        public int Jugador1Victorias { get; set; }
        public int Jugador1Derrotas { get; set; }
        public int Jugador1TotalPartidos { get; set; }
        public int Jugador1TotalVictorias { get; set; }

        public int Jugador2Id { get; set; }
        public string Jugador2Nombre { get; set; }
        public int Jugador2Victorias { get; set; }
        public int Jugador2Derrotas { get; set; }
        public int Jugador2TotalPartidos { get; set; }
        public int Jugador2TotalVictorias { get; set; }
    }
}
