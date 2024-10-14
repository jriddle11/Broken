using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Broken.Entities;
using Broken.UI;
using Broken.Audio;

namespace Broken
{
    /// <summary>
    /// Controller for the main game
    /// </summary>
    public class GameScreen
    {
        public bool IsActive;
        public bool Paused;

        GameContextHandler _contextHandler = new();
        PlayerHUD _playerHUD = new();

        public void LoadContent(Game game)
        {
            GameContext.CurrentRoom = new Room(game, 0);
            GameContext.Player = new Player(new Vector2(1900, 1300));
            GameContext.ActiveCharacters.Add(new Slime(new Vector2(1900, 1500), Color.LightGreen));
            GameContext.ActiveCharacters.Add(new Slime(new Vector2(1900, 1900), Color.PaleVioletRed));
            GameContext.ActiveCharacters.Add(new Slime(new Vector2(500, 500), Color.LightBlue));
            GameContext.ActiveCharacters.Add(new Slime(new Vector2(3000, 500), Color.LightGreen));
            GameContext.ActiveCharacters.Add(GameContext.Player);
            GameContext.ActiveCharacters.LoadContent(game);
            OutputManager.Camera.Boundaries = GameContext.CurrentRoomBounds;
            _playerHUD.LoadContent(game);
            _contextHandler.LoadContent(game);
        }

        public void StartGame()
        {
            IsActive = true;
            OutputManager.Camera.ForceZoom(.7f);
            OutputManager.Camera.CanZoom = false;
        }

        public void Update(GameTime gameTime)
        {
            if (!IsActive) return;
            if(Paused) return;
            OutputManager.Camera.Follow(GameContext.Player);
            _contextHandler.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if(!IsActive) return;

            DevManager.DrawCharacterInstructions(spriteBatch);

            _contextHandler.Draw(spriteBatch);
            _playerHUD.Draw(spriteBatch);
        }
    }
}
