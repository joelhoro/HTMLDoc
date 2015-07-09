using System;
using System.Collections.Generic;
using System.Linq;
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
}
