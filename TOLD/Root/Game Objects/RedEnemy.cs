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
    class RedEnemy : Enemy // see NormalEnemy.cs for more information on this code
    {
        int redEnemyLives = 3;
        Animation deathAnimation;
        ImageCounter redEnemyLivesCounter;
        public RedEnemy(bool v, Player player, List<ICollideable> collideableSprites, List<IMoveable> moveableSprites, Vector2 InitialVelocity)
            : base(v, player, collideableSprites, moveableSprites, InitialVelocity)
        {
            OnEnemyClick += Player_OnRedEnemyClick;

        }
        public void Player_OnRedEnemyClick()
        {
            if (redEnemyLives > 0)
            {
                redEnemyLives--;

                if (redEnemyLives == 0)
                {
                    isDying = true;
                    deathTimer = 1;
                    m_animation = deathAnimation;
                    enemyDamage.Play();
                    kills++;
                    int randomNumber = random.Next(3);

                    if (randomNumber == 0)
                    {
                        addCollideable(m_content, Hitbox.Location.ToVector2(), new WoodPile(true));
                    }
                    else if (randomNumber == 1)
                    {
                        addCollideable(m_content, Hitbox.Location.ToVector2(), new Nail(true));
                    }
                }
            }
        }

        public override void Load(ContentManager content, Vector2 pos)
        {
            base.Load(content, pos);
            m_animation = new Animation(content.Load<Texture2D>("redEnemySpriteSheet"), (1 / 6f), 4, 1, 0, 3);
            redEnemyLivesCounter = new ImageCounter(content.Load<Texture2D>("heart"), content.Load<Texture2D>("heartGrey"), Vector2.Zero, new Vector2(35, 0), 3, 2);
            deathAnimation = new Animation(content.Load<Texture2D>("redEnemyDeathSpriteSheet"), (1 / 6f), 4, 1, 0, 3, false);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (isDying == false)
            {
                redEnemyLivesCounter.Draw(spriteBatch);
            }
            base.Draw(spriteBatch);
        }
        public override void Update(GameTime gameTime)
        {
            if (isDying == true)
            {
                m_animation.Update(gameTime);
                deathTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (deathTimer <= 0)
                {
                    Destroyed = true;
                }
            }
            else
            {
                base.Update(gameTime);
            }
            redEnemyLivesCounter.SetCount(redEnemyLives);
            redEnemyLivesCounter.Position = m_position + new Vector2(-10, -25);
        }
    }
}
