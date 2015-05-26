using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTMLDoc.HtmlComponents
{
    public interface HTMLComponent
    {
        string ToHTML();
        string ToHTML(bool verbose);
    }

    public abstract class HTMLComponentBase : HTMLComponent
    {
        public virtual string ToHTML()
        {
            throw new NotImplementedException();
        }
        public string ToHTML(bool verbose)
        {
            if (verbose)
                return string.Format("<!-- Begin {0} -->\n{1}<!-- End {0} -->\n", GetType().Name, ToHTML());
            else
                return ToHTML();
        }
    }
}
