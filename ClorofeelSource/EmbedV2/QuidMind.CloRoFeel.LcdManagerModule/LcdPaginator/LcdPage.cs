using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuidMind.CloRoFeel.LcdManagerModule.LcdPaginator
{
    class LcdPage
    {
        public string Num { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }

        public Action<string, string, string> AttachedAction { get; set; }
    }
}
