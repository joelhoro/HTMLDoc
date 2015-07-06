﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HTMLDoc
{
    public class JSDoc
    {
        public const string TableVariableName = "htmldocTables";

        public string code = "";

        public string Contents()
        {
            return code;
        }
        public string Write(string fileName = "")
        {
            if (fileName == "")
                fileName = Path.GetRandomFileName() + ".js";

            File.WriteAllText(fileName, Contents());
            return fileName;
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
            code += string.Format(@"
                angular.module('htmlDocData', [])
                      .controller('htmlDocDataTableController', function ($scope){{
      	                $scope.tables = [ 
      	                {{
			                headers: {0}, 
                            data: {1}
      	                }}
      	                ]
                      }});

        		", JsonConvert.SerializeObject(headers), JsonConvert.SerializeObject(rows));
        }

    }
}
