using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WEBUI.Models.FigureStore
{
    public class FigureStoresView
    {
        public IEnumerable<FigureStoreView> FigureStores { get; set; }//todo
        public PagingInfoView PagingInfo { get; set; }
    }
}