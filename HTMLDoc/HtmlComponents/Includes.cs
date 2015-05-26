using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTMLDoc.HtmlComponents
{
    public class JSInclude : HTMLComponentBase
    {
        private string _link;
        public JSInclude(string link)
        {
            _link = link;
        }
        public override string ToHTML()
        {
            return string.Format("\t<script src='{0}'></script>\n", _link);
        }

        public override string ToString()
        {
            return string.Format("JSInclude[{0}]", _link);
        }
    }

    public class CSSInclude : HTMLComponentBase
    {
        private string _link;
        public CSSInclude(string link)
        {
            _link = link;
        }
        public override string ToHTML()
        {
            return string.Format("\t<link href='{0}' rel='stylesheet'>\n", _link);
        }
        public override string ToString()
        {
            return string.Format("CSSInclude[{0}]", _link);
        }
    }
}
