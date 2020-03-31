using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WEBUI.Models.Figures;

namespace WEBUI.Models.FigureStore
{
    public class CirlcesViewForAddingToStore
    {
        public IEnumerable<CirlceView> Cirlces { get; set; }//todo
        public PagingInfoView PagingInfo { get; set; }
        public int StoreId { get; set; }
        public string StoreName { get; set; }

    }

}