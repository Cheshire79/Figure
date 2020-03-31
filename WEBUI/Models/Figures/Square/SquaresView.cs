using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WEBUI.Models.Figures
{
    public class SquaresView
    {
        public List<SquareView> Squares { get; set; }//todo
        public PagingInfoView PagingInfo { get; set; }
    }
}