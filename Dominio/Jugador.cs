using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Jugador
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Username { get; set; }
        public string Codigo { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string FechaCreacion { get; set; }
        public int PartidosGanados{ get; set; }
        public int PartidosPerdidos{ get; set; }
        public int PartidosJugados{ get; set; }
        public List<Liga> Ligas { get; set; } = new List<Liga>();
    }
}
