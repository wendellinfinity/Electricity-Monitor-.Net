using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using ElectricityMonitor.Domain;

namespace ElectricityMonitor.Service {
     [ServiceContract]
     public interface IPushSensorDataService {
          [OperationContract]
          void PutSensorData(string nodeId, string power, string motion);
          [OperationContract]
          SensorDataSet GetSensorDataSet(string nodeId, string fromDate, string toDate);
          [OperationContract]
          SensorDataContainer GetSensorData(string nodeId);
     }
}
