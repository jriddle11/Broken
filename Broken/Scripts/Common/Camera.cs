using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Broken.Scripts.MainGame;

namespace Broken.Scripts.Common
{   
    /// <summary>
    /// Object for changing the viewport location and following the player
    /// </summary>
    public class Camera
    {
        public Matrix Transform { get; private set; } = Matrix.Identity;
        public Vector2 Boundaries;

        Vector2 _position;
        Viewport _viewport;

        public Camera()
        {
            _viewport = OutputManager.GraphicsDeviceManager.GraphicsDevice.Viewport;
        }

        public void Follow(Player player)
        {
            _position = new Vector2(player.Position.X + 128 - (_viewport.Width / 2),
                                    player.Position.Y + 128 - (_viewport.Height / 2));

            _position.X = MathHelper.Clamp(_position.X, 0, Boundaries.X - _viewport.Width);
            _position.Y = MathHelper.Clamp(_position.Y, 0, Boundaries.Y - _viewport.Height);
    
            //converts the 2d position into 3d space giving the new location of the camera
            Transform = Matrix.CreateTranslation(new Vector3(-_position, 0));
        }
    }
}
