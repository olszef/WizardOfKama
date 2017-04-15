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
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont spriteFontSmall;
        SpriteFont spriteFontNormal;
        const int screenWidth = 1600;
        const int screenHeight = 900;

        GameScreen activeScreen;
        MenuScreen menuScreen;
        ActionScreen actionScreen;
        EndScreen endScreen;
        List<GameScreen> gameScreens;
        GameStats endGameStats = new GameStats();

        KeyboardState keyboardState;
        KeyboardState oldKeyboardState;

        bool isGameLost = false;
        bool pauseRequest = false;
        bool exitRequest = false;
        string pauseDarkening_filepath;
        Texture2D pauseDarkeningTexture;

        Vector2 textSize;
        string gameOverTitle = "GAME OVER";
        string gameOverExitInstructions = "Sorry, You lost... Please Click 'Esc' to end the game.";
        string gameOverRestartInstructions = "Please Click 'R' to restart the game.";
        const float timeToShowWinScreen = 2000;
        float beforeWinScreenTimer = 0;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = screenWidth;  // set this value to the desired width of your window
            graphics.PreferredBackBufferHeight = screenHeight;   // set this value to the desired height of your window
            graphics.ApplyChanges();
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
            pauseDarkening_filepath = "backgrounds/PauseDarkening";

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

            spriteFontSmall = Content.Load<SpriteFont>("GameText24");
            spriteFontNormal = Content.Load<SpriteFont>("GameText36");
            pauseDarkeningTexture = Content.Load<Texture2D>(pauseDarkening_filepath);
            menuScreen = new MenuScreen(this, Content, spriteBatch, ScreenTypes.MenuScreen);
            actionScreen = new ActionScreen(this, Content, spriteBatch, ScreenTypes.ActionScreen);
            endScreen = new EndScreen(this, Content, spriteBatch, ScreenTypes.EndScreen);
            gameScreens = new List<GameScreen>
            {
                menuScreen,
                actionScreen,
                endScreen
            };
            activeScreen = menuScreen;
            activeScreen.Show();
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
            keyboardState = Keyboard.GetState();

            switch (activeScreen.ScreenType)
            {
                case ScreenTypes.MenuScreen:
                    if (CheckKey(Keys.Enter))
                    {
                        RecognizeActiveMenuComponent();
                    }
                    activeScreen.Update(gameTime);
                    break;
                case ScreenTypes.ActionScreen:
                    CheckPauseKeys(keyboardState);
                    ControlGameAndUserAction(gameTime);
                    break;
                case ScreenTypes.EndScreen:
                    activeScreen.Update(gameTime);
                    ExitOrRestartGame();
                    break;
            }

            oldKeyboardState = keyboardState;
            base.Update(gameTime);
        }

        private bool CheckKey(Keys theKey)
        {
            return keyboardState.IsKeyUp(theKey) &&
                oldKeyboardState.IsKeyDown(theKey);
        }

        private void RecognizeActiveMenuComponent()
        {
            switch (menuScreen.GetActiveItem())
            {
                case MenuItemTypes.MainMenu:
                    EnterSelectedItem();
                    break;
                case MenuItemTypes.Instructions:
                case MenuItemTypes.Controls:
                case MenuItemTypes.Credits:
                    menuScreen.ShowMenuComponent(MenuItemTypes.MainMenu);
                    break;
            }
        }

        private void EnterSelectedItem()
        {
            switch (menuScreen.GetMenuSelectedItem())
            {
                case MenuItemTypes.StartGame:
                    activeScreen.Hide();
                    activeScreen = actionScreen;
                    activeScreen.Show();
                    break;
                case MenuItemTypes.Instructions:
                    menuScreen.ShowMenuComponent(MenuItemTypes.Instructions);
                    break;
                case MenuItemTypes.Controls:
                    menuScreen.ShowMenuComponent(MenuItemTypes.Controls);
                    break;
                case MenuItemTypes.Credits:
                    menuScreen.ShowMenuComponent(MenuItemTypes.Credits);
                    break;
                case MenuItemTypes.Exit:
                    Exit();
                    break;
            }
        }

        private void ControlGameAndUserAction(GameTime gameTime)
        {
            if ((!pauseRequest || endGameStats.IsEndGame) && !exitRequest)
            {
                endGameStats = activeScreen.Update(gameTime);
                if (endGameStats.IsEndGame)
                {
                    PickEndScreen(gameTime);
                }
            }
            else
            {
                if (exitRequest)
                {
                    CloseWindowOrRequestAction();
                }
            }
        }

        private void PickEndScreen(GameTime gameTime)
        {
            if (endGameStats.IsWon)
            {
                ShowVictoryEndScreen(gameTime);
            }
            else
            {
                SetLosingAndCheckForRestart();
            }
        }

        private void ShowVictoryEndScreen(GameTime gameTime)
        {
            beforeWinScreenTimer += gameTime.ElapsedGameTime.Milliseconds;
            if (beforeWinScreenTimer >= timeToShowWinScreen)
            {
                activeScreen.Hide();
                activeScreen = endScreen;
                activeScreen.Show();
                endScreen.LoadGameStats(endGameStats);
            }
        }

        private void SetLosingAndCheckForRestart()
        {
            isGameLost = true;
            if (CheckKey(Keys.R))
            {
                Restart.RestartGame = true;
                Exit();
            }
        }

        private void CloseWindowOrRequestAction()
        {
            if (endGameStats.IsEndGame)
                Exit();
            else
            {
                WaitForUserAction();
            }
        }

        private void WaitForUserAction()
        {
            if (CheckKey(Keys.Y))
                Exit();
            else if (CheckKey(Keys.N))
                exitRequest = false;
        }

        private void ExitOrRestartGame()
        {
            if (CheckKey(Keys.Escape))
                Exit();
            else if (CheckKey(Keys.R))
            {
                Restart.RestartGame = true;
                Exit();
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
            foreach (GameScreen screen in gameScreens)
            {
                if (screen.IsEnabled)
                {
                    screen.Draw(gameTime);

                    // Draw game stop screens
                    spriteBatch.Begin();
                    if (!isGameLost)
                    {
                        if (pauseRequest || exitRequest)
                        {
                            {
                                spriteBatch.Draw(pauseDarkeningTexture, new Rectangle(0, 0, screenWidth, screenHeight), Color.White);
                                if (exitRequest)
                                    spriteBatch.DrawString(spriteFontSmall, "Do you really want to exit the game? Click (Y)es or (N)o ...", new Vector2(screenWidth / 2 - 400, screenHeight / 2), Color.GhostWhite);
                                else
                                    spriteBatch.DrawString(spriteFontSmall, "The game is paused. Click 'P' button to resume...", new Vector2(screenWidth / 2 - 300, screenHeight / 2), Color.GhostWhite);
                            }
                        }
                    }
                    else
                    {
                        spriteBatch.Draw(pauseDarkeningTexture, new Rectangle(0, 0, screenWidth, screenHeight), Color.White);
                        textSize = spriteFontNormal.MeasureString(gameOverTitle);
                        spriteBatch.DrawString(spriteFontNormal, gameOverTitle, new Vector2((screenWidth - textSize.X) / 2, 400), Color.Red);
                        textSize = spriteFontSmall.MeasureString(gameOverExitInstructions);
                        spriteBatch.DrawString(spriteFontSmall, gameOverExitInstructions, new Vector2((screenWidth - textSize.X) / 2, 500), Color.GhostWhite);
                        textSize = spriteFontSmall.MeasureString(gameOverRestartInstructions);
                        spriteBatch.DrawString(spriteFontSmall, gameOverRestartInstructions, new Vector2((screenWidth - textSize.X) / 2, 550), Color.GhostWhite);
                    }
                    spriteBatch.End();
                }
            }
            base.Draw(gameTime);
        }

        private void CheckPauseKeys(KeyboardState keyboardState)
        {
            // If key was not down before, but is down now, we toggle the
            // pause setting
            if (CheckKey(Keys.P))
            {
                if (!pauseRequest)
                    pauseRequest = true;
                else
                    pauseRequest = false;
            }

            if (CheckKey(Keys.Escape))
            {
                if (!exitRequest)
                    exitRequest = true;
            }
        }
    }
}
