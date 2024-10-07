using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Broken.Entities;
using Broken.UI;

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
        public CharacterList CurrentCharacters = new();
        private CharacterList DeadCharacters = new();
        public List<DeathAnimator> DeathAnimations = new();
        private List<DeathAnimator> FinishedAnimations = new();
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

        bool _isActive;
        Player _player;
        PlayerHUD _playerHUD = new();
        Room _currentRoom;
        ExpSystem _expSystem = new(2000);

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
            _expSystem.LoadContent(game);
        }

        public void StartGame()
        {
            _isActive = true;
            OutputManager.Camera.ForceZoom(.7f);
            OutputManager.Camera.CanZoom = true;
        }

        public void CharacterDeath(Character character)
        {

            for (int i = 0; i < character.Status.Experience; ++i)
            {
                _expSystem.GenerateExperience(character.CenterFloorPosition, 0.5f);
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
            if (!_isActive) return;
            DeadCleanUp(gameTime);
            OutputManager.Camera.Follow(_player);
            _currentRoom.Update(gameTime);
            CurrentCharacters.Update(gameTime);
            _expSystem.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if(!_isActive) return;
            _currentRoom.Draw(spriteBatch);
            CurrentCharacters.Draw(spriteBatch);
            _playerHUD.Draw(spriteBatch);
            _expSystem.Draw(spriteBatch);
            DrawDeathAnimations(spriteBatch);

            DevManager.DrawCharacterInstructions(spriteBatch);
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
