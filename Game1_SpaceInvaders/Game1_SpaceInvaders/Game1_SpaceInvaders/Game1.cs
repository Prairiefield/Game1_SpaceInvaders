using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Game1_SpaceInvaders
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        float rawr = 5;

        PlayerClass player1;
        public Projectile_General genericProjectile = new Projectile_General(0,false,0,0);

        public List<BasicAliens> currentAliens = new List<BasicAliens>();
        public Texture2D alienTexture;
        public Texture2D alien1;
        public Texture2D alien2;
        public Texture2D alien3;

        public Texture2D playerMoveAnimation;
        public Texture2D playerFireAnimation;
        public Texture2D bulletTexture;

        public List<Projectile_General> unCollided = new List<Projectile_General>();
        public List<Projectile_General> collided = new List<Projectile_General>();

        public KeyboardState newKeyboard, oldKeyboard;

        public int bufferWidth;
        public int bufferHeight;

        SpriteFont basicText, endText;

        public Texture2D blueColor;

        Levels level = new Levels();

        bool showDebug = false;

        enum GameState
        {
            Combat,
            Victory,
            Defeat,
            Shop,
            Paused,
        }

        GameState gameState;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferHeight = 600;
            graphics.PreferredBackBufferWidth = 800;
            IsMouseVisible = true;
            graphics.IsFullScreen = false;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {

            gameState = GameState.Combat;
            basicText = Content.Load<SpriteFont>("BasicText");
            endText = Content.Load<SpriteFont>("EndText");

            bufferHeight = graphics.PreferredBackBufferHeight;
            bufferWidth = graphics.PreferredBackBufferWidth;

            player1 = new PlayerClass(this, 64, 64);

            spriteBatch = new SpriteBatch(GraphicsDevice);

            alienTexture = Content.Load<Texture2D>("blackMana");
            blueColor = Content.Load<Texture2D>("blueMana");
            alien1 = Content.Load<Texture2D>("Alien 1");
            alien2 = Content.Load<Texture2D>("Alien 2");
            alien3 = Content.Load<Texture2D>("Alien 3");
            playerMoveAnimation = Content.Load<Texture2D>("TankLR");
            playerFireAnimation = Content.Load<Texture2D>("TankFire");
            bulletTexture = Content.Load<Texture2D>("Bullet");

            level.UpdateLevel(this);
            for (int i = 0; i < currentAliens.Count(); i++)
                currentAliens[i].LoadContent();
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            newKeyboard = Keyboard.GetState();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || newKeyboard.IsKeyDown(Keys.X) || newKeyboard.IsKeyDown(Keys.Escape))
                this.Exit();

            if (newKeyboard.IsKeyDown(Keys.D) && !oldKeyboard.IsKeyDown(Keys.D))
                showDebug = !showDebug;

            //IN COMBAT
            #region
            if (gameState == GameState.Combat)
            {
                //UPDATE BULLETS AND PLAYERS
                #region
                for (int i = 0; i < unCollided.Count(); i++)
                    unCollided[i].Update(this);
                player1.Update(this, gameTime);
                #endregion

                //MOVE ALIENS
                #region
                //MOVE LEFT OR RIGHT
                for (int i = 0; i < currentAliens.Count(); i++)
                    currentAliens[i].MoveX();

                //ALIENS COLLIDING WITH SIDES
                for (int i = 0; i < currentAliens.Count(); i++)
                    if (!((currentAliens[i].alienRect.X + currentAliens[i].alienRect.Width <= bufferWidth) && (currentAliens[i].alienRect.X >= 0)))
                        for (int j = 0; j < currentAliens.Count(); j++)
                            currentAliens[j].HitSide();

                //UPDATE ALIEN TEXTURES
                for (int i = 0; i < currentAliens.Count(); i++)
                    currentAliens[i].Update(gameTime, this);
                #endregion

                //ALIEN HIT DETECTION
                #region
                for (int i = 0; i < unCollided.Count(); i++)
                    for (int j = 0; j < currentAliens.Count(); j++)
                        if (unCollided[i].projectileHitBox.Intersects(currentAliens[j].alienRect))
                        {
                            unCollided.RemoveAt(i);
                            currentAliens[j].health--;
                            for (int k = 0; k < currentAliens.Count(); k++)
                                currentAliens[k].moveSpeed = currentAliens[k].moveSpeed + 0.2f;
                            break;
                        }
                #endregion

                //VICTORY CONDITION
                #region
                if (currentAliens.Count() == 0)
                    gameState = GameState.Victory;
                #endregion

                //DEFEAT CONDITION
                #region
                for (int i = 0; i < currentAliens.Count(); i++)
                    if ((currentAliens[i].alienRect.Y + currentAliens[i].alienRect.Height) > bufferHeight)
                    {
                        gameState = GameState.Defeat;
                        break;
                    }
                #endregion

                //PAUSE GAME
                #region
                if (newKeyboard.IsKeyDown(Keys.P) && !oldKeyboard.IsKeyDown(Keys.P))
                    gameState = GameState.Paused;
                #endregion

            }
            #endregion

            //WHEN PAUSED
            #region
            if (gameState == GameState.Paused)
            {
                if (newKeyboard.IsKeyDown(Keys.C) && !oldKeyboard.IsKeyDown(Keys.C))
                    gameState = GameState.Combat;
            }
            #endregion

            //IN VICTORY SCREEN
            #region
            if (gameState == GameState.Victory)
            {
                if (newKeyboard.IsKeyDown(Keys.C))
                {
                    gameState = GameState.Combat;
                    rawr = 5;
                    level.NextLevel(this);
                }
            }
            #endregion

            //IN DEFEAT SCREEN
            #region
            if (gameState == GameState.Defeat)
                if (newKeyboard.IsKeyDown(Keys.C))
                {
                    level.Reset(this, true);
                    gameState = GameState.Combat;
                    level.UpdateLevel(this);
                }
            #endregion


            oldKeyboard = newKeyboard;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();

            //DRAW COMBAT
            #region
            if (gameState == GameState.Combat || gameState == GameState.Paused)
            {

                for (int i = 0; i < unCollided.Count(); i++)
                    unCollided[i].Draw(spriteBatch, this);

                player1.Draw(this, spriteBatch);

                for (int i = 0; i < currentAliens.Count(); i++)
                    currentAliens[i].Draw(spriteBatch);
            }
            #endregion

            //DRAW PAUSE SCREEN
            #region
            if (gameState == GameState.Paused)
            {
                spriteBatch.DrawString(endText, "Paused", new Vector2(0, 0), Color.Black);
                spriteBatch.DrawString(endText, "Press C to continue", new Vector2(0, 35),Color.Black);
            }
            #endregion

            //DRAW VICTORY SCREEN
            #region
            if (gameState == GameState.Victory)
            {
                rawr -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                spriteBatch.DrawString(endText, "Victory", new Vector2(0, 0), Color.Black);
                spriteBatch.DrawString(endText, "Press C to continue", new Vector2(0, 35), Color.Black);
                spriteBatch.DrawString(endText, Math.Round(rawr).ToString(), new Vector2(bufferWidth / 2, bufferHeight / 2), Color.Black);
                if (rawr <= 0)
                {
                    rawr = 5;
                    gameState = GameState.Combat;
                    level.NextLevel(this);
                }
            }
            #endregion

            //DRAW DEFEAT SCREEN
            #region
            if (gameState == GameState.Defeat)
            {
                spriteBatch.DrawString(endText, "Defeat", new Vector2(0, 0), Color.Black);
                spriteBatch.DrawString(endText, "Press C to restart", new Vector2(0, 35), Color.Black);
            }
            #endregion

            //DRAW DEBUGGING ITEMS
            #region
            if (showDebug)
            {
                spriteBatch.DrawString(basicText, player1.playerFire.frames.ToString(), new Vector2(0, 0), Color.Black);
                spriteBatch.DrawString(basicText, player1.playerFire.numbrFrames.ToString(), new Vector2(0, 35), Color.Black);
            }
            #endregion

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
