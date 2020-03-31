
namespace DataLayer.Entities
{
    public class Square : Figure
    {
        public float Side { get; private set; }

        public Square(int? idInStore, int id, string name, float side)
            : base(idInStore, id, name)
        {
            Side = side;
        }
    }
}
