using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Broken.Scripts.Common;
using System.Collections.Generic;
using System;
using Broken.Scripts.Models;

namespace Broken
{
    public static class DevManager
    {

        private static Texture2D _singlePixelTexture;

        public static void Initialize()
        {
            _singlePixelTexture = new Texture2D(OutputManager.GraphicsDeviceManager.GraphicsDevice, 1, 1);
            Color[] colorData = new Color[1];
            colorData[0] = Color.White;
            _singlePixelTexture.SetData(colorData);
        }

        public static void DrawRectangle(SpriteBatch spriteBatch, Rectangle rect, Color color)
        {
            spriteBatch.Draw(_singlePixelTexture, rect, color);
        }

        public static void DrawRectangle(SpriteBatch spriteBatch, BoundingRectangle rectCol, Color color)
        {
            spriteBatch.Draw(_singlePixelTexture, new Rectangle((int)rectCol.X, (int)rectCol.Y, (int)rectCol.Width, (int)rectCol.Height), color);
        }
    }
}
