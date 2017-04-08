using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Design;
using Microsoft.Xna.Framework.Content;

namespace TheWizardOfKama
{
    class ActionScreen : GameScreen
    {
        List<Texture2D> backgrounds = new List<Texture2D>();
        Texture2D currentBackground;
        string backgroundForest_file;
        string backgroundCastle_file;
        string backgroundWindmill_file;
        string backgroundWinter_file;
        string backgroundField_file;
        Wizard wizard;
        MonsterGenerator monsterGenerator;
        LevelGenerator levelGenerator;
        private Texture2D wizardTexture;
        private string wizard_file;
        private Texture2D castingTexture;
        private string casting_file;
        private string lighting_file;
        private string water_file;
        private string gravity_file;
        private string shield_file;
        private string specialSpell_file;
        private string levelUpWaves_file;
        string endGate_file;
        private Texture2D[] spells = new Texture2D[5];
        Texture2D[] monsterTextures = new Texture2D[4];
        Texture2D[] monsterEffTextures = new Texture2D[5];
        Texture2D[] healthBarTexture = new Texture2D[2];
        Texture2D landerSpellTexture;
        Texture2D levelUpWaves;
        Texture2D endGate;
        string zombie_file;
        string zombieDeath_file;
        string birdy_file;
        string birdyExplosion_file;
        string lander_file;
        string orc_file;
        string orcDeath_file;
        string landerSpell_file;
        string landerExplosion_file;
        string monsterRespawn_file;
        string[] collisions;
        float actionTimer = 0;
        const float collisionPerFrame = 500;
        int gainedExp;
        bool levelUp = false;
        float levelUpTextTimer = 0;
        float levelUpAnimTimer = 0;
        const float levelUpTextShow = 2000;
        const float levelUpAnimationPerFrame = 40;
        int currentLvlUpFrame;
        int effectWidth;
        int effectHeight;
        const int levelUpEndFrame = 6;
        const int levelUpColumns = 7;
        const int levelUpRows = 1;
        Rectangle lvlUpSourceRectangle;
        Rectangle lvlUpDestinationRectangle;
        bool areAllMonstersDead = false;
        bool isWizardDead = false;
        // * Gate Texture Data *
        const int gateEndFrame = 15;
        const int gateColumns = 4;
        const int gateRows = 4;
        float gateAnimTimer = 0;
        Rectangle gateSourceRectangle;
        Rectangle gateDestinationRectangle;
        int currentgateFrame;
        int gateWidth;
        int gateHeight;
        const int gateXPosition = 1100;
        const int gateYPosition = 400;
        float totalPlayedTime = 0;
        string gameWinTitle = "VICTORY!";
        //********************************

        public ActionScreen(Game game, ContentManager content, SpriteBatch spriteBatch, string name) : base(game, content, spriteBatch, name)
        {
            Initalize();
            LoadContent();
            currentLvlUpFrame = 0;
            effectWidth = levelUpWaves.Width / levelUpColumns;
            effectHeight = levelUpWaves.Height / levelUpRows;
            gateWidth = endGate.Width / gateColumns;
            gateHeight = endGate.Height / gateRows;
        }

