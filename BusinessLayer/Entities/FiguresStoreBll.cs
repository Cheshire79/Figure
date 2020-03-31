using System.Collections.Generic;
using DataLayer.Entities;

namespace BusinessLayer.Entities
{
    public class FiguresStoreBll
    {
        public int Id { get; protected set; }
        public int Count 
        {get { return Figures.Count; } }
        public string Name { get; protected set; }

        public  List<FigureBll> Figures;
        public FiguresStoreBll(int id, string name)
        {
            Id = id;
            Name = name;
            Figures = new List<FigureBll>();
        }
        public void Add(FigureBll f)
        {
            Figures.Add(f);
        }
        public float GetAreas()
        {
            float areas = 0;
            for (int i = 0; i < Figures.Count; i++)
                areas += Figures[i].GetArea();
            return areas;
        }

    }
}
