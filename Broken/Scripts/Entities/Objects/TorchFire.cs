using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Diagnostics;

namespace Broken.Entities
{
    public class TorchFire : IGameObject
    {
        public bool Visited;
        public float AnimationSpeed = .1f;
        public Vector2 Position;
        public float Scale = .5f;

        const int FLAME_WIDTH = 84;
        const int FLAME_HEIGHT = 147;
        const int DETECTION_RANGE = 350;

        ParticleSystem _torchLight;
        Texture2D _fireTexture;
        Timer _animationTimer;
        int _animationX = 0;
        int _animationY = 0;
        BoundingCircle _detectionCollider;

        // Sound effect variables
        private SoundEffect _torchSound;
        private SoundEffect _torchWoosh;
        private SoundEffectInstance _soundInstance;
        private SoundEffectInstance _soundWooshInstance;

        public TorchFire(Vector2 pos, Game game)
        {
            Position = pos;
            LoadContent(game);
            _torchLight = new ParticleSystem(Position + new Vector2(15, 50), "My Assets/Menu/Fog", Color.Goldenrod, .03f, 10, .7f, .8f, 5f, 5, .04f);
            _torchLight.LoadContent(game);
            _detectionCollider = new BoundingCircle(Position + new Vector2(FLAME_WIDTH, FLAME_HEIGHT), DETECTION_RANGE);
        }

        public void Draw(SpriteBatch spriteBatch, float opacity = 1)
        {
            if (Visited)
            {
                Rectangle sourceRectangle = new Rectangle(_animationX * FLAME_WIDTH, _animationY * FLAME_HEIGHT, FLAME_WIDTH, FLAME_HEIGHT);
                spriteBatch.Draw(_fireTexture, Position, sourceRectangle, Color.White * opacity, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0.045f);
                _torchLight.Draw(spriteBatch);
            }
        }

        public void LoadContent(Game game)
        {
            _fireTexture = game.Content.Load<Texture2D>("External Assets/TorchFire/TorchFireSheet");
            _animationTimer = new Timer(AnimationSpeed);

            _torchSound = game.Content.Load<SoundEffect>("External Assets/TorchFire/fireCrackle");
            _torchWoosh = game.Content.Load<SoundEffect>("External Assets/TorchFire/fireWoosh");
            _soundInstance = _torchSound.CreateInstance();
            _soundWooshInstance = _torchWoosh.CreateInstance();
            _soundWooshInstance.Volume = .8f;
            _soundInstance.IsLooped = true; 

            if(Visited) _soundInstance.Play();
        }

        public void Update(GameTime gameTime)
        {
            if (_animationTimer.TimeIsUp(gameTime))
            {
                UpdateAnimationFrame();
                _animationTimer.Reset();
            }
            _torchLight.Update(gameTime);

            if (PlayerIsInRange())
            {
                if (!Visited)
                {
                    Visited = true;
                    _soundWooshInstance.Play();
                    _soundInstance.Play();
                }
                
            }
            if (Visited)
            {
                UpdateSoundPosition(GameScreen.Instance.GetPlayer.CenterPosition);
            }
        }

        private bool PlayerIsInRange()
        {
            return GameScreen.Instance.GetPlayer.FloorCollider.CollidesWith(_detectionCollider);
        }

        private void UpdateAnimationFrame()
        {
            _animationX = (_animationX + 1) % 3;
            _animationY = (_animationY + 1) % 3;
        }

        private void UpdateSoundPosition(Vector2 playerPosition)
        {
            // Calculate the vector from the torch to the player
            Vector2 direction = playerPosition - Position;

            // Calculate the distance
            float distance = direction.Length();

            // Normalize the direction
            if (distance > 0)
            {
                direction.Normalize();
            }

            // Set the sound instance's pan and volume based on distance
            // Max volume when the player is close, reduce it with distance
            float maxDistance = 600f; // Adjust based on how far you want the sound to carry
            float volume = MathHelper.Clamp(1.3f - (Math.Abs(distance) / maxDistance), 0, 1);
            float pan = Math.Clamp(0 - (direction.X / maxDistance),-1f,1f);

            // Apply volume and pan to the sound instance
            _soundInstance.Volume = volume;
            _soundInstance.Pan = pan;

            _soundWooshInstance.Pan = pan;
        }
    }
}
