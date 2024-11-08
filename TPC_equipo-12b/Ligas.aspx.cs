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

        private void CargarLigas()
        {
            LigaNegocio negocio = new LigaNegocio();
            ListaLigas = negocio.listarLigas();
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

        protected void btnSaveLiga_Click(object sender, EventArgs e)
        {
            string ligaNombre = txtLigaNombre.Text;
            bool crearLiga;

            LigaNegocio negocio = new LigaNegocio();
            crearLiga = negocio.CrearLiga(ligaNombre);

            if(crearLiga)
            {
                txtLigaNombre.Text = string.Empty;
                hiddenMessage.Value = "Liga creada correctamente.";
                CargarLigas();
            } else
            {
                hiddenMessage.Value = "Error en la creación.";
            }
        }

        protected void updateLiga_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(hiddenLigaId.Value))
            {
                string ligaNombre = txtLigaNombreUpdate.Text;
                int selectedLigaId = Convert.ToInt32(hiddenLigaId.Value);
                if (selectedLigaId != 0)
                {
                    bool updateLiga;
                    LigaNegocio negocio = new LigaNegocio();
                    updateLiga = negocio.UpdateLiga(selectedLigaId, ligaNombre);

                    if (updateLiga)
                    {
                        txtLigaNombreUpdate.Text = string.Empty;
                        hiddenMessage.Value = "Liga editada correctamente.";
                        CargarLigas();
                    }
                    else
                    {
                        hiddenMessage.Value = "Error en la edición.";
                    }

                }
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