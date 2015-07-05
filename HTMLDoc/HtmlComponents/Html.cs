using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTMLDoc.HtmlComponents
{
    public class HTML : HTMLComponentBase
    {
        private string _html;
        public HTML(string html)
        {
            _html = html;
        }
        public override string ToHTML()
        {
            return _html;
        }

        public override string ToString()
        {
            return string.Format("HTML[{0}]", _html);
        }
    }

    public class JScript : HTMLComponentBase
    {
        private string _code;
        public JScript(string code)
        {
            _code = code;
        }
        public override string ToHTML()
        {
            return String.Format("\t<script>\n{0}\n\t</script>\n", _code);
        }

        public override string ToString()
        {
            return string.Format("JScript[{0}]", _code);
        }
    }
}
