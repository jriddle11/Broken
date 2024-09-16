using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Broken.Scripts.MainGame;

namespace Broken.Scripts.Common
{
    /// <summary>
    /// Object for changing the viewport location, following the player, and zooming in/out in steps.
    /// </summary>
    public class Camera
    {
        public bool CanZoom = false;
        public Matrix Transform { get; private set; } = Matrix.Identity;
        public Vector2 Boundaries;

        Vector2 _position;
        Viewport _viewport;
        float _zoom = 1.0f;
        const float ZoomSpeed = 0.03f;  //3% each step of the mouse wheel
        const float MinZoom = 0.7f;
        const float MaxZoom = 1f; 
        int _previousScrollValue;

        public Camera()
        {
            _viewport = OutputManager.GraphicsDeviceManager.GraphicsDevice.Viewport;
            _previousScrollValue = Mouse.GetState().ScrollWheelValue;
        }

        /// <summary>
        /// Updates the camera's position to follow the player.
        /// </summary>
        public void Follow(Player player)
        {
            AdjustZoom();

            _position = new Vector2(player.Position.X + 128 - (_viewport.Width / 2) / _zoom,
                                    player.Position.Y + 128 - (_viewport.Height / 2) / _zoom);

            _position.X = MathHelper.Clamp(_position.X, 0, Boundaries.X - (_viewport.Width / _zoom));
            _position.Y = MathHelper.Clamp(_position.Y, 0, Boundaries.Y - (_viewport.Height / _zoom));

            Transform = Matrix.CreateTranslation(new Vector3(-_position, 0)) * Matrix.CreateScale(_zoom);
        }

        public void ForceZoom(float zoom)
        {
            _zoom = zoom;
        }

        public void AdjustZoom()
        {
            if (!CanZoom) return;

            MouseState mouseState = Mouse.GetState();

            int currentScrollValue = mouseState.ScrollWheelValue;

            int scrollDifference = currentScrollValue - _previousScrollValue;

            if (scrollDifference > 0)
            {
                _zoom += ZoomSpeed;
            }
            else if (scrollDifference < 0)
            {
                _zoom -= ZoomSpeed;
            }

            _zoom = MathHelper.Clamp(_zoom, MinZoom, MaxZoom);

            _previousScrollValue = currentScrollValue;
        }
    }
}
