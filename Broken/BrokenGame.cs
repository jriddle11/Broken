using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Broken.Audio;

namespace Broken
{
    public class BrokenGame : Game
    {
        public bool DebugMode = false;

        public static BrokenGame Instance;

        private SpriteBatch _spriteBatch;
        private GameController _controller;

        public BrokenGame()
        {
            Instance = this;
            OutputManager.GraphicsDeviceManager = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            OutputManager.LoadContent(this);
            DevManager.LoadContent(this);
            MusicPlayer.LoadContent(this);
            SoundEffectPlayer.LoadContent(this);

            _controller = new GameController();
            _controller.LoadContent(this);

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            if (DebugMode)
            {
                _controller.HandleDebugStart();
            }
        }

        protected override void Update(GameTime gameTime)
        {
            InputManager.Update(this);
            MusicPlayer.Update(gameTime);

            _controller.Update(gameTime);
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(19,19,19));

            _controller.Draw(_spriteBatch);

            base.Draw(gameTime);
        }
    }
}