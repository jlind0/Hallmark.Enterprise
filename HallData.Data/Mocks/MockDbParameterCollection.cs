using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;

namespace HallData.Data.Mocks
{
    public class MockDbParameterCollection : DbParameterCollection
    {
        private readonly List<DbParameter> Parameters = new List<DbParameter>();
        public MockDbParameterCollection() { }
        public override int Add(object value)
        {
            this.Parameters.Add((DbParameter)value);
            return 1;
        }

        public override void AddRange(Array values)
        {
            foreach(var parm in values.OfType<DbParameter>())
            {
                Add(parm);
            }
        }

        public override void Clear()
        {
            this.Parameters.Clear();
        }

        public override bool Contains(string value)
        {
            return Parameters.Any(p => p.ParameterName == value);
        }

        public override bool Contains(object value)
        {
            return this.Parameters.Any(p => p.Value.Equals(value));
        }

        public override void CopyTo(Array array, int index)
        {
            throw new NotImplementedException(); 
        }

        public override int Count
        {
            get { return this.Parameters.Count; }
        }

        public override System.Collections.IEnumerator GetEnumerator()
        {
            return this.Parameters.GetEnumerator();
        }

        protected override DbParameter GetParameter(string parameterName)
        {
            return this.Parameters.SingleOrDefault(p => p.ParameterName == parameterName);
        }

        protected override DbParameter GetParameter(int index)
        {
            return this.Parameters[index];
        }

        public override int IndexOf(string parameterName)
        {
            return this.Parameters.IndexOf(this.GetParameter(parameterName));
        }

        public override int IndexOf(object value)
        {
            return this.Parameters.IndexOf(this.Parameters.FirstOrDefault(p => p.Value.Equals(value)));
        }

        public override void Insert(int index, object value)
        {
            this.Parameters.Insert(index, (DbParameter)value);
        }

        public override bool IsFixedSize
        {
            get { return false; }
        }

        public override bool IsReadOnly
        {
            get { return false; }
        }

        public override bool IsSynchronized
        {
            get { return false; }
        }

        public override void Remove(object value)
        {
            this.Parameters.Remove((DbParameter)value);
        }

        public override void RemoveAt(string parameterName)
        {
            this.Parameters.RemoveAll(p => p.ParameterName == parameterName);
        }

        public override void RemoveAt(int index)
        {
            this.Parameters.RemoveAt(index);
        }

        protected override void SetParameter(string parameterName, DbParameter value)
        {
            this.Parameters[this.IndexOf(parameterName)] = value;
        }

        protected override void SetParameter(int index, DbParameter value)
        {
            this.Parameters[index] = value;
        }
        private readonly object sync = new object();
        public override object SyncRoot
        {
            get { return sync; }
        }
    }
}
