using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace TOLD
{
    class Player : AnimatedMoveableSprite // inherits from AnimatedMoveableSprite class
    {
        List<ICollideable> collideableSprites;
        List<IMoveable> moveableSprites;

        Dictionary<states, Animation> m_animations = new Dictionary<states, Animation>();

        protected override Animation m_animation { get { return m_animations[m_state]; } }
        private bool v;
        const float PlayerSpeed = 9.4f;
        public int lives = 5;
        public int woodPiles = 10;
        public int nails = 5;
        public int deaths = 0;
        protected Game1 Game;

        private SoundEffect DeathSound;

        SpriteEffects Effects;

        Vector2 livesPos;
        Vector2 woodPilesPos;

        SpriteFont Font;

        // States the player character can achieve
        enum states { Idle, Falling, Jumping, Walking }

        private states m_state;

        public Player(bool v, List<ICollideable> collideableSprites, List<IMoveable> moveableSprites, Game1 game)
        {
            // default state is idle
            m_state = states.Idle;

            this.v = v;
            this.collideableSprites = collideableSprites;
            this.moveableSprites = moveableSprites;
            Game = game;

            Effects = SpriteEffects.None;
        }

        // boolean to check if the player character can move 
        public bool CanMove(Vector2 velocity, IEnumerable<ICollideable> ignoreSprites = null)
        {
            List<ICollideable> ignoreList = new List<ICollideable>();
            if (ignoreSprites != null)
            {
                ignoreList.AddRange(ignoreSprites);
            }

            ignoreList.AddRange(collideableSprites.OfType<BigWindow>());
            ignoreList.AddRange(collideableSprites.OfType<SmallWindow>());
            ignoreList.AddRange(collideableSprites.OfType<Door>());
            ignoreList.AddRange(collideableSprites.OfType<Nail>());
            ignoreList.AddRange(collideableSprites.OfType<WoodPile>());


            Vector2 newPos = m_position + velocity;
            Rectangle newBounds = new Rectangle(newPos.ToPoint(), m_animation.FrameSize);

            if (newPos.X < 0 || newPos.Y < 0 || newPos.X + m_animation.FrameWidth > 1920 || newPos.Y > 1080 - m_animation.FrameHeight)
                return false;

            foreach (var collideable in collideableSprites.Where(s => !ignoreList.Contains(s)))
            {
                if (newBounds.Intersects(collideable.Hitbox))
                {
                    return false;
                }
            }
                return true;
        }
        public bool CanMoveOnLadOrPlat(Vector2 velocity, IEnumerable<ICollideable> laddersOrPlatforms)
        {

            Vector2 newPos = m_position + velocity;
            Rectangle newBounds = new Rectangle(newPos.ToPoint(), m_animation.FrameSize);

            foreach (var collideable in laddersOrPlatforms)
            {
                if (newBounds.Intersects(collideable.Hitbox))
                {
                    return true;
                }
            }
            return false;
        }

        public override Vector2 PerformMove(GameTime gameTime)
        {
            Vector2 velocity = Vector2.Zero;

            foreach (var woodPile in collideableSprites.OfType<WoodPile>())
            {
                if (Hitbox.Intersects(woodPile.Hitbox) && woodPiles < 10)
                {
                    woodPiles++;
                    woodPile.Destroyed = true;
                }
            }
            foreach (var nail in collideableSprites.OfType<Nail>())
            {
                if (Hitbox.Intersects(nail.Hitbox) && nails < 5)
                {
                    nails++;
                    nail.Destroyed = true;
                }
            }
           
            foreach (var door in collideableSprites.OfType<Door>())
            {
                if (Hitbox.Intersects(door.Hitbox))
                {
                    m_position = m_prevPosition;
                }
            }
            if (lives == 0)
            {
                Game.ScreenMgr.Switch(new GameOverScreen(Game));
                lives = 3;
                woodPiles = 0;
            }

            foreach (var enemy in moveableSprites.OfType<Enemy>())
            {
                if (Hitbox.Intersects(enemy.Hitbox))
                {
                    lives--;
                    deaths++;
                    m_position.X = 50;
                    m_position.Y = 650;
                    DeathSound.Play();
                }
            }

            // setting what happens if the character is currently in the falling state
            if (m_state == states.Falling)
            {
                foreach (var platform in collideableSprites.OfType<Platform>())
                {
                    if (Hitbox.Intersects(platform.Hitbox))
                    {
                        m_position.Y = platform.Hitbox.Y - m_animation.FrameHeight;
                        m_state = states.Idle;
                    }
                }

                if (m_position.Y >= Game1.screenHeight - m_animation.FrameHeight - 1)
                {
                    m_state = states.Idle;
                    m_position.Y = Game1.screenHeight - m_animation.FrameHeight;
                }
            }

            // setting what happens if the character is currently in the grounded state
            if (m_state == states.Idle || m_state == states.Walking)

            if (Input.isDown(Keys.W) || Input.isDown(Keys.Up)) 
            {
                if (m_state == states.Idle)
                {
                    m_state = states.Walking;
                }
                if (CanMove(new Vector2(0, -1)))
                {
                    velocity.Y -= 10;
                }
            }
            if (Input.isDown(Keys.S) || (Input.isDown(Keys.Down)))
            {
                if (m_state == states.Idle)
                {
                    m_state = states.Walking;
                }
                if (CanMove(new Vector2(0, 11)))
                {
                    velocity.Y += 10;
                }
            }
            if (Input.isDown(Keys.A) || (Input.isDown(Keys.Left)))
            {
                if (m_state == states.Idle)
                {
                    m_state = states.Walking;
                    
                }
                if (CanMove(new Vector2(-30, -0)))
                {
                    velocity.X -= 10;
                    Effects = SpriteEffects.FlipHorizontally;
                }
            }
            else if (Input.isDown(Keys.D) || (Input.isDown(Keys.Right)))
            {
                if (m_state == states.Idle)
                {
                    m_state = states.Walking;
                }
                if (CanMove(new Vector2(30, 0)))
                {
                    velocity.X += 10;
                    Effects = SpriteEffects.None;
                }
            }
            else
            {
                if (m_state == states.Walking)
                {
                    m_state = states.Idle;
                }
            }
         
                        if (Input.isDown(Keys.W))
                        {
                            m_state = states.Walking;
                             if (CanMoveOnLadOrPlat(new Vector2(0, -10), collideableSprites.OfType<Ladder>()))
                             {
                                 velocity.Y -= 10;
                             }
                        }
                        if (Input.isDown(Keys.S))
                        {
                            m_state = states.Walking;
                            if (CanMoveOnLadOrPlat(new Vector2(0, 10),collideableSprites.OfType<Ladder>()))
                            {
                                velocity.Y += 10;
                            }
                        }
                        if (Input.isDown(Keys.A))
                        {
                            m_state = states.Walking;
                            if (CanMoveOnLadOrPlat(new Vector2(-90, 0), collideableSprites.OfType<Ladder>()) || CanMoveOnLadOrPlat(new Vector2(-90, 0), collideableSprites.OfType<Platform>()))
                            {
                                velocity.X -= 10;
                                Effects = SpriteEffects.FlipHorizontally;
                            }
                        }
                        if (Input.isDown(Keys.D))
                        {
                            m_state = states.Walking;
                            if (CanMoveOnLadOrPlat(new Vector2(90, 0), collideableSprites.OfType<Ladder>())  || CanMoveOnLadOrPlat(new Vector2(90, 0), collideableSprites.OfType<Platform>()))
                            {
                                velocity.X += 10;
                                Effects = SpriteEffects.None;

                            }
                        }
    
            foreach(var platform in collideableSprites.OfType<Platform>())
            {
                if(Hitbox.Intersects(platform.Hitbox))
                {
                    m_position.Y = platform.Hitbox.Location.Y + 75;
                }
            }
            return velocity;
        }
        
        public override void ResetPosYGround()
        {
            base.ResetPosYGround();
        }
        public override void ResetPosXRight()
        {
            base.ResetPosXRight();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Move(gameTime);
        }
        public override void Load(ContentManager content, Vector2 pos)
        {
            livesPos = new Vector2(1180, 20);
            woodPilesPos = new Vector2(40, 20);
            Font = content.Load<SpriteFont>("Font");
            base.Load(content, pos);

            m_animations[states.Idle] = new Animation(content.Load<Texture2D>("playerSpriteSheet"), (1 / 5f), 4, 2, 0, 3);
            m_animations[states.Walking] = new Animation(content.Load<Texture2D>("playerSpriteSheet"), (1 / 7f), 4, 2, 4, 7);

            DeathSound = content.Load<SoundEffect>("deathSound");
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            m_animation.Draw(spriteBatch, m_position, Effects);
        }
    }
}
