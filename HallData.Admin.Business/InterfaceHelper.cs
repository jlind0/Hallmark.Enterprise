using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.Admin.ApplicationViews;
using HallData.Utilities;

namespace HallData.Admin.Business
{
    public static class InterfaceHelper
    {
        /// <summary>
        /// Gets attributes that that have different type-collection-key combinations for a name
        /// </summary>
        /// <param name="attributes"></param>
        /// <returns></returns>
        public static IEnumerable<IGrouping<InterfaceAttributeCompositionKey, InterfaceAttributeResult>> GetComposedAttributes(this IEnumerable<InterfaceAttributeResult> attributes)
        {
            return attributes.GroupBy(g => new InterfaceAttributeCompositionKey { TypeId = g.Type != null ? g.Type.InterfaceId : null as int?, IsCollection = g.IsCollection,  IsKey = g.IsKey, Name = g.Name }).GroupBy(g => g.Key.Name.ToLower()).Where(g => g.Count() > 1).SelectMany(g => g);
        }
        /// <summary>
        /// Gets attributes that have different collection-key for a name
        /// </summary>
        /// <param name="attributes"></param>
        /// <returns></returns>
        public static IEnumerable<IGrouping<InterfaceAttributeCompositionKey, InterfaceAttributeResult>> GetTypeMismatchedComposedAttributes(this IEnumerable<InterfaceAttributeResult> attributes)
        {
            return attributes.GetComposedAttributes().GroupBy(g => g.Key.Name.ToLower()).Where(g => g.All(gg => !g.All(ggg => ggg.Key.IsKey == gg.Key.IsKey && ggg.Key.IsCollection == gg.Key.IsCollection))).SelectMany(g => g);
        }
        /// <summary>
        /// Gets attributes that have different type for a name
        /// </summary>
        /// <param name="attributes"></param>
        /// <returns></returns>
        public static IEnumerable<IGrouping<InterfaceAttributeCompositionKey, InterfaceAttributeResult>> GetMismatchedComposedAttributes(this IEnumerable<InterfaceAttributeResult> attributes)
        {
            return attributes.GetComposedAttributes().GroupBy(g => g.Key.Name.ToLower()).Where(g => g.All(gg => g.All(ggg => ggg.Key.IsKey == gg.Key.IsKey && ggg.Key.IsCollection == gg.Key.IsCollection))).SelectMany(g => g);
        }
        public struct InterfaceAttributeCompositionKey
        {
            public int? TypeId { get; set; }
            public bool IsCollection { get; set; }
            public bool IsKey { get; set; }
            public string Name { get; set; }
            public override bool Equals(object obj)
            {
                InterfaceAttributeCompositionKey? key = obj as InterfaceAttributeCompositionKey?;
                if (key == null)
                    return false;
                return key.Value.IsCollection == this.IsCollection && key.Value.IsKey == this.IsKey && string.Equals(key.Value.Name, this.Name, StringComparison.InvariantCultureIgnoreCase) && key.Value.TypeId == this.TypeId;
            }
            public override int GetHashCode()
            {
                return HashCodeProvider.BuildHashCode(this.Name, this.IsKey, this.IsCollection, this.TypeId);
            }
        }
    }
}
