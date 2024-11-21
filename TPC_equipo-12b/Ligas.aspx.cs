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

            Jugador jugador = Session["Jugador"] as Jugador;

            if (jugador == null)
            {
                hiddenMessage.Value = "No se encontró información del usuario en la sesión.";
                hiddenMessageType.Value = "error";
                return;
            }

            ListaLigas = negocio.listarLigas(jugador.Id);

            if (ListaLigas != null && ListaLigas.Count > 0)
            {
                rptLigas.DataSource = ListaLigas;
                rptLigas.DataBind();
                lblSinLigas.Visible = false;
            }
            else
            {
                rptLigas.DataSource = null;
                rptLigas.DataBind();
                lblSinLigas.Visible = true;
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
                CargarLigas();
            }

        }

        protected void btnCrearLiga_Click(object sender, EventArgs e)
        {
            lblModalTitle.Text = "Crear Nueva Liga";
            hfLigaId.Value = string.Empty;
            txtLigaNombre.Text = string.Empty;

            ScriptManager.RegisterStartupScript(this, GetType(), "showCreateModal", "openUpdateModal();", true);
        }

        protected void btnUnirseLiga_Click(object sender, EventArgs e)
        {
            LigaNegocio ligaNegocio = new LigaNegocio();
            lblModalTitle.Text = "Unirse a una liga";
            hfLigaId.Value = string.Empty;
            txtLigaNombre.Text = string.Empty;
            Jugador jugadorSesion = (Jugador)Session["Jugador"];

            ScriptManager.RegisterStartupScript(this, GetType(), "showJoinModal", "openJoinModal();", true);
        }

        protected void btnJoinLiga_Click(object sender, EventArgs e)
        {
            string ligaCodigo = txtCodigoLiga.Text;
            LigaNegocio negocio = new LigaNegocio();
            Jugador jugadorSesion = (Jugador)Session["Jugador"];

            // buscar liga con codigo ingresado
            Liga liga = negocio.getLigaByCodigo(ligaCodigo);
            // si no se encuentra avisar
            if (liga == null)
            {
                hiddenMessage.Value = "No se encontró ninguna liga con el código ingresado.";
                hiddenMessageType.Value = "error";
            }
            else
            {
                // si se encuentra
                // chequear que el jugador no este unido a la liga
                if (liga.Jugadores.Find(j => j.Id == jugadorSesion.Id) != null)
                {
                    // si lo esta avisar
                    hiddenMessage.Value = "Ya estás participando en la liga.";
                    hiddenMessageType.Value = "error";
                }
                else
                {
                    // si no lo esta asociarlo
                    bool jugadorAsociado = negocio.AsociarJugadorALiga(liga.Id, jugadorSesion.Id);

                    if (jugadorAsociado)
                    {
                        hiddenMessage.Value = "Te has unido a la liga.";
                        hiddenMessageType.Value = "success";
                        CargarLigas();
                    }
                    else
                    {
                        hiddenMessage.Value = "Error al unirse a la liga.";
                        hiddenMessageType.Value = "error";
                    }

                }
            }
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

                ScriptManager.RegisterStartupScript(this, GetType(), "showEditModal", "openUpdateModal();", true);
            }
        }

        protected void btnSaveLiga_Click(object sender, EventArgs e)
        {
            string ligaNombre = txtLigaNombre.Text;
            LigaNegocio negocio = new LigaNegocio();
            bool isEdit = int.TryParse(hfLigaId.Value, out int ligaId) && ligaId > 0;
            Jugador jugadorSesion = (Jugador)Session["Jugador"];

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
                txtLigaNombre.Text = string.Empty;

                if (!isEdit)
                {
                    bool jugadorAsociado = negocio.AsociarJugadorALiga(ligaId, jugadorSesion.Id);

                    if (jugadorAsociado)
                    {
                        hiddenMessage.Value = "Liga creada correctamente.";
                        hiddenMessageType.Value = "success";
                        CargarLigas();
                    }
                }
                else
                {
                    hiddenMessage.Value = $"Liga editada correctamente.";
                    hiddenMessageType.Value = "success";
                    CargarLigas();
                }
                ListaLigas = negocio.listarLigas(jugadorSesion.Id);
            }
            else
            {
                hiddenMessage.Value = $"Error en la {(isEdit ? "edición" : "creación")} de la liga!";
                hiddenMessageType.Value = "error";
            }
        }

        protected void deleteLiga_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(hiddenLigaId.Value))
            {
                int selectedLigaId = Convert.ToInt32(hiddenLigaId.Value);
                if (selectedLigaId != 0)
                {
                    int deleteLiga;
                    LigaNegocio negocio = new LigaNegocio();
                    deleteLiga = negocio.DeleteLiga(selectedLigaId);

                    if (deleteLiga == 0)
                    {
                        hiddenMessage.Value = "No se puede eliminar ligas que tengan torneos asociados.";
                        hiddenMessageType.Value = "error";
                    } 
                    else if(deleteLiga == 1)
                    {
                        CargarLigas();
                        hiddenMessage.Value = "Liga eliminada correctamente.";
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