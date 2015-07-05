﻿using System;
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
            jsbody += String.Format(@"
	        // initialization
	        $(document).ready( function() {{
                tableCount = {0}.length;
                for (var i = 0; i < tableCount; i++)
                {{
                    var table = {0}[i];
                    DataInfoToTable(table);
                    var alldata= [table.headers].concat(table.data);
                    var csv_link = CsvLink(alldata,'data.csv');
                    csv_link.textContent = '[csv]';
                    var node = $('#{0}_'+i);
                    node.append(csv_link);
                    var link = JsonLink(alldata,'data.json');
                    link.textContent = '[json]';
                    node.append(link);
                }}
            }} );",  
            JSDoc.TableVariableName);
            return new JScript(jsbody);
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
            var addLink = true;
            var html = "";
            if (addLink)
                html += string.Format("\n\t\t\t<div id='{0}_{1}'></div>", JSDoc.TableVariableName, tableCount);
            html += string.Format(@"
			<table id='{0}_{1}' class='table bootstrap-table table-striped table-hover'>
				<thead></thead><tbody></tbody>
			</table>
		", JSDoc.TableVariableName,tableCount);
            tableCount++;
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
