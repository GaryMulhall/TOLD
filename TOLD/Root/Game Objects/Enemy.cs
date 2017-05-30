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
    class Enemy : AnimatedMoveableSprite // inherits from MoveableSprite class
    {

        static Random random = new Random();
        List<IMoveable> moveableSprites;
        List<ICollideable> collideableSprites;
        Player m_player;
        private event Action<Enemy> OnEnemyClick;
        ContentManager m_content;
        private SoundEffect enemyDamage;
        public int kills;
        public Enemy(bool v, Player player, List<ICollideable> collideableSprites, List<IMoveable> moveableSprites, Vector2 InitialVelocity)
        {
            OnEnemyClick += Player_OnEnemyClick;
            this.moveableSprites = moveableSprites;
            this.collideableSprites = collideableSprites;
            m_player = player;

        }
        public override void Load(ContentManager content, Vector2 pos)
        {
            m_content = content;
            base.Load(content, pos);
            m_animation = new Animation(content.Load<Texture2D>("enemySpriteSheet"), (1 / 6f), 4, 1, 0, 3);
            enemyDamage = content.Load<SoundEffect>("EnemyDamage");
        }
        public void addCollideable(ContentManager content, Vector2 pos, CollideableSprite newSprite)
        {
            newSprite.Load(content, pos);
            collideableSprites.Add(newSprite);
        }
        private void Player_OnEnemyClick(Enemy enemy)
        {
            enemy.Destroyed = true;
            enemyDamage.Play();
            kills++;
            int randomNumber = random.Next(2);

            if (randomNumber == 0)
            {
                addCollideable(m_content, enemy.Hitbox.Location.ToVector2(), new WoodPile(true));
            }
            else if (randomNumber == 1)
            {
                addCollideable(m_content, enemy.Hitbox.Location.ToVector2(), new Nail(true));
            }
        }

        public override Vector2 PerformMove(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            
            Vector2 Distance = m_player.Hitbox.Location.ToVector2() - m_position;
            foreach(var enemy in moveableSprites.OfType<Enemy>())
            {
                if (enemy.Hitbox.Contains(Input.WorldMousePosition) && Input.isMouseJustClicked() && Distance.Length() < 200)
                {
                    OnEnemyClick(enemy);
                }
            }
            Vector2 Direction = Vector2.Normalize(Distance);
            m_position += Direction * 2;
            base.Update(gameTime);
        }

    }
}

