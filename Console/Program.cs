using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HTMLDoc;

namespace Console
{
    class Program
    {
        static void Main()
        {
            Directory.SetCurrentDirectory(@"c:\Users\joel\Dropbox\Programming\HTML");

            var jsdoc = new JSDoc();
            //	var headers = new List<string>() { "AAA", "BBB", "CCC" };
            //	var table = new List<List<object>>() {
            //		new List<object>() { "XXX", 123, 5.4 },
            //		new List<object>() { "YuY", 54, 63 },
            //		new List<object>() { "YuY", DateTime.Now, 63 }
            //	};

            var csvFile = @"sample data\sampledata.csv";
            var reader = new StreamReader(csvFile);

            var csv = new CsvHelper.CsvReader(reader);
            csv.Configuration.AutoMap<SalesRecord>();
            var headers = typeof(SalesRecord).GetProperties().Select(p => p.Name);
            var records = csv.GetRecords<SalesRecord>().ToList();

            var htmldoc = new HTMLDoc.HTMLDoc();
            htmldoc.AddCSSLink("bootstrap/css/bootstrap.css");
            htmldoc.AddCSSLink("bootstrap/css/bootstrap-responsive.css");
            htmldoc.AddJSLink("bootstrap/js/jquery.js");
            htmldoc.AddJSLink("bootstrap/js/bootstrap.js");
            htmldoc.AddJSLink("lib/angular.min.js");
            
            // 
            htmldoc.AddJSLink("htmlDocTableDirective.js");
            htmldoc.StartBody();
            htmldoc.AddToBody("<H1>Table</H1>");
            htmldoc.AddTable<SalesRecord>(headers, records.Take(5));

            var persons = new List<PersonRecord>() {
		new PersonRecord { Name = "Bob", Age = 54 },
		new PersonRecord { Name = "Mitch", Age = 34 }		
	};

            htmldoc.AddTable<PersonRecord>(new[] { "Name", "Age" }, persons);
            htmldoc.AddToBody(string.Format("Created at {0}", DateTime.Now.ToString()));

            var fileName = "htmlDoc.html";
            htmldoc.Write(fileName);
            Process.Start(fileName);
        }
    }
}
