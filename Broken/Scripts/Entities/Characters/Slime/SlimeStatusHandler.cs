using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Broken.Entities
{
    public class SlimeStatusHandler : EnemyStatusHandler
    {
        public SlimeStatusHandler(EnemyDifficulty difficulty, CharacterStatus stats)
        {
            Difficulty = difficulty;
            Initialize(stats);
        }

        public override void HandleLevelUp()
        {

        }

        public override void Initialize(CharacterStatus stats)
        {
            base.Initialize(stats);
            Setup();
        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void Setup()
        {
            switch (Difficulty)
            {
                case EnemyDifficulty.Easy:
                    Status.MaxHealth = 6;
                    Status.Experience = 4;
                    break;
                case EnemyDifficulty.Medium:
                    Status.MaxHealth = 9;
                    break;
                case EnemyDifficulty.Hard:
                    Status.MaxHealth = 12;
                    break;
                case EnemyDifficulty.Nightmare:
                    Status.MaxHealth = 18;
                    break;
            }
            Status.Health = Status.MaxHealth;
        }
    }
}
