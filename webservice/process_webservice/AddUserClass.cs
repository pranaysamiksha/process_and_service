using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace process_webservice
{
    public class AddUserClass
    {
        public static string getSqlConnction() => ConfigurationManager.ConnectionStrings["EMP_monitoringConnectionString"].ConnectionString;
        static Boolean blnValue = false;
        public static Boolean InsertUser(string name, string email,int limit)
        {
            try
            {
                using (_serviceDataClassesDataContext context = new _serviceDataClassesDataContext(getSqlConnction()))
                {
                    user_master Country = new user_master
                    {
                        name = name,
                        email = email,
                        limit = limit,
                    };
                    context.user_masters.InsertOnSubmit(Country);
                    context.SubmitChanges();
                }
                blnValue = true;
            }
            catch (SqlException exSQL)
            {
                //CommonClass.RedirectToAdminErrorPage(exSQL.Message);
            }
            catch (Exception ex)
            {
                //CommonClass.RedirectToAdminErrorPage(ex.Message);
            }
            return blnValue;
        }
        public static Boolean UpdateUser(int user_masterID, string name, string email, int limit)
        {
            try
            {
                using (_serviceDataClassesDataContext context = new _serviceDataClassesDataContext(getSqlConnction()))
                {
                    var country = from Country in context.user_masters
                                  where Country.user_masterID == user_masterID
                                  select Country;

                    if (country.Count() > 0)
                    {
                        foreach (var v in country)
                        {
                            v.name = name;
                            v.email = email;
                            v.limit = limit;
                        }
                        context.SubmitChanges();
                    }
                }
                blnValue = true;
            }

            catch (SqlException exSQL)
            {
               // CommonClass.RedirectToAdminErrorPage(exSQL.Message);
            }
            catch (Exception ex)
            {
               // CommonClass.RedirectToAdminErrorPage(ex.Message);
            }
            return blnValue;
        }
        public static Boolean DeleteUser(int user_masterID)
        {
            try
            {
                using (_serviceDataClassesDataContext context = new _serviceDataClassesDataContext(getSqlConnction()))
                {
                    var country = from Country in context.user_masters
                                  where Country.user_masterID == user_masterID
                                  select Country;

                    if (country.Count() > 0)
                    {
                        foreach (var v in country)
                        {
                            context.user_masters.DeleteOnSubmit(v);
                        }
                        context.SubmitChanges();
                    }
                }
                blnValue = true;
            }
            catch (SqlException exSQL)
            {
               // CommonClass.RedirectToAdminErrorPage(exSQL.Message);
            }
            catch (Exception ex)
            {
              //  CommonClass.RedirectToAdminErrorPage(ex.Message);
            }
            return blnValue;
        }
    }
}