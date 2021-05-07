using System;
using System.IO;

namespace Log
{
    public class logWriter
    {
        public static void WriteLog(string strLog)
        {
            //added the log for tracking
            string sFilePath = "c:\\" + DateTime.Now.ToString("yyyyMM");
            string sFileName = "mscgrizhi" + DateTime.Now.ToString("dd") + ".log";
            sFileName = sFilePath + "\\" + sFileName; //The absolute path to the file
            if (!Directory.Exists(sFilePath))//Verify that the path exists
            {
                Directory.CreateDirectory(sFilePath);
                //Creating file if it is not exists
            }
            FileStream fs;
            StreamWriter sw;
            if (File.Exists(sFileName))
            //Verify that the file exists, append if there is one, and create if there is none
            {
                fs = new FileStream(sFileName, FileMode.Append, FileAccess.Write);
            }
            else
            {
                fs = new FileStream(sFileName, FileMode.Create, FileAccess.Write);
            }
            sw = new StreamWriter(fs);
            DateTime date = DateTime.Now;
            string time = date.ToString("yyyy-MM-dd HH:mm:ss:fff");
            sw.WriteLine(time + "---" + strLog);
            sw.Close();
            fs.Close();
            //string sqlStr = "insert into PaymentLog (RecordTime,Information) values('" + time + "','" + strLog + "')";
            //MyTool.sqlTool.InsertData(sqlStr);

        }
        public static void WriteDragonphoenix(string strLog)
        {
            string sFilePath = @"C:\Users\nznor\source\repos\test\CjcProject_Remittance\AccountOpening\Dragonphoenix.txt";
            string sFileName = "Dragonphoenix" + DateTime.Now.ToString("dd") + ".log";

            FileStream fs;
            StreamWriter sw;
            fs = new FileStream(sFilePath, FileMode.Append, FileAccess.Write);

            sw = new StreamWriter(fs);
            sw.WriteLine(strLog);
            sw.Close();
            fs.Close();

        }

    }
}