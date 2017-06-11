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
    public class GeneralAnimation
    {
        public Rectangle sourceRect;
        public Rectangle animationRect;
        public float elapsed;
        public float delay;
        public int numbrFrames;
        public int frames;

        public GeneralAnimation(int Delay, Rectangle Animation, Rectangle SourceRect)
        {
            delay = Delay;
            animationRect = Animation;
            sourceRect = SourceRect;
        }

        public void LoadContent()
        {
            numbrFrames = animationRect.Width / sourceRect.Width;
        }

        public void Update(GameTime gameTime, bool Reverse)
        {
            elapsed += (float)gameTime.ElapsedGameTime.Milliseconds;

            if (!Reverse)
            {
                if (elapsed >= delay)
                {
                    if (frames >= numbrFrames - 1)
                        frames = 0;
                    else
                        frames++;
                    elapsed = 0;
                }
            }

            else
            {
                if (elapsed >= delay)
                {
                    if (frames <= 0)
                        frames = numbrFrames - 1;
                    else
                        frames--;
                    elapsed = 0;
                }
            }

            sourceRect = new Rectangle(sourceRect.Width * frames, 0, sourceRect.Width, animationRect.Height);

        }
    }
}
