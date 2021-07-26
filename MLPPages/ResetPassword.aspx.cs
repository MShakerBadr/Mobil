using MLPPages.App_Code;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MLPPages
{
    public partial class ResetPassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ResetBtn_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["ID"] != null)
            {
                string token = Request.QueryString["ID"];
                string endPoint = ConfigurationManager.AppSettings["MobilAPIURL"] + @"/account/ConfirmForgetPassword/" + token + "/" + RePasswordTxt.Text;
                var client = new RestClient(endPoint);
                var json = client.MakeRequest();

                ResetBtn.Visible = false;
                SuccessMsg.Visible = true;
            }
        }
    }
}