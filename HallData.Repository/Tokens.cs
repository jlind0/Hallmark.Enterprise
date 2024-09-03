using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using HallData.Utilities;

namespace HallData.Repository
{
    public abstract class Token
    {
        public string PropertyName { get; private set; }
        public string[] PropertyPaths { get; private set; }
        public JObject GetTargetObject(JObject obj)
        {
            var target = obj;
            for (var i = 0; i < this.PropertyPaths.Length - 1; i++)
            {
                if (target.Property(this.PropertyPaths[i]) == null)
                    target.Add(this.PropertyPaths[i], new JObject());
                target = (JObject)target.Property(this.PropertyPaths[i]).Value;
            }
            return target;
        }
        public JToken GetValue(JObject obj)
        {
            JObject target = obj;
            for (var i = 0; i < PropertyPaths.Length - 1; i++)
            {
                var property = target.Property(PropertyPaths[i]);
                if (property == null)
                    return null;
                target = property.Value as JObject;
                if (target == null)
                    return null;
            }
            var prop = target.Property(PropertyPaths.Last());
            if (prop == null)
                return null;
            return prop.Value;
        }
        public void NullEmptyObjects(JObject obj)
        {
            Stack<KeyValuePair<string, JObject>> path = new Stack<KeyValuePair<string, JObject>>();
            var target = obj;
            for(var i = 0; i < this.PropertyPaths.Length; i++)
            {
                var property = target.Property(PropertyPaths[i]);
                if (property == null)
                    break;
                target = property.Value as JObject;
                if (target == null)
                    break;
                path.Push(new KeyValuePair<string, JObject>(PropertyPaths[i], target));
            }
            while(path.Count > 0)
            {
                var prop = path.Pop();
                if (prop.Value.HasValues)
                    break;
                var previousProp = obj;
                if (path.Count > 0)
                    previousProp = path.Peek().Value;
                previousProp.Remove(prop.Key);
            }
        }
        protected void SetPropertyName(string propertyName)
        {
            this.PropertyName = propertyName.Replace("#", "").Replace("?", "");
            this.PropertyPaths = PropertyName.Split('.');
        }
        public static JObject GetKey(JObject target)
        {
            return (JObject)target.Property("$key").Value;
        }
        public static void SetKey(JObject target, JObject key)
        {
            target.Add("$key", key);
        }
        public static void RemoveKey(JObject target)
        {
            target.Remove("$key");
        }
    }
    public class PropertyToken : Token
    {
        public PropertyToken(string path)
        {
            this.SetPropertyName(path.Split('|').Last());
        }

        public void SetValue(JObject target, JToken value)
        {
            var obj = this.GetTargetObject(target);
            obj.Add(this.PropertyPaths.Last(), value);
        }
    }
    public class ColumnToken
    {
        public string ColumnName { get; private set; }
        public bool IsKey { get; private set; }
        public bool IsVirtual { get; private set; }
        public string ResultPath { get; private set; }
        public PropertyToken BuildToken { get; private set; }
        public ColumnToken(string path)
        {
            this.ColumnName = path;
            this.IsKey = this.ColumnName.EndsWith("#");
            this.IsVirtual = this.ColumnName.Contains("?");
            BuildToken = new PropertyToken(path);
            var paths = path.Split('|');
            if (paths.Length == 1)
                ResultPath = "";
            else
                ResultPath = string.Join("|", paths.Take(paths.Length - 1)) + "|";
        }
    }
    public class CollectionToken : Token
    {
        public string ResultPath { get; private set; }
        public string CollectionPath { get; private set; }
        public string NextPath { get; private set; }
        public PropertyToken[] KeyMaps { get; private set; }
        public CollectionToken NextToken { get; private set; }
        public bool IsLeaf { get; private set; }
        public CollectionToken(string resultPath, string path)
        {
            this.ResultPath = resultPath;
            CollectionPath = path.Substring(0, path.IndexOf("|"));
            this.KeyMaps = CollectionPath.Substring(1, CollectionPath.LastIndexOf("$") - 1).Split(',').Select(kp => new PropertyToken(kp)).ToArray();
            this.SetPropertyName(CollectionPath.Remove(0, CollectionPath.LastIndexOf("$") + 1));
            this.NextPath = path.Remove(0, CollectionPath.Length + 1);
            if (NextPath.Contains("$"))
            {
                NextToken = new CollectionToken(resultPath, NextPath);
                IsLeaf = false;
            }
            else
                IsLeaf = true;
        }
        public bool IsKeyMap(JObject source, JObject target)
        {
            return this.KeyMaps.All(km => km.GetValue(source).ToString() == km.GetValue(target).ToString());
        }

