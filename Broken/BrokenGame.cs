using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Broken.Scripts.Common;
using Broken.Scripts.MainGame;

namespace Broken
{
    public class BrokenGame : Game
    {
        private SpriteBatch _spriteBatch;
        private MainMenuController _mainMenu;
        private GameController _gameController;

        public BrokenGame()
        {
            OutputManager.GraphicsDeviceManager = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            OutputManager.Initialize(this);

            _mainMenu = new MainMenuController();
            _gameController = GameController.Instance;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _mainMenu.LoadContent(this);
            _gameController.LoadContent(this);
        }

        protected override void Update(GameTime gameTime)
        {
            InputManager.Update(this);

            _mainMenu.Update(this, gameTime);
            _gameController.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(19,19,19));
            OutputManager.Camera.Boundaries = _gameController.CurrentRoomSize;

            _spriteBatch.Begin(SpriteSortMode.BackToFront, transformMatrix: OutputManager.Camera.Transform);
            _mainMenu.Draw(_spriteBatch);
            _gameController.Draw(_spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        public void HandleStartGame()
        {
            _mainMenu.MenuIsActive = false;
            _gameController.StartGame();
        }
    }
}