using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace TestPlatformer
{
    class SinglePlayerStatusScreen : Screen
    {
        public NextLevelPointer nextLevel;

        SpriteFont titleFont;

        //tracks state in this screen
        KeyboardState previousKeyboardState;
        KeyboardState currentKeyboardState;

        public Player tempPlayer;

        public bool startNextLevel;

        public SinglePlayerStatusScreen(ContentManager cReference, GraphicsDeviceManager gdmReference, SpriteBatch sbReference, Player player, NextLevelPointer nextLev)
            : base(cReference, gdmReference, sbReference)
        {
            nextLevel = nextLev;

            titleFont = cmReference.Load<SpriteFont>("Fonts/PlaceholderFont");

            tempPlayer = player;

            //this transfers the players the jewels to the cumulative tracker and resets the level jewel tracker
            tempPlayer.cumulativeRedJewelTracker += tempPlayer.redJewelLevelTracker;
            tempPlayer.redJewelLevelTracker = 0;

            tempPlayer.cumulativeGreenJewelTracker += tempPlayer.greenJewelLevelTracker;
            tempPlayer.greenJewelLevelTracker = 0;

            tempPlayer.cumulativeBlueJewelTracker += tempPlayer.blueJewelLevelTracker;
            tempPlayer.blueJewelLevelTracker = 0;

            //set the player's animation to the standing position and resets any weird moving factors
            tempPlayer.playerImageSheet.setFrameConfiguration(0, 0, 4);
            tempPlayer.playerImageSheet.frameTimeLimit = tempPlayer.defaultFrameTimeLimit;
            tempPlayer.isDashing = false;
            tempPlayer.exhaustedDash = false;
            tempPlayer.momentum.X = 0;
            tempPlayer.momentum.Y = 0;

            startNextLevel = false;
        }

        public override void updateScreen(GameTime gameTimer)
        {
            checkForUserInput();
        }

        public override void draw(GameTime gameTimer)
        {
            Vector2 scoreDrawModifier = titleFont.MeasureString("" + tempPlayer.cumulativeScore);

            sbReference.Begin();
            sbReference.DrawString(titleFont, "Player Status Screen", new Vector2(gdmReference.GraphicsDevice.Viewport.Width / 2 - 300, 0), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);
            
            sbReference.DrawString(titleFont, "Player One Score", new Vector2(270, 250), Color.White, 0f, Vector2.Zero, .5f, SpriteEffects.None, 0);
            sbReference.DrawString(titleFont, "" + tempPlayer.cumulativeScore, new Vector2(500 - (scoreDrawModifier.X*.5f), 300), Color.White, 0f, Vector2.Zero, .5f, SpriteEffects.None, 0);
            
            sbReference.DrawString(titleFont, "Next Level", new Vector2(850, 250), Color.White, 0f, Vector2.Zero, .5f, SpriteEffects.None, 0);
            if (nextLevel == NextLevelPointer.WorldOneLevelOne) {
                sbReference.DrawString(titleFont, "World One Level One", new Vector2(780, 400), Color.White, 0f, Vector2.Zero, .5f, SpriteEffects.None, 0);
            }
            else if (nextLevel == NextLevelPointer.WorldOneLevelTwo) {
                sbReference.DrawString(titleFont, "World One Level Two", new Vector2(780, 400), Color.White, 0f, Vector2.Zero, .5f, SpriteEffects.None, 0);
            }
            else if (nextLevel == NextLevelPointer.WorldOneLevelThree)
            {
                sbReference.DrawString(titleFont, "World One Level Three", new Vector2(780, 400), Color.White, 0f, Vector2.Zero, .5f, SpriteEffects.None, 0);
            }
            else if (nextLevel == NextLevelPointer.Credits)
            {
                sbReference.DrawString(titleFont, "Credits", new Vector2(780, 400), Color.White, 0f, Vector2.Zero, .5f, SpriteEffects.None, 0);
            }

            //red jewels
            sbReference.DrawString(titleFont, "Total Red \n  Jewels", new Vector2(270, 480), Color.White, 0f, Vector2.Zero, .2f, SpriteEffects.None, 0);
            sbReference.DrawString(titleFont, ""+tempPlayer.cumulativeRedJewelTracker, new Vector2(330 - (scoreDrawModifier.X * .3f), 535), Color.White, 0f, Vector2.Zero, .3f, SpriteEffects.None, 0);

            //green jewels
            sbReference.DrawString(titleFont, "Total Green \n  Jewels", new Vector2(350, 480), Color.White, 0f, Vector2.Zero, .2f, SpriteEffects.None, 0);
            sbReference.DrawString(titleFont, "" + tempPlayer.cumulativeGreenJewelTracker, new Vector2(420 - (scoreDrawModifier.X * .3f), 535), Color.White, 0f, Vector2.Zero, .3f, SpriteEffects.None, 0);

            //blue jewels
            sbReference.DrawString(titleFont, "Total Blue \n  Jewels", new Vector2(430, 480), Color.White, 0f, Vector2.Zero, .2f, SpriteEffects.None, 0);
            sbReference.DrawString(titleFont, "" + tempPlayer.cumulativeBlueJewelTracker, new Vector2(500 - (scoreDrawModifier.X * .3f), 535), Color.White, 0f, Vector2.Zero, .3f, SpriteEffects.None, 0);

            sbReference.DrawString(titleFont, "Press Start to go to the Next Level", new Vector2(380, (float)(gdmReference.GraphicsDevice.Viewport.Height*.9)), Color.White, 0f, Vector2.Zero, .5f, SpriteEffects.None, 0);
            tempPlayer.playerImageSheet.DrawAltPosition(new Vector2(gdmReference.GraphicsDevice.Viewport.Width / 2 - 300, gdmReference.GraphicsDevice.Viewport.Height/2), false, 0);
            sbReference.End();
        }

        public void checkForUserInput()
        {
            previousKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();

            if (previousKeyboardState.IsKeyUp(Keys.Enter) && currentKeyboardState.IsKeyDown(Keys.Enter)) {
                startNextLevel = true;
            }
        }
    }
}
