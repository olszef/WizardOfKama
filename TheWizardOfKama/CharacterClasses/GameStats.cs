using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheWizardOfKama
{
    class GameStats
    {
        int level;
        public int Level { get { return level; } }
        float playedTime = 0;
        public float PlayedTime { get { return playedTime; } }
        int noOfMonstersKilled = 0;
        public int NoOfMonstersKilled { get { return noOfMonstersKilled; } }
        bool isWon = false;
        public bool IsWon { get { return isWon; } }
        bool isEndGame = false;
        public bool IsEndGame { get { return isEndGame; } }

        public GameStats()
        { }

        public void SetFinalStats(int level, bool isWon, float playedTime)
        {
            isEndGame = true;
            this.level = level;
            this.isWon = isWon;
            this.playedTime = playedTime;
        }

        public void UpdateKilledMonstersNo(int noOfMonstersAtLevel)
        {
            noOfMonstersKilled += noOfMonstersAtLevel;
        }


    }
}
