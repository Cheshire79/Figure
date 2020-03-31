using System;

namespace BusinessLayer.Entities
{
   public class CirlceBll : FigureBll
    {
       public float Radius { get; protected set; }

       public CirlceBll(int? idInStore, int id, string name, float radius):base(idInStore,id,name)
        {
            Radius = radius;
        }
        public override float GetArea()
        {
            return (float)Math.PI * Radius * Radius;
        }

    }
}
