using System;
using BusinessLayer.Entities;

namespace WEBUI.Models.Figures
{
    public class FigureViewFactory
    {
        public static  FigureView Create(FigureBll f)
        {

            if (f is CirlceBll)
            {
                CirlceBll c = (CirlceBll)f;
                return new CirlceView() { IdInStore = c.IdInStore, Id = c.Id, Name = f.Name, Radius = c.Radius, Area = c.GetArea() };
            }
            if (f is SquareBll)
            {
                SquareBll s = (SquareBll)f;
                return new SquareView() { IdInStore = s.IdInStore, Id = s.Id, Name = f.Name, Side = s.Side, Area = s.GetArea() };
            }
            if (f is RectangleBll)
            {
                RectangleBll r = (RectangleBll)f;
                return new RectangleView() { IdInStore = r.IdInStore, Id = r.Id, Name = f.Name, Width = r.Width, Height = r.Height, Area = r.GetArea() };
            }
           throw new ArgumentException("There is no such Figure");
        }
    }
}