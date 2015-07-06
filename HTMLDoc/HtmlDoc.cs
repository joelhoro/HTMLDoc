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
            jsbody += @"angular.module('htmlDocApp', ['htmlDocTable', 'htmlDocData']);";
            return new JScript(jsbody);
        }

        private string initializationJs = "";

        public void AddJSLink(string link) { Add(new JSInclude(link)); }
        public void AddCSSLink(string link) { Add(new CSSInclude(link)); }
        public void AddToJSInitialization(string code) { initializationJs += code + "\n"; }
        public void AddToBody(string html) { Add(new HTML(html + "\n")); }

        public void Add(HTMLComponent component) { components.Add(component); }

        private HtmlDocDataTableComponent htmlDocData = new HtmlDocDataTableComponent();

        private int tableCount = 0;

        public void AddTable<T>(IEnumerable<string> headers, IEnumerable<T> rows)
        {
            var flatRows = rows.Select(r => HtmlDocDataTableComponent.FlattenObject<T>(r));
            AddTable(headers, flatRows);
        }

        public void AddTable(IEnumerable<string> headers, IEnumerable<IEnumerable<object>> rows)
        {
            htmlDocData.AddTable(headers, rows, tableCount);
            var addLink = true;
            var html = "";
            //if (addLink)
                //html += string.Format("\n\t\t\t<div id='{0}_{1}'></div>", JSDoc.TableVariableName, tableCount);
            html += string.Format(@"
		        <div ng-controller='htmlDocDataTableController'>
                    <div html-doc-table headers='tables[{0}].headers' data='tables[{0}].data'></div>
                </div>
		        ", tableCount);
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
            Add(new HTML("<body ng-app='htmlDocApp'>\n"));
        }

        public void EndBody()
        {
            Add(new HTML("</body>\n\n"));
        }

        public List<HTMLComponent> Components()
        {
            var allcomponents = components.ToList();
            var jsFileName = htmlDocData.Write(string.Format("data\\htmldoc_data_{0:yy-MM-dd-HHmmss}.js", DateTime.Now));
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
