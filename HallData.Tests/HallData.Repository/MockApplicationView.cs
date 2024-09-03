using System;
using System.Collections.Generic;
using HallData.ApplicationViews;
using Newtonsoft.Json;
using HallData.Validation;
using System.ComponentModel.DataAnnotations;

namespace HallData.Tests.HallData.Repository
{
	class MockApplicationView
	{
		// Status Type (most Entities have Status Type) 
		public class StatusTypeKey : IHasKey
		{
			[UpdateOperationParameter]
			[ChildKeyOperationParameter]
			public int? StatusTypeId { get; set; }

			[JsonIgnore]
			public int Key
			{
				get { return this.StatusTypeId ?? 0; }
				set { this.StatusTypeId = value; }
			}
		}
		public class StatusTypeResult : StatusTypeKey
		{
			[AddOperationParameter]
			[UpdateOperationParameter]
			[GlobalizedRequired("STATUSTYPE_NAME_REQUIRED")]
			public string Name { get; set; }
		}
		// Mock Type (most Entities have a Type)
		public class MockTypeKey : IHasKey
		{
			[UpdateOperationParameter]
			[ChildKeyOperationParameter]
			public int? MockTypeId { get; set; }

			[JsonIgnore]
			public int Key
			{
				get { return this.MockTypeId ?? 0; }
				set { this.MockTypeId = value; }
			}
		}
		public partial class MockType : MockTypeKey
		{
			[AddOperationParameter]
			[UpdateOperationParameter]
			[GlobalizedRequired]
			public string Name { get; set; }
		}
		// Mock Interface (of some Entity)
		public interface IMockId
		{
			Guid MockGuid { get; set; }
		}
		public interface IMockBaseKey : IHasKey<Guid>, IHasInstanceGuid
		{
			Guid? MockGuid { get; set; }
		}
		public interface IMockBase<TMockType> : IMockBaseKey
			where TMockType : MockTypeKey
		{
			TMockType MockType { get; set; }
			string Name { get; set; }
			string Code { get; set; }
		}
		public interface IMockBaseWithStatus<TMockType, TStatusType> : IMockBase<TMockType>
			where TMockType : MockTypeKey
			where TStatusType : StatusTypeKey
		{
			TStatusType Status { get; set; }
		}
		public interface IMockBase<TMockType, TStatusType> : IMockBaseWithStatus<TMockType, TStatusType>
			where TMockType : MockTypeKey
			where TStatusType : StatusTypeKey
		{
		}
		public interface IMockKey : IMockBaseKey
		{
		}
		public interface IMock<TMockType> : IMockKey, IMockBase<TMockType>
			where TMockType : MockTypeKey
		{
		}
		public interface IMockWithStatus<TMockType, TStatusType> : IMock<TMockType>, IMockBaseWithStatus<TMockType, TStatusType>
			where TMockType : MockTypeKey
			where TStatusType : StatusTypeKey
		{
		}
		public interface IMock<TMockType, TStatusType> : IMockWithStatus<TMockType, TStatusType>, IMockBase<TMockType, TStatusType>
			where TMockType : MockTypeKey
			where TStatusType : StatusTypeKey
		{
		}
		public interface IMockForAddBase : IMockBase<MockTypeKey>
		{
		}
		public interface IMockForAdd : IMockForAddBase
		{
		}
		public interface IMockForUpdateBase : IMock<MockTypeKey>
		{
		}
		public interface IMockForUpdate : IMockForUpdateBase
		{
		}
		public interface IMockResultBase : IMock<MockType, StatusTypeResult>
		{
		}
		public interface IMockResult : IMockResultBase
		{
		}
		// Mock Implementation (of some Entity)
		public class MockBaseKey : IMockBaseKey
		{
			public MockBaseKey()
			{
				this.InstanceGuid = Guid.NewGuid();
			}
			[UpdateOperationParameter]
			[ChildKeyOperationParameter]
			public virtual Guid? MockGuid { get; set; }
			[JsonIgnore]
			public Guid Key
			{
				get { return this.MockGuid ?? Guid.Empty; }
				set { this.MockGuid = value; }
			}
			public Guid InstanceGuid { get; private set; }
		}
		public class MockBase<TMockType> : MockBaseKey, IMockBase<TMockType>, IValidatableObject
			where TMockType : MockTypeKey
		{
			[ChildView]
			[AddOperationParameter]
			[UpdateOperationParameter]
			public virtual TMockType MockType { get; set; }
			[AddOperationParameter]
			[UpdateOperationParameter]
			public virtual string Name { get; set; }
			[AddOperationParameter]
			[UpdateOperationParameter]
			public virtual string Code { get; set; }

			public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
			{
				if (this.MockGuid == null)
				{
					if (this.MockType == null || this.MockType.MockTypeId == null)
						yield return ValidationResultFactory.Create(new ValidationResult("The Mock Must have a Mock Type if Mock Guid is not provided"), "MOCK_MOCKTYPE_REQUIRED");
					if (string.IsNullOrWhiteSpace(this.Name))
						yield return ValidationResultFactory.Create(new ValidationResult(""), "MOCK_NAME_REQUIRED");
					if (string.IsNullOrWhiteSpace(this.Code))
						yield return ValidationResultFactory.Create(new ValidationResult(""), "MOCK_CODE_REQUIRED");
				}
			}
		}
		public class MockBase<TMockType, TStatusType> : MockBase<TMockType>, IMockBaseWithStatus<TMockType, TStatusType>
			where TMockType : MockTypeKey
			where TStatusType : StatusTypeKey
		{
			[AddOperationParameter]
			[ChildView]
			public virtual TStatusType Status { get; set; }
		}
		public class MockKey : MockBaseKey, IMockKey
		{
		}
		public class Mock<TMockType> : MockBase<TMockType>, IMock<TMockType>
			where TMockType : MockTypeKey
		{
		}



