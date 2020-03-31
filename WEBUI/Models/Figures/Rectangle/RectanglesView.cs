using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WEBUI.Models.Figures
{
    public class RectanglesView
    {
        public List<RectangleView> Rectangles { get; set; }//todo
        public PagingInfoView PagingInfo { get; set; }
    }
}