using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace ElectricityMonitor.Domain {

     public class SensorDataContainer {
          private SensorData _sensor;

          public SensorData Sensor {
               get { return _sensor; }
               set { _sensor = value; }
          }
          
     }

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

          public static SensorDataContainer ExportSensorData(SensorData data) {
               SensorDataContainer sdc = null;
               try {
                    sdc = new SensorDataContainer() { Sensor = data };
               } catch (Exception) {
                    throw;
               }
               return(sdc);
          }

     }

}