        private void LoadContent()
        {
            spriteFontNormal = content.Load<SpriteFont>("GameText24");
            spriteFontBig = content.Load<SpriteFont>("GameText56");
            backgrounds.Add(content.Load<Texture2D>(backgroundForest_file));
            backgrounds.Add(content.Load<Texture2D>(backgroundField_file));
            backgrounds.Add(content.Load<Texture2D>(backgroundWindmill_file));
            backgrounds.Add(content.Load<Texture2D>(backgroundWinter_file));
            backgrounds.Add(content.Load<Texture2D>(backgroundCastle_file));
            levelUpWaves = content.Load<Texture2D>(levelUpWaves_file);
            endGate = content.Load<Texture2D>(endGate_file);
            wizardTexture = content.Load<Texture2D>(wizard_file);
            castingTexture = content.Load<Texture2D>(casting_file);
            spells[0] = content.Load<Texture2D>(lighting_file);
            spells[1] = content.Load<Texture2D>(water_file);
            spells[2] = content.Load<Texture2D>(gravity_file);
            spells[3] = content.Load<Texture2D>(shield_file);
            spells[4] = content.Load<Texture2D>(specialSpell_file);
            monsterTextures[0] = content.Load<Texture2D>(zombie_file);
            monsterTextures[1] = content.Load<Texture2D>(birdy_file);
            monsterTextures[2] = content.Load<Texture2D>(lander_file);
            monsterTextures[3] = content.Load<Texture2D>(orc_file);
            monsterEffTextures[0] = content.Load<Texture2D>(zombieDeath_file);
            monsterEffTextures[1] = content.Load<Texture2D>(birdyExplosion_file);
            monsterEffTextures[2] = content.Load<Texture2D>(landerExplosion_file);
            monsterEffTextures[3] = content.Load<Texture2D>(monsterRespawn_file);
            monsterEffTextures[4] = content.Load<Texture2D>(orcDeath_file);
            landerSpellTexture = content.Load<Texture2D>(landerSpell_file);
            wizard = new Wizard(wizardTexture, castingTexture, spells, screenWidth, screenHeight);
            LoadNextLevel();
            healthBarTexture[0] = content.Load<Texture2D>("monsters/HealthBar");
            healthBarTexture[1] = content.Load<Texture2D>("monsters/HealthBarFrame");
            InitCollPairs();
        }

        private void Initalize()
        {
            backgroundField_file = "backgrounds/Field";
            backgroundWindmill_file = "backgrounds/Windmill";
            backgroundWinter_file = "backgrounds/Winter";
            backgroundCastle_file = "backgrounds/Castle";
            backgroundForest_file = "backgrounds/Forest";
            wizard_file = "character/BasicWizardMoves";
            casting_file = "character/casting";
            lighting_file = "character/lighting";
            water_file = "character/water";
            gravity_file = "character/gravity";
            shield_file = "character/shield";
            specialSpell_file = "character/specialSpell";
            zombie_file = "monsters/zombie2";
            zombieDeath_file = "monsters/zombieDeath";
            birdy_file = "monsters/birdy";
            birdyExplosion_file = "monsters/birdyExplosion";
            lander_file = "monsters/lander";
            landerExplosion_file = "monsters/landerExplosion";
            landerSpell_file = "monsters/landerAttack";
            orc_file = "monsters/BasicOrcMoves";
            orcDeath_file = "monsters/orcDeath";
            levelUpWaves_file = "character/LevelUpWaves";
            monsterRespawn_file = "monsters/respawnAnim";
            endGate_file = "other/EndGate";
            levelGenerator = new LevelGenerator();
        }

        public override GameStats Update(GameTime gameTime)
        {
            totalPlayedTime += gameTime.ElapsedGameTime.Milliseconds;
            isWizardDead = wizard.UpdateWizardMoves(gameTime);

            if (areAllMonstersDead && !isWizardDead)
            {
                if (levelGenerator.LevelNumber != LevelGenerator.FinalLevel)
                {
                    ShowGateToNextLvl(gameTime);
                    if (wizard.Position.X > gateXPosition && wizard.Position.Y > gateYPosition)
                    {
                        LoadNextLevel();
                    }
                }
                else
                {
                    endGameStats.SetFinalStats(wizard.Level, true, totalPlayedTime);
                }
            }
            areAllMonstersDead = monsterGenerator.ControlMonster(gameTime, wizard.Position);
            CheckCollisions(gameTime);
            if (levelUp)
                AnimateLevelUp(gameTime);
            if (isWizardDead)
                endGameStats.SetFinalStats(wizard.Level, false, totalPlayedTime);

            return endGameStats;
        }

        private void InitCollPairs()
        {
            collisions = new string[]
            {
                "WizardMonster",
                "WizardMonsterSpell",
                "SpellMonster",
                "WizSpellMonSpell"
            };
        }

