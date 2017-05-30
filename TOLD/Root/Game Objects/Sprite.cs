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
   public class Sprite
    {
        private Texture2D m_texture;
        public Vector2 Position;
       
        public  void Load(ContentManager content, Vector2 pos, string textureName)
        {
            m_texture = content.Load<Texture2D>(textureName);
            Position = pos;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(m_texture, Position, Color.White);
        }
    }
}
