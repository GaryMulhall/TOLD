using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOLD
{
    enum Cursors { normal, hammer, sword }
    class Cursor
        
    {
        Dictionary<Cursors, Texture2D> m_cursors;
        Cursors m_current;

        public Cursor(ContentManager content)
        {
            m_cursors = new Dictionary<Cursors,Texture2D>();
            m_cursors[Cursors.normal] = content.Load<Texture2D>("cursor");
            m_cursors[Cursors.hammer] = content.Load<Texture2D>("hammer");
            m_cursors[Cursors.sword] = content.Load<Texture2D>("sword");
            m_current = Cursors.normal;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(m_cursors[m_current], Input.MousePosition.ToVector2(), Color.White);
        }
        public void SetCursor(Cursors cursor)
        {
            m_current = cursor;
        }
    }
}
