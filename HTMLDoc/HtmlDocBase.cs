using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTMLDoc
{
    public abstract class HtmlDocBase : JSDoc
    {
        public abstract string ModuleName { get;}

        protected string _tablecode;
        public string Tag;

        public static HtmlDocBase Create(bool useJson = true)
        {
            if (useJson)
                return new HtmlDocJson();
            else
                return new HtmlDocDataTableController();
        }

        public static IEnumerable<object> FlattenObject<T>(T row)
        {
            return row
                .GetType()
                .GetProperties()
                .Select(p => p.GetValue(row));
        }

        public void AddTable<T>(IEnumerable<string> headers, IEnumerable<T> rows, int tableCount)
        {
            var flatRows = rows.Select(r => FlattenObject<T>(r));
            AddTable(headers, flatRows, tableCount);
        }

        public abstract string AddTable(IEnumerable<string> headers, IEnumerable<IEnumerable<object>> rows, int tableCount);
    }
}
