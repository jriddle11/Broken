using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Broken
{
    public class StaticGameObjectList : IEnumerable<IGameObject>
    {
        private List<IGameObject> _gameObjects = new();

        public void Add(IGameObject obj)
        {
            _gameObjects.Add(obj);
        }

        public void LoadContent(Game game)
        {
            foreach (IGameObject obj in _gameObjects) obj.LoadContent(game);
        }

        public void Update(GameTime gameTime)
        {
            foreach(var obj in _gameObjects) obj.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch, float opacity = 1f)
        {
            foreach(var obj in _gameObjects) obj.Draw(spriteBatch, opacity);
        }

        public IEnumerator<IGameObject> GetEnumerator()
        {
            return _gameObjects.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
