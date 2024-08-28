using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Broken
{
    public interface IGameObject
    {
        void LoadContent(Game game);

        void Update(GameTime gameTime);

        void Draw(SpriteBatch spriteBatch, float opacity = 1f);
    }
}
