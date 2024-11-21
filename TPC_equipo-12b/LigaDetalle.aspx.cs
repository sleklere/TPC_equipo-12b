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
                    //if (Request.QueryString["id"] != null)
                    //{
                    //    ListaJugadores = negocio.getJugadoresByLigaId(ligaId);
                    //    txtCodigoLiga.Text = LigaData.Codigo;
                    //}
                    CargarJugadorPorLigaParaListado();
                    CargarPartidos();
                    CargarJugadoresPorLigaParaSelects(LigaData.Id);
                    CargarTiposPartido();
                }
            }
            
        }

        private void CargarJugadorPorLigaParaListado()
        {
            string ligaId = Request.QueryString["id"].ToString();

            if (Request.QueryString["id"] != null)
            {
                ListaJugadores = new List<Jugador>();
                LigaNegocio negocio = new LigaNegocio();
                ListaJugadores = negocio.getJugadoresByLigaId(ligaId);
                txtCodigoLiga.Text = LigaData.Codigo;
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

        private void CargarTiposPartido()
        {
            PartidoNegocio partidoNegocio = new PartidoNegocio();
            List<TipoPartido> tipoPartidos = partidoNegocio.ListarTiposPartidos();
            if (tipoPartidos == null)
            {
                hiddenMessage.Value = "Error al cargar los tipos de partido.";
            }

            ddlTipoPartido.DataSource = tipoPartidos;
            ddlTipoPartido.DataTextField = "TextoDelSelect";
            ddlTipoPartido.DataValueField = "Id";
            ddlTipoPartido.DataBind();
            ddlTipoPartido.Items.Insert(0, new ListItem("Seleccionar", ""));


            //ddlTipoPartidoEditar.DataSource = tipoPartidos;
            //ddlTipoPartidoEditar.DataTextField = "TextoDelSelect";
            //ddlTipoPartidoEditar.DataValueField = "Id";
            //ddlTipoPartidoEditar.DataBind();
        }

        private void CargarJugadoresPorLigaParaSelects(int ligaId)
        {
            JugadorNegocio jugadorNegocio = new JugadorNegocio();
            List<Jugador> jugadores = jugadorNegocio.ListarJugadoresByLiga(ligaId);

            ddlJugador1.DataSource = jugadores;
            ddlJugador1.DataTextField = "Nombre";
            ddlJugador1.DataValueField = "Id";
            ddlJugador1.DataBind();
            ddlJugador1.Items.Insert(0, new ListItem("Seleccionar", ""));

            ddlJugador2.DataSource = jugadores;
            ddlJugador2.DataTextField = "Nombre";
            ddlJugador2.DataValueField = "Id";
            ddlJugador2.DataBind();
            ddlJugador2.Items.Insert(0, new ListItem("Seleccionar", ""));
        }

        protected void btnGuardarPartido_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ddlJugador1.SelectedValue) ||
            string.IsNullOrEmpty(ddlJugador2.SelectedValue) ||
            string.IsNullOrEmpty(txtPuntosJugador1.Text) ||
            string.IsNullOrEmpty(txtPuntosJugador2.Text) ||
            string.IsNullOrEmpty(ddlTipoPartido.SelectedValue))
            {
                hiddenMessage.Value = "Completa los campos obligatorios.";
                hiddenMessageType.Value = "error";
                return;
            }

            PartidoNegocio partidoNegocio = new PartidoNegocio();

            int tipoPartido = int.Parse(ddlTipoPartido.SelectedValue);
            int jugador1Id = int.Parse(ddlJugador1.SelectedValue);
            int jugador2Id = int.Parse(ddlJugador2.SelectedValue);

            if(jugador1Id == jugador2Id)
            {
                hiddenMessage.Value = "Los jugadores deben ser distintos.";
                hiddenMessageType.Value = "error";
                return;
            }

            int puntosJugador1 = int.Parse(txtPuntosJugador1.Text);
            int puntosJugador2 = int.Parse(txtPuntosJugador2.Text);


            if (tipoPartido == 1)
            {
                if(puntosJugador1 < 11 && puntosJugador2 < 11)
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
                partidoNegocio.CrearPartido(LigaData.Id, jugadoresConPuntos, tipoPartido);
                //hiddenMessage.Value = $"Partido {(isEdit ? "editado" : "creado")} correctamente.";
                hiddenMessage.Value = $"Partido creado correctamente.";
            }

            LimpiarFormulario();
            CargarPartidos();
            CargarJugadorPorLigaParaListado();
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
                CargarPartidos();
                CargarJugadorPorLigaParaListado();
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
                        CargarJugadorPorLigaParaListado();
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