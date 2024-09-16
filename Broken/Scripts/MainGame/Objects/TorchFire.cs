using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Broken.Scripts.Common;

namespace Broken.Scripts.MainGame
{
    public class TorchFire : IGameObject
    {
        public bool Visited;
        public float AnimationSpeed = .1f;
        public Vector2 Position;
        public float Scale = .5f;

        const int FLAME_WIDTH = 84;
        const int FLAME_HEIGHT = 147;

        ParticleSystem _torchLight;
        Texture2D _fireTexture;
        Timer _animationTimer;
        int _animationX = 0;
        int _animationY = 0;
        BoundingCircle _detectionCollider;

        public TorchFire(Vector2 pos, Game game)
        {
            Position = pos;
            LoadContent(game);
            _torchLight = new ParticleSystem(Position + new Vector2(15, 50), "My Assets/Menu/Fog", Color.Goldenrod, .03f, 10, .7f, .8f, 5f, 5, .01f);
            _torchLight.LoadContent(game);
            _detectionCollider = new BoundingCircle(Position + new Vector2(FLAME_WIDTH, FLAME_HEIGHT), 400);
        }

        public void Draw(SpriteBatch spriteBatch, float opacity = 1)
        {
            if (Visited)
            {
                Rectangle sourceRectangle = new Rectangle(_animationX * FLAME_WIDTH, _animationY * FLAME_HEIGHT, FLAME_WIDTH, FLAME_HEIGHT);
                spriteBatch.Draw(_fireTexture, Position, sourceRectangle, Color.White * opacity, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0f);
                _torchLight.Draw(spriteBatch);
            }
        }

        public void LoadContent(Game game)
        {
            _fireTexture = game.Content.Load<Texture2D>("External Assets/TorchFire/TorchFireSheet");
            _animationTimer = new Timer(AnimationSpeed);
        }

        public void Update(GameTime gameTime)
        {
            if (_animationTimer.TimeIsUp(gameTime))
            {
                UpdateAnimationFrame();
                _animationTimer.Reset();
            }
            _torchLight.Update(gameTime);

            if (Visited) return;
            if (PlayerIsInRange())
            {
                Visited = true;
            }
        }

        private bool PlayerIsInRange()
        {
            return GameController.Instance.GetPlayer.GetFloorCollider().CollidesWith(_detectionCollider);
        }

        private void UpdateAnimationFrame()
        {
            _animationX = (_animationX + 1) % 3;
            _animationY = (_animationY + 1) % 3;
        }
    }
}
