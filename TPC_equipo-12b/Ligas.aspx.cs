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
        public int selectedLigaId;
        protected void Page_Load(object sender, EventArgs e)
        {
            LigaNegocio negocio = new LigaNegocio();
            ListaLigas = negocio.listarLigas();

            if (ListaLigas != null && ListaLigas.Count > 0)
            {
                rptLigas.DataSource = ListaLigas;
                rptLigas.DataBind();
            }
        }

        protected void btnSaveLiga_Click(object sender, EventArgs e)
        {
            string ligaNombre = txtLigaNombre.Text;
            bool crearLiga;

            LigaNegocio negocio = new LigaNegocio();
            crearLiga = negocio.CrearLiga(ligaNombre);

            if(crearLiga)
            {
                Response.Write("<script>alert('Liga \"" + ligaNombre + "\" guardada exitosamente!');</script>");
                txtLigaNombre.Text = string.Empty;
                ListaLigas = negocio.listarLigas();
            } else
            {
                Response.Write("<script>alert('Error en la creación!');</script>");
            }
        }

        protected void btnModalUpdate_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            string ligaId = button.CommandArgument;
            int id = int.Parse(ligaId);
            selectedLigaId = id;
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