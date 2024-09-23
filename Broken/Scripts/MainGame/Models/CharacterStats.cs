using Broken.Scripts.Interfaces;

namespace Broken.Scripts.MainGame
{
    public class CharacterStats
    {
        public int Health = 100;
        public int Level = 1;
        public int Experience = 0;
        public string Name;
        public bool IsAlive => Health > 0;

        public int MaxHealth = 100;
        public int MaxExperience = 10;

        private IStatHandler _statHandler;

        public CharacterStats(IStatHandler handler)
        {
            _statHandler = handler;
        }

        public void Update()
        {
            if (_statHandler == null) return;
            if(Experience > MaxExperience)
            {
                _statHandler.HandleLevelUp(this);
            }
        }

    }
}
