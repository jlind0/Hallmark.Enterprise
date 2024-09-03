using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.Business;
using HallData.Admin.ApplicationViews;
using HallData.ApplicationViews;
using System.Threading;


namespace HallData.Admin.Business
{
    public interface IReadOnlyInterfaceImplementation : IReadOnlyBusinessImplementation<InterfaceResult>
    {
        [GetMethod]
        [ServiceRoute("DoesNameExist", "{name}/Exists/")]
        Task<bool> DoesNameExist(string name, CancellationToken token = default(CancellationToken));
        
        [GetMethod]
        [ServiceRoute("AreInterfacesCommon", "{interfaceId1}/Common/{interfaceId2}/")]
        Task<bool> AreInterfacesCommon(int interfaceId1, int interfaceId2, CancellationToken token = default(CancellationToken));
        Task<bool> AreInterfacesCommon(IEnumerable<int> interfaceIds, CancellationToken token = default(CancellationToken));
    }
    public interface IInterfaceImplementationBase : IBusinessImplementation
    {
        [GetMethod]
        [ServiceRoute("CanInterfacesRelate", "{interfaceId}/Relate/{relatedInterfaceId}/")]
        Task<bool> CanInterfacesRelate(int interfaceId, int relatedInterfaceId, CancellationToken token = default(CancellationToken));
        Task<bool> CanInterfacesRelate(IEnumerable<int> interfaceIds, CancellationToken token = default(CancellationToken));
    }
    [Service("interfaces")]
    public interface IInterfaceImplementation : IDeletableBusinessImplementationWithBase<int, InterfaceResult, InterfaceForAdd, InterfaceForUpdate>, IReadOnlyInterfaceImplementation, IInterfaceImplementationBase
    {
        [GetMethod]
        [ServiceRoute("GetAttributes", "{interfaceId}/Attributes/")]
        [ServiceRoute("GetAttributesTypedViewDefault", "{interfaceId}/Attributes/TypedView/")]
        [ServiceRoute("GetAttributesTypedView", "{interfaceId}/Attributes/TypedView/{viewName}/")]
        [ServiceRoute("GetAttributesRecurseNone", "{interfaceId}/Attributes/recurse/none/")]
        [ServiceRoute("GetAttributesRecurseNoneTypedViewDefault", "{interfaceId}/Attributes/recurse/none/TypedView/")]
        [ServiceRoute("GetAttributesRecurseNoneTypedView", "{interfaceId}/Attributes/recurse/none/TypedView/{viewName}/")]
        Task<QueryResults<InterfaceAttributeResult>> GetAttributes(int interfaceId, string viewName = null, [JsonEncode]FilterContext<InterfaceAttributeResult> filter = null, [JsonEncode]SortContext<InterfaceAttributeResult> sort = null, [JsonEncode]PageDescriptor page = null, CancellationToken token = default(CancellationToken));
        [GetMethod]
        [ServiceRoute("GetAttributesRecurseDown", "{interfaceId}/Attributes/recurse/down")]
        [ServiceRoute("GetAttributesRecurseDownTypedViewDefault", "{interfaceId}/Attributes/recurse/down/TypedView/")]
        [ServiceRoute("GetAttributesRecurseDownTypedView", "{interfaceId}/Attributes/recurse/down/TypedView/{viewName}/")]
        Task<QueryResults<InterfaceAttributeResult>> GetAttributesWithDownwardRecursion(int interfaceId, string viewName = null, [JsonEncode]FilterContext<InterfaceAttributeResult> filter = null, [JsonEncode]SortContext<InterfaceAttributeResult> sort = null, [JsonEncode]PageDescriptor page = null, CancellationToken token = default(CancellationToken));
        [GetMethod]
        [ServiceRoute("GetAttributesRecurseUp", "{interfaceId}/Attributes/recurse/up")]
        [ServiceRoute("GetAttributesRecurseUpTypedViewDefault", "{interfaceId}/Attributes/recurse/up/TypedView/")]
        [ServiceRoute("GetAttributesRecurseUpTypedView", "{interfaceId}/Attributes/recurse/up/TypedView/{viewName}/")]
        Task<QueryResults<InterfaceAttributeResult>> GetAttributesWithUpwardRecursion(int interfaceId, string viewName = null, [JsonEncode]FilterContext<InterfaceAttributeResult> filter = null, [JsonEncode]SortContext<InterfaceAttributeResult> sort = null, [JsonEncode]PageDescriptor page = null, CancellationToken token = default(CancellationToken));
        [GetMethod]
        [ServiceRoute("GetAttributesRecurseBi", "{interfaceId}/Attributes/recurse/")]
        [ServiceRoute("GetAttributesRecurseBiTypedViewDefault", "{interfaceId}/Attributes/recurse/TypedView/")]
        [ServiceRoute("GetAttributesRecurseBiTypedView", "{interfaceId}/Attributes/recurse/TypedView/{viewName}/")]
        Task<QueryResults<InterfaceAttributeResult>> GetAttributesWithBiDirectionalRecursion(int interfaceId, string viewName = null, [JsonEncode]FilterContext<InterfaceAttributeResult> filter = null, [JsonEncode]SortContext<InterfaceAttributeResult> sort = null, [JsonEncode]PageDescriptor page = null, CancellationToken token = default(CancellationToken));
        [UpdateMethod]
        [ServiceRoute("RelateInterfaces", "{interfaceId}/Relate/{releatedInterfaceId}/")]
        Task<QueryResult<InterfaceResult>> RelateInterfaces(int interfaceId, int releatedInterfaceId, CancellationToken token = default(CancellationToken));
        [DeleteMethod]
        [ServiceRoute("UnRelateInterfaces", "{interfaceId}/Relate/{releatedInterfaceId}/")]
        Task UnRelateInterfaces(int interfaceId, int releatedInterfaceId, CancellationToken token = default(CancellationToken));
    }
    public interface IInterfacePassThroughImplementation : IInterfaceImplementation { }
}
