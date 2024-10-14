using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Broken.Entities
{
    public class GameContextHandler
    {
        private static ExperienceSystem _expSystem = new(1000);
        private static CharacterList _deadCharacters = new();

        private static List<DeathAnimator> _deathAnimations = new();
        private static List<DeathAnimator> _finishedAnimations = new();


        public static void Kill(Character character)
        {
            for (int i = 0; i < character.Status.Experience; ++i)
            {
                _expSystem.GenerateExperience(character.CenterFloorPosition, 0.5f);
            }
            _deadCharacters.Add(character);
            _deathAnimations.Add(new DeathAnimator(character.DrawRecord, character.Position));

        }

        public void LoadContent(Game game)
        {
            _expSystem.LoadContent(game);
        }

        public void Update(GameTime gameTime)
        {
            DeadCleanUp(gameTime);

            GameContext.CurrentRoom.Update(gameTime);
            GameContext.ActiveCharacters.Update(gameTime);

            _expSystem.Update(gameTime);

            CollisionHelper.ResolveCollisions(GameContext.ActiveCharacters, GameContext.CurrentRoom.RectangleColliders, 3);
        }

        private void DeadCleanUp(GameTime gameTime)
        {
            foreach (Character character in _deadCharacters)
            {
                GameContext.ActiveCharacters.Remove(character);
            }
            foreach (var animator in _deathAnimations)
            {
                animator.Update(gameTime);
                if (animator.IsDone)
                {
                    _finishedAnimations.Add(animator);
                }
            }
            foreach (var animator in _finishedAnimations)
            {
                _deathAnimations.Remove(animator);
            }
            _finishedAnimations.Clear();
            _deadCharacters.Clear();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            GameContext.CurrentRoom.Draw(spriteBatch);
            GameContext.ActiveCharacters.Draw(spriteBatch);

            _expSystem.Draw(spriteBatch);

            DrawDeathAnimations(spriteBatch);

        }

        private void DrawDeathAnimations(SpriteBatch spriteBatch)
        {
            foreach (var animator in _deathAnimations) animator.Draw(spriteBatch);
        }
    }
}
