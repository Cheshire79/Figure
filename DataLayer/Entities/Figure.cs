namespace DataLayer.Entities
{
    public class Figure
    {
        public int? IdInStore { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        protected Figure(int? idInStore, int id, string name)
        {
            IdInStore = idInStore;
            Id = id;
            Name = name;
        }
    }
}
