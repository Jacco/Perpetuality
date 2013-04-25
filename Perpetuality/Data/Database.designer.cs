﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18033
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Perpetuality.Data
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
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="Perpetuality")]
	public partial class DatabaseDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    #endregion
		
		public DatabaseDataContext() : 
				base(global::System.Configuration.ConfigurationManager.ConnectionStrings["PerpetualityConnectionString"].ConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public DatabaseDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public DatabaseDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public DatabaseDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public DatabaseDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.FindUser")]
		private int FindUser([global::System.Data.Linq.Mapping.ParameterAttribute(Name="Email", DbType="NVarChar(256)")] string email, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="Password", DbType="NVarChar(256)")] string password, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="UserID", DbType="BigInt")] ref System.Nullable<long> userID, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Bit")] ref System.Nullable<bool> confirmed)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), email, password, userID, confirmed);
			userID = ((System.Nullable<long>)(result.GetParameterValue(2)));
			confirmed = ((System.Nullable<bool>)(result.GetParameterValue(3)));
			return ((int)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.RegisterNewUser")]
		public int RegisterNewUser([global::System.Data.Linq.Mapping.ParameterAttribute(Name="Email", DbType="NVarChar(256)")] string email, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="Password", DbType="NVarChar(256)")] string password, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="ConfirmHash", DbType="VarChar(32)")] string confirmHash, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="PartnerMail", DbType="Bit")] System.Nullable<bool> partnerMail, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="UserID", DbType="BigInt")] ref System.Nullable<long> userID)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), email, password, confirmHash, partnerMail, userID);
			userID = ((System.Nullable<long>)(result.GetParameterValue(4)));
			return ((int)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.AuthenticateUser")]
		public int AuthenticateUser([global::System.Data.Linq.Mapping.ParameterAttribute(Name="Email", DbType="NVarChar(256)")] string email, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="Password", DbType="NVarChar(256)")] string password, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="IPAddress", DbType="VarChar(22)")] string iPAddress, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="Session", DbType="UniqueIdentifier")] ref System.Nullable<System.Guid> session)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), email, password, iPAddress, session);
			session = ((System.Nullable<System.Guid>)(result.GetParameterValue(3)));
			return ((int)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.ConfirmEmailAddress")]
		private int _ConfirmEmailAddress([global::System.Data.Linq.Mapping.ParameterAttribute(Name="Hash", DbType="VarChar(32)")] string hash)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), hash);
			return ((int)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.EndSession")]
		public int EndSession([global::System.Data.Linq.Mapping.ParameterAttribute(Name="Session", DbType="UniqueIdentifier")] System.Nullable<System.Guid> session, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="IPAddress", DbType="VarChar(22)")] string iPAddress)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), session, iPAddress);
			return ((int)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.GetConfirmationHash")]
		public ISingleResult<GetConfirmationHashResult> GetConfirmationHash([global::System.Data.Linq.Mapping.ParameterAttribute(Name="EmailAddress", DbType="NVarChar(265)")] string emailAddress)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), emailAddress);
			return ((ISingleResult<GetConfirmationHashResult>)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.GetUserEmail")]
		public int GetUserEmail([global::System.Data.Linq.Mapping.ParameterAttribute(Name="UserID", DbType="BigInt")] System.Nullable<long> userID, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="Email", DbType="NVarChar(256)")] ref string email)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), userID, email);
			email = ((string)(result.GetParameterValue(1)));
			return ((int)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.UpdateUserProfile")]
		public int _UpdateUserProfile([global::System.Data.Linq.Mapping.ParameterAttribute(Name="Session", DbType="UniqueIdentifier")] System.Nullable<System.Guid> session, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="IPAddress", DbType="VarChar(22)")] string iPAddress, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="Name", DbType="NVarChar(256)")] string name, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="Language", DbType="Char(2)")] string language)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), session, iPAddress, name, language);
			return ((int)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.GetUserProfile")]
		public ISingleResult<GetUserProfileResult> _GetUserProfile([global::System.Data.Linq.Mapping.ParameterAttribute(Name="Session", DbType="UniqueIdentifier")] System.Nullable<System.Guid> session, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="IPAddress", DbType="VarChar(22)")] string iPAddress)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), session, iPAddress);
			return ((ISingleResult<GetUserProfileResult>)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.GetPlayerState")]
		public int GetPlayerState([global::System.Data.Linq.Mapping.ParameterAttribute(Name="PlayerID", DbType="BigInt")] System.Nullable<long> playerID, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="WorldID", DbType="BigInt")] System.Nullable<long> worldID, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="Balance", DbType="Decimal(18,2)")] ref System.Nullable<decimal> balance, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="CreditProductionRate", DbType="Decimal(18,6)")] ref System.Nullable<decimal> creditProductionRate, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="GameDate", DbType="DateTime")] ref System.Nullable<System.DateTime> gameDate, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="InstalledPower", DbType="Decimal(18,2)")] ref System.Nullable<decimal> installedPower)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), playerID, worldID, balance, creditProductionRate, gameDate, installedPower);
			balance = ((System.Nullable<decimal>)(result.GetParameterValue(2)));
			creditProductionRate = ((System.Nullable<decimal>)(result.GetParameterValue(3)));
			gameDate = ((System.Nullable<System.DateTime>)(result.GetParameterValue(4)));
			installedPower = ((System.Nullable<decimal>)(result.GetParameterValue(5)));
			return ((int)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.InstallPlant")]
		public int InstallPlant([global::System.Data.Linq.Mapping.ParameterAttribute(Name="PlayerID", DbType="BigInt")] System.Nullable<long> playerID, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="WorldID", DbType="BigInt")] System.Nullable<long> worldID, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="PowerPlantTypeID", DbType="BigInt")] System.Nullable<long> powerPlantTypeID, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="Longitude", DbType="Decimal(18,6)")] System.Nullable<decimal> longitude, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="Latitude", DbType="Decimal(18,6)")] System.Nullable<decimal> latitude, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="InstallationSize", DbType="Int")] System.Nullable<int> installationSize, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="SolarPower", DbType="Decimal(18,2)")] System.Nullable<decimal> solarPower, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="CalculateOnly", DbType="Bit")] System.Nullable<bool> calculateOnly, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="Balance", DbType="Decimal(18,2)")] ref System.Nullable<decimal> balance, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="CreditProductionRate", DbType="Decimal(18,6)")] ref System.Nullable<decimal> creditProductionRate, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="GameDate", DbType="DateTime")] ref System.Nullable<System.DateTime> gameDate, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="InstalledPower", DbType="Decimal(18,2)")] ref System.Nullable<decimal> installedPower, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="BuildingCost", DbType="Decimal(18,2)")] ref System.Nullable<decimal> buildingCost, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="BuildingPower", DbType="Decimal(18,2)")] ref System.Nullable<decimal> buildingPower, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="CreditRevenuePerYear", DbType="Decimal(18,2)")] ref System.Nullable<decimal> creditRevenuePerYear)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), playerID, worldID, powerPlantTypeID, longitude, latitude, installationSize, solarPower, calculateOnly, balance, creditProductionRate, gameDate, installedPower, buildingCost, buildingPower, creditRevenuePerYear);
			balance = ((System.Nullable<decimal>)(result.GetParameterValue(8)));
			creditProductionRate = ((System.Nullable<decimal>)(result.GetParameterValue(9)));
			gameDate = ((System.Nullable<System.DateTime>)(result.GetParameterValue(10)));
			installedPower = ((System.Nullable<decimal>)(result.GetParameterValue(11)));
			buildingCost = ((System.Nullable<decimal>)(result.GetParameterValue(12)));
			buildingPower = ((System.Nullable<decimal>)(result.GetParameterValue(13)));
			creditRevenuePerYear = ((System.Nullable<decimal>)(result.GetParameterValue(14)));
			return ((int)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.GetWorldPlayerPlants")]
		public ISingleResult<GetWorldPlayerPlantsResult> GetWorldPlayerPlants([global::System.Data.Linq.Mapping.ParameterAttribute(Name="PlayerID", DbType="BigInt")] System.Nullable<long> playerID, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="WorldID", DbType="BigInt")] System.Nullable<long> worldID, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="MinLongitude", DbType="Decimal(18,6)")] System.Nullable<decimal> minLongitude, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="MaxLongitude", DbType="Decimal(18,6)")] System.Nullable<decimal> maxLongitude, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="MinLatutude", DbType="Decimal(18,6)")] System.Nullable<decimal> minLatutude, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="MaxLatitude", DbType="Decimal(18,6)")] System.Nullable<decimal> maxLatitude)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), playerID, worldID, minLongitude, maxLongitude, minLatutude, maxLatitude);
			return ((ISingleResult<GetWorldPlayerPlantsResult>)(result.ReturnValue));
		}
	}
	
	public partial class GetConfirmationHashResult
	{
		
		private string _strConfirmHash;
		
		public GetConfirmationHashResult()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_strConfirmHash", DbType="VarChar(32) NOT NULL", CanBeNull=false)]
		public string strConfirmHash
		{
			get
			{
				return this._strConfirmHash;
			}
			set
			{
				if ((this._strConfirmHash != value))
				{
					this._strConfirmHash = value;
				}
			}
		}
	}
	
	public partial class GetUserProfileResult
	{
		
		private long _autID;
		
		private System.DateTime _datCreated;
		
		private string _strLanguage;
		
		private string _strEmailAddress;
		
		private string _strName;
		
		public GetUserProfileResult()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_autID", DbType="BigInt NOT NULL")]
		public long autID
		{
			get
			{
				return this._autID;
			}
			set
			{
				if ((this._autID != value))
				{
					this._autID = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_datCreated", DbType="DateTime NOT NULL")]
		public System.DateTime datCreated
		{
			get
			{
				return this._datCreated;
			}
			set
			{
				if ((this._datCreated != value))
				{
					this._datCreated = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_strLanguage", DbType="Char(2) NOT NULL", CanBeNull=false)]
		public string strLanguage
		{
			get
			{
				return this._strLanguage;
			}
			set
			{
				if ((this._strLanguage != value))
				{
					this._strLanguage = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_strEmailAddress", DbType="NVarChar(256) NOT NULL", CanBeNull=false)]
		public string strEmailAddress
		{
			get
			{
				return this._strEmailAddress;
			}
			set
			{
				if ((this._strEmailAddress != value))
				{
					this._strEmailAddress = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_strName", DbType="NVarChar(256)")]
		public string strName
		{
			get
			{
				return this._strName;
			}
			set
			{
				if ((this._strName != value))
				{
					this._strName = value;
				}
			}
		}
	}
	
	public partial class GetWorldPlayerPlantsResult
	{
		
		private long _autID;
		
		private decimal _numLongitude;
		
		private decimal _numLatitude;
		
		private long _intPowerPlantTypeID;
		
		public GetWorldPlayerPlantsResult()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_autID", DbType="BigInt NOT NULL")]
		public long autID
		{
			get
			{
				return this._autID;
			}
			set
			{
				if ((this._autID != value))
				{
					this._autID = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_numLongitude", DbType="Decimal(18,6) NOT NULL")]
		public decimal numLongitude
		{
			get
			{
				return this._numLongitude;
			}
			set
			{
				if ((this._numLongitude != value))
				{
					this._numLongitude = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_numLatitude", DbType="Decimal(18,6) NOT NULL")]
		public decimal numLatitude
		{
			get
			{
				return this._numLatitude;
			}
			set
			{
				if ((this._numLatitude != value))
				{
					this._numLatitude = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_intPowerPlantTypeID", DbType="BigInt NOT NULL")]
		public long intPowerPlantTypeID
		{
			get
			{
				return this._intPowerPlantTypeID;
			}
			set
			{
				if ((this._intPowerPlantTypeID != value))
				{
					this._intPowerPlantTypeID = value;
				}
			}
		}
	}
}
#pragma warning restore 1591
