using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WEBUI.Models.Figures
{
    public class ParametrsForView
    {
        public int Page { get; set; }
        public ParametrsForView()
        {
            Page = 1; // c# 6
            PartOfName = "";
        }
        public string PartOfName { get; set; }
    }
}