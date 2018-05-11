using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace weixin_api
{
    public partial class Default : System.Web.UI.Page
    {
        WeChatMenu menu = new WeChatMenu();
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnCreateMenu_Click(object sender, EventArgs e)
        {
            string result = menu.createMenu(this.tbMenuContent.Text);
            tbResult.Text = result;
        }

        protected void btnDeleteMenu_Click(object sender, EventArgs e)
        {
            string result = menu.deleteMenu();
            tbResult.Text = result;
        }
    }
}