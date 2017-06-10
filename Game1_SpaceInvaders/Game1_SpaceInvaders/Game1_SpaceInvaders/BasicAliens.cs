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
    public class BasicAliens
    {
        GeneralAnimation alienAnimation;
        public Rectangle alienRect;
        public Texture2D alienTexture;
        public float moveSpeed;
        public bool goingRight;

        public BasicAliens(int XPos, int YPos, int EnemyType, Game1 mainGame)
        {
            if (EnemyType == 0)
            {
                moveSpeed = 10;
                alienTexture = mainGame.blueColor;
                alienRect = new Rectangle(XPos, YPos, 20, 20);
                alienAnimation = new GeneralAnimation(200, new Rectangle(0,0,1,1), new Rectangle(0,0,1,1));
            }
        }

        public void LoadContent(ContentManager Content)
        {
            alienAnimation.LoadContent(Content);
        }

        public void MoveX()
        {
            if (goingRight)
                alienRect.X += (int)moveSpeed;
            else
                alienRect.X += (int)-moveSpeed;
        }

        public void MoveY()
        {
            alienRect.Y += 5;
        }

        public void HitSide()
        {
            this.MoveY();
            goingRight = !goingRight;
            this.MoveX();
        }

        public void Update(GameTime gameTime)
        {
            alienAnimation.Update(gameTime);       
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(alienTexture, alienRect, alienAnimation.sourceRect,Color.White);
        }
    }
}
