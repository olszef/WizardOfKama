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
        protected string name;
        protected bool enabled;
		protected Game game;
		protected SpriteBatch spriteBatch;
		protected ContentManager content;
		protected string imageName; 
		protected Texture2D image;
		protected Rectangle imageRectangle;
        protected int screenWidth;
        protected int screenHeight;

        public bool Enabled
        {
            get { return enabled; }
        }

        public string Name
        {
            get { return name; }
        }

        public GameScreen(Game game, ContentManager content, SpriteBatch spriteBatch, string name)
		{
			this.game = game;
			this.content = content;
			this.spriteBatch = spriteBatch;
            this.name = name;
            screenWidth = game.Window.ClientBounds.Width;
            screenHeight = game.Window.ClientBounds.Height;
            Hide();
		}

		public virtual void Draw(GameTime gameTime)
		{
		}

        public virtual void Update(GameTime gameTime)
        {
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
