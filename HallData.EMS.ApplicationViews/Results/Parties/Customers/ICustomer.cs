using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.ApplicationViews;
using HallData.Validation;
using HallData.Utilities;
namespace HallData.EMS.ApplicationViews.Results
{
    public interface ICustomerId : IPartyId
    {
        Guid? CustomerOfPartyGuid { get; set; }
    }
    /// <summary>
    /// Customer Id
    /// </summary>
    public struct CustomerId : ICustomerId
    {
        /// <summary>
        /// Gets and Sets Customer Of Part yGuid
        /// </summary>
        public Guid? CustomerOfPartyGuid { get; set; }
        /// <summary>
        /// Gets and Sets Party Guid
        /// </summary>
        public Guid PartyGuid { get; set; }
        /// <summary>
        /// CustomerId constrictor 
        /// </summary>
        /// <param name="partyGuid">Party Guid parameter</param>
        /// <param name="customerOfGuid">Customer of Guid parameter</param>
        public CustomerId(Guid partyGuid, Guid? customerOfGuid) : this()
        {
            this.PartyGuid = partyGuid;
            this.CustomerOfPartyGuid = customerOfGuid;
        }
        /// <summary>
        /// Overridden to return Customer Id key
        /// </summary>
        /// <param name="obj">object parameter</param>
        /// <returns>Customer Of Party Guid</returns>
        public override bool Equals(object obj)
        {
            ICustomerId key = obj as ICustomerId;
            if (key == null)
                return false;
            return key.CustomerOfPartyGuid == this.CustomerOfPartyGuid && key.PartyGuid == this.PartyGuid;
        }
        /// <summary>
        /// Overridden to return Hash Code of the Party Guid
        /// </summary>
        /// <returns>Hash Code of the Party Guid</returns>
        public override int GetHashCode()
        {
            return HashCodeProvider.BuildHashCode(this.PartyGuid, this.CustomerOfPartyGuid);
        }
    }
    /// <summary>
    /// Interface for Customer Key
    /// </summary>
    public interface ICustomerKey : IPartyKey<CustomerId>
    {
        /// <summary>
        /// Gets and Sets Customer Of Party Guid
        /// </summary>
        /// <remarks>
        /// Guid of the Employer.
        /// </remarks>
        Guid? CustomerOfPartyGuid { get; set; }
    }
    /// <summary>
    /// Interface for Customer with restriction by Party type
    /// </summary>
    /// <typeparam name="TPartyType">Party type</typeparam>
    public interface ICustomer<TPartyType> : IParty<CustomerId, TPartyType>, ICustomerKey
        where TPartyType : PartyTypeKey
    {
        
    }
    public interface ICustomerWithStatus<TStatusType> : ICustomerKey
        where TStatusType: StatusTypeKey
    {
        TStatusType CustomerRelationshipStatus { get; set; }
    }
    /// <summary>
    /// Interface for Customer with restriction by Party and Status type
    /// </summary>
    /// <typeparam name="TPartyType">Party type</typeparam>
    /// <typeparam name="TStatusType">Status type</typeparam>
    public interface ICustomer<TPartyType, TStatusType> : ICustomer<TPartyType>, ICustomerWithStatus<TStatusType>, IParty<CustomerId, TPartyType, TStatusType>
        where TStatusType: StatusTypeKey
        where TPartyType: PartyTypeKey
    {
        
    }
    /// <summary>
    /// Interface for Customer with restriction by Party, Status and Organization type
    /// </summary>
    /// <typeparam name="TPartyType">Party type</typeparam>
    /// <typeparam name="TStatusType">Status type</typeparam>
    /// <typeparam name="TOrganization">Organization type</typeparam>
    public interface ICustomer<TPartyType, TStatusType, TOrganization> : ICustomer<TPartyType, TStatusType>
        where TStatusType: StatusTypeKey
        where TOrganization: IOrganizationKey
        where TPartyType : PartyTypeKey
    {
        TOrganization CustomerOf { get; set; }
    }
    public interface ICustomerForAddRelationship : ICustomerWithStatus<StatusTypeKey> { }
    public interface ICustomerForAddBase : ICustomer<PartyTypeKey, StatusTypeKey>, IPartyForAddBase<CustomerId> { }
    /// <summary>
    /// Interface for Customer for Add operation
    /// </summary>
    public interface ICustomerForAdd : ICustomerForAddBase, ICustomerForAddRelationship, IPartyForAdd<CustomerId> { }
    public interface ICustomerForUpdateRelationship : ICustomerKey
    {
        Guid? OriginalPartyGuid { get; set; }
        Guid? OriginalCustomerOfGuid { get; set; }
    }
    public interface ICustomerForUpdateBase : ICustomer<PartyTypeKey>, IPartyForUpdateBase<CustomerId> { }
    /// <summary>
    /// Interface for Customer for Update operation
    /// </summary>
    public interface ICustomerForUpdate : ICustomerForUpdateBase, IPartyForUpdate<CustomerId> { }
    public interface ICustomerResultBase : ICustomer<PartyTypeResult, StatusTypeResult, OrganizationResult>, IPartyResultBase<CustomerId> { }
    /// <summary>
    /// Interface for Customer Result
    /// </summary>
    public interface ICustomerResult : ICustomerResultBase, IPartyResult<CustomerId> { }
    /// <summary>
    /// Interface for Customer Generic with restriction by Party and Tier type
    /// </summary>
    /// <typeparam name="TPartyType">Party type</typeparam>
    /// <typeparam name="TTierType">Tier type</typeparam>
    public interface ICustomerGeneric<TPartyType, TTierType> : IPartyGeneric<CustomerId, TPartyType, TTierType>, 
        ICustomerOrganization<TPartyType, TTierType>, ICustomerPerson<TPartyType>
        where TPartyType: PartyTypeKey
        where TTierType : TierTypeKey { }
    /// <summary>
    /// Interface for Customer Generic with restriction by Party, Tier and Status type
    /// </summary>
    /// <typeparam name="TPartyType">Party type</typeparam>
    /// <typeparam name="TTierType">Tier type</typeparam>
    /// <typeparam name="TStatusType">Status type</typeparam>
    public interface ICustomerGeneric<TPartyType, TTierType, TStatusType> : IPartyGeneric<CustomerId, TPartyType, TTierType, TStatusType>, 
        ICustomerGeneric<TPartyType, TTierType>, ICustomerOrganization<TPartyType, TTierType, TStatusType>, ICustomerPerson<TPartyType, TStatusType>
        where TPartyType: PartyTypeKey
        where TTierType: TierTypeKey
        where TStatusType : StatusTypeKey { }
    /// <summary>
    /// Interface for Customer Generic with restriction by Party, Tier, Status and Organization type
    /// </summary>
    /// <typeparam name="TPartyType">Party type</typeparam>
    /// <typeparam name="TTierType">Tier type</typeparam>
    /// <typeparam name="TStatusType">Status type</typeparam>
    /// <typeparam name="TOrganization">Organization type</typeparam>
    public interface ICustomerGeneric<TPartyType, TTierType, TStatusType, TOrganization> : 
        ICustomerGeneric<TPartyType, TTierType, TStatusType>, ICustomerOrganization<TPartyType, TTierType, TStatusType, TOrganization>,
        ICustomerPerson<TPartyType, TStatusType, TOrganization>
        where TPartyType: PartyTypeKey
        where TTierType: TierTypeKey
        where TStatusType: StatusTypeKey
        where TOrganization : IOrganizationKey { }
    /// <summary>
    /// Interface for Customer Generic for Add operation
    /// </summary>
    public interface ICustomerGenericForAdd : ICustomerGeneric<PartyTypeKey, TierTypeKey, StatusTypeKey>, ICustomerPersonForAdd, ICustomerOrganizationForAdd { }
    /// <summary>
    /// Interface for Customer Generic for Update operation
    /// </summary>
    public interface ICustomerGenericForUpdate : ICustomerGeneric<PartyTypeKey, TierTypeKey>, ICustomerOrganizationForUpdate, ICustomerPersonForUpdate { }
    /// <summary>
    /// Interface for Customer Generic Result
    /// </summary>
    public interface ICustomerGenericResult : ICustomerGeneric<PartyTypeResult, TierType, StatusTypeResult, OrganizationResult>, ICustomerOrganizationResult, ICustomerPersonResult { }
}

