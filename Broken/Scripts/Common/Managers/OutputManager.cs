﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Broken
{
    public static class OutputManager
    {
        public static int ScreenWidth => GraphicsDeviceManager.GraphicsDevice.Viewport.Width;
        public static int ScreenHeight => GraphicsDeviceManager.GraphicsDevice.Viewport.Height;

        public static GraphicsDeviceManager GraphicsDeviceManager;

        public static Camera Camera;

        public static void LoadContent(Game game)
        {
            GraphicsDeviceManager.PreferredBackBufferWidth = 1920;
            GraphicsDeviceManager.PreferredBackBufferHeight = 1080;
            game.Window.IsBorderless = true;
            GraphicsDeviceManager.ApplyChanges();
            Camera = new Camera();
        }
    }
}
