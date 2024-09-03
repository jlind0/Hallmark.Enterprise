using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Threading;
using System.Data.Common;
using HallData.Utilities;

namespace HallData.Data
{
	/// <summary>
	/// A SQL Server Database Helper class for a given Database Name, this should be injected into repositories
	/// </summary>
	public abstract class Database
	{
		/// <summary>
		/// Constructs a <see cref="Database"/> for a database name
		/// </summary>
		/// <param name="databaseName"><see cref="Database.DatabaseName"/></param>
		public Database(string databaseName)
		{
			this.DatabaseName = databaseName;
		}
		protected string ConnectionString { get { return ConfigurationManager.ConnectionStrings[this.DatabaseName].ConnectionString; } }
        protected abstract DbCommand CreateDbCommand(string command);
		/// <summary>
		/// The database name
		/// </summary>
		public string DatabaseName
		{
			get;
			private set;
		}
		/// <summary>
		/// Creates and adds a <see cref="DbParameter"/> to a <see cref="DbCommand"/>
		/// </summary>
		/// <param name="command">Target command</param>
		/// <param name="name">Name of the parameter</param>
		/// <param name="dbType">Type of the parameter</param>
		/// <param name="value">Value of the parameter</param>
		/// <param name="direction">Direction of the parameter</param>
		/// <param name="size">Size of the parameter</param>
		/// <param name="scale">Scale of the parameter</param>
		/// <param name="sourceColumn">Source column for the parameter</param>
		/// <param name="sourceVersion">Source version of the parameter</param>
		/// <returns>The created SQL Parameter</returns>
		public virtual DbParameter AddParameter(DbCommand command, string name, DbType dbType, object value, ParameterDirection direction = ParameterDirection.Input, int? size = null, string sourceColumn = null, DataRowVersion sourceVersion = DataRowVersion.Current)
		{
			return command.AddParameter(name, dbType, value, direction, size, sourceColumn, sourceVersion);
		}
		/// <summary>
		/// Creates and adds a <see cref="DbParameter"/> to a <see cref="DbCommand"/>
		/// </summary>
		/// <param name="command">Target command</param>
		/// <param name="name">Name of the parameter</param>
		/// <param name="value">Value of the parameter</param>
		/// <param name="direction">Direction of the parameter</param>
		/// <param name="size">Size of the parameter</param>
		/// <param name="scale">Scale of the parameter</param>
		/// <param name="sourceColumn">Source column for the parameter</param>
		/// <param name="sourceVersion">Source version of the parameter</param>
		/// <returns>The created SQL Parameter</returns>
		public virtual DbParameter AddParameter(DbCommand command, string name, object value, ParameterDirection direction = ParameterDirection.Input, int? size = null, string sourceColumn = null, DataRowVersion sourceVersion = DataRowVersion.Current)
		{
			return command.AddParameter(name, value, direction, size, sourceColumn, sourceVersion);
		}
        public abstract DbParameter AddTableParameter(DbCommand cmd, string name, string typeName, DataTable value);
		/// <summary>
		/// Creates and adds an ouput <see cref="DbParameter"/> to a <see cref="DbCommand"/>
		/// </summary>
		/// <param name="command">Target command</param>
		/// <param name="name">Name of the parameter</param>
		/// <param name="dbType">Type of the parameter</param>
		/// <param name="size">Size of the parameter</param>
		/// <param name="scale">Scale of the parameter</param>
		/// <param name="sourceColumn">Source column for the parameter</param>
		/// <param name="sourceVersion">Source version of the parmater</param>
		/// <returns>The created SQL Parameter</returns>
		public virtual DbParameter AddOutputParameter(DbCommand command, string name, DbType dbType, int? size = null, string sourceColumn = null, DataRowVersion sourceVersion = DataRowVersion.Current)
		{
			return command.AddOutputParameter(name, dbType, size, sourceColumn, sourceVersion);
		}
		/// <summary>
		/// Creates and adds an ouput <see cref="DbParameter"/> to a <see cref="DbCommand"/>
		/// </summary>
		/// <param name="command">Target command</param>
		/// <param name="name">Name of the parameter</param>
		/// <param name="size">Size of the parameter</param>
		/// <param name="scale">Scale of the Parameter</param>
		/// <param name="sourceColumn">Source column for the parameter</param>
		/// <param name="sourceVersion">Source version for the parameter</param>
		/// <returns>The created SQL Parameter</returns>
		public virtual DbParameter AddOutputParameter(DbCommand command, string name, int size, string sourceColumn = null, DataRowVersion sourceVersion = DataRowVersion.Current)
		{
			return command.AddOutputParameter(name, size, sourceColumn, sourceVersion);
		}
		/// <summary>
		/// Creates a stored procedure command
		/// </summary>
		/// <param name="name">name of the stored procedure</param>
		/// <returns>The created command</returns>
		public virtual DbCommand CreateStoredProcCommand(string name)
		{
            DbCommand cmd = CreateDbCommand(name);
			cmd.CommandType = CommandType.StoredProcedure;
			return cmd;
		}
		/// <summary>
		/// Creates a query command
		/// </summary>
		/// <param name="query">the query</param>
		/// <returns>The created command</returns>
		public virtual DbCommand CreateSqlQueryCommand(string query)
		{
            DbCommand cmd = CreateDbCommand(query);
			cmd.CommandType = CommandType.Text;
			return cmd;
		}
        protected abstract Task<DbConnection> OpenConnection(CancellationToken token = default(CancellationToken));
        protected abstract DbConnection OpenConnectionSync();
		/// <summary>
		/// Executes a reader asynchronously
		/// </summary>
		/// <param name="cmd">The target command</param>
		/// <param name="token">cancellation token</param>
		/// <param name="readers">readers, in order of the result set</param>
		/// <returns>Task representing the execute work</returns>
		/// <exception cref="ArgumentException">readers cannot be null or empty</exception>
		/// <example>
		/// <code>
		/// public async Task&lt;IEnumerable&lt;Foo&gt;&gt; ReadFoos(CancellationToken token = default(CancellationToken))
		/// {
		///     var cmd = this.Database.CreateStoredProcCommand("dbo.usp_select_foo");
		///     List&lt;Foo&gt; foos = new List&lt;Foo&gt;();
		///     await this.Database.ExecuteReaderAsync(cmd, token, dr =>
		///     {
		///         Foo f = new Foo();
		///         f.Name = dr["Name"] as string;
		///         foos.Add(f);
		///     });
		///     return foos;
		/// }
		/// </code>
		/// </example>
		public virtual async Task ExecuteReaderAsync(DbCommand cmd, CancellationToken token = default(CancellationToken), params Action<DbDataReader>[] readers)
		{
			if (readers == null || readers.Length == 0)
				throw new ArgumentException("readers cannot be null or empty");
			using(var conn = await OpenConnection(token))
			{
				cmd.Connection = conn;
				using(var reader = await cmd.ExecuteReaderAsync(token))
				{
					int resultSet = 0;
					while(await reader.ReadAsync(token))
					{
						readers[resultSet](reader);
					}
					resultSet++;
					while(resultSet < readers.Length && await reader.NextResultAsync(token))
					{
						while(await reader.ReadAsync(token))
						{
							readers[resultSet](reader);
						}
						resultSet++;
					}
				}
			}
		}
        public abstract DbParameter AddXmlParameter(DbCommand cmd, string name, object value, Type type);
		/// <summary>
		/// Executes a reader syncronously
		/// </summary>
		/// <param name="cmd">Command to execute</param>
		/// <param name="readers">readers, in order of the result set</param>
		/// <exception cref="ArgumentException">readers cannot be null or empty</exception>
		/// <example>
		/// <code>
		/// public IEnumerable&lt;Foo&gt; ReadFoos()
		/// {
		///     var cmd = this.Database.CreateStoredProcCommand("dbo.usp_select_foo");
		///     List&lt;Foo&gt; foos = new List&lt;Foo&gt;();
		///     this.Database.ExecuteReader(cmd, dr =>
		///     {
		///         Foo f = new Foo();
		///         f.Name = dr["Name"] as string;
		///         foos.Add(f);
		///     });
		///     return foos;
		/// }
		/// </code>
		/// </example>
		public virtual void ExecuteReader(DbCommand cmd, params Action<DbDataReader>[] readers)
		{
			if (readers == null || readers.Length == 0)
				throw new ArgumentException("readers cannot be null or empty");
			using (var conn = OpenConnectionSync())
			{
				cmd.Connection = conn;
				using (var reader = cmd.ExecuteReader())
				{
					int resultSet = 0;
					while (reader.Read())
					{
						readers[resultSet](reader);
					}
					resultSet++;
					while (resultSet < readers.Length && reader.NextResult())
					{
						while (reader.Read())
						{
							readers[resultSet](reader);
						}
						resultSet++;
					}
				}
			}
		}
		/// <summary>
		/// Executes a reader asynchronously, passing in the available columns and result order index to the delegates
		/// </summary>
		/// <param name="cmd">The command to execute</param>
		/// <param name="readerAction">The action for the first reader, taken in a collection of available columns</param>
		/// <param name="nextResultReaderAction">The action for child readers, takes in a collection of available columns and the result set index</param>
		/// <param name="token">The cancellation token</param>
		/// <returns>A Task represeting ExecuteReader's work</returns>
		/// <example>
		/// <code>
		/// public async Task&lt;IEnumerable&lt;Foo&gt;&gt; ReadFoos(CancellationToken token = default(CancellationToken))
		/// {
		///     var cmd = this.Database.CreateStoredProcedureCommand("dbo.usp_select_foos");
		///     Dictionary&lt;int, Foo&gt; foos = new Dictionary&lt;int, Foo&gt;();
		///     await this.Database.ExecuteReader(cmd, (dr, availableColumns) =>{
		///         Foo f = new Foo();
		///         f.Id = (int)dr["Id"];
		///         f.Name (string)dr["Name"];
		///         foos.Add(f.Id, f);
		///     }, (dr, availableColumns, resultSet) =>{
		///         Bar b = new Bar();
		///         b.Id = (int)dr["Id"];
		///         b.FooId = (int)dr["FooId"];
		///         b.Name = (string)dr["Name"];
		///         foos[b.FooId].Bars.Add(b);
		///     }, token);
		///     return foos.Values;
		/// }
		/// </code>
		/// </example>
		public virtual async Task ExecuteReader(DbCommand cmd, Action<DbDataReader, IEnumerable<string>> readerAction, Action<DbDataReader, IEnumerable<string>, int> nextResultReaderAction = null, CancellationToken token = default(CancellationToken))
		{
			using(var conn = await OpenConnection(token))
			{
				cmd.Connection = conn;
				using(var reader = await cmd.ExecuteReaderAsync(token))
				{
					List<string> properties = null;
					while(await reader.ReadAsync(token))
					{
						reader.PopulateColumns(ref properties);
						readerAction(reader, properties);
					}

					if(nextResultReaderAction != null)
					{
						int level = 1;
						while(await reader.NextResultAsync(token))
						{
							List<string> childPropertes = null;
							while (await reader.ReadAsync(token))
							{
								reader.PopulateColumns(ref childPropertes);
								nextResultReaderAction(reader, childPropertes, level);
							}
							level++;
						}
					}
				}
			}
		}
		/// <summary>
		/// Executes a Non-Query asynchronously
		/// </summary>
		/// <param name="cmd">Command to execute</param>
		/// <param name="token">Cancellation Token</param>
		/// <returns>Task representing ExecuteNonQueryAsync work</returns>
		/// <example>
		/// <code>
		/// public Task UpdateFoo(Foo foo, CancellationToken token = default(CancellationToken))
		/// {
		///     var cmd = this.Database.CreateStoredProcedureCommand("usp_update_foo");
		///     //TODO: map parameters
		///     return this.Database.ExecuteNonQueryAsync(cmd, token);
		/// }
		/// </code>
		/// </example>
		public virtual async Task ExecuteNonQueryAsync(DbCommand cmd, CancellationToken token = default(CancellationToken))
		{
			using(var conn = await OpenConnection(token))
			{
				cmd.Connection = conn;
				await cmd.ExecuteNonQueryAsync(token);
			}
		}
		/// <summary>
		/// Executes a Non-Query syncronously
		/// </summary>
		/// <param name="cmd">Command to execute</param>
		/// <example>
		/// <code>
		/// public void UpdateFoo(Foo foo)
		/// {
		///     var cmd = this.Database.CreateStoredProcedureCommand("usp_update_foo");
		///     //TODO: map parameters
		///     this.Database.ExecuteNonQuery(cmd);
		/// }
		/// </code>
		/// </example>
		public virtual void ExecuteNonQuery(DbCommand cmd)
		{
			using(var conn = OpenConnectionSync())
			{
				cmd.Connection = conn;
				cmd.ExecuteNonQuery();
			}
		}
		/// <summary>
		/// Executes a Scalar asynchronously
		/// </summary>
		/// <param name="cmd">Command to execute</param>
		/// <param name="token">Cancellation Token</param>
		/// <returns>A Task representing ExecuteScalarAsync work with a result of the scalar</returns>
		/// <example>
		/// <code>
		/// public async Task AddFoo(Foo foo, CancellationToken token = default(CancellationToken))
		/// {
		///     var cmd = this.Database.CreateStoredProcedureCommand("usp_insert_foo");
		///     //TODO: map parameters
		///     var newId = (await this.Database.ExecuteScalarAsync(cmd, token)) as int?;
		///     if(newId != null)
		///         foo.Id = newId.Value;
		/// }
		/// </code>
		/// </example>
		public virtual async Task<object> ExecuteScalarAsync(DbCommand cmd, CancellationToken token = default(CancellationToken))
		{
			using (var conn = await OpenConnection(token))
			{
				cmd.Connection = conn;
				return await cmd.ExecuteScalarAsync(token);
			}
		}
		/// <summary>
		/// Executes a Scalar syncronously
		/// </summary>
		/// <param name="cmd">Command to execute</param>
		/// <returns>The scalar object</returns>
		/// <example>
		/// <code>
		/// public void AddFoo(Foo foo)
		/// {
		///     var cmd = this.Database.CreateStoredProcedureCommand("usp_insert_foo");
		///     //TODO: map parameters
		///     var newId = this.Database.ExecuteScalar(cmd) as int?;
		///     if(newId != null)
		///         foo.Id = newId.Value;
		/// }
		/// </code>
		/// </example>
		public virtual object ExecuteScalar(DbCommand cmd)
		{
			using (var conn = OpenConnectionSync())
			{
				cmd.Connection = conn;
				return cmd.ExecuteScalar();
			}
		}

