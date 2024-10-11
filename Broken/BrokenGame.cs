using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Broken.Audio;

namespace Broken
{
    public class BrokenGame : Game
    {
        public bool DebugMode = false;

        private SpriteBatch _spriteBatch;
        private MainMenuScreen _mainMenuScreen;
        private GameScreen _gameScreen;
        private PauseScreen _pauseScreen;

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
            SoundEffectPlayer.Initialize(this);

            _mainMenuScreen = MainMenuScreen.Instance;
            _gameScreen = GameScreen.Instance;
            _pauseScreen = PauseScreen.Instance;
             
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _mainMenuScreen.LoadContent(this);
            _gameScreen.LoadContent(this);
            _pauseScreen.LoadContent(this);

            if (DebugMode)
            {
                HandleStartGame();
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (InputManager.PressedEscape && _pauseScreen.IsActive)
            {
                if (_gameScreen.Paused)
                {
                    HandleUnpauseGame();
                }
                else
                {
                    HandlePauseGame();
                }
            }

            InputManager.Update(this);
            MusicPlayer.Update(gameTime);

            _mainMenuScreen.Update(this, gameTime);
            _gameScreen.Update(gameTime);
            _pauseScreen.Update(this, gameTime);

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
            _spriteBatch.Begin(sortMode: SpriteSortMode.BackToFront);
            _pauseScreen.Draw(_spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        public void HandleStartGame()
        {
            _mainMenuScreen.IsActive = false;
            _pauseScreen.IsActive = true;
            _gameScreen.StartGame();
        }

        public void HandlePauseGame()
        {
            _gameScreen.Paused = true;
            _pauseScreen.Paused = true;
        }

        public void HandleUnpauseGame()
        {
            _gameScreen.Paused = false;
            _pauseScreen.Paused= false;
        }

        public void HandleBackToMenu()
        {
            _gameScreen.IsActive = false;
            _pauseScreen.IsActive = false;
            _mainMenuScreen.IsActive = true;
        }
    }
}