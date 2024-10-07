using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Broken.Entities
{
    public class CharacterList : IGameObject, IEnumerable<Character>
    {
        private List<Character> _entities = new();

        public void Add(Character entity)
        {
            _entities.Add(entity);
        }
        public void Remove(uint id)
        {
            for (int i = _entities.Count - 1; i >= 0; i--)
            {
                if (_entities[i].ID == id)
                {
                    _entities.RemoveAt(i);
                    break;
                }
            }
        }
        public void Remove(Character entity) 
        { 
            Remove(entity.ID); 
        }
        public void Clear()
        {
            _entities.Clear();
        }
        public void Draw(SpriteBatch spriteBatch, float opacity = 1f)
        {
            foreach(var entiy in _entities) entiy.Draw(spriteBatch, opacity);
        }
        public void LoadContent(Game game)
        {
            foreach(var entiy in _entities) entiy.LoadContent(game);
        }
        public void Update(GameTime gameTime)
        {
            foreach(var entity in _entities) entity.Update(gameTime);
        }
        public IEnumerator<Character> GetEnumerator()
        {
            return _entities.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
