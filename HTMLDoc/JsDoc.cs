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
        public abstract string Extension();
        public string Tag;

        public abstract bool Include();
        public string Write(string fileName = "")
        {
            if (fileName == "")
                fileName = Path.GetRandomFileName() + ".js";

            File.WriteAllText(fileName, Contents());
            return fileName;
        }

        public string JsFileName() {
            return Write(@"data\\{0}.{1}".AsFormat(Tag,Extension()));
        }

        public IEnumerable<HTMLComponent> Includes()
        {
            if (Include())
                yield return new JSInclude(JsFileName()) as HTMLComponent;
        }
    }
}
