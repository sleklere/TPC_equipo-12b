using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TPC_equipo_12b
{
    public partial class Master : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected string IsActive(string page)
        {
            string currentPage = Request.Url.LocalPath;
            string isActive =  currentPage.EndsWith(page, StringComparison.OrdinalIgnoreCase) ? "active" : string.Empty;
            return isActive;
        }

    }
}