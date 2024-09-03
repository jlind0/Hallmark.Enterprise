using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.Utilities;

namespace HallData.EMS.ApplicationViews.Results
{
    public interface IEmployeeId : IPartyId
    {
        Guid? EmployerGuid { get; set; }
    }
    /// <summary>
    /// Employee Id
    /// </summary>
    public struct EmployeeId : IEmployeeId
    {
        /// <summary>
        /// Employer Id
        /// </summary>
        public Guid? EmployerGuid { get; set; }
        /// <summary>
        ///Party Id
        /// </summary>
        public Guid PartyGuid { get; set; }
        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="employerId">employer id parameter</param>
        /// <param name="partyId">party id parameter</param>
        public EmployeeId(Guid? employerId, Guid partyId)
            : this()
        {
            EmployerGuid = employerId;
            PartyGuid = partyId;
        }
        /// <summary>
        /// Overridden to return mach of the Employer id
        /// </summary>
        /// <param name="obj">object parameter</param>
        /// <returns>Employer id</returns>
        public override bool Equals(object obj)
        {
            IEmployeeId id = obj as IEmployeeId;
            if (id == null)
                return false;
            return id.EmployerGuid == this.EmployerGuid && id.PartyGuid == this.PartyGuid;
        }
        /// <summary>
        /// Overridden to return Hash code of party id
        /// </summary>
        /// <returns>Hash code of party id</returns>
        public override int GetHashCode()
        {
            return HashCodeProvider.BuildHashCode(this.PartyGuid, this.EmployerGuid);
        }
    }
    public interface IEmployeeKey : IPersonKey
    {

