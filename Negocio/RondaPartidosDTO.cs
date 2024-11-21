using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class RondaPartidosDTO
    {
        public RondaPartidosDTO(Ronda ronda, List<PartidoDTO> partidos) {
            this.Ronda = ronda;
            this.Partidos = partidos;
        }

        public Ronda Ronda { get; set; }
        public List<PartidoDTO> Partidos { get; set; }
    }
}
