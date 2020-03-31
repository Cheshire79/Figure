using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WEBUI.Models.Figures
{
    public class FiguresView
    {
        public IEnumerable<FigureView> Figures { get; set; }//todo
        public PagingInfoView PagingInfo { get; set; }
    }
}