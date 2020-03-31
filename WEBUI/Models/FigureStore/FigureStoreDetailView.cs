using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WEBUI.Models.Figures;

namespace WEBUI.Models.FigureStore
{
    public class FigureStoreDetailView
    {
         public int Id { get; private set; }
        public string Name { get; private set; }
        public int CountOfFigures { get; private set; }
        public float Area { get; private set; }
        public List<FigureView> Figures { get; set; }//todo
        public FigureStoreDetailView(int id, string name, int numberOfFigures, float area)
        {
            Id = id;
            Name = name;
            CountOfFigures = numberOfFigures;
            Area = area;
        }
        public PagingInfoView PagingInfo { get; set; }

    }
}