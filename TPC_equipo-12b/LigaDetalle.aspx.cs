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
        public Liga LigaData;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Jugador"] == null)
            {
                Response.Redirect("~/AccesoDenegado.aspx");
            } else
            {
                ListaJugadores = new List<Jugador>();
                if (!IsPostBack)
                {
                    if (Request.QueryString["id"] != null)
                    {

                        LigaNegocio negocio = new LigaNegocio();
                        string ligaId = Request.QueryString["id"].ToString();

                        LigaData = negocio.getLigaById(int.Parse(ligaId)); 
                        ListaJugadores = negocio.getJugadoresByLigaId(ligaId);

                        txtCodigoLiga.Text = LigaData.Codigo;
                    }
                }
            }
            
        }
    }
}