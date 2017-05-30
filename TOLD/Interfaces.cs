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
    interface ILoadable
    {
        void Load(ContentManager content, Vector2 pos);
    }
    interface IUpdateable
    {
        void Update(GameTime gameTime);
    }
    interface IDrawable
    {
        void Draw(SpriteBatch spriteBatch);
    }


    interface ICollideable : ILoadable, IUpdateable, IDrawable
    {
        void CollideWithScreenBounds();
        void ResetPosXLeft();
        void ResetPosXRight();
        void ResetPosYGround();
        void ResetPosYCeiling();

        Rectangle Hitbox { get; }

        bool Destroyed
        {
            get; set;

        }
    }

    interface IMoveable : ICollideable
    {

        void Move(GameTime gameTime);

    }
}
