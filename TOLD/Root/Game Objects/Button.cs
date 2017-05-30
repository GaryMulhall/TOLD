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
   public class Button
    {
        private Texture2D m_texture;
        private Texture2D m_highlightTexture;
        private Vector2 m_position;
        private bool v;

        public bool isEnabled = true;

        public Button(bool v)
        {
            this.v = v;
        }

        public Button()
        {
        }

        public  void Load(ContentManager content, Vector2 pos, string textureName, string highlightName)
        {
            m_texture = content.Load<Texture2D>(textureName);
            m_highlightTexture = content.Load<Texture2D>(highlightName);
            m_position = pos;
        }

        public bool IsClicked()
        {
            if(isEnabled == false)
            {
                return false;
            }
            Rectangle bounds = new Rectangle(m_position.ToPoint(), m_texture.Bounds.Size);
            return (bounds.Contains(Mouse.GetState().Position) && Input.isMouseJustClicked());
        }
    

        public void Draw(SpriteBatch spriteBatch)
        {
            if(isEnabled == false)
            {
                return;
            }
            Rectangle bounds = new Rectangle(m_position.ToPoint(), m_texture.Bounds.Size);
            if (bounds.Contains(Mouse.GetState().Position))
            {
                spriteBatch.Draw(m_highlightTexture, m_position, Color.White);
            }
            else
            {
                spriteBatch.Draw(m_texture, m_position, Color.White);
            }
        }
    }
}
