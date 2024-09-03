using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.Admin.ApplicationViews;
using HallData.Utilities;

namespace HallData.Admin.Business
{
	public static class DataViewColumnHelper
	{
		/// <summary>
		/// Name-ResultName combinations must be unqiue accross all columns, Name-ResultName-Alias-IsRequired-IsVirtual must be the same for all Name-Result combinations and Different Name-ResultName-Alias-IsRequired-IsVirtual-InterfaceAttribute combinations must be outputted
		/// </summary>
		/// <param name="columns"></param>
		/// <param name="attributeComposed"></param>
		/// <returns></returns>
        public static bool DoColumnsCompose(IEnumerable<DataViewColumnResult> columns, out IEnumerable<IGrouping<ColumnCompositionKey, DataViewColumnResult>> attributeComposed)
		{
			var sharedResultName = columns.GroupBy(g => g.ResultName.ToLowerInvariant());
			var sharedName = columns.GroupBy(g => g.Name.ToLowerInvariant());
			var composed = columns.GroupBy(g => new ColumnCompositionKey(g.ResultName, g.Name, g.Alias, g.IsRequired, g.IsVirtual));
			attributeComposed = composed.Where(g => g.Select(c => c.InterfaceAttribute.InterfaceAttributeId).Distinct().Count() != g.Count());
			return composed.Where(
					g => sharedResultName.Where(s => s.Key == g.Key.ResultName).Count() != g.Count() 
						|| sharedName.Where(s => s.Key == g.Key.Name).Count() != g.Count()
				).Count() == 0;
		}

		public struct ColumnCompositionKey
		{
			public string ResultName { get; set; }
			public string Name { get; set; }
			public string Alias { get; set; }
			public bool IsRequired { get; set; }
			public bool IsVirtual { get; set; }

			public ColumnCompositionKey(string resultName, string name, string alias, bool? isRequired, bool? isVirtual) : this()
			{
				this.ResultName = resultName.ToLowerInvariant();
				this.Name = name.ToLowerInvariant();
				this.Alias = alias;
				this.IsRequired = isRequired ?? false;
				this.IsVirtual = isVirtual ?? false;
			}

			public override bool Equals(object obj)
			{
				ColumnCompositionKey? key = obj as ColumnCompositionKey?;
				if (key == null)
					return false;
				return key.Value.ResultName == this.ResultName && key.Value.Name == this.Name && key.Value.Alias == this.Alias && key.Value.IsRequired == this.IsRequired && key.Value.IsVirtual == this.IsVirtual;
			}

			public override int GetHashCode()
			{
				return HashCodeProvider.BuildHashCode(this.ResultName, this.Name, this.Alias, this.IsRequired, this.IsVirtual);
			}
		}
	}
}
