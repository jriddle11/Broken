using Microsoft.Xna.Framework;

namespace Broken.Scripts.MainGame
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
