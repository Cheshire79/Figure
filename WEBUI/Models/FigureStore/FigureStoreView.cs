using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WEBUI.Models.FigureStore
{
    public class FigureStoreView
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public int CountOfFigures { get; private set; }
        public float Area { get; private set; }

        public FigureStoreView(int id, string name, int numberOfFigures, float area)
        {
            Id = id;
            Name = name;
            CountOfFigures = numberOfFigures;
            Area = area;
        }

    }
}