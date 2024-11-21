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
    public partial class TorneoDetalle : System.Web.UI.Page
    {
        public Torneo TorneoData;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Jugador"] == null)
            {
                Response.Redirect("~/AccesoDenegado.aspx");
            }
            else
            {
                TorneoNegocio negocio = new TorneoNegocio();
                string torneoId = Request.QueryString["id"].ToString();

                TorneoData = negocio.GetTorneoById(int.Parse(torneoId));

                if (!IsPostBack)
                {
                    CargarRondasConPartidos();
                }
            }
        }

        private void CargarRondasConPartidos()
        {
            TorneoNegocio torneoNegocio = new TorneoNegocio();
            List<RondaPartidosDTO> rondasConPartidos = null;

            try
            {
                rondasConPartidos = torneoNegocio.ListarRondasConPartidos(TorneoData.Id);

                if (rondasConPartidos != null && rondasConPartidos.Count > 0)
                {
                    rptRondas.DataSource = rondasConPartidos;
                    rptRondas.DataBind();

                    //foreach (RondaPartidosDTO ronda in rondasConPartidos)
                    //{
                    //    if (ronda.Partidos != null && ronda.Partidos.Count > 0)
                    //    {
                    //        rptRondas
                    //    }

                    //}
                }
                //else
                //{
                //    lblSinDatos.Text = "No hay partidos disponibles para este torneo.";
                //    lblSinDatos.Visible = true;
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al cargar las rondas con partidos: " + ex.Message);
            }
        }

        protected void rptRondas_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                // obtengo el dto asociado al item actual
                RondaPartidosDTO rondaPartidos = (RondaPartidosDTO)e.Item.DataItem;

                // buscar el pepeater anidado en el item actual
                Repeater rptPartidos = (Repeater)e.Item.FindControl("rptPartidos");

                if (rptPartidos != null)
                {
                    rptPartidos.DataSource = rondaPartidos.Partidos;
                    rptPartidos.DataBind();
                }
            }
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
            int tipoPartido= int.Parse(hiddenTipoPartido.Value);

            int puntosJugador1 = int.Parse(txtEditPuntosJugador1.Text);
            int puntosJugador2 = int.Parse(txtEditPuntosJugador2.Text);


            if (tipoPartido == 1)
            {
                if (puntosJugador1 < 11 && puntosJugador2 < 11)
                {
                    hiddenMessage.Value = "El partido es a 11 puntos";
                    hiddenMessageType.Value = "error";
                    return;
                }
                if (puntosJugador1 > 9 && puntosJugador2 > 9 && Math.Abs(puntosJugador1 - puntosJugador2) != 2)
                {
                    hiddenMessage.Value = "La diferencia debe ser de 2 puntos.";
                    hiddenMessageType.Value = "error";
                    return;
                }
            }
            if (tipoPartido == 2)
            {
                if (puntosJugador1 < 21 && puntosJugador2 < 21)
                {
                    hiddenMessage.Value = "El partido es a 21 puntos";
                    hiddenMessageType.Value = "error";
                    return;
                }
                if (puntosJugador1 > 19 && puntosJugador2 > 19 && Math.Abs(puntosJugador1 - puntosJugador2) != 2)
                {
                    hiddenMessage.Value = "La diferencia debe ser de 2 puntos.";
                    hiddenMessageType.Value = "error";
                    return;
                }
            }

            PartidoNegocio partidoNegocio = new PartidoNegocio();
            bool updatePartido;
            updatePartido = partidoNegocio.ActualizarPartido(selectedPartidoId, jugador1Id, puntosJugador1, jugador2Id, puntosJugador2, ganadorId);

            if (updatePartido)
            {
                hiddenMessage.Value = "Partido editado correctamente.";
                hiddenMessageType.Value = "success";
                //CargarPartidos();
                //CargarJugadorPorLigaParaListado();
                CargarRondasConPartidos();
            }
            else
            {
                hiddenMessage.Value = "Error en la edición.";
                hiddenMessageType.Value = "error";
            }
        }

    }
}