        private void CheckCollisions(GameTime gameTime)
        {
            foreach (string collisionPair in collisions)
            {
                bool isCollision = false;
                switch (collisionPair)
                {
                    case "WizardMonster":
                        if (monsterGenerator.MonstersLeft > 0 && wizard.MoveState != MoveState.Dead)
                        {
                            if (monsterGenerator.ActiveMonster is Birdy)
                            {
                                if (CollisionDetector.CirclesIntersection(wizard.CharCircle, monsterGenerator.BirdyCircle))
                                {
                                    wizard.HandleWizardCollision(true, monsterGenerator.ActiveMonster.AttackPower);
                                    monsterGenerator.HandleMonsterCollision(0, monsterGenerator.ActiveMonster.Health);
                                }
                            }
                            else
                            {
                                if (monsterGenerator.ActiveMonster is Zombie)
                                {
                                    isCollision = CollisionDetector.CircRectIntersection(wizard.CharCircle, monsterGenerator.ZombieRect);
                                }
                                else if (monsterGenerator.ActiveMonster is Lander)
                                {
                                    isCollision = CollisionDetector.CirclesIntersection(wizard.CharCircle, monsterGenerator.LanderCircle);
                                }
                                else if (monsterGenerator.ActiveMonster is Orc)
                                {
                                    isCollision = CollisionDetector.CircRectIntersection(wizard.CharCircle, monsterGenerator.OrcRect);
                                }

                                actionTimer += gameTime.ElapsedGameTime.Milliseconds;
                                if (isCollision)
                                {
                                    wizard.HandleWizardCollision(false, 0);
                                    if (monsterGenerator.MonsterState == MonsterState.Attack)
                                    {
                                        if (actionTimer > collisionPerFrame)
                                        {
                                            actionTimer = 0;
                                            wizard.HandleWizardCollision(true, monsterGenerator.ActiveMonster.AttackPower);
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    case "WizardMonsterSpell":
                        if (monsterGenerator.Spells.Count > 0)
                        {
                            for (int spellNum = 0; spellNum < monsterGenerator.Spells.Count; spellNum++)
                            {
                                if (monsterGenerator.Spells[spellNum].EndOfLife != true)
                                {
                                    if (CollisionDetector.CirclesIntersection(wizard.CharCircle, (Circle)monsterGenerator.Spells[spellNum].Figure))
                                    {
                                        wizard.HandleWizardCollision(true, monsterGenerator.Spells[spellNum].SpellPower);
                                        monsterGenerator.HandleSpellCollision(spellNum);
                                    }
                                }
                            }
                        }
                        break;
                    case "SpellMonster":
                        if (wizard.Spells.Count > 0)
                        {
                            Circle circle1 = new Circle();
                            Circle circle2 = new Circle();
                            Rectangle rect1 = new Rectangle();
                            Rectangle rect2 = new Rectangle();
                            bool isRect1 = false;
                            bool isRect2 = false;

                            // pick monster figure
                            if (monsterGenerator.ActiveMonster is Zombie)
                            {
                                rect2 = monsterGenerator.ZombieRect;
                                isRect2 = true;
                            }
                            else if (monsterGenerator.ActiveMonster is Birdy)
                            {
                                circle2 = monsterGenerator.BirdyCircle;
                                isRect2 = false;
                            }
                            else if (monsterGenerator.ActiveMonster is Lander)
                            {
                                circle2 = monsterGenerator.LanderCircle;
                                isRect2 = false;
                            }
                            else if (monsterGenerator.ActiveMonster is Orc)
                            {
                                rect2 = monsterGenerator.OrcRect;
                                isRect2 = true;
                            }

                            for (int spellNum = 0; spellNum < wizard.Spells.Count; spellNum++)
                            {
                                // pick spell figure
                                if (wizard.Spells[spellNum].Name == "lighting" || wizard.Spells[spellNum].Name == "special")
                                {
                                    rect1 = (Rectangle)wizard.Spells[spellNum].Figure;
                                    isRect1 = true;
                                }
                                else
                                {
                                    circle1 = (Circle)wizard.Spells[spellNum].Figure;
                                    isRect1 = false;
                                }

                                // pick a method to be called depending on objects' shapes
                                if (isRect1 && isRect2)
                                {
                                    isCollision = rect1.Intersects(rect2);
                                }
                                else if (isRect1 && !isRect2)
                                {
                                    isCollision = CollisionDetector.CircRectIntersection(circle2, rect1);
                                }
                                else if (!isRect1 && isRect2)
                                {
                                    isCollision = CollisionDetector.CircRectIntersection(circle1, rect2);
                                }
                                else
                                {
                                    isCollision = CollisionDetector.CirclesIntersection(circle1, circle2);
                                }

                                if (isCollision)
                                {
                                    if (wizard.Spells[spellNum].Name != "shield")
                                    {
                                        wizard.HandleSpellCollision(spellNum);
                                        gainedExp = monsterGenerator.HandleMonsterCollision(0, wizard.Spells[spellNum].SpellPower);
                                        if (gainedExp > 0)
                                            levelUp = wizard.GainExperience(gainedExp);
                                    }
                                    else
                                    {
                                        monsterGenerator.HandleMonsterCollision(1, wizard.Spells[spellNum].SpellPower);
                                    }
                                }
                            }
                        }
                        break;

                    case "WizSpellMonSpell":
                        if (wizard.Spells.Count > 0 && monsterGenerator.Spells.Count > 0)
                        {
                            for (int wizSpellNum = 0; wizSpellNum < wizard.Spells.Count; wizSpellNum++)
                            {
                                if (wizard.Spells[wizSpellNum].Name == "shield")
                                {
                                    for (int monSpellNum = 0; monSpellNum < monsterGenerator.Spells.Count; monSpellNum++)
                                    {
                                        if (CollisionDetector.CirclesIntersection((Circle)wizard.Spells[wizSpellNum].Figure, (Circle)monsterGenerator.Spells[monSpellNum].Figure))
                                        {
                                            monsterGenerator.HandleSpellCollision(monSpellNum);
                                        }
                                    }
                                }
                            }
                        }
                        break;
                }
            }
        }

        private void AnimateLevelUp(GameTime gameTime)
        {
            levelUpTextTimer += gameTime.ElapsedGameTime.Milliseconds;
            levelUpAnimTimer += gameTime.ElapsedGameTime.Milliseconds;
            if (levelUpAnimTimer >= levelUpAnimationPerFrame)
            {
                currentLvlUpFrame++;
                levelUpAnimTimer = 0;
                if (currentLvlUpFrame > levelUpEndFrame)
                    currentLvlUpFrame = 0;
            }

            int row = (int)((float)currentLvlUpFrame / (float)levelUpColumns);
            int column = currentLvlUpFrame % levelUpColumns;
            lvlUpSourceRectangle = new Rectangle(effectWidth * column, effectHeight * row, effectWidth, effectHeight);
            lvlUpDestinationRectangle = new Rectangle((int)wizard.Position.X + 30, (int)wizard.Position.Y - 200, effectWidth, effectHeight);
            if (levelUpTextTimer >= levelUpTextShow)
            {
                levelUp = false;
                levelUpTextTimer = 0;
                currentLvlUpFrame = 0;
            }
        }

        private void ShowGateToNextLvl(GameTime gameTime)
        {
            gateAnimTimer += gameTime.ElapsedGameTime.Milliseconds;

            if (gateAnimTimer >= 40)
            {
                gateAnimTimer = 0;
                currentgateFrame++;

                if (currentgateFrame > gateEndFrame)
                    currentgateFrame = 0;

                int row = (int)((float)currentgateFrame / (float)gateColumns);
                int column = currentgateFrame % gateColumns;
                gateSourceRectangle = new Rectangle(gateWidth * column, gateHeight * row, gateWidth, gateHeight);
                gateDestinationRectangle = new Rectangle(gateXPosition, gateYPosition, gateWidth, gateHeight);
            }
        }

        private void LoadNextLevel()
        {
            int backgroundNoToLoad = levelGenerator.GenerateLevel();
            currentBackground = backgrounds[backgroundNoToLoad];
            backgrounds.RemoveAt(backgroundNoToLoad);
            wizard.RelocateWizard();
            if (levelGenerator.LevelNumber == LevelGenerator.FinalLevel)
            {
                monsterGenerator = new MonsterGenerator(monsterTextures, monsterEffTextures, healthBarTexture, screenWidth, screenHeight);
            }
            else if (levelGenerator.LevelNumber == LevelGenerator.TrainingLevel)
            {
                monsterGenerator = new MonsterGenerator();
            }
            else
            {
                monsterGenerator = new MonsterGenerator(monsterTextures, landerSpellTexture, monsterEffTextures, healthBarTexture, screenWidth, screenHeight);
            }

            areAllMonstersDead = false;
            currentgateFrame = 0;
            endGameStats.UpdateKilledMonstersNo(monsterGenerator.MonstersLeft);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(currentBackground, new Rectangle(0, 0, screenWidth, screenHeight), Color.White);
            spriteBatch.End();

            monsterGenerator.Draw(spriteBatch);
            wizard.DrawWizard(spriteBatch);
            wizard.DrawWizardSpells(spriteBatch);
            monsterGenerator.DrawMonsterSpells(spriteBatch);

            spriteBatch.Begin();
            spriteBatch.DrawString(spriteFontNormal, "Wizard HP: " + wizard.Health, new Vector2(50, 50), Color.Red);
            spriteBatch.DrawString(spriteFontNormal, "Wizard MP: " + wizard.Mana, new Vector2(50, 100), Color.DeepSkyBlue);
            spriteBatch.DrawString(spriteFontNormal, "Character lvl: " + wizard.Level, new Vector2(300, 50), Color.Gold);
            spriteBatch.DrawString(spriteFontNormal, "Monsters left: " + monsterGenerator.MonstersLeft, new Vector2(screenWidth - 600, 50), Color.Silver);
            spriteBatch.DrawString(spriteFontNormal, "Stage " + levelGenerator.LevelNumber + "/4", new Vector2(screenWidth - 300, 50), Color.Silver);
            // Draw levelup string
            if (levelUp)
            {
                spriteBatch.DrawString(spriteFontBig, "Level Up!", new Vector2(wizard.Position.X, wizard.Position.Y - 100), Color.Firebrick);
                spriteBatch.Draw(levelUpWaves, lvlUpDestinationRectangle, lvlUpSourceRectangle, Color.White);
            }
            // Draw level end gate
            if (areAllMonstersDead && levelGenerator.LevelNumber != LevelGenerator.FinalLevel)
            {
                spriteBatch.Draw(endGate, gateDestinationRectangle, gateSourceRectangle, Color.White);
            }
            // Draw training level instructions
            if (levelGenerator.LevelNumber == LevelGenerator.TrainingLevel)
            {
                spriteBatch.DrawString(spriteFontNormal, "Welcome!", new Vector2(500, 150), Color.FloralWhite);
                spriteBatch.DrawString(spriteFontNormal, "This is the training level.", new Vector2(500, 200), Color.White);
                spriteBatch.DrawString(spriteFontNormal, "You can start your journey by using the magic gate!", new Vector2(500, 250), Color.White);
            }
            if (endGameStats.IsWon && this.enabled)
            {
                textSize = spriteFontBig.MeasureString(gameWinTitle);
                spriteBatch.DrawString(spriteFontBig, gameWinTitle, new Vector2((screenWidth - textSize.X) / 2, 300), Color.DeepPink);
            }
            spriteBatch.End();
        }
    }
}
