using Microsoft.Xna.Framework;

namespace Broken
{
    /// <summary>
    /// Individual particle in the <see cref="ParticleSystem"/>
    /// </summary>
    public struct Particle
    {
        public Vector2 Position;
        public Vector2 Velocity;
        public float LifeTime;
        public float Age;
        public float Scale;
        public Color Color;
        public bool IsAlive => Age < LifeTime;

        public Particle(Vector2 position, Vector2 velocity, float lifeTime, float scale, Color color)
        {
            Position = position;
            Velocity = velocity;
            LifeTime = lifeTime;
            Age = 0f;
            Scale = scale;
            Color = color;
        }

        public void Update(float deltaTime)
        {
            Position += Velocity * deltaTime;
            Age += deltaTime;
        }

        public void Reset(Vector2 position, Vector2 velocity, float lifeTime, float scale, Color color)
        {
            Position = position;
            Velocity = velocity;
            LifeTime = lifeTime;
            Age = 0f;
            Scale = scale;
            Color = color;
        }
    }
}
