using process_pousse.pms_process;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace process_pousse
{
    internal class Program
    {
        static void Main(string[] args)
        {
            

            string processName = Process.GetCurrentProcess().ProcessName;
            int processId = Process.GetCurrentProcess().Id;
            Process[] oProcesses = Process.GetProcessesByName(processName);
            if (oProcesses.Length > 1)
            {

                foreach (var process in Process.GetProcessesByName(processName))
                {
                    if (process.Id != processId)
                    {
                        process.Kill();
                        string userName = System.Configuration.ConfigurationManager.AppSettings["user_id"];


                        pms_process.process obj = new pms_process.process();
                        int intervalMilliseconds = Convert.ToInt32(obj.getActiveCount(Convert.ToInt32(userName)));
                        Timer timer = new Timer(ExecuteFunction, null, 0, intervalMilliseconds);

                        Task t = Getprocess(args);
                        t.Wait();

                        //Console.WriteLine("Press any key to exit...");
                        new ManualResetEvent(false).WaitOne();
                        timer.Dispose();
                    }
                }

            }
            else
            {


                string userName = System.Configuration.ConfigurationManager.AppSettings["user_id"];
                pms_process.process obj = new pms_process.process();
                int intervalMilliseconds = Convert.ToInt32(obj.getActiveCount(Convert.ToInt32(userName)));
                Timer timer = new Timer(ExecuteFunction, null, 0, intervalMilliseconds);

                Task t = Getprocess(args);
                t.Wait();

                //Console.WriteLine("Press any key to exit...");
                new ManualResetEvent(false).WaitOne();
                timer.Dispose();
            }




        }
        static void ExecuteFunction(object state)
        {
            string userName = System.Configuration.ConfigurationManager.AppSettings["user_id"];
            takeScreeshot(Convert.ToInt32(userName));
            //takeDekstop(Convert.ToInt32(userName));
            GetChromehistory(Convert.ToInt32(userName));
            Getedgehistory(Convert.ToInt32(userName));
            

        }
        public static  void takeScreeshot(int userid)
        {
            try
            {
                WriteLog("start image process");
                pms_process.process obj = new pms_process.process();
                var image = ScreenCapture.CaptureActiveWindow();
                System.IO.MemoryStream ms = new MemoryStream();
                image.Save(ms, ImageFormat.Jpeg);
                byte[] byteImage = ms.ToArray();
                var SigBase64 = Convert.ToBase64String(byteImage); // Get Base64
                string result =obj.convertImage(userid, SigBase64);
                WriteLog(""+ result + "");

            }
            catch (Exception ex)
            {
               string m= ex.Message;
                WriteLog("takeScreeshot ex: "+ex.Message+"");
            }
            

        }
        public static void takeDekstop(int userid)
        {
            try
            {
                WriteLog("start takeDekstop process");
                pms_process.process obj = new pms_process.process();
                var image = ScreenCapture.CaptureDesktop();
                System.IO.MemoryStream ms = new MemoryStream();
                image.Save(ms, ImageFormat.Jpeg);
                byte[] byteImage = ms.ToArray();
                var SigBase64 = Convert.ToBase64String(byteImage); // Get Base64
                string result = obj.convertImage(userid, SigBase64);
                WriteLog("" + result + "");

            }
            catch (Exception ex)
            {
                string m = ex.Message;
                WriteLog("takeDekstop ex: " + ex.Message + "");
            }


        }

        public static void GetChromehistory(int userid)
        {
            try
            {
                WriteLog("start GetChromehistory");
                //// string path = @"\Microsoft\Edge\User Data\Default\History";
                string name = Environment.UserName;
                string source = @"C:\\Users\\"+ name + "\\AppData\\Local\\Google\\Chrome\\User Data\\Default\\History";
                string target = @""+System.Configuration.ConfigurationManager.AppSettings["temphistory"];

                if (File.Exists(target))
                {
                    File.Delete(target);
                }

                File.Copy(source, target);

                string cs = @"Data Source=" + target;


                using (SQLiteConnection c = new SQLiteConnection(cs))
                {
                    c.Open();
                    DataSet dataSet = new DataSet();
                    SQLiteDataAdapter adapter = new SQLiteDataAdapter("select * from urls order by last_visit_time desc", cs);

                    adapter.Fill(dataSet);
                    var allHistoryItems = new List<ChromeHistoryItem>();
                    if (dataSet != null && dataSet.Tables.Count > 0 & dataSet.Tables[0] != null)
                    {
                        DataTable dt = dataSet.Tables[0];
                        foreach (DataRow historyRow in dt.Rows)
                        {
                            ChromeHistoryItem historyItem = new ChromeHistoryItem()
                            {
                                URL = Convert.ToString(historyRow["url"]),
                                Title = Convert.ToString(historyRow["title"])
                            };

                            // Chrome stores time elapsed since Jan 1, 1601 (UTC format) in microseconds
                            long utcMicroSeconds = Convert.ToInt64(historyRow["last_visit_time"]);

                            // Windows file time UTC is in nanoseconds, so multiplying by 10
                            DateTime gmtTime = DateTime.FromFileTimeUtc(10 * utcMicroSeconds);

                            // Converting to local time
                            DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(gmtTime, TimeZoneInfo.Local);
                            historyItem.VisitedTime = localTime;

                            allHistoryItems.Add(historyItem);

                            pms_process.process obj = new pms_process.process();
                            obj.Addbrowser_history(userid, "google crom", Convert.ToString(historyRow["url"]),
                                Convert.ToString(historyRow["title"]), localTime.ToString());

                        }
                    }
                    c.Close();
                    c.Dispose();
                }


            }
            catch (Exception ex)
            {

                WriteLog("GetChromehistory ex: " + ex.Message + "");

            }

        }

        public static void Getedgehistory(int userid)
        {
            try
            {
                WriteLog("start Get_edge_history");
                //// string path = @"\Microsoft\Edge\User Data\Default\History";
                string name = Environment.UserName;
                string source = @"C:\\Users\\" + name + "\\AppData\\Local\\Microsoft\\Edge\\User Data\\Default\\History";
                string target = @"" + System.Configuration.ConfigurationManager.AppSettings["temphistory1"];

                if (File.Exists(target))
                {
                    File.Delete(target);
                }

                File.Copy(source, target);

                string cs = @"Data Source=" + target;


                using (SQLiteConnection c = new SQLiteConnection(cs))
                {
                    c.Open();
                    DataSet dataSet = new DataSet();
                    SQLiteDataAdapter adapter = new SQLiteDataAdapter("select * from urls order by last_visit_time desc", cs);

                    adapter.Fill(dataSet);
                    var allHistoryItems = new List<ChromeHistoryItem>();
                    if (dataSet != null && dataSet.Tables.Count > 0 & dataSet.Tables[0] != null)
                    {
                        DataTable dt = dataSet.Tables[0];
                        foreach (DataRow historyRow in dt.Rows)
                        {
                            ChromeHistoryItem historyItem = new ChromeHistoryItem()
                            {
                                URL = Convert.ToString(historyRow["url"]),
                                Title = Convert.ToString(historyRow["title"])
                            };

                            // Chrome stores time elapsed since Jan 1, 1601 (UTC format) in microseconds
                            long utcMicroSeconds = Convert.ToInt64(historyRow["last_visit_time"]);

                            // Windows file time UTC is in nanoseconds, so multiplying by 10
                            DateTime gmtTime = DateTime.FromFileTimeUtc(10 * utcMicroSeconds);

                            // Converting to local time
                            DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(gmtTime, TimeZoneInfo.Local);
                            historyItem.VisitedTime = localTime;

                            allHistoryItems.Add(historyItem);

                            pms_process.process obj = new pms_process.process();
                            obj.Addbrowser_history(userid, "edge", Convert.ToString(historyRow["url"]),
                                Convert.ToString(historyRow["title"]), localTime.ToString());

                        }
                    }
                    c.Close();
                    c.Dispose();
                }


            }
            catch (Exception ex)
            {

                WriteLog("Getedgehistory ex: " + ex.Message + "");

            }

        }
        public static void WriteLog(string strLog)
        {
            StreamWriter log;
            FileStream fileStream = null;
            DirectoryInfo logDirInfo = null;
            FileInfo logFileInfo;

            string logFilePath = System.Configuration.ConfigurationManager.AppSettings["localfolder"];
            logFilePath = logFilePath + "Log-" + System.DateTime.Today.ToString("MM-dd-yyyy") + "." + "txt";
            logFileInfo = new FileInfo(logFilePath);
            logDirInfo = new DirectoryInfo(logFileInfo.DirectoryName);
            if (!logDirInfo.Exists) logDirInfo.Create();
            if (!logFileInfo.Exists)
            {
                fileStream = logFileInfo.Create();
            }
            else
            {
                fileStream = new FileStream(logFilePath, FileMode.Append);
            }
            log = new StreamWriter(fileStream);
            log.WriteLine(strLog);
            log.Close();
        }

        static async Task Getprocess(string[] args)
        {
            string userName = System.Configuration.ConfigurationManager.AppSettings["user_id"];
            ActiveApplicationPropertyThread activeWorker = new ActiveApplicationPropertyThread();
            Thread activeThread = new Thread(activeWorker.StartThread);
            activeThread.Start();
            Dictionary<string, Double> temp;
            while (true)
            {
                //Console.WriteLine(activeWorker.AppInfo);
                //if (activeWorker.AppInfo == "chrome.exe")
                //{
                    pms_process.process obj = new pms_process.process();
                    string abc = obj.Addprocess_master(Convert.ToInt32(userName), "", activeWorker.AppInfo,
                         "", "", "",
                         "", "");
                //}

                Thread.Sleep(1000);
            }

            
        }

        //public static void Getprocess(int userid)
        //{
        //    Process[] processlist = Process.GetProcesses();

        //    foreach (Process theprocess in processlist)
        //    {
        //        try
        //        {
        //            var process_name = theprocess.ProcessName;
        //            if (process_name == "chrome")
        //            {
        //                var procStartTime = System.Diagnostics.Process.GetProcessById(theprocess.Id).StartTime;
        //                var processid = theprocess.Id;
        //                var appname = theprocess.MainWindowTitle;
        //                var TotalProcessorTime = theprocess.TotalProcessorTime;
        //                var UserProcessorTime = theprocess.UserProcessorTime;
        //                var StartInfo = theprocess.StartInfo;

        //                pms_process.process obj = new pms_process.process();
        //               string abc= obj.Addprocess_master(userid, processid.ToString(), process_name.ToString(),
        //                    procStartTime.ToString(), appname.ToString(), TotalProcessorTime.ToString(),
        //                    UserProcessorTime.ToString(), StartInfo.ToString());

        //                //WriteLog("processid: " + processid + "");
        //                //WriteLog("ProcessName: " + process_name + "");
        //                //WriteLog("procStartTime: " + procStartTime + "");
        //                //WriteLog("appname: " + appname + "");
        //                //WriteLog("TotalProcessorTime: " + TotalProcessorTime + "");
        //                //WriteLog("UserProcessorTime: " + UserProcessorTime + "");
        //                //WriteLog("StartInfo: " + StartInfo + "");


        //            }

        //        }
        //        catch (Exception e)
        //        {
        //            // WriteLog("ProcessName: " + theprocess.ProcessName + " Access denied");
        //        }

        //    }

        //}

    }

  
    public class ChromeHistoryItem

    {

        public string URL { get; set; }

        public string Title { get; set; }

        public DateTime VisitedTime { get; set; }

    }
   
    // GetChromehistory();

    //Process[] processlist = Process.GetProcesses();

    //foreach (Process theprocess in processlist)
    //{
    //    try
    //    {
    //        var procStartTime = System.Diagnostics.Process.GetProcessById(theprocess.Id).StartTime;
    //        var process_name = theprocess.ProcessName;
    //        var processid = theprocess.Id;
    //        var appname = theprocess.MainWindowTitle;
    //        var TotalProcessorTime = theprocess.TotalProcessorTime;
    //        var UserProcessorTime = theprocess.UserProcessorTime;
    //        var StartInfo = theprocess.StartInfo;

    //        //Console.WriteLine("Process: {0} ID: {1}", theprocess.ProcessName, theprocess.Id);




    //        if (process_name == "chrome")
    //        {
    //            WriteLog("processid: " + processid + "");
    //            WriteLog("ProcessName: " + process_name + "");
    //            WriteLog("procStartTime: " + procStartTime + "");
    //            WriteLog("appname: " + appname + "");
    //            WriteLog("TotalProcessorTime: " + TotalProcessorTime + "");
    //            WriteLog("UserProcessorTime: " + UserProcessorTime + "");
    //            WriteLog("StartInfo: " + StartInfo + "");

    //            WriteLog("/n /n /n");
    //        }

    //    }
    //    catch (Exception e)
    //    {
    //       // WriteLog("ProcessName: " + theprocess.ProcessName + " Access denied");
    //    }

    //}
}
