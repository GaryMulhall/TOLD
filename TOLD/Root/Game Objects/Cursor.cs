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
    //gives the class 3 different cursors based on where the mouse is located on the screen
    enum Cursors { normal, hammer, sword }
    class Cursor
        
    {
        //creates a dictionary of different textures to be drawn
        Dictionary<Cursors, Texture2D> m_cursors;
        Cursors m_current;

        public Cursor(ContentManager content)
        {
            //What determines which cursor should be drawn is handled in the GameScreen.cs, but this says if the cursor should be "normal" draw the "normal cursor" etc.
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
        //Sets the cursor to the "normal cursor" by default
        public void SetCursor(Cursors cursor)
        {
            m_current = cursor;
        }
    }
}
