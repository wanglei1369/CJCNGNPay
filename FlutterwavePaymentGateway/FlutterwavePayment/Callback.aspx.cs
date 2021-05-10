using Newtonsoft.Json.Linq;
using PaymentBackend;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace FlutterwavePayment
{
    public partial class Callback : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Request.ServerVariables["REQUEST_METHOD"] == "POST" && !IsPostBack)
            {
                int num = Request.Params.Count;
                if (num != 0)
                {
                    string transactionId = Request.Params["transaction_id"];
                    string endPoint = "https://api.flutterwave.com/v3/transactions/"+transactionId+"/verify";
                    string headerKey = ConfigurationManager.AppSettings["headerKey"];
                    string headerValue = ConfigurationManager.AppSettings["headerValue"];
                    string resString = HttpRequest.HttpGet(endPoint,headerKey,headerValue);
                    var responseData = new JObject();
                    string status = "false";
                    if (!string.IsNullOrEmpty(resString))
                    {
                        try
                        {
                            responseData = JObject.Parse(resString);
                            status = responseData["status"].ToString();
                        }
                        catch (Exception ex)
                        {
                            Response.Write( "false");
                        }

                        
                    }                    
                    
                    if (status == "success")
                    {
                        string strData = responseData["data"].ToString();
                        var jData = JObject.Parse(strData);
                        string orderId = jData["tx_ref"].ToString();
                        string amount = jData["amount"].ToString();
                        string currency = jData["currency"].ToString();
                        string message = "false";
                        string responseResult = jData["processor_response"].ToString();
                        if (responseResult == "Approved")
                        {
                            message = "success";
                        }
                        transactionId = "CJC-NGN" + transactionId;
                        string result = SendResult(orderId, currency, amount, transactionId,message);
                        if (result == "success")
                        {
                            Response.Write(result);
                        }
                    }           
                }
            }
        }

        public string SendResult(string orderId, string orderCurrency, string orderAmount, string transactionId, string message)
        {
            string url = ConfigurationManager.AppSettings["LwCallBackUrl"];
            string signType = ConfigurationManager.AppSettings["LwSignType"];
            string key = ConfigurationManager.AppSettings["LWTrustPayKey"];
            string preMd5 = signType + orderId + orderAmount + orderCurrency + transactionId + message + key;
            string sign = ConvertMD5.GetMD5WithString(preMd5);
            sign = sign.ToLower();

            Dictionary<string, string> values = new Dictionary<string, string>
            {
                {"signType", signType},
                {"orderNo", orderId},
                {"orderAmount", orderAmount},
                {"orderCurrency", orderCurrency},
                {"transactionId", transactionId},
                {"status", message},
                {"sign", sign}
            };
            string time = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            string log = "--TrustPay --time:" + time + ";message :" + message + ";orderid :" + orderId + ";orderAmount : " +
orderAmount + ";orderCurrency : " + orderCurrency + ";trsactionid :" + transactionId;
            Log.logWriter.WriteLog(log);
            string sHtmlText = HttpRequest.HttpPost(url, values);
            return sHtmlText;
        }
    }
}