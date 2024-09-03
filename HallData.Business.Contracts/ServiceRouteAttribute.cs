using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HallData.Business
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple=true)]
    public class ServiceRoute : Attribute
    {
        public string RoutePath { get; private set; }
        public string Key { get; private set; }
        public int[] IdPathMapping { get; private set; }
        public ServiceRoutePriorty Priority { get; private set; }
        public ServiceRoute(string key, string routePath, params int[] idPathMapping)
        {
            this.RoutePath = routePath;
            this.Key = key;
            string[] paths = this.RoutePath.Split('/');
            this.Priority = paths[0].Contains("{") ? ServiceRoutePriorty.Key : ServiceRoutePriorty.Root;
            if (idPathMapping == null || idPathMapping.Length == 0)
            {
                int idCountForPath = 0;
                List<int> mapping = new List<int>();
                for (var i = 0; i < paths.Length; i++)
                {
                    if (paths[i].Contains("{"))
                        idCountForPath++;
                    else if (idCountForPath > 0)
                    {
                        mapping.Add(idCountForPath);
                        idCountForPath = 0;
                    }
                }
                if (idCountForPath > 0)
                    mapping.Add(idCountForPath);
                if (mapping.Count == 0)
                    mapping.Add(0);
                this.IdPathMapping = mapping.ToArray();
            }
            else
                this.IdPathMapping = idPathMapping;
        }
    }
    public enum ServiceRoutePriorty
    {
        Root = 0,
        Key = 1
    }
}
