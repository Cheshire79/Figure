using System.Collections.Generic;
using WEBUI.Models.Figures;

namespace WEBUI.Models.FigureStore
{
    public class RectanglesViewForAddingToStore
    {
        public IEnumerable<RectangleView> Rectangles { get; set; }//todo
        public PagingInfoView PagingInfo { get; set; }
        public int StoreId { get; set; }
        public string StoreName { get; set; }
    }
}