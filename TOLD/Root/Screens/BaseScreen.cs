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
    abstract class BaseScreen : IScreen
    {
        protected Game1 Game;

        public BaseScreen(Game1 game)
        {
            Game = game;
        }

        public abstract void Load(ContentManager content, Vector2 pos);
        
        public abstract void Update(GameTime gameTime);

        public abstract void Draw(SpriteBatch spriteBatch);

    }
}
