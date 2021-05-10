using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace PaymentBackend
{


    #region MD5
    public class ConvertMD5
    {
        /// <summary>
        /// Generating the sign for api
        /// </summary>
        /// <param name="input">string</param>
        /// <returns></returns>
        public static string GetMD5WithString(String input)
        {
            MD5 md5Hash = MD5.Create();
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder str = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                str.Append(data[i].ToString("x2"));
            }
            return str.ToString();
        }

        public static bool CheckMD5(string strValue, string sign)
        {
            if (strValue != null && strValue != "")
            {
                string result = GetMD5WithString(strValue);
                if (result == sign)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

        }

    }

    #endregion

    #region Post Request API (form stubmit)

    public class  HttpRequest
    {
        /// <summary>
        /// Create the request, constructed as form HTML (default)
        /// </summary>
        /// <param name="url">string</param>
        /// <param name="sParaTemp">Array</param>
        /// <param name="strMethod">Submission method. Two values are optional: post and gett</param>
        /// <param name="strButtonValue">Confirm that the button displays text</param>
        /// <returns>Submit the form HTML text</returns>
        public static string BuildRequest(string url, Dictionary<string, string> sParaTemp, string strMethod, string strButtonValue)
        {

            StringBuilder sbHtml = new StringBuilder();

            sbHtml.Append("<form id='PaySubmit' name='PaySubmit' action='" + url 
                + "' method='" + strMethod.ToLower().Trim() + "'>");

            foreach (KeyValuePair<string, string> temp in sParaTemp)
            {
                sbHtml.Append("<input type='hidden' name='" + temp.Key + "' value='" 
                    + temp.Value + "'/>");
            }

            //Please do not include the name attribute in the submit button control
            sbHtml.Append("<input type='submit' value='" + strButtonValue 
                + "' style='display:none;'></form>");

            sbHtml.Append("<script>document.forms['PaySubmit'].submit();</script>");

            return sbHtml.ToString();
        }

        /// <summary>
        /// /DP pay out request form
        /// </summary>
        /// <param name="url"></param>
        /// <param name="sParaTemp"></param>
        /// <param name="strMethod"></param>
        /// <param name="strButtonValue"></param>
        /// <returns></returns>

        public static string DPPOBuildRequest(string url, Dictionary<string, string> sParaTemp, string strMethod, string strButtonValue)
        {

            StringBuilder sbHtml = new StringBuilder();

            sbHtml.Append("<form id='DPpayout' name='DPpayout' action='" + url
                + "' method='" + strMethod.ToLower().Trim() + "'>");

            foreach (KeyValuePair<string, string> temp in sParaTemp)
            {
                if (!temp.Key.Equals("getamount"))
                {
                    sbHtml.Append("<input type='hidden' name='" + temp.Key + "' value='"
                    + temp.Value + "'/>");
                }
                else
                {
                    sbHtml.Append("<input type='hidden' type='number' name='" + temp.Key + "' value='" + temp.Value + "'/>");
                }

            }

            //Please do not include the name attribute in the submit button control
            sbHtml.Append("<input type='submit' value='" + strButtonValue
                + "' style='display:none;'></form>");

            sbHtml.Append("<script>document.forms['DPpayout'].submit();</script>");

            return sbHtml.ToString();
        }

        /// <summary>
        /// Post request API for getting the response message
        /// </summary>
        /// <param name="url">URL</param>
        /// <param name="data">JSON</param>
        /// <returns></returns>
        public static string HttpPost(string url, string data)
        {
            try
            {
                //create psot request
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/json;charset=UTF-8";
                byte[] payload = Encoding.UTF8.GetBytes(data);
                request.ContentLength = payload.Length;

                //send post request
                Stream writer = request.GetRequestStream();
                writer.Write(payload, 0, payload.Length);
                writer.Close();

                //get response
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream stream = response.GetResponseStream();
                StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                string value = reader.ReadToEnd();

                reader.Close();
                stream.Close();
                response.Close();

                return value;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        /// <summary>
        /// Post request API for getting the response message
        /// </summary>
        /// <param name="url">URL</param>
        /// <param name="data">JSON</param>
        /// <param name="header">AUTH</param>
        /// <returns></returns>
        public static string HttpPost(string url, string data, string headerKey, string headerValue)
        {
            try
            {
                //create psot request
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/json;charset=UTF-8";
                request.Headers[headerKey] = headerValue;
                byte[] payload = Encoding.UTF8.GetBytes(data);
                request.ContentLength = payload.Length;

                //send post request
                Stream writer = request.GetRequestStream();
                writer.Write(payload, 0, payload.Length);
                writer.Close();

                //get response
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream stream = response.GetResponseStream();
                StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                string value = reader.ReadToEnd();

                reader.Close();
                stream.Close();
                response.Close();

                return value;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public static string HttpGet(string url, string data)
        {
            try
            {
                //create psot request
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                request.ContentType = "text/html;charset=UTF-8";
                byte[] payload = Encoding.UTF8.GetBytes(data);
                request.ContentLength = payload.Length;

                //send post request
                Stream writer = request.GetRequestStream();
                writer.Write(payload, 0, payload.Length);
                writer.Close();

                //get response
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream stream = response.GetResponseStream();
                StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                string value = reader.ReadToEnd();

                reader.Close();
                stream.Close();
                response.Close();

                return value;
            }
            catch (Exception)
            {
                return "";
            }
        }

        public static string HttpGet(string url, string headerKey, string headerValue)
        {
            try
            {
                var request = WebRequest.Create(url);
                request.Method = "GET";
                request.ContentType = "application/json;charset=UTF-8";
                request.Headers[headerKey] = headerValue;
                var webResponse = request.GetResponse();
                var webStream = webResponse.GetResponseStream();

                var reader = new StreamReader(webStream);
                var data = reader.ReadToEnd();


               return data;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// 指定Post地址使用Get 方式获取全部字符串
        /// </summary>
        /// <param name="url">请求后台地址</param>
        /// <returns></returns>
        public static string HttpPost(string url, Dictionary<string, string> dic)
        {
            string result = "";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";
            #region 添加Post 参数
            StringBuilder builder = new StringBuilder();
            int i = 0;
            foreach (var item in dic)
            {
                if (i > 0)
                    builder.Append("&");
                builder.AppendFormat("{0}={1}", item.Key, item.Value);
                i++;
            }
            byte[] data = Encoding.UTF8.GetBytes(builder.ToString());
            req.ContentLength = data.Length;
            using (Stream reqStream = req.GetRequestStream())
            {
                reqStream.Write(data, 0, data.Length);
                reqStream.Close();
            }
            #endregion
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            Stream stream = resp.GetResponseStream();
            //获取响应内容
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                result = reader.ReadToEnd();
            }
            return result;
        }
    }
    #endregion

    #region generated UniqueData
    /// <summary>
    /// Generating unique number
    /// </summary>
    public class UniqueData
    {
        private static object obj = new object();
        private static int GuidInt { get { return Guid.NewGuid().GetHashCode(); } }
        private static string GuidIntStr { get { return Math.Abs(GuidInt).ToString(); } }

        /// <summary>
        /// Generating
        /// </summary>
        /// <param name="mark">prefix</param>
        /// <param name="timeType">Exact type 1 day,2 hours,3 minutes, 4 seconds (default) </param>
        /// <param name="id">id Less than or equal to 0 randomly generates the id</param>
        /// <returns></returns>
        public static string Gener(string mark, int timeType, int id)
        {
            lock (obj)
            {
                var number = mark;
                var ticks = (DateTime.Now.Ticks - GuidInt).ToString();
                int fillCount = 0;//Complement digit

                number += GetTimeStr(timeType, out fillCount);
                if (id > 0)
                {
                    number += ticks.Substring(ticks.Length - (fillCount + 3)) + id.ToString().PadLeft(10, '0');
                }
                else
                {
                    number += ticks.Substring(ticks.Length - (fillCount + 3)) + GuidIntStr.PadLeft(10, '0');
                }
                return number;
            }
        }

        /// <summary>
        /// generate
        /// </summary>
        /// <param name="mark">prefix</param>
        /// <param name="timeType">Exact type 1 day,2 hours,3 minutes, 4 seconds, 5 millisecond (default)</param>
        /// <param name="id">id Less than or equal to 0 randomly generates the id</param>
        /// <returns></returns>
        public static string GenerLong(string mark, int timeType, long id)
        {
            lock (obj)
            {
                var number = mark;
                var ticks = (DateTime.Now.Ticks - GuidInt).ToString();
                int fillCount = 0;//Complement digit

                number += GetTimeStr(timeType, out fillCount);
                if (id > 0)
                {
                    number += ticks.Substring(ticks.Length - fillCount) + id.ToString().PadLeft(19, '0');
                }
                else
                {
                    number += GuidIntStr.PadLeft(10, '0') + ticks.Substring(ticks.Length - (9 + fillCount));
                }
                return number;
            }
        }

        /// <summary>
        /// 获取时间字符串
        /// </summary>
        /// <param name="timeType">Exact type 1 day,2 hours,3 minutes, 4 seconds, 5 millisecond (default)</param>
        /// <param name="fillCount">Complement digit</param>
        /// <returns></returns>
        private static string GetTimeStr(int timeType, out int fillCount)
        {
            var time = DateTime.Now;
            if (timeType == 1)
            {
                fillCount = 10;
                return time.ToString("yyyyMMdd");
            }
            else if (timeType == 2)
            {
                fillCount = 8;
                return time.ToString("yyyyMMddHH");
            }
            else if (timeType == 3)
            {
                fillCount = 6;
                return time.ToString("yyyyMMddHHmm");
            }
            else if (timeType == 4)
            {
                fillCount = 4;
                return time.ToString("yyyyMMddHHmmss");
            }
            else
            {
                fillCount = 0;
                return time.ToString("yyyyMMddHHmmssffff");
            }
        }
    }
    #endregion

}