using System;
using System.IO.Ports;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;

namespace MonitorNode {
     public class Runner {

          public static void Main() {
               Runner monitor;
               monitor = new Runner();
               monitor.Run();
          }

          //Measuring AC mains energy use the non-invasive current transformer method
          //Sketch calculates - Irms and Apparent power. Vrms needs to be set below.
          //OpenEnergyMonitor.org project licenced under GNU General Public Licence
          //Author: Trystan Lea 
          //For analog read
          double value;
          //Constants to convert ADC divisions into mains current values.
          double ADCvoltsperdiv = 0.0048;
          double VDoffset = 2.4476; //Initial value (corrected as program runs)
          //Equation of the line calibration values
          double factorA = 15.2; //factorA = CT reduction factor / rsens
          double Ioffset = -0.08;
          //Constants set voltage waveform amplitude.
          double SetV = 230.0;
          //Counter
          int i = 0;
          int samplenumber = 2000;
          //Used for calculating real, apparent power, Irms and Vrms.
          double sumI = 0.0;

          int sum1i = 0;
          double sumVadc = 0.0;

          double Vadc, Vsens, Isens, Imains, sqI, Irms;
          double apparentPower;

          SerialPort _serialPort = new SerialPort("COM1", 9600, Parity.None, 8, StopBits.One);
          AnalogInput _current = new AnalogInput(Pins.GPIO_PIN_A0);
          string message;

          void Run() {
               _serialPort.Open();
               while (true) {
                    value = _current.Read();
                    //Summing counter
                    i++;
                    //Voltage at ADC
                    Vadc = value * ADCvoltsperdiv;
                    //Remove voltage divider offset
                    Vsens = Vadc - VDoffset;
                    //Current transformer scale to find Imains
                    Imains = Vsens;
                    //Calculates Voltage divider offset.
                    sum1i++; sumVadc = sumVadc + Vadc;
                    if (sum1i >= 1000) {
                         VDoffset = sumVadc / sum1i;
                         sum1i = 0;
                         sumVadc = 0.0;
                    }
                    //Root-mean-square method current
                    //1) square current values
                    sqI = Imains * Imains;
                    //2) sum 
                    sumI = sumI + sqI;
                    if (i >= samplenumber) {
                         i = 0;
                         //Calculation of the root of the mean of the current squared (rms)
                         Irms = factorA * Sqrt(sumI / samplenumber) + Ioffset;
                         //Calculation of the root of the mean of the voltage squared (rms)                     
                         apparentPower = Irms * SetV;
                         message = "[";
                         message += apparentPower.ToString();
                         message += ";";
                         message += SetV.ToString();
                         message += ";";
                         message += Irms.ToString();
                         message += "]";
                         SendSerial(message);
                         //Reset values ready for next sample.
                         sumI = 0.0;
                         Thread.Sleep(1000);
                    }
               }
          }

          /// <summary>
          /// Sends message to serial port
          /// </summary>
          /// <param name="message"></param>
          private void SendSerial(string message) {
               int bytesToRead = _serialPort.BytesToRead;
               byte[] buffer = new byte[message.Length];
               if (bytesToRead > 0) {
                    // get the waiting data
                    byte[] bufferIn = new byte[bytesToRead];
                    // READ any data received
                    _serialPort.Read(bufferIn, 0, bufferIn.Length);
               }
               buffer = System.Text.Encoding.UTF8.GetBytes(message);
               _serialPort.Write(buffer, 0, buffer.Length);
          }

          /// http://highfieldtales.wordpress.com/tag/netduino/
          /// Returns the square root of a specified number
          ///
          /// A number
          /// square root of x
          private static double Sqrt(double x) {
               double i = 0;
               double x1 = 0.0F;
               double x2 = 0.0F;

               if (x == 0F) return 0F;

               while ((i * i) <= x)
                    i += 0.1F;

               x1 = i;

               for (int j = 0; j < 10; j++) {
                    x2 = x;
                    x2 /= x1;
                    x2 += x1;
                    x2 /= 2;
                    x1 = x2;
               }

               return x2;

          }

     }
}
