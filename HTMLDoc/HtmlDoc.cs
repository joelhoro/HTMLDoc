using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HTMLDoc.HtmlComponents;

namespace HTMLDoc
{
    public class HTMLDoc
    {

        private List<string> _jsLinks = new List<string>();
        private List<string> _cssLinks = new List<string>();

        private List<HTMLComponent> components = new List<HTMLComponent>();
        public bool VerboseHTML = false;

        private JScript JSInitialization()
        {
            var jsbody = initializationJs;
            for (int i = 0; i < tableCount; i++)
            {
                var variableName = String.Format(JSDoc.TableVariableName, i);
                jsbody += string.Format("\tDataInfoToTable({0});\n", variableName);
                // TODO: clean up
                jsbody += string.Format(@"
				var {0}_alldata= [{0}.headers].concat({0}.data);
				var {0}_csv_link = CsvLink({0}_alldata,'data.csv');
				{0}_csv_link.textContent = '[csv]';
				$('#{0}').append({0}_csv_link);
				var {0}_link = JsonLink({0}_alldata,'data.json');
				{0}_link.textContent = '[json]';
				$('#{0}').append({0}_link);
			", variableName);

            }
            var initialization = String.Format(@"
	// initialization
	$(document).ready( function() {{
		{0}	}} );
		", jsbody);
            return new JScript(initialization);
        }

        private string initializationJs = "";

        public void AddJSLink(string link) { Add(new JSInclude(link)); }
        public void AddCSSLink(string link) { Add(new CSSInclude(link)); }
        public void AddToJSInitialization(string code) { initializationJs += code + "\n"; }
        public void AddToBody(string html) { Add(new HTML(html + "\n")); }

        public void Add(HTMLComponent component) { components.Add(component); }

        private JSDoc jsDoc = new JSDoc();

        private int tableCount = 0;

        public void AddTable<T>(IEnumerable<string> headers, IEnumerable<T> rows)
        {
            var flatRows = rows.Select(r => JSDoc.FlattenObject<T>(r));
            AddTable(headers, flatRows);
        }

        public void AddTable(IEnumerable<string> headers, IEnumerable<IEnumerable<object>> rows)
        {
            jsDoc.AddTable(headers, rows, tableCount);
            var tableVariableName = string.Format(JSDoc.TableVariableName, tableCount++);
            var addLink = true;
            var html = "";
            if (addLink)
                html += string.Format("<div id='{0}'></div>", tableVariableName);
            html += string.Format(@"
			<table id='{0}' class='table bootstrap-table table-striped table-hover'>
				<thead></thead><tbody></tbody>
			</table>
		", tableVariableName);
            Add(new HTML(html));
        }

        public HTMLDoc()
        {
            Add(new HTML("<html>\n<head>\n"));
        }

        public void StartBody()
        {
            Add(new HTML("</head>\n\n"));
            Add(new HTML("<body>\n"));
        }

        public void EndBody()
        {
            Add(new HTML("</body>\n\n"));
        }

        public List<HTMLComponent> Components()
        {
            var allcomponents = components.ToList();
            var jsFileName = jsDoc.Write(string.Format("data\\htmldoc_data_{0:yy-MM-dd-HHmmss}.js", DateTime.Now));
            allcomponents.Add(new JSInclude(jsFileName));
            allcomponents.Add(JSInitialization());
            return allcomponents;
        }

        public string Contents()
        {
            var contents = "";
            foreach (var cpnt in Components())
            {
                contents += cpnt.ToHTML(VerboseHTML);
            }

            return contents;
        }

        public string Write(string fileName = "")
        {
            if (fileName == "")
                fileName = Path.GetRandomFileName() + ".html";
            File.WriteAllText(fileName, Contents());
            return fileName;
        }
    }
}