        public void SetCollection(IEnumerable<JObject> targets, IEnumerable<KeyValuePair<JObject, JObject>> values)
        {
            var objTargets = targets.Select(t => new { Key = GetKey(t), Value = GetTargetObject(t), ParentValue = t }).Select(
                t => new
                {
                    Value = t.Value,
                    ParentValue = t.ParentValue,
                    TargetValues = GetArray(t.Value),
                    ChildValues = values.Where(v => IsKeyMap(v.Key, t.Key))
                });
            foreach(var targetArray in objTargets)
            {
                if (targetArray.ChildValues.Count() == 0)
                {
                    NullEmptyObjects(targetArray.ParentValue);
                    continue;
                }
                if(IsLeaf)
                {
                    JArray arry = targetArray.TargetValues;
                    if(arry == null)
                    {
                        arry = new JArray();
                        targetArray.Value.Add(this.PropertyPaths.Last(), arry);
                    }
                    foreach(var val in targetArray.ChildValues)
                    {
                        arry.Add(val.Value);
                    }
                }
                else
                    NextToken.SetCollection(targetArray.TargetValues.Cast<JObject>(), values);
            }
            
            
        }
        private JArray GetArray(JObject target)
        {
            var prop = target.Property(this.PropertyPaths.Last());
            if (prop == null)
                return null;
            return prop.Value as JArray;
        }
    }
    public abstract class ResultTokens
    {
        protected Dictionary<string, List<ColumnToken>> KeyColumnDic { get; private set; }
        protected Dictionary<string, List<ColumnToken>> ResultColumnDic { get; private set; }
        public int ResultIndex { get; private set; }
        public IEnumerable<ColumnToken> GetKeys(string path = "")
        {
            if (path == "")
                return KeyColumnDic[""];
            return KeyColumnDic.Where(kv => path.Contains(kv.Key)|| kv.Key == "").SelectMany(v => v.Value);
        }
        public IEnumerable<ColumnToken> GetColumns(string path = "")
        {
            return ResultColumnDic[path];
        }
        public virtual IEnumerable<string> GetPaths()
        {
            return ResultColumnDic.Keys;
        }
        public ResultTokens(int resultIndex)
        {
            this.KeyColumnDic = new Dictionary<string, List<ColumnToken>>();
            this.KeyColumnDic.Add("", new List<ColumnToken>());
            this.ResultColumnDic = new Dictionary<string, List<ColumnToken>>();
            this.ResultIndex = resultIndex;
        }
        public override bool Equals(object obj)
        {
            ResultTokens tokens = obj as ResultTokens;
            if (tokens == null)
                return false;
            return tokens.ResultIndex == this.ResultIndex;
        }
        public override int GetHashCode()
        {
            return this.ResultIndex.GetHashCode();
        }
        protected void AddColumns(IEnumerable<ColumnToken> keys, IEnumerable<ColumnToken> values, string path = "")
        {
            List<ColumnToken> keyValues;
            if(!KeyColumnDic.TryGetValue(path, out keyValues))
            {
                keyValues = new List<ColumnToken>();
                KeyColumnDic[path] = keyValues;
            }
            List<ColumnToken> resultValues;
            if(!ResultColumnDic.TryGetValue(path, out resultValues))
            {
                resultValues = new List<ColumnToken>();
                ResultColumnDic[path] = resultValues;
            }
            keyValues.AddRange(keys);
            resultValues.AddRange(values);
        }
        public virtual IEnumerable<ColumnToken> GetColumnsForAllPaths()
        {
            return this.ResultColumnDic.Values.SelectMany(c => c);
        }
    }
    public class ParentResultTokens : ResultTokens
    {
        public ParentResultTokens(IEnumerable<string> columns) : base(0)
        {
            var columnTokens = columns.Select(c => new ColumnToken(c));
            this.AddColumns(columnTokens.Where(c => c.IsKey), columnTokens);
        }
    }
    public class ChildResultTokens : ResultTokens
    {
        private Dictionary<string, CollectionToken> Collections { get; set; }
        public CollectionToken GetCollectionToken(string path)
        {
            return Collections[path];
        }
        public ChildResultTokens(int level, IEnumerable<string> columns) : base(level)
        {
            this.Collections = new Dictionary<string, CollectionToken>();
            var columnTokens = columns.Select(c => new ColumnToken(c));
            foreach(var pathGroup in columnTokens.GroupBy(c => c.ResultPath))
            {
                AddColumns(pathGroup.Where(c => c.IsKey), pathGroup, pathGroup.Key);
                if (!string.IsNullOrEmpty(pathGroup.Key))
                    Collections.Add(pathGroup.Key, new CollectionToken(pathGroup.Key, pathGroup.Key));
            }
        }
        public override IEnumerable<ColumnToken> GetColumnsForAllPaths()
        {
            return this.ResultColumnDic.Where(kv => kv.Key != "").SelectMany(kv => kv.Value);
        }
        public override IEnumerable<string> GetPaths()
        {
            return this.KeyColumnDic.Keys.Where(k => k != "");
        }
        public IEnumerable<CollectionToken> GetCollectionTokens()
        {
            return this.Collections.Values;
        }
    }
}
