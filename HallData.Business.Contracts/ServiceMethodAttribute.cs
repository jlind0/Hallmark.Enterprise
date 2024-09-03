using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HallData.Business
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ServiceMethod : Attribute
    {
        public ServiceMethodTypes MethodType { get; set; }
        public bool AcceptSessionHeader { get; set; }
        public bool RequireSessionHeader { get; set; }
        public ServiceMethod(ServiceMethodTypes methodType = ServiceMethodTypes.Get, 
            bool acceptSessionHeader = true, bool requireSessionHeader = true)
        {
            this.MethodType = methodType;
            this.AcceptSessionHeader = acceptSessionHeader;
            this.RequireSessionHeader = acceptSessionHeader && requireSessionHeader;
        }
    }
    public class GetMethod : ServiceMethod
    {
        public GetMethod( bool acceptSessionHeader = true, bool requireSessionHeader = true) : 
            base(ServiceMethodTypes.Get, acceptSessionHeader, requireSessionHeader) { }
    }
    public class AddMethod : ServiceMethod
    {
        public AddMethod( bool acceptSessionHeader = true, bool requireSessionHeader = true) : 
            base(ServiceMethodTypes.Add, acceptSessionHeader, requireSessionHeader) { }
    }
    public class DeleteMethod : ServiceMethod
    {
        public DeleteMethod(bool acceptSessionHeader = true, bool requireSessionHeader = true) : 
            base(ServiceMethodTypes.Delete, acceptSessionHeader, requireSessionHeader) { }
    }
    public class UpdateMethod : ServiceMethod
    {
        public UpdateMethod(bool acceptSessionHeader = true, bool requireSessionHeader = true) : 
            base(ServiceMethodTypes.Update, acceptSessionHeader, requireSessionHeader) { }
    }
    public enum ServiceMethodTypes
    {
        Get,
        Add,
        Update,
        Delete
    }
    [AttributeUsage(AttributeTargets.Method)]
    public class ServiceMethodResultAttribute : Attribute
    {
        public Type ResultType { get; set; }
        public ServiceMethodResultAttribute(Type resultType)
        {
            this.ResultType = resultType;
        }
    }
}
