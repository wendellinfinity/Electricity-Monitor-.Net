﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ElectricityMonitor.DataAccess
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="ElectricityMonitorDB")]
	public partial class SensorDatabaseDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    #endregion
		
		public SensorDatabaseDataContext() : 
				base(global::System.Configuration.ConfigurationManager.ConnectionStrings["ElectricityMonitorDBConnectionString"].ConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public SensorDatabaseDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public SensorDatabaseDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public SensorDatabaseDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public SensorDatabaseDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<SensorFeed> SensorFeeds
		{
			get
			{
				return this.GetTable<SensorFeed>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.SensorFeed")]
	public partial class SensorFeed
	{
		
		private long _ID;
		
		private int _NodeID;
		
		private System.DateTime _TimeStamp;
		
		private System.Nullable<double> _PowerLevel;
		
		private System.Nullable<bool> _MotionDetected;
		
		public SensorFeed()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ID", AutoSync=AutoSync.Always, DbType="BigInt NOT NULL IDENTITY", IsDbGenerated=true)]
		public long ID
		{
			get
			{
				return this._ID;
			}
			set
			{
				if ((this._ID != value))
				{
					this._ID = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_NodeID", DbType="Int NOT NULL")]
		public int NodeID
		{
			get
			{
				return this._NodeID;
			}
			set
			{
				if ((this._NodeID != value))
				{
					this._NodeID = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_TimeStamp", DbType="DateTime NOT NULL")]
		public System.DateTime TimeStamp
		{
			get
			{
				return this._TimeStamp;
			}
			set
			{
				if ((this._TimeStamp != value))
				{
					this._TimeStamp = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_PowerLevel", DbType="Float")]
		public System.Nullable<double> PowerLevel
		{
			get
			{
				return this._PowerLevel;
			}
			set
			{
				if ((this._PowerLevel != value))
				{
					this._PowerLevel = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_MotionDetected", DbType="Bit")]
		public System.Nullable<bool> MotionDetected
		{
			get
			{
				return this._MotionDetected;
			}
			set
			{
				if ((this._MotionDetected != value))
				{
					this._MotionDetected = value;
				}
			}
		}
	}
}
#pragma warning restore 1591