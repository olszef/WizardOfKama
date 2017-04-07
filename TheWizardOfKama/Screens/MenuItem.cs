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
        const float blinkPerFrame = 500;

        public MenuItem(Game game, ContentManager content, SpriteBatch spriteBatch, string imagePath, string name) : base(game, content, spriteBatch, imagePath, name)
        {
            textItem = "Return to Main Menu";
            itemColor = normalColor;
            itemSize = spriteFontNormal.MeasureString(textItem);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // text blinkig
            animationTimer += gameTime.ElapsedGameTime.Milliseconds;
            if (animationTimer >= blinkPerFrame)
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

            //textSize = spriteFontBig.MeasureString(name);
            spriteBatch.Begin();
            //spriteBatch.DrawString(spriteFontBig, name, new Vector2((screenWidth - textSize.X) / 2, 100), Color.Gold);
            spriteBatch.DrawString(spriteFontNormal, textItem, new Vector2((screenWidth - itemSize.X) / 2, screenHeight - 100), itemColor);
            spriteBatch.End();
        }
    }
}
