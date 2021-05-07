using Log;
using Newtonsoft.Json.Linq;
using System;
using System.Configuration;

namespace FlutterwavePayment
{
    public partial class Payment : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.ServerVariables["REQUEST_METHOD"] == "POST" && !IsPostBack)
            {
                int numOfParamsCount = Request.Params.Count;
                if (numOfParamsCount != 0)
                {
                    try
                    {
                        string _orderNo = Request.Params["orderNo"];
                        string _orderAmount = Request.Params["orderAmount"];
                        string _orderCurrency = Request.Params["orderCurrency"];
                        string _customerName = Request.Params["customerName"];
                        string paymentUrl = requestURL(_orderNo, _orderAmount, _orderCurrency, _customerName);
                        Response.Redirect(paymentUrl, false);
                    }

                    catch (Exception ex)
                    {
                        logWriter.WriteLog(ex.Message.ToString());
                    }
                }
            }
        }

        private string requestURL(string orderNo, string orderAmount, string orderCurrency, string customerName)
        {
            string apiUrl = ConfigurationManager.AppSettings["apiUrl"];
            string headerKey = ConfigurationManager.AppSettings["headerKey"];
            string headerValue = ConfigurationManager.AppSettings["headerValue"];
            string redirect_url = ConfigurationManager.AppSettings["redirect_url"];
            string para = "{\"tx_ref\":\"" + orderNo + "\"," +
                "\"amount\":\"" + orderAmount + "\"," +
                "\"currency\":\"" + orderCurrency + "\"," +
                "\"redirect_url\":\"" + redirect_url + "\"," +
                "\"payment_options\":\"card\"," +
                "\"customer\":{\"email\":\"user@gmail.com\",\"phonenumber\":\"080****4528\",\"name\":\"" + customerName + "\"}," +
                "\"customizations\":{\"title\":\"CJC Markets NGN Payment\",\"description\":\"CJCMarkets NGN payment isn't free. Pay the price\",\"logo\":\"https://www.dropbox.com/s/gno3rigq4bpd8go/cjc%20logo%202D%20color.png?dl=0\"}}";
            string sHtmlText = PaymentBackend.HttpRequest.HttpPost(apiUrl, para, headerKey, headerValue);
            var result = JObject.Parse(sHtmlText);
            string strParams = result["data"].ToString();
            var jParams = JObject.Parse(strParams);
            string paymentUrl = jParams["link"].ToString();
            Log.logWriter.WriteLog("--TrustPay--Deposit:" + sHtmlText);

            return paymentUrl;
        }
    }
}