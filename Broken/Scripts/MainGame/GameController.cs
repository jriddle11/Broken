using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Broken.Scripts.MainGame;
using Broken.Scripts.Common;

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

        private GameController()
        {
            _player = new Player();
            _player.Position = new Vector2(1900, 600);
        }

        public void LoadContent(Game game)
        {
            _player.LoadContent(game);
            _currentRoom = new Room(game, 0);
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
            _player.Update(gameTime, _currentRoom.WallColliders);
            OutputManager.Camera.Follow(_player);
            _currentRoom.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if(!_isActive) return;
            _player.Draw(spriteBatch);
            _currentRoom.Draw(spriteBatch);
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
