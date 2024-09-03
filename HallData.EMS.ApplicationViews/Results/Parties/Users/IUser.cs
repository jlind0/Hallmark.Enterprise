using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.ApplicationViews;
using HallData.Validation;

namespace HallData.EMS.ApplicationViews.Results
{
    /// <summary>
    /// Interface for User Key
    /// </summary>
    public interface IUserKey : IPersonKey<Guid> 
    {
        
    }

    /// <summary>
    /// Interface for User with restriction by Title, Party and Organization type
    /// </summary>
    /// <typeparam name="TPartyType">Party type</typeparam>
    /// <typeparam name="TOrganization">Organization type</typeparam>
    public interface IUser<TPartyType, TOrganization> : IUserKey, IPerson<Guid, TPartyType>
        where TPartyType: PartyTypeKey
        where TOrganization: IOrganizationKey
    {
        /// <summary>
        /// Organization User Of
        /// </summary>
        /// <remarks>Organization user belongs to</remarks>
        TOrganization UserOf { get; set; }
        /// <summary>
        /// User Name
        /// </summary>
        /// <remarks>Name of the user</remarks>
        string UserName { get; set; }
    }

    /// <summary>
    /// Interface for User with restriction by Title, Party, Status and Organization type
    /// </summary>
    /// <typeparam name="TPartyType">Party type</typeparam>
    /// <typeparam name="TOrganization">Organization type</typeparam>
    /// <typeparam name="TStatusType">Status type</typeparam>
    public interface IUser<TPartyType, TOrganization, TStatusType> : IUser<TPartyType, TOrganization>, IPerson<Guid, TPartyType, TStatusType>
        where TPartyType : PartyTypeKey
        where TStatusType: StatusTypeKey
        where TOrganization: IOrganizationKey
    {
        /// <summary>
        /// User Relationship Status
        /// </summary>
        /// <remarks>Status of the user relationship</remarks>
        TStatusType UserRelationshipStatus { get; set; }
    }
    public interface IUserForAddBase : IUser<PartyTypeKey, OrganizationKey, StatusTypeKey>, IPersonForAddBase
    {
        /// <summary>
        /// User password
        /// </summary>
        string Password { get; set; }
    }
    /// <summary>
    /// Interface for User for Add operation
    /// </summary>
    public interface IUserForAdd : IUserForAddBase, IPersonForAdd
    {
        string PasswordHash { get; set; }
    }
    public interface IUserForUpdateBase : IUser<PartyTypeKey, OrganizationKey>, IPersonForUpdateBase { }
    /// <summary>
    /// Interface for User for Update operation
    /// </summary>
    public interface IUserForUpdate : IUserForUpdateBase, IPersonForUpdate { }
    public interface IUserResultBase : IUser<PartyTypeResult, OrganizationResult, StatusTypeResult>, IPersonResultBase { }
    /// <summary>
    /// Interface for User Result 
    /// </summary>
    public interface IUserResult : IUserResultBase, IPersonResult { }
}
