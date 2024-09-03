using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using HallData.ApplicationViews;
using System.Reflection;
using System.Collections;
using HallData.Utilities;
using System.Data.Common;

namespace HallData.Data
{
	/// <summary>
	/// <see cref="DbCommand"/> extensions
	/// </summary>
	public static class IDbCommandExtensions
	{
		/// <summary>
		/// Creates and adds a <see cref="DbParameter"/> to a <see cref="DbCommand"/>
		/// </summary>
		/// <param name="command">Target command</param>
		/// <param name="name">Name of parameter</param>
		/// <param name="dbType">Type of parameter</param>
		/// <param name="value">Value for parameter</param>
		/// <param name="direction">Direction of parameter</param>
		/// <param name="size">Size of parameter</param>
		/// <param name="scale">Scale of parameter</param>
		/// <param name="sourceColumn">Source column of parameter</param>
		/// <param name="sourceVersion">Source version of parameter</param>
		/// <returns>Created DbParameter</returns>
		public static DbParameter AddParameter(this DbCommand command, string name, DbType dbType, object value, ParameterDirection direction = ParameterDirection.Input, int? size = null, string sourceColumn = null, DataRowVersion sourceVersion = DataRowVersion.Current)
		{
			DbParameter parm = command.CreateParameter();
			ConfigureParameter(parm, name, value, dbType, direction, size, sourceColumn, sourceVersion);
			command.Parameters.Add(parm);
			return parm;
		}
		/// <summary>
		/// Creates and adds a <see cref="DbParameter"/> to a <see cref="DbCommand"/>
		/// </summary>
		/// <param name="command">Target command</param>
		/// <param name="name">Name of parameter</param>
		/// <param name="value">Value for parameter</param>
		/// <param name="direction">Direction of parameter</param>
		/// <param name="size">Size of parameter</param>
		/// <param name="scale">Scale of parameter</param>
		/// <param name="sourceColumn">Source column of parameter</param>
		/// <param name="sourceVersion">Source version of parameter</param>
		/// <returns>Created DbParameter</returns>
		public static DbParameter AddParameter(this DbCommand command, string name, object value, ParameterDirection direction = ParameterDirection.Input, int? size = null, string sourceColumn = null, DataRowVersion sourceVersion = DataRowVersion.Current)
		{
			DbParameter parm = command.CreateParameter();
			ConfigureParameter(parm, name, value, null, direction, size, sourceColumn, sourceVersion);
			command.Parameters.Add(parm);
			return parm;
		}
		/// <summary>
		/// Creates and adds an output <see cref="DbParameter"/> to a <see cref="DbCommand"/>
		/// </summary>
		/// <param name="command">Target command</param>
		/// <param name="name">Name of parameter</param>
		/// <param name="dbType">Type of parameter</param>
		/// <param name="size">Size of parameter</param>
		/// <param name="scale">Scale of parameter</param>
		/// <param name="sourceColumn">Source column of parameter</param>
		/// <param name="sourceVersion">Source version of parameter</param>
		/// <returns>Created DbParameter</returns>
		public static DbParameter AddOutputParameter(this DbCommand command, string name, DbType dbType, int? size = null, string sourceColumn = null, DataRowVersion sourceVersion = DataRowVersion.Current)
		{
			DbParameter parm = command.CreateParameter();
			ConfigureParameter(parm, name, null, dbType, ParameterDirection.Output, size, sourceColumn, sourceVersion);
			command.Parameters.Add(parm);
			return parm;
		}
		/// <summary>
		/// Creates and adds an output <see cref="DbParameter"/> to a <see cref="DbCommand"/>
		/// </summary>
		/// <param name="command">Target command</param>
		/// <param name="name">Name of parameter</param>
		/// <param name="size">Size of parameter</param>
		/// <param name="scale">Scale of parameter</param>
		/// <param name="sourceColumn">Source column of parameter</param>
		/// <param name="sourceVersion">Source version of parameter</param>
		/// <returns>Created DbParameter</returns>
		public static DbParameter AddOutputParameter(this DbCommand command, string name, int size, string sourceColumn = null, DataRowVersion sourceVersion = DataRowVersion.Current)
		{
			DbParameter parm = command.CreateParameter();
			ConfigureParameter(parm, name, null, null, ParameterDirection.Output, size, sourceColumn, sourceVersion);
			command.Parameters.Add(parm);
			return parm;
		}
		private static void ConfigureParameter(DbParameter parameter, string name, object value, DbType? dbType = null, ParameterDirection direction = ParameterDirection.Input, int? size = null, string sourceColumn = null, DataRowVersion version = DataRowVersion.Current)
		{
			parameter.ParameterName = name;
			parameter.Value = value;
			parameter.Direction = direction;
			if (dbType != null)
				parameter.DbType = dbType.Value;
			if (size != null)
				parameter.Size = size.Value;
			if (sourceColumn != null)
				parameter.SourceColumn = sourceColumn;
			parameter.SourceVersion = version;
		}
		/// <summary>
		/// Maps parameters for an object to a <see cref="DbCommand"/>
		/// </summary>
		/// <typeparam name="T">Type of object</typeparam>
		/// <param name="cmd">Target command</param>
		/// <param name="obj">Source object</param>
		/// <param name="operation">Operation</param>
		/// <param name="path">The path of the command, only used internally</param>
		/// <param name="type">The type of the object, defaults to typeof(T), only used internally</param>
		public static void MapParameters<T>(this DbCommand cmd, T obj, Database db, ViewOperations operation, bool populateNullOrEmpty = false, string path = "", Type type = null, List<PropertyInfo> processedPropertyChildren = null)
		{
			if (type == null)
				type = typeof(T);
			if (processedPropertyChildren == null)
				processedPropertyChildren = new List<PropertyInfo>();
			var properties = type.GetPropertiesCached();
			string cmdPath = path.Replace(".", "_");
			foreach (var prop in properties.Where(p => !processedPropertyChildren.Contains(p)))
			{
				
				var operations = prop.GetCustomAttributesCached<ViewOperationParameterAttribute>(true);
				if (operations != null && operations.Any(o => o.Operation == operation || (o.Operation == ViewOperations.ChildKey && !string.IsNullOrEmpty(path))))
				{
					var val = obj == null ? null : prop.GetValue(obj);
					if (val != null)
					{
						var childView = prop.GetCustomAttributeCached<ChildViewAttribute>(true);
						if (childView != null)
						{
							var newProcessedChildren = processedPropertyChildren.ToList();
							newProcessedChildren.Add(prop);
							MapParameters(cmd, val, db, operation, populateNullOrEmpty, string.Format("{0}{1}.", path, prop.Name), prop.PropertyType, newProcessedChildren);
						}
						else
						{
							var childCollection = prop.GetCustomAttributeCached<ChildViewCollectionAttribute>(true);

							if (childCollection != null)
							{
                                string parmName = string.Format("{0}{1}", cmdPath, prop.Name);
                                if (childCollection.ParameterType == CollectionParameterType.DataTable)
                                {
                                    IEnumerable<DataTableMapping> mappings;
                                    var dt = MapTableParameters(prop.PropertyType, val as IEnumerable, operation, out mappings, childCollection.DataTableTypeName, db);
                                    if (dt.Rows.Count > 0 || populateNullOrEmpty)
                                        db.AddTableParameter(cmd, string.Format("{0}{1}", cmdPath, prop.Name), childCollection.DataTableTypeName, dt);
                                    cmd.MapDataMappings(mappings, parmName, populateNullOrEmpty, db);
                                    continue;
                                }
                                else
                                {
                                    db.AddXmlParameter(cmd, parmName, val, prop.PropertyType);
                                }
							}
							var mergeCollection = prop.GetCustomAttributeCached<ChildViewMergeAttribute>(true);
							if (mergeCollection != null)
							{
								IMerge<IMergable> merge = val as IMerge<IMergable>;
								if (merge != null)
								{
                                    if (merge.AddCollection.Count > 0)
									{
                                        var collectionType = prop.PropertyType.GetPropertyCached("Add").PropertyType;
                                        string parmName = string.Format("{0}{1}_Add", cmdPath, prop.Name);
                                        if (mergeCollection.ParameterType == CollectionParameterType.DataTable && !string.IsNullOrWhiteSpace(mergeCollection.AddDataTableTypeName))
                                        {
                                            IEnumerable<DataTableMapping> mappings;
                                            var addTable = MapTableParameters(collectionType, merge.AddCollection, ViewOperations.Add, out mappings, mergeCollection.AddDataTableTypeName, db);
                                            if (addTable.Rows.Count > 0)
                                            {
                                                db.AddTableParameter(cmd, parmName, mergeCollection.AddDataTableTypeName, addTable);
                                            }
                                            cmd.MapDataMappings(mappings, parmName, populateNullOrEmpty, db);
                                        }
                                        else if (mergeCollection.ParameterType == CollectionParameterType.Xml)
                                            db.AddXmlParameter(cmd, parmName, merge.AddCollection, collectionType);
									}
									if (merge.UpdateCollection.Count > 0)
									{
                                        string parmName = string.Format("{0}{1}_Update", cmdPath, prop.Name);
                                        var collectionType = prop.PropertyType.GetPropertyCached("Update").PropertyType;
                                        if (mergeCollection.ParameterType == CollectionParameterType.DataTable && !string.IsNullOrWhiteSpace(mergeCollection.UpdateDataTableTypeName))
                                        {
                                            IEnumerable<DataTableMapping> mappings;
                                            var updateTable = MapTableParameters(collectionType, merge.UpdateCollection, ViewOperations.Update, out mappings, mergeCollection.UpdateDataTableTypeName, db);
                                            if (updateTable.Rows.Count > 0)
                                            {
                                                db.AddTableParameter(cmd, parmName, mergeCollection.UpdateDataTableTypeName, updateTable);
                                            }
                                            cmd.MapDataMappings(mappings, parmName, populateNullOrEmpty, db);
                                        }
                                        else if (mergeCollection.ParameterType == CollectionParameterType.Xml)
                                            db.AddXmlParameter(cmd, parmName, merge.UpdateCollection, collectionType);
									}
									if (merge.HardDeleteCollection.Count > 0)
									{
                                        string parmName = string.Format("{0}{1}_HardDelete", cmdPath, prop.Name);
                                        var collectionType = prop.PropertyType.GetPropertyCached("HardDelete").PropertyType;
                                        if (mergeCollection.ParameterType == CollectionParameterType.DataTable && !string.IsNullOrWhiteSpace(mergeCollection.DeleteDataTableTypeName))
                                        {
                                            IEnumerable<DataTableMapping> mappings;
                                            var deleteTable = MapTableParameters(collectionType, merge.HardDeleteCollection, ViewOperations.ChildKey, out mappings, mergeCollection.DeleteDataTableTypeName, db);
                                            if (deleteTable.Rows.Count > 0)
                                            {
                                                db.AddTableParameter(cmd, parmName, mergeCollection.DeleteDataTableTypeName, deleteTable);
                                            }
                                            cmd.MapDataMappings(mappings, parmName, populateNullOrEmpty, db);
                                        }
                                        else if (mergeCollection.ParameterType == CollectionParameterType.Xml)
                                            db.AddXmlParameter(cmd, parmName, merge.HardDeleteCollection, collectionType);
									}
									if (merge.SoftDeleteCollection.Count > 0)
									{
                                        string parmName = string.Format("{0}{1}_SoftDelete", cmdPath, prop.Name);
                                        var collectionType = prop.PropertyType.GetPropertyCached("SoftDelete").PropertyType;
                                        if (mergeCollection.ParameterType == CollectionParameterType.DataTable && !string.IsNullOrWhiteSpace(mergeCollection.DeleteDataTableTypeName))
                                        {
                                            IEnumerable<DataTableMapping> mappings;
                                            var deleteTable = MapTableParameters(merge.SoftDeleteCollection.GetType(), merge.SoftDeleteCollection, ViewOperations.ChildKey, out mappings, mergeCollection.DeleteDataTableTypeName, db);
                                            if (deleteTable.Rows.Count > 0 || populateNullOrEmpty)
                                            {
                                                db.AddTableParameter(cmd, parmName, mergeCollection.DeleteDataTableTypeName, deleteTable);
                                            }
                                            cmd.MapDataMappings(mappings, parmName, populateNullOrEmpty, db);
                                        }
                                        else if(mergeCollection.ParameterType == CollectionParameterType.Xml)
                                            db.AddXmlParameter(cmd, parmName, merge.SoftDeleteCollection, collectionType);
									}
								}
								continue;
							}
							var relationshipMerge = prop.GetCustomAttributeCached<ChildViewRelationshipMergeAttribute>(true);
							if (relationshipMerge != null)
							{
								IRelationshipMerge<IRelationshipMergeable> merge = val as IRelationshipMerge<IRelationshipMergeable>;
								if (merge != null)
								{
									if (merge.AddCollection.Count > 0)
									{
                                        string parmName = string.Format("{0}{1}_Add", cmdPath, prop.Name);
                                        var collectionType = prop.PropertyType.GetPropertyCached("Add").PropertyType;
                                        if (relationshipMerge.ParameterType == CollectionParameterType.DataTable && !string.IsNullOrEmpty(relationshipMerge.DataTableTypeName))
                                        {
                                            IEnumerable<DataTableMapping> mappings;
                                            var addTable = MapTableParameters(collectionType, merge.AddCollection, ViewOperations.ChildKey, out mappings, relationshipMerge.DataTableTypeName, db);
                                            db.AddTableParameter(cmd, parmName, relationshipMerge.DataTableTypeName, addTable);
                                            cmd.MapDataMappings(mappings, parmName, populateNullOrEmpty, db);
                                        }
                                        else if (mergeCollection.ParameterType == CollectionParameterType.Xml)
                                            db.AddXmlParameter(cmd, parmName, merge.AddCollection, collectionType);
									}
									if (merge.RemoveCollection.Count > 0)
									{
                                        string parmName = string.Format("{0}{1}_Remove", cmdPath, prop.Name);
                                        var collectionType = prop.PropertyType.GetPropertyCached("Remove").PropertyType;
                                        if (relationshipMerge.ParameterType == CollectionParameterType.DataTable && !string.IsNullOrEmpty(relationshipMerge.DataTableTypeName))
                                        {
                                            IEnumerable<DataTableMapping> mappings;
                                            var removeTable = MapTableParameters(collectionType, merge.RemoveCollection, ViewOperations.ChildKey, out mappings, relationshipMerge.DataTableTypeName, db);
                                            db.AddTableParameter(cmd, parmName, relationshipMerge.DataTableTypeName, removeTable);
                                            cmd.MapDataMappings(mappings, parmName, populateNullOrEmpty, db);
                                        }
                                        else if (mergeCollection.ParameterType == CollectionParameterType.Xml)
                                            db.AddXmlParameter(cmd, parmName, merge.RemoveCollection, collectionType);
									}
								}
								continue;
							}
							if (val != null || populateNullOrEmpty)
								cmd.AddParameter(string.Format("{0}{1}", cmdPath, prop.Name), val);
						}
					}
				}
			}
		}

