
namespace Broken.Entities
{
    public class CharacterStatus
    {
        public int Health = 6;
        public int Level = 1;
        public int Experience = 0;
        public string Name;
        public bool IsAlive => Health > 0;
        public bool Invulnerable = false;

        public int MaxHealth = 6;
        public int MaxExperience = 10;
    }
}
