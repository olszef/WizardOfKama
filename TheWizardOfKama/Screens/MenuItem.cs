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
    class MenuItem : MenuComponent
    {
        float animationTimer = 0;
        const float blinkPerMiliseconds = 500;

        public MenuItem(Game game, ContentManager content, SpriteBatch spriteBatch, string imagePath, MenuItemTypes componentType) : base(game, content, spriteBatch, imagePath, componentType)
        {
            this.name = componentType.ToString();
            textItem = "Return to Main Menu";
            itemColor = normalColor;
            itemSize = spriteFontNormal.MeasureString(textItem);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            animationTimer += gameTime.ElapsedGameTime.Milliseconds;
            if (animationTimer >= blinkPerMiliseconds)
            {
                animationTimer = 0;
                if (itemColor == normalColor)
                    itemColor = highlitedColor;
                else
                    itemColor = normalColor;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            spriteBatch.Begin();
            spriteBatch.DrawString(spriteFontNormal, textItem, new Vector2((screenWidth - itemSize.X) / 2, screenHeight - 100), itemColor);
            spriteBatch.End();
        }
    }
}
