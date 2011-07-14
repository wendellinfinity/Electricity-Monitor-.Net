using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ElectricityMonitor.DataAccess;
using System.Data.Linq.Mapping;
using System.Collections.ObjectModel;

namespace ElectricityMonitor.Repository {
     public class Repository {

          public static T SelectLatestByPK<T>(String id) where T : class {
               try {
                    using (SensorDatabaseDataContext context = new SensorDatabaseDataContext()) {
                         // get the table by the type passed in
                         var table = context.GetTable<T>();

                         // get the metamodel mappings (database to
                         // domain objects)
                         MetaModel modelMap = table.Context.Mapping;

                         // get the data members for this type
                         ReadOnlyCollection<MetaDataMember> dataMembers = modelMap.GetMetaType(typeof(T)).DataMembers;

                         // find the primary key field name
                         // by checking for IsPrimaryKey
                         string pk = (dataMembers.Single<MetaDataMember>(m => m.IsPrimaryKey)).Name;

                         // return a single object where the id argument
                         // matches the primary key field value
                         //table.OrderByDescending(m => m.TimeStamp);
                         return table.First<T>(delegate(T t) {
                              String memberId = t.GetType().GetProperty(pk).GetValue(t, null).ToString();
                              return memberId.ToString() == id.ToString();
                         });
                    }
               } catch (Exception) {
                    throw;
               }
          }

     }
}