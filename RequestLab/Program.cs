using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace RequestLab {
     class Program {
          static void Main(string[] args) {
               HttpWebRequest request;
               request = (HttpWebRequest)HttpWebRequest.Create(@"http://localhost/ElectricityMonitor/PushSensorDataService.svc/sensor/8000/pop");
               request.Headers["Authorization"] = "Basic w.a.capili";
               request.Method = "GET";
               request.BeginGetResponse(new AsyncCallback(Callback), request);
               while (true) ;
          }

          static void Callback(IAsyncResult asynchronousResult) {
               HttpWebRequest request = (HttpWebRequest)asynchronousResult.AsyncState;
               HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(asynchronousResult);
               string soapResponse;
               using (System.IO.StreamReader read = new System.IO.StreamReader(response.GetResponseStream())) {
                    soapResponse = read.ReadToEnd();
               }
               SensorData sd = SensorData.ReadSensorData(soapResponse);
               Console.ReadLine();
          }

     }
}
