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
    class Orc : Monster
    {
        const float idlePerFrame = 150;
        const float runPerFrame = 70;
        const float attackPerFrame = 100;
        //const int attackStartFrame = 14;
        //const int attackEndFrame = 20;
        const int prepareStartFrame = 14;
        const int prepareEndFrame = 18;
        const int attackStartFrame = 19;
        const int attackEndFrame = 20;
        const int hurtStartFrame = 21;
        const int hurtEndFrame = 27;
        const float hurtPerFrame = 120;
        const int idleStartFrame = 0;
        const int idleEndFrame = 6;
        const int runStartFrame = 7;
        const int runEndFrame = 13;
        const int runSpeed = 10;
        public const int orcColumns = 7;
        public const int orcRows = 4;
        public const int orcEffColumns = 1;
        public const int orcEffRows = 7;
        const int effStartFrame = 0;
        public const int effEndFrame = 6;
        const float effPerFrame = 80;
        const float startXPosition = 700;
        const float startYPosition = 0;

        public Orc() : base()
        {
            position = new Vector2(startXPosition, startYPosition);
            oldState = MonsterState.Idle;
            currentFrame = idleStartFrame;
            health = 110;
            attackPower = 15;
            experienceForPlayer = 3000;
        }

        public int Move(GameTime gameTime, MonsterState orcState)
        {
            animationTimer += gameTime.ElapsedGameTime.Milliseconds;

            switch (orcState)
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
                            currentFrame = runStartFrame;
                    }
                    break;

                case MonsterState.Prepare:
                    if (oldState != MonsterState.Prepare)
                        currentFrame = prepareStartFrame;

                    if (animationTimer >= attackPerFrame)
                    {
                        animationTimer = 0;
                        if (currentFrame < prepareEndFrame)
                            currentFrame++;
                    }
                    break;

                case MonsterState.Attack:
                    if (oldState != MonsterState.Attack)
                        currentFrame = attackStartFrame;

                    if (animationTimer >= attackPerFrame)
                    {
                        animationTimer = 0;
                        currentFrame++;
                        /*if (currentFrame < attackEndFrame)
                            currentFrame++;*/
                        if (currentFrame > attackEndFrame)
                            currentFrame = prepareStartFrame;
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
            oldState = orcState;
            return currentFrame;
        }
    }
}
