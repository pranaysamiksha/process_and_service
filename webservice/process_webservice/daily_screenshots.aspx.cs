using AjaxControlToolkit.HTMLEditor.ToolbarButton;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace process_webservice
{
    public partial class daily_screenshots : System.Web.UI.Page
    {
        public static string getSqlConnction() => ConfigurationManager.ConnectionStrings["EMP_monitoringConnectionString"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    filluser();
                }
            }
            catch (Exception ex)
            {
                // CommonClass.RedirectToAdminErrorPage(ex.Message);
            }
        }
        protected void filluser()
        {
            using (_serviceDataClassesDataContext context = new _serviceDataClassesDataContext(getSqlConnction()))
            {
                var content = (from d in context.user_masters
                               select d);
                if (content.Count() > 0)
                {
                    ddladmin_login_id.DataSource = content;
                    ddladmin_login_id.DataTextField = "name";
                    ddladmin_login_id.DataValueField = "user_masterID";
                    ddladmin_login_id.DataBind();
                    ddladmin_login_id.Items.Insert(0, new ListItem("Select Users", "0"));
                }
            }
        }

        protected void btnsubmit_Click(object sender, EventArgs e)
        {
            try
            {
                using (_serviceDataClassesDataContext context = new _serviceDataClassesDataContext(getSqlConnction()))
                {
                    var content = (from d in context.screen_captures
                                   where d.user_masterID == Convert.ToInt32(ddladmin_login_id.SelectedValue)
                                   && Convert.ToDateTime(d.entrydate).Date == Convert.ToDateTime(txtFromDate.Text) 
                                   select d);
                    if (content.Count() > 0)
                    {
                        StringBuilder str = new StringBuilder();
                        foreach(var v in content)
                        {
                            str.Append("<div class='col-md-3'>");
                            str.Append("<a class='lightbox' href='"+v.image_patch.Replace("~/", "")+"' >");
                            str.Append("<img src='"+v.image_patch.Replace("~/", "") + "' width='100%'>");
                            str.Append("</a>");
                            str.Append("<div class='btn - group' style='width: 100 %;'>"+v.entrydate+"</div>");
                            str.Append("</div>");

                        }
                        dvinner.InnerHtml = str.ToString();
                    }
                    else
                    {
                        dvinner.InnerHtml = "";
                    }
                                
                }
            }
            catch (Exception ex)
            {
                // CommonClass.RedirectToAdminErrorPage(ex.Message);
            }
        }
    }
}