using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    internal class Torneo
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public string TipoTorneo { get; set; }
        public string Estado { get; set; }
    }
}
