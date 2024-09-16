using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Broken.Scripts.Models
{
    public class Vector2Entity
    {
        public float X { get; set; }
        public float Y { get; set; }

        public static Vector2Entity ConvertToEntity(Vector2 v)
        {
            return new Vector2Entity() { X = v.X, Y = v.Y };
        }

        public static List<Vector2Entity> ConvertToEntities(List<Vector2> list)
        {
            List<Vector2Entity> entities = new List<Vector2Entity>();
            foreach(var vect in list)
            {
                entities.Add(ConvertToEntity(vect));
            }
            return entities;
        }

        public Vector2 ConvertToVector()
        {
            return new Vector2(X, Y);
        }
    }
}
