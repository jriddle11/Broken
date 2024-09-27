using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Broken.Scripts.MainGame;
using Broken.Scripts;
using System;
using System.Collections.Generic;

namespace Broken.Scripts.MainGame
{
    /// <summary>
    /// Controller for the main game
    /// </summary>
    public class GameController
    {
        private static GameController _instance;

        public static GameController Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameController();
                }
                return _instance;
            }
        }
        public EntityList CurrentEntities = new();
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
        Room _currentRoom;

        public void LoadContent(Game game)
        {
            _currentRoom = new Room(game, 0);
            _player = new Player(new Vector2(1900, 1300));
            CurrentEntities.Add(new Slime(new Vector2(1900, 1500), Color.LightGreen));
            CurrentEntities.Add(new Slime(new Vector2(1900, 1900), Color.PaleVioletRed));
            CurrentEntities.Add(new Slime(new Vector2(500, 500), Color.LightBlue));
            CurrentEntities.Add(new Slime(new Vector2(3000, 500), Color.LightGreen));
            CurrentEntities.Add(_player);
            CurrentEntities.LoadContent(game);
            OutputManager.Camera.Boundaries = CurrentRoomSize;
        }

        public void StartGame()
        {
            _isActive = true;
            OutputManager.Camera.ForceZoom(.7f);
            OutputManager.Camera.CanZoom = true;
        }

        public void Update(GameTime gameTime)
        {
            if (!_isActive) return;
            OutputManager.Camera.Follow(_player);
            _currentRoom.Update(gameTime);
            CurrentEntities.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if(!_isActive) return;
            _currentRoom.Draw(spriteBatch);
            CurrentEntities.Draw(spriteBatch);
            DevManager.DrawCharacterInstructions(spriteBatch);
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
