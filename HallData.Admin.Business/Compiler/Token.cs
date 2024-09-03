using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.Admin.ApplicationViews;
using HallData.Admin.Business;
using System.Collections.Concurrent;
using System.Threading;

namespace HallData.Admin.Compiler
{
    public abstract class Token
    {
        private ConcurrentStack<Token> Children { get; set; }
        protected CompilerCache Cache { get; private set; }
        public Token(CompilerCache cache)
        {
            this.Cache = cache;
        }
        protected abstract Task<string> CompileSelf(CancellationToken token = default(CancellationToken));
        public abstract Task Prepare(CancellationToken token = default(CancellationToken));
        public async Task<string> Compile(CancellationToken token = default(CancellationToken))
        {
            StringBuilder builder = new StringBuilder();
            Token t;
            while(Children.TryPop(out t))
            {
                builder.Insert(0, await t.Compile(token));
            }
            builder.Insert(0, await t.CompileSelf(token));
            return builder.ToString();
        }
        protected void AppendToken(Token token)
        {
            Children.Push(token);
        }
    }
    public class DataViewResultLevelToken : Token
    {
        protected DataViewResultResult Result { get; private set; }
        public DataViewResultLevelToken(DataViewResultResult result, CompilerCache cache)
            : base(cache)
        {
            this.Result = result;
        }

        public override Task Prepare(CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        protected async override Task<string> CompileSelf(CancellationToken token = default(CancellationToken))
        {
            if (this.Result.ResultIndex == 0)
                return "";
            var keyColumns = await this.Cache.GetKeyColumns(this.Result.DataViewResultId.Value, token);
            
            foreach(var column in keyColumns.Where(c => c.ParentResultColumn != null && c.ParentResultColumn.DataViewColumnId != null))
            {

            }
            throw new NotImplementedException();
        }
    }
    
    public class DataViewColumnToken : Token
    {
        protected DataViewColumnResult Column { get; private set; }
        public DataViewColumnToken(DataViewColumnResult column, CompilerCache cache)
            :base(cache)
        {
            this.Column = column;
        }


        protected override Task<string> CompileSelf(CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public override Task Prepare(CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }
    }
}
