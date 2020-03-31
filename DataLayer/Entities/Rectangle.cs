using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Entities
{
    public class Rectangle : Figure
    {
        public float Width { get; set; }
        public float Height { get; set; }

        public Rectangle(int? idInStore, int id, string name, float width, float height)
            : base(idInStore, id, name)
        {
            Width = width;
            Height = height;
        }
    }
}
