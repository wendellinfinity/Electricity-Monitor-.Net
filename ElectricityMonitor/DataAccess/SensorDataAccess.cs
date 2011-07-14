using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace ElectricityMonitor.DataAccess {

     public class SensorDataAccess {

          private const string CN = @"Data Source=(local)\SQLEXP2008;Initial Catalog=ElectricityMonitorDB;Integrated Security=True";

          public static void SaveSensorData(Domain.SensorData sd) {
               SqlConnection connection = new SqlConnection(CN);
               SqlCommand insert = new SqlCommand();
               SqlParameter nodeId, timeStamp, powerLevel, motionDetected;
               insert.CommandText = "INSERT INTO [SensorFeed] (NodeID, TimeStamp, PowerLevel, MotionDetected) VALUES (@NodeID, @TimeStamp, @PowerLevel, @MotionDetected)";
               nodeId = new SqlParameter("@NodeID", System.Data.SqlDbType.Int);
               nodeId.Value = sd.NodeId;
               timeStamp = new SqlParameter("@TimeStamp", System.Data.SqlDbType.DateTime);
               timeStamp.Value = sd.TimeStamp;
               powerLevel = new SqlParameter("@PowerLevel", System.Data.SqlDbType.Float);
               powerLevel.Value = sd.PowerLevel;
               motionDetected = new SqlParameter("@MotionDetected", System.Data.SqlDbType.Bit);
               motionDetected.Value = 0;
               insert.Parameters.Add(nodeId);
               insert.Parameters.Add(timeStamp);
               insert.Parameters.Add(powerLevel);
               insert.Parameters.Add(motionDetected);
               insert.Connection = connection;
               try {
                    connection.Open();
                    insert.ExecuteNonQuery();
               } catch (Exception ex) {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
               } finally {
                    connection.Close();
                    insert.Dispose();
                    connection.Dispose();
               }
          }

          public static Domain.SensorData GetLatestNode(int nodeId) {
               SqlConnection connection = new SqlConnection(CN);
               SqlCommand command = new SqlCommand();
               SqlDataReader reader = null;
               SqlParameter nodeIdParam;
               Domain.SensorData node;
               node = null;
               command.CommandText = "SELECT TOP 1 NodeId,TimeStamp,PowerLevel FROM [SensorFeed] WHERE NodeID=@NodeID ORDER BY TimeStamp DESC";
               nodeIdParam = new SqlParameter("@NodeID", System.Data.SqlDbType.Int);
               nodeIdParam.Value = nodeId;
               command.Parameters.Add(nodeIdParam);
               command.Connection = connection;
               try {
                    connection.Open();
                    reader = command.ExecuteReader();
                    while (reader.Read()) {
                         node = new Domain.SensorData() { NodeId = reader.GetInt32(0), TimeStamp = reader.GetDateTime(1), PowerLevel = reader.GetDouble(2), HasMotion = false };
                    }
                    reader.Close();
               } catch (Exception ex) {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
               } finally {
                    connection.Close();
                    if (reader != null) reader.Dispose();
                    command.Dispose();
                    connection.Dispose();
               }
               return (node);
          }

     }

}