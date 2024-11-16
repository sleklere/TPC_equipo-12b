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
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["Jugador"] == null)
            {
                Response.Redirect("~/AccesoDenegado.aspx");
            } else
            {
                if (!IsPostBack)
                {
                    CargarLigas();
                    CargarJugadores();
                    CargarPartidos();
                }
            }

            //if (Session["Jugador"] != null)
            //{
            //    var jugador = (Jugador)Session["Jugador"];
            //    System.Diagnostics.Debug.WriteLine("Jugador en inicio:");
            //    System.Diagnostics.Debug.WriteLine("ID: " + jugador.Id);
            //    System.Diagnostics.Debug.WriteLine("Nombre: " + jugador.Nombre);
            //}
         
        }

        private void CargarLigas()
        {
            LigaNegocio ligaNegocio = new LigaNegocio();
            ddlLiga.DataSource = ligaNegocio.listarLigas();
            ddlLiga.DataTextField = "Nombre";
            ddlLiga.DataValueField = "Id";
            ddlLiga.DataBind();
        }

        private void CargarJugadores()
        {
            JugadorNegocio jugadorNegocio = new JugadorNegocio();
            List<Jugador> jugadores = jugadorNegocio.listarJugadores();
            ddlJugador1.DataSource = jugadores;
            ddlJugador1.DataTextField = "Nombre";
            ddlJugador1.DataValueField = "Id";
            ddlJugador1.DataBind();

            ddlJugador2.DataSource = jugadores;
            ddlJugador2.DataTextField = "Nombre";
            ddlJugador2.DataValueField = "Id";
            ddlJugador2.DataBind();
        }

        private void CargarPartidos()
        {
            PartidoNegocio partidoNegocio = new PartidoNegocio();
            List<Partido> partidos = partidoNegocio.listarPartidos();
            if (partidos == null)
            {
                hiddenMessage.Value = "Error al cargar los partidos.";
            }
            rptPartidos.DataSource = partidos;
            rptPartidos.DataBind();
        }

        protected void btnGuardarPartido_Click(object sender, EventArgs e)
        {
            PartidoNegocio partidoNegocio = new PartidoNegocio();
            int ligaId = int.Parse(ddlLiga.SelectedValue);

            int jugador1Id = int.Parse(ddlJugador1.SelectedValue);
            int jugador2Id = int.Parse(ddlJugador2.SelectedValue);

            int puntosJugador1 = int.Parse(txtPuntosJugador1.Text);
            int puntosJugador2 = int.Parse(txtPuntosJugador2.Text);

            var jugadoresConPuntos = new List<(int jugadorId, int puntos)>
    {
        (jugador1Id, puntosJugador1),
        (jugador2Id, puntosJugador2)
    };

            if (int.TryParse(hfPartidoId.Value, out int partidoId) && partidoId > 0)
            {
                //partidoNegocio.ActualizarPartido(partidoId, resultado, jugadoresConPuntos);
            }
            else
            {
                partidoNegocio.CrearPartido(ligaId, jugadoresConPuntos);
                //hiddenMessage.Value = $"Partido {(isEdit ? "editado" : "creado")} correctamente.";
                hiddenMessage.Value = $"Partido creado correctamente.";
            }

            LimpiarFormulario();
            CargarPartidos();
        }

        private void LimpiarFormulario()
        {
            hfPartidoId.Value = string.Empty;
            txtResultado.Text = string.Empty;
            txtPuntosJugador1.Text = string.Empty;
            txtPuntosJugador2.Text = string.Empty;
            ddlJugador1.ClearSelection();
            ddlJugador2.ClearSelection();
            lblFormularioTitulo.Text = "Crear Partido";
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            hfPartidoId.Value = string.Empty;
            txtResultado.Text = string.Empty;
            lblFormularioTitulo.Text = "Crear Partido";
        }

        protected void gvPartidos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int partidoId = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "Editar")
            {
                //Partido partido = ObtenerPartidoPorId(partidoId); 
                //hfPartidoId.Value = partido.Id.ToString();
                //ddlLiga.SelectedValue = partido.LigaId.ToString();
                //ddlJugador1.SelectedValue = partido.Jugador1Id.ToString();
                //ddlJugador2.SelectedValue = partido.Jugador2Id.ToString();
                //txtResultado.Text = partido.Resultado;
                lblFormularioTitulo.Text = "Editar Partido";
            }
            else if (e.CommandName == "Eliminar")
            {
                PartidoNegocio partidoNegocio = new PartidoNegocio();
                partidoNegocio.EliminarPartido(partidoId);
                CargarPartidos();
            }
        }
    }
}