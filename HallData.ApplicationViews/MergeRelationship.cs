using System;
using System.Collections.Generic;
using System.Collections;
using HallData.Utilities;
using Newtonsoft.Json;

namespace HallData.ApplicationViews
{
	public enum RelationshipMergeActions
	{
		Add,
		Remove
	}

	public interface IRelationshipMergeable
	{
		RelationshipMergeActions MergeAction { get; set; }
	}

	public interface IRelationshipMerge<out TMerge> : IEnumerable<TMerge>
		where TMerge: IRelationshipMergeable
	{
		Type DataType { get; }
		IList AddCollection { get; }
		IList RemoveCollection { get; }
	}

	public interface IRelationshipMerge<TData, out TMerge> : IRelationshipMerge<TMerge>
		where TMerge: IRelationshipMergeable
	{
		List<TData> Add { get; set; }
		List<TData> Remove { get; set; }
	}

	public abstract class RelationshipMerge<TMerge> : IRelationshipMerge<TMerge>
		where TMerge: IRelationshipMergeable
	{
		[JsonIgnore]
		public abstract Type DataType { get; }

		[JsonIgnore]
		public abstract IList AddCollection { get; }

		[JsonIgnore]
		public abstract IList RemoveCollection { get; }

		public IEnumerator<TMerge> GetEnumerator()
		{
			foreach (var add in this.AddCollection)
			{
				var m = add.CreateRelatedInstance<TMerge>();
				m.MergeAction = RelationshipMergeActions.Add;
				yield return m;
			}

			foreach (var remove in this.RemoveCollection)
			{
				var m = remove.CreateRelatedInstance<TMerge>();
				m.MergeAction = RelationshipMergeActions.Remove;
				yield return m;
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
	}

	[JsonObject]
	public class RelationshipMerge<TData, TMerge> : RelationshipMerge<TMerge>
		where TMerge: IRelationshipMergeable
	{
		public RelationshipMerge()
		{
			this.Add = new List<TData>();
			this.Remove = new List<TData>();
		}

		public override Type DataType
		{
			get { return typeof(TData); }
		}

		public override IList AddCollection
		{
			get { return this.Add; }
		}

		public override IList RemoveCollection
		{
			get { return this.Remove; }
		}

		[JsonProperty]
		public List<TData> Add { get; set; }

		[JsonProperty]
		public List<TData> Remove { get; set; }
	}
}
