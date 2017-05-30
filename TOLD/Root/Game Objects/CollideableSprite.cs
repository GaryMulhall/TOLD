using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace TOLD
{
    abstract class CollideableSprite : ICollideable //CollideableSprite  inherits properties from the ICollideable interface
    {
        protected Texture2D m_texture;
        protected Vector2 m_position;
        protected Vector2 m_prevPosition;
        public SoundEffect repairSound;

        protected bool m_visible = true;

        public virtual Rectangle Hitbox
        {
            get
            {
                return new Rectangle((int)m_position.X, (int)m_position.Y, m_texture.Width, m_texture.Height);
            }
        }

        public virtual void CollideWithScreenBounds()
        {

        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (m_visible)
            {
                if (m_texture == null)
                {
                    throw new InvalidOperationException("A texture has not been loaded. Check the Content folder!");
                }
                spriteBatch.Draw(m_texture, m_position, Color.White);
            }

        }

        public bool Destroyed
        {
            get; set;
        }

        public virtual void Load(ContentManager content, Vector2 pos)
        {
            repairSound = content.Load<SoundEffect>("RepairSound");
            m_texture = null;
            m_position = pos;
        }
        public virtual void ResetPosXLeft()
        {
            m_position.X = m_prevPosition.X;
        }
        public virtual void ResetPosXRight()
        {
            m_position.X = m_prevPosition.X;
        }
        public virtual void ResetPosYGround()
        {
            m_position.Y = m_prevPosition.Y;
        }
        public virtual void ResetPosYCeiling()
        {
            m_position.Y = m_prevPosition.Y;
        }
        
        public virtual void Update(GameTime gameTime)
        {
            CollideWithScreenBounds();
            m_prevPosition = m_position;
        }
    }
    abstract class AnimatedCollideableSprite : CollideableSprite
    {
        protected virtual Animation m_animation { get; set; }

        public override Rectangle Hitbox
        {
            get
            {
                return new Rectangle((int)m_position.X, (int)m_position.Y, m_animation.FrameWidth, m_animation.FrameHeight);
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            m_animation.Draw(spriteBatch, m_position);
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            m_animation.Update(gameTime);
        }
        public override void Load(ContentManager content, Vector2 pos)
        {
            base.Load(content, pos);
            m_visible = false;
        }
    }
}
