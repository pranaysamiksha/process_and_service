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
    public partial class browserhistory : System.Web.UI.Page
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
                fillDataGrid();
            }
            catch (Exception ex)
            {
                // CommonClass.RedirectToAdminErrorPage(ex.Message);
            }
        }
        protected void dgCountries_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            dgCountries.CurrentPageIndex = e.NewPageIndex;
            fillDataGrid();

        }
        protected void fillDataGrid()
        {
            try
            {
                using (_serviceDataClassesDataContext context = new _serviceDataClassesDataContext(getSqlConnction()))
                {
                    var content = (from d in context.browser_histories
                                   orderby d.last_visit_time descending
                                   where d.user_masterID == Convert.ToInt32(ddladmin_login_id.SelectedValue)
                                    && Convert.ToDateTime(d.entrydate).Date == Convert.ToDateTime(txtFromDate.Text)
                                   select d);

                    if (content.Count() > 0)
                    {
                        dgCountries.DataSource = content;
                        dgCountries.DataBind();
                        // dvrecord.InnerHtml = "";
                        if (content.Count() > 10)
                        {
                            dgCountries.PagerStyle.Visible = true;
                        }
                        else
                        {
                            dgCountries.PagerStyle.Visible = false;
                        }
                    }
                    else
                    {
                        dgCountries.DataSource = null;
                        dgCountries.DataBind();
                        //  dvrecord.InnerHtml = "No Record Found";
                    }
                }
            }
            catch (Exception ex)
            {
                //  CommonClass.RedirectToAdminErrorPage(ex.Message);
            }
        }
    }
}