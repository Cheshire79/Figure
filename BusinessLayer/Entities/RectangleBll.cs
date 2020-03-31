using DataLayer.Entities;

namespace BusinessLayer.Entities
{
// ReSharper disable once InconsistentNaming
    public class RectangleBll: FigureBll
    {
        public float Width { get; protected set; }
        public float Height { get; protected set; }

        public RectangleBll(int? idInStore, int id, string name, float width, float height)
            : base(idInStore, id, name)
        {
            Width = width;
            Height = height;
        }
        public override float GetArea()
        {
            return Width * Height;

        }

    }
}
