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

                LigaNegocio negocio = new LigaNegocio();
                string ligaId = Request.QueryString["id"].ToString();

                LigaData = negocio.getLigaById(int.Parse(ligaId));

                if (!IsPostBack)
                {
                    if (Request.QueryString["id"] != null)
                    {
                        ListaJugadores = negocio.getJugadoresByLigaId(ligaId);
                        txtCodigoLiga.Text = LigaData.Codigo;
                    }

                    CargarPartidos();
                }
            }
            
        }

        private void CargarPartidos()
        {
            PartidoNegocio partidoNegocio = new PartidoNegocio();
            List<ListarPartidosDTO> partidos = partidoNegocio.listarPartidos(LigaData.Id);
            if (partidos == null)
            {
                hiddenMessage.Value = "Error al cargar los partidos.";
            }
            rptPartidos.DataSource = partidos;
            rptPartidos.DataBind();
        }

        protected void btnUpdatePartido_Click(object sender, EventArgs e)
        {
            if (
            string.IsNullOrEmpty(txtEditPuntosJugador1.Text) ||
            string.IsNullOrEmpty(txtEditPuntosJugador2.Text))
            {
                hiddenMessage.Value = "Completa los campos.";
                hiddenMessageType.Value = "error";
                return;
            }

            int selectedPartidoId = int.Parse(hiddenPartidoId.Value);

            int jugador1Id = int.Parse(hiddenUpdateJ1Id.Value);
            int jugador2Id = int.Parse(hiddenUpdateJ2Id.Value);
            int ganadorId = int.Parse(hiddenUpdateGanadorId.Value);

            int puntosJugador1 = int.Parse(txtEditPuntosJugador1.Text);
            int puntosJugador2 = int.Parse(txtEditPuntosJugador2.Text);

            PartidoNegocio partidoNegocio = new PartidoNegocio();
            bool updatePartido;
            updatePartido = partidoNegocio.ActualizarPartido(selectedPartidoId, jugador1Id, puntosJugador1, jugador2Id, puntosJugador2, ganadorId);

            if (updatePartido)
            {
                hiddenMessage.Value = "Partido editado correctamente.";
                hiddenMessageType.Value = "success";
                CargarPartidos();
            }
            else
            {
                hiddenMessage.Value = "Error en la edición.";
                hiddenMessageType.Value = "error";
            }
        }

        protected void deletePartido_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(hiddenPartidoId.Value))
            {
                int selectedPartidoId = Convert.ToInt32(hiddenPartidoId.Value);
                ClientScript.RegisterStartupScript(this.GetType(), "showIdInConsole",
                $"console.log('selectedPartidoId: {selectedPartidoId}');", true);
                if (selectedPartidoId != 0)
                {
                    bool deletePartido;
                    PartidoNegocio negocio = new PartidoNegocio();
                    deletePartido = negocio.EliminarPartido(selectedPartidoId);
                    ClientScript.RegisterStartupScript(this.GetType(), "showIdInConsole",
                    $"console.log('deletePartido: {deletePartido}');", true);

                    if (deletePartido)
                    {
                        CargarPartidos();
                        hiddenMessage.Value = "Partido eliminada correctamente.";
                        hiddenMessageType.Value = "success";
                    }
                    else
                    {
                        hiddenMessage.Value = "Error en la eliminación";
                        hiddenMessageType.Value = "error";
                    }

                }
            }
        }
    }
}