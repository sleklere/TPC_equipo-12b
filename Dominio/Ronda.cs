using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Ronda
    {
        public Ronda(int id, int numero, string nombre, int torneoId)
        {
            this.Id = id;
            this.Numero = numero;
            this.Nombre = nombre;
            this.TorneoId = torneoId;
        }
        public Ronda(int id, int numero, string nombre)
        {
            this.Id = id;
            this.Numero = numero;
            this.Nombre = nombre;
        }

        public int Id {  get; set; }
        public int Numero {  get; set; }
        public string Nombre {  get; set; }
        public int TorneoId {  get; set; }
    }
}
