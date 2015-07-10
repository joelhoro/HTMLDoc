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

        const string AppName = "htmlDocApp";
        const string DocModule = "htmlDoc";
        const string DownloadLinkModule = "downloadlinks";

        private readonly bool useJsonTables = true;

        public HTMLDoc(bool useJsonTables = true)
        {
            this.useJsonTables = useJsonTables;
            htmlDocData = HtmlDocBase.Create(useJsonTables);
            AngularModules = new List<string>();

            Add(new HTML("<html>\n<head>\n"));
        }

        public void AddCSSandJSLinks()
        {
            var bootstrapDir = "lib/3rdParty/bootstrap-3.3.5-dist";
            AddCSSLink(@"{0}/css/bootstrap.min.css".AsFormat(bootstrapDir));
            AddJSLink(@"{0}/js/jquery-1.11.3.min.js".AsFormat(bootstrapDir));
            AddJSLink(@"{0}/js/bootstrap.js".AsFormat(bootstrapDir));
            AddJSLink("lib/3rdParty/angular.min.js");

            AddJSLink("lib/downloadlinks.js");
            AddJSLink("lib/htmldoc.js");

            AngularModules.AddRange(new[] { AppName, DocModule, htmlDocData.ModuleName, DownloadLinkModule });
        }


        private List<string> AngularModules;

        private JScript JSInitialization()
        {
            var jsbody = initializationJs;
            var modules = string.Join(", ", AngularModules.Select(m => "'" + m + "'"));
            jsbody += String.Format(@"angular.module('{0}', [{1}]);", AppName, modules);
            return new JScript(jsbody);
        }

        private string initializationJs = "";

        public void AddJSLink(string link) { Add(new JSInclude(link)); }
        public void AddCSSLink(string link) { Add(new CSSInclude(link)); }
        public void AddToJSInitialization(string code) { initializationJs += code + "\n"; }
        public void AddToBody(string html) { Add(new HTML(html + "\n")); }

        public void Add(HTMLComponent component) { components.Add(component); }

        private HtmlDocBase htmlDocData;

        private int tableCount = 0;
        private bool _bodyStarted = false;

        public void AddTable<T>(IEnumerable<T> rows)
        {
            var dictionaries = rows.Select(HtmlDocDataTableController.ToDictionary<T>);
            AddTable(dictionaries);
        }

        private void AddTable(IEnumerable<Dictionary<string,object>> dictionaries)
        {
            var html = htmlDocData.AddTable(dictionaries, tableCount);
            tableCount++;
            Add(new HTML(html));
        }

        public void StartBody()
        {
            Add(new HTML("</head>\n\n"));
            Add(new HTML(String.Format("<body ng-app='{0}'>\n", AppName)));
            _bodyStarted = true;
        }

        public static HTML BodyEnd()
        {
            return new HTML("</body>\n\n");
        }
        public void EndBody()
        {
            Add(BodyEnd());
        }

        public List<HTMLComponent> Components()
        {
            var allcomponents = components.ToList();

            var jsIncludes = htmlDocData.Includes();
            allcomponents.AddRange(jsIncludes);
            allcomponents.Add(JSInitialization());
            if (_bodyStarted)
                allcomponents.Add(BodyEnd());
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
