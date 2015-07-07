﻿using System;
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
        private static Tuple<IEnumerable<SalesRecord>, IEnumerable<string>> GetRecords()
        {
            var csvFile = @"sample data\sampledata.csv";
            var reader = new StreamReader(csvFile);

            var csv = new CsvHelper.CsvReader(reader);
            csv.Configuration.AutoMap<SalesRecord>();
            var headers = typeof(SalesRecord).GetProperties().Select(p => p.Name);
            var records = csv.GetRecords<SalesRecord>();
            return Tuple.Create(records, headers);
        }

        static void Main()
        {
            Directory.SetCurrentDirectory(@"..\..\..\HTMLDoc\assets");


            var htmldoc = new HTMLDoc.HTMLDoc();
            htmldoc.AddCSSLink("lib/3rdParty/bootstrap/css/bootstrap.css");
            htmldoc.AddCSSLink("lib/3rdParty/bootstrap/css/bootstrap-responsive.css");
            htmldoc.AddJSLink("lib/3rdParty/bootstrap/js/jquery.js");
            htmldoc.AddJSLink("lib/3rdParty/bootstrap/js/bootstrap.js");
            htmldoc.AddJSLink("lib/3rdParty/angular.min.js");
            
            // 
            htmldoc.AddJSLink("lib/htmldoc_utils.js");
            htmldoc.AddJSLink("lib/htmldoctableDirective.js");
            htmldoc.StartBody();
            htmldoc.AddToBody("<H1>Table</H1>");

            var data = GetRecords();
            var records = data.Item1;
            var headers = data.Item2;
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
