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
    class MainMenu : MenuComponent
    {
        string[] menuItems;
        int selectedIndex;

        public int SelectedIndex
        {
            get { return selectedIndex; }
            set
            {
                selectedIndex = value;
                if (selectedIndex < 0)
                    selectedIndex = menuItems.Length - 1;
                if (selectedIndex >= menuItems.Length)
                    selectedIndex = 0;
            }
        }

        public MainMenu(Game game, ContentManager content, SpriteBatch spriteBatch, string imagePath, string name) : base(game, content, spriteBatch, imagePath, name)
        {
            menuItems = new string[]
            {
                "Start new game",
                "   Instructions",
                "      Controls",
                "       Credits",
                "          Exit"
            };
            lineSpacing = 5;
            MeasureMenu();
        }

        private void MeasureMenu()
        {
            menuHeight = 0;
            menuWidth = 0;
            foreach (string item in menuItems)
            {
                Vector2 size = spriteFontSmall.MeasureString(item);
                if (size.X > menuWidth)
                    menuWidth = size.X;
                menuHeight += spriteFontSmall.LineSpacing + lineSpacing;
            }

            position = new Vector2((screenWidth - menuWidth) / 2, (screenHeight - menuHeight) / 2);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (CheckKey(Keys.Down))
            {
                selectedIndex++;
                if (selectedIndex == menuItems.Length)
                    selectedIndex = menuItems.Length - 1;
            }
            if (CheckKey(Keys.Up))
            {
                selectedIndex--;
                if (selectedIndex < 0)
                    selectedIndex = 0;
            }

            oldKeyboardState = keyboardState;
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            Vector2 location = position;

            for (int i = 0; i < menuItems.Length; i++)
            {
                if (i == selectedIndex)
                    itemColor = highlitedColor;
                else
                    itemColor = normalColor;

                spriteBatch.Begin();
                spriteBatch.DrawString(spriteFontSmall, menuItems[i], location, itemColor);
                location.Y += spriteFontSmall.LineSpacing + lineSpacing;
                spriteBatch.End();
            }
        }
    }
}
