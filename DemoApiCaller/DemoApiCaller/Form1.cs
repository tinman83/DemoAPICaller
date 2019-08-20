using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DemoApiCaller
{
    public partial class Form1 : Form
    {
        private const string URL = "http://localhost:9991/api";

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(URL);

            httpClient.DefaultRequestHeaders.Accept.Add(
           new MediaTypeWithQualityHeaderValue("application/json"));

            var requestMessage = new RequestMessage
            {
                amount = 10,
                tellerId = "test",
                tranKey = "tranKey"

            };

            StringContent requestMessageContent = new StringContent(JsonConvert.SerializeObject(requestMessage));

            HttpResponseMessage response = httpClient.PostAsync("Sale", requestMessageContent).Result;  // Blocking call! Program will wait here until a response is received or a timeout occurs.
            if (response.IsSuccessStatusCode)
            {

                var responseResult = response.Content.ReadAsStringAsync().Result;

                // Parse the response body.
                ResponseObject documentContent = Newtonsoft.Json.JsonConvert.DeserializeObject<ResponseObject>(responseResult);
               
            }
            else
            {
                //Post NOT Successfull, show error message

                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }

          

            //Dispose once all HttpClient calls are complete. This is not necessary if the containing object will be disposed of; for example in this case the HttpClient instance will be disposed automatically when the application terminates so the following call is superfluous.
            httpClient.Dispose();

        }

    }

    public class RequestMessage
    {
        public decimal amount { get; set; }
        public decimal cashback { get; set; }
        public string tellerId { get; set; }
        public string tranKey { get; set; }

        public string virtualMid { get; set; }
        public string virtualTid { get; set; }

    }

    public class ResponseObject
    {
        public string aid { get; set; }
        public decimal amount { get; set; }
        public string appExpiryDate { get; set; }

        //NOTE. Add more properties for the ResponseMessage

    }
}
