using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class TipoPartido
    {
        public int Id { get; set; }
        public int Sets { get; set; }
        public int Puntos { get; set; }
        public string TextoDelSelect { get; set; }
    }
}
