using Microsoft.Xna.Framework;

namespace Broken.Entities
{
    public struct EnemyState
    {
        public bool LowHealth = false;
        public bool InRangeToAttack = false;
        public bool OutOfPlayerAttackRange = false;
        public bool AwareOfPlayer = false;

        public EnemyState()
        {

        }
    }
}
