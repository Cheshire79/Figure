using System.Collections.Generic;
using WEBUI.Models.Figures;

namespace WEBUI.Models.FigureStore
{
    public class SquaresViewForAddingToStore
    {
        public IEnumerable<SquareView> Squares { get; set; }//todo
        public PagingInfoView PagingInfo { get; set; }
        public int StoreId { get; set; }
        public string StoreName { get; set; }

    }
}