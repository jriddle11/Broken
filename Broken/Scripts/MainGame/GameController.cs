using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Broken.Scripts.MainGame;
using Broken.Scripts.Common;
using System.Linq;
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
        public List<Character> CurrentActiveEntities
        {
            get
            {
                if(_currentRoom == null) return null;
                return _currentRoom.Entities;
            }
        }
        public Vector2 CurrentRoomSize
        {
            get
            {
                return GetCurrentRoomSize();
            }
            set
            {
                CurrentRoomSize = value;
            }
        }
        public Vector2 PlayerPosition
        {
            get
            {
                return GetPlayerPosition();
            }
        }
        public Player GetPlayer
        {
            get
            {
                return _player;
            }
        }

        bool _isActive;
        Player _player;
        Room _currentRoom;
        Slime _slime;

        Texture2D _instructions;

        public void LoadContent(Game game)
        {
            _currentRoom = new Room(game, 0);
            _player = new Player();
            _player.Position = new Vector2(1900, 600);
            _instructions = game.Content.Load<Texture2D>("My Assets/Menu/CharacterInstructions");
            //_slime = new Slime();
            //_slime.Position = new Vector2(1900, 1500);
            _player.LoadContent(game);
            //_slime.LoadContent(game);
            _currentRoom.Entities.Add(_player);
            //_currentRoom.Entities.Add(_slime);
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
            _player.Update(gameTime, _currentRoom.RectangleColliders);
            //_slime.Update(gameTime, _currentRoom.RectangleColliders);
            OutputManager.Camera.Follow(_player);
            _currentRoom.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if(!_isActive) return;
            _player.Draw(spriteBatch);
            _currentRoom.Draw(spriteBatch);
            //_slime.Draw(spriteBatch);
            spriteBatch.Draw(_instructions, _player.Position - new Vector2(600, 0), Color.White);
        }

        private Vector2 GetPlayerPosition()
        {
            return _player.Position;
        }
        
        private Vector2 GetCurrentRoomSize()
        {
            if(_currentRoom == null)
            {
                return new Vector2(1900, 600);
            }
            else
            {
                return _currentRoom.Bounds;
            }
        }
    }
}
