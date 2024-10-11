using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

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

        public static bool HoldingSpace;
        public static bool HoldingEscape;
        public static bool HoldingShift;

        public static bool IsHolding => HoldingDown || HoldingLeft || HoldingUp || HoldingRight;

        public static bool PressedDown;
        public static bool PressedUp;
        public static bool PressedLeft;
        public static bool PressedRight;

        public static bool PressedSpace;
        public static bool PressedEscape;

        private static KeyboardState _priorKeyboardState;
        private static KeyboardState _currentKeyboardState;
        private static MouseState _currentMouseState;
        private static MouseState _priorMouseState;

        public static Direction GetMouseDirection(Vector2 playerPosition)
        {
            MouseState mouseState = Mouse.GetState();
            Vector2 mouseScreenPosition = new Vector2(mouseState.X, mouseState.Y);
            Vector2 mouseWorldPosition = ScreenToWorld(mouseScreenPosition, OutputManager.Camera.Transform);
            return DirectionHelper.GetDirection(playerPosition, mouseWorldPosition);
        }

        public static Direction GetKeyboardDirection(Direction oldDir)
        {
            Direction direction = oldDir;
            if (HoldingLeft) direction = Direction.Left;
            if (HoldingRight) direction = Direction.Right;
            if (HoldingUp) direction = Direction.Up;
            if (HoldingDown) direction = Direction.Down;
            if (HoldingLeft && HoldingUp) direction = Direction.ULeft;
            if (HoldingRight && HoldingUp) direction = Direction.URight;
            if (HoldingLeft && HoldingDown) direction = Direction.DLeft;
            if (HoldingRight && HoldingDown) direction = Direction.DRight;
            return direction;
        }

        private static Vector2 ScreenToWorld(Vector2 screenPosition, Matrix cameraTransform)
        {
            Matrix inverseTransform = Matrix.Invert(cameraTransform);

            Vector2 worldPosition = Vector2.Transform(screenPosition, inverseTransform);
            return worldPosition;
        }

        public static void Update(Game game)
        {

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

            //Holding button
            HoldingDown = _currentKeyboardState.IsKeyDown(Keys.S) || _currentKeyboardState.IsKeyDown(Keys.Down);
            HoldingUp = _currentKeyboardState.IsKeyDown(Keys.W) || _currentKeyboardState.IsKeyDown(Keys.Up);
            HoldingLeft = _currentKeyboardState.IsKeyDown(Keys.A) || _currentKeyboardState.IsKeyDown(Keys.Left);
            HoldingRight = _currentKeyboardState.IsKeyDown(Keys.D) || _currentKeyboardState.IsKeyDown(Keys.Right);

            HoldingSpace = _currentKeyboardState.IsKeyDown(Keys.Space);
            HoldingShift = _currentKeyboardState.IsKeyDown(Keys.LeftShift) || _currentKeyboardState.IsKeyDown(Keys.RightShift);
            HoldingEscape = _currentKeyboardState.IsKeyDown(Keys.Escape);

            //Pressing button
            PressedDown = HoldingDown && _priorKeyboardState.IsKeyUp(Keys.S) && _priorKeyboardState.IsKeyUp(Keys.Down);
            PressedUp = HoldingUp && _priorKeyboardState.IsKeyUp(Keys.W) && _priorKeyboardState.IsKeyUp(Keys.Up);
            PressedRight = HoldingRight && _priorKeyboardState.IsKeyUp(Keys.Right) && _priorKeyboardState.IsKeyUp(Keys.D);
            PressedLeft = HoldingLeft && _priorKeyboardState.IsKeyUp(Keys.Left) && _priorKeyboardState.IsKeyUp(Keys.A);

            PressedSpace = HoldingSpace && _priorKeyboardState.IsKeyUp(Keys.Space);
            PressedEscape = HoldingEscape && _priorKeyboardState.IsKeyUp(Keys.Escape);


            #endregion
        }
    }
}
