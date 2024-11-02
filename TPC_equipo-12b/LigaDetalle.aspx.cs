using Dominio;
using Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TPC_equipo_12b
{
    public partial class LigaDetalle : System.Web.UI.Page
    {
        public List<Jugador> ListaJugadores { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            ListaJugadores = new List<Jugador>();
            if (!IsPostBack)
            {
                if (Request.QueryString["id"] != null)
                {

                    LigaNegocio negocio = new LigaNegocio();
                    string ligaId = Request.QueryString["id"].ToString();

                    ListaJugadores = negocio.getJugadoresByLigaId(ligaId);
                }
            }
        }
    }
}