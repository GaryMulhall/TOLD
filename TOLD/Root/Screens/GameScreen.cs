using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections;
using Microsoft.Xna.Framework.Input;
using System.IO;
using Microsoft.Xna.Framework.Audio;

namespace TOLD
{
    //GameScreen inherits from the BaseScreen class and contains most things which will be displayed on screen during gameplay
    class GameScreen : BaseScreen
    {
        int currentLevel;
        List<IMoveable> moveableSprites;
        List<ICollideable> collideableSprites;

        Random random = new Random();

        Button continueButton;
        SpriteFont font;
        public int repairs;
        bool isPaused = false;
        ContentManager m_content;

        Camera camera;
        Cursor cursor;
        private SoundEffect PickupSound;
        ImageCounter livesCounter;
        ImageCounter woodPileCounter;
        ImageCounter nailCounter;
        Player m_player { get { return moveableSprites.OfType<Player>().First(); } }

        Sprite Floor;
        float elapsedTime = 0;

        public GameScreen(Game1 game)
            : base(game)
        {
        }
        private void RepairCount()
        {
            repairs++;
        }
        public override void Load(ContentManager content, Vector2 pos)
        {
            continueButton = new Button();
            continueButton.Load(content, new Vector2(1400, 750), "continueButton", "continueButtonHighlight");
            continueButton.isEnabled = false;
            moveableSprites = new List<IMoveable>();
            collideableSprites = new List<ICollideable>();
            Floor = new Sprite();
            Floor.Load(content, new Vector2(0, 650), "floor");
            cursor = new Cursor(content);
            LoadLevel(content, 1);
            livesCounter = new ImageCounter(content.Load<Texture2D>("heartActive"), content.Load<Texture2D>("heartInactive"), new Vector2(Game.GraphicsDevice.Viewport.Width - 340, 10), new Vector2(50, 0), 5, 4);
            woodPileCounter = new ImageCounter(content.Load<Texture2D>("woodPileActive"), content.Load<Texture2D>("woodPileInactive"), new Vector2(Game.GraphicsDevice.Viewport.Width / 2 - 170, 10), new Vector2(30, 0), 10, 9);
            nailCounter = new ImageCounter(content.Load<Texture2D>("nailActive"), content.Load<Texture2D>("nailInactive"), new Vector2(80, 10), new Vector2(40, 0), 5, 4);
            font = content.Load<SpriteFont>("Font");
            PickupSound = content.Load<SoundEffect>("Pickup");
            camera = new Camera(Game.GraphicsDevice.Viewport);
            Input.camera = camera;
            m_content = content;
        }
        //Levels are loaded using .CSV files within the content folder. Positions of objects on screen are handled the same way for the most part
        private void LoadLevel(ContentManager content, int level)
        {
            currentLevel = level;

            string levelName = "Level" + level.ToString() + "Positions.csv";

            collideableSprites.Clear();
            moveableSprites.Clear();

            string[] lines;
            //Checks if the level exists using the path of folders
            if (File.Exists("../../../Levels/" + levelName))
            {
                lines = File.ReadAllLines("../../../Levels/" + levelName);
            }
            else if (File.Exists("Levels/" + levelName))
            {
                lines = File.ReadAllLines("Levels/" + levelName);
            }
                //Throws an exception if the level does not exist/cannot be found
            else { throw new FileNotFoundException("Level file not found!"); }
            //Checking the .CSV files within the level folders and reading/drawing positions using comma delimitation
            foreach (string line in lines)
            {
                //Commas seperate information in the .CSV files
                string[] values = line.Split(',');
                //The first space comes first and is the name of the object
                if (!string.IsNullOrWhiteSpace(values[0]))
                {
                    float x;
                    float y;
                    //Here the second and third cells in the row are checked and used as the X and Y positions of the object
                    try
                    {
                        float.TryParse(values[1], out x);
                        float.TryParse(values[2], out y);
                    }
                    catch { continue; }
                    //Checking the first cell and drawing the correct object
                    if (values[0] == "Wall")
                    {
                        addCollideable(content, new Vector2(x, y), new Wall(true));
                    }
                    if (values[0] == "Platform")
                    {
                        addCollideable(content, new Vector2(x, y), new Platform(true));
                    }
                    else if (values[0] == "Ladder")
                    {
                        addCollideable(content, new Vector2(x, y), new Ladder(true));
                    }
                    else if (values[0] == "Player")
                    {
                        addMoveable(content, new Vector2(x, y), new Player(true, collideableSprites, moveableSprites, Game));
                    }
                    else if (values[0] == "Door")
                    {
                        var door = new Door(true, moveableSprites);
                        door.OnRepair += RepairCount;
                        addCollideable(content, new Vector2(x, y), door);
                    }
                    else if (values[0] == "WoodPile")
                    {
                        addCollideable(content, new Vector2(x, y), new WoodPile(true));
                    }
                    else if (values[0] == "BigWindow")
                    {
                        var window = new BigWindow(true, moveableSprites);
                        window.OnRepair += RepairCount;
                        addCollideable(content, new Vector2(x, y), window);
                    }
                    else if (values[0] == "SmallWindow")
                    {
                        var window = new SmallWindow(true, moveableSprites);
                        window.OnRepair += RepairCount;
                        addCollideable(content, new Vector2(x, y), window);
                    }
                }
            }
        }

