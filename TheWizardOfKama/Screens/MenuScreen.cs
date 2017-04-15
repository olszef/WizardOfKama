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

        public MenuScreen(Game game, ContentManager content, SpriteBatch spriteBatch, ScreenTypes screenType) : base(game, content, spriteBatch, screenType)
        {
            mainMenu = new MainMenu(game, content, spriteBatch, "backgrounds/MainMenu", MenuItemTypes.MainMenu);
            instructions = new MenuItem(game, content, spriteBatch, "backgrounds/InstructionsItem", MenuItemTypes.Instructions);
            controls = new MenuItem(game, content, spriteBatch, "backgrounds/ControlsItem", MenuItemTypes.Controls);
            credits = new MenuItem(game, content, spriteBatch, "backgrounds/CreditsItem", MenuItemTypes.Credits);
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

        public override GameStats Update(GameTime gameTime)
        {
            foreach (MenuComponent component in menuComponents)
            {
                if (component.IsEnabled)
                    component.Update(gameTime);
            }

            return endGameStats;
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            foreach (MenuComponent component in menuComponents)
            {
                if (component.IsEnabled)
                {
                    component.Draw(gameTime);
                }
            }
        }

        public MenuItemTypes GetActiveItem()
        {
            return activeMenu.ComponentType;
        }

        public MenuItemTypes GetMenuSelectedItem()
        {
            return (MenuItemTypes)mainMenu.SelectedIndex;
        }

        public void ShowMenuComponent(MenuItemTypes itemToShow)
        {
            activeMenu.Hide();
            activeMenu = menuComponents[(int)itemToShow];
            activeMenu.Show();
        }
    }
}
