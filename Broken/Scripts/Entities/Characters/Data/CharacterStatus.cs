
namespace Broken.Entities
{
    public class CharacterStatus
    {
        public string Name;
        public int Level = 1;

        public int Health = 6;
        public int MaxHealth = 6;

        public int Experience = 9;
        public int MaxExperience = 10;
        public float ExperienceGatherRange = 200;
        public bool IsAlive => Health > 0;
        public bool Invulnerable = false;
    }
}
