using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HTMLDoc
{
    public static class Extensions
    {
        /// <summary>
        /// Syntactic sugar, i.e. String.Format("You are {0}", "Bob" ) becomes "You are {0}".AsFormat("Bob")
        /// </summary>
        /// <param name="mask"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string AsFormat(this string mask, params object[] args)
        {
            return String.Format(mask, args);
        }
    }

    public static class Utils
    {
        public static string ReadResource(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
