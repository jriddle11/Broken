using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Broken
{
    public static class InputManager
    {
        //Mouse
        public static bool Mouse1Clicked;

        //Keyboard
        public static bool HoldingUp;
        public static bool HoldingDown;
        public static bool HoldingLeft;
        public static bool HoldingRight;

        public static bool PressedDown;
        public static bool PressedUp;
        public static bool PressedLeft;
        public static bool PressedRight;

        private static KeyboardState _priorKeyboardState;
        private static KeyboardState _currentKeyboardState;
        private static MouseState _currentMouseState;
        private static MouseState _priorMouseState;

        public static void Update(Game game)
        {
            #region Temporary emergency escape

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                game.Exit();

            #endregion

            #region Mouse

            _priorMouseState = _currentMouseState;
            _currentMouseState = Mouse.GetState();

            Mouse1Clicked = false;
            if (_currentMouseState.LeftButton == ButtonState.Pressed && _priorMouseState.LeftButton == ButtonState.Released)
            {
                Mouse1Clicked = true;
            }
            else
            {
                Mouse1Clicked = false;
            }

            #endregion

            #region Keyboard

            _priorKeyboardState = _currentKeyboardState;
            _currentKeyboardState = Keyboard.GetState();

            HoldingDown = _currentKeyboardState.IsKeyDown(Keys.S) || _currentKeyboardState.IsKeyDown(Keys.Down);
            HoldingUp = _currentKeyboardState.IsKeyDown(Keys.W) || _currentKeyboardState.IsKeyDown(Keys.Up);
            HoldingLeft = _currentKeyboardState.IsKeyDown(Keys.A) || _currentKeyboardState.IsKeyDown(Keys.Left);
            HoldingRight = _currentKeyboardState.IsKeyUp(Keys.D) || _currentKeyboardState.IsKeyDown(Keys.Right);

            PressedDown = HoldingDown && _priorKeyboardState.IsKeyUp(Keys.S) && _priorKeyboardState.IsKeyUp(Keys.Down);
            PressedUp = HoldingUp && _priorKeyboardState.IsKeyUp(Keys.W) && _priorKeyboardState.IsKeyUp(Keys.Up);
            PressedRight = HoldingRight && _priorKeyboardState.IsKeyUp(Keys.Right) && _priorKeyboardState.IsKeyUp(Keys.D);
            PressedLeft = HoldingLeft && _priorKeyboardState.IsKeyUp(Keys.Left) && _priorKeyboardState.IsKeyUp(Keys.A);

            #endregion
        }
    }
}
