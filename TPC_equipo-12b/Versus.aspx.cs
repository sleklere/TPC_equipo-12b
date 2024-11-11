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
    public partial class Versus : System.Web.UI.Page
    {
        VersusDTO datosVersus;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Jugador"] == null)
            {
                Response.Redirect("~/AccesoDenegado.aspx");
            } else
            {
                if (!IsPostBack)
                {
                    CargarJugadores();
                }
            }
            
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

        protected void ddlJugador1_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarVersus(sender, e);
        }
        protected void ddlJugador2_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarVersus(sender, e);
        }

        protected void CargarVersus(object sender, EventArgs e)
        {
            // Obtener los IDs seleccionados
            int jugador1Id = int.Parse(ddlJugador1.SelectedValue);
            int jugador2Id = int.Parse(ddlJugador2.SelectedValue);

            // Verificar que ambos jugadores estén seleccionados y no sean el mismo
            if (jugador1Id > 0 && jugador2Id > 0 && jugador1Id != jugador2Id)
            {
                // Llamar al método ObtenerHeadToHead en la capa de Negocio
                PartidoNegocio partidoNegocio = new PartidoNegocio();
                datosVersus = partidoNegocio.ObtenerVersus(jugador1Id, jugador2Id);

                // Validar que se obtuvieron datos y mostrarlos en la interfaz
                if (datosVersus != null)
                {
                    Console.WriteLine("Datos recuperados correctamente");
                    lblJugador1Nombre.Text = datosVersus.Jugador1Nombre;
                    lblJugador1Victorias.Text = datosVersus.Jugador1Victorias.ToString();

                    lblJugador2Nombre.Text = datosVersus.Jugador2Nombre;
                    lblJugador2Victorias.Text = datosVersus.Jugador2Victorias.ToString();
                }
                else
                {
                    hiddenMessage.Value = "No se encontraron datos para el head-to-head.";
                }
            }
            else
            {
                hiddenMessage.Value = "Por favor selecciona dos jugadores diferentes.";
            }
        }

    }
}