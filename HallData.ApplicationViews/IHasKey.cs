using System;

namespace HallData.ApplicationViews
{
	/// <summary>
	/// This interface is used by the repository and business frameworks to communicate key information on a view
	/// </summary>
	/// <typeparam name="TKey">The type of the key</typeparam>
	public interface IHasKey<TKey>
	{
		/// <summary>
		/// A property to get and set the Key on the entity
		/// </summary>
		TKey Key { get; set; }
	}

	/// <summary>
	/// An integer implemention of <see cref="IHasKey{TKey}"/>
	/// </summary>
	public interface IHasKey : IHasKey<int> { }

	public interface IHasInstanceGuid
	{
		Guid InstanceGuid { get; }
	}
}
