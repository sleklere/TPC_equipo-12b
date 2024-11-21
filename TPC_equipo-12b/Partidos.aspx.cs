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
    public partial class Partidos : System.Web.UI.Page
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
                    //CargarLigas();
                    //CargarJugadores();
                    CargarPartidos();
                } else
                {
                    string targetControl = Request.Form["__EVENTTARGET"];
                    if (targetControl == ddlLiga.UniqueID)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "OpenModal", "$('#creatModal').modal('show');", true);
                    }
                }
            }
        }

        //private void CargarLigas()
        //{
        //    LigaNegocio ligaNegocio = new LigaNegocio();
        //    ddlLiga.DataSource = ligaNegocio.listarLigas();
        //    ddlLiga.DataTextField = "Nombre";
        //    ddlLiga.DataValueField = "Id";
        //    ddlLiga.DataBind();
        //}

        protected void ddlLiga_SelectedIndexChanged(object sender, EventArgs e)
        {
            string ligaSeleccionada = ddlLiga.SelectedValue;

            if (!string.IsNullOrEmpty(ligaSeleccionada))
            {
                int ligaId = int.Parse(ligaSeleccionada);
                CargarJugadoresPorLiga(ligaId);
            }
            else
            {
                LimpiarJugadores();
            }
        }

        private void CargarJugadoresPorLiga(int ligaId)
        {
            // Lógica para cargar jugadores según la liga seleccionada
            JugadorNegocio jugadorNegocio = new JugadorNegocio();
            List<Jugador> jugadores = jugadorNegocio.ListarJugadoresByLiga(ligaId);

            ddlJugador1.DataSource = jugadores;
            ddlJugador1.DataTextField = "Nombre";
            ddlJugador1.DataValueField = "Id";
            ddlJugador1.DataBind();

            ddlJugador2.DataSource = jugadores;
            ddlJugador2.DataTextField = "Nombre";
            ddlJugador2.DataValueField = "Id";
            ddlJugador2.DataBind();
        }

        private void LimpiarJugadores()
        {
            ddlJugador1.Items.Clear();
            ddlJugador2.Items.Clear();
        }

        //private void CargarJugadores()
        //{
        //    if (!string.IsNullOrEmpty(ddlLiga.SelectedValue))
        //    {
        //        int ligaId = int.Parse(ddlLiga.SelectedValue); // Obtiene el Id de la liga seleccionada

        //        JugadorNegocio jugadorNegocio = new JugadorNegocio();
        //        List<Jugador> jugadores = jugadorNegocio.ListarJugadoresByLiga(ligaId); // Filtra los jugadores por liga

        //        ddlJugador1.DataSource = jugadores;
        //        ddlJugador1.DataTextField = "Nombre";
        //        ddlJugador1.DataValueField = "Id";
        //        ddlJugador1.DataBind();

        //        ddlJugador2.DataSource = jugadores;
        //        ddlJugador2.DataTextField = "Nombre";
        //        ddlJugador2.DataValueField = "Id";
        //        ddlJugador2.DataBind();
        //    }
        //    else
        //    {
        //        // Manejo de error o mensaje para seleccionar una liga válida
        //        ddlJugador1.Items.Clear();
        //        ddlJugador2.Items.Clear();
        //    }
        //}

        private void CargarPartidos()
        {
            PartidoNegocio partidoNegocio = new PartidoNegocio();
            List<ListarPartidosDTO> partidos = partidoNegocio.listarPartidos();
            if (partidos == null)
            {
                hiddenMessage.Value = "Error al cargar los partidos.";
            }
            rptPartidos.DataSource = partidos;
            rptPartidos.DataBind();
        }

        protected void btnGuardarPartido_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ddlLiga.SelectedValue) ||
            string.IsNullOrEmpty(ddlJugador1.SelectedValue) ||
            string.IsNullOrEmpty(ddlJugador2.SelectedValue) ||
            string.IsNullOrEmpty(txtPuntosJugador1.Text) ||
            string.IsNullOrEmpty(txtPuntosJugador2.Text))
            {
                hiddenMessage.Value = "Completa los campos obligatorios.";
                hiddenMessageType.Value = "error";
                return;
            }

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
                //partidoNegocio.CrearPartido(ligaId, jugadoresConPuntos);
                //hiddenMessage.Value = $"Partido {(isEdit ? "editado" : "creado")} correctamente.";
                hiddenMessage.Value = $"Partido creado correctamente.";
            }

            LimpiarFormulario();
            CargarPartidos();
        }

        private void LimpiarFormulario()
        {
            hfPartidoId.Value = string.Empty;
            txtPuntosJugador1.Text = string.Empty;
            txtPuntosJugador2.Text = string.Empty;
            ddlJugador1.ClearSelection();
            ddlJugador2.ClearSelection();
            //lblFormularioTitulo.Text = "Crear Partido";
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            hfPartidoId.Value = string.Empty;
            //lblFormularioTitulo.Text = "Crear Partido";
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
                //lblFormularioTitulo.Text = "Editar Partido";
            }
            else if (e.CommandName == "Eliminar")
            {
                PartidoNegocio partidoNegocio = new PartidoNegocio();
                partidoNegocio.EliminarPartido(partidoId);
                CargarPartidos();
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
            int ganadorId= int.Parse(hiddenUpdateGanadorId.Value);

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