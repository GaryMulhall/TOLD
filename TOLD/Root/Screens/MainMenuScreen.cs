using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace TOLD
{
    class MainMenuScreen : BaseScreen
        //MainMenuScreen class inherits properties from the Basescreen class
    {
        private Sprite m_texture;
        Button playButton;
        Button quitButton;
        Button helpButton;
        Cursor cursor;
        public Song backGroundMusic;

        PlankModel model;

        public MainMenuScreen(Game1 game) : base(game)
        {
        }

        public override void Load(ContentManager content, Vector2 pos)
        {
            m_texture = new Sprite();
            m_texture.Load(content, new Vector2(0, 0), "mainmenu");

            playButton = new Button();
            playButton.Load(content, new Vector2(1400, 350), "playButton", "playButtonHighlight");

            quitButton = new Button();
            quitButton.Load(content, new Vector2(1400, 750), "quitButton", "quitButtonHighlight");

            helpButton = new Button();
            helpButton.Load(content, new Vector2(1400, 550), "helpButton", "helpButtonHighlight");

            backGroundMusic = content.Load<Song>("BackGroundMusic");
            MediaPlayer.Volume = 0.08f;
            MediaPlayer.Play(backGroundMusic);
            MediaPlayer.IsRepeating = true;
            cursor = new Cursor(content);

            model = new PlankModel(content);

        }

        public override void Update(GameTime gameTime)
        {
            bool enterPressed = Input.isDown(Keys.Enter);
            if (enterPressed || playButton.IsClicked())
            {
                Game.ScreenMgr.Switch(new GameScreen(Game));
            }
            if(quitButton.IsClicked())
            {
                Game.Exit();
            }
            if (helpButton.IsClicked())
            {
                Game.ScreenMgr.Switch(new HelpScreen(Game));
            }
            cursor.SetCursor(Cursors.normal);

            model.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            m_texture.Draw(spriteBatch);
            playButton.Draw(spriteBatch);
            quitButton.Draw(spriteBatch);
            helpButton.Draw(spriteBatch);
            cursor.Draw(spriteBatch);
            spriteBatch.End();
            spriteBatch.Begin();
            model.Draw(Game.GraphicsDevice);
            cursor.Draw(spriteBatch);
            spriteBatch.End();
        }

    }
}
