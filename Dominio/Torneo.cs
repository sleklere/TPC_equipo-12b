using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Torneo
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int LigaId { get; set; }
        public string FechaCreacion { get; set; }
        public int GanadorId { get; set; }
        public string GanadorNombre { get; set; }
        public List<Jugador> Jugadores { get; set; }
    }
}
