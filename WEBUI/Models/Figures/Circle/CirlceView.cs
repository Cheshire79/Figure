using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WEBUI.Models.Figures
{
    public class CirlceView : FigureView
    {
        public float Radius { get; set; }
        public override string Info {
            get { return " Radius = " + Radius; } 
        }
    }
}