		private static readonly Lazy<ConcurrentDictionary<string, TableTypeColumn[]>> TableTypeColumnMappingLazy = 
			new Lazy<ConcurrentDictionary<string, TableTypeColumn[]>>(() => new ConcurrentDictionary<string, TableTypeColumn[]>(), LazyThreadSafetyMode.PublicationOnly);

		private static ConcurrentDictionary<string, TableTypeColumn[]> TableColumnMapping
		{
			get { return TableTypeColumnMappingLazy.Value; }
		}

		internal TableTypeColumn[] ReadTableTypeColumns(string tableTypeName)
		{
			TableTypeColumn[] columns;

			if (!TableColumnMapping.TryGetValue(tableTypeName, out columns))
			{
				var cmd = this.CreateStoredProcCommand("udt.usp_select_tabletype_definition");
				string[] schemaTableArray = tableTypeName.Split('.');

				cmd.AddParameter("tablename", schemaTableArray.Length == 1 ? schemaTableArray[0] : schemaTableArray[1]);
				if (schemaTableArray.Length > 1)
				{
					cmd.AddParameter("schema", schemaTableArray[0]);
				}
				
				List<TableTypeColumn> tableTypeColumns = new List<TableTypeColumn>();
				this.ExecuteReader(cmd, dr =>
				{
					TableTypeColumn tableTypeColumn = new TableTypeColumn();
					tableTypeColumn.ColumnName = dr["name"] as string;
					tableTypeColumn.OrderIndex = (int)dr["orderindex"];
					tableTypeColumns.Add(tableTypeColumn);
				});
				columns = tableTypeColumns.ToArray();
				TableColumnMapping.TryAdd(tableTypeName, columns);
			}

			return columns;
		}
	}
    public class SqlDatabase : Database
    {
        /// <summary>
		/// Constructs a <see cref="Database"/> for a database name
		/// </summary>
		/// <param name="databaseName"><see cref="Database.DatabaseName"/></param>
        public SqlDatabase(string databaseName) : base(databaseName) { }
        protected override async Task<DbConnection> OpenConnection(CancellationToken token = default(CancellationToken))
        {
            SqlConnection con = new SqlConnection(this.ConnectionString);
            await con.OpenAsync(token);
            return con;
        }

