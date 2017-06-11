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
using System.Text;

namespace Game1_SpaceInvaders
{
    public class PlayerClass
    {
        public GeneralAnimation playerMove;
        Texture2D playerMoveAnimation;
        public GeneralAnimation playerFire;
        Texture2D playerFireAnimation;
        public Rectangle playerRect;
        public int moveXSpeed;
        bool flipped = false;
        
        enum PlayerState
        {
            Firing,
            Moving,
            Stunned,
        }

        PlayerState playerState = PlayerState.Moving;

        public PlayerClass(Game1 mainGame, int PlayerWidth, int PlayerHeight)
        {
            moveXSpeed = 3;
            playerRect = new Rectangle(mainGame.bufferWidth / 2, mainGame.bufferHeight - PlayerHeight, PlayerWidth, PlayerHeight);
            playerMove = new GeneralAnimation(100, new Rectangle(0, 0, 256, 64), new Rectangle(0, 0, 64, 64));
            playerFire = new GeneralAnimation(50, new Rectangle(0, 0, 256, 64), new Rectangle(0, 0, 64, 64));
            playerMoveAnimation = mainGame.playerMoveAnimation;
            playerFireAnimation = mainGame.playerFireAnimation;
            playerMove.LoadContent();
            playerFire.LoadContent();
        }

        public void MoveX(int Direction)
        {
            if (Direction == 1)
                playerRect.X += moveXSpeed;
            else if (Direction == -1)
                playerRect.X += -moveXSpeed;
        }

        public int CenterX()
        {
            return playerRect.X + (playerRect.Width / 2);
        }

        public void Update(Game1 mainGame, GameTime gameTime)
        {
            if (playerState == PlayerState.Moving)
            {
                if (mainGame.newKeyboard.IsKeyDown(Keys.Left) && (playerRect.X > 0))
                {
                    MoveX(-1);
                    playerMove.Update(gameTime, false);
                    flipped = true;
                }
                else if (mainGame.newKeyboard.IsKeyDown(Keys.Right) && (playerRect.X + playerRect.Width < mainGame.bufferWidth))
                {
                    MoveX(1);
                    playerMove.Update(gameTime, false);
                    flipped = false;
                }

                if (mainGame.newKeyboard.IsKeyDown(Keys.Space) && !mainGame.oldKeyboard.IsKeyDown(Keys.Space))
                {
                    mainGame.unCollided.Add(new Projectile_General(-10, false, CenterX() - (mainGame.genericProjectile.projectileHitBox.Width / 2), playerRect.Y));
                    playerFire.frames = 0;
                    playerState = PlayerState.Firing;
                }
            }

            if (playerState == PlayerState.Firing)
            {
                playerFire.Update(gameTime, false);
            } 
            
        }

        public void Draw(Game1 mainGame, SpriteBatch spriteBatch)
        {
            if (playerState == PlayerState.Moving)
            {
                if (flipped)
                    spriteBatch.Draw(mainGame.playerMoveAnimation, playerRect, playerMove.sourceRect, Color.White, 0, new Vector2(0, 0), SpriteEffects.FlipHorizontally, 0f);
                else
                    spriteBatch.Draw(mainGame.playerMoveAnimation, playerRect, playerMove.sourceRect, Color.White);
            }

            if (playerState == PlayerState.Firing)
            {
                if (flipped)
                    spriteBatch.Draw(mainGame.playerFireAnimation, playerRect, playerFire.sourceRect, Color.White, 0, new Vector2(0, 0), SpriteEffects.FlipHorizontally, 0f);
                else
                    spriteBatch.Draw(mainGame.playerFireAnimation, playerRect, playerFire.sourceRect, Color.White);
                
                if (playerFire.frames == playerFire.numbrFrames - 1)
                    playerState = PlayerState.Moving;
            }
        }
    }
}
