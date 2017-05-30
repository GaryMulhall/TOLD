using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;

namespace TOLD
{
    class SmallWindow : CollideableSprite // inherits from CollideableSprite class
    {
        List<IMoveable> moveableSprites;
        Player m_player { get { return moveableSprites.OfType<Player>().First(); } }     
        Texture2D IntactTexture;
        List<Texture2D> DamagedTextures = new List<Texture2D>();
        List<Texture2D> RepairedTextures = new List<Texture2D>();
        private int CurrentState;
        SpriteFont font;
        Button continueButton;
        enum states { Intact, Damaged, Repaired }

        public event Action OnRepair;
        private states m_state = states.Intact;

        static Random random = new Random();
        private bool v;

        public SmallWindow(bool v, List<IMoveable> moveableSprites)
        {
            OnRepair += Repair;
            this.moveableSprites = moveableSprites;
            this.v = v;
            m_visible = false;
        }

        public override Rectangle Hitbox
        {
            get
            {
                return new Rectangle((int)m_position.X, (int)m_position.Y, IntactTexture.Width, IntactTexture.Height);
            }
        }
        public void WindowDamage()
        {
            int randomNumber = random.Next(2500);

            if (randomNumber <= 1)
            {
                Damage();
            }
            Vector2 Distance = m_player.Hitbox.Location.ToVector2() - m_position;
            Vector2 Direction = Vector2.Normalize(Distance);
            if (m_player.woodPiles > 0 && m_player.nails > 0 && Hitbox.Contains(Input.WorldMousePosition) && Input.isMouseJustReleased() && Distance.Length() < 380)
            {
                if (m_state == states.Damaged || (m_state == states.Repaired && CurrentState != RepairedTextures.Count - 1))
                {
                    repairSound.Play();
                    m_player.woodPiles--;
                    m_player.nails--;
                    OnRepair();
                }
            }
        }
        public override void Update(GameTime gameTime)
        {
            WindowDamage();
        }
        public override void Load(ContentManager content, Vector2 pos)
        {
            base.Load(content, pos);
            continueButton = new Button();
            continueButton.Load(content, new Vector2(540, 310), "continueButton", "continueButtonHighlight");
            font = content.Load<SpriteFont>("Font");
            IntactTexture = (content.Load<Texture2D>("smallWindow"));
            DamagedTextures.Add(content.Load<Texture2D>("smallWindowDmg1"));
            DamagedTextures.Add(content.Load<Texture2D>("smallWindowDmg2"));
            RepairedTextures.Add(content.Load<Texture2D>("smallWindowRep1"));
            RepairedTextures.Add(content.Load<Texture2D>("SmallWindowRep2"));

            CurrentState = 0;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
           
            if (m_state == states.Intact)
            {
                spriteBatch.Draw(IntactTexture, m_position, Color.White);
            }
            else if (m_state == states.Damaged)
            {
                spriteBatch.Draw(DamagedTextures[CurrentState], m_position, Color.White);
            }
            else if (m_state == states.Repaired)
            {
                spriteBatch.Draw(RepairedTextures[CurrentState], m_position, Color.White);
            }
        }

        public bool IsDestroyed()
        {
            return m_state == states.Damaged && CurrentState == DamagedTextures.Count - 1;
        }
        public void Damage()
        {
            if (m_state == states.Intact)
            {
                m_state = states.Damaged;
                CurrentState = 0;
            }
            else if (m_state == states.Damaged)
            {
                if (CurrentState < DamagedTextures.Count - 1)
                {
                    CurrentState++;
                }
            }
            else if (m_state == states.Repaired)
            {
                if (CurrentState > 0)
                {
                    CurrentState--;
                }
                else
                {
                    m_state = states.Damaged;
                    CurrentState = 0;
                }
            }
        }
        public void Repair()
        {
            if (m_state == states.Damaged)
            {
                m_state = states.Repaired;
                CurrentState = 0;
            }

            else if (m_state == states.Repaired)
            {
                if (CurrentState < RepairedTextures.Count - 1)
                {
                    CurrentState++;
                }
            }
        }
    }
}
