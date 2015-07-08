using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTMLDoc
{
    public class HtmlDocJson : HtmlDocBase
    {
        public override string ModuleName { get { return "htmlDoc"; } }

        public override bool Include()
        {
            return false;
        }

        public HtmlDocJson()
        {
            _tablecode = "";
            Tag = string.Format("htmldoc_data_{0:yy-MM-dd-HHmmss}", DateTime.Now);
        }

        public override string Contents()
        {
            return string.Format(@"[ {0} ]",_tablecode);

        }

        public override string AddTable(IEnumerable<string> headers, IEnumerable<IEnumerable<object>> rows, int tableCount)
        {
            if (tableCount != 0)
                _tablecode += ", ";
            _tablecode += string.Format(@"
      	                {{
			                ""headers"" : {0}, 
                            ""data"": {1}
      	                }}
        		", JsonConvert.SerializeObject(headers), JsonConvert.SerializeObject(rows));

            return string.Format(@"
                <div ng-controller='htmlDocJsonLoader' source='data\{0}.js'>
                	<div html-doc-table headers='data[{1}].headers' data='data[{1}].data'></div>
                </div>
		        ", Tag, tableCount);
        }
    }
}
