using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using HTMLDoc.HtmlComponents;

namespace HTMLDoc
{
    public abstract class JSDoc
    {
        public abstract string Contents();

        public abstract bool Include();
        public string Write(string fileName = "")
        {
            if (fileName == "")
                fileName = Path.GetRandomFileName() + ".js";

            File.WriteAllText(fileName, Contents());
            return fileName;
        }

        public IEnumerable<HTMLComponent> Includes(string tag)
        {
            var jsFileName = Write(@"data\\" + tag + ".js");
            if (Include())
                yield return new JSInclude(jsFileName) as HTMLComponent;
        }
    }
}
