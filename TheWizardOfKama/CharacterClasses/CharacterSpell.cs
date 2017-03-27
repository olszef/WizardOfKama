using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Design;

namespace TheWizardOfKama
{
    class CharacterSpell : Spell
    {
        public CharacterSpell(string name, int spellPower, Texture2D texture, float castStartXPosition, float castStartYPosition, GameTime gameTime, int startFrame, int endFrame, int textureColumns, int textureRows, float animationFrames, float magicSpeed) 
            : base(name, spellPower, texture, castStartXPosition, castStartYPosition, gameTime, startFrame, endFrame, textureColumns, textureRows, animationFrames, magicSpeed)
        {
            this.name = name;
            this.spellPower = spellPower;
            this.texture = texture;
            position.X = castStartXPosition;
            position.Y = castStartYPosition;
            currentFrame = startFrame;
            this.gameTime = gameTime;
            this.endFrame = endFrame;
            this.startFrame = startFrame;
            this.textureColumns = textureColumns;
            this.textureRows = textureRows;
            this.MagicSpeed = magicSpeed;
            this.animationFrames = animationFrames;
            width = texture.Width / textureColumns;
            height = texture.Height / textureRows;

            if (name == "lighting")
                figure = new Rectangle();
            else
                figure = new Circle();
        }

        public bool CalculatePosition(Vector2 wizardPosition)
        {
            animationTimer += gameTime.ElapsedGameTime.Milliseconds;
            if (animationTimer > animationFrames)
            {
                animationTimer = 0;
                if (name == "shield")
                {
                    position.X = wizardPosition.X - 70;
                    position.Y = wizardPosition.Y - 50;
                }
                else
                {
                    position.X += MagicSpeed;
                }         
                
                currentFrame++;
            }

            if (endOfLife)
            {
                if (name == "lighting")
                    figure = new Rectangle();
                else
                    figure = new Circle();
            }
            else
            {
                if (name == "lighting")
                    figure = new Rectangle((int)position.X, (int)position.Y, width, height);
                else
                    figure = new Circle(new Vector2((position.X + (width / 2)), (position.Y + (height / 2))), width / 2);
            }

            if (currentFrame > endFrame && endOfLife == false)
                currentFrame = startFrame;
            else if (currentFrame > endFrame && endOfLife == true)
                return true;

            return false;
        }
    }
}
