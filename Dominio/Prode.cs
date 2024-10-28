using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    internal class Prode
    {
        public int Id { get; set; }
        public int PartidoId { get; set; }
        public int JugadorId { get; set; }
        public string Apuesta { get; set; }
    }
}
