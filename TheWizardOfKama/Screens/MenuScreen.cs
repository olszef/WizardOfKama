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
    class MenuScreen : GameScreen
    {
        MenuComponent activeMenu;
        MainMenu mainMenu;
        MenuItem instructions;
        MenuItem controls;
        MenuItem credits;
        List<MenuComponent> menuComponents;

        public MenuScreen(Game game, ContentManager content, SpriteBatch spriteBatch, string name) : base(game, content, spriteBatch, name)
        {
            mainMenu = new MainMenu(game, content, spriteBatch, "backgrounds/MainMenu", "Main Menu");
            instructions = new MenuItem(game, content, spriteBatch, "backgrounds/InstructionsItem", "Instructions");
            controls = new MenuItem(game, content, spriteBatch, "backgrounds/ControlsItem", "Controls");
            credits = new MenuItem(game, content, spriteBatch, "backgrounds/CreditsItem", "Credits");
            activeMenu = mainMenu;
            activeMenu.Show();
            menuComponents = new List<MenuComponent>
            {
                mainMenu,
                instructions,
                controls,
                credits
            };
        }

        public override void Update(GameTime gameTime)
        {
            foreach (MenuComponent component in menuComponents)
            {
                if (component.Enabled)
                    component.Update(gameTime);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            foreach (MenuComponent component in menuComponents)
            {
                if (component.Enabled)
                {
                    component.Draw(gameTime);
                }
            }
        }

        public string GetActiveItemName()
        {
            return activeMenu.Name;
        }

        public int GetMenuSelectedItem()
        {
            return mainMenu.SelectedIndex;
        }

        public void ShowMenuComponent(int itemToShow)
        {
            activeMenu.Hide();
            activeMenu = menuComponents[itemToShow];
            activeMenu.Show();
        }
    }
}
