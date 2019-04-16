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
        //creates a dictionary of different states (and the animations that go with them) the Player class can achieve based on actions in game
        Dictionary<states, Animation> m_animations = new Dictionary<states, Animation>();

        protected override Animation m_animation { get { return m_animations[m_state]; } }
        private bool v;
        const float PlayerSpeed = 9.4f;
        public int lives = 5;
        public int woodPiles = 10;
        public int nails = 5;
        public int deaths = 0;
        protected Game1 Game;
        float deathTimer = 0;
        float respawnTimer = 0;

        private SoundEffect DeathSound;
        private SoundEffect PickupSound;
        SpriteEffects Effects;

        Vector2 livesPos;
        Vector2 woodPilesPos;

        SpriteFont Font;

        // States the player character can achieve
        enum states { Idle, Walking, Dying, RespawningIdle, RespawningWalking }

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
            //if the player goes off the screen, reset the players position
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
        //a boolean to check if the player is contained within a ladders hitbox or standing on a platform
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
                    PickupSound.Play();
                    woodPiles++;
                    woodPile.Destroyed = true;
                }
            }
            //allows the player to collect a nail as long as they have less than 5 nails currently
            foreach (var nail in collideableSprites.OfType<Nail>())
            {
                if (Hitbox.Intersects(nail.Hitbox) && nails < 5)
                {
                    PickupSound.Play();
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
            //if the player runs out of lives the GameOverScreen will be drawn
            if (lives == 0)
            {
                Game.ScreenMgr.Switch(new GameOverScreen(Game));
                lives = 3;
                woodPiles = 0;
            }
            //if the player collides with an enemy, player dies, animations reset and a sound is played
            if (m_state !=states.Dying && m_state !=states.RespawningIdle && m_state !=states.RespawningWalking)
            {
                foreach (var normalEnemy in moveableSprites.OfType<NormalEnemy>())
                {
                    if (Hitbox.Intersects(normalEnemy.Hitbox))
                    {
                        deathTimer = 1;
                        m_state = states.Dying;
                        m_animations[states.Dying].Reset();
                        DeathSound.Play();
                    }
                }
                //see comment above
                foreach (var redEnemy in moveableSprites.OfType<RedEnemy>())
                {
                    if (Hitbox.Intersects(redEnemy.Hitbox))
                    {
                        deathTimer = 1;
                        m_state = states.Dying;
                        m_animations[states.Dying].Reset();
                        DeathSound.Play();
                    }
                }
            }

            // setting what happens if the character is currently in the grounded state
                if (m_state != states.Dying)
                {
                    //setting what happens when the player moves based on different situations within the game
                    //is the player idle? is the player respawning? etc.
                    if (Input.isDown(Keys.W) || Input.isDown(Keys.Up))
                    {
                        if (m_state == states.Idle)
                        {
                            m_state = states.Walking;
                        }
                        if (m_state == states.RespawningIdle)
                        {
                            m_state = states.RespawningWalking;
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
                        if (m_state == states.RespawningIdle)
                        {
                            m_state = states.RespawningWalking;
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
                        if (m_state == states.RespawningIdle)
                        {
                            m_state = states.RespawningWalking;
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
                        if (m_state == states.RespawningIdle)
                        {
                            m_state = states.RespawningWalking;
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
                        if (m_state == states.RespawningWalking)
                        {
                            m_state = states.RespawningIdle;
                        }
                    }

                    if (Input.isDown(Keys.W))
                    {
                        if (m_state == states.Idle)
                        {
                            m_state = states.Walking;
                        }
                        if (m_state == states.RespawningIdle)
                        {
                            m_state = states.RespawningWalking;
                        } if (CanMoveOnLadOrPlat(new Vector2(0, -10), collideableSprites.OfType<Ladder>()))
                        {
                            velocity.Y -= 10;
                        }
                    }
                    if (Input.isDown(Keys.S))
                    {
                        if (m_state == states.Idle)
                        {
                            m_state = states.Walking;
                        }
                        if (m_state == states.RespawningIdle)
                        {
                            m_state = states.RespawningWalking;
                        } if (CanMoveOnLadOrPlat(new Vector2(0, 10), collideableSprites.OfType<Ladder>()))
                        {
                            velocity.Y += 10;
                        }
                    }
                    if (Input.isDown(Keys.A))
                    {
                        if (m_state == states.Idle)
                        {
                            m_state = states.Walking;
                        }
                        if (m_state == states.RespawningIdle)
                        {
                            m_state = states.RespawningWalking;
                        } 
                        if (CanMoveOnLadOrPlat(new Vector2(-90, 0), collideableSprites.OfType<Ladder>()) || CanMoveOnLadOrPlat(new Vector2(-90, 0), collideableSprites.OfType<Platform>()))
                        {
                            velocity.X -= 10;
                            Effects = SpriteEffects.FlipHorizontally;
                        }
                    }
                    if (Input.isDown(Keys.D))
                    {
                        if (m_state == states.Idle)
                        {
                            m_state = states.Walking;
                        }
                        if (m_state == states.RespawningIdle)
                        {
                            m_state = states.RespawningWalking;
                        } if (CanMoveOnLadOrPlat(new Vector2(90, 0), collideableSprites.OfType<Ladder>()) || CanMoveOnLadOrPlat(new Vector2(90, 0), collideableSprites.OfType<Platform>()))
                        {
                            velocity.X += 10;
                            Effects = SpriteEffects.None;
                        }
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
            //if the player is currently dying, play the deathanimation, give the player 5 seconds invulnerability, reset the position, remove a life and add a death
            if (m_state == states.Dying)
            {
                deathTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                m_animation.Update(gameTime);

                if (deathTimer <= 0)
                {
                    respawnTimer = 5;
                    m_state = states.RespawningIdle;
                    m_position.X = 50;
                    m_position.Y = 650;
                    lives--;
                    deaths++;
                }
            }
            else if(m_state == states.RespawningIdle || m_state == states.RespawningWalking)
            {
                m_animation.Update(gameTime);
                respawnTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (respawnTimer <= 0)
                {
                    m_state = states.Idle;
                }
            }
            else
            {
                base.Update(gameTime);
            }
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
            m_animations[states.Dying] = new Animation(content.Load<Texture2D>("playerDyingSpriteSheet"), (1 / 5f), 4, 1, 0, 3, false);
            m_animations[states.RespawningIdle] = new Animation(content.Load<Texture2D>("playerRespawnSpriteSheet"), (1/5f), 4, 1, 0, 3);
            m_animations[states.RespawningWalking] = new Animation(content.Load<Texture2D>("playerRespawnWalkingSpriteSheet"), (1 / 6f), 4, 1, 0, 3);
            DeathSound = content.Load<SoundEffect>("deathSound");
            PickupSound = content.Load<SoundEffect>("pickup");
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            m_animation.Draw(spriteBatch, m_position, Effects);
        }
    }
}
