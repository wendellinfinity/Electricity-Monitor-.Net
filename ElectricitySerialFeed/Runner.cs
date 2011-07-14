//http://msmvps.com/blogs/coad/archive/2005/03/23/39466.aspx

using System.IO.Ports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;

namespace ElectricitySerialFeed {
     class Runner {
          // Create the serial port with basic settings
          static private SerialPort _port = new SerialPort("COM4", 9600, Parity.None, 8, StopBits.One);
          static private string _serviceURL = @"http://localhost/ElectricityMonitor/PushSensorDataService.svc/sensor/8080/push/{0}/0";

          [STAThread]
          static void Main(string[] args) {
               // initialize the sensor port, mine was registered as COM8, you may check yours
               // through the hardware devices from control panel
               int bytesToRead = 0;
               string chunk, message;
               _port.Open();
               try {
                    bool start;
                    start = false;
                    message = "";
                    while (true) {
                         // check if there are bytes incoming
                         bytesToRead = _port.BytesToRead;
                         if (bytesToRead > 0) {
                              byte[] input = new byte[bytesToRead];
                              // read the Xbee's input
                              _port.Read(input, 0, bytesToRead);
                              // convert the bytes into string
                              chunk = System.Text.Encoding.UTF8.GetString(input);
                              if (chunk.IndexOf("[") >= 0) {
                                   start = true;
                              }
                              if (start) {
                                   message += chunk;
                              }
                              if (chunk.IndexOf("]") >= 0) {
                                   start = false;
                                   // clean up code
                                   message = message.Trim().Replace("[", "").Replace("]", "");
                                   Console.WriteLine("Received: " + message);
                                   ParseMessage(message);
                                   message = "";
                              }
                              Console.WriteLine("");
                         }
                    }

               } finally {
                    // again always close the serial ports!
                    _port.Close();
               }
          }

          static void ParseMessage(string message) {
               string[] splitted;
               double apparentPower, setV, irms;
               splitted = message.Split(';');
               try {
                    apparentPower = double.Parse(splitted[0]);
                    setV = double.Parse(splitted[1]);
                    irms = double.Parse(splitted[2]);
                    SendSensorData(apparentPower, setV, irms);

               } catch (Exception e) {
                    Console.WriteLine(e.Message);
                    Console.WriteLine("");
               } finally {

               }

          }

          static void SendSensorData(double apparentPower, double setV, double irms) {
               HttpWebRequest request;
               request = (HttpWebRequest)HttpWebRequest.Create(string.Format(_serviceURL, apparentPower));
               request.Headers["Authorization"] = "Basic w.a.capili";
               request.Method = "GET";
               request.BeginGetResponse(new AsyncCallback(SendComplete), request);
          }

          static void SendComplete(IAsyncResult asynchronousResult) {
               HttpWebRequest request = (HttpWebRequest)asynchronousResult.AsyncState;
               HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(asynchronousResult);
               string soapResponse;
               using (System.IO.StreamReader read = new System.IO.StreamReader(response.GetResponseStream())) {
                    soapResponse = read.ReadToEnd();
               }
               Console.WriteLine("Sensor data sent!");
               Console.WriteLine("");
          }

     }
}

