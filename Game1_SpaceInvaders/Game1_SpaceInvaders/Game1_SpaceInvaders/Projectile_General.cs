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
    public class Projectile_General
    {
        public int projectileSpeed;
        public Rectangle projectileHitBox;
        public bool explosive;
        public Rectangle projectileExplosion;
        public bool collided;

        public Projectile_General(int Speed, bool Explosive, int XStart, int YStart)
        {
            projectileSpeed = Speed;
            projectileHitBox = new Rectangle(XStart, YStart, 3, 8);
            explosive = Explosive;
            projectileExplosion = new Rectangle(0, 0, 20, 20);
            collided = false;
        }

        public void Update(Game1 mainGame)
        {
            projectileHitBox.Location = new Point(projectileHitBox.X, projectileHitBox.Y + projectileSpeed);

            if (projectileHitBox.Y < 0 || projectileHitBox.Y > mainGame.bufferHeight)
                mainGame.unCollided.Remove(this);
        }

        public void Draw(SpriteBatch spriteBatch, Game1 mainGame)
        {
            if (!collided)
                spriteBatch.Draw(mainGame.alienTexture, projectileHitBox, Color.White);
            else
                spriteBatch.Draw(mainGame.alienTexture, projectileExplosion, Color.White);

        }
    }
}
