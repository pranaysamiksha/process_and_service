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
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace process_pousse
{
    internal class Program
    {
        [DllImport("kernel32.dll", EntryPoint = "GetStdHandle", SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll", EntryPoint = "AllocConsole", SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int AllocConsole();

        private const int STD_OUTPUT_HANDLE = -11;
        private const int MY_CODE_PAGE = 437;
        private static bool showConsole = true; //Or false if you don't want to see the console

        public static string oldprocessname = "";
        public static int oldprocesscount = 0;
        static void Main(string[] args)
        {
            try
            {
                if (showConsole)
                {
                    AllocConsole();
                    IntPtr stdHandle = GetStdHandle(STD_OUTPUT_HANDLE);
                    Microsoft.Win32.SafeHandles.SafeFileHandle safeFileHandle = new Microsoft.Win32.SafeHandles.SafeFileHandle(stdHandle, true);
                    FileStream fileStream = new FileStream(safeFileHandle, FileAccess.Write);
                    System.Text.Encoding encoding = System.Text.Encoding.GetEncoding(MY_CODE_PAGE);
                    StreamWriter standardOutput = new StreamWriter(fileStream, encoding);
                    standardOutput.AutoFlush = true;
                    Console.SetOut(standardOutput);
                }

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
                            Timer timer1 = new Timer(ExecuteFunctionAfter60, null, 0, 600000);
                            Task t = Getprocess(args);
                            t.Wait();

                           // new ManualResetEvent(false).WaitOne();
                            timer.Dispose();
                            timer1.Dispose();

                        }
                    }

                }
                else
                {


                    string userName = System.Configuration.ConfigurationManager.AppSettings["user_id"];
                    pms_process.process obj = new pms_process.process();
                    int intervalMilliseconds = Convert.ToInt32(obj.getActiveCount(Convert.ToInt32(userName)));
                    Timer timer = new Timer(ExecuteFunction, null, 0, intervalMilliseconds);
                    Timer timer1 = new Timer(ExecuteFunctionAfter60, null, 0, 600000);

                    Task t = Getprocess(args);
                    t.Wait();

                   // new ManualResetEvent(false).WaitOne();
                    timer.Dispose();
                    timer1.Dispose();
                }


            }
            catch (Exception ex)
            {
                string userName = System.Configuration.ConfigurationManager.AppSettings["user_id"];
                pms_process.process obj = new pms_process.process();
                obj.insertErrorlog(Convert.ToInt32(userName), "Main Metthod", ex.Message,ex.StackTrace.ToString());
                string m = ex.Message;
                WriteLog("main method ex: " + ex.Message + "");
            }


        }
        static void ExecuteFunction(object state)
        {
            string userName = System.Configuration.ConfigurationManager.AppSettings["user_id"];
            takeScreeshot(Convert.ToInt32(userName));
        }
        static void ExecuteFunctionAfter60(object state)
        {
            string userName = System.Configuration.ConfigurationManager.AppSettings["user_id"];
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
                string userName = System.Configuration.ConfigurationManager.AppSettings["user_id"];
                pms_process.process obj = new pms_process.process();
                obj.insertErrorlog(Convert.ToInt32(userName), "takeScreeshot", ex.Message, ex.StackTrace.ToString());
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
                string userName = System.Configuration.ConfigurationManager.AppSettings["user_id"];
                pms_process.process obj = new pms_process.process();
                obj.insertErrorlog(Convert.ToInt32(userName), "takeDekstop", ex.Message, ex.StackTrace.ToString());

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
                    string source = @"C:\\Users\\" + name + "\\AppData\\Local\\Google\\Chrome\\User Data\\Default\\History";
                    string target = @"" + System.Configuration.ConfigurationManager.AppSettings["temphistory"];

                    if (File.Exists(target))
                    {
                        File.Delete(target);
                    }

                    File.Copy(source, target, true);

                    string cs = @"Data Source=" + target;


                    using (SQLiteConnection c = new SQLiteConnection(cs))
                    {
                        c.Open();
                        DataSet dataSet = new DataSet();
                        SQLiteDataAdapter adapter = new SQLiteDataAdapter("select *, datetime((last_visit_time/1000000)-11644473600, 'unixepoch', 'localtime') AS time from urls order by last_visit_time desc", cs);

                        adapter.Fill(dataSet);
                        var allHistoryItems = new List<ChromeHistoryItem>();
                        if (dataSet != null && dataSet.Tables.Count > 0 & dataSet.Tables[0] != null)
                        {
                            DataTable dt = dataSet.Tables[0];
                            pms_process.process obj = new pms_process.process();
                            obj.Addbrowser_history(userid, "google crom", dt);
                            
                        }

                        c.Close();
                        c.Dispose();
                    }
                

            }
            catch (Exception ex)
            {
                string userName = System.Configuration.ConfigurationManager.AppSettings["user_id"];
                pms_process.process obj = new pms_process.process();
                obj.insertErrorlog(Convert.ToInt32(userName), "GetChromehistory", ex.Message, ex.StackTrace.ToString());

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
                        SQLiteDataAdapter adapter = new SQLiteDataAdapter("select *, datetime((last_visit_time/1000000)-11644473600, 'unixepoch', 'localtime') AS time from urls order by last_visit_time desc", cs);

                        adapter.Fill(dataSet);
                        var allHistoryItems = new List<ChromeHistoryItem>();
                        if (dataSet != null && dataSet.Tables.Count > 0 & dataSet.Tables[0] != null)
                        {
                            DataTable dt = dataSet.Tables[0];
                            pms_process.process obj = new pms_process.process();
                            obj.Addbrowser_history(userid, "edge", dt);
                            
                        }
                        c.Close();
                        c.Dispose();
                    }
                


            }
            catch (Exception ex)
            {
                string userName = System.Configuration.ConfigurationManager.AppSettings["user_id"];
                pms_process.process obj = new pms_process.process();
                obj.insertErrorlog(Convert.ToInt32(userName), "Getedgehistory", ex.Message, ex.StackTrace.ToString());


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
            try
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
                    if (oldprocessname == activeWorker.AppInfo)
                    {
                        oldprocesscount = oldprocesscount + 1;
                    }
                    else
                    {
                        pms_process.process obj = new pms_process.process();
                        string abc = obj.Addprocess_master(Convert.ToInt32(userName), "", oldprocessname,
                             "", "", "",
                             "", oldprocesscount.ToString());
                        oldprocessname = activeWorker.AppInfo;
                        oldprocesscount = 0;
                    }

                    //}

                    Thread.Sleep(1000);
                }
            }
            catch (Exception ex)
            {
                string userName = System.Configuration.ConfigurationManager.AppSettings["user_id"];
                pms_process.process obj = new pms_process.process();
                obj.insertErrorlog(Convert.ToInt32(userName), "Getprocess", ex.Message, ex.StackTrace.ToString());
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
