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
    class Birdy : Monster
    {
        const int hurtStartFrame = 4;
        const int hurtEndFrame = 5;
        const float hurtPerFrame = 100;
        const int flyStartFrame = 0;
        const int flyEndFrame = 3;
        const float flyPerFrame = 20;
        const int flySpeed = 15;
        const int effStartFrame = 0;
        public const int effEndFrame = 30;
        const float effPerFrame = 20;
        public const int birdyColumns = 4;
        public const int birdyRows = 2;
        public const int birdyEffColumns = 8;
        public const int birdyEffRows = 4;
        const float startXPosition = 1300;
        const float startYPosition = 600;

        public Birdy() : base()
        {
            position = new Vector2(startXPosition, startYPosition);
            oldState = MonsterState.Idle;
            currentFrame = flyStartFrame;
            health = 15;
            attackPower = 15;
        }

        public int Move(GameTime gameTime, MonsterState birdyState)
        {
            animationTimer += gameTime.ElapsedGameTime.Milliseconds;

            switch (birdyState)
            {
                case MonsterState.Run:
                    if (oldState != MonsterState.Run)
                        currentFrame = flyStartFrame;

                    if (animationTimer > flyPerFrame)
                    {
                        position.X -= flySpeed;
                        animationTimer = 0;
                        currentFrame++;
                        if (currentFrame >= flyEndFrame)
                            currentFrame = flyStartFrame;
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

                        if (animationTimer > hurtPerFrame)
                        {
                            animationTimer = 0;
                            currentFrame++;
                            if (currentFrame > hurtEndFrame)
                                currentFrame = hurtStartFrame;
                        }
                    }
                    break;

                default: // idle
                    if (oldState != MonsterState.Idle)
                        currentFrame = flyStartFrame;

                    if (animationTimer > flyPerFrame)
                    {
                        animationTimer = 0;
                        currentFrame++;
                        if (currentFrame >= flyEndFrame)
                            currentFrame = flyStartFrame;
                    }
                    break;
            }
            oldState = birdyState;
            return currentFrame;
        }
    }
}
