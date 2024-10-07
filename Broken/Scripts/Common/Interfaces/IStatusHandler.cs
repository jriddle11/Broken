using Broken.Entities;
using Microsoft.Xna.Framework;

namespace Broken
{
    public interface IStatusHandler
    {
        public void HandleLevelUp();

        public void Update(GameTime gameTime);

        public void Damage(int i);
    }
}
