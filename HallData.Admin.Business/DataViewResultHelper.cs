using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.Admin.ApplicationViews;
using HallData.Utilities;


namespace HallData.Admin.Business
{
    public static class DataViewResultHelper
    {
        /// <summary>
        /// Returns Data View Results that have different interfaceid-collectioninterfaceattributeids for a given result index
        /// </summary>
        /// <param name="results"></param>
        /// <returns></returns>
        public static IEnumerable<IGrouping<int, DataViewResultResult>> GetComposedResults(IEnumerable<DataViewResultResult> results)
        {
            var resultsGroup = results.ToLookup(g => g.ResultIndex.Value);
            return results.GroupBy(g => new DataViewResultCompositionKey(g.ResultIndex.Value, g.Interface.InterfaceId.Value, g.CollectionInterfaceAttribute.InterfaceAttributeId)).Where(g =>
                g.Count() != resultsGroup[g.Key.ResultIndex].Count()).SelectMany(g => g).GroupBy(g => g.ResultIndex.Value);
        }
        public struct DataViewResultCompositionKey
        {
            public int ResultIndex { get; set; }
            public int InterfaceId { get; set; }
            public int? CollectionInterfaceAttributeId { get; set; }
            public DataViewResultCompositionKey(int resultIndex, int interfaceId, int? collectionInterfaceAttributeId) : this()
            {
                this.ResultIndex = resultIndex;
                this.CollectionInterfaceAttributeId = collectionInterfaceAttributeId;
                this.InterfaceId = interfaceId;
            }
            public override bool Equals(object obj)
            {
                DataViewResultCompositionKey? id = obj as DataViewResultCompositionKey?;
                if (id == null)
                    return false;
                return id.Value.CollectionInterfaceAttributeId == this.CollectionInterfaceAttributeId && id.Value.ResultIndex == this.ResultIndex && id.Value.InterfaceId == this.InterfaceId;
            }
            public override int GetHashCode()
            {
                return HashCodeProvider.BuildHashCode(this.ResultIndex, this.InterfaceId, this.CollectionInterfaceAttributeId);
            }
        }
    }
}
