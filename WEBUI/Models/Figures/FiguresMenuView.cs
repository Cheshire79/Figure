using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WEBUI.Models.Figures
{
    public class FiguresMenuView
    {
        public IEnumerable<FigureMenuView> Figures { get; set; }
        public FigureType SelectedFigureType { get; set; }
    }
}