		private static void MapDataMappings(this DbCommand cmd, IEnumerable<DataTableMapping> mappings, string parmName, bool populteNullOrEmpty, Database db)
		{
			foreach (var mapping in mappings.Where(dt => populteNullOrEmpty || dt.Table.Rows.Count > 0))
			{
                db.AddTableParameter(cmd, string.Format("{0}_{1}", parmName, mapping.Path), mapping.DataTableTypeName, mapping.Table);
			}
		}

		private static DataTable MapTableParameters(Type collectionType, IEnumerable values, ViewOperations operation, out IEnumerable<DataTableMapping> mappings, string tableTypeName, Database db)
		{
			DataTable dt = new DataTable();
			Type t = collectionType.GetCollectionValueType();
			Dictionary<PropertyInfo, DataTableMapping> mappingDic = new Dictionary<PropertyInfo, DataTableMapping>();
			dt.MapTableParameters(values, t, operation, mappingDic, db);
			SetColumnsOrder(dt, db.ReadTableTypeColumns(tableTypeName).OrderBy(c => c.OrderIndex).Select(c => c.ColumnName).ToArray());
			mappings = mappingDic.Values;
			return dt;
		}

		private class DataTableMapping
		{
			public DataTableMapping(PropertyInfo property, string dataTableTypeName, string path)
			{
				Table = new DataTable();
				Properties = new Dictionary<string, List<PropertyInfo>>();
				this.DataTableTypeName = dataTableTypeName;
				this.Property = property;
				this.Path = path;
			}
			public DataTable Table { get; private set; }
			public Dictionary<string, List<PropertyInfo>> Properties { get; private set; }
			public string DataTableTypeName { get; private set; }
			public PropertyInfo Property { get; private set; }
			public string Path { get; private set; }
		}

