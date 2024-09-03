using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HallData.EMS.ApplicationViews.Results
{
    /// <summary>
    /// Interface for Person Key  
    /// </summary>
    public interface IPersonKey : IPartyKey { }
    /// <summary>
    /// Interface for Person Key with Key type
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public interface IPersonKey<TKey> : IPartyKey<TKey>, IPersonKey { }
    /// <summary>
    /// Interface for Person with restriction by Party type
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TPartyType"></typeparam>
    public interface IPerson<TKey, TPartyType> : IParty<TKey, TPartyType> , IPersonKey<TKey>
        where TPartyType: PartyTypeKey
    {
        /// <summary>
        /// First Name
        /// </summary>
        /// <remarks>
        /// A person first name
        /// </remarks>
        string FirstName { get; set; }
        /// <summary>
        /// Last Name
        /// </summary>
        /// <remarks>
        /// A person last name
        /// </remarks>
        string LastName { get; set; }
        /// <summary>
        /// Salutation
        /// </summary>
        /// <remarks>
        /// Example of Salutation: "Mr", "Mrs", "Miss"
        /// </remarks>
        string Salutation { get; set; }
        /// <summary>
        /// Middle Name
        /// </summary>
        /// <remarks>
        /// A person middle name
        /// </remarks>
        string MiddleName { get; set; }
        /// <summary>
        /// Suffix
        /// </summary>
        /// <remarks>
        /// Examples of Suffix: "Jr", "3rd", "III"
        /// </remarks>
        string Suffix { get; set; }
    }
    /// <summary>
    /// Interface for Person with restriction by Party and Status type
    /// </summary>
    /// <typeparam name="TKey">Key type</typeparam>
    /// <typeparam name="TPartyType">Party type</typeparam>
    /// <typeparam name="TStatusType">Status type</typeparam>
    public interface IPerson<TKey, TPartyType, TStatusType> : IPerson<TKey, TPartyType>, IParty<TKey, TPartyType, TStatusType>
        where TPartyType : PartyTypeKey
        where TStatusType : StatusTypeKey { }
    public interface IPersonForAddBase<TKey> : IPerson<TKey, PartyTypeKey, StatusTypeKey>, IPartyForAddBase<TKey> { } 
    /// <summary>
    /// Interface for Person for Add operation with Key type
    /// </summary>
    /// <typeparam name="TKey">Key type</typeparam>
    public interface IPersonForAdd<TKey> : IPersonForAddBase<TKey>, IPartyForAdd<TKey> { }

    public interface IPersonForUpdateBase<TKey> : IPerson<TKey, PartyTypeKey>, IPartyForUpdateBase<TKey> { }
    /// <summary>
    /// Interface for Person for Update operation with Key type
    /// </summary>
    /// <typeparam name="TKey">Key type</typeparam>
    public interface IPersonForUpdate<TKey> : IPersonForUpdateBase<TKey>, IPartyForUpdate<TKey> { }
    public interface IPersonForAddBase : IPersonForAddBase<Guid>, IPartyForAddBase { }
    /// <summary>
    /// Interface for Person for Add operation
    /// </summary>
    public interface IPersonForAdd : IPersonForAdd<Guid>, IPartyForAdd, IPersonForAddBase { }
    public interface IPersonForUpdateBase : IPersonForUpdateBase<Guid>, IPartyForUpdateBase { }
    /// <summary>
    /// Interface for Person for Update operation
    /// </summary>
    public interface IPersonForUpdate : IPersonForUpdate<Guid>, IPartyForUpdate, IPersonForUpdateBase { }
    public interface IPersonResultBase<TKey> : IPerson<TKey, PartyTypeResult, StatusTypeResult>, IPartyResultBase<TKey> { }
    /// <summary>
    /// Interface for Person Result with Key type
    /// </summary>
    /// <typeparam name="TKey">Key type</typeparam>
    public interface IPersonResult<TKey> : IPersonResultBase<TKey>, IPartyResult<TKey> { }
    public interface IPersonResultBase : IPersonResultBase<Guid>, IPartyResultBase { }
    /// <summary>
    /// Interface for Person Result
    /// </summary>
    public interface IPersonResult : IPersonResult<Guid>, IPartyResult, IPersonResultBase { }
}
