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
        public Rectangle playerRect;
        public int moveXSpeed;

        public PlayerClass(Game1 mainGame, int PlayerWidth, int PlayerHeight)
        {
            moveXSpeed = 5;
            playerRect = new Rectangle(mainGame.bufferWidth / 2, mainGame.bufferHeight - PlayerHeight, PlayerWidth, PlayerHeight);
        }

        public void MoveX(int Direction)
        {
            if (Direction == 1)
                playerRect.Location = new Point(playerRect.X + moveXSpeed, playerRect.Y);
            else if (Direction == -1)
                playerRect.Location = new Point(playerRect.X - moveXSpeed, playerRect.Y);
        }

        public int CenterX()
        {
            return playerRect.X + (playerRect.Width / 2);
        }

        public void Update(Game1 mainGame)
        {
            if (mainGame.newKeyboard.IsKeyDown(Keys.Left) && (playerRect.X > 0))
                MoveX(-1);
            else if (mainGame.newKeyboard.IsKeyDown(Keys.Right) && (playerRect.X + playerRect.Width < mainGame.bufferWidth))
                MoveX(1);
        }
    }
}
