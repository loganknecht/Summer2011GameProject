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
    //the title screen was implemented in a rush of updates during the game state integration... the notes may not be very ... precise
    class TitleScreen : Screen
    {
        //controls the start blinking on the start menu
        bool pressStartIsFlashing;
        bool pressStartShow;
        int pressStartFlashTimer;
        int pressStartFlashTimerDuration;

        //controls the tile option input from the user
        bool titleOptionsAreShowing;
        int titleOptionSelected;
        int titleOptionHoverSelection;

        //tracks state in this screen
        KeyboardState previousKeyboardState;
        KeyboardState currentKeyboardState;

        //background image
        Texture2D titleScreenImage;

        //placeholder font
        SpriteFont titleFont;
        Vector2 titleFontLocation;

        //passes into the base screen the default constructor parameters, then it loads the images and sets the states to their appropriate values
        public TitleScreen(ContentManager cManager, GraphicsDeviceManager gdmReference, SpriteBatch sbReference) : base(cManager, gdmReference, sbReference) {
            titleScreenImage = cmReference.Load<Texture2D>("TitleScreens/CrosswindsTitle");

            pressStartIsFlashing = true;
            pressStartShow = true;
            pressStartFlashTimer = 0;
            pressStartFlashTimerDuration = 90;

            titleOptionsAreShowing = false;
            titleOptionSelected = 0;
            titleOptionHoverSelection = 1;

            titleFont = cmReference.Load<SpriteFont>("Fonts/PlaceholderFont");
            titleFontLocation = new Vector2(graphicsDeviceManagerReference.GraphicsDevice.Viewport.TitleSafeArea.Center.X, graphicsDeviceManagerReference.GraphicsDevice.Viewport.TitleSafeArea.Center.Y);
        }

        //returns the option selected, if 0 nothing has been selected yet, if 1 the game start has been selected, and if 2 the options have been selected
        public int getTitleOptionSelected {
            get {
                return titleOptionSelected;
            }
        }

        //handles the screen logic
        public override void updateScreen(GameTime gameTimer) {
            checkForUserInput();

            if (pressStartIsFlashing)
            {
                pressStartFlashTimer++;
                if (pressStartFlashTimer < pressStartFlashTimerDuration)
                {
                    pressStartFlashTimer++;
                }
                else
                {
                    pressStartFlashTimer = 0;
                    if (pressStartShow)
                    {
                        pressStartShow = false;
                    }
                    else
                    {
                        pressStartShow = true;
                    }
                }
            }
            else if (titleOptionsAreShowing) {

            }
        }

        public override void draw(GameTime gameTimer) {
            spriteBatchReference.Begin();
            
            spriteBatchReference.Draw(titleScreenImage, new Vector2(0, 0), Color.White);
            spriteBatchReference.DrawString(titleFont, "Crosswinds", titleFontLocation/2, Color.White);

            if (pressStartIsFlashing) {
                if (pressStartShow) {
                    spriteBatchReference.DrawString(titleFont, "Press Start", new Vector2(titleFontLocation.X-120, titleFontLocation.Y+150), Color.White, 0f, Vector2.Zero, .5f, SpriteEffects.None, 0f);
                }
                else { 
                    //do nothing
                }
            }
            else if (titleOptionsAreShowing) {
                spriteBatchReference.DrawString(titleFont, "Start Game", new Vector2(titleFontLocation.X - 100, titleFontLocation.Y + 135), Color.White, 0f, Vector2.Zero, .4f, SpriteEffects.None, 0f);
                spriteBatchReference.DrawString(titleFont, "Options", new Vector2(titleFontLocation.X - 100, titleFontLocation.Y + 190), Color.White, 0f, Vector2.Zero, .4f, SpriteEffects.None, 0f);
                if (titleOptionHoverSelection == 1) {
                    spriteBatchReference.DrawString(titleFont, ".", new Vector2(titleFontLocation.X - 130, titleFontLocation.Y + 95), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                }
                if (titleOptionHoverSelection == 2)
                {
                    spriteBatchReference.DrawString(titleFont, ".", new Vector2(titleFontLocation.X - 130, titleFontLocation.Y + 150), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                }
            }

            spriteBatchReference.End();
        }

        //the main thing that needs to be understood is that if you're pressing the entre key it will be as if you're selecting
        //if you press the shift key it will be as though you're pressing the back button
        //up and down arrows select the option
        public void checkForUserInput() {
            previousKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();

            if (previousKeyboardState.IsKeyUp(Keys.Enter) && currentKeyboardState.IsKeyDown(Keys.Enter)) {
                if (pressStartIsFlashing && !titleOptionsAreShowing) {
                    pressStartIsFlashing = false;
                    titleOptionsAreShowing = true;
                }
                else if (!pressStartIsFlashing && titleOptionsAreShowing) {
                    titleOptionSelected = titleOptionHoverSelection;
                }
            }
            if (previousKeyboardState.IsKeyUp(Keys.RightShift) && currentKeyboardState.IsKeyDown(Keys.RightShift))
            {
                if (!pressStartIsFlashing && titleOptionsAreShowing)
                {
                    pressStartIsFlashing = true;
                    titleOptionsAreShowing = false;
                }
            }

            if (previousKeyboardState.IsKeyUp(Keys.Up) && currentKeyboardState.IsKeyDown(Keys.Up))
            {
                if (!pressStartIsFlashing && titleOptionsAreShowing)
                {
                    if (titleOptionHoverSelection == 1) {
                        titleOptionHoverSelection = 2;
                    }
                    else if (titleOptionHoverSelection == 2) {
                        titleOptionHoverSelection = 1;
                    }
                }
            }

            if (previousKeyboardState.IsKeyUp(Keys.Down) && currentKeyboardState.IsKeyDown(Keys.Down))
            {
                if (!pressStartIsFlashing && titleOptionsAreShowing)
                {
                    if (titleOptionHoverSelection == 1)
                    {
                        titleOptionHoverSelection = 2;
                    }
                    else if (titleOptionHoverSelection == 2)
                    {
                        titleOptionHoverSelection = 1;
                    }
                }
            }

        }
    }
}
