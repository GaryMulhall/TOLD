using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;

namespace TOLD
{
    class Wall : CollideableSprite // inherits from CollideableSprite class
    {
        private bool v;

        public Wall(bool v)
        {
            this.v = v;
        }

        public void Update()
        {
        }
        public override void Load(ContentManager content, Vector2 pos)
        {
            base.Load(content, pos);
            m_texture = content.Load<Texture2D>("wall");
        }
    }
}
