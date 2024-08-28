using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Broken.Common.GameTimer;

namespace Broken.Common.ParticleSystem
{
    public class ParticleSystem : IGameObject
    {
        public Vector2 Position;

        private List<Particle> _particles;
        private Texture2D _texture;
        private Color _color;
        private float _opacity;
        private string _textureName;
        private float _particleSpeed;
        private float _scale;
        private float _particleLifeTime;
        private int _maxParticles;
        private Timer _particleTimer;
        private Random _random;

        public ParticleSystem(Vector2 position, string texture, Color color, float opacity = 1f, float speed = 80f, float scale = 1f, double pace = 0.4, float lifeTime = 5f, int maxParticles = 10)
        {
            Position = position;
            _textureName = texture;
            _color = color;
            _opacity = opacity;
            _particleSpeed = speed;
            _scale = scale;
            _particleTimer = new Timer(pace);
            _particleLifeTime = lifeTime;
            _maxParticles = maxParticles;
            _particles = new List<Particle>();
            _random = new Random();
        }

        public void LoadContent(Game game)
        {
            _texture = game.Content.Load<Texture2D>(_textureName);
        }

        public void Update(GameTime gameTime)
        {

            for (int i = 0; i < _particles.Count; i++)
            {
                _particles[i].Update((float)gameTime.ElapsedGameTime.TotalSeconds);
                if (!_particles[i].IsAlive())
                {
                    _particles.RemoveAt(i);
                }
            }

            // Add new particles
            if(_particleTimer.TimeIsUp(gameTime))
            {
                if (_particles.Count < _maxParticles)
                {
                    GenerateNewParticle();
                }
                _particleTimer.Reset();
            }
        }

        private void GenerateNewParticle()
        {
            float angle = (float)(_random.NextDouble() * MathHelper.TwoPi);
            Vector2 velocity = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * _particleSpeed;
            float scale = (float)(_random.NextDouble() * 0.5 + _scale);
            Color color = _color * (float)(_random.NextDouble() * 0.5 + 0.5);

            Particle newParticle = new Particle(Position, velocity, _particleLifeTime, scale, color);
            _particles.Add(newParticle);
        }

        public void Draw(SpriteBatch spriteBatch, float opacity = 1f)
        {
            foreach (var particle in _particles)
            {
                float remainingLife = 1f - (particle.Age / particle.LifeTime);
                Color color = particle.Color * remainingLife * _opacity * opacity;
                spriteBatch.Draw(_texture, particle.Position, null, color, 0f, new Vector2(_texture.Width / 2, _texture.Height / 2), particle.Scale, SpriteEffects.None, 0f);
            }
        }
    }

}
