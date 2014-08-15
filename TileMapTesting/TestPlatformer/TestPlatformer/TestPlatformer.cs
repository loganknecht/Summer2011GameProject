using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace TestPlatformer
{
    public class TestPlatformer : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        ScreenManager screenManager;

        TestScreen tScreen;
        RegalFalls RegalFalls;

        KeyboardState previousKeyboardState;
        KeyboardState currentKeyboardState;

        int splashScreenTimer;
        int splashScreenDuration;

        //tracks the game's flow and progression. Used to move the game state through desired screens
        enum possibleGameStates { SplashScreen, Cinematic, TitleScreen, Options, GameStarted, GamePlaying, PausedGame, QuitGame };
        possibleGameStates currentGameState = possibleGameStates.SplashScreen;

        //makes the window a default of 1280*720, sets content config, then creates the screen manager
        public TestPlatformer() {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            Content.RootDirectory = "Content";

            screenManager = new ScreenManager();

            //creates the splash screen timer and sets the duration timer for the splash screen
            splashScreenTimer = 0;
            splashScreenDuration = 120;
        }

        protected override void Initialize() {
            base.Initialize();
        }

        protected override void LoadContent() {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            tScreen = new TestScreen(Content, graphics, spriteBatch, Color.Orange);
            RegalFalls = new RegalFalls(Content, graphics, spriteBatch, Color.CornflowerBlue);
            screenManager.addScreen(new SplashScreen(Content, graphics, spriteBatch));
        } 

        protected override void UnloadContent() {
        }

        protected override void Update(GameTime gameTime) {
            previousKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            if (currentKeyboardState.IsKeyDown(Keys.Escape) && previousKeyboardState.IsKeyUp(Keys.Escape) && currentGameState != possibleGameStates.PausedGame)
            {
                this.Exit();
            }

            switch(currentGameState) {
                case(possibleGameStates.SplashScreen):
                    splashScreenTimer++;
                    if (splashScreenTimer > splashScreenDuration) {
                        screenManager.removeScreen();
                        screenManager.addScreen(new TitleScreen(Content, graphics, spriteBatch));
                        currentGameState = possibleGameStates.TitleScreen;
                    }
                    break;
                case(possibleGameStates.Cinematic):
                    break;
                case(possibleGameStates.TitleScreen):
                    if(screenManager.getCurrentScreen is TitleScreen){
                        TitleScreen tempTitleScreen = (TitleScreen)screenManager.getCurrentScreen;
                        //System.Diagnostics.Debug.WriteLine(tempTitleScreen.getTitleOptionSelected);
                        if (tempTitleScreen.getTitleOptionSelected == 0) { 
                            //do nothing no choice selected
                        }
                        else if (tempTitleScreen.getTitleOptionSelected == 1) {
                            currentGameState = possibleGameStates.GamePlaying;
                            screenManager.removeScreen();
                            screenManager.addScreen(new RegalFalls(Content, graphics, spriteBatch, Color.CornflowerBlue) );
                        }
                        else if (tempTitleScreen.getTitleOptionSelected == 2) { 
                            //do nothing
                        }
                    }
                    break;
                case(possibleGameStates.GameStarted):
                    break;
                case (possibleGameStates.GamePlaying):   
                    if (previousKeyboardState.IsKeyUp(Keys.Enter) && currentKeyboardState.IsKeyDown(Keys.Enter))
                    {
                        currentGameState = possibleGameStates.PausedGame;
                        Screen tempPreviousScreen  = screenManager.getCurrentScreen;
                        screenManager.addScreen(new PauseScreen(Content, graphics, spriteBatch, tempPreviousScreen));
                    }
                    break;
                case(possibleGameStates.PausedGame):
                    if (previousKeyboardState.IsKeyUp(Keys.Escape) && currentKeyboardState.IsKeyDown(Keys.Escape))
                    {
                        currentGameState = possibleGameStates.GamePlaying;
                        screenManager.removeScreen();
                    }

                    if (screenManager.getCurrentScreen is PauseScreen)
                    {
                        if (previousKeyboardState.IsKeyUp(Keys.Enter) && currentKeyboardState.IsKeyDown(Keys.Enter))
                        {
                            PauseScreen tempPauseScreen = (PauseScreen)screenManager.getCurrentScreen;

                            //called so that the pause screen returns the correct selected menu item otherwise the event is delayed detected and needs to be pressed twice
                            if (tempPauseScreen.getHoveringSelectedMenuItem == 0)
                            {
                                //do nothing no choice selected
                            }
                            else if (tempPauseScreen.getHoveringSelectedMenuItem == 1)
                            {
                                currentGameState = possibleGameStates.GamePlaying;
                                screenManager.removeScreen();
                            }
                            else if (tempPauseScreen.getHoveringSelectedMenuItem == 2)
                            {
                                currentGameState = possibleGameStates.TitleScreen;
                                screenManager.removeScreen();
                                screenManager.addScreen(new TitleScreen(Content, graphics, spriteBatch));
                            }
                        }
                    }

                    break;
            }
            screenManager.update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            //GraphicsDevice.Clear(Color.CornflowerBlue);
            screenManager.drawCurrentScreen(gameTime);
        }
    }
}
