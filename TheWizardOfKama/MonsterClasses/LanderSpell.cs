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
    class LanderSpell : Spell
    {
        public LanderSpell(string name, int hitPoints, Texture2D texture, float castStartXPosition, float castStartYPosition, GameTime gameTime, int startFrame, int endFrame, int textureColumns, int textureRows, float animationFrames, float magicSpeed) 
            : base(name, hitPoints, texture, castStartXPosition, castStartYPosition, gameTime, startFrame, endFrame, textureColumns, textureRows, animationFrames, magicSpeed)
        {
            this.spellPower = hitPoints;
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
            figure = new Circle();
        }

        public bool CalculatePosition()
        {
            animationTimer += gameTime.ElapsedGameTime.Milliseconds;
            if (animationTimer > animationFrames)
            {
                position.X -= MagicSpeed;
                animationTimer = 0;
                currentFrame++;
            }

            if (endOfLife)
            {
                figure = new Circle();
            }
            else
            {
                figure = new Circle(new Vector2((position.X + (width / 2)), (position.Y + (height / 2))), width * 1/4);
            }

            if (currentFrame >= endFrame && endOfLife == false)
                currentFrame = startFrame;
            else if (currentFrame >= endFrame && endOfLife == true)
                return true;

            return false;
        }
    }
}
