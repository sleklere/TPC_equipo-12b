using Dominio;
using Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace TPC_equipo_12b
{
    public partial class Torneos : System.Web.UI.Page
    {
        public List<Torneo> ListaTorneos { get; set; }
        public int selectedTorneoId;
        public List<Liga> ListaLigas;


        private void CargarTorneos()
        {
            TorneoNegocio negocio = new TorneoNegocio();
            Jugador jugadorLogueado = (Jugador)Session["Jugador"];
            ListaTorneos = negocio.ListarTorneos(jugadorLogueado.Id);

            if (ListaTorneos != null && ListaTorneos.Count > 0)
            {
                rptTorneos.DataSource = ListaTorneos;
                rptTorneos.DataBind();
                lblSinTorneos.Visible = false;
            } else
            {
                rptTorneos.DataSource = null;
                rptTorneos.DataBind();
                lblSinTorneos.Visible = true;
            }
        }

        private void CargarOpcionesLigas()
        {
            LigaNegocio negocio = new LigaNegocio();
            Jugador jugadorLogueado = (Jugador)Session["Jugador"];

            ListaLigas = negocio.ListarLigasJugador(jugadorLogueado.Id);

            if (ListaLigas != null && ListaLigas.Count > 0)
            {
                ddlLiga.DataSource = ListaLigas;
                ddlLiga.DataTextField = "Nombre";
                ddlLiga.DataValueField = "Id";
                ddlLiga.DataBind();
                ddlLiga.Items.Insert(0, new ListItem("Seleccionar", ""));
            }

        }

        protected void ddlLiga_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlLiga.SelectedValue))
            {
                int ligaId = int.Parse(ddlLiga.SelectedValue);
                CargarJugadoresPorLiga(ligaId);

                // evitar cierre del modal
                ScriptManager.RegisterStartupScript(this, GetType(), "showModal", "openModal();", true);
            }
            else
            {
                rptJugadores.DataSource = null;
                rptJugadores.DataBind();
            }
        }

        private void CargarJugadoresPorLiga(int ligaId)
        {
            JugadorNegocio jugadorNegocio = new JugadorNegocio();
            List<Jugador> jugadores = jugadorNegocio.ListarJugadoresByLiga(ligaId);

            if (jugadores != null && jugadores.Count > 0)
            {
                rptJugadores.DataSource = jugadores;
                rptJugadores.DataBind();
            }
            else
            {
                rptJugadores.DataSource = null;
                rptJugadores.DataBind();
            }
        }

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
                    CargarTorneos();
                    CargarOpcionesLigas();
                }
            }

        }

        protected void btnCrearTorneo_Click(object sender, EventArgs e)
        {
            lblModalTitle.Text = "Crear Nuevo Torneo";
            hfTorneoId.Value = string.Empty;
            txtTorneoNombre.Text = string.Empty;

            ScriptManager.RegisterStartupScript(this, GetType(), "showCreateModal", "openModal();", true);
        }

        protected void btnEditarTorneo_Command(object sender, CommandEventArgs e)
        {
            int torneoId = Convert.ToInt32(e.CommandArgument);

            TorneoNegocio negocio = new TorneoNegocio();
            Torneo torneo = negocio.GetTorneoById(torneoId);

            if (torneo != null)
            {
                lblModalTitle.Text = "Editar Torneo";
                hfTorneoId.Value = torneoId.ToString();
                txtTorneoNombre.Text = torneo.Nombre;

                ScriptManager.RegisterStartupScript(this, GetType(), "showEditModal", "openUpdateModal();", true);
            }
        }

        protected void btnSaveTorneo_Click(object sender, EventArgs e)
        {
            string torneoNombre = txtTorneoNombre.Text;
            var jugadoresSeleccionadosIds = new List<int>();

            foreach (RepeaterItem item in rptJugadores.Items)
            {
                var chkJugador = (HtmlInputCheckBox)item.FindControl("Checkbox1");
                if (chkJugador != null && chkJugador.Checked)
                {
                    int jugadorId = int.Parse(chkJugador.Value);
                    jugadoresSeleccionadosIds.Add(jugadorId);
                }
            }

            string mensajeError = null;

            if (string.IsNullOrEmpty(torneoNombre))
            {
                mensajeError = "El torneo debe tener un nombre.";
            }
            else if (jugadoresSeleccionadosIds.Count % 2 != 0)
            {
                mensajeError = "La cantidad de jugadores debe ser una potencia de 2 (4, 8, 16, etc)";
            }
            else if (jugadoresSeleccionadosIds.Count == 0)
            {
                mensajeError = "No es posible crear un torneo sin jugadores involucrados.";
            }

            if (mensajeError != null)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showCreateModal", "openModal();", true);
                hiddenMessage.Value = mensajeError;
                hiddenMessageType.Value = "error";
                return;
            }

            TorneoNegocio negocio = new TorneoNegocio();
            bool isEdit = int.TryParse(hfTorneoId.Value, out int torneoId) && torneoId > 0;
            Jugador jugadorSesion = (Jugador)Session["Jugador"];

            if (isEdit)
            {
                negocio.UpdateTorneo(torneoId, torneoNombre);
            }
            else
            {
                string ligaId = ddlLiga.SelectedValue;
                torneoId = negocio.CrearTorneo(torneoNombre, int.Parse(ddlLiga.SelectedValue));

                if (torneoId > 0)
                {
                    negocio.AsociarJugadores(torneoId, jugadoresSeleccionadosIds);
                    negocio.GenerarPartidos(torneoId, jugadoresSeleccionadosIds);
                }
            }

            if (torneoId > 0)
            {
                txtTorneoNombre.Text = string.Empty;
                hiddenMessage.Value = isEdit ? $"Torneo editado correctamente." : "Torneo creado correctamente.";
                hiddenMessageType.Value = "success";
                CargarTorneos();
            }
            else
            {
                hiddenMessage.Value = $"Error en la {(isEdit ? "edición" : "creación")} del torneo!";
                hiddenMessageType.Value = "error";
            }
        }

        protected void deleteTorneo_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(hiddenTorneoId.Value))
            {
                int selectedTorneoId = Convert.ToInt32(hiddenTorneoId.Value);
                if (selectedTorneoId != 0)
                {
                    bool deleteTorneo;
                    TorneoNegocio negocio = new TorneoNegocio();
                    deleteTorneo = negocio.DeleteTorneo(selectedTorneoId);

                    if (deleteTorneo)
                    {
                        CargarTorneos();
                        hiddenMessage.Value = "Torneo eliminado correctamente.";
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