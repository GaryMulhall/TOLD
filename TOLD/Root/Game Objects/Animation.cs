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
    class Animation
    {
        private readonly Texture2D m_spritesheet;
        private readonly float m_timePerFrame;

        public readonly int FrameWidth;
        public readonly int FrameHeight;

        public Point FrameSize { get { return new Point(FrameWidth, FrameHeight); }}

        private readonly int m_columns;
        private readonly int m_rows;

        private readonly int m_startFrame;
        private readonly int m_endFrame;

        private readonly bool m_repeating;

        private int m_frame;
        private float m_timeElapsed;
        //Setting the required parameters for an animation to be played (with the exception of repeating being optional)
        public Animation(Texture2D spritesheet, float timePerFrame, int columns, int rows, int startFrame=0, int endFrame=-1, bool repeating=true)
        {
            m_spritesheet = spritesheet;
            m_timePerFrame = timePerFrame;

            FrameWidth = m_spritesheet.Width / columns;
            FrameHeight = m_spritesheet.Height / rows;

            m_columns = columns;
            m_rows = rows;

            m_repeating = repeating;

            m_startFrame = startFrame;
            m_endFrame = endFrame;
            //The total frames of the animation is the total number of columns multiplied by the total number of rows allowing spritesheets to vary in size
            int totalFrames = m_columns * m_rows;
            if (m_endFrame == -1)
            {
                m_endFrame = totalFrames - 1;
            }
        }
        //If the animation should be reset, the current frame will be switched to frame 0
        public void Reset()
        {
            m_frame = 0;
        }

        public void Update(GameTime gameTime)
        {
            m_timeElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (m_timeElapsed > m_timePerFrame)
            {
                m_frame++;

                if (m_frame >= m_endFrame)
                {
                    if (m_repeating)
                    {
                        m_frame = m_startFrame;
                    }
                    else
                    {
                        m_frame = m_endFrame;
                    }
                }

                m_timeElapsed -= m_timePerFrame;
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, SpriteEffects Effects = SpriteEffects.None)
        {
            //Math which handles the animations playing
            int row = (int)Math.Floor((double)m_frame / (double)m_columns);
            int column = m_frame - (row * m_columns);

            Rectangle rect = new Rectangle(FrameWidth * column, FrameHeight * row, FrameWidth, FrameHeight);
            
            spriteBatch.Draw(m_spritesheet, position, rect, Color.White, 0, Vector2.Zero, 1, Effects, 0);
        }
    }
}
