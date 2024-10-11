using Broken.Entities;
using Microsoft.Xna.Framework;

namespace Broken
{
    public interface IStatusHandler
    {
        public void Update(GameTime gameTime);

        public void Damage(int i);
    }
}
