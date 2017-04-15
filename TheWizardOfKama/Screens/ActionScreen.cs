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
        string backgroundForest_filePath;
        string backgroundCastle_filePath;
        string backgroundWindmill_filePath;
        string backgroundWinter_filePath;
        string backgroundField_filePath;
        string wizard_filePath;
        string casting_filePath;
        string lighting_filePath;
        string water_filePath;
        string gravity_filePath;
        string shield_filePath;
        string specialSpell_filePath;
        string levelUpWaves_filePath;
        string endGate_filePath;
        string zombie_filePath;
        string zombieDeath_filePath;
        string birdy_filePath;
        string birdyExplosion_filePath;
        string lander_filePath;
        string orc_filePath;
        string orcDeath_filePath;
        string landerSpell_filePath;
        string landerExplosion_filePath;
        string monsterRespawn_filePath;
        string healthBar_filePath;
        string healthBarFrame_filePath;

        List<Texture2D> backgrounds = new List<Texture2D>();
        Texture2D currentBackground;
        Texture2D wizardTexture;
        Texture2D castingTexture;
        Texture2D[] spells = new Texture2D[5];
        Texture2D[] monsterTextures = new Texture2D[4];
        Texture2D[] monsterEffTextures = new Texture2D[5];
        Texture2D[] healthBarTexture = new Texture2D[2];
        Texture2D landerSpellTexture;
        Texture2D levelUpWaves;
        Texture2D endGate;

        Wizard wizard;
        MonsterGenerator monsterGenerator;
        LevelGenerator levelGenerator;
        GameTime gameTime;

        string[] collisions;
        bool isCollision = false;
        Circle circle1 = new Circle();
        Circle circle2 = new Circle();
        Rectangle rect1 = new Rectangle();
        Rectangle rect2 = new Rectangle();
        bool isRect1 = false;
        bool isRect2 = false;

        float actionTimer = 0;
        const float collisionPerMiliseconds = 500;
        int gainedExp;

        bool levelUp = false;
        float levelUpTextTimer = 0;
        float levelUpAnimTimer = 0;
        const float levelUpTextShowTime = 2000;
        const float levelUpAnimPerMiliseconds = 40;
        int currentLvlUpFrame;
        const int levelUpEndFrame = 6;
        const int levelUpColumns = 7;
        const int levelUpRows = 1;
        Rectangle lvlUpSourceRectangle;
        Rectangle lvlUpDestinationRectangle;

        int effectWidth;
        int effectHeight;

        bool areAllMonstersDead = false;
        bool isWizardDead = false;
        float totalPlayedTime = 0;
        string gameWinTitle = "VICTORY!";

        const int gateEndFrame = 15;
        const int gateColumns = 4;
        const int gateRows = 4;
        float gateAnimTimer = 0;
        const float gateAnimPerMiliseconds = 40;
        Rectangle gateSourceRectangle;
        Rectangle gateDestinationRectangle;
        int currentgateFrame;
        int gateWidth;
        int gateHeight;
        const int gateXPosition = 1100;
        const int gateYPosition = 400;

        public ActionScreen(Game game, ContentManager content, SpriteBatch spriteBatch, ScreenTypes screenType) : base(game, content, spriteBatch, screenType)
        {
            Initalize();
            LoadContent();
            currentLvlUpFrame = 0;
            effectWidth = levelUpWaves.Width / levelUpColumns;
            effectHeight = levelUpWaves.Height / levelUpRows;
            gateWidth = endGate.Width / gateColumns;
            gateHeight = endGate.Height / gateRows;
        }

        private void Initalize()
        {
            backgroundField_filePath = "backgrounds/Field";
            backgroundWindmill_filePath = "backgrounds/Windmill";
            backgroundWinter_filePath = "backgrounds/Winter";
            backgroundCastle_filePath = "backgrounds/Castle";
            backgroundForest_filePath = "backgrounds/Forest";
            wizard_filePath = "character/BasicWizardMoves";
            casting_filePath = "character/casting";
            lighting_filePath = "character/lighting";
            water_filePath = "character/water";
            gravity_filePath = "character/gravity";
            shield_filePath = "character/shield";
            specialSpell_filePath = "character/specialSpell";
            zombie_filePath = "monsters/Zombie";
            zombieDeath_filePath = "monsters/ZombieDeath";
            birdy_filePath = "monsters/Birdy";
            birdyExplosion_filePath = "monsters/BirdyExplosion";
            lander_filePath = "monsters/Lander";
            landerExplosion_filePath = "monsters/LanderExplosion";
            landerSpell_filePath = "monsters/LanderAttack";
            orc_filePath = "monsters/BasicOrcMoves";
            orcDeath_filePath = "monsters/OrcDeath";
            levelUpWaves_filePath = "character/LevelUpWaves";
            monsterRespawn_filePath = "monsters/RespawnAnim";
            endGate_filePath = "other/EndGate";
            healthBar_filePath = "monsters/HealthBar";
            healthBarFrame_filePath = "monsters/HealthBarFrame";
            levelGenerator = new LevelGenerator();
        }

        private void LoadContent()
        {
            spriteFontNormal = content.Load<SpriteFont>("GameText24");
            spriteFontBig = content.Load<SpriteFont>("GameText56");
            backgrounds.Add(content.Load<Texture2D>(backgroundForest_filePath));
            backgrounds.Add(content.Load<Texture2D>(backgroundField_filePath));
            backgrounds.Add(content.Load<Texture2D>(backgroundWindmill_filePath));
            backgrounds.Add(content.Load<Texture2D>(backgroundWinter_filePath));
            backgrounds.Add(content.Load<Texture2D>(backgroundCastle_filePath));
            levelUpWaves = content.Load<Texture2D>(levelUpWaves_filePath);
            endGate = content.Load<Texture2D>(endGate_filePath);
            wizardTexture = content.Load<Texture2D>(wizard_filePath);
            castingTexture = content.Load<Texture2D>(casting_filePath);
            spells[0] = content.Load<Texture2D>(lighting_filePath);
            spells[1] = content.Load<Texture2D>(water_filePath);
            spells[2] = content.Load<Texture2D>(gravity_filePath);
            spells[3] = content.Load<Texture2D>(shield_filePath);
            spells[4] = content.Load<Texture2D>(specialSpell_filePath);
            monsterTextures[0] = content.Load<Texture2D>(zombie_filePath);
            monsterTextures[1] = content.Load<Texture2D>(birdy_filePath);
            monsterTextures[2] = content.Load<Texture2D>(lander_filePath);
            monsterTextures[3] = content.Load<Texture2D>(orc_filePath);
            monsterEffTextures[0] = content.Load<Texture2D>(zombieDeath_filePath);
            monsterEffTextures[1] = content.Load<Texture2D>(birdyExplosion_filePath);
            monsterEffTextures[2] = content.Load<Texture2D>(landerExplosion_filePath);
            monsterEffTextures[3] = content.Load<Texture2D>(monsterRespawn_filePath);
            monsterEffTextures[4] = content.Load<Texture2D>(orcDeath_filePath);
            landerSpellTexture = content.Load<Texture2D>(landerSpell_filePath);
            healthBarTexture[0] = content.Load<Texture2D>(healthBar_filePath);
            healthBarTexture[1] = content.Load<Texture2D>(healthBarFrame_filePath);
            wizard = new Wizard(wizardTexture, castingTexture, spells, screenWidth, screenHeight);
            LoadNextLevel();
            InitCollPairs();
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

        public override GameStats Update(GameTime gameTime)
        {
            this.gameTime = gameTime;
            totalPlayedTime += gameTime.ElapsedGameTime.Milliseconds;

            isWizardDead = wizard.UpdateWizardMoves(gameTime);
            CheckEndLevelCriteria();
            areAllMonstersDead = monsterGenerator.ControlMonster(gameTime, wizard.Position);
            CheckCollisions();

            if (levelUp)
                AnimateLevelUp();

            if (isWizardDead)
                endGameStats.SetFinalStats(wizard.Level, false, totalPlayedTime);

            return endGameStats;
        }

        private void CheckEndLevelCriteria()
        {
            if (areAllMonstersDead && !isWizardDead)
            {
                if (levelGenerator.LevelNumber != LevelGenerator.FinalLevel)
                {
                    AnimateGateAndCheckPassing();
                }
                else
                {
                    endGameStats.SetFinalStats(wizard.Level, true, totalPlayedTime);
                }
            }
        }

        private void AnimateGateAndCheckPassing()
        {
            AnimateGateToNextLvl();
            if (wizard.Position.X > gateXPosition && wizard.Position.Y > gateYPosition)
            {
                LoadNextLevel();
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

        private void CheckCollisions()
        {
            foreach (string collisionPair in collisions)
            {
                isCollision = false;
                switch (collisionPair)
                {
                    case "WizardMonster":
                        if (monsterGenerator.MonstersLeft > 0 && wizard.MoveState != MoveState.Dead)
                            CheckWizardMonsterCollision();
                        break;

                    case "WizardMonsterSpell":
                        if (monsterGenerator.Spells.Count > 0)
                        {
                            CheckWizardMonsterSpellCollision();
                        }
                        break;

                    case "SpellMonster":
                        if (wizard.Spells.Count > 0)
                        {
                            CheckWizardsSpellMonsterCollision();
                        }
                        break;

                    case "WizSpellMonSpell":
                        if (wizard.Spells.Count > 0 && monsterGenerator.Spells.Count > 0)
                        {
                            CheckWizSpellMonSpellCollision();
                        }
                        break;
                }
            }
        }


        private void CheckWizardMonsterCollision()
        {
            if (monsterGenerator.ActiveMonster is Birdy)
            {
                TestWizardBirdyColl();
            }
            else
            {
                TestWizardOtherMonstersColl();
                actionTimer += gameTime.ElapsedGameTime.Milliseconds;
                if (isCollision)
                {
                    ControlWizardAndCheckIfAttacked();
                }
            }
        }

        private void TestWizardBirdyColl()
        {
            if (CollisionDetector.CirclesIntersection(wizard.CharCircle, monsterGenerator.BirdyCircle))
            {
                wizard.HandleWizardCollision(true, monsterGenerator.ActiveMonster.AttackPower);
                monsterGenerator.HandleMonsterCollision(0, monsterGenerator.ActiveMonster.Health);
            }
        }

        private void TestWizardOtherMonstersColl()
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
        }

        private void ControlWizardAndCheckIfAttacked()
        {
            wizard.HandleWizardCollision(false, 0);
            if (monsterGenerator.MonsterState == MonsterState.Attack)
            {
                if (actionTimer > collisionPerMiliseconds)
                {
                    actionTimer = 0;
                    wizard.HandleWizardCollision(true, monsterGenerator.ActiveMonster.AttackPower);
                }
            }
        }


        private void CheckWizardMonsterSpellCollision()
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


        private void CheckWizardsSpellMonsterCollision()
        {
            SetMonsterShape();

            for (int spellNum = 0; spellNum < wizard.Spells.Count; spellNum++)
            {
                SetSpellShape(spellNum);
                TestShapesCollisions();

                if (isCollision)
                {
                    ControlCharactersDependingOnSpell(spellNum);
                }
            }
        }

        private void SetMonsterShape()
        {
            isRect1 = false;
            isRect2 = false;

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
        }

        private void SetSpellShape(int spellNum)
        {
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
        }

        private void TestShapesCollisions()
        {
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
        }

        private void ControlCharactersDependingOnSpell(int spellNum)
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


        private void CheckWizSpellMonSpellCollision()
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


        private void AnimateLevelUp()
        {
            levelUpTextTimer += gameTime.ElapsedGameTime.Milliseconds;
            levelUpAnimTimer += gameTime.ElapsedGameTime.Milliseconds;
            if (levelUpAnimTimer >= levelUpAnimPerMiliseconds)
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
            if (levelUpTextTimer >= levelUpTextShowTime)
            {
                levelUp = false;
                levelUpTextTimer = 0;
                currentLvlUpFrame = 0;
            }
        }

        private void AnimateGateToNextLvl()
        {
            gateAnimTimer += gameTime.ElapsedGameTime.Milliseconds;
            if (gateAnimTimer >= gateAnimPerMiliseconds)
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
            DrawWizardsStats();
            DrawLevelUpAnimation();
            DrawGateToNextLevel();
            DrawTrainingLevelInstructions();
            DrawVictoryText();
            spriteBatch.End();
        }

        private void DrawWizardsStats()
        {
            spriteBatch.DrawString(spriteFontNormal, "Wizard HP: " + wizard.Health, new Vector2(50, 50), Color.Red);
            spriteBatch.DrawString(spriteFontNormal, "Wizard MP: " + wizard.Mana, new Vector2(50, 100), Color.DeepSkyBlue);
            spriteBatch.DrawString(spriteFontNormal, "Character lvl: " + wizard.Level, new Vector2(300, 50), Color.Gold);
            spriteBatch.DrawString(spriteFontNormal, "Monsters left: " + monsterGenerator.MonstersLeft, new Vector2(screenWidth - 600, 50), Color.Silver);
            spriteBatch.DrawString(spriteFontNormal, "Stage " + levelGenerator.LevelNumber + "/4", new Vector2(screenWidth - 300, 50), Color.Silver);
        }

        private void DrawLevelUpAnimation()
        {
            if (levelUp)
            {
                spriteBatch.DrawString(spriteFontBig, "Level Up!", new Vector2(wizard.Position.X, wizard.Position.Y - 100), Color.Firebrick);
                spriteBatch.Draw(levelUpWaves, lvlUpDestinationRectangle, lvlUpSourceRectangle, Color.White);
            }
        }

        private void DrawGateToNextLevel()
        {
            if (areAllMonstersDead && levelGenerator.LevelNumber != LevelGenerator.FinalLevel)
            {
                spriteBatch.Draw(endGate, gateDestinationRectangle, gateSourceRectangle, Color.White);
            }
        }

        private void DrawTrainingLevelInstructions()
        {
            if (levelGenerator.LevelNumber == LevelGenerator.TrainingLevel)
            {
                spriteBatch.DrawString(spriteFontNormal, "Welcome!", new Vector2(500, 150), Color.FloralWhite);
                spriteBatch.DrawString(spriteFontNormal, "This is the training level.", new Vector2(500, 200), Color.White);
                spriteBatch.DrawString(spriteFontNormal, "You can start your journey by using the magic gate!", new Vector2(500, 250), Color.White);
            }
        }

        private void DrawVictoryText()
        {
            if (endGameStats.IsWon && this.isEnabled)
            {
                textSize = spriteFontBig.MeasureString(gameWinTitle);
                spriteBatch.DrawString(spriteFontBig, gameWinTitle, new Vector2((screenWidth - textSize.X) / 2, 300), Color.DeepPink);
            }
        }
    }
}
