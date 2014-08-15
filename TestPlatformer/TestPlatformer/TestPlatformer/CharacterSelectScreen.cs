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
    class CharacterSelectScreen : Screen
    {
        //This boolean will only be triggered if a device's back button is pressed and no players have joined
        public bool goBackToTitleScreen;

        //everytime a playerOne joins increment this, every time a playerOne quits decrement
        public int currentPlayersJoined;
        //tracks maximum players allowed, will help control different play modes
        public int maxPlayersAllowed;

        //delays input from gamepads and keyboards so it doesn't go straight to the next screen
        public int selectionDelayTimer;
        public int selectionDelayMax;

        //these are the underlay positions, they're used to create and track the expected draw location of the characters
        public Rectangle characterOneUnderlay;
        public Rectangle characterTwoUnderlay;
        public Rectangle characterThreeUnderlay;
        public Rectangle characterFourUnderlay;

        //this is an independant selector that's used to merge the playerOne selection and choice so that their playerOne is controlled correctly (very important)
        public CharacterSelector playerOneSelector;
        public CharacterSelector playerTwoSelector;
        public CharacterSelector playerThreeSelector;
        public CharacterSelector playerFourSelector;

        //Keyboard State
        public KeyboardState previousKeyboardState;
        public KeyboardState currentKeyboardState;

        //playerOne One GamePad Detection
        public GamePadState playerOnePreviousGamePadState;
        public GamePadState playerOneCurrentGamePadState;

        //playerOne Two GamePad Detection
        public GamePadState playerTwoPreviousGamePadState;
        public GamePadState playerTwoCurrentGamePadState;

        //playerOne Three GamePad Detection
        public GamePadState playerThreePreviousGamePadState;
        public GamePadState playerThreeCurrentGamePadState;

        //playerOne Four GamePad Detection
        public GamePadState playerFourPreviousGamePadState;
        public GamePadState playerFourCurrentGamePadState;

        //tracks the game mode, lets you know what to infer based on that
        public GameMode gameMode;

        //default players
        public Player playerOne;
        public Player playerTwo;
        public Player playerThree;
        public Player playerFour;

        //general game fonst
        SpriteFont gameFont;

        public ImageLibrary imageLibrary;

        //IMAGE STUFF FOR DRAWING THE CHARACTER SELECTION STUFF
        AnimatedImage redTile;
        AnimatedImage orangeTile;
        AnimatedImage yellowTile;
        AnimatedImage greenTile;
        //-----------------------------------------------------

        public CharacterSelectScreen(ContentManager cManager, GraphicsDeviceManager gdmReference, SpriteBatch sbReference, ImageLibrary iLibrary, GameMode gMode)
            : base(cManager, gdmReference, sbReference)
        {
            gameFont = cManager.Load<SpriteFont>("Fonts/PlaceholderFont");
            imageLibrary = iLibrary;

            playerOne = null;
            playerTwo = null;
            playerThree = null;
            playerFour = null;

            gameMode = gMode;
            if (gameMode == GameMode.SinglePlayer) {
                maxPlayersAllowed = 1;
            }
            else if (gameMode == GameMode.CoopPlayOnline || gameMode == GameMode.CoopPlayOffline) {
                maxPlayersAllowed = 2;
            }
            else if (gameMode == GameMode.CompetitivePlayOnline || gameMode == GameMode.CompetitivePlayOffline)
            {
                maxPlayersAllowed = 4;
            }

            //players joined tracker, and the boolean that lets us know when it's time to go back
            currentPlayersJoined = 0;
            goBackToTitleScreen = false;

            //delay time
            selectionDelayTimer = 0;
            selectionDelayMax = 15;

            redTile = imageLibrary.basicRedTile;
            orangeTile = imageLibrary.basicOrangeTile;
            yellowTile = imageLibrary.basicYellowTile;
            greenTile = imageLibrary.basicGreenTile;

            //sets the expected rectangle that's supposed to represent the underlay
            characterOneUnderlay = new Rectangle(0, 200, 560, 192);
            characterTwoUnderlay = new Rectangle(gdmReference.GraphicsDevice.Viewport.Width - 560, 200, 560, 192);
            characterThreeUnderlay = new Rectangle(0, 450, 560, 192);
            characterFourUnderlay = new Rectangle(gdmReference.GraphicsDevice.Viewport.Width - 560, 450, 560, 192);

            //will be deprecated but will be initialized as such
            playerTwoSelector = new CharacterSelector(cmReference, gdmReference, sbReference, imageLibrary, new Vector2(characterTwoUnderlay.X, characterTwoUnderlay.Y), 1, 0);
            playerThreeSelector = new CharacterSelector(cmReference, gdmReference, sbReference, imageLibrary, new Vector2(characterThreeUnderlay.X, characterThreeUnderlay.Y), 0, 1);
            playerFourSelector = new CharacterSelector(cmReference, gdmReference, sbReference, imageLibrary, new Vector2(characterFourUnderlay.X, characterFourUnderlay.Y), 1, 1);
        }
        public override void updateScreen(GameTime gameTimer)
        {
            //if the delay timer was maxed then perform the runtime
            if (selectionDelayTimer > selectionDelayMax)
            {
                //accounts for the single playerOne mode
                if (gameMode == GameMode.SinglePlayer)
                {
                    checkForKeyboardUserInput();
                    checkForGamePadInput();

                    if (playerOne != null)
                    {
                        if (!playerOneSelector.selectorLocked)
                        {
                            if (playerOne.controllerBeingUsed == 0)
                            {
                                playerOneSelector.updateSelectorViaKeyboard(previousKeyboardState, currentKeyboardState);
                            }
                            else if (playerOne.controllerBeingUsed == 1)
                            {
                                playerOneSelector.updateSelectorViaGamePad(playerOnePreviousGamePadState, playerOneCurrentGamePadState);
                            }
                        }
                        //System.Diagnostics.Debug.WriteLine("PlayerOne Joined");
                    }
                    else
                    {
                        //System.Diagnostics.Debug.WriteLine("PlayerOne Not Joined");
                    }
                }
            }
            //if delay timer is slow then just increment
            else {
                selectionDelayTimer++;
            }
        }

        public override void draw(GameTime gameTimer)
        {
            if (gameMode == GameMode.SinglePlayer)
            {
                sbReference.Begin();

                //-------------------------playerOne UNDERLAY BEGINS HERE-------------------------
                //playerOne one underlay
                redTile.DrawAltPosition(new Vector2(characterOneUnderlay.X, characterOneUnderlay.Y), false, new Vector2(characterOneUnderlay.Width / 16, characterOneUnderlay.Height / 16), 1f);
                imageLibrary.characterOneSpriteSheet.setFrameConfiguration(0, 0, 0);
                imageLibrary.characterOneSpriteSheet.DrawAltPosition(new Vector2(characterOneUnderlay.X + characterOneUnderlay.Width - imageLibrary.characterOneSpriteSheet.frameSize.X-50, characterOneUnderlay.Y+40), false, 0f);
                //playerOne two underlay
                orangeTile.DrawAltPosition(new Vector2(characterTwoUnderlay.X, characterTwoUnderlay.Y), false, new Vector2(characterTwoUnderlay.Width / 16, characterTwoUnderlay.Height / 16), 1f);
                imageLibrary.characterTwoSpriteSheet.setFrameConfiguration(0, 0, 0);
                imageLibrary.characterTwoSpriteSheet.DrawAltPosition(new Vector2(characterTwoUnderlay.X + 50, characterOneUnderlay.Y + 40), true, 0f);
                //playerOne three underlay
                yellowTile.DrawAltPosition(new Vector2(characterThreeUnderlay.X, characterThreeUnderlay.Y), false, new Vector2(characterThreeUnderlay.Width / 16, characterThreeUnderlay.Height / 16), 1f);
                //playerOne four underlay
                greenTile.DrawAltPosition(new Vector2(characterFourUnderlay.X, characterFourUnderlay.Y), false, new Vector2(characterFourUnderlay.Width / 16, characterFourUnderlay.Height / 16), 1f);
                //-------------------------playerOne UNDERLAY ENDS HERE-------------------------

                if (playerOne != null)
                {
                    if (playerOneSelector.hoverHorizontalSelection == 0 && playerOneSelector.hoverVerticalSelection == 0) {
                        playerOneSelector.drawAltPosition(new Vector2(characterOneUnderlay.X, characterOneUnderlay.Y));
                    }
                    else if (playerOneSelector.hoverHorizontalSelection == 1 && playerOneSelector.hoverVerticalSelection == 0) {
                        playerOneSelector.drawAltPosition(new Vector2(characterTwoUnderlay.X, characterTwoUnderlay.Y));
                    }
                    else if (playerOneSelector.hoverHorizontalSelection == 0 && playerOneSelector.hoverVerticalSelection == 1) {
                        playerOneSelector.drawAltPosition(new Vector2(characterThreeUnderlay.X, characterThreeUnderlay.Y));
                    }
                    else if (playerOneSelector.hoverHorizontalSelection == 1 && playerOneSelector.hoverVerticalSelection == 1) {
                        playerOneSelector.drawAltPosition(new Vector2(characterFourUnderlay.X, characterFourUnderlay.Y));
                    }
                }

                sbReference.DrawString(gameFont, "Character Selection!", new Vector2(gdmReference.GraphicsDevice.Viewport.TitleSafeArea.Width / 2 - 290, 20), Color.White, 0.0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
                sbReference.DrawString(gameFont, "Press Start To Join", new Vector2(gdmReference.GraphicsDevice.Viewport.TitleSafeArea.Width / 2 - 130, 100), Color.White, 0.0f, new Vector2(0, 0), .5f, SpriteEffects.None, 0f);
                sbReference.End();
            }
        }

        public void checkForKeyboardUserInput()
        {
            previousKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();

            //THIS IS FOR THE BACK BUTTON TO SHOW THE PRESS START MENU AGAIN BECAUSE YOU KNOW WHAT THAT'S HOW I WANTED IT, DEAL WITH IT
            if (gameMode == GameMode.SinglePlayer)
            {
                if (previousKeyboardState.IsKeyUp(Keys.RightShift) && currentKeyboardState.IsKeyDown(Keys.RightShift))
                {
                    if (currentPlayersJoined == 0)
                    {
                        goBackToTitleScreen = true;
                    }
                    else if (currentPlayersJoined >= 1)
                    {
                        if (playerOne != null)
                        {
                            if (playerOneSelector.selectorLocked)
                            {
                                playerOneSelector.selectorLocked = false;
                                selectionDelayTimer = 0;
                            }
                            else
                            {
                                playerOne = null;
                                currentPlayersJoined--;
                            }
                        }
                    }
                }

                //THIS CHECKS THE CONFIRMATION INPUT FROM THE KEYBOARD
                if (previousKeyboardState.IsKeyUp(Keys.Enter) && currentKeyboardState.IsKeyDown(Keys.Enter))
                {
                    //System.Diagnostics.Debug.WriteLine("Enter Pressed");
                    if (playerOne == null)
                    {
                        playerOne = new Player(cmReference, sbReference, gdmReference, imageLibrary, 0);
                        playerOneSelector = new CharacterSelector(cmReference, gdmReference, sbReference, imageLibrary, new Vector2(characterOneUnderlay.X, characterOneUnderlay.Y), 0, 0);
                        currentPlayersJoined++;
                    }
                    else
                    {
                        //do nothing the first spot is taken
                    }
                }
            }
        }
        
        //handles gamepad input
        public void checkForGamePadInput() {
            //starts to check what mode it is in and bases decisions on that
            if (gameMode == GameMode.SinglePlayer)
            {
                //Grabs all playerOne controller states
                if (GamePad.GetState(PlayerIndex.One).IsConnected)
                {
                    playerOnePreviousGamePadState = playerOneCurrentGamePadState;
                    playerOneCurrentGamePadState = GamePad.GetState(PlayerIndex.One);

                    if (playerOnePreviousGamePadState.IsButtonUp(Buttons.B) && playerOneCurrentGamePadState.IsButtonDown(Buttons.B))
                    {
                        if (currentPlayersJoined == 0)
                        {
                            goBackToTitleScreen = true;
                        }
                        else if (currentPlayersJoined >= 1)
                        {
                            if (playerOne != null)
                            {
                                if (playerOneSelector.selectorLocked)
                                {
                                    playerOneSelector.selectorLocked = false;
                                    selectionDelayTimer = 0;
                                }
                                else
                                {
                                    playerOne = null;
                                    currentPlayersJoined--;
                                }
                            }
                        }
                    }

                    //THIS CHECKS THE CONFIRMATION INPUT FROM THE KEYBOARD
                    if (playerOnePreviousGamePadState.IsButtonUp(Buttons.A) && playerOneCurrentGamePadState.IsButtonDown(Buttons.A))
                    {
                        //System.Diagnostics.Debug.WriteLine("Enter Pressed");
                        if (playerOne == null)
                        {
                            playerOne = new Player(cmReference, sbReference, gdmReference, imageLibrary, 1);
                            playerOneSelector = new CharacterSelector(cmReference, gdmReference, sbReference, imageLibrary, new Vector2(characterOneUnderlay.X, characterOneUnderlay.Y), 0, 0);
                            currentPlayersJoined++;
                        }
                        else
                        {
                            //do nothing the first spot is taken
                        }
                    }
                }

                if (GamePad.GetState(PlayerIndex.Two).IsConnected)
                {
                    playerTwoPreviousGamePadState = playerTwoCurrentGamePadState;
                    playerTwoCurrentGamePadState = GamePad.GetState(PlayerIndex.Two);
                }

                if (GamePad.GetState(PlayerIndex.Three).IsConnected)
                {
                    playerThreePreviousGamePadState = playerThreeCurrentGamePadState;
                    playerThreeCurrentGamePadState = GamePad.GetState(PlayerIndex.Three);
                }

                if (GamePad.GetState(PlayerIndex.Four).IsConnected)
                {
                    playerFourPreviousGamePadState = playerFourCurrentGamePadState;
                    playerFourCurrentGamePadState = GamePad.GetState(PlayerIndex.Four);
                }
            }
        }
    }
}
