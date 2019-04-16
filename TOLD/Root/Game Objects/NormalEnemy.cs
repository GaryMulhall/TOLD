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
    class NormalEnemy : Enemy //interits from the Enemy class which contains generic properties from all different types of enemies
    {
        Animation deathAnimation;
        public NormalEnemy(bool v, Player player, List<ICollideable> collideableSprites, List<IMoveable> moveableSprites, Vector2 InitialVelocity)
            : base(v, player, collideableSprites, moveableSprites, InitialVelocity)
        {
            OnEnemyClick += Player_OnEnemyClick;

        }
        //Setting what should happen when the player clicks on a NormalEnemy
        //the enemy is dying (plays death animation), deathtimer is set to 1 second to allow the animation to play then there is a chance of the NormalEnemy dropping either a woodpile or a nail
        public void Player_OnEnemyClick()
        {
            isDying = true;
            deathTimer = 1;
            m_animation = deathAnimation;
            enemyDamage.Play();
            kills++;
            int randomNumber = random.Next(2);

            if (randomNumber == 0)
            {
                addCollideable(m_content, Hitbox.Location.ToVector2(), new WoodPile(true));
            }
            else if (randomNumber == 1)
            {
                addCollideable(m_content, Hitbox.Location.ToVector2(), new Nail(true));
            }
        }
        public override void Load(ContentManager content, Vector2 pos)
        {
            base.Load(content, pos);
            m_animation = new Animation(content.Load<Texture2D>("enemySpriteSheet"), (1 / 6f), 4, 1, 0, 3);
            deathAnimation = new Animation(content.Load<Texture2D>("normalEnemyDeathSpriteSheet"),  (1 / 6f), 4, 1, 0, 3, false);
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
        }
    }
}
