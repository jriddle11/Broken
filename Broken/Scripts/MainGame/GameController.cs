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
        public Vector2 CurrentRoomSize = new Vector2(1000, 1000);
        bool _isActive;
        Player _player;
        Room _currentRoom;

        public GameController()
        {
            _player = new Player();
            _player.Position = new Vector2(1900, 600);
        }

        public void LoadContent(Game game)
        {
            _player.LoadContent(game);
            _currentRoom = new Room(game, 0);
            CurrentRoomSize = _currentRoom.Bounds;
            OutputManager.Camera.Boundaries = CurrentRoomSize;
        }

        public void StartGame()
        {
            _isActive = true;
        }

        public void Update(GameTime gameTime)
        {
            if (!_isActive) return;
            _player.Update(gameTime, _currentRoom.WallColliders);
            OutputManager.Camera.Follow(_player);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if(!_isActive) return;
            _player.Draw(spriteBatch);
            _currentRoom.Draw(spriteBatch);
        }
    }
}
