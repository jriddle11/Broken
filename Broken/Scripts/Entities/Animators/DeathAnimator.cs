using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Broken.Entities
{
    public class DeathAnimator
    {
        public bool IsDone { get; private set; } = false;

        private DrawRecord _drawRecord;
        private Vector2 _position;
        private float _elapsedTime;
        private float _animationDuration = .5f; // Total time for animation (in seconds)
        private float _fadeTime = 0f;        // Time when fading starts
        private float _opacity = 1f;

        public DeathAnimator(DrawRecord drawRecord, Vector2 position)
        {
            _drawRecord = drawRecord;
            _position = position;
        }

        public void Update(GameTime gameTime)
        {
            if (IsDone) return;

            // Update elapsed time
            _elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Calculate progress in the animation
            float progress = Math.Min(_elapsedTime / _animationDuration, 1f);

            // Start fading after `fadeTime` has passed
            if (_elapsedTime > _fadeTime)
            {
                float fadeProgress = (_elapsedTime - _fadeTime) / (_animationDuration - _fadeTime);
                _opacity = Math.Max(1f - fadeProgress, 0f);
            }

            // End animation when the duration is complete
            if (_elapsedTime >= _animationDuration)
            {
                IsDone = true;
            }
        }

        public void Draw(SpriteBatch spriteBatch, float opacityOverride = 1f)
        {
            if (IsDone) return;

            // Final opacity considers both the animation fade and any external override
            float finalOpacity = _opacity * opacityOverride;

            // Draw the character sprite
            spriteBatch.Draw(
                _drawRecord.Texture,                  // The texture
                _position,                            // Position of the sprite
                _drawRecord.SourceRectangle,          // Source rectangle
                _drawRecord.Color * finalOpacity,     // Color (with fade applied)
                _drawRecord.Rotation,                 // Rotation
                Vector2.Zero, // Center of rotation
                _drawRecord.Scale,                    // Scale factor
                _drawRecord.SpriteEffects,            // Sprite effects (flip, etc.)
                _drawRecord.LayerDepth                // Layer depth for sorting
            );
        }
    }
}
