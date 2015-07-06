using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HTMLDoc
{
    public abstract class JSDoc
    {
        public abstract string Contents();

        public string Write(string fileName = "")
        {
            if (fileName == "")
                fileName = Path.GetRandomFileName() + ".js";

            File.WriteAllText(fileName, Contents());
            return fileName;
        }


    }
}
