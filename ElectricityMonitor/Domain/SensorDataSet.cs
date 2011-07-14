using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace ElectricityMonitor.Domain {

     /// <summary>
     /// Collection of sensor data
     /// </summary>
     [CollectionDataContract(Name = "SensorDataSet")]
     public class SensorDataSet : List<SensorData> {
          public SensorDataSet() { }
          public SensorDataSet(List<SensorData> data) : base(data) { }

          private double _average;
          public double Average {
               get { return this._average; }
               set { this._average= value; }
          }
          
     }

}