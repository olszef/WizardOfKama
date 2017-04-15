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
	abstract class GameScreen
	{
        protected SpriteFont spriteFontSmall;
        protected SpriteFont spriteFontNormal;
        protected SpriteFont spriteFontBig;
        protected SpriteFont spriteFontLarge;
        protected Vector2 textSize;

        protected bool isEnabled;
        protected ScreenTypes screenType;

		protected Game game;
		protected SpriteBatch spriteBatch;
		protected ContentManager content;
        protected int screenWidth;
        protected int screenHeight;
        protected GameStats endGameStats = new GameStats();

        public bool IsEnabled
        {
            get { return isEnabled; }
        }

        public ScreenTypes ScreenType
        {
            get { return screenType; }
        }

        public GameScreen(Game game, ContentManager content, SpriteBatch spriteBatch, ScreenTypes screenType)
		{
			this.game = game;
			this.content = content;
			this.spriteBatch = spriteBatch;
            this.screenType = screenType;
            screenWidth = game.Window.ClientBounds.Width;
            screenHeight = game.Window.ClientBounds.Height;
            Hide();
		}

        public virtual GameStats Update(GameTime gameTime)
        {
            return endGameStats;
        }

        public virtual void Draw(GameTime gameTime)
		{
		}

        public virtual void Show()
		{
			this.isEnabled = true;
		}

		public virtual void Hide()
		{
			this.isEnabled = false;
		}
	}
}
