using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace process_webservice
{
    public partial class app_usage : System.Web.UI.Page
    {
        public decimal TotalWorkinghours=0;
        public static string getSqlConnction() => ConfigurationManager.ConnectionStrings["EMP_monitoringConnectionString"].ConnectionString;
        public string answer = "";
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
                    var content = (from d in context.process_masters
                                   where d.user_masterID == Convert.ToInt32(ddladmin_login_id.SelectedValue)
                                    && Convert.ToDateTime(d.entrydate).Date == Convert.ToDateTime(txtFromDate.Text)
                                   select new
                                   {
                                       d.processName,
                                       countr =  Convert.ToDecimal(d.countr)

                                   });

                    if (content.Count() > 0)
                    {
                        TotalWorkinghours = content.Select(t => t.countr).Sum();

                        TimeSpan t1 = TimeSpan.FromSeconds(Convert.ToDouble(TotalWorkinghours));
                         answer = string.Format("{0:D2}h:{1:D2}m:{2:D2}s:{3:D3}ms",
                t1.Hours,
                t1.Minutes,
                t1.Seconds,
                t1.Milliseconds);

                        dgCountries.DataSource = content;
                        dgCountries.DataBind();
                        dgCountries.ShowFooter = true;
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
        protected void dgCountries_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    HiddenField hfcountr = (HiddenField)e.Item.FindControl("hfcountr");
                    Label lbltime = (Label)e.Item.FindControl("lbltime");

                    TimeSpan t1 = TimeSpan.FromSeconds(Convert.ToDouble(hfcountr.Value));
                    string answer1 = string.Format("{0:D2}h:{1:D2}m:{2:D2}s:{3:D3}ms",
           t1.Hours,
           t1.Minutes,
           t1.Seconds,
           t1.Milliseconds);
                    lbltime.Text = answer1.ToString();


                }
            }
            catch (SqlException exSQL)
            {
                //  CommonClass.RedirectToAdminErrorPage(exSQL.Message);
            }
            catch (Exception ex)
            {
                //   CommonClass.RedirectToAdminErrorPage(ex.Message);
            }
        }

    }

}