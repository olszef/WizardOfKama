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
    class Control
    {
        KeyboardState state;
        List<Keys> keysPressed;
        List<Keys> controlKeys = new List<Keys>();
        private const float clickTimePassed = 10;
        int[] comboKeyPosition = new int[3];
        float clickTimer = 0;

        public KeysCombo KeysDown(GameTime gameTime)
        {
            clickTimer += gameTime.ElapsedGameTime.Milliseconds;
            if (controlKeys.Count > 0 && clickTimer > clickTimePassed)
            {
                clickTimer = 0;
                controlKeys.Clear();
                //controlKeys.RemoveAt(0);
            }

            state = Keyboard.GetState();
            keysPressed = (state.GetPressedKeys()).ToList();

            if (keysPressed.Contains(Keys.Left))
                controlKeys.Add(Keys.Left);

            if (keysPressed.Contains(Keys.Right))
                controlKeys.Add(Keys.Right);

            if (keysPressed.Contains(Keys.Up))
                controlKeys.Add(Keys.Up);

            if (keysPressed.Contains(Keys.Down))
                controlKeys.Add(Keys.Down);

            if (keysPressed.Contains(Keys.W))
                controlKeys.Add(Keys.W);

            if (keysPressed.Contains(Keys.A))
                controlKeys.Add(Keys.A);

            if (keysPressed.Contains(Keys.S))
                controlKeys.Add(Keys.S);

            if (keysPressed.Contains(Keys.D))
                controlKeys.Add(Keys.D);

            if (keysPressed.Contains(Keys.X))
                controlKeys.Add(Keys.X);

            if (controlKeys != null)
            {
                if (controlKeys.Count == 1)
                {
                    if (controlKeys.Contains(Keys.Left))
                        return KeysCombo.LeftArrow;
                    if (controlKeys.Contains(Keys.Right))
                        return KeysCombo.RightArrow;
                    if (controlKeys.Contains(Keys.Up))
                        return KeysCombo.UpArrow;
                    if (controlKeys.Contains(Keys.Down))
                        return KeysCombo.DownArrow;
                    if (controlKeys.Contains(Keys.W))
                        return KeysCombo.W;
                    if (controlKeys.Contains(Keys.A))
                        return KeysCombo.A;
                    if (controlKeys.Contains(Keys.S))
                        return KeysCombo.S;
                    if (controlKeys.Contains(Keys.D))
                        return KeysCombo.D;
                    else
                        return KeysCombo.None;
                }
                else
                {
                    if (controlKeys.Contains(Keys.Left) && controlKeys.Contains(Keys.Up))
                        return KeysCombo.UpLeftArrows;
                    if (controlKeys.Contains(Keys.Right) && controlKeys.Contains(Keys.Up))
                        return KeysCombo.UpRightArrows;
                    if (controlKeys.Contains(Keys.Left) && controlKeys.Contains(Keys.Down) && controlKeys.Contains(Keys.X))
                    {
                        comboKeyPosition[0] = controlKeys.FindIndex(x => x == Keys.Left);
                        comboKeyPosition[1] = controlKeys.FindIndex(x => x == Keys.Down);
                        comboKeyPosition[2] = controlKeys.FindIndex(x => x == Keys.X);
                        if (comboKeyPosition[0] < comboKeyPosition[1] && comboKeyPosition[1] < comboKeyPosition[2])
                            return KeysCombo.DownLeftX;
                        else
                            return KeysCombo.None; //do zmiany
                    }
                    else
                        return KeysCombo.None; //do zmiany
                }
            }
            else
                return KeysCombo.None;
        }
    }
}
