using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using System.Threading;

namespace HallData.Data.Mocks
{
    public class MockDbCommand : DbCommand
    {
        private MockDbParameterCollection parameters = new MockDbParameterCollection();
        public MockDbCommand(Func<DbParameter> parameterFactory, Func<DbCommand, System.Data.CommandBehavior, DbDataReader> dataReaderFactory = null, Func<DbCommand, int> nonQueryFactory = null, Func<DbCommand, object> scalarFactory = null)
        {
            this.DataReaderFactory = dataReaderFactory;
            this.ExecuteNonQueryFactory = nonQueryFactory;
            this.ExecuteScalarFactory = scalarFactory;
            this.ParameterFactory = parameterFactory;
            parameters = new MockDbParameterCollection();
        }
        public override void Cancel()
        {
            
        }

        public override string CommandText
        {
            get;
            set;
        }

        public override int CommandTimeout
        {
            get;
            set;
        }

        public override System.Data.CommandType CommandType
        {
            get;
            set;
        }
        private Func<DbParameter> ParameterFactory { get; set; }
        protected override DbParameter CreateDbParameter()
        {
            return ParameterFactory();
        }

        protected override DbConnection DbConnection
        {
            get;
            set;
        }

        protected override DbParameterCollection DbParameterCollection
        {
            get { return this.parameters; }
        }

        protected override DbTransaction DbTransaction
        {
            get;
            set;
        }

        public override bool DesignTimeVisible
        {
            get;
            set;
        }
        private Func<DbCommand, System.Data.CommandBehavior, DbDataReader> DataReaderFactory { get; set; }
        protected override DbDataReader ExecuteDbDataReader(System.Data.CommandBehavior behavior)
        {
            return this.DataReaderFactory(this, behavior);
        }
        private Func<DbCommand, int> ExecuteNonQueryFactory { get; set; }
        public override int ExecuteNonQuery()
        {
            return this.ExecuteNonQueryFactory(this);
        }
        private Func<DbCommand, object> ExecuteScalarFactory { get; set; }
        public override object ExecuteScalar()
        {
            return this.ExecuteScalarFactory(this);
        }

        public override void Prepare()
        {
            
        }

        public override System.Data.UpdateRowSource UpdatedRowSource
        {
            get;
            set;
        }
    }
}