        protected override DbConnection OpenConnectionSync()
        {
            SqlConnection con = new SqlConnection(this.ConnectionString);
            con.Open();
            return con;
        }
        protected override DbCommand CreateDbCommand(string command)
        {
            return new SqlCommand(command);
        }
        public override DbParameter AddTableParameter(DbCommand cmd, string name, string typeName, DataTable value)
        {
            var parm = (SqlParameter)cmd.CreateParameter();
            parm.ParameterName = name;
            parm.SqlDbType = SqlDbType.Structured;
            parm.Value = value;
            parm.TypeName = typeName;
            cmd.Parameters.Add(parm);
            return parm;
        }

        public override DbParameter AddXmlParameter(DbCommand cmd, string name, object value, Type type)
        {
            var xml = value.ConvertToSqlXml(type);
            var parm = (SqlParameter)AddParameter(cmd, name, xml);
            parm.SqlDbType = SqlDbType.Xml;
            return parm;
        }
    }
	/// <summary>
	/// Factory class for creating <see cref="Database"/>
	/// </summary>
	public static class DatabaseFactory
	{
		/// <summary>
		/// Creates a database for a database name
		/// </summary>
		/// <param name="databaseName">Database Name</param>
		/// <returns>Constructed Database</returns>
		public static Database CreateDatabase(string databaseName)
		{
            return new SqlDatabase(databaseName);
		}
	}
}
