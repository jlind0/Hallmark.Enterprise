using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.ApplicationViews;
using Newtonsoft.Json;

namespace HallData.EMS.ApplicationViews
{
	public class PrimaryProductAddress<TMechanismType, TStatusType> : PrimaryAddress<TMechanismType, TStatusType>, IPrimaryProductAddress<TMechanismType, TStatusType>
		where TMechanismType : MechanismTypeKey
		where TStatusType : StatusTypeKey
	{
	}

	public class PrimaryProductAddressForAddUpdate : PrimaryProductAddress<MechanismTypeKey, StatusTypeKey>, IPrimaryProductAddressForAddUpdate
	{
		[JsonIgnore]
		public override MechanismTypeKey MechanismType
		{
			get
			{
				return new MechanismTypeKey() { MechanismTypeId = (int)Enums.MechanismTypes.Address };
			}
			set
			{

			}
		}
		[UpdateOperationParameter]
		public override StatusTypeKey Status
		{
			get
			{
				return base.Status;
			}
			set
			{
				base.Status = value;
			}
		}
	}

	public class PrimaryProductAddress : PrimaryProductAddress<MechanismTypeResult, StatusTypeResult>, IPrimaryProductAddressResult
	{
		public DateTime? CreateDate { get; set; }

		public DateTime? UpdateDate { get; set; }

		public ContactMechanismType ContactMechanismType { get; set; }
	}

	public class PrimaryProductEmail<TMechanismType, TStatusType> : PrimaryEmail<TMechanismType, TStatusType>, IPrimaryProductEmail<TMechanismType, TStatusType>
		where TMechanismType : MechanismTypeKey
		where TStatusType : StatusTypeKey
	{
	}

	public class PrimaryProductEmailForAddUpdate : PrimaryProductEmail<MechanismTypeKey, StatusTypeKey>, IPrimaryProductEmailForAddUpdate
	{
		[JsonIgnore]
		public override MechanismTypeKey MechanismType
		{
			get
			{
				return new MechanismTypeKey() { MechanismTypeId = (int)Enums.MechanismTypes.Email };
			}
			set
			{

			}
		}
		[UpdateOperationParameter]
		public override StatusTypeKey Status
		{
			get
			{
				return base.Status;
			}
			set
			{
				base.Status = value;
			}
		}
	}

	public class PrimaryProductEmail : PrimaryProductEmail<MechanismTypeResult, StatusTypeResult>, IPrimaryProductEmailResult
	{

		public DateTime? CreateDate { get; set; }

		public DateTime? UpdateDate { get; set; }

		public ContactMechanismType ContactMechanismType { get; set; }
	}

	public class PrimaryProductPhone<TMechanismType, TStatusType> : PrimaryPhone<TMechanismType, TStatusType>, IPrimaryProductPhone<TMechanismType, TStatusType>
		where TMechanismType : MechanismTypeKey
		where TStatusType : StatusTypeKey
	{
	}

	public class PrimaryProductPhoneForAddUpdate : PrimaryProductPhone<MechanismTypeKey, StatusTypeKey>, IPrimaryProductPhoneForAddUpdate
	{
		[JsonIgnore]
		public override MechanismTypeKey MechanismType
		{
			get
			{
				return new MechanismTypeKey() { MechanismTypeId = (int)Enums.MechanismTypes.Phone };
			}
			set
			{

			}
		}
		[UpdateOperationParameter]
		public override StatusTypeKey Status
		{
			get
			{
				return base.Status;
			}
			set
			{
				base.Status = value;
			}
		}
	}

	public class PrimaryProductPhone : PrimaryProductPhone<MechanismTypeResult, StatusTypeResult>, IPrimaryProductPhoneResult
	{
		public DateTime? CreateDate { get; set; }

		public DateTime? UpdateDate { get; set; }

		public ContactMechanismType ContactMechanismType { get; set; }
	}
}
