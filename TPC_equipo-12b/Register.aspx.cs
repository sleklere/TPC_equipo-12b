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
    public partial class Register : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnCrear_Click(object sender, EventArgs e)
        {
            string nombre = txtNombre.Text;
            string apellido = txtApellido.Text;
            string username = txtUsername.Text;
            string password = txtPassword.Text;
            string email = txtEmail.Text;

            if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(apellido) ||
                string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) ||
                string.IsNullOrEmpty(email))
            {
                hiddenMessage.Value = "Todos los campos son obligatorios.";
                return;
            }

            if (password.Length < 8)
            {
                hiddenMessage.Value = "La contraseña debe tener al menos 8 caracteres.";
                return;
            }

            try
            {
                Jugador nuevoJugador = new Jugador
                {
                    Nombre = nombre,
                    Apellido = apellido,
                    Username = username,
                    Password = password,
                    Email = email,
                };
                
                JugadorNegocio negocio = new JugadorNegocio();

                bool emailExistente = negocio.emailExistente(email);

                if (emailExistente)
                {
                    lblMessage.ForeColor = System.Drawing.Color.Green;
                    hiddenMessage.Value = "Este email ya esta en uso.";
                    return;
                }

                string emailFormato = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                if (!System.Text.RegularExpressions.Regex.IsMatch(email, emailFormato))
                {
                    hiddenMessage.Value = "El email ingresado no tiene un formato válido.";
                    return;
                }

                bool resultado = negocio.CrearJugador(nuevoJugador);

                if (resultado)
                {
                    //hiddenMessage.Value = "Usuario creado correctamente.";
                    Response.Redirect("Login.aspx");
                }
                else
                {
                    hiddenMessage.Value = "Error al crear usuario.";
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error: " + ex.Message;
            }
        }
    }
}