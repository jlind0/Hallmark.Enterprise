using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.EMS.ApplicationViews.Results;
using HallData.ApplicationViews;
using HallData.Validation;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace HallData.EMS.ApplicationViews
{
	public class SessionKey : ProductKey, ISessionKey
	{

	}

	public class Session<TProductType> : Product<TProductType>, ISession<TProductType>
		where TProductType : ProductTypeKey
	{

	}

	public class Session<TProductType, TTrack, TOwner> : Session<TProductType>, ISessionWithOwner<TProductType, TOwner>, ISessionWithParent<TProductType, TTrack>
		where TProductType : ProductTypeKey
		where TTrack : ITrackKey
		where TOwner : IOrganizationKey
	{
		[AddOperationParameter]
		[ChildView]
		public virtual TOwner Owner { get; set; }
		[ChildView]
		[AddOperationParameter]
		public virtual TTrack Track { get; set; }
		[ChildView]
		[JsonIgnore]
		public virtual TTrack ParentProduct
		{
			get { return this.Track; }
			set { this.Track = value; }
		}
	}

	public class Session<TProductType, TStatusType, TOwner, TBusinessUnit, TCreator, TTrack> : Session<TProductType, TTrack, TOwner>,
		ISession<TProductType, TStatusType, TOwner, TBusinessUnit, TCreator>
		where TProductType : ProductTypeKey
		where TStatusType : StatusTypeKey
		where TOwner : IOrganizationKey
		where TBusinessUnit : IBusinessUnitKey
		where TCreator : IUserKey
		where TTrack : ITrackKey
	{
		[ChildView]
		public TStatusType Status { get; set; }
		[ChildView]
		public TBusinessUnit BusinessUnit { get; set; }
		[ChildView]
		public TCreator Creator { get; set; }

		public DateTime? CreateDate { get; set; }
	}

	public class SessionForAddBase : Session<ProductTypeKey, TrackKey, OrganizationKey>, ISessionForAdd
	{
		[AddOperationParameter]
		[ChildView]
		public PrimaryProductAddressForAddUpdate AttendenceLocation { get; set; }
		[AddOperationParameter]
		[ChildView]
		public PrimaryProductEmailForAddUpdate ContactEmail { get; set; }
		[AddOperationParameter]
		[ChildView]
		public PrimaryProductPhoneForAddUpdate ContactPhone { get; set; }

		[JsonIgnore]
		public override Guid? ProductGuid
		{
			get
			{
				return base.ProductGuid;
			}
			set
			{
				base.ProductGuid = value;
			}
		}
	}

	public class SessionForAddTrack : SessionForAddBase
	{
		[JsonIgnore]
		public override TrackKey Track
		{
			get
			{
				return base.Track;
			}
			set
			{
				base.Track = value;
			}
		}
		[JsonIgnore]
		public override ProductTypeKey ProductType
		{
			get
			{
				return new ProductTypeKey() { ProductTypeId = (int)Enums.ProductTypes.Session };
			}
			set
			{

			}
		}
	}

	public class SessionForAdd : SessionForAddBase
	{
		[GlobalizedRequired("SESSION_TRACK_REQUIRED")]
		public override TrackKey Track
		{
			get
			{
				return base.Track;
			}
			set
			{
				base.Track = value;
			}
		}
		public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			foreach (var valid in base.Validate(validationContext))
				yield return valid;
			if (this.Track == null || this.Track.ProductGuid == null)
				yield return ValidationResultFactory.Create(new ValidationResult("Track is Required for Session"), "SESSION_TRACK_REQUIRED");
		}
	}

	public class SessionForUpdate : Session<ProductTypeKey>, ISessionForUpdate
	{
		[GlobalizedRequired("SESSION_PRODUCTGUID_REQUIRED")]
		public override Guid? ProductGuid
		{
			get
			{
				return base.ProductGuid;
			}
			set
			{
				base.ProductGuid = value;
			}
		}
		[JsonIgnore]
		public override ProductTypeKey ProductType
		{
			get
			{
				return new ProductTypeKey() { ProductTypeId = (int)Enums.ProductTypes.Session };
			}
			set
			{

			}
		}
		[UpdateOperationParameter]
		[ChildView]
		public PrimaryProductAddressForAddUpdate AttendenceLocation { get; set; }
		[UpdateOperationParameter]
		[ChildView]
		public PrimaryProductEmailForAddUpdate ContactEmail { get; set; }
		[UpdateOperationParameter]
		[ChildView]
		public PrimaryProductPhoneForAddUpdate ContactPhone { get; set; }
	}

	public class SessionResult : Session<ProductType, StatusTypeResult, OrganizationResult, BusinessUnitResult, UserResult, TrackResult>, ISessionResult
	{

		public PrimaryProductAddress AttendenceLocation { get; set; }

		public PrimaryProductEmail ContactEmail { get; set; }

		public PrimaryProductPhone ContactPhone { get; set; }
	}
}