		//public interface IMockKey : IHasInstanceGuid
		//{
		//	Guid? MockGuid { get; set; }
		//}
		//public interface IMockKey<TKey> : IHasKey<TKey>, IMockKey
		//{
		//}
		//public interface IMock<TKey, TMockType> : IMockKey<TKey>
		//	where TMockType : MockTypeKey
		//{
		//	TMockType MockType { get; set; }
		//}
		//public interface IMock<TKey, TMockType, TStatusType> : IMock<TKey, TMockType>
		//	where TStatusType : StatusTypeKey
		//	where TMockType   : MockTypeKey
		//{
		//	TStatusType Status { get; set; }
		//}
		//public interface IMockForAddBase<TKey> : IMock<TKey, MockTypeKey, StatusTypeKey>
		//{
		//}
		//public interface IMockForAdd<TKey> : IMockForAddBase<TKey>
		//{
		//}
		//public interface IMockForAddBase : IMockForAdd<Guid>
		//{
		//}
		//public interface IMockForAdd : IMockForAddBase
		//{
		//}
		//public interface IMockForUpdateBase<TKey> : IMock<TKey, MockTypeKey>
		//{
		//}
		//public interface IMockForUpdateBase : IMockForUpdateBase<Guid>
		//{
		//}
		//public interface IMockForUpdate<TKey> : IMockForUpdateBase<TKey>
		//{
		//}
		//public interface IMockForUpdate : IMockForUpdate<Guid>, IMockForUpdateBase
		//{
		//}
		//public interface IMockResultBase<TKey> : IMock<TKey, MockTypeResult, StatusTypeResult>
		//{
		//}
		//public interface IMockResult<TKey> : IMockResultBase<TKey>
		//{
		//}
		//public interface IMockResultBase : IMockResultBase<Guid>
		//{
		//}
		//public interface IMockResult : IMockResult<Guid>, IMockResultBase
		//{
		//}
		//// Mock Implementation (of some Entity)
		//public abstract class MockKey<TKey> : IMockKey<TKey>
		//{
		//	public MockKey()
		//	{
		//		this.InstanceGuid = Guid.NewGuid();
		//	}
		//	[JsonIgnore]
		//	public abstract TKey Key { get; set; }
		//	[UpdateOperationParameter]
		//	[ChildKeyOperationParameter]
		//	public virtual Guid? MockGuid { get; set; }
		//	[JsonIgnore]
		//	public Guid InstanceGuid { get; private set; }
		//}
		//public class MockKey : MockKey<Guid>
		//{
		//	public override Guid Key
		//	{
		//		get { return this.MockGuid ?? Guid.Empty; }
		//		set { this.MockGuid = value; }
		//	}
		//}
		//public abstract class Mock<TKey, TMockType> : MockKey<TKey>, IMock<TKey, TMockType>, IValidatableObject
		//	where TMockType : MockTypeKey
		//{
		//	[ChildView]
		//	[AddOperationParameter]
		//	[GlobalizedRequired("MOCK_MOCKTYPE_REQUIRED")]
		//	public virtual TMockType MockType { get; set; }
		//	public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		//	{
		//		if (this.MockType == null || this.MockType.MockTypeId == null)
		//			yield return ValidationResultFactory.Create(new ValidationResult("Mock Type is required"), "MOCK_MOCKTYPE_REQUIRED");
		//	}
		//}
		//public abstract class Mock<TKey, TMockType, TStatusType> : Mock<TKey, TMockType>, IMock<TKey, TMockType, TStatusType>
		//	where TMockType : MockTypeKey
		//	where TStatusType : StatusTypeKey
		//{
		//	[ChildView]
		//	[AddOperationParameter]
		//	public virtual TStatusType Status { get; set; }
		//}
		//public class MockGuid<TMockType> : Mock<Guid, TMockType>      //??????????????
		//	where TMockType : MockTypeKey
		//{
		//	public override Guid Key
		//	{
		//		get { return this.MockGuid ?? Guid.Empty; }
		//		set { this.MockGuid = value; }
		//	}
		//}
		//public class MockForAdd : MockForAddBase
		//{
		//	[JsonIgnore]
		//	public override Guid? MockGuid
		//	{
		//		get { return base.MockGuid; }
		//		set { base.MockGuid = value; }
		//	}
		//}
		//public class MockForUpdate : MockGenericGuid<MockTypeKey>, IMockGenericForUpdate
		//{
		//	[GlobalizedRequired("MOCK_GUID_REQUIRED_FORUPDATE")]
		//	public override Guid? MockGuid
		//	{
		//		get { return base.MockGuid; }
		//		set { base.MockGuid = value; }
		//	}
		//}
		//public class MockResult : MockGenericGuid<MockTypeResult, StatusTypeResult>, IMockGenericResult
		//{
		//}
	}
}