        /// <summary>
        /// Employer Guid
        /// </summary>
        Guid? EmployerGuid { get; set; }
    }
    /// <summary>
    /// Interface for Employee
    /// </summary>
    public interface IEmployeeKey<TKey> : IEmployeeKey, IPersonKey<TKey>
        where TKey: IEmployeeId
    {
    }
    /// <summary>
    /// Interface for Employee with restriction by Party and Title type
    /// </summary>
    /// <typeparam name="TPartyType">Party type</typeparam>
    /// <typeparam name="TTitleType">Title type</typeparam>
    public interface IEmployeeBase<TKey, TPartyType, TTitleType> : IPerson<TKey, TPartyType>, IEmployeeKey<TKey>
        where TPartyType: PartyTypeKey
        where TTitleType: TitleTypeKey
        where TKey: IEmployeeId
    {
        /// <summary>
        /// Title type
        /// </summary>
        /// <remarks>Type of the title</remarks>
        TTitleType Title { get; set; }
    }
    public interface IEmployee<TPartyType, TTitleType> : IEmployeeKey, IEmployeeBase<EmployeeId, TPartyType, TTitleType>
        where TPartyType: PartyTypeKey
        where TTitleType : TitleTypeKey { }
    public interface IEmployeeBaseWithStatus<TKey, TStatusType> : IEmployeeKey<TKey>
        where TKey: IEmployeeId
        where TStatusType: StatusTypeKey
    {
        /// <summary>
        /// Employee Relationship Status
        /// </summary>
        /// <remarks>Status of the Employee relationship</remarks>
        TStatusType EmployeeRelationshipStatus { get; set; }
    }
    public interface IEmployeeWithStatus<TStatusType> : IEmployeeBaseWithStatus<EmployeeId, TStatusType>
        where TStatusType : StatusTypeKey { }
    /// <summary>
    /// Interface for Employee with restriction by Party, Title and Status type
    /// </summary>
    /// <typeparam name="TPartyType">Party type</typeparam>
    /// <typeparam name="TTitleType">Title type</typeparam>
    /// <typeparam name="TStatusType">Status type</typeparam>
    public interface IEmployeeBase<TKey, TPartyType, TTitleType, TStatusType> : IEmployeeBaseWithStatus<TKey, TStatusType>, IEmployeeBase<TKey, TPartyType, TTitleType>, IPerson<TKey, TPartyType, TStatusType>
        where TPartyType : PartyTypeKey
        where TTitleType : TitleTypeKey
        where TStatusType: StatusTypeKey
        where TKey: IEmployeeId
    {
        
    }
    public interface IEmployee<TPartyType, TTitleType, TStatusType> : IEmployeeWithStatus<TStatusType>, IEmployeeBase<EmployeeId, TPartyType, TTitleType, TStatusType>, IEmployee<TPartyType, TTitleType>
        where TPartyType: PartyTypeKey
        where TTitleType: TitleTypeKey
        where TStatusType : StatusTypeKey { }
    public interface IEmployeeBaseWithEmployer<TKey, TStatusType, TOrganization> : IEmployeeBaseWithStatus<TKey, TStatusType>
        where TKey: IEmployeeId
        where TStatusType: StatusTypeKey
        where TOrganization: IOrganizationKey
    {
        /// <summary>
        /// Organization of the Employer
        /// </summary>
        TOrganization Employer { get; set; }
    }
    public interface IEmployeeWithEmployer<TStatusType, TOrganization> : IEmployeeBaseWithEmployer<EmployeeId, TStatusType, TOrganization>, IEmployeeWithStatus<TStatusType>
        where TStatusType: StatusTypeKey
        where TOrganization : IOrganizationKey { }
    /// <summary>
    /// Interface for Employee with restriction by Party, Title, Status and Organization type
    /// </summary>
    /// <typeparam name="TPartyType">Party type</typeparam>
    /// <typeparam name="TTitleType">Title type</typeparam>
    /// <typeparam name="TStatusType">Status type</typeparam>
    /// <typeparam name="TOrganization">Organization type</typeparam>
    public interface IEmployeeBase<TKey, TPartyType, TTitleType, TStatusType, TOrganization> : IEmployeeBaseWithEmployer<TKey, TStatusType, TOrganization>, IEmployeeBase<TKey, TPartyType, TTitleType, TStatusType>
        where TStatusType : StatusTypeKey
        where TPartyType : PartyTypeKey
        where TTitleType : TitleTypeKey
        where TOrganization: IOrganizationKey
        where TKey: IEmployeeId
    {
        
    }
    public interface IEmployee<TPartyType, TTitleType, TStatusType, TOrganization> : IEmployeeWithEmployer<TStatusType, TOrganization>, IEmployeeBase<EmployeeId, TPartyType, TTitleType, TStatusType, TOrganization>, IEmployee<TPartyType, TTitleType, TStatusType>
        where TStatusType : StatusTypeKey
        where TPartyType : PartyTypeKey
        where TTitleType : TitleTypeKey
        where TOrganization : IOrganizationKey { }
    public interface IEmployeeForAddRelationship<TKey> : IEmployeeBaseWithStatus<TKey, StatusTypeKey>
        where TKey : IEmployeeId { }
    public interface IEmployeeForAddRelationship : IEmployeeForAddRelationship<EmployeeId>, IEmployeeWithStatus<StatusTypeKey> { }
    public interface IEmployeeForAddBase<TKey> : IEmployeeBase<TKey, PartyTypeKey, TitleTypeKey, StatusTypeKey>, IPersonForAddBase<TKey>
        where TKey: IEmployeeId
    { }
    public interface IEmployeeForAddBase : IEmployeeForAddBase<EmployeeId>, IEmployee<PartyTypeKey, TitleTypeKey, StatusTypeKey> { }
    /// <summary>
    /// Interface for Employee for Add operation
    /// </summary>
    public interface IEmployeeForAdd<TKey> : IEmployeeForAddBase<TKey>, IPersonForAdd<TKey>
        where TKey: IEmployeeId
    { }
    public interface IEmployeeForAdd : IEmployeeForAdd<EmployeeId>, IEmployeeForAddBase { }
    public interface IEmployeeForUpdateRelationship<TKey> : IEmployeeKey<TKey>
        where TKey : IEmployeeId
    {
        Guid? OriginalEmployerGuid { get; set; }
        Guid? OriginalPartyGuid { get; set; }
    }
    public interface IEmployeeForUpdateRelationship: IEmployeeForUpdateRelationship<EmployeeId> { }
    public interface IEmployeeForUpdateBase<TKey> : IEmployeeBase<TKey, PartyTypeKey, TitleTypeKey>, IPersonForUpdateBase<TKey>
        where TKey: IEmployeeId{ }
    public interface IEmployeeForUpdateBase : IEmployeeForUpdateBase<EmployeeId>, IEmployee<PartyTypeKey, TitleTypeKey> { }
    /// <summary>
    /// Interface for Employee for Update operation
    /// </summary>
    public interface IEmployeeForUpdate<TKey> : IEmployeeForUpdateBase<TKey>, IPersonForUpdate<TKey>
        where TKey : IEmployeeId{ }
    public interface IEmployeeForUpdate : IEmployeeForUpdate<EmployeeId>, IEmployeeForUpdateBase { }
    public interface IEmployeeResultBase<TKey, TOrganization> : IEmployeeBase<TKey, PartyTypeResult, TitleTypeName, StatusTypeResult, TOrganization>, IPersonResultBase<TKey>
        where TKey: IEmployeeId
        where TOrganization : IOrganizationKey { }
    public interface IEmployeeResultBase<TKey> : IEmployeeResultBase<TKey, OrganizationResult>
        where TKey: IEmployeeId
    { }
    public interface IEmployeeResultBase : IEmployeeResultBase<EmployeeId>, IEmployee<PartyTypeResult, TitleTypeName, StatusTypeResult, OrganizationResult> { }
    /// <summary>
    /// Interface for Employee Result
    /// </summary>
    public interface IEmployeeResult<TKey, TOrganization> : IEmployeeResultBase<TKey, TOrganization>, IPersonResult<TKey> 
        where TKey: IEmployeeId
        where TOrganization: IOrganizationKey
    { }
    public interface IEmployeeResult<TKey> : IEmployeeResult<TKey, OrganizationResult>
        where TKey : IEmployeeId { }
    public interface IEmployeeResult : IEmployeeResult<EmployeeId>, IEmployeeResultBase { }
}