		private static void MapTableParameters(this DataTable table, IEnumerable collection, Type elementType, ViewOperations operation, Dictionary<PropertyInfo, DataTableMapping> mappings, Database db)
		{
			Dictionary<string, List<PropertyInfo>> properties = new Dictionary<string, List<PropertyInfo>>();
			BuildColumns(properties, table, elementType, operation, mappings, db);
			table.PopulateTableParameters(collection, properties, mappings);
		}

		private static void PopulateTableParameters(this DataTable table, IEnumerable collection, Dictionary<string, List<PropertyInfo>> properties, 
			Dictionary<PropertyInfo, DataTableMapping> mappings, Guid? parentInstanceId = null)
		{
			if (collection != null)
			{
				foreach (object obj in collection)
				{
					table.PopulateTableRow(obj, properties, mappings, parentInstanceId);
				}
			}
		}

		public static void SetColumnsOrder(this DataTable table, params String[] columnNames)
		{
			int columnIndex = 0;
			foreach (var column in columnNames)
			{
				var c = table.Columns[columnNames[columnIndex]];
				if (c != null)
				{
					table.Columns[columnNames[columnIndex]].SetOrdinal(columnIndex);
					columnIndex++;
				}
			}
		}

		private static bool PopulateTableRow(this DataTable table, object obj, Dictionary<string, List<PropertyInfo>> properties, 
			Dictionary<PropertyInfo, DataTableMapping> mappings, Guid? parentInstanceId)
		{
			DataRow dr = table.NewRow();
			bool filled = false;
			IHasInstanceGuid instanceGuid = obj as IHasInstanceGuid;
			Guid? instanceId = instanceGuid != null ? instanceGuid.InstanceGuid : null as Guid?;
			if (instanceId != null)
				dr["__InstanceGuid"] = instanceId.Value;
			if (parentInstanceId != null)
				dr["__ParentInstanceGuid"] = parentInstanceId.Value;
			foreach (var propertyPath in properties)
			{
				var target = GetTargetObject(obj, propertyPath.Key);
				if (target != null)
					filled = MapTableObject(obj, properties, mappings, parentInstanceId, propertyPath.Key, propertyPath.Value, dr, instanceId) || filled;
			}

			if (filled)
				table.Rows.Add(dr);

			//SetColumnsOrder(table, new string[] { "__InstanceGuid", "__ParentInstanceGuid", "PartyGuid", "id", "roleId", "orderindex", "status.statustypeid", "startdate", "enddate", "isdefault" });

			return filled;
		}

