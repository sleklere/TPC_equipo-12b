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

        protected void Page_Load(object sender, EventArgs e)
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

            int ligaId = negocio.CrearLiga(ligaNombre);

            if (ligaId > 0)
            {
                bool asociacionExitosa = negocio.AsociarJugadoresALiga(ligaId, jugadoresAgregados);

                if (asociacionExitosa)
                {
                    Response.Write("<script>alert('Liga \"" + ligaNombre + "\" guardada y jugadores asociados exitosamente!');</script>");
                    txtLigaNombre.Text = string.Empty;
                    ListaLigas = negocio.listarLigas();
                    Session["JugadoresAgregados"] = null;
                }
                else
                {
                    Response.Write("<script>alert('Liga creada, pero falló la asociación con los jugadores!');</script>");
                }
            }
            else
            {
                Response.Write("<script>alert('Error en la creación de la liga!');</script>");
            }
        }

        protected void updateLiga_Click(object sender, EventArgs e)
        {
            //int selectedLiga = int.Parse(hiddenLigaId.Value); // Lee el ID desde el campo oculto
            //string ligaNombre = txtLigaNombreUpdate.Text;
            //if (selectedLiga != 0)
            //{
            //    bool updateLiga;
            //    //string ligaNombre = txtLigaNombreUpdate.Text;
            //    LigaNegocio negocio = new LigaNegocio();
            //    updateLiga = negocio.UpdateLiga(selectedLigaId, ligaNombre);

            //    Response.Write("<script>alert('Liga \"" + selectedLigaId + "\" editada exitosamente!');</script>");

            //    if (updateLiga)
            //    {
            //        Response.Write("<script>alert('Liga \"" + ligaNombre + "\" editada exitosamente!');</script>");
            //        txtLigaNombreUpdate.Text = string.Empty;
            //        ListaLigas = negocio.listarLigas();
            //    }
            //    else
            //    {
            //        Response.Write("<script>alert('Error en la edicion!');</script>");
            //    }

            //}
        }
    }
}