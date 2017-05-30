using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
using System.Xml.Serialization;

namespace TOLD
{
    class Game1 : Game
    {
        GraphicsDeviceManager graphics;

        public ScreenManager ScreenMgr;

        public static int screenWidth = 1920;
        public static int screenHeight = 1080;
        private SpriteBatch spriteBatch;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = screenWidth;
            graphics.PreferredBackBufferHeight = screenHeight;

            IsMouseVisible = true;

            Content.RootDirectory = "Content";
        }

        public struct HighScore
        {
            public string[] PlayerName;
            public int[] Score;
            public int Count;

            public HighScore(int count)
            {
                PlayerName = new string[count];
                Score = new int[count];

                Count = count;
            }
        }


        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            ScreenMgr = new ScreenManager(Content);

            ScreenMgr.Switch(new MainMenuScreen(this));
        }
        protected override void UnloadContent()
        {

        }
        protected override void Update(GameTime gameTime)
        {
            Input.Update();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            ScreenMgr.Update(gameTime);

            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Gray);
            IsMouseVisible = false;
            ScreenMgr.Draw(spriteBatch);
            base.Draw(gameTime);
        }
    }
}
