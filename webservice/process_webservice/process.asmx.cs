using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Linq.Mapping;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;

namespace process_webservice
{
    /// <summary>
    /// Summary description for process
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]

    public class process : System.Web.Services.WebService
    {
        public static string getSqlConnction() => ConfigurationManager.ConnectionStrings["EMP_monitoringConnectionString"].ConnectionString;

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string getActiveCount(int user_masterID)
        {
            string returnValue = "";

            try
            {

                using (_serviceDataClassesDataContext catcontext = new _serviceDataClassesDataContext(getSqlConnction()))
                {
                    var cust = from c in catcontext.user_masters
                               where  c.user_masterID == user_masterID
                               select c;
                    if (cust.Count() > 0)
                    {
                        //return new JavaScriptSerializer().Serialize(cust);
                        returnValue = cust.FirstOrDefault().limit.ToString();
                    }
                    else
                    {
                        returnValue = "Invalid UserName or Password.";
                    }
                }
            }
            catch (SqlException exsql)
            {
                returnValue = exsql.Message;
            }
            catch (Exception ex)
            {
                returnValue = ex.Message;
            }
            return returnValue;

        }

        [WebMethod]
        public string Addprocess_master(int user_masterID, string processid,string processName,
            string procStartTime,string appname,string totalProcessorTime,string userProcessorTime,
            string StartInfo)
        {
            string returnValue = "";
            try
            {

                
                using (_serviceDataClassesDataContext catcontext = new _serviceDataClassesDataContext(getSqlConnction()))
                {
                    decimal abc = Checkprocesexist(user_masterID, processName);
                    if (abc == 0)
                    {
                        process_master cm = new process_master
                        {
                            user_masterID = user_masterID,
                            processid = processid,
                            processName = processName,
                            procStartTime = procStartTime,
                            appname = appname,
                            totalProcessorTime = totalProcessorTime,
                            userProcessorTime = userProcessorTime,
                            countr = Convert.ToInt32(StartInfo),
                            entrydate = System.DateTime.Now,
                        };
                        catcontext.process_masters.InsertOnSubmit(cm);
                        catcontext.SubmitChanges();
                    }
                    else
                    {
                        abc = abc + Convert.ToInt32(StartInfo);
                        var content = (from d in catcontext.process_masters
                                       where d.user_masterID == user_masterID
                                       && d.processName == processName &&
                                       Convert.ToDateTime(d.entrydate).Date == 
                                       Convert.ToDateTime(System.DateTime.Now).Date
                                       select d);
                        if (content.Count() > 0)
                        {
                            foreach(var v in content)
                            {
                                v.countr = abc;
                                catcontext.SubmitChanges();
                            }
                        }


                    }
                }
                returnValue = "process added successfully.";
            }
            catch (SqlException exsql)
            {
                returnValue = exsql.Message;
            }
            catch (Exception ex)
            {
                returnValue = ex.Message;
            }
            return returnValue;
        }

        [WebMethod]
        public string Addbrowser_history(int user_masterID, string browser_name,DataTable dt)
        {
            string returnValue = "";
            try
            {
                Task t = async(user_masterID, browser_name, dt);
                t.Wait();
               // Thread thread = new Thread(new ThreadStart(async(user_masterID, browser_name, dt))) ;
                //thread.Start();
                returnValue = "browser added successfully.";
            }
            catch (SqlException exsql)
            {
                returnValue = exsql.Message;
            }
            catch (Exception ex)
            {
                locallog(user_masterID, "local browser history1", ex.Message.ToString(), ex.StackTrace.ToString());
                returnValue = ex.Message;
            }
            return returnValue;
        }

        public async Task async(int user_masterID, string browser_name, DataTable dt)
        {
            try
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow historyRow in dt.Rows)
                    {
                        //long utcMicroSeconds = Convert.ToInt64(historyRow["last_visit_time"]);
                        //DateTime gmtTime = DateTime.FromFileTimeUtc(10 * utcMicroSeconds);
                        //DateTime localTime1 = TimeZoneInfo.ConvertTimeFromUtc(gmtTime,TimeZoneInfo.Utc);
                        //var localTime= localTime1.Date.ToString("yyyy-MM-dd HH:MM:ss");
                        var localTime = historyRow["time"];

                        if (CheckHistry(user_masterID, Convert.ToString(historyRow["title"]), localTime.ToString()) == false)
                        {
                            using (_serviceDataClassesDataContext catcontext = new _serviceDataClassesDataContext(getSqlConnction()))
                            {
                                browser_history cm = new browser_history
                                {
                                    user_masterID = user_masterID,
                                    browser_name = browser_name,
                                    url = Convert.ToString(historyRow["url"]),
                                    title = Convert.ToString(historyRow["title"]),
                                    last_visit_time = localTime.ToString(),
                                    entrydate = System.DateTime.Now,

                                };
                                catcontext.browser_histories.InsertOnSubmit(cm);
                                catcontext.SubmitChanges();
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                locallog(user_masterID, "local browser history", ex.Message.ToString(), ex.StackTrace.ToString());
            }
        }

        [WebMethod]
        public string convertImage(int user_masterID, string imgtext)
        {
            string returnValue = "";
            try
            {
                byte[] bytesimage = Convert.FromBase64String(imgtext.Trim());
                string path = ByteArrayToImageFilebyMemoryStream(bytesimage);
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime currentDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);


                using (_serviceDataClassesDataContext catcontext = new _serviceDataClassesDataContext(getSqlConnction()))
                {
                    screen_capture cm = new screen_capture
                    {
                        user_masterID = user_masterID,
                        image_patch= path,
                        entrydate= currentDateTime,

                    };
                    catcontext.screen_captures.InsertOnSubmit(cm);
                    catcontext.SubmitChanges();
                }
                returnValue = "convertImage added successfully.";
            }
            catch (SqlException exsql)
            {
                returnValue = exsql.Message;
            }
            catch (Exception ex)
            {
                returnValue = ex.Message;
            }
            return returnValue;
        }
        public string ByteArrayToImageFilebyMemoryStream(byte[] imageByte)
        {

            MemoryStream ms = new MemoryStream(imageByte);
            Image image = Image.FromStream(ms);
            string tempName = System.DateTime.Now.TimeOfDay.ToString().Replace(":", "").Replace(" ", "").Replace(".", ""); // for unique name
            string path = "~/Salesman/" + tempName + ".png";
            image.Save(Server.MapPath(path));
            return path;
        }
        public bool CheckHistry(int user_masterID, string  title,string last_visit_time)
        {
            bool id = false;
            using (_serviceDataClassesDataContext context = new _serviceDataClassesDataContext(getSqlConnction()))
            {
                var content = (from d in context.browser_histories
                               where d.user_masterID == user_masterID
                               && d.title == title && d.last_visit_time== last_visit_time
                               select d);
                if (content.Count() > 0)
                {
                    id = true;
                }
            }
            return id;
        }
        public decimal Checkprocesexist(int user_masterID, string processName)
        {
            int sec = 0;
            using (_serviceDataClassesDataContext context = new _serviceDataClassesDataContext(getSqlConnction()))
            {
                var content = (from d in context.process_masters
                               where d.user_masterID == user_masterID
                               && d.processName == processName && 
                               Convert.ToDateTime(d.entrydate).Date == Convert.ToDateTime(System.DateTime.Now).Date
                               select d);
                if (content.Count() > 0)
                {
                    sec =Convert.ToInt32(content.FirstOrDefault().countr);
                }
                else
                {
                    sec = 0;
                }
            }
            return sec;
        }
        [WebMethod]
        public string insertErrorlog(int user_masterID, string mothod, string errortrace,string stack)
        {
            string returnValue = "";
            try
            {

                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime currentDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);


                using (_serviceDataClassesDataContext catcontext = new _serviceDataClassesDataContext(getSqlConnction()))
                {
                    errorlog cm = new errorlog
                    {
                        user_masterID = user_masterID,
                        mothod = mothod,
                        entrydate = currentDateTime.ToString(),
                        errortrace= errortrace,
                        error_stack= stack

                    };
                    catcontext.errorlogs.InsertOnSubmit(cm);
                    catcontext.SubmitChanges();
                }
                returnValue = "eroor added successfully.";
            }
            catch (SqlException exsql)
            {
                returnValue = exsql.Message;
            }
            catch (Exception ex)
            {
                returnValue = ex.Message;
            }
            return returnValue;
        }

        public void locallog(int user_masterID, string mothod, string errortrace, string stack)
        {
            string returnValue = "";
            try
            {

                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime currentDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);


                using (_serviceDataClassesDataContext catcontext = new _serviceDataClassesDataContext(getSqlConnction()))
                {
                    errorlog cm = new errorlog
                    {
                        user_masterID = user_masterID,
                        mothod = mothod,
                        entrydate = currentDateTime.ToString(),
                        errortrace = errortrace,
                        error_stack = stack

                    };
                    catcontext.errorlogs.InsertOnSubmit(cm);
                    catcontext.SubmitChanges();
                }
                returnValue = "eroor added successfully.";
            }
            catch (SqlException exsql)
            {
                returnValue = exsql.Message;
            }
            catch (Exception ex)
            {
                returnValue = ex.Message;
            }
            
        }

    }
    
}
