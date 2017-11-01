using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI.WebControls;


namespace BrutusAPItest
{
   public class BrutusAPI
    {
        private static Encoding enc8 = Encoding.UTF8;
        private string apiKey;
        private string apiSecret;
        private string eu;
       public string baseUrl;
       public int timeout;
        /// <summary>
        /// Initialize class with credentials
        /// </summary>
        /// <param name="a">apiKey</param>
        /// <param name="b">apiSecret</param>Store
        public BrutusAPI(string a, string b)
        {
            apiKey = a;
            apiSecret = b;
            eu = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            baseUrl = "<brutus api base url>";
            timeout = 100000;
        }

        public string PublicRequest(string method)
        {
            var content = new MemoryStream();
            var reqUrl = baseUrl+method;
            var webReq = (HttpWebRequest)WebRequest.Create(reqUrl);
            webReq.Method = "GET";
            webReq.ContentType = "application/x-www-form-urlencoded";
            webReq.Proxy = null;
            try
            {
                WebResponse response = webReq.GetResponse();
                {
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        responseStream.CopyTo(content);
                    }
                }

                return enc8.GetString(content.ToArray());
            }
            catch (Exception exe)
            {
                return "";
            }
        }

        public async Task<string> PublicRequestAsync(string method)
        {
            var content = new MemoryStream();
            var reqUrl = baseUrl + method;
            var webReq = (HttpWebRequest)WebRequest.Create(reqUrl);
            webReq.Method = "GET";
            webReq.ContentType = "application/x-www-form-urlencoded";
            webReq.Proxy = null;

            try
            {
                WebResponse response = await webReq.GetResponseAsync();
                {
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        responseStream.CopyTo(content);
                    }
                }

                return enc8.GetString(content.ToArray());

            }
            catch (Exception exe)
            {
                return "";
            }
        }
        public string getTime()
        {
            var response = PublicRequest("gettime");
            return response;
        }

       public string privateRequest(string method, string postData = null)
       {
           string signature;
           TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1);
           int secondsSinceEpoch = (int) t.TotalSeconds;
           string nonce = secondsSinceEpoch.ToString();
           var reqUrl = baseUrl + method;
           var strUrl = method + "&apikey=" + apiKey + "&nonce=" + nonce;
           byte[] secretkeyBytes = Encoding.UTF8.GetBytes(apiSecret);
           byte[] inputBytes = Encoding.UTF8.GetBytes(strUrl);
           using (var hmac = new HMACSHA512(secretkeyBytes))
           {
               byte[] hashValue = hmac.ComputeHash(inputBytes);
               signature = BitConverter.ToString(hashValue).Replace("-", "").ToLower();
              
           }

           var content = new MemoryStream();
           var webReq = (HttpWebRequest) WebRequest.Create(reqUrl);
           webReq.ContentType = "application/x-www-form-urlencoded";
           webReq.Method = "POST";
           webReq.Headers.Add("Key:" + apiKey);
           webReq.Headers.Add("Sign:" + signature);
           webReq.Headers.Add("Nonce:" + nonce);
           webReq.Proxy = null;
            
           webReq.Timeout = timeout;
           if (postData !=  null)
           {
           
           byte[] data = enc8.GetBytes(postData);

           using (var stream = webReq.GetRequestStream())
           {
               stream.Write(data, 0, data.Length);

           }
       }

       string responseContent = null;

            try
            {
                using (WebResponse response = webReq.GetResponse())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        using (StreamReader sr99 = new StreamReader(stream))
                        {
                            responseContent = sr99.ReadToEnd();
                        }
                    }
                }
                return responseContent;
            }
            catch (Exception exe)
            {
                return "server error";
            }
        }

        public string getAccount()
        {
            var response = privateRequest("getaccount");
            return response;
        }

        public string newTransaction(string name,string email,string item,string amount,string transactionid,string itemdescription="",string notes="")
        {
            NameValueCollection postdata = HttpUtility.ParseQueryString(String.Empty);

            postdata.Add("name", name);
            postdata.Add( "email",email);
            postdata.Add("item", item);
            postdata.Add("amount", amount);
            postdata.Add("transactionid", transactionid);
            postdata.Add("notes", notes);
            postdata.Add("itemdescription", itemdescription);
        
            return response;
        }

        public string getTransactions()
        {
            var response = privateRequest("gettransactions");
            return response;
        }
    }

    }

