using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOLD
{
    abstract class Enemy : AnimatedMoveableSprite // inherits from AnimatedMoveableSprite class
    {
        protected static Random random = new Random();
        List<IMoveable> moveableSprites;
        List<ICollideable> collideableSprites;
        Player m_player;
        protected event Action OnEnemyClick;
        protected ContentManager m_content;
        protected SoundEffect enemyDamage;
        public int kills;
        protected float deathTimer = 0;
        protected bool isDying = false;
        public Enemy(bool v,Player player, List<ICollideable> collideableSprites, List<IMoveable> moveableSprites, Vector2 InitialVelocity)
        {
            this.moveableSprites = moveableSprites;
            this.collideableSprites = collideableSprites;
            m_player = player;
        }
        public override void Load(ContentManager content, Vector2 pos)
        {
            m_content = content;
            base.Load(content, pos);
            enemyDamage = content.Load<SoundEffect>("EnemyDamage");
        }
        public void addCollideable(ContentManager content, Vector2 pos, CollideableSprite newSprite)
        {
            newSprite.Load(content, pos);
            collideableSprites.Add(newSprite);
        }
            public override Vector2 PerformMove(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            Vector2 Distance = m_player.Hitbox.Location.ToVector2() - m_position;
            {
                if (Hitbox.Contains(Input.WorldMousePosition) && Input.isMouseJustClicked() && Distance.Length() < 200)
                {
                    OnEnemyClick();
                }
            }
            Vector2 Direction = Vector2.Normalize(Distance);
            m_position += Direction * 2;
            base.Update(gameTime);
        }

    }
}

