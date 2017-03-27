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
    abstract class Spell
    {
        protected string name;
        public string Name { get { return name; } }
        protected int spellPower;
        public int SpellPower { get { return spellPower; } }
        protected int width;
        protected int height;
        public int Width { get { return width; } }
        public int Height { get { return height; } }
        protected Vector2 position;
        public Vector2 Position { get { return position; } }
        protected Vector2 opponentPosition;
        protected Texture2D texture;
        protected Rectangle sourceRectangle;
        protected Rectangle destinationRectangle;
        protected int currentFrame = 0;
        protected int textureColumns;
        protected int textureRows;
        protected float animationTimer = 0;
        protected GameTime gameTime;
        protected int startFrame;
        protected int endFrame;
        protected float animationFrames;
        public float MagicSpeed;
        protected bool endOfLife = false;
        public bool EndOfLife { get { return endOfLife; } }
        protected IEquatable<Rectangle> figure;
        public IEquatable<Rectangle> Figure { get { return figure; } }

        public Spell(string name, int hitPoints, Texture2D texture, float castStartXPosition, float castStartYPosition, GameTime gameTime, int startFrame, int endFrame,
            int textureColumns, int textureRows, float animationFrames, float magicSpeed)
        { }

        public virtual void HandleCollsion(int collStartFrame, int collEndFrame, float collPerFrame)
        {
            endOfLife = true;
            MagicSpeed = 0;
            startFrame = collStartFrame;
            endFrame = collEndFrame;
            animationFrames = collPerFrame;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            int row = (int)((float)currentFrame / (float)textureColumns);
            int column = currentFrame % textureColumns;

            sourceRectangle = new Rectangle(width * column, height * row, width, height);
            destinationRectangle = new Rectangle((int)position.X, (int)position.Y, width, height);

            spriteBatch.Begin();
            spriteBatch.Draw(texture, destinationRectangle, sourceRectangle, Color.White);
            spriteBatch.End();
        }
    }
}
