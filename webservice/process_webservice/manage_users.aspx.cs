using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace process_webservice
{
    public partial class manage_users : System.Web.UI.Page
    {
        public static string getSqlConnction() => ConfigurationManager.ConnectionStrings["EMP_monitoringConnectionString"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                   
                    if (Request.QueryString["deleteid"] != null)
                    {
                        AddUserClass.DeleteUser(Convert.ToInt32(Request.QueryString["deleteid"]));

                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Users and Related Info, Deleted Successfully!');window.location ='manage_users.aspx';", true);
                    }

                    
                    if (Request.QueryString["mode"] == null)
                    {
                        fillDataGrid();
                    }
                    
                }
              
            }
            catch (Exception ex)
            {
                //CommonClass.RedirectToAdminErrorPage(ex.Message);
            }
        }
        protected void fillDataGrid()
        {
            try
            {
                using (_serviceDataClassesDataContext context = new _serviceDataClassesDataContext(getSqlConnction()))
                {
                    var content = (from d in context.user_masters
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
        protected void dgCountries_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    HiddenField hfuser_masterID = (HiddenField)e.Item.FindControl("hfuser_masterID");
                    HiddenField hfname = (HiddenField)e.Item.FindControl("hfname");
                    HiddenField hfemail = (HiddenField)e.Item.FindControl("hfemail");
                    HiddenField hflimit = (HiddenField)e.Item.FindControl("hflimit");

                    HtmlAnchor aEdit = (HtmlAnchor)e.Item.FindControl("aEdit");

                    aEdit.HRef = "javascript:____add('" + hfuser_masterID.Value + "','" + hfname.Value + "','" + hfemail.Value + "','"+ hflimit.Value + "','');";

                    HtmlAnchor ancDelete = (HtmlAnchor)e.Item.FindControl("ancDelete");
                    ancDelete.HRef = "manage_users.aspx?deleteid=" + hfuser_masterID.Value + "";

                    

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

        protected void dgCountries_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            dgCountries.CurrentPageIndex = e.NewPageIndex;
            fillDataGrid();

        }

        protected void btnsubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (hfuser_masterID.Value == "0")
                {
                    if (AddUserClass.InsertUser( txtname.Text,txtemail.Text,Convert.ToInt32(txtlimit.Text)) == true)
                    {

                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Record Insert Successfully');window.location ='manage_users.aspx';", true);
                    }
                }
                else
                {
                    if (AddUserClass.UpdateUser(Convert.ToInt32(hfuser_masterID.Value), txtname.Text, txtemail.Text, Convert.ToInt32(txtlimit.Text)) == true)
                    {

                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Record Update Successfully');window.location ='manage_users.aspx';", true);
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