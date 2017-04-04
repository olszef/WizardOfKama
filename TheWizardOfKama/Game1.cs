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
        SpriteFont spriteFontNormal;
        const int screenWidth = 1600;
        const int screenHeight = 900;
        GameScreen activeScreen;
        MenuScreen menuScreen;
        ActionScreen actionScreen;
        //EndScreen endScreen;
        List<GameScreen> gameScreens;
        KeyboardState keyboardState;
        KeyboardState oldKeyboardState;
        bool paused = false;
        bool pauseKeyDown = false;
        bool exit = false;
        bool escKeyDown = false;
        Texture2D pauseDarkening;
        string pauseDarkening_file;

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
            pauseDarkening_file = "backgrounds/pauseDarkening";

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
            spriteFontNormal = Content.Load<SpriteFont>("GameText24");
            pauseDarkening = Content.Load<Texture2D>(pauseDarkening_file);
            menuScreen = new MenuScreen(this, Content, spriteBatch, "menuScreen");
            actionScreen = new ActionScreen(this, Content, spriteBatch, "actionScreen");
            //endScreen = new EndScreen(this, Content, spriteBatch, "endScreen");
            gameScreens = new List<GameScreen>
            {
                menuScreen,
                actionScreen,
                //endScreen
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
            /*if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();*/

            // TODO: Add your update logic here
            switch (activeScreen.Name)
            {
                case "menuScreen":
                    if (CheckKey(Keys.Enter))
                    {
                        //****** cały switch do klasy MenuScreen?
                        switch (menuScreen.GetActiveItemName())
                        {
                            case "Main Menu":
                                switch (menuScreen.GetMenuSelectedItem())
                                {
                                    case 0:
                                        activeScreen.Hide();
                                        activeScreen = actionScreen;
                                        activeScreen.Show();
                                        break;
                                    case 1:
                                        //Show Info and instructions
                                        menuScreen.ShowMenuComponent(1);
                                        break;
                                    case 2:
                                        //Show Credits
                                        menuScreen.ShowMenuComponent(2);
                                        break;
                                    case 3:
                                        //Show Credits
                                        menuScreen.ShowMenuComponent(3);
                                        break;
                                    case 4:
                                        this.Exit();
                                        break;
                                }
                                break;
                            case "Instructions":
                                //Back to Main menu
                                menuScreen.ShowMenuComponent(0);
                                break;
                            case "Controls":
                                //Back to Main menu
                                menuScreen.ShowMenuComponent(0);
                                break;
                            case "Credits":
                                //Back to Main menu
                                menuScreen.ShowMenuComponent(0);
                                break;
                        }
                    }
                    activeScreen.Update(gameTime);
                    break;
                case "actionScreen":
                    //TODO:
                    checkPauseKeys(keyboardState);
                    if (!paused && !exit)
                        activeScreen.Update(gameTime);
                    else
                    {
                        if (exit)
                        {
                            if (CheckKey(Keys.Y))
                                Exit();
                            else if (CheckKey(Keys.N))
                                exit = false;
                        }
                    }
                    break;
                /*case "endScreen":
                    //TODO:
                    activeScreen.Update(gameTime);
                    break;*/
            }
            oldKeyboardState = keyboardState;
            base.Update(gameTime);
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
                if (screen.Enabled)
                {
                    screen.Draw(gameTime);
                    // Draw pause screen
                    if (paused || exit)
                    {
                        spriteBatch.Begin();
                        spriteBatch.Draw(pauseDarkening, new Rectangle(0, 0, screenWidth, screenHeight), Color.White);
                        if (exit)
                            spriteBatch.DrawString(spriteFontNormal, "Do you really want to exit the game? Click (Y)es or (N)o ...", new Vector2(screenWidth / 2 - 400, screenHeight / 2), Color.GhostWhite);
                        else
                            spriteBatch.DrawString(spriteFontNormal, "The game is paused. Click 'P' button to resume...", new Vector2(screenWidth / 2 - 300, screenHeight / 2), Color.GhostWhite);
                        spriteBatch.End();
                    }
                }
            }
            base.Draw(gameTime);
        }

        bool CheckKey(Keys theKey)
        {
            return keyboardState.IsKeyUp(theKey) &&
                oldKeyboardState.IsKeyDown(theKey);
        }

        private void checkPauseKeys(KeyboardState keyboardState)
        {
            // If key was not down before, but is down now, we toggle the
            // pause setting
            if (CheckKey(Keys.P))
            {
                if (!paused)
                    paused = true;
                else
                    paused = false;
            }

            if (CheckKey(Keys.Escape))
            {
                if (!exit)
                    exit = true;
            }
        }
    }
}
