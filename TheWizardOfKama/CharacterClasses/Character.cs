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
    class Character
    {
        Rectangle sourceRectangle;
        Rectangle destinationRectangle;
        KeysCombo keysCombo;
        KeysCombo oldKeysCombo;
        MoveState moveState = MoveState.Stand;
        public MoveState MoveState { get { return moveState; } }
        MoveState oldState = MoveState.Stand;
        Texture2D charTexture;
        Texture2D castTexture;
        Texture2D drawTexture;
        const int charRows = 7;
        const int charColumns = 4;
        const int castRows = 3;
        const int castColumns = 1;
        private int currentFrame;
        int charWidth;
        int charHeight;
        int castWidth;
        int castHeight;
        Control control;
        float animationTimer = 0;
        float castingTimer = 0;
        float shieldTimer = 0;
        int shieldTimerMultiplier = 3;
        float manaRegenerationTimer = 0;
        const float manaRegenerationTime = 2000;
        const int manaRegenerated = 2;
        const float startXPosition = 20;
        const float startYPosition = 480;
        const int idlePerFrame = 80;
        const int idleStartFrame = 0;
        const int idleEndFrame = 2;
        const int walkPerFrame = 40;
        const int walkRightStartFrame = 5;
        const int walkRightEndFrame = 7;
        const int walkLeftStartFrame = 25;
        const int walkLeftEndFrame = 27;
        const int jumpPerFrame = 40;
        const int jumpRightStartFrame = 12;
        const int jumpRightEndFrame = 14;
        const int jumpLeftStartFrame = 16;
        const int jumpLeftEndFrame = 19;
        const int hurtPerFrame = 80;
        const int hurtStartFrame = 20;
        const int hurtEndFrame = 21;
        const int deadPerFrame = 100;
        const int deadStartFrame = 20;
        const int deadEndFrame = 23;
        const int castingStartFrame = 0;
        const int castingEndFrame = 2;
        const float castingAnimationLength = 500;
        const float walkSpeed = 20;
        Vector2 position;
        public Vector2 Position { get { return position; } }
        const float basicPosMofifier = 0;
        float posYModifier;
        float posXModifier;
        const float jumpSpeed = 0.1f;
        float jumpMovement;
        float magicFrame;
        float castStartXPosition;
        float castStartYPosition;
        Texture2D[] spellsTextures;
        List<CharacterSpell> spells = new List<CharacterSpell>();
        public List<CharacterSpell> Spells { get { return spells; } }
        int screenWidth;
        int screenHeight;
        Dictionary<string, SpellParam> spellParams;
        //* Collision data *
        Circle charCircle;
        public Circle CharCircle { get { return charCircle; } }
        bool isWizMonColl = false;
        //* Character stats data *
        int health;
        public int Health { get { return health; } }
        int mana;
        public int Mana { get { return mana; } }
        int level = 1;
        public int Level { get { return level; } }
        int experience = 0;
        const int expToNextLvl = 1000;
        int baseMana = 100;
        const int baseHealth = 100;
        bool noMana = false;
        bool specialAbilityLeft = true;
        //**************************************************************

        public Character(Texture2D wizardTexture, Texture2D castingTexture, Texture2D[] spells, int screenWidth, int screenHeight)
        {
            charTexture = wizardTexture;
            castTexture = castingTexture;
            currentFrame = 0;
            charWidth = charTexture.Width / charColumns;
            charHeight = charTexture.Height / charRows;
            castWidth = castingTexture.Width / castColumns;
            castHeight = castingTexture.Height / castRows;
            position = new Vector2(startXPosition, startYPosition);
            control = new Control();
            posYModifier = basicPosMofifier;
            health = baseHealth;
            mana = baseMana;
            this.spellsTextures = spells;
            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;
            InitSpellParamDict();
        }

        private void InitSpellParamDict()
        {
            spellParams = new Dictionary<string, SpellParam>
            {
                {"lighting", new SpellParam(5, 8) },
                {"water", new SpellParam(7, 50) },
                {"gravity", new SpellParam(3, 7) },
                {"shield", new SpellParam(20, 0) },
                {"special", new SpellParam(99, 9999) },
            };
        }

        public void UpdateWizard(GameTime gameTime)
        {
            if (moveState == MoveState.Hurt)
            {
                GetHurt(gameTime);
            }
            if (moveState == MoveState.Dead)
            {
                AnimateWizardsDeath(gameTime);
            }
            else
            {
                PickState(gameTime);
                charCircle = new Circle(new Vector2((position.X + (charWidth / 2)), (position.Y + (charHeight / 2))), charWidth / 2);
                
                //regenerate mana
                if (mana < baseMana)
                {
                    manaRegenerationTimer += gameTime.ElapsedGameTime.Milliseconds;
                    if (manaRegenerationTimer >= manaRegenerationTime)
                    {
                        manaRegenerationTimer = 0;
                        mana += manaRegenerated;
                        if (mana > baseMana)
                            mana = baseMana;
                    }
                } 
            }
            UpdateSpells(gameTime);
            isWizMonColl = false;
        }

        public void DrawWizard(SpriteBatch spriteBatch)
        {

            if (moveState == MoveState.Cast)
            {
                int row = currentFrame;
                int column = 0;

                sourceRectangle = new Rectangle(castWidth * column, castHeight * row, castWidth, castHeight);
                destinationRectangle = new Rectangle((int)position.X, (int)position.Y, castWidth, castHeight);
                drawTexture = castTexture;
            }
            else
            {
                int row = (int)((float)currentFrame / (float)charColumns);
                int column = currentFrame % charColumns;

                sourceRectangle = new Rectangle(charWidth * column, charHeight * row, charWidth, charHeight);
                destinationRectangle = new Rectangle((int)position.X, (int)position.Y, charWidth, charHeight);
                drawTexture = charTexture;
            }

            spriteBatch.Begin();
            spriteBatch.Draw(drawTexture, destinationRectangle, sourceRectangle, Color.White);
            spriteBatch.End();
        }

        public void DrawWizardSpells(SpriteBatch spriteBatch)
        {
            foreach (CharacterSpell spell in spells)
                spell.Draw(spriteBatch);

        }

        private void Jump(GameTime gameTime)
        {
            animationTimer += gameTime.ElapsedGameTime.Milliseconds;
            if (animationTimer > jumpPerFrame)
            {
                animationTimer = 0;
                position.Y -= (-300)*((float)Math.Pow(posYModifier,2)) + (300*posYModifier);
                if (position.X > 0 && (position.X + charWidth) < screenWidth)
                    if (!isWizMonColl)
                        position.X += jumpMovement;
                    else
                        position.X -= 10;
                posYModifier += jumpSpeed;
                posXModifier += jumpMovement;

                if ((currentFrame == jumpRightStartFrame + 1) || (currentFrame == jumpLeftStartFrame + 1) )
                {
                    if (position.Y >= startYPosition + 50)
                    {
                        position.Y = startYPosition;
                        posYModifier = basicPosMofifier;
                        posXModifier = basicPosMofifier;
                        moveState = MoveState.Stand;
                    }
                }
                else
                {
                    currentFrame++;
                }
            }
        }

        private void Cast(GameTime gameTime)
        {
            animationTimer += gameTime.ElapsedGameTime.Milliseconds;
            castingTimer += gameTime.ElapsedGameTime.Milliseconds;
            if (animationTimer > magicFrame)
            {
                animationTimer = 0;
                currentFrame++;
                if (currentFrame > castingEndFrame)
                    currentFrame = castingStartFrame + 1;

                if (castingTimer > castingAnimationLength)
                {
                    castingTimer = 0;
                    moveState = MoveState.Stand;
                }                    
            }
        }

        private void UpdateSpells(GameTime gameTime)
        {
            if (spells != null)
            {
                foreach (CharacterSpell spell in spells)
                {
                    if (spell.EndOfLife == true)
                    {
                        if(spell.CalculatePosition(position))
                        {
                            spells.Remove(spell);
                            break;
                        }
                    }
                    else
                    {
                        if (spell.Position.X >= screenWidth)
                        {
                            spells.Remove(spell);
                            break;
                        }
                        else
                        {
                            // detect if spell is 'shield'
                            if (spell.Name == "shield")
                            {
                                shieldTimer += gameTime.ElapsedGameTime.Milliseconds;
                                if (shieldTimer > castingAnimationLength * shieldTimerMultiplier)
                                {
                                    spells.Remove(spell);
                                    shieldTimer = 0;
                                    break;
                                }
                                else
                                    spell.CalculatePosition(position);
                            }
                            else
                                spell.CalculatePosition(position);
                        }
                    }
                }
            }
        }

        private void PickState(GameTime gameTime)
        {
            keysCombo = control.KeysDown(gameTime);
            if ((moveState != MoveState.Jump) && (moveState != MoveState.Cast))
            {
                switch (keysCombo)
                {
                    case KeysCombo.RightArrow:
                        if (oldKeysCombo != KeysCombo.RightArrow)
                            currentFrame = walkRightStartFrame;

                        animationTimer += gameTime.ElapsedGameTime.Milliseconds;
                        if (animationTimer > walkPerFrame)
                        {
                            animationTimer = 0;
                            if ((position.X + charWidth) < screenWidth && !isWizMonColl)
                                position.X += walkSpeed;
                            currentFrame++;
                            if (currentFrame > walkRightEndFrame)
                                currentFrame = walkRightStartFrame;
                        }
                        break;
                    case KeysCombo.LeftArrow:
                        if (oldKeysCombo != KeysCombo.LeftArrow)
                            currentFrame = walkLeftStartFrame;

                        animationTimer += gameTime.ElapsedGameTime.Milliseconds;
                        if (animationTimer > walkPerFrame)
                        {
                            animationTimer = 0;
                            if (position.X > 0)
                                position.X -= walkSpeed;
                            currentFrame++;
                            if (currentFrame > walkLeftEndFrame)
                                currentFrame = walkLeftStartFrame;
                        }
                        break;
                    case KeysCombo.UpArrow:
                        currentFrame = jumpRightStartFrame;
                        moveState = MoveState.Jump;
                        posYModifier = basicPosMofifier;
                        jumpMovement = 0;
                        break;
                    case KeysCombo.UpRightArrows:
                        currentFrame = jumpRightStartFrame;
                        moveState = MoveState.Jump;
                        posYModifier = basicPosMofifier;
                        jumpMovement = 10;
                        break;
                    case KeysCombo.UpLeftArrows:
                        currentFrame = jumpLeftStartFrame;
                        moveState = MoveState.Jump;
                        posYModifier = basicPosMofifier;
                        jumpMovement = -10;
                        break;
                    case KeysCombo.W:
                        if (spellParams["lighting"].SpellCost <= mana)
                        {
                            currentFrame = castingStartFrame;
                            moveState = MoveState.Cast;
                            magicFrame = 110;
                            castStartXPosition = position.X + castWidth / 4 * 3;
                            castStartYPosition = position.Y - 100;
                            mana -= spellParams["lighting"].SpellCost;
                            spells.Add(new CharacterSpell("lighting", spellParams["lighting"].SpellPower, spellsTextures[0], castStartXPosition, castStartYPosition, gameTime, 0, 9, 4, 3, 30, 20));
                        }
                        else
                        {
                            noMana = true;
                        }
                        break;
                    case KeysCombo.A:
                        if (spellParams["water"].SpellCost <= mana)
                        {
                            currentFrame = castingStartFrame;
                            moveState = MoveState.Cast;
                            magicFrame = 110;
                            castStartXPosition = position.X + castWidth / 4 * 3;
                            castStartYPosition = position.Y;
                            mana -= spellParams["water"].SpellCost;
                            spells.Add(new CharacterSpell("water", spellParams["water"].SpellPower, spellsTextures[1], castStartXPosition, castStartYPosition, gameTime, 5, 12, 5, 5, 30, 20));
                        }
                        else
                        {
                            noMana = true;
                        }
                        break;
                    case KeysCombo.S:
                        if (spellParams["gravity"].SpellCost <= mana)
                        {
                            currentFrame = castingStartFrame;
                            moveState = MoveState.Cast;
                            magicFrame = 110;
                            castStartXPosition = position.X + castWidth / 4 * 3;
                            castStartYPosition = position.Y;
                            mana -= spellParams["gravity"].SpellCost;
                            spells.Add(new CharacterSpell("gravity", spellParams["gravity"].SpellPower, spellsTextures[2], castStartXPosition, castStartYPosition, gameTime, 4, 10, 5, 4, 30, 20));
                        }
                        else
                        {
                            noMana = true;
                        }
                        break;
                    case KeysCombo.D:
                        if (spellParams["shield"].SpellCost <= mana)
                        {
                            currentFrame = castingStartFrame;
                            moveState = MoveState.Cast;
                            magicFrame = 110;
                            castStartXPosition = position.X;
                            castStartYPosition = position.Y;
                            mana -= spellParams["shield"].SpellCost;
                            spells.Add(new CharacterSpell("shield", spellParams["shield"].SpellPower, spellsTextures[3], castStartXPosition, castStartYPosition, gameTime, 0, 0, 1, 1, 30, 0));
                        }
                        else
                        {
                            noMana = true;
                        }
                        break;
                    case KeysCombo.DownLeftX:
                        if (spellParams["special"].SpellCost <= mana && specialAbilityLeft)
                        {
                            castStartXPosition = position.X;
                            castStartYPosition = position.Y;
                            mana -= spellParams["special"].SpellCost;
                            specialAbilityLeft = false;
                            spells.Add(new CharacterSpell("special", spellParams["special"].SpellPower, spellsTextures[4], castStartXPosition, castStartYPosition, gameTime, 0, 4, 1, 27, 80, 0));
                        }
                        else
                        {
                            noMana = true;
                        }
                        break;
                    default:
                        animationTimer += gameTime.ElapsedGameTime.Milliseconds;
                        if (animationTimer > idlePerFrame)
                        {
                            animationTimer = 0;
                            currentFrame++;
                            if (currentFrame > idleEndFrame)
                                currentFrame = idleStartFrame;
                        }
                        break;
                }
                oldKeysCombo = keysCombo;
            }

            // condition to continue idle when the cast button is pressed, but the wizard has not enough mana
            if (noMana)
            {
                animationTimer += gameTime.ElapsedGameTime.Milliseconds;
                if (animationTimer > idlePerFrame)
                {
                    animationTimer = 0;
                    currentFrame++;
                    if (currentFrame > idleEndFrame)
                        currentFrame = idleStartFrame;
                }
                noMana = false;
            }

            // perform additional action
            switch (moveState)
            {
                case MoveState.Jump:
                    Jump(gameTime);
                    break;
                case MoveState.Cast:
                    Cast(gameTime);
                    break;
                default:
                    break;
            }
        }

        private void GetHurt(GameTime gameTime)
        {
            position.Y = startYPosition;
            if (oldState != MoveState.Hurt)
                currentFrame = hurtStartFrame;

            animationTimer += gameTime.ElapsedGameTime.Milliseconds;
            if (animationTimer > hurtPerFrame)
            {
                animationTimer = 0;
                currentFrame++;
            }
            if (currentFrame > hurtEndFrame)
            {
                currentFrame = idleStartFrame;
                moveState = MoveState.Stand;
                oldKeysCombo = KeysCombo.None;
            }

            oldState = moveState;
        }

        private void AnimateWizardsDeath (GameTime gameTime)
        {
            position.Y = startYPosition;
            if (oldState != MoveState.Dead)
                currentFrame = deadStartFrame;

            animationTimer += gameTime.ElapsedGameTime.Milliseconds;
            if (animationTimer > deadPerFrame)
            {
                animationTimer = 0;
                if (currentFrame < deadEndFrame)
                    currentFrame++;
            }

            oldState = moveState;
        }

        public void HandleWizardCollision(bool isHurt, int hitPoints)
        {
            isWizMonColl = true;
            if (isHurt)
            {
                health -= hitPoints;
                if (health <= 0)
                {
                    moveState = MoveState.Dead;
                    if (health < 0)
                        health = 0;
                }
                else
                {
                    moveState = MoveState.Hurt;
                }
            }
        }

        public void HandleSpellCollision(int spellNumber)
        {
            int startFrame = 0, endFrame = 0, perFrame = 0;
            switch(Spells[spellNumber].Name)
            {
                case "lighting":
                    startFrame = 10;
                    endFrame = 11;
                    perFrame = 60;
                    break;
                case "water":
                    startFrame = 13;
                    endFrame = 24;
                    perFrame = 40;
                    break;
                case "gravity":
                    startFrame = 11;
                    endFrame = 19;
                    perFrame = 40;
                    break;
                case "special":
                    break;
            }
            spells[spellNumber].HandleCollsion(startFrame, endFrame, perFrame);
        }

        public bool GainExperience(int expPoints)
        {
            experience += expPoints;
            if (experience >= level * expToNextLvl)
            {
                level += 1;
                LevelUp();
                return true;
            }
            else
                return false;
        }

        private void LevelUp()
        {
            // Each level increase max Mana Points and get healed
            baseMana += 5;
            mana = baseMana;
            health = baseHealth;

            // Additional bonus depending on the level (only up to lvl 10)
            switch(level)
            {
                case 2:
                    spellParams["lighting"].SpellCost -= 1;
                    break;
                case 3:
                    spellParams["water"].SpellCost -= 1;
                    break;
                case 4:
                    spellParams["gravity"].SpellCost -= 1;
                    break;
                case 5:
                    spellParams["shield"].SpellCost -= 5;
                    break;
                case 6:
                    spellParams["lighting"].SpellPower += 2;
                    break;
                case 7:
                    spellParams["water"].SpellPower += 2;
                    break;
                case 8:
                    spellParams["gravity"].SpellPower += 2;
                    break;
                case 9:
                    shieldTimerMultiplier += 1;
                    break;
                case 10:
                    specialAbilityLeft = true;
                    break;
            }



        }
    }
}
