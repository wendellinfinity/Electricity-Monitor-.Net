using System;
using System.Net;
using System.Xml;
using System.Xml.Serialization;

namespace RequestLab {
     /// <summary>
     /// Sensor data object
     /// </summary>
     [XmlRoot("SensorData")]
     public class SensorData {

          private int _nodeId;
          public int NodeId {
               get { return this._nodeId; }
               set { this._nodeId = value; }
          }

          private DateTime _timeStamp;
          [XmlElement("TimeStamp")]
          public DateTime TimeStamp {
               get { return this._timeStamp; }
               set { this._timeStamp = value; }
          }

          private double _powerLevel;
          [XmlElement("PowerLevel")]
          public double PowerLevel {
               get { return this._powerLevel; }
               set { this._powerLevel = value; }
          }

          private bool _hasMotion;
          [XmlElement("HasMotion")]
          public bool HasMotion {
               get { return this._hasMotion; }
               set { this._hasMotion = value; }
          }

          public SensorData() { }

          public static SensorData ReadSensorData(string soap) {
               SensorData sd;
               XmlTextReader xmread;
               sd = null;
               try {
                    sd = new SensorData();
                    using (System.IO.StringReader read = new System.IO.StringReader(soap)) {
                         xmread = new XmlTextReader(read);
                         xmread.ReadStartElement("SensorDataContainer");
                         xmread.ReadStartElement("Sensor");
                         xmread.ReadStartElement("HasMotion");
                         sd.HasMotion = bool.Parse(xmread.ReadString());
                         xmread.ReadEndElement();
                         xmread.ReadStartElement("NodeId");
                         sd.NodeId = int.Parse(xmread.ReadString());
                         xmread.ReadEndElement();
                         xmread.ReadStartElement("PowerLevel");
                         sd.PowerLevel = int.Parse(xmread.ReadString());
                         xmread.ReadEndElement();
                         xmread.ReadStartElement("TimeStamp");
                         sd.TimeStamp = DateTime.Parse(xmread.ReadString());
                         xmread.ReadEndElement();
                         xmread.ReadEndElement();
                         xmread.ReadEndElement();
                    }
               } catch (Exception) {
                    throw;
               }
               return (sd);
          }
     }
}
