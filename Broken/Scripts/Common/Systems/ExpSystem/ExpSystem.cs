using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Broken.Entities;

namespace Broken
{
    public class ExpSystem : IGameObject
    {
        private Texture2D _texture;
        private Experience[] _experiencePool;
        private int _experienceIndex;
        private int _maxExperience;
        private BoundingRectangle _collider = new BoundingRectangle(Vector2.Zero, 40, 40);
        private Vector2 _colliderPositionOffset = new Vector2(-20, -10);

        private SoundEffect _xpSound;
        private SoundEffectInstance _soundInstance;


        private const int FRAME_WIDTH = 712;
        private const int FRAME_HEIGHT = 712;
        private Player _player => GameScreen.Instance.GetPlayer;

        public ExpSystem(int maxExperience = 20)
        {
            _maxExperience = maxExperience;
            _experiencePool = new Experience[_maxExperience];
            _experienceIndex = 0;
        }

        public void LoadContent(Game game)
        {
            _texture = game.Content.Load<Texture2D>("My Assets/XP");
            _xpSound = game.Content.Load<SoundEffect>("External Assets/SoundEffects/XpGet");
            _soundInstance = _xpSound.CreateInstance();
            _soundInstance.Volume = .25f;
        }

        public void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            for (int i = 0; i < _experiencePool.Length; i++)
            {
                if (_experiencePool[i].IsVisible)
                {
                    _experiencePool[i].Update(deltaTime);
                }
            }
        }

        public void GenerateExperience(Vector2 position, float moveDuration = 1f)
        {
            float angle = (float)(RandomHelper.NextDouble() * MathHelper.TwoPi);
            Vector2 velocity = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * RandomHelper.Next(400, 600);

            if (_experiencePool[_experienceIndex].IsVisible)
            {
                CollectExp(ref _experiencePool[_experienceIndex]);
            }

            _experiencePool[_experienceIndex].Reset(position, velocity, moveDuration);
            _experienceIndex = (_experienceIndex + 1) % _maxExperience;
        }

        private void WallCollisionCheck(ref Experience exp)
        {
            foreach(var wall in GameScreen.Instance.CurrentRoom.RectangleColliders)
            {
                if (_collider.CollidesWith(wall))
                {
                    if (_collider.X < wall.X || _collider.X + _collider.Width > wall.X + wall.Width)
                    {
                        exp.Velocity = new Vector2(-exp.Velocity.X, exp.Velocity.Y); 
                    }

                    if (_collider.Y < wall.Y || _collider.Y + _collider.Height > wall.Y + wall.Height)
                    {
                        exp.Velocity = new Vector2(exp.Velocity.X, -exp.Velocity.Y); 
                    }

                    exp.Velocity *= 0.9f;
                }
            }
        }

        private void PlayerCollisionCheck(ref Experience exp)
        {
            if (_collider.CollidesWith(_player.Collider) && !exp.IsMoving)
            {
                CollectExp(ref exp);
            }
        }

        private void CollectExp(ref Experience exp)
        {
            _player.Status.Experience++;
            exp.IsVisible = false;
            PlayExpSound();
        }

        private void PlayExpSound()
        {
            if(_soundInstance.State == SoundState.Playing)
            {
                _soundInstance.Stop();
                _soundInstance.Pitch = Math.Clamp(_soundInstance.Pitch + .1f, -1, .7f);
                _soundInstance.Play();
            }
            else
            {
                _soundInstance.Pitch = 0f;
                _soundInstance.Play();
            }
        }

        public void Draw(SpriteBatch spriteBatch, float opacity = 1)
        {
            for (int i= 0; i < _experiencePool.Length; ++i)
            {
                if (_experiencePool[i].IsVisible)
                {
                    _collider.X = _experiencePool[i].Position.X + _colliderPositionOffset.X;
                    _collider.Y = _experiencePool[i].Position.Y + _colliderPositionOffset.Y;
                    
                    PlayerCollisionCheck(ref _experiencePool[i]);
                    if(!_experiencePool[i].IsNearPlayer)WallCollisionCheck(ref _experiencePool[i]);

                    Color color = Color.White * opacity;
                    int frameX = _experiencePool[i].CurrentFrame * FRAME_WIDTH;
                    Rectangle sourceRect = new Rectangle(frameX, 0, FRAME_WIDTH, FRAME_HEIGHT);

                    spriteBatch.Draw(
                        _texture,
                        _experiencePool[i].Position,
                        sourceRect,
                        color,
                        0f,
                        new Vector2(FRAME_WIDTH / 2, FRAME_HEIGHT / 2),
                        .1f,
                        SpriteEffects.None,
                        GameScreen.Instance.GetRoomDepth(_collider)
                    );
                }
            }
        }
    }
}
