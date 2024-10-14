using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Broken.Entities;
using Microsoft.Xna.Framework.Graphics;

namespace Broken
{
    public class GameController
    {

        private MainMenuScreen _mainMenuScreen;
        private GameScreen _gameScreen;
        private PauseScreen _pauseScreen;

        public GameController()
        {
            _mainMenuScreen = new();
            _mainMenuScreen.StartGame += HandleStartGame;
            _mainMenuScreen.Quit += HandleQuit;

            _gameScreen = new();

            _pauseScreen = new();
            _pauseScreen.Resume += HandleUnpauseGame;
            _pauseScreen.Pause += HandlePauseGame;
            _pauseScreen.Quit += HandleQuit;
        }

        public void LoadContent(Game game)
        {
            _mainMenuScreen.LoadContent(game);
            _gameScreen.LoadContent(game);
            _pauseScreen.LoadContent(game);
        }

        public void Update(GameTime gameTime)
        {
            _mainMenuScreen.Update(gameTime);
            _gameScreen.Update(gameTime);
            _pauseScreen.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            OutputManager.Camera.Boundaries = GameContext.CurrentRoomBounds;
            spriteBatch.Begin(SpriteSortMode.BackToFront, transformMatrix: OutputManager.Camera.Transform);
            _gameScreen.Draw(spriteBatch);
            spriteBatch.End();
            spriteBatch.Begin(sortMode: SpriteSortMode.BackToFront);
            _pauseScreen.Draw(spriteBatch);
            _mainMenuScreen.Draw(spriteBatch);
            spriteBatch.End();
        }

        public void HandleStartGame()
        {
            _mainMenuScreen.IsActive = false;
            _pauseScreen.IsActive = true;
            _gameScreen.StartGame();
        }

        public void HandleQuit()
        {
            BrokenGame.Instance.Exit();
        }

        public void HandlePauseGame()
        {
            _gameScreen.Paused = true;
            _pauseScreen.Paused = true;
        }

        public void HandleUnpauseGame()
        {
            _gameScreen.Paused = false;
            _pauseScreen.Paused = false;
        }

        public void HandleBackToMenu()
        {
            _gameScreen.IsActive = false;
            _pauseScreen.IsActive = false;
            _mainMenuScreen.IsActive = true;
        }

        public void HandleDebugStart()
        {
            HandleStartGame();
        }
    }
}
