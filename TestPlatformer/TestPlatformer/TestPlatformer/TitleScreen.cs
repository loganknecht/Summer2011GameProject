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

        //controls the title play mode option input from the user (single playerOne, coop, free-for-all)
        bool titleOptionsAreShowing;
        int titleOptionSelected;
        int titleOptionHoverSelection;
        int maxTitleOptions;

        bool choosePlayMode;
        int playModeSelected;
        int playModeHoverSelected;
        int playModeSelectionMax;

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
            titleOptionSelected = -1;
            titleOptionHoverSelection = 0;
            maxTitleOptions = 3;

            choosePlayMode = false;
            playModeSelected = -1;
            playModeHoverSelected = 0;
            playModeSelectionMax = 1;

            titleFont = cmReference.Load<SpriteFont>("Fonts/PlaceholderFont");
            titleFontLocation = new Vector2(gdmReference.GraphicsDevice.Viewport.TitleSafeArea.Center.X, gdmReference.GraphicsDevice.Viewport.TitleSafeArea.Center.Y);
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
            else if (choosePlayMode) { 
            }
        }

        public override void draw(GameTime gameTimer) {
            sbReference.Begin();
            
            sbReference.Draw(titleScreenImage, new Vector2(0, 0), Color.White);
            sbReference.DrawString(titleFont, "Crosswinds", titleFontLocation/2, Color.White);

            if (pressStartIsFlashing) {
                if (pressStartShow) {
                    sbReference.DrawString(titleFont, "Press Start", new Vector2(titleFontLocation.X-120, titleFontLocation.Y+150), Color.White, 0f, Vector2.Zero, .5f, SpriteEffects.None, 0f);
                }
                else { 
                    //do nothing
                }
            }
            else if (titleOptionsAreShowing) {
                sbReference.DrawString(titleFont, "Single Player Game", new Vector2(titleFontLocation.X - 120, titleFontLocation.Y + 135), Color.White, 0f, Vector2.Zero, .4f, SpriteEffects.None, 0f);
                sbReference.DrawString(titleFont, "Co-operative Play (2v2)", new Vector2(titleFontLocation.X - 140, titleFontLocation.Y + 165), Color.White, 0f, Vector2.Zero, .4f, SpriteEffects.None, 0f);
                sbReference.DrawString(titleFont, "Free For All", new Vector2(titleFontLocation.X - 75, titleFontLocation.Y + 195), Color.White, 0f, Vector2.Zero, .4f, SpriteEffects.None, 0f);
                sbReference.DrawString(titleFont, "Options", new Vector2(titleFontLocation.X - 55, titleFontLocation.Y + 225), Color.White, 0f, Vector2.Zero, .4f, SpriteEffects.None, 0f);
                
                if (titleOptionHoverSelection == 0) {
                    sbReference.DrawString(titleFont, ".", new Vector2(titleFontLocation.X - 140, titleFontLocation.Y + 95), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                }
                else if (titleOptionHoverSelection == 1)
                {
                    sbReference.DrawString(titleFont, ".", new Vector2(titleFontLocation.X - 160, titleFontLocation.Y + 125), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                }
                else if (titleOptionHoverSelection == 2) {
                    sbReference.DrawString(titleFont, ".", new Vector2(titleFontLocation.X - 95, titleFontLocation.Y + 155), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                }
                else if (titleOptionHoverSelection == 3) {
                    sbReference.DrawString(titleFont, ".", new Vector2(titleFontLocation.X - 75, titleFontLocation.Y + 185), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                }

            }
            else if (choosePlayMode) {
                sbReference.DrawString(titleFont, "Online Mode", new Vector2(titleFontLocation.X - 80, titleFontLocation.Y + 165), Color.White, 0f, Vector2.Zero, .4f, SpriteEffects.None, 0f);
                sbReference.DrawString(titleFont, "Offline Mode", new Vector2(titleFontLocation.X - 80, titleFontLocation.Y + 195), Color.White, 0f, Vector2.Zero, .4f, SpriteEffects.None, 0f);

                if (playModeHoverSelected == 0)
                {
                    sbReference.DrawString(titleFont, ".", new Vector2(titleFontLocation.X - 100, titleFontLocation.Y + 125), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                }
                else if (playModeHoverSelected == 1)
                {
                    sbReference.DrawString(titleFont, ".", new Vector2(titleFontLocation.X - 100, titleFontLocation.Y + 155), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                }
            }

            sbReference.End();
        }

        //the main thing that needs to be understood is that if you're pressing the entre key it will be as if you're selecting
        //if you press the shift key it will be as though you're pressing the back button
        //up and down arrows select the option
        public void checkForUserInput() {
            previousKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();

            if (previousKeyboardState.IsKeyUp(Keys.Enter) && currentKeyboardState.IsKeyDown(Keys.Enter)) {
                if (pressStartIsFlashing)
                {
                    pressStartIsFlashing = false;
                    titleOptionsAreShowing = true;
                }
                else if (titleOptionsAreShowing)
                {
                    titleOptionSelected = titleOptionHoverSelection;
                    if (titleOptionSelected == 1 || titleOptionSelected == 2) {
                        titleOptionsAreShowing = false;
                        choosePlayMode = true;
                        playModeHoverSelected = 0;
                    }    
                }
                else if (choosePlayMode) {
                    playModeSelected = playModeHoverSelected;
                }
            }
            //THIS IS FOR THE BACK BUTTON TO SHOW THE PRESS START MENU AGAIN BECAUSE YOU KNOW WHAT THAT'S HOW I WANTED IT DEAL WITH IT
            if (previousKeyboardState.IsKeyUp(Keys.RightShift) && currentKeyboardState.IsKeyDown(Keys.RightShift))
            {
                if (titleOptionsAreShowing)
                {
                    pressStartIsFlashing = true;
                    titleOptionsAreShowing = false;
                }
                if (choosePlayMode) {
                    titleOptionsAreShowing = true;
                    choosePlayMode = false;
                    titleOptionSelected = -1;
                }
            }

            //Selector moving up
            if (previousKeyboardState.IsKeyUp(Keys.Up) && currentKeyboardState.IsKeyDown(Keys.Up))
            {
                if (!pressStartIsFlashing && titleOptionsAreShowing && !choosePlayMode)
                {
                    titleOptionHoverSelection--;
                }
                else if (!pressStartIsFlashing && !titleOptionsAreShowing && choosePlayMode) {
                    playModeHoverSelected--;
                }
            }

            //Selector moving down
            if (previousKeyboardState.IsKeyUp(Keys.Down) && currentKeyboardState.IsKeyDown(Keys.Down))
            {
                if (!pressStartIsFlashing && titleOptionsAreShowing && !choosePlayMode)
                {
                    titleOptionHoverSelection++;
                }
                else if (!pressStartIsFlashing && !titleOptionsAreShowing && choosePlayMode) {
                    playModeHoverSelected++;
                }
            }
            
            //THIS HANDLES THE ROLLOVER FOR THE TITLE SELECTION!
            if (titleOptionHoverSelection < 0) {
                titleOptionHoverSelection = maxTitleOptions;
            }
            else if (titleOptionHoverSelection > maxTitleOptions) {
                titleOptionHoverSelection = 0;
            }

            //HANDLES THE ONLINE/OFFLINE PLAYMODE ROLLOVER
            if (playModeHoverSelected < 0)
            {
                playModeHoverSelected = playModeSelectionMax;
            }
            else if (playModeHoverSelected > playModeSelectionMax)
            {
                playModeHoverSelected = 0;
            }
        }
    }
}
