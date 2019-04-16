using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOLD
{
    class PlayerEntry : BaseScreen //allowsthe player to enter their name to be displayed on the highscore screen
        //this is done by checking which keys are/have been pressed then writing them to a file and drawing them to the highscore screen
    {
   private Sprite m_texture;

        Button continueButton;

        SpriteFont font;
        Cursor cursor;
        string entry = "";

        int score;

        KeyboardState prevKeybState;

        public PlayerEntry(Game1 game, int Score) : base(game)
        {
            score = Score;
        }

        public override void Load(ContentManager content, Vector2 pos)
        {
            m_texture = new Sprite();
            m_texture.Load(content, new Vector2(0, 0), "playerEntry");

            continueButton = new Button();
            continueButton.Load(content, new Vector2(1400, 750), "continueButton", "continueButton");

            font = content.Load<SpriteFont>("Font");
            cursor = new Cursor(content);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            m_texture.Draw(spriteBatch);
           
            continueButton.Draw(spriteBatch);
            spriteBatch.DrawString(font, entry + "|", new Vector2(400, 550), Color.Black);
            cursor.Draw(spriteBatch);
            spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
          
            KeyboardState currKeybState = Keyboard.GetState();

            Keys[] currDown = currKeybState.GetPressedKeys();
            Keys[] prevDown = prevKeybState.GetPressedKeys();

            foreach (Keys key in prevDown.Where(k => !currDown.Contains(k)))
            {
                if (key >= Keys.A && key <= Keys.Z)
                {
                    entry += key.ToString();
                }

                if (key == Keys.Back && entry.Count() > 0)
                {
                    entry = entry.Remove(entry.Count() - 1);
                }

                if (key == Keys.Space)
                {
                    entry += " ";
                }

                if (!string.IsNullOrEmpty(entry) && key == Keys.Enter)
                {
                    Game.ScreenMgr.Switch(new HighScoreScreen(Game, entry, score));
                }
            }

            if (continueButton.IsClicked())
            {
                Game.ScreenMgr.Switch(new HighScoreScreen(Game, entry, score));
            }

            prevKeybState = currKeybState;
        }
        
    }
}

