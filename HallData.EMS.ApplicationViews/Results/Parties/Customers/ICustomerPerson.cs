using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HallData.EMS.ApplicationViews.Results
{
    /// <summary>
    /// Interface for Customer Person Key 
    /// </summary>
    public interface ICustomerPersonKey : IPersonKey<CustomerId>, ICustomerKey { }
    /// <summary>
    /// Interface for Customer Person with restriction by Party type
    /// </summary>
    /// <typeparam name="TPartyType">Party type</typeparam>
    public interface ICustomerPerson<TPartyType> : IPerson<CustomerId, TPartyType>, ICustomer<TPartyType>, ICustomerPersonKey
        where TPartyType: PartyTypeKey
    {
        /// <summary>
        /// Gets and Sets Job Title
        /// </summary>
        /// <remarks>
        /// Job Title of the customer person.
        /// </remarks>
        string JobTitle { get; set; }
        /// <summary>
        /// Gets and Sets Works for
        /// </summary>
        /// <remarks>
        /// Employer name
        /// </remarks>
        string WorksFor { get; set; }
    }
    public interface ICustomerPersonWithStatus<TStatusType> : ICustomerPersonKey, ICustomerWithStatus<TStatusType>
        where TStatusType : StatusTypeKey { }
    /// <summary>
    /// Interface for Customer Person with restriction by Party and Status type
    /// </summary>
    /// <typeparam name="TPartyType">Party type</typeparam>
    /// <typeparam name="TStatusType">Status type</typeparam>
    public interface ICustomerPerson<TPartyType, TStatusType> : ICustomerPerson<TPartyType>, IPerson<CustomerId, TPartyType, TStatusType>,
        ICustomer<TPartyType, TStatusType>
        where TStatusType : StatusTypeKey
        where TPartyType : PartyTypeKey { }
    /// <summary>
    /// Interface for Customer Person with restriction by Party, Status and Organization type
    /// </summary>
    /// <typeparam name="TPartyType">Party type</typeparam>
    /// <typeparam name="TStatusType">Status type</typeparam>
    /// <typeparam name="TOrganization">Organization type</typeparam>
    public interface ICustomerPerson<TPartyType, TStatusType, TOrganization> : ICustomerPerson<TPartyType, TStatusType>,
        ICustomer<TPartyType, TStatusType, TOrganization>
        where TStatusType : StatusTypeKey
        where TPartyType : PartyTypeKey
        where TOrganization : IOrganizationKey { }
    public interface ICustomerPersonForAddRelationship : ICustomerPersonWithStatus<StatusTypeKey>, ICustomerForAddRelationship { }
    public interface ICustomerPersonForAddBase : ICustomerPerson<PartyTypeKey, StatusTypeKey>, ICustomerForAddBase, IPersonForAddBase<CustomerId> { }
    /// <summary>
    /// Interface for Customer Person for Add operation 
    /// </summary>
    public interface ICustomerPersonForAdd : ICustomerPersonForAddBase, ICustomerForAdd, IPersonForAdd<CustomerId>, ICustomerPersonForAddRelationship { }
    public interface ICustomerPersonForUpdateRelationship : ICustomerPersonKey, ICustomerForUpdateRelationship { }
    public interface ICustomerPersonForUpdateBase : ICustomerPerson<PartyTypeKey>, ICustomerForUpdateBase, IPersonForUpdateBase<CustomerId> { }
    /// <summary>
    /// Interface for Customer Person for Update operation 
    /// </summary>
    public interface ICustomerPersonForUpdate : ICustomerPersonForUpdateBase, ICustomerForUpdate, IPersonForUpdate<CustomerId> { }
    public interface ICustomerPersonResultBase : ICustomerPerson<PartyTypeResult, StatusTypeResult, OrganizationResult>, ICustomerResultBase, IPersonResultBase<CustomerId> { }
    /// <summary>
    /// Interface for Customer Person Result 
    /// </summary>
    public interface ICustomerPersonResult : ICustomerPersonResultBase, ICustomerResult, IPersonResult<CustomerId> { }
}
