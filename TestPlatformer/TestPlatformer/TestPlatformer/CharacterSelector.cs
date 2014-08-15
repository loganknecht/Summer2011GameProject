using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TestPlatformer
{
    class CharacterSelector
    {
        public ContentManager cmReference;
        public GraphicsDeviceManager gdmReference;
        public SpriteBatch sbReference;

        public Vector2 worldPosition;

        public Texture2D selectorImage;

        public bool selectorLocked;

        public int hoverHorizontalSelection;
        public int hoverVerticalSelection;

        public int horizontalSelectionMax;
        public int verticalSelectionMax;

        public int selectionDelayTimer;
        public int selectionDelayMax;

        public CharacterSelector(ContentManager cm, GraphicsDeviceManager gdm, SpriteBatch sb, ImageLibrary iLibrary, Vector2 worldPos, int hovHorSelection, int hovVerSelection)
        {
            cmReference = cm;
            gdmReference = gdm;
            sbReference = sb;

            hoverHorizontalSelection = hovHorSelection;
            hoverVerticalSelection = hovVerSelection;

            horizontalSelectionMax = 1;
            verticalSelectionMax = 1;

            selectorImage = iLibrary.characterSelector;
            
            worldPosition = worldPos;

            selectorLocked = false;
            selectionDelayTimer = 0;
            selectionDelayMax = 15;
        }
        public void draw() {
            sbReference.Draw(selectorImage, worldPosition, Color.White);
        }
        public void drawAltPosition(Vector2 altPos)
        {
            sbReference.Draw(selectorImage, altPos, Color.White);
        }

        public void updateSelectorViaKeyboard(KeyboardState previousKBState, KeyboardState currentKBState) {
            if (selectionDelayTimer < selectionDelayMax)
            {
                selectionDelayTimer++;
            }
            else
            {
                //selector moved left
                if (previousKBState.IsKeyUp(Keys.A) && currentKBState.IsKeyDown(Keys.A))
                {
                    hoverHorizontalSelection--;
                }
                //selector moved right
                if (previousKBState.IsKeyUp(Keys.D) && currentKBState.IsKeyDown(Keys.D))
                {
                    hoverHorizontalSelection++;
                }
                //selector moved up
                if (previousKBState.IsKeyUp(Keys.W) && currentKBState.IsKeyDown(Keys.W))
                {
                    hoverVerticalSelection--;
                }
                //selector moved down
                if (previousKBState.IsKeyUp(Keys.S) && currentKBState.IsKeyDown(Keys.S))
                {
                    hoverVerticalSelection++;
                }

                //horizontal rollover
                if (hoverHorizontalSelection < 0)
                {
                    hoverHorizontalSelection = horizontalSelectionMax;
                }
                if (hoverHorizontalSelection > horizontalSelectionMax)
                {
                    hoverHorizontalSelection = 0;
                }

                //vertical rollover
                if (hoverVerticalSelection < 0)
                {
                    hoverVerticalSelection = verticalSelectionMax;
                }
                if (hoverVerticalSelection > verticalSelectionMax)
                {
                    hoverVerticalSelection = 0;
                }

                //this is the select character option
                if (previousKBState.IsKeyUp(Keys.Enter) && currentKBState.IsKeyDown(Keys.Enter))
                {
                    selectorLocked = true;
                }
                //this is the select character option
                if (previousKBState.IsKeyUp(Keys.RightShift) && currentKBState.IsKeyDown(Keys.RightShift))
                {
                    selectorLocked = false;
                }
            }
        }

        public void updateSelectorViaGamePad(GamePadState previousGPState, GamePadState currentGPState)
        {
            if (selectionDelayTimer < selectionDelayMax)
            {
                selectionDelayTimer++;
            }
            else
            {
                //selector moved left
                if (previousGPState.IsButtonUp(Buttons.DPadLeft) && currentGPState.IsButtonDown(Buttons.DPadLeft))
                {
                    hoverHorizontalSelection--;
                }
                //selector moved right
                if (previousGPState.IsButtonUp(Buttons.DPadRight) && currentGPState.IsButtonDown(Buttons.DPadRight))
                {
                    hoverHorizontalSelection++;
                }
                //selector moved up
                if (previousGPState.IsButtonUp(Buttons.DPadUp) && currentGPState.IsButtonDown(Buttons.DPadUp))
                {
                    hoverVerticalSelection--;
                }
                //selector moved down
                if (previousGPState.IsButtonUp(Buttons.DPadDown) && currentGPState.IsButtonDown(Buttons.DPadDown))
                {
                    hoverVerticalSelection++;
                }

                //horizontal rollover
                if (hoverHorizontalSelection < 0)
                {
                    hoverHorizontalSelection = horizontalSelectionMax;
                }
                if (hoverHorizontalSelection > horizontalSelectionMax)
                {
                    hoverHorizontalSelection = 0;
                }

                //vertical rollover
                if (hoverVerticalSelection < 0)
                {
                    hoverVerticalSelection = verticalSelectionMax;
                }
                if (hoverVerticalSelection > verticalSelectionMax)
                {
                    hoverVerticalSelection = 0;
                }

                //this is the select character option
                if (previousGPState.IsButtonUp(Buttons.A) && currentGPState.IsButtonDown(Buttons.A))
                {
                    selectorLocked = true;
                }
                //this is the select character option
                if (previousGPState.IsButtonUp(Buttons.B) && currentGPState.IsButtonDown(Buttons.B))
                {
                    selectorLocked = false;
                }
            }
        }
    }
}
