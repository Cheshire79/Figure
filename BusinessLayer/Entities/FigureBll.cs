namespace BusinessLayer.Entities
{
   public abstract  class FigureBll
    {
       public int Id { get; protected set; }
       public int? IdInStore { get; protected set; }
       public string Name { get; protected set; }
       public abstract float GetArea();
       protected FigureBll(int? idInStore, int id, string name)
       {
           IdInStore = idInStore;
           Id = id;
           Name = name;
       }
    }
}
