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
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void btnLogin_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text;
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(password) ||
               string.IsNullOrEmpty(email))
            {
                hiddenMessage.Value = "Todos los campos son obligatorios.";
                return;
            }

            string emailFormato = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            if (!System.Text.RegularExpressions.Regex.IsMatch(email, emailFormato))
            {
                hiddenMessage.Value = "El email ingresado no tiene un formato válido.";
                return;
            }

            JugadorNegocio negocio = new JugadorNegocio();
            Jugador jugadorLoggeado = negocio.Login(email, password);

            if (jugadorLoggeado != null)
            {
                Session["Jugador"] = jugadorLoggeado;
                Response.Redirect("~/");
            }
            else
            {
                hiddenMessage.Value = "Email o contraseña incorrectos.";
            }
        }
    }
}