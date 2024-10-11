using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Broken.Entities
{
    public class PlayerStatusHandler : IStatusHandler
    {
        private CharacterStatus _status;

        private bool Initialized = false;

        public PlayerStatusHandler(CharacterStatus stats)
        {
            Initialize(stats);
        }

        public void Update(GameTime gameTime)
        {
            if (!Initialized) throw new Exception("EnemyStatHandler was updated before being initialized");
            if (_status.Experience >= _status.MaxExperience) HandleLevelUp();
        }

        private void HandleLevelUp()
        {
            _status.Level++;
            _status.Experience -= _status.MaxExperience;
            if (_status.Experience < 0) _status.Experience = 0;
        }

        private void Initialize(CharacterStatus stats)
        {
            _status = stats;
            Initialized = true;
        }

        public void Damage(int i)
        {
            if (!_status.Invulnerable) _status.Health -= i;
        }
    }
}
