using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    internal class Partido
    {
        public int Id { get; set; }
        public string Fecha { get; set; }
        public int TipoPartidoId { get; set; }
        public int Duracion { get; set; }
    }
}