        public void addCollideable(ContentManager content, Vector2 pos, CollideableSprite newSprite)
        {
            newSprite.Load(content, pos);
            collideableSprites.Add(newSprite);
        }
        public void addMoveable(ContentManager content, Vector2 pos, IMoveable newSprite)
        {
            newSprite.Load(content, pos);
            moveableSprites.Add(newSprite);
        }

        public override void Update(GameTime gameTime)
        {
            elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            cursor.SetCursor(Cursors.normal);
            continueButton.isEnabled = (repairs >= 15);
            isPaused = continueButton.isEnabled;

            if (moveableSprites.OfType<Player>().Count() > 0)
            {
                Player player = moveableSprites.OfType<Player>().First();
                livesCounter.SetCount(player.lives);
                woodPileCounter.SetCount(player.woodPiles);
                nailCounter.SetCount(player.nails);
            }
            //The current level can be reset using the R key (for testing only)
            if (Input.beenPressed(Keys.R))
            {
                LoadLevel(Game.Content, currentLevel);
            }
            //Levels can be skipped through using the Num Pad 0 key (for testing only)
            if (Input.beenPressed(Keys.NumPad0))
            {
                LoadLevel(Game.Content, currentLevel + 1);
            }
            //The game can be paused using the P key
            bool pPressed = Input.beenPressed(Keys.P);
            //If P is pressed the game will switch to the pause screen and freeze the game in the background
            if (pPressed)
            {
                Game.ScreenMgr.Switch(new PauseScreen(Game));
            }
            //Checking if big windows are destroyed and drawing an enemy at a 1 in 750 chance (60 x per second) if they are
            foreach (var bigWindow in collideableSprites.OfType<BigWindow>())
            {
                if (bigWindow.IsDestroyed() && random.Next(750) == 1)
                {
                    addMoveable(m_content, bigWindow.Hitbox.Location.ToVector2(), new NormalEnemy(true, moveableSprites.OfType<Player>().First(), collideableSprites, moveableSprites, new Vector2(0, 1)));
                }
                //Checking if big windows are destroyed and drawing a stronger enemy at a 1 in 3000 chance (60 x per second) if they are

                if (bigWindow.IsDestroyed() && random.Next(3000) == 1)
                {
                    addMoveable(m_content, bigWindow.Hitbox.Location.ToVector2(), new RedEnemy(true, moveableSprites.OfType<Player>().First(), collideableSprites, moveableSprites, new Vector2(0, 1)));
                }
                //Changing the cursor to the hammer if it is contained by a big window
                if (bigWindow.Hitbox.Contains((Input.WorldMousePosition)))
                {
                    cursor.SetCursor(Cursors.hammer);
                }
            }
            //See above comments
            foreach (var smallWindow in collideableSprites.OfType<SmallWindow>())
            {
                if (smallWindow.IsDestroyed() && random.Next(1300) == 1)
                {
                    addMoveable(m_content, smallWindow.Hitbox.Location.ToVector2(), new NormalEnemy(true, moveableSprites.OfType<Player>().First(), collideableSprites, moveableSprites, new Vector2(0, 1)));
                }
                if (smallWindow.IsDestroyed() && random.Next(3000) == 1)
                {
                    addMoveable(m_content, smallWindow.Hitbox.Location.ToVector2(), new RedEnemy(true, moveableSprites.OfType<Player>().First(), collideableSprites, moveableSprites, new Vector2(0, 1)));
                }
                if (smallWindow.Hitbox.Contains((Input.WorldMousePosition)))
                {
                    cursor.SetCursor(Cursors.hammer);
                }
            }
            //See above comments
            foreach (var door in collideableSprites.OfType<Door>())
            {
                if (door.IsDestroyed() && random.Next(900) == 1)
                {
                    addMoveable(m_content, door.Hitbox.Location.ToVector2(), new NormalEnemy(true, moveableSprites.OfType<Player>().First(), collideableSprites, moveableSprites, new Vector2(0, 1)));
                }
                if (door.IsDestroyed() && random.Next(3000) == 1)
                {
                    addMoveable(m_content, door.Hitbox.Location.ToVector2(), new RedEnemy(true, moveableSprites.OfType<Player>().First(), collideableSprites, moveableSprites, new Vector2(0, 1)));
                }
                if (door.Hitbox.Contains((Input.WorldMousePosition)))
                {
                    cursor.SetCursor(Cursors.hammer);
                }
            }
            //Setiing the cursor to the sword if it is hovering over an enemy
            foreach (var enemy in moveableSprites.OfType<Enemy>())
            {
                if (enemy.Hitbox.Contains((Input.WorldMousePosition)))
                {
                    cursor.SetCursor(Cursors.sword);
                }
            }
            foreach (var redEnemy in moveableSprites.OfType<RedEnemy>())
            {
                if (redEnemy.Hitbox.Contains((Input.WorldMousePosition)))
                {
                    cursor.SetCursor(Cursors.sword);
                }
            }
            //If the player clicks on a nail within the world and the nail counter is not full it is collected and added to the counter
            foreach (var nail in collideableSprites.OfType<Nail>())
            {
                if (nail.Hitbox.Contains(Input.WorldMousePosition) && Input.isMouseJustClicked() && m_player.nails < 5)
                {
                    nail.Destroyed = true;
                    m_player.nails++;
                    PickupSound.Play();
                }
            }
            foreach (var woodPile in collideableSprites.OfType<WoodPile>())
            {
                if (woodPile.Hitbox.Contains(Input.WorldMousePosition) && Input.isMouseJustClicked() && m_player.woodPiles < 10)
                {
                    woodPile.Destroyed = true;
                    m_player.woodPiles++;
                    PickupSound.Play();
                }
            }
            //Game runs if it is not paused (WHODDA THUNK IT?)
            if (isPaused == false)
            {
                foreach (var moveable in moveableSprites)
                {
                    moveable.Update(gameTime);
                }
                foreach (var collidable in collideableSprites)
                {
                    collidable.Update(gameTime);
                }
            }
          //The continue button can be clicked at the end of levels. If the player is not on level 5, the next level is loaded, else, the highscore entry screen is loaded
            if (continueButton.IsClicked())
            {
                if (currentLevel < 5)
                {
                    LoadLevel(Game.Content, currentLevel + 1);
                    repairs = 0;
                }
                else if (currentLevel > 4 && continueButton.IsClicked())
                {
                    Game.ScreenMgr.Switch(new PlayerEntry(Game, (int)elapsedTime));
                }
            }
            collideableSprites.RemoveAll(sprite => sprite.Destroyed);
            moveableSprites.RemoveAll(sprite => sprite.Destroyed);
            camera.Update(m_player.Hitbox.Location.ToVector2());
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(transformMatrix:camera.GetViewMatrix());

            Floor.Draw(spriteBatch);

            foreach (var collidable in collideableSprites)
            {
                collidable.Draw(spriteBatch);
            }
            foreach (var moveable in moveableSprites)
            {
                moveable.Draw(spriteBatch);
            }
            foreach (var enemy in moveableSprites.OfType<Enemy>())
            {
                enemy.Draw(spriteBatch);
            }
            foreach (var redEnemy in moveableSprites.OfType<RedEnemy>())
            {
                redEnemy.Draw(spriteBatch);
            }
            spriteBatch.End();

            spriteBatch.Begin();
            cursor.Draw(spriteBatch);
            continueButton.Draw(spriteBatch);
            livesCounter.Draw(spriteBatch);
            woodPileCounter.Draw(spriteBatch);
            nailCounter.Draw(spriteBatch);
            cursor.Draw(spriteBatch);
            TimeSpan time = new TimeSpan(0, 0, (int)elapsedTime);
            spriteBatch.End();
        }
    }
}
