using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheWizardOfKama
{
    class LevelGenerator
    {
        int levelNumber = -1;
        public int LevelNumber { get { return levelNumber; } }
        public const int FinalLevel = 4;
        public const int TrainingLevel = 0;
        int backgroundTexture;
        Random random = new Random();      

        public int GenerateLevel()
        {
            levelNumber++;
            if (levelNumber == TrainingLevel)
                backgroundTexture = 0;
            else
                backgroundTexture = random.Next(FinalLevel - levelNumber);

            return backgroundTexture;
        }
    }
}
