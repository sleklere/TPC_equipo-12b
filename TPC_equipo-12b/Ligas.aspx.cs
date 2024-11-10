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
    public partial class Ligas : System.Web.UI.Page
    {
        public List<Liga> ListaLigas { get; set; }
        public int selectedLigaId;
        private List<Jugador> jugadoresAgregados;


        private void CargarLigas()
        {
            LigaNegocio negocio = new LigaNegocio();
            ListaLigas = negocio.listarLigas();

            if (!IsPostBack)
            {
                jugadoresAgregados = new List<Jugador>();
                Session["JugadoresAgregados"] = jugadoresAgregados;
                CargarJugadoresAgregados();
            }
            else
            {
                jugadoresAgregados = (List<Jugador>)Session["JugadoresAgregados"];
            }

            if (ListaLigas != null && ListaLigas.Count > 0)
            {
                rptLigas.DataSource = ListaLigas;
                rptLigas.DataBind();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            CargarLigas();
        }

        protected void btnBuscarJugador_Click(object sender, EventArgs e)
        {
            JugadorNegocio jugadorNegocio = new JugadorNegocio();
            string codigoJugador = txtCodigoJugador.Text.Trim();
            lblMensajeBusqueda.Visible = false;

            Jugador jugador = jugadorNegocio.findJugadorByCodigo(codigoJugador);

            if (jugador != null)
            {
                if (!jugadoresAgregados.Any(j => j.Codigo == codigoJugador))
                {
                    jugadoresAgregados.Add(jugador);
                    Session["JugadoresAgregados"] = jugadoresAgregados;
                    CargarJugadoresAgregados(); // recargar el Repeater
                    lblMensajeBusqueda.Text = $"Jugador con código {codigoJugador} encontrado y agregado.";
                    lblMensajeBusqueda.CssClass = "text-success";
                }
                else
                {
                    lblMensajeBusqueda.Text = $"El jugador con código {codigoJugador} ya fue agregado.";
                    lblMensajeBusqueda.CssClass = "text-warning";
                }
            }
            else
            {
                lblMensajeBusqueda.Text = $"No se encontró un jugador con el código {codigoJugador}.";
                lblMensajeBusqueda.CssClass = "text-danger";
            }

            lblMensajeBusqueda.Visible = true;
        }
        protected void CargarJugadoresAgregados()
        {
            jugadoresAgregados = (List<Jugador>)Session["JugadoresAgregados"];
            rptJugadoresAgregados.DataSource = jugadoresAgregados;
            rptJugadoresAgregados.DataBind();
        }


        protected void btnCrearLiga_Click(object sender, EventArgs e)
        {
            lblModalTitle.Text = "Crear Nueva Liga";
            hfLigaId.Value = string.Empty;
            txtLigaNombre.Text = string.Empty;
            jugadoresAgregados.Clear();
            Session["JugadoresAgregados"] = jugadoresAgregados;
            CargarJugadoresAgregados();

            ScriptManager.RegisterStartupScript(this, GetType(), "showCreateModal", "openUpdateModal();", true);
        }
        protected void btnEditarLiga_Command(object sender, CommandEventArgs e)
        {
            int ligaId = Convert.ToInt32(e.CommandArgument);

            LigaNegocio negocio = new LigaNegocio();
            Liga liga = negocio.getLigaById(ligaId);

            if (liga != null)
            {
                lblModalTitle.Text = "Editar Liga";
                hfLigaId.Value = ligaId.ToString(); // asignar id al campo oculto
                txtLigaNombre.Text = liga.Nombre;

                // cargar los jugadores
                jugadoresAgregados = liga.Jugadores;
                Session["JugadoresAgregados"] = jugadoresAgregados;
                CargarJugadoresAgregados();

                ScriptManager.RegisterStartupScript(this, GetType(), "showEditModal", "openUpdateModal();", true);
            }
        }

        protected void rptJugadoresAgregados_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Eliminar")
            {
                int jugadorId = int.Parse(e.CommandArgument.ToString());

                List<Jugador> jugadoresAgregados = Session["JugadoresAgregados"] as List<Jugador> ?? new List<Jugador>();

                jugadoresAgregados.RemoveAll(j => j.Id == jugadorId);

                Session["JugadoresAgregados"] = jugadoresAgregados;

                rptJugadoresAgregados.DataSource = jugadoresAgregados;
                rptJugadoresAgregados.DataBind();
            }
        }
        protected void btnSaveLiga_Click(object sender, EventArgs e)
        {
            string ligaNombre = txtLigaNombre.Text;
            LigaNegocio negocio = new LigaNegocio();
            List<Jugador> jugadoresAgregados = Session["JugadoresAgregados"] as List<Jugador> ?? new List<Jugador>();
            bool isEdit = int.TryParse(hfLigaId.Value, out int ligaId) && ligaId > 0;

            if (isEdit)
            {
                negocio.UpdateLiga(ligaId, ligaNombre);
            }
            else
            {
                ligaId = negocio.CrearLiga(ligaNombre);
            }

            if (ligaId > 0)
            {
                bool asociacionExitosa = negocio.AsociarJugadoresALiga(ligaId, jugadoresAgregados);

                if (asociacionExitosa)
                {
                    hiddenMessage.Value = $"Liga {(isEdit ? "editada" : "creada")} correctamente.";
                    CargarLigas();
                    txtLigaNombre.Text = string.Empty;
                    ListaLigas = negocio.listarLigas();
                    Session["JugadoresAgregados"] = null;
                }
                else
                {
                    //hiddenMessage.Value = "Liga creada, pero falló la asociación con los jugadores!";
                    hiddenMessage.Value = $"Liga {(isEdit ? "editada" : "creada")}, pero falló la asociación con los jugadores!";
                }
            }
            else
            {
                hiddenMessage.Value = $"Error en la {(isEdit ? "edición" : "creación")} de la liga!";
            }
        }

        protected void deleteLiga_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(hiddenLigaId.Value))
            {
                int selectedLigaId = Convert.ToInt32(hiddenLigaId.Value);
                if (selectedLigaId != 0)
                {
                    bool deleteLiga;
                    LigaNegocio negocio = new LigaNegocio();
                    deleteLiga = negocio.DeleteLiga(selectedLigaId);

                    if (deleteLiga)
                    {
                        CargarLigas();
                        hiddenMessage.Value = "Liga eliminada correctamente.";
                    }
                    else
                    {
                        hiddenMessage.Value = "Error en la eliminación";
                    }

                }
            }
        }
    }
}