using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Design;
using System.Collections.Generic;
using System.Linq;

namespace TheWizardOfKama
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        KeyboardState keyboardState;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private int windowWidth;
        private int windowHeight;
        private Texture2D background;
        private string background_file;
        Character wizard;
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
        private string specAbility_file;
        private Texture2D[] spells = new Texture2D[5];
        Texture2D[] monsterTextures = new Texture2D[3];
        Texture2D[] monsterEffTextures = new Texture2D[4];
        Texture2D landerSpellTexture;
        Texture2D pauseDarkening;
        string zombie_file;
        string zombieDeath_file;
        string birdy_file;
        string birdyExplosion_file;
        string lander_file;
        string landerSpell_file;
        string landerExplosion_file;
        string pauseDarkening_file;
        string monsterRespawn_file;
        const int ScreenWidth = 1600;
        const int ScreenHeight = 900;
        string[] collisions;
        float actionTimer = 0;
        const float collisionPerFrame = 500;
        private bool paused = false;
        private bool pauseKeyDown = false;
        SpriteFont font;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = ScreenWidth;  // set this value to the desired width of your window
            graphics.PreferredBackBufferHeight = ScreenHeight;   // set this value to the desired height of your window
            graphics.ApplyChanges();
            windowWidth = graphics.PreferredBackBufferWidth;
            windowHeight = graphics.PreferredBackBufferHeight;
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            background_file = "backgrounds/forest1";
            wizard_file = "character/WizardMoves";
            casting_file = "character/casting";
            lighting_file = "character/lighting";
            water_file = "character/water";
            gravity_file = "character/gravity";
            shield_file = "character/shield";
            specAbility_file = "character/kameha";
            zombie_file = "monsters/zombie2";
            zombieDeath_file = "monsters/zombieDeath";
            birdy_file = "monsters/birdy";
            birdyExplosion_file = "monsters/birdyExplosion";
            lander_file = "monsters/lander";
            landerExplosion_file = "monsters/landerExplosion";
            landerSpell_file = "monsters/landerAttack";
            pauseDarkening_file = "backgrounds/pauseDarkening";
            monsterRespawn_file = "monsters/respawnAnim";
            levelGenerator = new LevelGenerator();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);


            // TODO: use this.Content to load your game content here
            font = Content.Load<SpriteFont>("GameText");
            background = Content.Load<Texture2D>(background_file);
            pauseDarkening = Content.Load<Texture2D>(pauseDarkening_file);
            wizardTexture = Content.Load<Texture2D>(wizard_file);
            castingTexture = Content.Load<Texture2D>(casting_file);
            spells[0] = Content.Load<Texture2D>(lighting_file);
            spells[1] = Content.Load<Texture2D>(water_file);
            spells[2] = Content.Load<Texture2D>(gravity_file);
            spells[3] = Content.Load<Texture2D>(shield_file);
            spells[4] = Content.Load<Texture2D>(specAbility_file);
            monsterTextures[0] = Content.Load<Texture2D>(zombie_file);
            monsterTextures[1] = Content.Load<Texture2D>(birdy_file);
            monsterTextures[2] = Content.Load<Texture2D>(lander_file);
            monsterEffTextures[0] = Content.Load<Texture2D>(zombieDeath_file);
            monsterEffTextures[1] = Content.Load<Texture2D>(birdyExplosion_file);
            monsterEffTextures[2] = Content.Load<Texture2D>(landerExplosion_file);
            monsterEffTextures[3] = Content.Load<Texture2D>(monsterRespawn_file);
            landerSpellTexture = Content.Load<Texture2D>(landerSpell_file);
            wizard = new Character(wizardTexture, castingTexture, spells, ScreenWidth, ScreenHeight);
            monsterGenerator = new MonsterGenerator(monsterTextures, landerSpellTexture, monsterEffTextures, ScreenWidth, ScreenHeight);
            monsterGenerator.healthBarTexture[0] = Content.Load<Texture2D>("monsters/HealthBar");
            monsterGenerator.healthBarTexture[1] = Content.Load<Texture2D>("monsters/HealthBarFrame");
            InitCollPairs();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            keyboardState = Keyboard.GetState();
            checkPauseKey(keyboardState);

            if (!paused)
            {
                wizard.UpdateWizard(gameTime);
                monsterGenerator.ControlMonster(gameTime, wizard.Position);
                checkCollisions(gameTime);
                base.Update(gameTime);
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            spriteBatch.Draw(background, new Rectangle(0, 0, windowWidth, windowHeight), Color.White);
            spriteBatch.End();
                        
            monsterGenerator.Draw(spriteBatch);
            wizard.DrawWizard(spriteBatch);
            wizard.DrawWizardSpells(spriteBatch);
            monsterGenerator.DrawMonsterSpells(spriteBatch);

            spriteBatch.Begin();
            spriteBatch.DrawString(font, "Wizard HP: " + wizard.Health, new Vector2(50, 50), Color.Red);
            spriteBatch.DrawString(font, "Wizard MP: " + wizard.Mana, new Vector2(50, 100), Color.DeepSkyBlue);
            spriteBatch.DrawString(font, "Character lvl: " + wizard.Level, new Vector2(300, 50), Color.Gold);
            spriteBatch.DrawString(font, "Monsters left: " + monsterGenerator.MonstersLeft, new Vector2(ScreenWidth - 600, 50), Color.Silver);
            spriteBatch.DrawString(font, "Stage " + levelGenerator.LevelNumber + "/3", new Vector2(ScreenWidth - 300, 50), Color.Silver);
            if (paused)
            {
                spriteBatch.Draw(pauseDarkening, new Rectangle(0, 0, windowWidth, windowHeight), Color.White);
                spriteBatch.DrawString(font, "The game is paused. Click 'P' button to resume...", new Vector2(ScreenWidth / 2 - 300, ScreenHeight / 2), Color.GhostWhite);

            }
            spriteBatch.End();
            base.Draw(gameTime);
        }

        private void checkPauseKey(KeyboardState keyboardState)
        {
            bool pauseKeyDownThisFrame = (keyboardState.IsKeyDown(Keys.P));
            // If key was not down before, but is down now, we toggle the
            // pause setting
            if (!pauseKeyDown && pauseKeyDownThisFrame)
            {
                if (!paused)
                    paused = true;
                else
                    paused = false;
            }
            pauseKeyDown = pauseKeyDownThisFrame;
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

        private void checkCollisions(GameTime gameTime)
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

                            for (int spellNum = 0; spellNum < wizard.Spells.Count; spellNum++)
                            {
                                // pick spell figure
                                if (wizard.Spells[spellNum].Name == "lighting")
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
                                        monsterGenerator.HandleMonsterCollision(0, wizard.Spells[spellNum].SpellPower);
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
                                        if(CollisionDetector.CirclesIntersection((Circle)wizard.Spells[wizSpellNum].Figure, (Circle)monsterGenerator.Spells[monSpellNum].Figure))
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
    }
}
