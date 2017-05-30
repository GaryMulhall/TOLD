
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOLD
{
    abstract class MoveableSprite : CollideableSprite, IMoveable
    {
        public void Move(GameTime gameTime)
        {
            Vector2 velocity = PerformMove(gameTime);
            m_position += velocity;
        }

        public abstract Vector2 PerformMove(GameTime gameTime);
    }

    abstract class AnimatedMoveableSprite : AnimatedCollideableSprite, IMoveable
    {
        public void Move(GameTime gameTime)
        {
            Vector2 velocity = PerformMove(gameTime);
            m_position += velocity;
        }

        public abstract Vector2 PerformMove(GameTime gameTime);
    }

}
