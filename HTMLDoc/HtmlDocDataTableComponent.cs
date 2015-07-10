using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTMLDoc
{
    public class HtmlDocDataTableController : HtmlDocBase
    {
        public override string ModuleName { get { return "htmlDocData"; } }

        public override bool Include()
        {
            return true;
        }

        public override string Extension()
        {
            return "js";
        }

        public HtmlDocDataTableController()
        {
            _tablecode = "";
            Tag = string.Format("htmldoc_data_{0:yy-MM-dd-HHmmss}", DateTime.Now);
        }

        public override string Contents()
        {
            return string.Format(@"
                angular.module('{2}', [])
                      .controller('{1}', function ($scope){{
      	                $scope.tables = [ 
                        {0}
      	                ]
                      }});

        		", _tablecode, Tag, ModuleName);

        }

        public override string AddTable(IEnumerable<Dictionary<string,object>> rows, int tableCount)
        {
            if (tableCount != 0)
                _tablecode += ", ";
            _tablecode += JsonConvert.SerializeObject(rows);

            return string.Format(@"
		        <div ng-controller='{1}'>
                    <div html-doc-table data='tables[{0}]'></div>
                </div>
		        ", tableCount, Tag);
        }
    }
}
