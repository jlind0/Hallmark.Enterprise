using System;
using System.Collections.Generic;
using HallData.Utilities;
using System.Collections;
using Newtonsoft.Json;

namespace HallData.ApplicationViews
{
	public interface IMergable
	{
		MergeActions MergeAction { get; set; }
	}

	public enum MergeActions
	{
		Add,
		Update,
		HardDelete,
		SoftDelete
	}

	public interface IMerge<out TCommon> : IEnumerable<TCommon>
		where TCommon: IMergable
	{
		Type AddType { get; }
		Type UpdateType { get; }
		Type DeleteType { get; }
		IList AddCollection { get; }
		IList UpdateCollection { get; }
		IList HardDeleteCollection { get; }
		IList SoftDeleteCollection { get; }
	}

	public interface IMerge<TAdd, TUpdate, TDelete, out TCommon> : IMerge<TCommon>
		where TCommon: IMergable
	{
		List<TAdd> Add { get; set; }
		List<TUpdate> Update { get; set; }
		List<TDelete> HardDelete { get; set; }
		List<TDelete> SoftDelete { get; set; }
	}

	[JsonObject]
	public abstract class Merge<TCommon> : IMerge<TCommon>
		where TCommon: IMergable
	{
		[JsonIgnore]
		public abstract Type AddType { get; }
		[JsonIgnore]
		public abstract Type UpdateType { get; }
		[JsonIgnore]
		public abstract Type DeleteType { get; }
		[JsonIgnore]
		public abstract IList AddCollection { get; }
		[JsonIgnore]
		public abstract IList UpdateCollection { get; }
		[JsonIgnore]
		public abstract IList HardDeleteCollection { get; }
		[JsonIgnore]
		public abstract IList SoftDeleteCollection { get; }

		public abstract IEnumerator<TCommon> GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
	}

	[JsonObject]
	public class Merge<TAdd, TUpdate, TDelete, TCommon> : Merge<TCommon>, IMerge<TAdd, TUpdate, TDelete, TCommon>
		where TCommon: IMergable
	{
		public Merge()
		{
			this.Add = new List<TAdd>();
			this.Update = new List<TUpdate>();
			this.HardDelete = new List<TDelete>();
			this.SoftDelete = new List<TDelete>();
		}

		[JsonProperty]
		public List<TAdd> Add { get; set; }
		public override Type AddType
		{
			get { return typeof(TAdd); }
		}

		public override IList AddCollection
		{
			get { return this.Add; }
		}

		[JsonProperty]
		public List<TUpdate> Update { get; set; }
		public override Type UpdateType
		{
			get { return typeof(TUpdate); }
		}

		public override IList UpdateCollection
		{
			get { return this.Update; }
		}

		[JsonProperty]
		public List<TDelete> HardDelete { get; set; }
		public override IList HardDeleteCollection
		{
			get { return this.HardDelete; }
		}

		[JsonProperty]
		public List<TDelete> SoftDelete { get; set; }
		public override IList SoftDeleteCollection
		{
			get { return this.SoftDelete; }
		}

		public override Type DeleteType
		{
			get { return typeof(TDelete); }
		}

		public override IEnumerator<TCommon> GetEnumerator()
		{
			foreach(var add in this.Add)
			{
				var c = add.CreateRelatedInstance<TCommon>();
				c.MergeAction = MergeActions.Add;
				yield return c;
			}

			foreach(var update in this.Update)
			{
				var c = update.CreateRelatedInstance<TCommon>();
				c.MergeAction = MergeActions.Update;
				yield return c;
			}

			foreach(var delete in this.HardDelete)
			{
				var c = delete.CreateRelatedInstance<TCommon>();
				c.MergeAction = MergeActions.HardDelete;
				yield return c;
			}

			foreach (var delete in this.SoftDelete)
			{
				var c = delete.CreateRelatedInstance<TCommon>();
				c.MergeAction = MergeActions.SoftDelete;
				yield return c;
			}
		}
	}
}
