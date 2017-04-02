using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Design;

namespace TheWizardOfKama
{
    abstract class Monster
    {
        protected int experienceForPlayer;
        public int ExpereinceForPlayer { get { return experienceForPlayer; } }
        protected int health;
        public int Health { get { return health; } }
        protected int attackPower;
        public int AttackPower { get { return attackPower; } }
        protected Vector2 position;
        public Vector2 Position { get { return position; } }
        protected int currentFrame;
        protected float animationTimer = 0;
        protected MonsterState oldState;
        protected bool endOfLife = false;
        public bool EndOfLife { get { return endOfLife; } }
                
        public void CalculateEffectPosition(int monWidth, int monHeight, int effWidth, int effHeight)
        {
            float xCenter = position.X + monWidth / 2;
            float yCenter = position.Y + monHeight / 2;

            float newXpos = xCenter - effWidth / 2;
            float newYpos = yCenter - effHeight / 2;

            position.X = newXpos;
            position.Y = newYpos;

            // assuming that all death-effects start from frame zero
            currentFrame = 0;
        }

        public bool GetHurt(int hitPoints)
        {
            health -= hitPoints;
            if (health <= 0)
            {
                endOfLife = true;
            }

            return endOfLife;
        }
    }
}
