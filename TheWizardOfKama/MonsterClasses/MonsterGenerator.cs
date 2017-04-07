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
    class MonsterGenerator
    {
        //* Level data *
        int screenWidth;
        int screenHeight;
        //* Drawing data *
        Texture2D[] monsterTextures;
        Texture2D[] monsterEffTextures;
        Texture2D drawTexture;
        Texture2D spellTexture;
        Texture2D[] healthBarTexture = new Texture2D[2];
        Rectangle sourceRectangle;
        Rectangle destinationRectangle;
        Rectangle healthBarSrcRect;
        Rectangle healthBarFrameSrcRect;
        Rectangle healthBarDesRect;
        Rectangle healthBarFrameDesRect;
        float healthDrawingFactor;
        int healthBarDrawingDispl;
        int monTextureWidth;
        int monTextureHeight;
        int monTextureColumns;
        int monTextureRows;
        int effTextureWidth;
        int effTextureHeight;
        int effTextureColumns;
        int effTextureRows;
        int monsterNumber;
        int monsterNumSpawn;
        int currentFrame;
        const int respawnStartFrame = 0;
        const int respawnEndFrame = 5;
        const int respawnColumns = 6;
        const int respawnRows = 1;
        bool isLoading = false;
        //* Timers *
        float animationTimer;
        float getHurtTimer;
        float respawnTimer;
        float deathLastTimer;
        const float hurtTime = 1000;
        const float respawnTime = 2000;
        const float deathLastTime = 1000;
        const float zombieChangeStateTime = 1000;
        const float birdyChangeStateTime = 350;
        const float landerChangeStateTime = 500;
        const float respawnPerFrame = 40;
        //* Monsters classes *
        Monster activeMonster;
        public Monster ActiveMonster { get { return activeMonster; } }
        Zombie zombie;
        Birdy birdy;
        Lander lander;
        List<Monster> monsterList = new List<Monster>();
        public int MonstersLeft { get { return monsterList.Count(); } }
        List<LanderSpell> spells = new List<LanderSpell>();
        public List<LanderSpell> Spells { get { return spells; } }
        Directions direction;
        public Directions Direction { get { return direction; } }
        MonsterState monsterState = MonsterState.Dead;
        public MonsterState MonsterState { get { return monsterState; } }
        MonsterState oldState = MonsterState.Dead;
        //* Collision data *
        Rectangle zombieRect;
        public Rectangle ZombieRect { get { return zombieRect; } }
        Circle birdyCircle;
        public Circle BirdyCircle { get { return birdyCircle; } }
        Circle landerCircle;
        public Circle LanderCircle { get { return landerCircle; } }
        bool isWizardShieldActive = false;
        //* Other data *
        Random random = new Random();
        Vector2 wizardPosition;
        bool areAllMonstersDead = false;
        //***********************************************************************

        public MonsterGenerator(Texture2D[] monsterTextures, Texture2D spellTexture, Texture2D[] monsterEffTextures, Texture2D[] healthBarTexture, int screenWidth, int screenHeight)
        {
            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;
            this.spellTexture = spellTexture;
            this.monsterEffTextures = monsterEffTextures;
            this.healthBarTexture = healthBarTexture;
            //monsterList = new List<Monster>();
            monsterNumber = random.Next(1,1);
            this.monsterTextures = new Texture2D[monsterNumber];
            for (int i = 0; i < monsterNumber; i++)
            {
                switch (random.Next(0,1))
                {
                    case 0:
                        monsterList.Add(new Zombie());
                        this.monsterTextures[i] = monsterTextures[0];
                        break;
                    case 1:
                        monsterList.Add(new Birdy());  
                        this.monsterTextures[i] = monsterTextures[1];
                        break;
                    case 2:
                        monsterList.Add(new Lander());  
                        this.monsterTextures[i] = monsterTextures[2];
                        break;
                }
            }
        }
        public MonsterGenerator()
        { }

        public bool ControlMonster(GameTime gameTime, Vector2 wizardPosition)
        {
            this.wizardPosition = wizardPosition;
            animationTimer += gameTime.ElapsedGameTime.Milliseconds;
            if ((monsterState == MonsterState.Dead) && (monsterList.Count > 0))
            {
                deathLastTimer += gameTime.ElapsedGameTime.Milliseconds;
                if ((deathLastTimer >= deathLastTime && activeMonster is Zombie) || activeMonster == null || activeMonster is Birdy || activeMonster is Lander)
                {
                    monsterNumSpawn = monsterList.Count - 1;
                    respawnTimer += gameTime.ElapsedGameTime.Milliseconds;
                    if (respawnTimer >= respawnTime)
                    {
                        LoadNextMonster(gameTime);
                        respawnTimer = 0;
                        deathLastTimer = 0;
                        isLoading = false;
                        healthDrawingFactor = (float)healthBarTexture[0].Width / (float)activeMonster.Health;
                    }
                    else
                    {
                        if (!isLoading)
                        {
                            monTextureColumns = respawnColumns;
                            monTextureRows = respawnRows;
                            drawTexture = monsterEffTextures[3];
                            monTextureWidth = drawTexture.Width / monTextureColumns;
                            monTextureHeight = drawTexture.Height / monTextureRows;
                            isLoading = true;
                        }

                        if (animationTimer >= respawnPerFrame)
                        {
                            currentFrame++;
                            animationTimer = 0;

                            if (currentFrame > respawnEndFrame)
                                currentFrame = respawnStartFrame;
                        }
                    }
                }
            }
            else if ((monsterState != MonsterState.Dead) && (monsterList.Count > 0))
            {
                // ******** Zombie control section ********
                if (activeMonster is Zombie)
                {
                    ControlZombie(gameTime);
                }

                // ******** Birdy control section ********
                else if (activeMonster is Birdy)
                {
                    ControlBirdy(gameTime);
                }

                // ******** Lander control section ********
                else if (activeMonster is Lander)
                {
                    ControlLander(gameTime);
                }
            }
            else if ((monsterState == MonsterState.Dead) && (monsterList.Count == 0))
            {
                areAllMonstersDead = true;
            }

            // update monster spell
            ControlMonsterSpell(gameTime);

            // initialize wizard shield flag
            isWizardShieldActive = false;

            // return level state
            return areAllMonstersDead;
        }

        private void LoadNextMonster(GameTime gameTime)
        {
            animationTimer = 0;
            activeMonster = monsterList[monsterList.Count - 1];
            monsterState = MonsterState.Idle;
            if (activeMonster is Zombie)
            {
                zombie = activeMonster as Zombie;
                monTextureColumns = Zombie.zombieColumns;
                monTextureRows = Zombie.zombieRows;
                currentFrame = zombie.Move(gameTime, monsterState);
                healthBarDrawingDispl = 80;
            }
            else if (activeMonster is Birdy)
            {
                birdy = activeMonster as Birdy;
                monTextureColumns = Birdy.birdyColumns;
                monTextureRows = Birdy.birdyRows;
                currentFrame = birdy.Move(gameTime, monsterState);
                healthBarDrawingDispl = 0;
            }
            else
            {
                lander = activeMonster as Lander;
                monTextureColumns = Lander.landerColumns;
                monTextureRows = Lander.landerRows;
                currentFrame = lander.Move(gameTime, monsterState, Directions.Left);
                healthBarDrawingDispl = 150;
            }

            drawTexture = monsterTextures[monsterNumber - 1];
            monTextureWidth = drawTexture.Width / monTextureColumns;
            monTextureHeight = drawTexture.Height / monTextureRows;
        }

        private void ControlZombie(GameTime gameTime)
        {
            // detect if zombie should get hurt/dead state
            if (monsterState == MonsterState.Hurt)
            {
                // zombie is destroyed - gets hit multiple times by wizard's spell
                if (zombie.EndOfLife)
                {
                    // zombie wasn't in hurt state OR zombie was in hurt state and wizard destroys zombie
                    if (oldState != MonsterState.Hurt || (oldState == MonsterState.Hurt && !zombieRect.IsEmpty))
                    {
                        // change texture to explosion effect sprite and calculate new texture dimensions
                        drawTexture = monsterEffTextures[0];
                        effTextureColumns = Zombie.zombieEffColumns;
                        effTextureRows = Zombie.zombieEffRows;
                        effTextureWidth = drawTexture.Width / effTextureColumns;
                        effTextureHeight = drawTexture.Height / effTextureRows;
                        // calculate new position vector, so the effect could occur in the middle of the monster texture
                        zombie.CalculateEffectPosition(monTextureWidth, monTextureHeight, effTextureWidth, effTextureHeight);
                        // place effect texture as monster texture so it could be drawn
                        monTextureColumns = effTextureColumns;
                        monTextureRows = effTextureColumns;
                        monTextureWidth = effTextureWidth;
                        monTextureHeight = effTextureHeight;
                        // draw an empty rectangle so that no collision was detected with the monster
                        zombieRect = new Rectangle();
                    }
                }
                // zombie is hurt, but not destroyed - gets hit by a spell
                else
                {
                    getHurtTimer += gameTime.ElapsedGameTime.Milliseconds;
                }
            }
            // perform other action than 'get hurt'
            else
            {
                // if wizard's shield is inactive perform different actions
                if (!isWizardShieldActive)
                {
                    if (animationTimer >= zombieChangeStateTime)
                    {
                        // if the distance between characters is lower than 250, change states between attack and idle
                        if ((zombie.Position.X - wizardPosition.X) <= 250)
                        {
                            if (monsterState == MonsterState.Idle)
                                monsterState = MonsterState.Attack;
                            else
                                monsterState = MonsterState.Idle;
                        }
                        // if the distance between characters is higher than 250, change states between run and idle
                        else
                        {
                            if (monsterState == MonsterState.Idle)
                                monsterState = MonsterState.Run;
                            else
                                monsterState = MonsterState.Idle;
                        }
                        animationTimer = 0;
                    }
                    // change of state peformed outside the timer, so that the monster always stop before the character
                    else
                    {
                        // when distance between charcaters is lower than 200 and monster was running to wizard -> stand for a while
                        if ((zombie.Position.X - wizardPosition.X) <= 200 && monsterState == MonsterState.Run)
                        {
                            monsterState = MonsterState.Idle;
                        }
                    }
                }
                // when wizard's shield is active - stand (idle)
                else
                    monsterState = MonsterState.Idle;
            }

            oldState = monsterState;
            currentFrame = zombie.Move(gameTime, monsterState);

            if (zombie.EndOfLife)
            {
                if (currentFrame >= Zombie.effEndFrame)
                {
                    monsterList.RemoveAt(monsterList.Count - 1);
                    monsterState = MonsterState.Dead;
                }
            }
            else
            {
                if (monsterState == MonsterState.Hurt && getHurtTimer >= hurtTime)
                {
                    monsterState = MonsterState.Run;
                    getHurtTimer = 0;
                }
                zombieRect = new Rectangle((int)zombie.Position.X + 100, ((int)zombie.Position.Y), monTextureWidth, monTextureHeight);
            }
        }

        private void ControlBirdy(GameTime gameTime)
        {
            // detect if birdy should get hurt/dead state
            if (monsterState == MonsterState.Hurt)
            {
                // birdy is destroyed - crashes with the wizard or gets hit multiple times by wizard's spell
                if (birdy.EndOfLife)
                {
                    // birdy hits the wizard OR birdy was in hurt state and wizard runs into birdy or destroys it
                    if (oldState != MonsterState.Hurt || (oldState == MonsterState.Hurt && birdyCircle.Radius > 0))
                    {
                        // change texture to explosion effect sprite and calculate new texture dimensions
                        drawTexture = monsterEffTextures[1];
                        effTextureColumns = Birdy.birdyEffColumns;
                        effTextureRows = Birdy.birdyEffRows;
                        effTextureWidth = drawTexture.Width / effTextureColumns;
                        effTextureHeight = drawTexture.Height / effTextureRows;
                        // calculate new position vector, so the effect could occur in the middle of the monster texture
                        birdy.CalculateEffectPosition(monTextureWidth, monTextureHeight, effTextureWidth, effTextureHeight);
                        // place effect texture as monster texture so it could be drawn
                        monTextureColumns = effTextureColumns;
                        monTextureRows = effTextureColumns;
                        monTextureWidth = effTextureWidth;
                        monTextureHeight = effTextureHeight;
                        // draw an empty circle so that no collision was detected with the monster
                        birdyCircle = new Circle();
                    }
                }
                // birdy is hurt, but not destroyed - gets hit by a spell
                else
                {
                    getHurtTimer += gameTime.ElapsedGameTime.Milliseconds;
                }
            }
            // perform other action than 'get hurt'
            else
            {
                // if wziard's shield is inactive fly into wizard
                if (!isWizardShieldActive)
                {
                    if (animationTimer >= birdyChangeStateTime && monsterState == MonsterState.Idle)
                    {
                        monsterState = MonsterState.Run;
                        animationTimer = 0;
                    }
                }
                // if wzaird's shield is active, stop before the shield
                else
                {
                    monsterState = MonsterState.Idle;
                }
            }

            oldState = monsterState;
            currentFrame = birdy.Move(gameTime, monsterState);

            if (birdy.EndOfLife)
            {
                if (currentFrame >= Birdy.effEndFrame)
                {
                    monsterList.RemoveAt(monsterList.Count - 1);
                    monsterState = MonsterState.Dead;
                }
            }
            else
            {
                if (monsterState == MonsterState.Hurt && getHurtTimer >= hurtTime)
                {
                    monsterState = MonsterState.Run;
                    getHurtTimer = 0;
                }
                birdyCircle = new Circle(new Vector2((birdy.Position.X + (monTextureWidth / 2)), (birdy.Position.Y + (monTextureHeight / 2))), monTextureWidth / 2);
            }
        }

        private void ControlLander(GameTime gameTime)
        {
            // detect if lander should get hurt/dead state
            if (monsterState == MonsterState.Hurt)
            {
                // lander is destroyed - crashes with the wizard or gets hit multiple times by wizard's spell
                if (lander.EndOfLife)
                {
                    // lander wasn't in hurt state OR was in hurt state and wizard destroys lander
                    if (oldState != MonsterState.Hurt || (oldState == MonsterState.Hurt && landerCircle.Radius > 0))
                    {
                        // change texture to explosion effect sprite and calculate new texture dimensions
                        drawTexture = monsterEffTextures[2];
                        effTextureColumns = Lander.landerEffColumns;
                        effTextureRows = Lander.landerEffRows;
                        effTextureWidth = drawTexture.Width / effTextureColumns;
                        effTextureHeight = drawTexture.Height / effTextureRows;
                        // calculate new position vector, so the effect could occur in the middle of the monster texture
                        lander.CalculateEffectPosition(monTextureWidth, monTextureHeight, effTextureWidth, effTextureHeight);
                        // place effect texture as monster texture so it could be drawn
                        monTextureColumns = effTextureColumns;
                        monTextureRows = effTextureColumns;
                        monTextureWidth = effTextureWidth;
                        monTextureHeight = effTextureHeight;
                        // draw an empty circle so that no collision was detected with the monster
                        landerCircle = new Circle();
                    }
                }
                // lander is hurt, but not destroyed - gets hit by a spell
                else
                {
                    getHurtTimer += gameTime.ElapsedGameTime.Milliseconds;
                }
            }
            // perform other action than 'get hurt'
            else
            {
                // if wizard's shield is inactive perform standard Lander's actions
                if (!isWizardShieldActive)
                {
                    if (animationTimer >= landerChangeStateTime)
                    {
                        if ((lander.Position.X - wizardPosition.X) <= 250)
                        {
                            if (monsterState == MonsterState.Attack)
                                monsterState = MonsterState.Run;
                            else
                            {
                                monsterState = MonsterState.Attack;
                            }
                            direction = Directions.Right;
                        }
                        else
                        {
                            do
                            {
                                monsterState = (MonsterState)random.Next(3);
                            } while (monsterState == oldState);
                            direction = (Directions)random.Next(2);
                            if (monsterState == MonsterState.Cast)
                            {
                                float castHightMod = random.Next(1, 4);
                                spells.Add(new LanderSpell("landerSpell", 1, spellTexture, lander.Position.X, (lander.Position.Y + 80 * castHightMod), gameTime, 0, 0, 5, 4, 1, 10));
                            }
                        }
                        animationTimer = 0;
                    }
                    // change of state peformed outside the timer, so that the monster always stop before the character
                    else
                    {
                        if ((lander.Position.X - wizardPosition.X) <= 250 && monsterState == MonsterState.Run)
                        {
                            monsterState = MonsterState.Attack;
                            direction = Directions.Right;
                        }
                    }
                }
                // if wizard's shield is active, stop before it
                else
                    monsterState = MonsterState.Idle;
            }

            oldState = monsterState;
            if (lander.Position.X + monTextureWidth >= screenWidth && (lander.Position.X - wizardPosition.X) > 250)
            {
                monsterState = MonsterState.Run;
                direction = Directions.Left;
            }
            else if (lander.Position.X + monTextureWidth >= screenWidth && (lander.Position.X - wizardPosition.X) <= 250)
            {
                monsterState = MonsterState.Attack;
                direction = Directions.Left;

            }
            currentFrame = lander.Move(gameTime, monsterState, direction);

            if (lander.EndOfLife)
            {
                if (currentFrame >= Lander.effEndFrame)
                {
                    monsterList.RemoveAt(monsterList.Count - 1);
                    monsterState = MonsterState.Dead;
                }
            }
            else
            {
                if (monsterState == MonsterState.Hurt && getHurtTimer >= hurtTime)
                {
                    monsterState = MonsterState.Idle;
                    getHurtTimer = 0;
                }
                landerCircle = new Circle(new Vector2((lander.Position.X + (monTextureWidth / 2)), (lander.Position.Y + (monTextureHeight / 2))), monTextureWidth / 2);
            }
        }

        private void ControlMonsterSpell(GameTime gameTime)
        {
            //******** Lander's casting spell *******
            foreach (LanderSpell spell in spells)
            {
                if (spell.EndOfLife == true)
                {
                    if (spell.CalculatePosition())
                    {
                        spells.Remove(spell);
                        break;
                    }
                }
                else
                {
                    if (spell.Position.X <= (0 - spell.Width))
                    {
                        spells.Remove(spell);
                        break;
                    }
                    else
                    {
                        spell.CalculatePosition();
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (monsterList.Count > 0)
            {
                int row = (int)((float)currentFrame / (float)monTextureColumns);
                int column = currentFrame % monTextureColumns;
                sourceRectangle = new Rectangle(monTextureWidth * column, monTextureHeight * row, monTextureWidth, monTextureHeight);
                if (isLoading)
                    destinationRectangle = new Rectangle((int)monsterList[monsterNumSpawn].Position.X, (int)monsterList[monsterNumSpawn].Position.Y, monTextureWidth, monTextureHeight);
                else
                {
                    destinationRectangle = new Rectangle((int)activeMonster.Position.X, (int)activeMonster.Position.Y, monTextureWidth, monTextureHeight);
                    healthBarSrcRect = new Rectangle(0, 0, healthBarTexture[0].Width, healthBarTexture[0].Height);
                    healthBarFrameSrcRect = new Rectangle(0, 0, healthBarTexture[1].Width, healthBarTexture[1].Height);
                    healthBarDesRect = new Rectangle((int)activeMonster.Position.X + healthBarDrawingDispl + 2, (int)activeMonster.Position.Y - 28, (int)(activeMonster.Health * healthDrawingFactor), healthBarTexture[0].Height);
                    healthBarFrameDesRect = new Rectangle((int)activeMonster.Position.X + healthBarDrawingDispl, (int)activeMonster.Position.Y - 30, healthBarTexture[1].Width, healthBarTexture[1].Height);
                }
                spriteBatch.Begin();
                spriteBatch.Draw(drawTexture, destinationRectangle, sourceRectangle, Color.White);
                if (!isLoading)
                {
                    if (activeMonster.Health > 0)
                    {
                        spriteBatch.Draw(healthBarTexture[0], healthBarDesRect, healthBarSrcRect, Color.White);
                        spriteBatch.Draw(healthBarTexture[1], healthBarFrameDesRect, healthBarFrameSrcRect, Color.White);
                    }
                }
                spriteBatch.End();
            }
        }

        public void DrawMonsterSpells(SpriteBatch spriteBatch)
        {
            foreach (LanderSpell spell in spells)
                spell.Draw(spriteBatch);
        }

        public int HandleMonsterCollision(int mode, int hitPoints)
        {
            /* Modes:
            0 - for spells different than shield
            1 - for shield spell
            */            
            switch (mode)
            {
                case 0:
                    monsterState = MonsterState.Hurt;
                    activeMonster.GetHurt(hitPoints);
                    break;
                case 1:
                    isWizardShieldActive = true;
                    break;
            }

            if (activeMonster.EndOfLife)
                return activeMonster.ExpereinceForPlayer;
            else
                return 0;
        }

        public void HandleSpellCollision(int spellNumber)
        {
            spells[spellNumber].HandleCollsion(7, 19, 20);
        }
    }
}
