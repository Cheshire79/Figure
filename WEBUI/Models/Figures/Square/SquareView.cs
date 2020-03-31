using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WEBUI.Models.Figures
{
    public class SquareView : FigureView
    {
        public float Side { get; set; }
        public override string Info
        {
            get { return " Side = " + Side; }
        }
    }
}