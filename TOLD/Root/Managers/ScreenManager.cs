using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TOLD
{
    interface IScreen
    {
        void Load(ContentManager content, Vector2 pos);
        void Update(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch);
    }

    class ScreenManager
    {
        private IScreen Current;
        private IScreen Previous;

        private ContentManager Content;

        public ScreenManager(ContentManager content)
        {
            Content = content;
        }

        public void Switch(IScreen screen)
        {
            if (screen == null)
                throw new ArgumentNullException("screen");

            Previous = Current;
            Current = screen;
            Current.Load(Content, new Vector2());
        }

        public void SwitchBack()
        {

            if(Current == Previous)
            {
                throw new InvalidOperationException("Cannot use SwitchBack twice in a row");
            }
            Current = Previous;
        }

        public void Update(GameTime gameTime)
        {
            Current.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Current.Draw(spriteBatch);
        }
    }
}
