using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

        public override string Extension()
        {
            return "json";
        }

        public HtmlDocJson()
        {
            _tablecode = "";
            Tag = string.Format("htmldoc_data_{0:yy-MM-dd-HHmmss}", DateTime.Now);
        }

        public static string LoadTemplate(string templateName)
        {
            return Utils.ReadResource("HTMLDoc.assets.html.{0}.html".AsFormat(templateName));
        }

        public override string Contents()
        {
            return string.Format(@"[ {0} ]",_tablecode);

        }

        public override string AddTable(IEnumerable<Dictionary<string, object>> rows, int tableCount)
        {
            if (tableCount != 0)
                _tablecode += ", ";
            _tablecode += JsonConvert.SerializeObject(rows);

            var downloadLink = LoadTemplate("downloadlinktemplate");
            var downloadCsv = downloadLink.AsFormat(tableCount, "csv","CSV");
            var downloadJson = downloadLink.AsFormat(tableCount, "json", "Json");

            var template = LoadTemplate("tabletemplate");
            return string.Format(template, JsFileName(), tableCount, downloadCsv, downloadJson);
        }
    }
}
