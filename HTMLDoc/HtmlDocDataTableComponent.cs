using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTMLDoc
{
    public class HtmlDocDataTableComponent : JSDoc
    {
        public string tablecode = "";

        public override string Contents()
        {
            return string.Format(@"
                angular.module('htmlDocData', [])
                      .controller('htmlDocDataTableController', function ($scope){{
      	                $scope.tables = [ 
                        {0}
      	                ]
                      }});

        		", tablecode);

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

        public void AddTable(IEnumerable<string> headers, IEnumerable<IEnumerable<object>> rows, int tableCount)
        {
            tablecode += string.Format(@"
      	                {{
			                headers: {0}, 
                            data: {1}
      	                }},
        		", JsonConvert.SerializeObject(headers), JsonConvert.SerializeObject(rows));
        }

    }
}
