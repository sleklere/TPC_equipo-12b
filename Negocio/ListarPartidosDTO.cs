﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class ListarPartidosDTO
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public int TipoPartidoId { get; set; }
        public int Duracion { get; set; }
        public int Jugador1Id{ get; set; }
        public string Jugador1Nombre { get; set; }
        public int PuntosJugador1 { get; set; }
        public int Jugador2Id { get; set; }
        public string Jugador2Nombre { get; set; }
        public int PuntosJugador2 { get; set; }
        public int GanadorId { get; set; }
        public string GanadorNombre { get; set; }
        public string NombreLiga { get; set; }
        public string NombreTorneo { get; set; }
    }
}
