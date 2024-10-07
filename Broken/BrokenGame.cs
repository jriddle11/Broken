using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Broken.Audio;

namespace Broken
{
    public class BrokenGame : Game
    {
        public bool DebugMode = true;

        private SpriteBatch _spriteBatch;
        private MainMenuScreen _mainMenuScreen;
        private GameScreen _gameScreen;

        public BrokenGame()
        {
            OutputManager.GraphicsDeviceManager = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            OutputManager.Initialize(this);
            DevManager.Initialize(this);
            MusicPlayer.Initialize(this);

            _mainMenuScreen = MainMenuScreen.Instance;
            _gameScreen = GameScreen.Instance;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _mainMenuScreen.LoadContent(this);
            _gameScreen.LoadContent(this);

            if (DebugMode)
            {
                HandleStartGame();
            }
        }

        protected override void Update(GameTime gameTime)
        {
            InputManager.Update(this);
            MusicPlayer.Update(gameTime);

            _mainMenuScreen.Update(this, gameTime);
            _gameScreen.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(19,19,19));
            OutputManager.Camera.Boundaries = _gameScreen.CurrentRoomSize;

            _spriteBatch.Begin(SpriteSortMode.BackToFront, transformMatrix: OutputManager.Camera.Transform);
            _mainMenuScreen.Draw(_spriteBatch);
            _gameScreen.Draw(_spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        public void HandleStartGame()
        {
            MusicPlayer.SlowStop(1.5f);
            _mainMenuScreen.IsActive = false;
            _gameScreen.StartGame();
        }
    }
}