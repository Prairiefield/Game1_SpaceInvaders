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

        PlayerClass player1;
        Projectile_General genericProjectile = new Projectile_General(0,false,0,0);

        public List<BasicAliens> currentAliens = new List<BasicAliens>();
        public Texture2D alienTexture;

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

            player1 = new PlayerClass(this, 20, 40);

            spriteBatch = new SpriteBatch(GraphicsDevice);

            alienTexture = Content.Load<Texture2D>("blackMana");
            blueColor = Content.Load<Texture2D>("blueMana");


            level.UpdateLevel(this);
            for (int i = 0; i < currentAliens.Count(); i++)
                currentAliens[i].LoadContent(Content);
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            newKeyboard = Keyboard.GetState();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || newKeyboard.IsKeyDown(Keys.X))
                this.Exit();


            //IN COMBAT
            #region
            if (gameState == GameState.Combat)
            {
                //UPDATE BULLETS AND PLAYERS
                #region
                for (int i = 0; i < unCollided.Count(); i++)
                    unCollided[i].Update(this);
                player1.Update(this);
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
                    currentAliens[i].Update(gameTime);
                #endregion

                //FIRE PROJECTILE
                #region
                if (newKeyboard.IsKeyDown(Keys.Space) && !oldKeyboard.IsKeyDown(Keys.Space))
                    unCollided.Add(new Projectile_General(-10, false, player1.CenterX() - (genericProjectile.projectileHitBox.Width / 2), player1.playerRect.Y));
                #endregion

                //ALIEN HIT DETECTION
                #region
                for (int i = 0; i < unCollided.Count(); i++)
                    for (int j = 0; j < currentAliens.Count(); j++)
                        if (unCollided[i].projectileHitBox.Intersects(currentAliens[j].alienRect))
                        {
                            unCollided.RemoveAt(i);
                            currentAliens.RemoveAt(j);
                            for (int k = 0; k < currentAliens.Count(); k++)
                                currentAliens[k].moveSpeed = currentAliens[k].moveSpeed + 0.5f;
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
                    spriteBatch.Draw(alienTexture, unCollided[i].projectileHitBox, Color.White);

                spriteBatch.Draw(alienTexture, player1.playerRect, Color.White);

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
                spriteBatch.DrawString(endText, "Victory", new Vector2(0, 0), Color.Black);
                spriteBatch.DrawString(endText, "Press C to continue", new Vector2(0, 35), Color.Black);
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

            }
            #endregion

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
