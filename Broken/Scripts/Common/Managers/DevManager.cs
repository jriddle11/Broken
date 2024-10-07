using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;
using Broken.Entities;

namespace Broken
{
    public static class DevManager
    {

        static Texture2D _singlePixelTexture;
        static Texture2DList Textures = new();

        const int CIRCLE_RADIUS = 7;

        public static void Initialize(Game game)
        {
            LoadContent(game);
        }

        public static void LoadContent(Game game)
        {
            Textures.Add("quit", "My Assets/Menu/Instructions", game);
            Textures.Add("start", "My Assets/Menu/ClickStart", game);
            Textures.Add("character", "My Assets/Menu/CharacterInstructions", game);
            Textures.Add("circle", "My Assets/Dev/circle", game);
            _singlePixelTexture = new Texture2D(OutputManager.GraphicsDeviceManager.GraphicsDevice, 1, 1);
            Color[] colorData = new Color[1];
            colorData[0] = Color.White;
            _singlePixelTexture.SetData(colorData);
        }

        public static void DrawRectangle(SpriteBatch spriteBatch, Rectangle rect, Color color)
        {
            spriteBatch.Draw(_singlePixelTexture, rect, color);
        }

        public static void DrawRectangle(SpriteBatch spriteBatch, Vector2 position, Rectangle rect, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effect)
        {

            spriteBatch.Draw(_singlePixelTexture, position, rect, color, rotation, origin, scale, effect, .01f);

        }

        public static void DrawRectangle(SpriteBatch spriteBatch, BoundingRectangle rectCol, Color color)
        {
            spriteBatch.Draw(_singlePixelTexture, new Rectangle((int)rectCol.X, (int)rectCol.Y, (int)rectCol.Width, (int)rectCol.Height), color);
        }

        public static void DrawCircle(SpriteBatch spriteBatch, BoundingCircle col, Color color)
        {
            Texture2D circleTexture = Textures["circle"];

            // Calculate the position to draw the circle texture
            Vector2 origin = new Vector2(circleTexture.Width / 2f, circleTexture.Height / 2f);
            Vector2 position = col.Center - origin; // Adjust position to center the circle

            // Set the width and height based on the BoundingCircle's radius
            Rectangle sourceRectangle = new Rectangle(0, 0, circleTexture.Width, circleTexture.Height);
            Vector2 size = new Vector2(col.Radius * 2, col.Radius * 2); // Diameter for width and height

            // Draw the texture using the size as destination rectangle
            spriteBatch.Draw(circleTexture, position, sourceRectangle, color, 0f, origin, size / new Vector2(circleTexture.Width, circleTexture.Height), SpriteEffects.None, 0f);

        }

        public static void DrawMenuInstructions(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Textures["quit"], new Vector2(0, 600), Color.White);
            spriteBatch.Draw(Textures["start"], new Vector2(0, 300), Color.White);
        }

        public static void DrawCharacterInstructions(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Textures["character"], new Vector2(1400, 1300), null, Color.LightBlue * 0.3f, 0f, Vector2.Zero, 1f, SpriteEffects.None, .98f);
        }
    }
}
