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
        public const string ModuleName = "htmlDocData";

        private string _tablecode;
        public readonly string Tag;

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

        public override string AddTable(IEnumerable<string> headers, IEnumerable<IEnumerable<object>> rows, int tableCount)
        {
            _tablecode += string.Format(@"
      	                {{
			                headers: {0}, 
                            data: {1}
      	                }},
        		", JsonConvert.SerializeObject(headers), JsonConvert.SerializeObject(rows));

            return string.Format(@"
		        <div ng-controller='{1}'>
                    <div html-doc-table headers='tables[{0}].headers' data='tables[{0}].data'></div>
                </div>
		        ", tableCount, Tag);
        }
    }
}
