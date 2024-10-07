using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Broken.UI
{
    public class MenuText : IGameObject
    {
        public string Text;
        public Vector2 Position;
        public Color FontColor = Color.Black;
        public Color HighLightColor = Color.Black;
        public bool IsHighlighted = false;
        public float Scale = 1f;
        public float LastTimeHovered;

        private SpriteFont _jorvikFont;

        public MenuText(string text, Vector2 position, Color fontColor, Color highlightColor)
        {
            Text = text;
            Position = position;
            FontColor = fontColor;
            HighLightColor = highlightColor;
        }

        public bool IsMouseHovering(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();
            Vector2 textSize = _jorvikFont.MeasureString(Text) * 1.1f;
            Vector2 halfSize = textSize / 2;
            Rectangle textRectangle = new Rectangle((int)(Position.X - halfSize.X), (int)(Position.Y - halfSize.Y), (int)textSize.X, (int)textSize.Y);

            return textRectangle.Contains(mouseState.X, mouseState.Y);
        }

        public void LoadContent(Game game)
        {
            _jorvikFont = game.Content.Load<SpriteFont>("Jorvik Informal");
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch, float opacity = 1f)
        {
            Color color = IsHighlighted ? HighLightColor : FontColor;
            color *= opacity;
            Vector2 textSize = _jorvikFont.MeasureString(Text);
            spriteBatch.DrawString(_jorvikFont, Text, Position, color, 0f, textSize / 2, Scale, SpriteEffects.None, 0f);
        }
    }
}
