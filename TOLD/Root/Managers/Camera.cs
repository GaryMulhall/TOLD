using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace TOLD
{
    class Camera // this camera will centre around the player if the screen resolution is lower than 1080p
    {
        private Vector2 m_position;
        private Viewport m_viewport;
        private Rectangle m_bounds
        {
            get
            {
                return new Rectangle(m_position.ToPoint(), m_viewport.Bounds.Size);
            }
        }
        public Camera(Viewport viewport)
        {
            m_viewport = viewport;
        }
        public void Update(Vector2 targetPosition)
        {
            m_position = (m_viewport.Bounds.Size.ToVector2() / 2) - targetPosition;
            if(m_position.X > 0)
            {
                m_position.X = 0;
            }
            else if (m_position.X < -(1920 - m_viewport.Width))
            {
                m_position.X = -(1920 - m_viewport.Width);
            }
            if (m_position.Y > 0)
            {
                m_position.Y = 0;
            }
            else if (m_position.Y < -(1080 - m_viewport.Height))
            {
                m_position.Y = -(1080 - m_viewport.Height);
            }
        }
        public Matrix GetViewMatrix()
        {
            return Matrix.CreateTranslation(new Vector3(m_position, 0f));
        }

        public Vector2 ScreenToWorld(Vector2 worldCoords)
        {
           Matrix mat = Matrix.CreateTranslation(new Vector3(-m_position, 0f));
            return Vector2.Transform(worldCoords, mat);
        }
    }
    
}
