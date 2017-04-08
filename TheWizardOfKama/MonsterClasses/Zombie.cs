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
    class Zombie : Monster
    {
        const float idlePerFrame = 150;
        const float runPerFrame = 40;
        const float attackPerFrame = 100;
        const int attackStartFrame = 0;
        const int attackEndFrame = 5;
        const int hurtStartFrame = 6;
        const int hurtEndFrame = 10;
        const float hurtPerFrame = 120;
        const int idleStartFrame = 12;
        const int idleEndFrame = 15;
        const int runStartFrame = 18;
        const int runEndFrame = 27;
        const int runSpeed = 10;
        public const int zombieColumns = 6;
        public const int zombieRows = 5;
        public const int zombieEffColumns = 1;
        public const int zombieEffRows = 8;
        const int effStartFrame = 0;
        public const int effEndFrame = 7;
        const float effPerFrame = 80;
        const float startXPosition = 1300;
        const float startYPosition = 450;

        public Zombie() : base()
        {
            position = new Vector2(startXPosition, startYPosition);
            oldState = MonsterState.Idle;
            currentFrame = idleStartFrame;
            health = 50;
            attackPower = 7;
            experienceForPlayer = 450;
        }

        public int Move(GameTime gameTime, MonsterState zombieState)
        {
            animationTimer += gameTime.ElapsedGameTime.Milliseconds;

            switch (zombieState)
            {
                case MonsterState.Run:
                    if (oldState != MonsterState.Run)
                        currentFrame = runStartFrame;

                    if (animationTimer >= runPerFrame)
                    {
                        animationTimer = 0;
                        position.X -= runSpeed;
                        currentFrame++;
                        if (currentFrame > runEndFrame)
                            currentFrame = runStartFrame + 5;
                    }
                    break;

                case MonsterState.Attack:
                    if (oldState != MonsterState.Attack)
                        currentFrame = attackStartFrame;

                    if (animationTimer >= attackPerFrame)
                    {
                        animationTimer = 0;
                        currentFrame++;
                        if (currentFrame > attackEndFrame)
                            currentFrame = attackStartFrame;
                    }
                    break;

                case MonsterState.Hurt:
                    if (EndOfLife)
                    {
                        if (oldState != MonsterState.Hurt)
                            currentFrame = effStartFrame;

                        if (animationTimer > effPerFrame)
                        {
                            animationTimer = 0;
                            currentFrame++;
                        }
                    }
                    else
                    {
                        if (oldState != MonsterState.Hurt)
                            currentFrame = hurtStartFrame;

                        if (animationTimer >= hurtPerFrame)
                        {
                            animationTimer = 0;
                            currentFrame++;
                            if (currentFrame > hurtEndFrame)
                                currentFrame = hurtStartFrame;
                        }
                    }
                    break;

                default: // Idle
                    if (oldState != MonsterState.Idle)
                        currentFrame = idleStartFrame;

                    if (animationTimer > idlePerFrame)
                    {
                        animationTimer = 0;
                        currentFrame++;
                        if (currentFrame >= idleEndFrame)
                            currentFrame = idleStartFrame;
                    }
                    break;
            }
            oldState = zombieState;
            return currentFrame;
        }
    }
}
