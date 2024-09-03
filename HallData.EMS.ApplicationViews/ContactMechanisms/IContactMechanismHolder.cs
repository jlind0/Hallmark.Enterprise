using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.ApplicationViews;

namespace HallData.EMS.ApplicationViews
{
    /// <summary>
    /// Defines relationship contract between entity and contact mechanism without entity id
    /// </summary>
    /// <remarks>
    /// Should not be implemented directly </remarks>
    public interface IContactMechanismHolderId
    {
        Guid ContactMechanismGuid { get; set; }
        string ContactMechanismTypeName { get; set; }
    }

    /// <summary>
    /// Defines relationship contract between entity and contact mechanism with entity id
    /// </summary>
    /// <typeparam name="TId">Type of the entity id </typeparam>
    public interface IContactMechanismHolderId<TId> : IContactMechanismHolderId
    {
        TId Id { get; set; }
    }

    public interface IContactMechanismHolderKey<TKey> : IHasKey<TKey>
        where TKey : IContactMechanismHolderId
    {
        Guid? ContactMechanismGuid { get; set; }
        string ContactMechanismTypeName { get; set; }
    }

    public interface IContactMechanismHolder<TKey, TContactMechanismType> : IContactMechanismHolderKey<TKey>
        where TKey : IContactMechanismHolderId
        where TContactMechanismType: ContactMechanismTypeKey
    {
        TContactMechanismType ContactMechanismType { get; set; }
    }

    public interface IContactMechanismHolderWithStatus<TKey, TStatusType> : IContactMechanismHolderKey<TKey>
        where TKey: IContactMechanismHolderId
        where TStatusType: StatusTypeKey
    {
        TStatusType Status { get; set; }
    }

    public interface IContactMechanismHolder<TKey, TContactMechanism, TMechanismType> : IContactMechanismHolderKey<TKey>
        where TContactMechanism: IContactMechanism<TMechanismType>
        where TMechanismType : MechanismTypeKey
        where TKey: IContactMechanismHolderId
    {
        TContactMechanism ContactMechanism { get; set; }
    }

    public interface IContactMechanismHolder<TKey, TContactMechanism, TMechanismType, TStatusType> :
        IContactMechanismHolder<TKey, TContactMechanism, TMechanismType>, IContactMechanismHolderWithStatus<TKey, TStatusType>
        where TContactMechanism: IContactMechanism<TMechanismType, TStatusType>
        where TMechanismType : MechanismTypeKey
        where TKey: IContactMechanismHolderId
        where TStatusType: StatusTypeKey
    {}

    public interface IContactMechanismHolderForAdd<TKey, TContactMechanism> : IContactMechanismHolder<TKey, TContactMechanism, MechanismTypeKey, StatusTypeKey> 
        where TContactMechanism: IContactMechanismForAdd
        where TKey : IContactMechanismHolderId
    { }

    public interface IContactMechanismHolderForAddRelationship<TKey> : IContactMechanismHolderWithStatus<TKey, StatusTypeKey>
        where TKey : IContactMechanismHolderId
    { }

    public interface IContactMechanismHolderForUpdate<TKey, TContactMechanism> : IContactMechanismHolder<TKey, TContactMechanism, MechanismTypeKey> 
        where TContactMechanism: IContactMechanismForUpdate
        where TKey : IContactMechanismHolderId
    { }

    public interface IContactMechanismHolderForUpdateRelationship<TKey> : IContactMechanismHolderKey<TKey>
        where TKey : IContactMechanismHolderId
    {
        Guid? OriginalContactMechanismGuid { get; set; }
        string OriginalContactMechanismTypeName { get; set; }
    }

    public interface IContactMechanismHolderForMergeRelationship<TKey> : IContactMechanismHolderForAddRelationship<TKey>, IContactMechanismHolderForUpdateRelationship<TKey>, IMergable
        where TKey : IContactMechanismHolderId 
    { }

    public interface IContactMechanismHolderResult<TKey, TContactMechanism> : IContactMechanismHolder<TKey, TContactMechanism, MechanismTypeResult, StatusTypeResult>, IContactMechanismHolder<TKey, ContactMechanismType>
        where TContactMechanism : IContactMechanismResult
        where TKey : IContactMechanismHolderId
    { }

    public interface IContactMechanismHolderForMerge<TKey, TContactMechanism> : IContactMechanismHolderForAdd<TKey, TContactMechanism>, IMergable
        where TContactMechanism : IContactMechanismForAdd
        where TKey : IContactMechanismHolderId
    { }

}
