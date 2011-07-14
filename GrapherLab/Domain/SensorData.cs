using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Xml;

namespace GrapherLab.Domain {
     /// <summary>
     /// Sensor data object
     /// </summary>
     public class SensorData {

          private int _nodeId;
          public int NodeId {
               get { return this._nodeId; }
               set { this._nodeId = value; }
          }

          private DateTime _timeStamp;
          public DateTime TimeStamp {
               get { return this._timeStamp; }
               set { this._timeStamp = value; }
          }

          private double _powerLevel;
          public double PowerLevel {
               get { return this._powerLevel; }
               set { this._powerLevel = value; }
          }

          private bool _hasMotion;
          public bool HasMotion {
               get { return this._hasMotion; }
               set { this._hasMotion = value; }
          }

          public SensorData() { }

          public static SensorData ReadSensorData(string soap) {
               SensorData sd;
               sd = null;
               try {
                    sd = new SensorData();
                    using (System.IO.StringReader read = new System.IO.StringReader(soap)) {
                         using (XmlReader xmread = XmlReader.Create(read)) {
                              xmread.ReadStartElement("SensorDataContainer");
                              xmread.ReadStartElement("Sensor");
                              xmread.ReadStartElement("HasMotion");
                              sd.HasMotion = xmread.ReadContentAsBoolean();
                              xmread.ReadEndElement();
                              xmread.ReadStartElement("NodeId");
                              sd.NodeId = xmread.ReadContentAsInt();
                              xmread.ReadEndElement();
                              xmread.ReadStartElement("PowerLevel");
                              sd.PowerLevel = xmread.ReadContentAsInt();
                              xmread.ReadEndElement();
                              xmread.ReadStartElement("TimeStamp");
                              sd.TimeStamp = xmread.ReadContentAsDateTime();
                              xmread.ReadEndElement();
                              xmread.ReadEndElement();
                              xmread.ReadEndElement();
                         }
                    }
               } catch (Exception) {
                    throw;
               }
               return (sd);
          }
     }
}
