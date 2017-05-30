using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOLD
{
    static class Input
    {
        public static Camera camera = null;
        public static KeyboardState currentKey;
        public static KeyboardState prevKey;

        public static MouseState currentMouseState;
        public static MouseState prevMouseState;

        public static Point MousePosition { get { return currentMouseState.Position; } }

        public static Vector2 WorldMousePosition
        {
            get
            {
                if (camera == null)
                {
                    throw new InvalidOperationException("Camera is not set correctly!");
                }
                return camera.ScreenToWorld(MousePosition.ToVector2());
            }
        }
        public static void Update()
        {
            prevKey = currentKey;
            currentKey = Keyboard.GetState();

            prevMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();
        }

        public static bool isLeftMouseDown()
        {
            return (currentMouseState.LeftButton == ButtonState.Pressed);
        }

        public static bool wasLeftMouseDown()
        {
            return (prevMouseState.LeftButton == ButtonState.Pressed);
        }

        public static bool isMouseJustClicked()
        {
            return (isLeftMouseDown() && !wasLeftMouseDown());
        }
        public static bool isMouseJustReleased()
        {
            return (!isLeftMouseDown()) && wasLeftMouseDown();
        }

        public static bool isDown(Keys key)
        {
            return currentKey.IsKeyDown(key);
        }
        public static bool isUp(Keys key)
        {
            return currentKey.IsKeyUp(key);
        }
        public static bool wasUp(Keys key)
        {
            return prevKey.IsKeyUp(key);
        }
        public static bool wasDown(Keys key)
        {
            return prevKey.IsKeyDown(key);
        }
        public static bool justPressed(Keys key)
        {
            return isDown(key) && wasUp(key);
        }
        public static bool beenPressed(Keys key)
        {
            return isUp(key) && wasDown(key);
        }
    }

}
