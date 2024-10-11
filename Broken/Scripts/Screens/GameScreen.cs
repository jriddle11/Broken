using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Broken.Entities;
using Broken.UI;
using Broken.Audio;

namespace Broken
{
    /// <summary>
    /// Controller for the main game
    /// </summary>
    public class GameScreen
    {
        private static GameScreen _instance;
        public static GameScreen Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameScreen();
                }
                return _instance;
            }
        }

        public bool IsActive;
        public bool Paused;

        public CharacterList CurrentCharacters = new();
        private CharacterList DeadCharacters = new();

        public List<DeathAnimator> DeathAnimations = new();
        private List<DeathAnimator> FinishedAnimations = new();

        public ExpSystem ExpSystem = new(1000);
        public Player GetPlayer
        {
            get
            {
                return _player;
            }
        }
        public Room CurrentRoom
        {
            get
            {
                return _currentRoom;
            }
        }
        public Vector2 CurrentRoomSize
        {
            get
            {
                if (_currentRoom == null)
                {
                    return new Vector2(1900, 600);
                }
                else
                {
                    return _currentRoom.Bounds;
                }
            }
        }
        public Vector2 PlayerPosition
        {
            get
            {
                return _player.Position;
            }
        }

        Player _player;
        PlayerHUD _playerHUD = new();
        Room _currentRoom;

        public void LoadContent(Game game)
        {
            _currentRoom = new Room(game, 0);
            _player = new Player(new Vector2(1900, 1300));
            CurrentCharacters.Add(new Slime(new Vector2(1900, 1500), Color.LightGreen));
            CurrentCharacters.Add(new Slime(new Vector2(1900, 1900), Color.PaleVioletRed));
            CurrentCharacters.Add(new Slime(new Vector2(500, 500), Color.LightBlue));
            CurrentCharacters.Add(new Slime(new Vector2(3000, 500), Color.LightGreen));
            CurrentCharacters.Add(_player);
            CurrentCharacters.LoadContent(game);
            OutputManager.Camera.Boundaries = CurrentRoomSize;
            _playerHUD.LoadContent(game);
            ExpSystem.LoadContent(game);
        }

        public void StartGame()
        {
            IsActive = true;
            OutputManager.Camera.ForceZoom(.7f);
            OutputManager.Camera.CanZoom = true;
        }

        public void CharacterDeath(Character character)
        {

            for (int i = 0; i < character.Status.Experience; ++i)
            {
                ExpSystem.GenerateExperience(character.CenterFloorPosition, 0.5f);
            }
            DeadCharacters.Add(character);
            DeathAnimations.Add(new DeathAnimator(character.DrawRecord, character.Position));

        }

        private void DeadCleanUp(GameTime gameTime)
        {
            foreach(Character character in DeadCharacters)
            {
                CurrentCharacters.Remove(character);
            }
            foreach(var animator in DeathAnimations)
            {
                animator.Update(gameTime);
                if (animator.IsDone)
                {
                    FinishedAnimations.Add(animator);
                }
            }
            foreach(var animator in FinishedAnimations)
            {
                DeathAnimations.Remove(animator);
            }
            FinishedAnimations.Clear();
            DeadCharacters.Clear();
        }

        public void Update(GameTime gameTime)
        {
            if (!IsActive) return;
            if(Paused) return;
            DeadCleanUp(gameTime);
            OutputManager.Camera.Follow(_player);
            _currentRoom.Update(gameTime);
            CurrentCharacters.Update(gameTime);
            ExpSystem.Update(gameTime);
            CollisionHelper.ResolveCollisions(CurrentCharacters, CurrentRoom.RectangleColliders, 3);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if(!IsActive) return;
            _currentRoom.Draw(spriteBatch);
            CurrentCharacters.Draw(spriteBatch);
            ExpSystem.Draw(spriteBatch);
            DrawDeathAnimations(spriteBatch);

            DevManager.DrawCharacterInstructions(spriteBatch);

            _playerHUD.Draw(spriteBatch);
        }

        private void DrawDeathAnimations(SpriteBatch spriteBatch)
        {
            foreach(var animator in DeathAnimations) animator.Draw(spriteBatch);
        }

        public float GetRoomDepth(BoundingRectangle collider)
        {
            var height = Instance.CurrentRoomSize.Y;
            var center = collider.Y + collider.Height;

            float depth = 1f - (center / height);
            if (depth > .9f)
            {
                depth = .9f;
            }

            if (depth < .1f)
            {
                depth = .1f;
            }
            return depth;
        }
    }
}
