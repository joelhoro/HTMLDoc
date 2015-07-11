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
        public static Dictionary<string,object> ToDictionary<T>(T row)
        {
            return row
                .GetType()
                .GetProperties()
                .ToDictionary(p => p.Name, p => p.GetValue(row));
        }

        public abstract string AddTable(IEnumerable<Dictionary<string, object>> rows, int tableCount);
    }
}
