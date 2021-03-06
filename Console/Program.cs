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

            var htmldoc = new HTMLDoc.HTMLDoc(useJsonTables: true);
            htmldoc.AddCSSandJSLinks();

            htmldoc.StartBody();
            htmldoc.AddToBody("<H1>Table</H1>");

            var data = GetRecords();
            var records = data.Item1;
            var headers = data.Item2;
            htmldoc.AddTable<SalesRecord>(records.Take(5));

            var persons = new List<PersonRecord>() {
		        new PersonRecord { Name = "Bob", Age = 54 },
		        new PersonRecord { Name = "Mitch", Age = 34 }		
	        };

            htmldoc.AddToBody("And here goes another table");
            htmldoc.AddTable<PersonRecord>(persons);
            htmldoc.AddToBody(string.Format("Created at {0}", DateTime.Now.ToString()));

            var fileName = "htmlDoc.html";
            var server = "localhost:8080";
            
            htmldoc.Write(fileName);
            Process.Start("chrome", "http://{0}/{1}".AsFormat(server, fileName));
        }
    }
}
