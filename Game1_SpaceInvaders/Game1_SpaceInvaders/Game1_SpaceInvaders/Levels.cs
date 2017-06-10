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
    class Levels
    {

        //TO ADD A LEVEL 
        //ADD THE NAME OF THE LEVEL IN "enum Level" BEFFORE THE FINAL LEVEL
        //IN THE METHOD UpdateLevel(Game1 mainGame) ADD THE IF STATMENT FOR WHEN IT'S THE LEVEL
        //THEN ADD THE ENEMIES TO THE LEVEL BY REFERING TO THE AliensSetUp() METHOD

        enum Level
        {
            intro,
            first,
            second,
            third,
            fourth,
            final,
        }
        
        Level level = Level.first;

        public void Reset(Game1 mainGame, bool FullReset)
        {
            mainGame.currentAliens.Clear();
            mainGame.unCollided.Clear();
            if (FullReset)
                level = Level.first;
        }

        public void NextLevel(Game1 mainGame)
        {
            mainGame.currentAliens.Clear();
            mainGame.unCollided.Clear();
            if (level == Level.final)
                level = Level.intro;
            level++;

            UpdateLevel(mainGame);

            for (int i = 0; i < mainGame.currentAliens.Count(); i++)
                mainGame.currentAliens[i].LoadContent(mainGame.Content);
        }

        public void AliensSetUp(int numberRows, int numberColumns, int spaceBetween, int XStart, int YStart, int AlienType, Game1 mainGame)
        {
            for (int r = 0; r < numberRows; r++)
                for (int c = 0; c < numberColumns; c++)
                    mainGame.currentAliens.Add(new BasicAliens((c * (spaceBetween)) + XStart, (r * spaceBetween) + YStart, AlienType, mainGame));
        }

        public void UpdateLevel(Game1 mainGame)
        {
            //TO CREATE LINES OF ENEMIES USE THE METHOD:
            //AliensSetUp(Rows, Columns, Spacing, Starting X, Starting Y, Enemy Type(see BasicAliens class), mainGame)
            if (level == Level.first)
                AliensSetUp(1, 6, 50, 0, 0, 0, mainGame);
            if (level == Level.second)
                AliensSetUp(2, 8, 50, 0, 0, 0, mainGame);
            if (level == Level.third)
                AliensSetUp(3, 10, 50, 0, 0, 0, mainGame);
            if (level == Level.fourth)
                AliensSetUp(4, 12, 50, 0, 0, 0, mainGame);
        }
    }
}
