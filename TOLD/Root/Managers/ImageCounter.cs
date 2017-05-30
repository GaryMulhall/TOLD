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
    class ImageCounter
    {
        Texture2D m_activeTexture;
        Texture2D m_inactiveTexture;

        Vector2 m_position;
        Vector2 m_offset;

        int m_maxCount;
        int m_count;

        public ImageCounter(Texture2D activeTexture, Texture2D inactiveTexture, Vector2 position, Vector2 offset, int maxCount, int count)
        {
            m_activeTexture = activeTexture;
            m_inactiveTexture = inactiveTexture;

            m_position = position;
            m_offset = offset;

            m_maxCount = maxCount;
            m_count = count;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < m_maxCount; i++)
            {
                Texture2D texture = m_inactiveTexture;

                if (i < m_count)
                {
                    texture = m_activeTexture;
                }

                spriteBatch.Draw(texture, m_position + (m_offset * i), Color.White);
            }
        }

        public void IncreaseCount()
        {
            m_count++;
        }

        public void DecreaseCount()
        {
            m_count--;
        }

        public void SetCount(int count)
        {
            m_count = count;
        }
    }
}
