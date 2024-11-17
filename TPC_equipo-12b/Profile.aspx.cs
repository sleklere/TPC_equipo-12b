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
    public partial class Profile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Jugador"] == null)
            {
                Response.Redirect("~/AccesoDenegado.aspx");
            } else
            {
                Jugador jugador = (Jugador)Session["Jugador"];
                int idJugador = jugador.Id;

                PartidoNegocio negocio = new PartidoNegocio();

                List<PartidoDTO> list = negocio.Ultimos10PartidosByJugadorID(idJugador);

                PartidosRepeater.DataSource = list;
                PartidosRepeater.DataBind();
            }
        }

        public void btnEditarTraerDatos_Click(object sender, EventArgs e)
        {
            Jugador jugador = (Jugador)Session["Jugador"];

            txtNombre.Text = jugador.Nombre;
            txtApellido.Text = jugador.Apellido;
            txtUsername.Text = jugador.Username;
            txtEmail.Text = jugador.Email;

            ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenModal", "openUpdateModal();", true);
        }

        public void btnUpdatePerfil_Click(object sender, EventArgs e)
        {
            Jugador jugador = (Jugador)Session["Jugador"];

            if (
                string.IsNullOrEmpty(txtNombre.Text) ||
                string.IsNullOrEmpty(txtApellido.Text) ||
                string.IsNullOrEmpty(txtUsername.Text) ||
                string.IsNullOrEmpty(txtEmail.Text)
            )
            {
                hiddenMessage.Value = "Todos los campos son obligatorios.";
                hiddenMessageType.Value = "error";
                return;
            }

            if (
                txtNombre.Text == jugador.Nombre &&
                txtApellido.Text == jugador.Apellido && 
                txtUsername.Text == jugador.Username && 
                txtEmail.Text == jugador.Email
            )
            {
                hiddenMessage.Value = "Tenes que cambiar por lo menos un campo.";
                hiddenMessageType.Value = "error";
                return;
            }

            int id = jugador.Id;
            string nombre = txtNombre.Text;
            string apellido = txtApellido.Text;
            string username = txtUsername.Text; 
            string email = txtEmail.Text;

            string emailFormato = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            if (!System.Text.RegularExpressions.Regex.IsMatch(email, emailFormato))
            {
                hiddenMessage.Value = "El email ingresado no tiene un formato válido.";
                return;
            }

            JugadorNegocio negocio = new JugadorNegocio();
            Jugador updateJugador= negocio.UpdateJugador(id, nombre, apellido, username, email);

            if (updateJugador != null)
            {
                Session["Jugador"] = updateJugador;
                hiddenMessage.Value = "Perfil editado correctamente.";
                hiddenMessageType.Value = "success";
            }
            else
            {
                hiddenMessage.Value = "Error en la edición.";
                hiddenMessageType.Value = "error";
            }

        }

        public void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            Session.Abandon(); 
            Response.Redirect("~/"); 
        }

    }
}