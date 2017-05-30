using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TOLD
{
    class PauseScreen : BaseScreen
    {
        private Sprite m_texture;

        Button resumeButton;
        Button mainMenuButton;

        Cursor cursor;

        public PauseScreen(Game1 game) : base(game)
        {
        }

        public override void Load(ContentManager content, Vector2 pos)
        {
            m_texture = new Sprite();
            m_texture.Load(content, new Vector2(0, 0), "pauseScreen");

            resumeButton = new Button();
            resumeButton.Load(content, new Vector2(1400, 550), "playButton", "playButtonHighlight");

            mainMenuButton = new Button();
            mainMenuButton.Load(content, new Vector2(1400, 750), "mainMenuButton", "mainMenuButtonHighlight");
            cursor = new Cursor(content);
        }

        public override void Update(GameTime gameTime)
        {
            cursor.SetCursor(Cursors.normal);
            if (Input.beenPressed(Keys.P))
            {
                Game.ScreenMgr.SwitchBack();
            }
            if (mainMenuButton.IsClicked())
            {
                Game.ScreenMgr.Switch(new MainMenuScreen(Game));
            }
            if (resumeButton.IsClicked())
            {
                Game.ScreenMgr.SwitchBack();
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            m_texture.Draw(spriteBatch);
            resumeButton.Draw(spriteBatch);
            mainMenuButton.Draw(spriteBatch);
            cursor.Draw(spriteBatch);
            spriteBatch.End();
        }
    }
}
