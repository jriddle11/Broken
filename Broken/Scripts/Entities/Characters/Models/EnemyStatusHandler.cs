using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Broken.Entities
{
    public abstract class EnemyStatusHandler : IStatusHandler
    {
        protected EnemyDifficulty Difficulty;
        protected CharacterStatus Status;
        protected bool Initialized = false;

        public virtual void Update(GameTime gameTime)
        {
            if (!Initialized) throw new Exception("EnemyStatHandler was updated before being initialized");
        }

        protected abstract void Setup();
        
        public virtual void Initialize(CharacterStatus stats)
        {
            Status = stats;
            Initialized = true;
        }

        public virtual void Damage(int i)
        {
            if(!Status.Invulnerable) Status.Health -= i;
        }

    }
}
