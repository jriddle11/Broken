using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Broken
{
    public class Vector2Record
    {
        public float X { get; set; }
        public float Y { get; set; }

        public static Vector2Record ConvertToEntity(Vector2 v)
        {
            return new Vector2Record() { X = v.X, Y = v.Y };
        }

        public static List<Vector2Record> ConvertToEntities(List<Vector2> list)
        {
            List<Vector2Record> entities = new List<Vector2Record>();
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
