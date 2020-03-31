namespace DataLayer.Entities
{
    public class Cirlce : Figure
    {
        public float Radius { get; set; }

        public Cirlce(int? idInStore, int id, string name, float radius)
            : base(idInStore, id, name)
        {
            Radius = radius;
        }
    }
}
