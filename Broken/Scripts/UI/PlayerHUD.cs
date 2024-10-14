using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Broken.Entities;

namespace Broken.UI
{
    public class PlayerHUD : IGameObject
    {
        public float Scale { get; set; } = .7f;

        TextureLibrary Textures = new();

        static Player Player => GameContext.Player;

        static int MaxHealthBars => Player.Status.MaxHealth;
        static int NumHealthBars => Player.Status.Health;

        static int MaxExp => Player.Status.MaxExperience;
        static int NumExp => Player.Status.Experience;

        Vector2 _barPosition = Vector2.Zero;
        Color _expColor = new Color(133, 45, 168);
        MenuText _currentLevel;
        Vector2 _lvlOffset = new Vector2(380, 30);

        public PlayerHUD()
        {
            _currentLevel = new MenuText("", Vector2.Zero, Color.WhiteSmoke * 0.7f, Color.WhiteSmoke) { Centered = false, Scale = 0.5f };
            _barPosition = new Vector2(50, 50);
        }

        public void LoadContent(Game game)
        {
            Textures.Add("healthbar", "My Assets/UI/Healthbar", game);
            Textures.Add("backbar", "My Assets/UI/backbar", game);
            Textures.Add("bar", "My Assets/UI/health", game);
            _currentLevel.LoadContent(game);
        }

        public void Update(GameTime gameTime)
        {
        }

        public void Draw(SpriteBatch spriteBatch, float opacity = 1)
        {
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.BackToFront);

            DrawLevelText(spriteBatch, opacity);
            DrawExpBar(spriteBatch);
            DrawHealthBars(spriteBatch);

            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.BackToFront, transformMatrix: OutputManager.Camera.Transform);
        }

        private void DrawLevelText(SpriteBatch spriteBatch, float opacity)
        {
            _currentLevel.Position = _barPosition + _lvlOffset;
            _currentLevel.Text = "LVL " + Player.Status.Level;
            _currentLevel.Draw(spriteBatch, opacity);
        }

        private void DrawHealthBars(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Textures["backbar"], _barPosition, null, Color.White, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0.02f);
            spriteBatch.Draw(Textures["healthbar"], _barPosition, null, Color.White, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0f);
            Texture2D healthTexture = Textures["bar"];
            Color color = GetHealthColor() * 0.8f;
            float spacing = 3f * Scale;

            float totalWidth = (Textures["healthbar"].Width - 20 - ((MaxHealthBars - 1) * spacing)) * Scale;
            float healthBarSegmentWidth = totalWidth / MaxHealthBars;

            for (int i = 0; i < NumHealthBars; i++)
            {
                Vector2 barPosition = new Vector2(_barPosition.X + 10 * Scale + (i * (healthBarSegmentWidth + spacing)), _barPosition.Y);

                spriteBatch.Draw(
                    healthTexture,
                    barPosition,
                    null,
                    color,
                    0f,
                    Vector2.Zero,
                    new Vector2(healthBarSegmentWidth / healthTexture.Width, Scale),
                    SpriteEffects.None,
                    0.01f
                );
            }
        }

        private void DrawExpBar(SpriteBatch spriteBatch)
        {
            Vector2 offset = new Vector2(0, 50 * Scale);
            spriteBatch.Draw(Textures["backbar"], _barPosition + offset, null, Color.White, 0f, Vector2.Zero, new Vector2(Scale, 0.3f * Scale), SpriteEffects.None, 0.02f);
            spriteBatch.Draw(Textures["healthbar"], _barPosition + offset, null, Color.Black, 0f, Vector2.Zero, new Vector2(Scale, 0.3f * Scale), SpriteEffects.None, 0f);

            Texture2D expTexture = Textures["bar"];

            float expRatio = NumExp / (float)MaxExp;
            float expBarWidth = (Textures["healthbar"].Width - 20) * expRatio * Scale;

            spriteBatch.Draw(
                expTexture,
                _barPosition + offset,
                null,
                _expColor,
                0f,
                Vector2.Zero,
                new Vector2(expBarWidth / expTexture.Width, 0.3f * Scale),
                SpriteEffects.None,
                0.015f
            );
        }

        private Color GetHealthColor()
        {
            return (NumHealthBars / (float)MaxHealthBars < 0.5f) ? Color.Red : Color.LightGreen;
        }
    }

}
