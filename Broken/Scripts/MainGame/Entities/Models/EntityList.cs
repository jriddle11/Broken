using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Broken.Scripts.MainGame
{
    public class EntityList : IGameObject, IEnumerable<Entity>
    {
        private List<Entity> Entities = new();

        public void Add(Entity entity)
        {
            Entities.Add(entity);
        }
        public void Remove(uint id)
        {
            foreach(var entity in Entities) if(entity.ID == id) Entities.Remove(entity);
        }
        public void Remove(Entity entity) 
        { 
            Entities.Remove(entity); 
        }
        public void Clear()
        {
            Entities.Clear();
        }
        public void Draw(SpriteBatch spriteBatch, float opacity = 1f)
        {
            foreach(var entiy in Entities) entiy.Draw(spriteBatch, opacity);
        }
        public void LoadContent(Game game)
        {
            foreach(var entiy in Entities) entiy.LoadContent(game);
        }
        public void Update(GameTime gameTime)
        {
            foreach(var entity in Entities) entity.Update(gameTime);
        }
        public IEnumerator<Entity> GetEnumerator()
        {
            return Entities.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
