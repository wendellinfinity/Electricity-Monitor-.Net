using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.ServiceModel.Activation;
using ElectricityMonitor.Domain;


namespace ElectricityMonitor.Service {
     [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
     public class PushSensorDataService : IPushSensorDataService {

          Dictionary<int, SensorDataSet> _sensors = new Dictionary<int, SensorDataSet>();

          #region ISensorDataService Members

          [WebInvoke(Method = "GET", UriTemplate = "sensor/{nodeId}/push/{power}/{motion}")]
          public void PutSensorData(string nodeId, string power, string motion) {
               int nodeId0;
               if (int.TryParse(nodeId, out nodeId0)) {
                    SensorData sd;
                    float power0;
                    bool motion0;
                    if (!this._sensors.ContainsKey(nodeId0) || this._sensors[nodeId0] == null) {
                         this._sensors[nodeId0] = new SensorDataSet();
                    }
                    if (!float.TryParse(power, out power0)) {
                         power0 = 0;
                    }
                    if (!bool.TryParse(motion, out motion0)) {
                         motion0 = false;
                    }
                    sd = new SensorData() { NodeId = nodeId0, PowerLevel = power0, HasMotion = motion0, TimeStamp = DateTime.Now };
                    DataAccess.SensorDataAccess.SaveSensorData(sd);
                    //this._sensors[nodeId0].Add(sd);
               }
          }

          private double GetRandom() {
               Random r = new Random();
               return (r.Next(10, 180));
          }

          [WebGet(UriTemplate = "sensor/{nodeId}/range/{fromDate}/{toDate}")]
          public SensorDataSet GetSensorDataSet(string nodeId, string fromDate, string toDate) {
               int nodeId0;
               if (int.TryParse(nodeId, out nodeId0)) {
                    SensorDataSet sds = new SensorDataSet();
                    sds.Add(DataAccess.SensorDataAccess.GetLatestNode(nodeId0));
                    return sds;

                    /*
                    if (!this._sensors.ContainsKey(nodeId0) || this._sensors[nodeId0] == null) {
                         SensorDataSet sds = new SensorDataSet();
                         sds.Add(new SensorData() { NodeId = 8000, PowerLevel = this.GetRandom(), TimeStamp = DateTime.Now });
                         return sds;
                    }
                    if (fromDate != null && !"".Equals(fromDate.Trim()) && toDate != null && !"".Equals(toDate.Trim())) {
                         DateTime start, end;
                         if (DateTime.TryParseExact(fromDate, "MMddyyyy", null, System.Globalization.DateTimeStyles.None, out start) &&
                              DateTime.TryParseExact(toDate, "MMddyyyy", null, System.Globalization.DateTimeStyles.None, out end)) {
                              SensorDataSet sds = new SensorDataSet();
                              var range = from sensor in this._sensors[nodeId0]
                                          where (sensor.TimeStamp >= start) &&
                                             (sensor.TimeStamp <= end)
                                          select sensor;
                              foreach (var d in range)
                                   sds.Add(d);
                              return sds;
                         }
                         return null;
                    } else {
                         // return all sensor data
                         return this._sensors[nodeId0];
                    }
                     * */
               }
               return null;
          }

          [WebGet(UriTemplate = "sensor/{nodeId}/pop")]
          public SensorDataContainer GetSensorData(string nodeId) {
               int nodeId0;
               if (int.TryParse(nodeId, out nodeId0)) {
                    SensorData sd = DataAccess.SensorDataAccess.GetLatestNode(nodeId0);
                    if (sd == null) {
                         sd = new SensorData() { PowerLevel = 0 };
                    } else {
                         sd.PowerLevel = sd.PowerLevel / 100;
                    }
                    return SensorData.ExportSensorData(sd);
               }
               return null;
          }

          #endregion

     }
}