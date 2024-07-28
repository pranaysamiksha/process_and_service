using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace process_webservice
{
    
    public partial class dashborad : System.Web.UI.Page
    {
        public string strusername = "";
        public string totalwork = "";
        public static string getSqlConnction() => ConfigurationManager.ConnectionStrings["EMP_monitoringConnectionString"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                fillDataGrid();
            }
        }
        protected void fillDataGrid()
        {
            try
            {
                using (_serviceDataClassesDataContext context = new _serviceDataClassesDataContext(getSqlConnction()))
                {
                    var content = (from d in context.user_masters
                                   select new
                                   {
                                       d.name,
                                       d.user_masterID
                                   });

                    if (content.Count() > 0)
                    {
                        foreach(var v in content)
                        {
                            strusername += v.name.Trim() + ",";
                            decimal hours =Convert.ToDecimal(totalWork(v.user_masterID))/60;
                            hours = hours / 60;
                            totalwork += hours.ToString() + ",";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //  CommonClass.RedirectToAdminErrorPage(ex.Message);
            }
        }
        public string totalWork(int user_masterID)
        {
            string work = "";
            using (_serviceDataClassesDataContext context = new _serviceDataClassesDataContext(getSqlConnction()))
            {
                var content = (from d in context.process_masters
                               where d.user_masterID == Convert.ToInt32(user_masterID)
                                && Convert.ToDateTime(d.entrydate).Date == Convert.ToDateTime(System.DateTime.Now).Date
                               select new
                               {
                                   d.processName,
                                   countr = Convert.ToInt32(d.countr)

                               });

                if (content.Count() > 0)
                {
                    work = content.Select(t => t.countr).Sum().ToString();

                }
                else
                {
                    work = "0";
                }
            }

            return work;

        }
    }
}