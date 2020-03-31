using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WEBUI.Models.Figures
{
    public abstract class FigureView
    {
        public int? IdInStore { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public float Area { get; set; }
        public virtual string Info
        {
            get { return ""; }
        }

    }
}