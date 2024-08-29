using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Broken
{
    public class BrokenGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private MainMenuController _mainMenu;
        private GameController _gameController;

        public BrokenGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _graphics.HardwareModeSwitch = false;
            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
            Window.IsBorderless = true;
            _graphics.ApplyChanges();

            _mainMenu = new MainMenuController(_graphics);
            _gameController = new GameController(_graphics);

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

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(19,19,19));

            _spriteBatch.Begin();
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