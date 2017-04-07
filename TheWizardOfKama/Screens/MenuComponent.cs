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
    class MenuComponent
    {
        protected bool enabled;
        protected int lineSpacing;
        protected string name;
        protected string textItem;
        protected string titleText = "The Wizard Of Kama";
        protected Vector2 itemSize;
        protected Vector2 textSize;

        protected Color normalColor = Color.Silver;
        protected Color highlitedColor = Color.DeepPink;
        protected Color itemColor;

        protected KeyboardState keyboardState;
        protected KeyboardState oldKeyboardState;

        protected SpriteBatch spriteBatch;
        protected SpriteFont spriteFontNormal;
        protected SpriteFont spriteFontBig;
        protected SpriteFont spriteFontLarge;
        protected string imagePath;
        protected Texture2D image;
        protected Rectangle imageRectangle;
        protected ContentManager content;
        protected Game game;

        protected Vector2 position;
        protected float menuWidth = 0f;
        protected float menuHeight = 0f;
        protected float screenWidth = 0f;
        protected float screenHeight = 0f;

        public bool Enabled
        {
            get { return enabled; }
        }

        public string Name
        {
            get { return name; }
        }

        public MenuComponent(Game game, ContentManager content, SpriteBatch spriteBatch, string imagePath, string name)
        {
            this.name = name;
            this.spriteBatch = spriteBatch;
            this.content = content;
            this.game = game;
            screenWidth = game.Window.ClientBounds.Width;
            screenHeight = game.Window.ClientBounds.Height;
            this.imagePath = imagePath;
            LoadContent();
            imageRectangle = new Rectangle(
                0,
                0,
                (int)screenWidth,
                (int)screenHeight);
            Hide();

        }

        protected bool CheckKey(Keys theKey)
        {
            return keyboardState.IsKeyUp(theKey) &&
                oldKeyboardState.IsKeyDown(theKey);
        }

        public virtual void Update(GameTime gameTime)
        {
            keyboardState = Keyboard.GetState();
        }

        private void LoadContent()
        {
            image = content.Load<Texture2D>(imagePath);
            spriteFontNormal = content.Load<SpriteFont>("GameText36");
            spriteFontBig = content.Load<SpriteFont>("GameText56");
            spriteFontLarge = content.Load<SpriteFont>("GameText70");
        }

        public virtual void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(image, imageRectangle, Color.White);
            // Draw game title
            textSize = spriteFontLarge.MeasureString(titleText);
            spriteBatch.DrawString(spriteFontLarge, titleText, new Vector2(((float)screenWidth - textSize.X) / 2, 30), Color.Black);
            textSize = spriteFontBig.MeasureString(titleText);
            spriteBatch.DrawString(spriteFontBig, titleText, new Vector2(((float)screenWidth - textSize.X) / 2, 30), Color.WhiteSmoke);
            // Draw MenuComponent name
            textSize = spriteFontBig.MeasureString(name);
            spriteBatch.DrawString(spriteFontBig, name, new Vector2((screenWidth - textSize.X) / 2, 100), Color.Gold);
            spriteBatch.End();
        }

        public virtual void Show()
        {
            this.enabled = true;
        }

        public virtual void Hide()
        {
            this.enabled = false;
        }
    }
}
