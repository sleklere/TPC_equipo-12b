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
    public partial class Profile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Jugador"] == null)
            {
                Response.Redirect("~/AccesoDenegado.aspx");
            } else
            {
                Jugador jugador = (Jugador)Session["Jugador"];
                int idJugador = jugador.Id;

                PartidoNegocio negocio = new PartidoNegocio();

                List<PartidoDTO> list = negocio.Ultimos10PartidosByJugadorID(idJugador);

                PartidosRepeater.DataSource = list;
                PartidosRepeater.DataBind();
            }
        }

        public void btnEditar_Click(object sender, EventArgs e)
        {

        }

        public void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            Session.Abandon(); 
            Response.Redirect("~/"); 
        }
    }
}