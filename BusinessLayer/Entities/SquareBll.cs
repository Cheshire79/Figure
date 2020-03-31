namespace BusinessLayer.Entities
{
    public class SquareBll : FigureBll
    {
        public float Side { get; private set; }

        public SquareBll(int? idInStore, int id, string name, float side)
            : base(idInStore, id, name)
        {
            Side = side;
        }
        public override float GetArea()
        {
            return Side * Side;
        }

    }
}
