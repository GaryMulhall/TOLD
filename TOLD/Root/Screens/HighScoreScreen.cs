using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOLD
{
    class HighScoreScreen : BaseScreen
    {
        private Sprite m_texture;

        SpriteFont font;
        Button quitButton;
        Button mainMenuButton;
        Cursor cursor;
        Dictionary<string, int> highScore = new Dictionary<string, int>();
  
        public HighScoreScreen(Game1 game)
            : base(game)
        {
            highScore = LoadFile("HallOfFame.csv");
  
        }

        public HighScoreScreen(Game1 game, string name, int score)
            : base(game)
        {
            highScore = LoadFile("HighScore.csv");
            highScore[name] = score;
            highScore.OrderByDescending(item => item.Value);
            SaveFile("HighScore.csv", highScore);
        }

        public override void Load(ContentManager content, Vector2 pos)
        {
            m_texture = new Sprite();
            m_texture.Load(content, new Vector2(0, 0), "highScoreScreen");

            quitButton = new Button();
            quitButton.Load(content, new Vector2(1400, 750), "quitButton", "quitButtonHighlight");

            mainMenuButton = new Button();
            mainMenuButton.Load(content, new Vector2(1400, 550), "mainMenuButton", "mainMenuButtonHighlight");
            font = content.Load<SpriteFont>("Font");
            cursor = new Cursor(content);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            m_texture.Draw(spriteBatch);
            quitButton.Draw(spriteBatch);
            mainMenuButton.Draw(spriteBatch);
            cursor.Draw(spriteBatch);
            int padding = 0;

            foreach (var item in highScore)
            {
                string line = item.Key + ":" + item.Value.ToString();
                spriteBatch.DrawString(font, line, new Vector2 (500, 650 + padding), Color.Black);
                padding += 35;
            }
            spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            if (quitButton.IsClicked())
            {
                Game.Exit();
            }
            if (mainMenuButton.IsClicked())
            {
                Game.ScreenMgr.Switch(new MainMenuScreen(Game));
            }
        }

        private Dictionary<string, int> LoadFile(string FileName)
        {
            Dictionary<string, int> result = new Dictionary<string, int>();

            if (File.Exists(FileName))
            {
                string[] lines = File.ReadAllLines(FileName);
                foreach (string line in lines)
                {
                    string[] values = line.Split(',');

                    string name = values[0];
                    int score;
                    try { int.TryParse(values[1], out score); }
                    catch { continue; }
                    result[name] = score;
                }
            }
            return result;
        }

        private void SaveFile(string FileName, Dictionary<string, int> dict)
        {
            List<string> lines = new List<string>();
            foreach(var item in dict)
            {
                string line = item.Key + "," + item.Value.ToString();
                lines.Add(line);
            }
            File.WriteAllLines(FileName, lines);
        }
    }
}

