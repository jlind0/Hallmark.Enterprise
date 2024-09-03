using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data.Common;
using HallData.Utilities;

namespace HallData.Data.Mocks
{
	public class MockDatabase : Database
	{
		private Func<string, DbCommand> IDbCommandFactory { get; set; }

		public MockDatabase(Func<string, DbCommand> IDbCommandFactory) : base("test")
		{
			this.IDbCommandFactory = IDbCommandFactory;
		}

		protected override Task<DbConnection> OpenConnection(CancellationToken token = default(CancellationToken))
		{
			return Task.FromResult<DbConnection>(new SqlConnection());
		}

		protected override DbConnection OpenConnectionSync()
		{
			return new SqlConnection();
		}

		protected override DbCommand CreateDbCommand(string command)
		{
			return this.IDbCommandFactory(command);
		}

		public override DbParameter AddTableParameter(DbCommand cmd, string name, string typeName, System.Data.DataTable value)
		{
			var parm = cmd.CreateParameter();
			parm.DbType = System.Data.DbType.Object;
			parm.ParameterName = name;
			parm.Value = value;
			cmd.Parameters.Add(parm);
			return parm;
		}

        public override DbParameter AddXmlParameter(DbCommand cmd, string name, object value, Type type)
        {
            var parm = AddParameter(cmd, name, value.Serialize(type));
            parm.DbType = System.Data.DbType.Xml;
            return parm;
        }
    }
}
