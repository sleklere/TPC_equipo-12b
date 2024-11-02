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
        public List<Liga> ListaLigas {  get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            LigaNegocio negocio = new LigaNegocio();
            ListaLigas = negocio.listarLigas();
        }
    }
}