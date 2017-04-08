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
    class Lander : Monster
    {
        const float idlePerFrame = 80;
        const int idleStartFrame = 0;
        const int idleEndFrame = 7;
        const int effStartFrame = 0;
        public const int effEndFrame = 8;
        const float effPerFrame = 80;
        const int hurtStartFrame = 8;
        const int hurtEndFrame = 9;
        const float hurtPerFrame = 80;
        const float runPerFrame = 30;
        const int runSpeed = 10;
        int runMultiplier;
        const float maxAttckDistance = 600;
        float attackDistance;
        public const int landerColumns = 4;
        public const int landerRows = 3;
        public const int landerEffColumns = 3;
        public const int landerEffRows = 3;
        const float startXPosition = 1000;
        const float startYPosition = 400;
        Random random = new Random();

        public Lander() : base()
        {
            position = new Vector2(startXPosition, startYPosition);
            oldState = MonsterState.Idle;
            currentFrame = idleStartFrame;
            health = 70;
            attackPower = 9;
            experienceForPlayer = 700;
        }

        public int Move(GameTime gameTime, MonsterState landerState, Directions direction)
        {
            animationTimer += gameTime.ElapsedGameTime.Milliseconds;

            switch (landerState)
            {
                case MonsterState.Run:
                    if (animationTimer >= runPerFrame)
                    {
                        animationTimer = 0;
                        runMultiplier = random.Next(1, 3);
                        if (direction == Directions.Left)
                            position.X -= runMultiplier * runSpeed;
                        else
                            position.X += runMultiplier * runSpeed;

                        currentFrame++;
                        if (currentFrame > idleEndFrame)
                            currentFrame = idleStartFrame;
                    }
                    break;

                case MonsterState.Attack:
                    if (oldState != MonsterState.Attack)
                        attackDistance = 0;

                    if (animationTimer >= runPerFrame)
                    {
                        animationTimer = 0;
                        if (attackDistance >= maxAttckDistance)
                        {
                            float currDistance = 10 * runSpeed;
                            attackDistance = 0;
                            position.X -= currDistance;
                        }
                        else
                        {
                            if (direction == Directions.Right)
                            {
                                float currDistance = runSpeed;
                                attackDistance += currDistance;
                                position.X += currDistance;
                            }
                        }

                        currentFrame++;
                        if (currentFrame > idleEndFrame)
                            currentFrame = idleStartFrame;
                    }
                    break;

                case MonsterState.Hurt:
                    if (endOfLife)
                    {
                        if (oldState != MonsterState.Hurt)
                            currentFrame = effStartFrame;

                        if (animationTimer >= effPerFrame)
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

                default: //Idle or Cast

                    if (animationTimer >= idlePerFrame)
                    {
                        animationTimer = 0;
                        currentFrame++;
                        if (currentFrame > idleEndFrame)
                            currentFrame = idleStartFrame;
                    }
                    break;
            }
            oldState = landerState;
            return currentFrame;
        }
    }
}
