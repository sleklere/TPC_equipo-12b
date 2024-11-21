using Dominio;
using Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TPC_equipo_12b
{
    public partial class Versus : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Jugador"] == null)
            {
                Response.Redirect("~/AccesoDenegado.aspx");
            }
            else
            {
                if (!IsPostBack)
                {
                    LimpiarValores();
                    CargarJugadores();
                }
            }

        }

        private void CargarJugadores()
        {
            Jugador jugador = Session["Jugador"] as Jugador;

            JugadorNegocio jugadorNegocio = new JugadorNegocio();
            List<Jugador> jugadores = jugadorNegocio.listarJugadoresVersus(jugador.Id);
            ddlJugador1.DataSource = jugadores;
            ddlJugador1.DataTextField = "Username";
            ddlJugador1.DataValueField = "Id";
            ddlJugador1.DataBind();
            ddlJugador1.Items.Insert(0, new ListItem("--Seleccione un jugador--", "-1"));

            ddlJugador2.DataSource = jugadores;
            ddlJugador2.DataTextField = "Username";
            ddlJugador2.DataValueField = "Id";
            ddlJugador2.DataBind();
            ddlJugador2.Items.Insert(0, new ListItem("--Seleccione un jugador--", "-1"));
        }

        protected void ddlJugador1_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarVersus();
        }
        protected void ddlJugador2_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarVersus();
        }

        protected void CargarVersus()
        {
            int jugador1Id = int.Parse(ddlJugador1.SelectedValue);
            int jugador2Id = int.Parse(ddlJugador2.SelectedValue);

            if (jugador1Id == -1 || jugador2Id == -1) return;

            if (jugador1Id == jugador2Id)
            {
                hiddenMessage.Value = "Por favor selecciona dos jugadores diferentes.";
                return;
            }

            PartidoNegocio partidoNegocio = new PartidoNegocio();
            JugadorNegocio jugadorNegocio = new JugadorNegocio();
            VersusDTO datosVersus = partidoNegocio.ObtenerVersus(jugador1Id, jugador2Id);

            if (datosVersus != null)
            {
                Console.WriteLine("Datos recuperados correctamente");
                //lblJugador1Nombre.Text = datosVersus.Jugador1Nombre;
                lblJugador1Victorias.Text = datosVersus.Jugador1Victorias.ToString();
                lblJugador1TotalPartidos.Text = datosVersus.Jugador1TotalPartidos.ToString();
                string totalVictoriasJ1 = datosVersus.Jugador1TotalVictorias.ToString();
                string totalDerrotasJ1 = (datosVersus.Jugador1TotalPartidos - datosVersus.Jugador1TotalVictorias).ToString();
                lblVDHistoricoJ1.Text = totalVictoriasJ1 + "-" + totalDerrotasJ1;
                double pvJ1 = (((double)datosVersus.Jugador1TotalVictorias / (double)datosVersus.Jugador1TotalPartidos) * 100.0);
                porcentajeVictoriasJ1.Text = $"{pvJ1:F}%";
                lblRachaJ1.Text = jugadorNegocio.RachaVictorias(jugador1Id).ToString();
                lblTorneosJ1.Text = datosVersus.TorneosGanadosJugador1.ToString();

                //lblJugador2Nombre.Text = datosVersus.Jugador2Nombre;
                lblJugador2Victorias.Text = datosVersus.Jugador2Victorias.ToString();
                lblJugador2TotalPartidos.Text = datosVersus.Jugador2TotalPartidos.ToString();
                string totalVictoriasJ2 = datosVersus.Jugador2TotalVictorias.ToString();
                string totalDerrotasJ2 = (datosVersus.Jugador2TotalPartidos - datosVersus.Jugador2TotalVictorias).ToString();
                lblVDHistoricoJ2.Text = totalVictoriasJ2 + "-" + totalDerrotasJ2;
                double pvJ2 = (((double)datosVersus.Jugador2TotalVictorias / (double)datosVersus.Jugador2TotalPartidos) * 100.0);
                porcentajeVictoriasJ2.Text = $"{pvJ2:F}%";
                lblRachaJ2.Text = jugadorNegocio.RachaVictorias(jugador2Id).ToString();
                lblTorneosJ2.Text = datosVersus.TorneosGanadosJugador2.ToString();


                if (datosVersus.Jugador1Victorias == datosVersus.Jugador2Victorias)
                {
                    lblJugador1Victorias.CssClass = "";
                    lblJugador2Victorias.CssClass = "";
                }
                else
                {
                    lblJugador1Victorias.CssClass = datosVersus.Jugador1Victorias > datosVersus.Jugador2Victorias ? "versus-victoria" : "versus-derrota";
                    lblJugador2Victorias.CssClass = datosVersus.Jugador2Victorias > datosVersus.Jugador1Victorias ? "versus-victoria" : "versus-derrota";
                }

                CargarPartidosVersus(jugador1Id, jugador2Id);
            }
            else
            {
                hiddenMessage.Value = "No se encontraron datos para el head-to-head.";
                LimpiarValores();
            }
        }

        protected void LimpiarValores()
        {
            lblJugador1Victorias.Text = "";
            lblJugador1TotalPartidos.Text = "-";
            lblVDHistoricoJ1.Text = "-";
            porcentajeVictoriasJ1.Text = "-";

            lblJugador2Victorias.Text = "";
            lblJugador2TotalPartidos.Text = "-";
            lblVDHistoricoJ2.Text = "-";
            porcentajeVictoriasJ2.Text = "-";
            rptPartidos.DataSource = null;
            rptPartidos.DataBind();
            lblJugador1Victorias.CssClass = "";
            lblJugador2Victorias.CssClass = "";
        }

        protected void CargarPartidosVersus(int jugador1Id, int jugador2Id)
        {
            PartidoNegocio partidoNegocio = new PartidoNegocio();
            List<ListarPartidosDTO> partidos = partidoNegocio.listarPartidosEntreJugadores(jugador1Id, jugador2Id);
            if (partidos == null)
            {
                hiddenMessage.Value = "Error al cargar los partidos.";
            }
            rptPartidos.DataSource = partidos;
            rptPartidos.DataBind();
        }

    }
}