		private static bool MapTableObject(object obj, Dictionary<string, List<PropertyInfo>> properties, 
			Dictionary<PropertyInfo, DataTableMapping> mappings, Guid? parentInstanceId, string path, List<PropertyInfo> propertiesToMap, DataRow dr, Guid? instanceId)
		{
			IHasInstanceGuid targetInstanceGuid = obj as IHasInstanceGuid;
			if (targetInstanceGuid != null)
				dr[string.Format("{0}__InstanceGuid", path)] = targetInstanceGuid.InstanceGuid;
			bool filled = false;
			foreach (var property in propertiesToMap)
			{
				var val = property.GetValue(obj);

				var col = val as IEnumerable;
				if (col != null)
				{
					var merge = val as IMerge<IMergable>;
					if (merge != null)
					{
						var mergeGeneric = property.PropertyType.GetGenericInterfaceCached(typeof(IMerge<,,,>));
						var addProperty = mergeGeneric.GetPropertyCached("Add");
						addProperty.MapChildTable(merge.AddCollection, mappings, instanceId);
						var updateProperty = mergeGeneric.GetPropertyCached("Update");
						updateProperty.MapChildTable(merge.UpdateCollection, mappings, instanceId);
						var hardDeleteProperty = mergeGeneric.GetPropertyCached("HardDelete");
						hardDeleteProperty.MapChildTable(merge.HardDeleteCollection, mappings, instanceId);
						var softDeleteProperty = mergeGeneric.GetPropertyCached("SoftDelete");
						softDeleteProperty.MapChildTable(merge.SoftDeleteCollection, mappings, instanceId);
						continue;
					}
					var mergeRelationship = col as IRelationshipMerge<IRelationshipMergeable>;
					if (mergeRelationship != null)
					{
						var mergeRelationshipGeneric = property.PropertyType.GetGenericInterfaceCached(typeof(IRelationshipMerge<,>));
						var addProperty = mergeRelationshipGeneric.GetPropertyCached("Add");
						addProperty.MapChildTable(mergeRelationship.AddCollection, mappings, instanceId);
						var removeProperty = mergeRelationshipGeneric.GetPropertyCached("Remove");
						removeProperty.MapChildTable(mergeRelationship.RemoveCollection, mappings, instanceId);
						continue;
					}
					property.MapChildTable(col, mappings, instanceId);
					continue;
				}

				if (val != null)
				{

					switch (val.GetType().FullName)
					{
						case "System.DateTime":
							if ((DateTime)val == DateTime.MinValue)
							{
								dr[string.Format("{0}{1}", path, property.Name)] = null;
							}
							else
							{
								dr[string.Format("{0}{1}", path, property.Name)] = (DateTime)val;
							}
							break;
						case "System.Boolean":
							short booleanBit = Convert.ToInt16(val);
							dr[string.Format("{0}{1}", path, property.Name)] = booleanBit;
							break;
						default:
							dr[string.Format("{0}{1}", path, property.Name)] = val;
							break;
					}
					filled = true;
				}
			}
			return filled;
		}
		private static void MapChildTable(this PropertyInfo property, IEnumerable col, Dictionary<PropertyInfo, DataTableMapping> mappings, Guid? parentGuid)
		{
			DataTableMapping dtm;
			if (mappings.TryGetValue(property, out dtm))
			{
				dtm.Table.PopulateTableParameters(col, dtm.Properties, mappings, parentGuid);
			}
		}
		private static object GetTargetObject(object obj, string path)
		{
			if (path == "")
				return obj;
			string[] paths = path.Split('.');
			object target = obj;
			foreach(var p in paths)
			{
				var prop = target.GetType().GetPropertyCached(p);
				if (prop == null)
					return null;
				var val = prop.GetValue(target);
				if (val == null)
					return null;
				target = val;
			}
			return target;
		}
		private static void BuildColumns(Dictionary<string, List<PropertyInfo>> properties, DataTable table, Type type, ViewOperations operation, 
			Dictionary<PropertyInfo, DataTableMapping> mappings, Database db, List<PropertyInfo> processedChildren = null, string path = "")
		{
			if(processedChildren == null)
				processedChildren = new List<PropertyInfo>();
			List<PropertyInfo> props;
			if (!properties.TryGetValue(path, out props))
			{
				props = new List<PropertyInfo>();
				properties[path] = props;
			}
			if (string.IsNullOrEmpty(path))
			{
				table.Columns.Add("__InstanceGuid");
				table.Columns.Add("__ParentInstanceGuid");
			}
			foreach (var prop in type.GetPropertiesCached().Where(p => !processedChildren.Contains(p)).OrderBy(p => p.Name))
			{
				var operations = prop.GetCustomAttributesCached<ViewOperationParameterAttribute>(true);
				if (operations != null && operations.Any())
				{
					if (prop.PropertyType.HasInterface(typeof(IHasInstanceGuid)))
						table.Columns.Add(string.Format("{0}{1}__InstanceGuid", path, prop.Name));

					var childCollection = prop.GetCustomAttributeCached<ChildViewCollectionAttribute>(true);
					string columnPath = string.Format("{0}{1}", path, prop.Name);
					if (childCollection != null)
					{
						var newList = new List<PropertyInfo>();
						newList.Add(prop);
						var childColMapping = prop.BuildDataTableMapping(mappings, childCollection.DataTableTypeName, operation, columnPath, string.Format("{0}{1}.", path, prop.Name), newList, db);
						if (childColMapping.Table.Columns.Count > 2)
							props.Add(prop);
					}
					var childMerge = prop.GetCustomAttributeCached<ChildViewMergeAttribute>(true);
					if (childMerge != null)
					{
						Type mergeType = prop.PropertyType.GetGenericInterfaceCached(typeof(IMerge<,,,>));
						if (mergeType != null)
						{
							var newList = new List<PropertyInfo>();
							newList.Add(prop);
							var addProperty = mergeType.GetPropertyCached("Add");
							addProperty.BuildDataTableMapping(mappings, childMerge.AddDataTableTypeName, ViewOperations.Add, string.Format("{0}_Add", columnPath), string.Format("{0}{1}_Add.", path, prop.Name), newList, db);
							newList = new List<PropertyInfo>();
							newList.Add(prop);
							var updateProperty = mergeType.GetPropertyCached("Update");
							updateProperty.BuildDataTableMapping(mappings, childMerge.UpdateDataTableTypeName, ViewOperations.Update, string.Format("{0}_Update", columnPath), string.Format("{0}{1}_Update.", path, prop.Name), newList, db);
							newList = new List<PropertyInfo>();
							newList.Add(prop);
							var hardDeleteProperty = mergeType.GetPropertyCached("HardDelete");
							hardDeleteProperty.BuildDataTableMapping(mappings, childMerge.DeleteDataTableTypeName, ViewOperations.ChildKey, string.Format("{0}_HardDelete", columnPath), string.Format("{0}{1}_HardDelete.", path, prop.Name), newList, db);
							newList = new List<PropertyInfo>();
							newList.Add(prop);
							var softDeleteProperty = mergeType.GetPropertyCached("SoftDelete");
							softDeleteProperty.BuildDataTableMapping(mappings, childMerge.DeleteDataTableTypeName, ViewOperations.ChildKey, string.Format("{0}_SoftDelete", columnPath), string.Format("{0}{1}_SoftDelete.", path, prop.Name), newList, db);
							props.Add(prop);
							continue;
						}
					}
					var childRelationshipMerge = prop.GetCustomAttributeCached<ChildViewRelationshipMergeAttribute>(true);
					if (childRelationshipMerge != null)
					{
						Type mergeType = prop.PropertyType.GetGenericInterfaceCached(typeof(IRelationshipMerge<,>));
						if (mergeType != null)
						{
							var newList = new List<PropertyInfo>();
							newList.Add(prop);
							var addProperty = mergeType.GetPropertyCached("Add");
							addProperty.BuildDataTableMapping(mappings, childRelationshipMerge.DataTableTypeName, ViewOperations.Add, string.Format("{0}_Add", columnPath), string.Format("{0}{1}_Add.", path, prop.Name), newList, db);
							newList = new List<PropertyInfo>();
							newList.Add(prop);
							var removeProperty = mergeType.GetPropertyCached("Remove");
							removeProperty.BuildDataTableMapping(mappings, childRelationshipMerge.DataTableTypeName, ViewOperations.Add, string.Format("{0}_Add", columnPath), string.Format("{0}{1}_Add.", path, prop.Name), newList, db);
							props.Add(prop);
							continue;
						}
					}
					var childView = prop.GetCustomAttributeCached<ChildViewAttribute>(true);
					if (childView != null)
					{
						var newList = processedChildren.ToList();
						newList.Add(prop);
						BuildColumns(properties, table, prop.PropertyType, operation, mappings, db, newList, string.Format("{0}{1}.", path, prop.Name));
						continue;
					}
					table.Columns.Add(string.Format("{0}{1}", path, prop.Name));
					props.Add(prop);
				}
			}
		}

		private static DataTableMapping BuildDataTableMapping(this PropertyInfo property, Dictionary<PropertyInfo, DataTableMapping> mappings, 
			string dataTableTypeName, ViewOperations operation, string columnPath, string path, List<PropertyInfo> processedChildren, Database db)
		{
			DataTableMapping childColMapping;
			if (!mappings.TryGetValue(property, out childColMapping))
			{
				childColMapping = new DataTableMapping(property, dataTableTypeName, columnPath);
				mappings[property] = childColMapping;
				BuildColumns(childColMapping.Properties, childColMapping.Table, property.PropertyType.GetCollectionValueType(), operation, mappings, db, processedChildren, string.Format("{0}{1}.", path, property.Name));
                SetColumnsOrder(childColMapping.Table, db.ReadTableTypeColumns(dataTableTypeName).OrderBy(c => c.OrderIndex).Select(c => c.ColumnName).ToArray());
			}
			return childColMapping;
		}
	}
}
