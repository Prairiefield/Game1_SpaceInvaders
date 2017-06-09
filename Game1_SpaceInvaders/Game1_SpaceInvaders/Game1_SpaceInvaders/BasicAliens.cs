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
            }
        }

        public void MoveX()
        {
            if (goingRight)
                alienRect.Location = new Point((int)(alienRect.X + moveSpeed), alienRect.Y);
            else
                alienRect.Location = new Point((int)(alienRect.X - moveSpeed), alienRect.Y);
        }

        public void MoveY()
        {
            alienRect.Location = new Point(alienRect.X, alienRect.Y + 5);
        }

        public void HitSide()
        {
            this.MoveY();
            goingRight = !goingRight;
            this.MoveX();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(alienTexture, alienRect, Color.White);
        }
    }
}
