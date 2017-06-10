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
    class GeneralAnimation
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

        public void LoadContent(ContentManager Content)
        {
            numbrFrames = animationRect.Width / sourceRect.Width;

        }

        public void Update(GameTime gameTime)
        {
            elapsed += (float)gameTime.ElapsedGameTime.Milliseconds;
            if (elapsed >= delay)
            {
                if (frames >= numbrFrames)
                    frames = 0;
                else
                    frames++;
                elapsed = 0;
            }

            sourceRect = new Rectangle((animationRect.Width / numbrFrames) * frames, 0, (animationRect.Width / numbrFrames), animationRect.Height);
        }
    }
}
