using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace process_webservice
{
    public partial class index : System.Web.UI.Page
    {
        public static string getSqlConnction() => ConfigurationManager.ConnectionStrings["EMP_monitoringConnectionString"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            using (_serviceDataClassesDataContext context = new _serviceDataClassesDataContext(getSqlConnction()))
            {
                var content = (from d in context.admin_logins
                               where d.username == username.Value && d.password == inputChoosePassword.Value
                               select d);
                if (content.Count() > 0)
                {


                    foreach (var v in content)
                    {
                        Response.Cookies["User"]["UserID"] = v.admin_login_id.ToString();
                        Response.Cookies["AdminCookie"]["AdminName"] = v.username.ToString();
                        //Response.Write("hi:"+Response.Cookies["AdminCookie"]["AdminName"].ToString()+"id"+Response.Cookies["User"]["UserID"].ToString()+"DateTime="+System.DateTime.Now.AddHours(8).ToString());
                        HttpCookie aCookie = new HttpCookie("session");
                        aCookie.Value = "Merchant";
                        aCookie.Expires = DateTime.Now.AddDays(1);
                        Response.Cookies.Add(aCookie);
                    }
                    //Response.Redirect("Default.aspx",true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Login Successfully');window.location ='dashborad.aspx';", true);
                }
                else
                {
                    Response.Write("<script>alert('Invalid Login Credentials Supplied!');</script>");
                }
            }
        }
    }
}