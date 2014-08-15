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
    class Player
    {
        public AnimatedImage playerImageSheet;
        public int defaultFrameTimeLimit;

        public SpriteBatch sbReference;
        public ContentManager cmReference;
        public GraphicsDeviceManager gdmReference;

        public Texture2D boundingRectangleColor;

        public Vector2 previousWorldPosition;
        public Vector2 currentWorldPosition;
        public Vector2 velocity;
        public Vector2 momentum;

        public int characterSelected;
        public int cumulativeScore;

        public int redJewelLevelTracker;
        public int cumulativeRedJewelTracker;
        
        public int greenJewelLevelTracker;
        public int cumulativeGreenJewelTracker;
        
        public int blueJewelLevelTracker;
        public int cumulativeBlueJewelTracker;

        //tracks the movement in each direction
        public bool movingLeft;
        public bool movingRight;
        public bool movingUp;
        public bool movingDown;

        //default no key press state
        public bool standing;

        //movement states the player can have, these will overide the standing flag and set it to false so that the appropriate animation is played
        public bool isJumping;
        public bool jumpExhausted;
        public bool wallSlideJump;
        public bool isFalling;
        public bool isFallingFromGravity;
        public bool isDashing;
        public bool exhaustedDash;
        
        public bool wallSliding;
        public Platform wallSlideReference;

        //hurdle action state
        public Hurdle currentActionHurdleReference;
        public bool actionButtonBeingPressed;
        public bool actionStateIsActive;
        public bool hurdleActionStateActive;
        public bool upTheHurdle;
        public bool overTheHurdle;
        public int hurdleTimer;
        public int hurdleTimerMax;
        public Vector2 hurdleDistance;

        //swing rope action state
        public bool swingRopeActionStateActive;
        public bool ejectPlayerFromRope;

        //ladder action state
        public bool ladderActionStateActive;
        public bool holdingLadder;
        public bool climbingLadder;
        public bool slidingDownLadder;
        public Ladder ladderReference;

        public bool isGrinding;
        public GrindRail grindRailReference;

        public bool tightRopeWalkActionStateActive;
        public bool tightRopeWalkingLeft;
        public bool tightRopeWalkingRight;

        public bool tightRopeHangingActionStateActive;
        public bool tightRopeHangingLeft;
        public bool tightRopeHangingRight;
        public bool isHanging;
        public TightRope tightRopeReference;

        //tracker for jump duration
        public int jumpTimer;
        //static number that is the bounds for the jumpTimer
        public int jumpTimerMax;

        //tracks the time for dash
        public int dashTimer;
        //tracks the max time allowed for dashing
        public int dashTimerMax;

        public int controllerBeingUsed;

        //accounts for players facing
        public bool facingLeft;
        public bool facingRight;

        public bool playerCollisionOccurred;
        public bool collisionOnPlayerLeft;
        public bool collisionOnPlayerRight;
        public bool collisionOnPlayerTop;
        public bool collisionOnPlayerBottom;

        public float layerDepth;

        //death state
        public bool wasKilled;

        //gets position + width
        public float rightBounding
        {
            get
            {
                return currentWorldPosition.X + width;
            }
        }

        //gets position + height
        public float bottomBounding
        {
            get
            {
                return currentWorldPosition.Y + height;
            }
        }

        public int width
        {
            get
            {
                return (int)playerImageSheet.frameSize.X;
            }
        }

        public int height
        {
            get
            {
                return (int)playerImageSheet.frameSize.Y;
            }
        }


        /// 
        /// Uses the default rectangle provided by the animated image class because the expectation is that the dimations
        /// of the split image will be symetric for all frames
        /// 
        public Rectangle boundingRectangle
        {
            get
            {
                return new Rectangle((int)currentWorldPosition.X, (int)currentWorldPosition.Y, playerImageSheet.imageBounding.Width, playerImageSheet.imageBounding.Height);
            }
        }

        public Player(ContentManager cm, SpriteBatch sb, GraphicsDeviceManager gdm, ImageLibrary il, int pIndex)
        {
            //configures the image sheet associated with the player
            playerImageSheet = il.characterOneSpriteSheet;
            characterSelected = 1;
            cumulativeScore = 0;
            redJewelLevelTracker = 0;
            cumulativeRedJewelTracker = 0;
            greenJewelLevelTracker = 0;
            cumulativeGreenJewelTracker = 0;
            blueJewelLevelTracker = 0;
            cumulativeBlueJewelTracker = 0;

            defaultFrameTimeLimit = 4;
            playerImageSheet.frameTimeLimit = defaultFrameTimeLimit;
            playerImageSheet.setFrameConfiguration(0, 0, 1);
            //playerImageSheet.isAnimating = false;

            boundingRectangleColor = cm.Load<Texture2D>("CollisionColor");

            //sets the references to the spritebatch and content manager so this class can define items in the content pipeline and draw to the same area
            sbReference = sb;
            cmReference = cm;
            gdmReference = gdm;

            //environment variables
            previousWorldPosition = Vector2.Zero;
            currentWorldPosition = Vector2.Zero;
            velocity = new Vector2(12, 25);
            momentum = new Vector2(0,0);

            movingLeft = false;
            movingRight = false;
            movingUp = false;
            movingDown = false;

            standing = true;

            isJumping = false;
            jumpExhausted = false;
            isFalling = false;
            isFallingFromGravity = false;
            isDashing = false;
            exhaustedDash = false;
            wallSliding = false;
            wallSlideJump = false;

            jumpTimer = 0;
            jumpTimerMax = 20;

            dashTimer = 0;
            dashTimerMax = 10;

            currentActionHurdleReference = null;
            actionButtonBeingPressed = false;
            actionStateIsActive = false;
            hurdleActionStateActive = false;
            upTheHurdle = false;
            overTheHurdle = false;
            hurdleTimer = 0;
            hurdleTimerMax = 10;
            hurdleDistance = Vector2.Zero;

            swingRopeActionStateActive = false;
            ejectPlayerFromRope = false;

            ladderActionStateActive = false;
            holdingLadder = false;
            climbingLadder = false;
            slidingDownLadder = false;
            ladderReference = null;

            isGrinding = false;
            grindRailReference = null;

            tightRopeWalkActionStateActive = false;
            tightRopeWalkingLeft = false;
            tightRopeWalkingRight = false;
            tightRopeHangingActionStateActive = false;
            tightRopeHangingLeft = false;
            tightRopeHangingRight = false;
            isHanging = false;
            tightRopeReference = null;

            facingLeft = false;
            facingRight = true;

            playerCollisionOccurred = false;
            collisionOnPlayerLeft = false;
            collisionOnPlayerRight = false;
            collisionOnPlayerTop = false;
            collisionOnPlayerBottom = false;

            wasKilled = false;

            layerDepth = .5f;
            //this sets the playerIndex if applicable, null indicates that it is controled by the keyboard and not a gamepad
            controllerBeingUsed = pIndex;

        }

        public void Initialize()
        {
        }

        //draws at different position
        public void DrawAltPosition(Vector2 position)
        {
            if (facingRight)
            {
                playerImageSheet.DrawAltPosition(position, false, layerDepth);
            }
            else if (facingLeft)
            {
                playerImageSheet.DrawAltPosition(position, true, layerDepth);
            }
        }

        public void drawBoundingBox(Vector2 position)
        {
            //top
            sbReference.Draw(boundingRectangleColor, new Rectangle((int)position.X, (int)position.Y, boundingRectangle.Width, 1), Color.White);
            //left
            sbReference.Draw(boundingRectangleColor, new Rectangle((int)position.X, (int)position.Y, 1, boundingRectangle.Height), Color.White);
            //right
            sbReference.Draw(boundingRectangleColor, new Rectangle((int)position.X + boundingRectangle.Width - 1, (int)position.Y, 1, boundingRectangle.Height), Color.White);
            //bottom
            sbReference.Draw(boundingRectangleColor, new Rectangle((int)position.X, (int)position.Y + boundingRectangle.Height - 1, boundingRectangle.Width, 1), Color.White);
        }

        //remember replace gravity with environment class eventually
        public void Update(GameTime gameTime)
        {
            playerImageSheet.Update(gameTime);
        }

        public void changeCMReference(ContentManager cm)
        {
            cmReference = cm;
        }

        public void changeSBReference(SpriteBatch sb)
        {
            sbReference = sb;
            playerImageSheet.spriteBatchReference = sb;
        }

        public void changeGDMReference(GraphicsDeviceManager newGDM)
        {
            gdmReference = newGDM;
        }

        public void checkAndFixWindowCollision()
        {
            if (currentWorldPosition.X < gdmReference.GraphicsDevice.Viewport.TitleSafeArea.Left)
            {
                currentWorldPosition.X = gdmReference.GraphicsDevice.Viewport.TitleSafeArea.Left;
            }
            if (currentWorldPosition.Y < gdmReference.GraphicsDevice.Viewport.TitleSafeArea.Top)
            {
                currentWorldPosition.Y = gdmReference.GraphicsDevice.Viewport.TitleSafeArea.Top;
            }
            if (rightBounding > gdmReference.GraphicsDevice.Viewport.TitleSafeArea.Right)
            {
                currentWorldPosition.X = gdmReference.GraphicsDevice.Viewport.TitleSafeArea.Right - width;
            }
            if (bottomBounding > gdmReference.GraphicsDevice.Viewport.TitleSafeArea.Bottom)
            {
                currentWorldPosition.Y = gdmReference.GraphicsDevice.Viewport.TitleSafeArea.Bottom - height;
                //this is set to false so that when a character lands they can jump again
                //this needs to occur in every bottom bounded collision
                if (isFalling)
                {
                    isFalling = false;
                    if (movingRight || movingLeft)
                    {
                        playerImageSheet.setFrameConfiguration(18, 18, 28);
                        playerImageSheet.frameTimeLimit = defaultFrameTimeLimit;
                    }
                }
                if (isFallingFromGravity)
                {
                    isFallingFromGravity = false;
                    if (movingRight || movingLeft)
                    {
                        playerImageSheet.setFrameConfiguration(18, 18, 28);
                        playerImageSheet.frameTimeLimit = defaultFrameTimeLimit;
                    }
                    else if (standing)
                    {
                        playerImageSheet.setFrameConfiguration(0, 0, 4);
                        playerImageSheet.frameTimeLimit = defaultFrameTimeLimit;
                    }
                }
                if (exhaustedDash)
                {
                    exhaustedDash = false;
                }
            }
        }

        public void checkForPlayerKeyboardInput(KeyboardState previousKBState, KeyboardState currentKBState)
        {
            if(!wasKilled) {
                if (actionStateIsActive)
                {
                    if (previousKBState.IsKeyUp(Keys.N) && currentKBState.IsKeyDown(Keys.N))
                    {
                        actionButtonBeingPressed = true;
                    }
                    if (previousKBState.IsKeyDown(Keys.N) && currentKBState.IsKeyDown(Keys.N))
                    {
                        actionButtonBeingPressed = true;
                    }
                    if (previousKBState.IsKeyDown(Keys.N) && currentKBState.IsKeyUp(Keys.N))
                    {
                        if (!ladderActionStateActive)
                        {
                            actionButtonBeingPressed = false;
                            actionStateIsActive = false;
                            if (hurdleActionStateActive)
                            {
                                hurdleActionStateActive = false;
                            }
                            if (swingRopeActionStateActive)
                            {
                                swingRopeActionStateActive = false;
                                ejectPlayerFromRope = true;
                            }
                        }
                    }
                    //swing rope action state
                    if (swingRopeActionStateActive)
                    {
                        if (previousKBState.IsKeyUp(Keys.A) && currentKBState.IsKeyDown(Keys.A))
                        {
                            facingLeft = true;
                            facingRight = false;
                            movingLeft = true;
                            movingRight = false;

                            //sets to the appropriate swing animation
                            playerImageSheet.setFrameConfiguration(18, 18, 25);
                            playerImageSheet.frameTimeLimit = defaultFrameTimeLimit;
                        }
                        if (previousKBState.IsKeyDown(Keys.A) && currentKBState.IsKeyUp(Keys.A))
                        {
                            movingLeft = false;
                            if (currentKBState.IsKeyUp(Keys.D))
                            {
                                //sets to the isHanging on the rope animation
                                playerImageSheet.setFrameConfiguration(0, 0, 4);
                                playerImageSheet.frameTimeLimit = defaultFrameTimeLimit;
                            }
                        }
                        if (previousKBState.IsKeyUp(Keys.D) && currentKBState.IsKeyDown(Keys.D))
                        {
                            facingLeft = false;
                            facingRight = true;
                            movingRight = true;
                            movingLeft = false;
                            //sets to the appropriate swing animation
                            playerImageSheet.setFrameConfiguration(18, 18, 25);
                            playerImageSheet.frameTimeLimit = defaultFrameTimeLimit;
                        }
                        if (previousKBState.IsKeyDown(Keys.D) && currentKBState.IsKeyUp(Keys.D))
                        {
                            movingRight = false;
                            if (currentKBState.IsKeyUp(Keys.A))
                            {
                                //sets to the isHanging on the rope animation
                                playerImageSheet.setFrameConfiguration(0, 0, 4);
                                playerImageSheet.frameTimeLimit = defaultFrameTimeLimit;
                            }
                        }
                        //checks if dash was pressed
                        if (previousKBState.IsKeyUp(Keys.B) &&
                            currentKBState.IsKeyDown(Keys.B))
                        {
                            if (exhaustedDash == false)
                            {
                                isDashing = true;
                                dashTimer = 0;
                                playerImageSheet.setFrameConfiguration(54, 54, 54);
                                playerImageSheet.frameTimeLimit = defaultFrameTimeLimit;
                            }
                        }
                        //tracks dashing modifiers, if dashing and within the duration modifies the position accordingly
                        //else it's outside duration and sets dashing to false, but that it's been exhausted, the exhaustedDash
                        //needs to be reset by collision objects otherwise it breaks
                        if (isDashing)
                        {
                            if (dashTimer < dashTimerMax &&
                                exhaustedDash == false)
                            {
                                //if you want to be able to dash without having to move change these to "facingLeft" and "facingRight"
                                dashTimer++;
                            }
                            else
                            {
                                isDashing = false;
                                if ((movingLeft || movingRight))
                                {
                                    playerImageSheet.setFrameConfiguration(18, 18, 25);
                                    playerImageSheet.frameTimeLimit = defaultFrameTimeLimit;
                                }
                            }
                        }
                        if (!movingLeft && !movingRight)
                        {
                            //would set to default hold animation
                            playerImageSheet.setFrameConfiguration(0, 0, 4);
                            playerImageSheet.frameTimeLimit = defaultFrameTimeLimit;
                        }
                        else
                        {

                        }

                    }
                    //ladder action state input
                    if (ladderActionStateActive)
                    {
                        //---------------------------- down presses here ----------------------------
                        if (previousKBState.IsKeyUp(Keys.W) && currentKBState.IsKeyDown(Keys.W))
                        {
                            slidingDownLadder = false;
                            climbingLadder = true;

                            playerImageSheet.setFrameConfiguration(91, 91, 94);
                            playerImageSheet.frameTimeLimit = defaultFrameTimeLimit;
                        }
                        if (previousKBState.IsKeyUp(Keys.S) && currentKBState.IsKeyDown(Keys.S))
                        {
                            playerImageSheet.setFrameConfiguration(92, 92, 92);
                            playerImageSheet.frameTimeLimit = defaultFrameTimeLimit;
                            slidingDownLadder = true;
                            climbingLadder = false;
                        }
                        if (previousKBState.IsKeyUp(Keys.M) && currentKBState.IsKeyDown(Keys.M))
                        {
                            playerImageSheet.setFrameConfiguration(36, 36, 42);
                            playerImageSheet.frameTimeLimit = 6;
                            playerImageSheet.isAnimating = true;

                            isJumping = true;
                            jumpTimer = jumpTimerMax / 4;
                            ladderActionStateActive = false;
                            slidingDownLadder = false;
                            climbingLadder = false;
                            holdingLadder = false;
                        }
                        if (previousKBState.IsKeyUp(Keys.A) && currentKBState.IsKeyDown(Keys.A)) {
                            movingLeft = true;
                            facingLeft = true;
                            movingRight = false;
                            facingRight = false;
                        }
                        if (previousKBState.IsKeyUp(Keys.D) && currentKBState.IsKeyDown(Keys.D))
                        {
                            movingRight = true;
                            facingRight = true;
                            movingLeft = false;
                            facingLeft = false;
                        }

                        //---------------------------- up presses here ----------------------------
                        if (previousKBState.IsKeyDown(Keys.W) && currentKBState.IsKeyUp(Keys.W))
                        {
                            climbingLadder = false;
                        }
                        if (previousKBState.IsKeyDown(Keys.S) && currentKBState.IsKeyUp(Keys.S))
                        {
                            slidingDownLadder = false;
                        }
                        if (previousKBState.IsKeyDown(Keys.A) && currentKBState.IsKeyUp(Keys.A))
                        {
                            movingLeft = false;
                        }
                        if (previousKBState.IsKeyDown(Keys.D) && currentKBState.IsKeyUp(Keys.D))
                        {
                            movingRight = false;
                        }
                        //---------------------------- state based occurences go here ----------------------------
                        if (!climbingLadder && !slidingDownLadder)
                        {
                            playerImageSheet.isAnimating = false;
                            if (previousKBState.IsKeyUp(Keys.M) && currentKBState.IsKeyDown(Keys.M))
                            {
                                playerImageSheet.isAnimating = true;
                            }
                            else
                            {
                                holdingLadder = true;
                            }
                        }
                        else
                        {
                            playerImageSheet.isAnimating = true;
                            holdingLadder = false;
                        }
                    }
                }
                else
                {
                    //checks if keys are down
                    if (previousKBState.IsKeyUp(Keys.A) && currentKBState.IsKeyDown(Keys.A))
                    {
                        if (movingLeft)
                        {
                            //do nothing
                        }
                        else
                        {
                            if (!isHanging && !isGrinding && !isJumping)
                            {
                                //fixes animations for the character's fall
                                if (isFalling)
                                {
                                    playerImageSheet.setFrameConfiguration(72, 72, 72);
                                    playerImageSheet.frameTimeLimit = defaultFrameTimeLimit;
                                }
                                else if (isFallingFromGravity)
                                {
                                    playerImageSheet.setFrameConfiguration(73, 73, 73);
                                    playerImageSheet.frameTimeLimit = defaultFrameTimeLimit;
                                }
                                else
                                {
                                    playerImageSheet.setFrameConfiguration(18, 18, 28);
                                    playerImageSheet.frameTimeLimit = defaultFrameTimeLimit;
                                }
                            }
                            else
                            {
                                if (isHanging)
                                {
                                    playerImageSheet.setFrameConfiguration(91, 91, 94);
                                    playerImageSheet.frameTimeLimit = defaultFrameTimeLimit;
                                }
                                else if (isGrinding)
                                {
                                    playerImageSheet.setFrameConfiguration(55, 55, 55);
                                    playerImageSheet.frameTimeLimit = defaultFrameTimeLimit;
                                }
                            }

                            movingLeft = true;
                            movingRight = false;
                            facingLeft = true;
                            facingRight = false;
                        }
                    }
                    if (previousKBState.IsKeyUp(Keys.D) && currentKBState.IsKeyDown(Keys.D))
                    {
                        if (movingRight)
                        {
                            //do nothing
                        }
                        else
                        {
                            if (!isHanging && !isGrinding && !isJumping)
                            {
                                //fixes animations for the character's fall
                                if (isFalling)
                                {
                                    playerImageSheet.setFrameConfiguration(72, 72, 72);
                                    playerImageSheet.frameTimeLimit = defaultFrameTimeLimit;

                                }
                                else if (isFallingFromGravity)
                                {
                                    playerImageSheet.setFrameConfiguration(73, 73, 73);
                                    playerImageSheet.frameTimeLimit = defaultFrameTimeLimit;
                                }
                                else
                                {
                                    playerImageSheet.setFrameConfiguration(18, 18, 28);
                                    playerImageSheet.frameTimeLimit = defaultFrameTimeLimit;
                                }
                            }
                            else {
                                if (isHanging)
                                {
                                    playerImageSheet.setFrameConfiguration(91, 91, 94);
                                    playerImageSheet.frameTimeLimit = defaultFrameTimeLimit;
                                }
                                else if (isGrinding)
                                {
                                    playerImageSheet.setFrameConfiguration(55, 55, 55);
                                    playerImageSheet.frameTimeLimit = defaultFrameTimeLimit;
                                }
                            }
                            movingLeft = false;
                            movingRight = true;
                            facingRight = true;
                            facingLeft = false;
                        }
                    }

                    //--------------------------- checks for up and down key presses ------------------------------------
                    if (previousKBState.IsKeyUp(Keys.W) && currentKBState.IsKeyDown(Keys.W))
                    {
                        movingUp = true;
                    }
                    if (previousKBState.IsKeyUp(Keys.S) && currentKBState.IsKeyDown(Keys.S))
                    {
                        movingDown = true;
                    }
                    //---------------------------------------------------------------------------------------------------

                    //-------------checks if dash was pressed----------------
                    if (previousKBState.IsKeyUp(Keys.B) &&
                        currentKBState.IsKeyDown(Keys.B) && !wallSliding)
                    {
                        if (exhaustedDash == false && !isHanging && !isGrinding)
                        {
                            isDashing = true;
                            dashTimer = 0;
                            playerImageSheet.setFrameConfiguration(54, 54, 54);
                            playerImageSheet.frameTimeLimit = defaultFrameTimeLimit;
                        }
                    }

                    //checks if dash was released
                    if (previousKBState.IsKeyDown(Keys.B) && currentKBState.IsKeyUp(Keys.B) && !isGrinding)
                    {
                        //if you are jumping or falling or falling from gravity the exhausted dash is triggered
                        //if you aren't jumping/falling/falling from gravity it's safe to assume you're probably moving or standing on 
                        //surface, because of this the exhausted dash won't be flagged and you can dash again
                        if (isJumping || isFalling || isFallingFromGravity)
                        {
                            if (isDashing)
                            {
                                isDashing = false;
                                exhaustedDash = true;
                            }

                            if (isFalling)
                            {
                                playerImageSheet.setFrameConfiguration(72, 72, 72);
                                playerImageSheet.frameTimeLimit = defaultFrameTimeLimit;
                            }
                            else if (isFallingFromGravity)
                            {
                                playerImageSheet.setFrameConfiguration(73, 73, 73);
                                playerImageSheet.frameTimeLimit = defaultFrameTimeLimit;
                            }
                            else if (isJumping)
                            {
                                playerImageSheet.setFrameConfiguration(36, 36, 42);
                                playerImageSheet.frameTimeLimit = defaultFrameTimeLimit;
                            }

                        }
                        else
                        {
                            if (isDashing)
                            {
                                isDashing = false;
                                exhaustedDash = true;
                            }
                            if (!isFalling && !isFallingFromGravity && !isJumping && !isHanging)
                            {
                                if (movingLeft || movingRight)
                                {
                                    playerImageSheet.setFrameConfiguration(18, 18, 25);
                                    playerImageSheet.frameTimeLimit = defaultFrameTimeLimit;
                                }
                            }

                        }
                    }

                    //--------------------------------------------------------
                    if (previousKBState.IsKeyDown(Keys.W) && currentKBState.IsKeyUp(Keys.W))
                    {
                        movingUp = false;
                    }
                    if (previousKBState.IsKeyDown(Keys.S) && currentKBState.IsKeyUp(Keys.S))
                    {
                        movingDown = false;
                    }

                    //checks if keys are up
                    if (previousKBState.IsKeyDown(Keys.A) && currentKBState.IsKeyUp(Keys.A))
                    {
                        movingLeft = false;
                        if (isHanging && !movingRight) {
                            playerImageSheet.setFrameConfiguration(91, 91, 91);
                        }
                        else if (isGrinding && !movingRight) {
                            playerImageSheet.setFrameConfiguration(55, 55, 55);
                        }
                    }
                    if (previousKBState.IsKeyDown(Keys.D) && currentKBState.IsKeyUp(Keys.D))
                    {
                        movingRight = false;
                        if (isHanging && !movingLeft)
                        {
                            playerImageSheet.setFrameConfiguration(91, 91, 91);
                        }
                        else if (isGrinding && !movingRight)
                        {
                            playerImageSheet.setFrameConfiguration(55, 55, 55);
                        }
                    }

                    //jump
                    if (previousKBState.IsKeyUp(Keys.M) &&
                        currentKBState.IsKeyDown(Keys.M) && !jumpExhausted)
                    {
                        if (wallSliding)
                        {
                            if (movingLeft)
                            {
                                movingLeft = false;
                                facingLeft = false;
                                movingRight = true;
                                facingRight = true;
                            }
                            else if (movingRight)
                            {
                                movingLeft = true;
                                facingLeft = true;
                                movingRight = false;
                                facingRight = false;
                            }
                            else {
                                movingLeft = false;
                                movingRight = false;
                            }
                            isFalling = false;
                            isFallingFromGravity = false;
                            wallSliding = false;
                            wallSlideJump = true;
                            wallSlideReference = null;
                        }

                        if (!jumpExhausted) {
                            isFalling = false;
                            isFallingFromGravity = false;
                            isJumping = true;
                            jumpTimer = 0;
                            if (!isDashing)
                            {
                                playerImageSheet.setFrameConfiguration(36, 36, 42);
                                playerImageSheet.frameTimeLimit = defaultFrameTimeLimit*2;
                            }
                        }
                    }

                    //jump
                    if (previousKBState.IsKeyDown(Keys.M) && currentKBState.IsKeyUp(Keys.M))
                    {
                        //if you want to enable double jumping just remove the if check and set 
                        //isJumping to false and remove isFalling
                        if (isJumping)
                        {
                            isJumping = false;
                            jumpExhausted = true;
                            if (currentKBState.IsKeyDown(Keys.M))
                            {
                                isFalling = true;
                            }
                            else { 
                                isFallingFromGravity = true;
                            }
                            
                            if (!isDashing)
                            {
                                playerImageSheet.setFrameConfiguration(72, 72, 72);
                                playerImageSheet.frameTimeLimit = defaultFrameTimeLimit;
                            }
                        }

                        if (wallSliding)
                        {
                            wallSliding = false;
                            wallSlideReference = null;
                        }
                    }

                    if (previousKBState.IsKeyUp(Keys.N) && currentKBState.IsKeyDown(Keys.N))
                    {
                        actionButtonBeingPressed = true;
                    }
                    if (previousKBState.IsKeyDown(Keys.N) && currentKBState.IsKeyDown(Keys.N))
                    {
                        actionButtonBeingPressed = true;
                    }
                    if (previousKBState.IsKeyDown(Keys.N) && currentKBState.IsKeyUp(Keys.N))
                    {
                        actionButtonBeingPressed = false;
                    }

                    //if you're moving/falling/fallingFromGravity/jumping standing is set to false
                    //if not standing is set to true and the animation strip is updated
                    if (movingLeft ||
                        movingRight ||
                        isJumping ||
                        isFalling ||
                        isFallingFromGravity ||
                        isDashing || 
                        isHanging || 
                        isGrinding)
                    {
                        standing = false;
                    }
                    else
                    {
                        standing = true;
                        playerImageSheet.setFrameConfiguration(0, 0, 4);
                        playerImageSheet.frameTimeLimit = defaultFrameTimeLimit;
                    }

                    if (!isJumping) {
                        if(currentKBState.IsKeyUp(Keys.M)) {
                            isFalling = false;
                            isFallingFromGravity = true;
                        }
                        else if(currentKBState.IsKeyDown(Keys.M)) {
                            isFalling = true;
                            isFallingFromGravity = false;
                        }
                    }

                    //checks to see if you're falling and the key was released, if it does gravity takes over
                    if (isFalling && currentKBState.IsKeyUp(Keys.M) && !wallSliding)
                    {
                        isFalling = false;
                        isFallingFromGravity = true;
                        playerImageSheet.setFrameConfiguration(73, 73, 73);
                        playerImageSheet.frameTimeLimit = defaultFrameTimeLimit;
                    }
                    //checks to see if you're falling and press the key to slow the fall
                    if (isFallingFromGravity && isFalling == false && currentKBState.IsKeyDown(Keys.M) && !wallSliding)
                    {
                        isFalling = true;
                        isFallingFromGravity = false;
                        playerImageSheet.setFrameConfiguration(72, 72, 72);
                        playerImageSheet.frameTimeLimit = defaultFrameTimeLimit;
                    }

                    if (wallSlideJump)
                    {
                        if (jumpTimer > jumpTimerMax || currentKBState.IsKeyUp(Keys.M))
                        {
                            if(currentKBState.IsKeyDown(Keys.A)) {
                                movingRight = false;
                                facingRight = false;
                                movingLeft = true;
                                facingLeft = true;
                            }
                            else if (currentKBState.IsKeyDown(Keys.D))
                            {
                                movingRight = true;
                                facingRight = true;
                                movingLeft = false;
                                facingLeft = false;
                            }
                            else {
                                movingLeft = false;
                                movingRight = false;
                            }
                        }
                    }
                }
            }
        }

        public void checkForPlayerGamePadInput(GamePadState previousGPState, GamePadState currentGPState)
        {
            if (!wasKilled)
            {
                if (actionStateIsActive)
                {
                    if (previousGPState.IsButtonUp(Buttons.LeftShoulder) && currentGPState.IsButtonDown(Buttons.LeftShoulder))
                    {
                        actionButtonBeingPressed = true;
                    }
                    if (previousGPState.IsButtonDown(Buttons.LeftShoulder) && currentGPState.IsButtonDown(Buttons.LeftShoulder))
                    {
                        actionButtonBeingPressed = true;
                    }
                    if (previousGPState.IsButtonDown(Buttons.LeftShoulder) && currentGPState.IsButtonUp(Buttons.LeftShoulder))
                    {
                        if (!ladderActionStateActive)
                        {
                            actionButtonBeingPressed = false;
                            actionStateIsActive = false;
                            if (hurdleActionStateActive)
                            {
                                hurdleActionStateActive = false;
                            }
                            if (swingRopeActionStateActive)
                            {
                                swingRopeActionStateActive = false;
                                ejectPlayerFromRope = true;
                            }
                        }
                    }
                    //swing rope action state
                    if (swingRopeActionStateActive)
                    {
                        if (previousGPState.IsButtonUp(Buttons.DPadLeft) && currentGPState.IsButtonDown(Buttons.DPadLeft))
                        {
                            facingLeft = true;
                            facingRight = false;
                            movingLeft = true;
                            movingRight = false;

                            //sets to the appropriate swing animation
                            playerImageSheet.setFrameConfiguration(18, 18, 25);
                            playerImageSheet.frameTimeLimit = defaultFrameTimeLimit;
                        }
                        if (previousGPState.IsButtonDown(Buttons.DPadLeft) && currentGPState.IsButtonUp(Buttons.DPadLeft))
                        {
                            movingLeft = false;
                            if (currentGPState.IsButtonUp(Buttons.DPadRight))
                            {
                                //sets to the isHanging on the rope animation
                                playerImageSheet.setFrameConfiguration(0, 0, 4);
                                playerImageSheet.frameTimeLimit = defaultFrameTimeLimit;
                            }
                        }
                        if (previousGPState.IsButtonUp(Buttons.DPadRight) && currentGPState.IsButtonDown(Buttons.DPadRight))
                        {
                            facingLeft = false;
                            facingRight = true;
                            movingRight = true;
                            movingLeft = false;
                            //sets to the appropriate swing animation
                            playerImageSheet.setFrameConfiguration(18, 18, 25);
                            playerImageSheet.frameTimeLimit = defaultFrameTimeLimit;
                        }
                        if (previousGPState.IsButtonDown(Buttons.DPadRight) && currentGPState.IsButtonUp(Buttons.DPadRight))
                        {
                            movingRight = false;
                            if (currentGPState.IsButtonUp(Buttons.DPadLeft))
                            {
                                //sets to the isHanging on the rope animation
                                playerImageSheet.setFrameConfiguration(0, 0, 4);
                                playerImageSheet.frameTimeLimit = defaultFrameTimeLimit;
                            }
                        }

                        //checks if dash was pressed
                        if (previousGPState.IsButtonUp(Buttons.X) &&
                            currentGPState.IsButtonDown(Buttons.X))
                        {
                            if (exhaustedDash == false)
                            {
                                isDashing = true;
                                dashTimer = 0;
                                playerImageSheet.setFrameConfiguration(54, 54, 54);
                                playerImageSheet.frameTimeLimit = defaultFrameTimeLimit;
                            }
                        }
                        //tracks dashing modifiers, if dashing and within the duration modifies the position accordingly
                        //else it's outside duration and sets dashing to false, but that it's been exhausted, the exhaustedDash
                        //needs to be reset by collision objects otherwise it breaks
                        if (isDashing)
                        {
                            if (dashTimer < dashTimerMax &&
                                exhaustedDash == false)
                            {
                                //if you want to be able to dash without having to move change these to "facingLeft" and "facingRight"
                                dashTimer++;
                            }
                            else
                            {
                                isDashing = false;
                                if ((movingLeft || movingRight))
                                {
                                    playerImageSheet.setFrameConfiguration(18, 18, 25);
                                    playerImageSheet.frameTimeLimit = defaultFrameTimeLimit;
                                }
                            }
                        }
                        if (!movingLeft && !movingRight)
                        {
                            //would set to default hold animation
                            playerImageSheet.setFrameConfiguration(0, 0, 4);
                            playerImageSheet.frameTimeLimit = defaultFrameTimeLimit;
                        }
                        else
                        {

                        }

                    }
                    //ladder action state input
                    if (ladderActionStateActive)
                    {
                        //---------------------------- down presses here ----------------------------
                        if (previousGPState.IsButtonUp(Buttons.DPadUp) && currentGPState.IsButtonDown(Buttons.DPadUp))
                        {
                            slidingDownLadder = false;
                            climbingLadder = true;

                            playerImageSheet.setFrameConfiguration(91, 91, 94);
                            playerImageSheet.frameTimeLimit = defaultFrameTimeLimit;
                        }
                        if (previousGPState.IsButtonUp(Buttons.DPadDown) && currentGPState.IsButtonDown(Buttons.DPadDown))
                        {
                            playerImageSheet.setFrameConfiguration(92, 92, 92);
                            playerImageSheet.frameTimeLimit = defaultFrameTimeLimit;
                            slidingDownLadder = true;
                            climbingLadder = false;
                        }
                        if (previousGPState.IsButtonUp(Buttons.A) && currentGPState.IsButtonDown(Buttons.A))
                        {
                            playerImageSheet.setFrameConfiguration(36, 36, 42);
                            playerImageSheet.frameTimeLimit = 6;
                            playerImageSheet.isAnimating = true;

                            isJumping = true;
                            jumpTimer = jumpTimerMax / 4;
                            ladderActionStateActive = false;
                            slidingDownLadder = false;
                            climbingLadder = false;
                            holdingLadder = false;
                        }
                        if (previousGPState.IsButtonUp(Buttons.DPadLeft) && currentGPState.IsButtonDown(Buttons.DPadLeft))
                        {
                            movingLeft = true;
                            facingLeft = true;
                            movingRight = false;
                            facingRight = false;
                        }
                        if (previousGPState.IsButtonUp(Buttons.DPadRight) && currentGPState.IsButtonDown(Buttons.DPadRight))
                        {
                            movingRight = true;
                            facingRight = true;
                            movingLeft = false;
                            facingLeft = false;
                        }

                        //---------------------------- up presses here ----------------------------
                        if (previousGPState.IsButtonDown(Buttons.DPadUp) && currentGPState.IsButtonUp(Buttons.DPadUp))
                        {
                            climbingLadder = false;
                        }
                        if (previousGPState.IsButtonDown(Buttons.DPadDown) && currentGPState.IsButtonUp(Buttons.DPadDown))
                        {
                            slidingDownLadder = false;
                        }
                        if (previousGPState.IsButtonDown(Buttons.DPadLeft) && currentGPState.IsButtonUp(Buttons.DPadLeft))
                        {
                            movingLeft = false;
                        }
                        if (previousGPState.IsButtonDown(Buttons.DPadRight) && currentGPState.IsButtonUp(Buttons.DPadRight))
                        {
                            movingRight = false;
                        }
                        //---------------------------- state based occurences go here ----------------------------
                        if (!climbingLadder && !slidingDownLadder)
                        {
                            playerImageSheet.isAnimating = false;
                            if (previousGPState.IsButtonUp(Buttons.A) && currentGPState.IsButtonDown(Buttons.A))
                            {
                                playerImageSheet.isAnimating = true;
                            }
                            else
                            {
                                holdingLadder = true;
                            }
                        }
                        else
                        {
                            playerImageSheet.isAnimating = true;
                            holdingLadder = false;
                        }
                    }
                }
                else
                {
                    //checks if keys are down
                    if (previousGPState.IsButtonUp(Buttons.DPadLeft) && currentGPState.IsButtonDown(Buttons.DPadLeft) || previousGPState.IsButtonDown(Buttons.DPadLeft) && currentGPState.IsButtonDown(Buttons.DPadLeft))
                    {
                        if (movingLeft)
                        {
                            //do nothing
                        }
                        else
                        {
                            if (!isHanging && !isGrinding)
                            {
                                //fixes animations for the character's fall
                                if (isFalling)
                                {
                                    playerImageSheet.setFrameConfiguration(72, 72, 72);
                                    playerImageSheet.frameTimeLimit = defaultFrameTimeLimit;
                                }
                                else if (isFallingFromGravity)
                                {
                                    playerImageSheet.setFrameConfiguration(73, 73, 73);
                                    playerImageSheet.frameTimeLimit = defaultFrameTimeLimit;
                                }
                                else
                                {
                                    playerImageSheet.setFrameConfiguration(18, 18, 28);
                                    playerImageSheet.frameTimeLimit = defaultFrameTimeLimit;
                                }
                            }
                            else
                            {
                                if (isHanging)
                                {
                                    playerImageSheet.setFrameConfiguration(91, 91, 94);
                                    playerImageSheet.frameTimeLimit = defaultFrameTimeLimit;
                                }
                                else if (isGrinding)
                                {
                                    playerImageSheet.setFrameConfiguration(55, 55, 55);
                                    playerImageSheet.frameTimeLimit = defaultFrameTimeLimit;
                                }
                            }
                        }

                        movingLeft = true;
                        movingRight = false;
                        facingLeft = true;
                        facingRight = false;
                    }
                    if (previousGPState.IsButtonUp(Buttons.DPadRight) && currentGPState.IsButtonDown(Buttons.DPadRight) || previousGPState.IsButtonDown(Buttons.DPadRight) && currentGPState.IsButtonDown(Buttons.DPadRight))
                    {
                        if (movingRight)
                        {
                            //do nothing
                        }
                        else
                        {
                            if (!isHanging  && !isGrinding)
                            {
                                //fixes animations for the character's fall
                                if (isFalling)
                                {
                                    playerImageSheet.setFrameConfiguration(72, 72, 72);
                                    playerImageSheet.frameTimeLimit = defaultFrameTimeLimit;

                                }
                                else if (isFallingFromGravity)
                                {
                                    playerImageSheet.setFrameConfiguration(73, 73, 73);
                                    playerImageSheet.frameTimeLimit = defaultFrameTimeLimit;
                                }
                                else
                                {
                                    playerImageSheet.setFrameConfiguration(18, 18, 28);
                                    playerImageSheet.frameTimeLimit = defaultFrameTimeLimit;
                                }
                            }
                            else
                            {
                                if (isHanging)
                                {
                                    playerImageSheet.setFrameConfiguration(91, 91, 94);
                                    playerImageSheet.frameTimeLimit = defaultFrameTimeLimit;
                                }
                                else if (isGrinding) {
                                    playerImageSheet.setFrameConfiguration(55, 55, 55);
                                    playerImageSheet.frameTimeLimit = defaultFrameTimeLimit;
                                }
                            }
                        }

                        movingLeft = false;
                        movingRight = true;
                        facingRight = true;
                        facingLeft = false;
                    }

                    //--------------------------- checks for up and down key presses ------------------------------------
                    if (previousGPState.IsButtonUp(Buttons.DPadUp) && currentGPState.IsButtonDown(Buttons.DPadUp))
                    {
                        movingUp = true;
                    }
                    if (previousGPState.IsButtonUp(Buttons.DPadDown) && currentGPState.IsButtonDown(Buttons.DPadDown))
                    {
                        movingDown = true;
                    }
                    //---------------------------------------------------------------------------------------------------

                    //-------------checks if dash was pressed----------------
                    if (previousGPState.IsButtonUp(Buttons.X) &&
                        currentGPState.IsButtonDown(Buttons.X))
                    {
                        if (exhaustedDash == false && !isHanging && !isGrinding)
                        {
                            isDashing = true;
                            dashTimer = 0;
                            playerImageSheet.setFrameConfiguration(54, 54, 54);
                            playerImageSheet.frameTimeLimit = defaultFrameTimeLimit;
                        }
                    }

                    //checks if dash was released
                    if (previousGPState.IsButtonDown(Buttons.X) && currentGPState.IsButtonUp(Buttons.X))
                    {
                        //if you are jumping or falling or falling from gravity the exhausted dash is triggered
                        //if you aren't jumping/falling/falling from gravity it's safe to assume you're probably moving or standing on 
                        //surface, because of this the exhausted dash won't be flagged and you can dash again
                        if (isJumping || isFalling || isFallingFromGravity)
                        {
                            if (isDashing)
                            {
                                isDashing = false;
                                exhaustedDash = true;
                            }

                            if (isFalling)
                            {
                                playerImageSheet.setFrameConfiguration(72, 72, 72);
                                playerImageSheet.frameTimeLimit = defaultFrameTimeLimit;
                            }
                            else if (isFallingFromGravity)
                            {
                                playerImageSheet.setFrameConfiguration(73, 73, 73);
                                playerImageSheet.frameTimeLimit = defaultFrameTimeLimit;
                            }
                            else if (isJumping)
                            {
                                playerImageSheet.setFrameConfiguration(36, 36, 42);
                                playerImageSheet.frameTimeLimit = defaultFrameTimeLimit;
                            }

                        }
                        else
                        {
                            if (isDashing)
                            {
                                isDashing = false;
                                exhaustedDash = true;
                            }
                            if (!isFalling && !isFallingFromGravity && !isJumping && !isHanging && !isGrinding)
                            {
                                if (movingLeft || movingRight)
                                {
                                    playerImageSheet.setFrameConfiguration(18, 18, 25);
                                    playerImageSheet.frameTimeLimit = defaultFrameTimeLimit;
                                }
                            }

                        }
                    }
                    //--------------------------------------------------------
                    if (previousGPState.IsButtonDown(Buttons.DPadUp) && currentGPState.IsButtonUp(Buttons.DPadUp))
                    {
                        movingUp = false;
                    }
                    if (previousGPState.IsButtonDown(Buttons.DPadDown) && currentGPState.IsButtonUp(Buttons.DPadDown))
                    {
                        movingDown = false;
                    }

                    //checks if keys are up
                    if (currentGPState.IsButtonUp(Buttons.DPadLeft))
                    {
                        movingLeft = false;
                        if (isHanging && !movingRight)
                        {
                            playerImageSheet.setFrameConfiguration(91, 91, 91);
                        }
                        else if (isGrinding && !movingRight)
                        {
                            playerImageSheet.setFrameConfiguration(55, 55, 55);
                        }
                    }
                    if (currentGPState.IsButtonUp(Buttons.DPadRight))
                    {
                        movingRight = false;
                        if (isHanging && !movingLeft)
                        {
                            playerImageSheet.setFrameConfiguration(91, 91, 91);
                        }
                        else if (isGrinding && !movingLeft)
                        {
                            playerImageSheet.setFrameConfiguration(55, 55, 55);
                        }
                    }

                    //jump
                    if (previousGPState.IsButtonUp(Buttons.A) &&
                        currentGPState.IsButtonDown(Buttons.A))
                    {
                        if (isJumping || isFalling || isFallingFromGravity)
                        {
                        }
                        else
                        {
                            isJumping = true;
                            jumpTimer = 0;
                            if (!isDashing)
                            {
                                playerImageSheet.setFrameConfiguration(36, 36, 42);
                                playerImageSheet.frameTimeLimit = defaultFrameTimeLimit;
                            }
                        }
                    }

                    //jump
                    if (previousGPState.IsButtonDown(Buttons.A) && currentGPState.IsButtonUp(Buttons.A))
                    {
                        //if you want to enable double jumping just remove the if check and set 
                        //isJumping to false and remove isFalling
                        if (isJumping)
                        {
                            isJumping = false;
                            isFalling = true;
                            if (!isDashing)
                            {
                                playerImageSheet.setFrameConfiguration(72, 72, 72);
                                playerImageSheet.frameTimeLimit = defaultFrameTimeLimit;
                            }
                        }
                    }

                    if (previousGPState.IsButtonUp(Buttons.LeftShoulder) && currentGPState.IsButtonDown(Buttons.LeftShoulder))
                    {
                        actionButtonBeingPressed = true;
                    }
                    if (previousGPState.IsButtonDown(Buttons.LeftShoulder) && currentGPState.IsButtonDown(Buttons.LeftShoulder))
                    {
                        actionButtonBeingPressed = true;
                    }
                    if (previousGPState.IsButtonDown(Buttons.LeftShoulder) && currentGPState.IsButtonUp(Buttons.LeftShoulder))
                    {
                        actionButtonBeingPressed = false;
                    }

                    //if you're moving/falling/fallingFromGravity/jumping standing is set to false
                    //if not standing is set to true and the animation strip is updated
                    if (movingLeft ||
                        movingRight ||
                        isJumping ||
                        isFalling ||
                        isFallingFromGravity ||
                        isDashing ||
                        isHanging || 
                        isGrinding)
                    {
                        standing = false;
                    }
                    else
                    {
                        standing = true;
                        playerImageSheet.setFrameConfiguration(0, 0, 4);
                        playerImageSheet.frameTimeLimit = defaultFrameTimeLimit;
                    }

                    //checks to see if you're falling and the key was released, if it does gravity takes over
                    if (isFalling && currentGPState.IsButtonUp(Buttons.A))
                    {
                        isFalling = false;
                        isFallingFromGravity = true;
                        playerImageSheet.setFrameConfiguration(73, 73, 73);
                        playerImageSheet.frameTimeLimit = defaultFrameTimeLimit;
                    }
                    //checks to see if you're falling and press the key to slow the fall
                    if (isFallingFromGravity && isFalling == false && currentGPState.IsButtonDown(Buttons.A))
                    {
                        isFalling = true;
                        isFallingFromGravity = false;
                        playerImageSheet.setFrameConfiguration(72, 72, 72);
                        playerImageSheet.frameTimeLimit = defaultFrameTimeLimit;
                    }
                }
            }
        }
        //TO DO: make an environment class for the stages to pass in so I know how to modify the movement accordingly
        // potential modifiers: wind, terrain slickness(may just handle this in the wall collision class), gravity, etc 
        // in the mean time just keep track of items individually
        public Vector2 calculateNewPosition(Vector2 gravity)
        {
            //saves the old position before modifying it and updating it accordingly
            previousWorldPosition = currentWorldPosition;
            Vector2 newPosition = currentWorldPosition;

            if (!wasKilled)
            {
                if (actionStateIsActive)
                {
                    //hurdle action state
                    if (hurdleActionStateActive)
                    {
                        if (currentActionHurdleReference != null)
                        {
                            //TO DO: have it run through the hurdle's action state sequence here
                            if (upTheHurdle)
                            {
                                hurdleTimer++;
                                if (movingLeft)
                                {
                                    newPosition.X += hurdleDistance.X / hurdleTimerMax;
                                }
                                if (movingRight)
                                {
                                    newPosition.X -= hurdleDistance.X / hurdleTimerMax;
                                }
                                newPosition.Y -= hurdleDistance.Y / hurdleTimerMax;

                                if (hurdleTimer > hurdleTimerMax)
                                {
                                    upTheHurdle = false;
                                    overTheHurdle = true;
                                    if (movingLeft)
                                    {
                                        hurdleDistance.X = currentActionHurdleReference.getBoundingBox.Left - boundingRectangle.Right;
                                    }
                                    if (movingRight)
                                    {
                                        hurdleDistance.X = currentActionHurdleReference.getBoundingBox.Right - boundingRectangle.Left;
                                    }
                                    hurdleDistance.Y = currentActionHurdleReference.getBoundingBox.Top - boundingRectangle.Bottom;
                                    hurdleTimer = 0;
                                    hurdleTimerMax = 10;
                                }
                            }

                            if (overTheHurdle)
                            {
                                hurdleTimer++;
                                newPosition.X += hurdleDistance.X / hurdleTimerMax;
                                newPosition.Y -= hurdleDistance.Y / hurdleTimerMax;
                                if (hurdleTimer > hurdleTimerMax)
                                {
                                    overTheHurdle = false;
                                    if (movingLeft)
                                    {
                                        momentum.X = -50;
                                    }
                                    if (movingRight)
                                    {
                                        momentum.X = 50;
                                    }
                                }
                            }

                            if (!upTheHurdle && !overTheHurdle)
                            {
                                hurdleActionStateActive = false;
                            }
                        }
                        else
                        {
                            hurdleActionStateActive = false;
                        }
                    }
                    //ladder action state
                    if (ladderActionStateActive)
                    {
                        if (climbingLadder)
                        {
                            if (boundingRectangle.Top >= ladderReference.getBoundingBox.Top - height)
                            {
                                if (boundingRectangle.Top < ladderReference.getBoundingBox.Top - height / 1.5)
                                {
                                    playerImageSheet.setFrameConfiguration(95, 95, 95);
                                }
                                newPosition.Y -= 3;
                            }
                            else
                            {
                                ladderActionStateActive = false;
                                slidingDownLadder = false;
                                climbingLadder = false;
                                holdingLadder = false;
                                isJumping = false;
                            }
                        }
                        else if (slidingDownLadder)
                        {
                            if (boundingRectangle.Bottom < ladderReference.getBoundingBox.Bottom)
                            {
                                newPosition.Y += 6;
                            }
                            else
                            {
                                ladderActionStateActive = false;
                                slidingDownLadder = false;
                                climbingLadder = false;
                                holdingLadder = false;
                                isJumping = false;
                            }
                        }
                        else
                        {
                            //do nothing
                        }
                    }

                    //resets the action state flag if nothing is pressed
                    if (hurdleActionStateActive || ladderActionStateActive || swingRopeActionStateActive)
                    {
                        actionStateIsActive = true;
                    }
                    else
                    {
                        currentActionHurdleReference = null;
                        ladderReference = null;
                        actionStateIsActive = false;
                    }
                    return newPosition;
                }
                else 
                {
                    //accounts for movement in these directions
                    if (movingLeft)
                    {
                        if (isGrinding)
                        {
                            newPosition.X -= velocity.X * 2;
                        }
                        else {
                            newPosition.X -= velocity.X;
                        }
                    }
                    if (movingRight)
                    {
                        if (isGrinding)
                        {
                            newPosition.X += velocity.X * 2;
                        }
                        else
                        {
                            newPosition.X += velocity.X;
                        }
                    }

                    //accounts for the action states, standing, jumping, falling, dashing
                    if (isJumping)
                    {
                        if (jumpTimer < jumpTimerMax)
                        {
                            //This basically ups the velocity of the player and diminishes it based on the duration of the jump, the longer the jump the less high you will go
                            //Abe:  If you want to do it based on progress, use a parabolic velocity function - e.g. x(t) = v * t * cos(angle), y(t) = v * t * sin(angle) - 0.5 * g * t^2 
                            newPosition.Y -= velocity.Y - (velocity.Y * ((float)jumpTimer / (float)jumpTimerMax));
                        }
                        else
                        {
                            jumpExhausted = true;
                            isJumping = false;
                            //possibly set the falling animation here and then on any collision have the player check if they're falling and set the animation accordingly
                            isFalling = true;
                            if (!isDashing)
                            {
                                playerImageSheet.setFrameConfiguration(72, 72, 72);
                                playerImageSheet.frameTimeLimit = defaultFrameTimeLimit;
                            }
                        }
                        jumpTimer++;
                        //System.Diagnostics.Debug.WriteLine(jumpTimer);
                    }

                    if (isFalling)
                    {
                        //if falling this is added to counteract gravity, set to 0 for no effect or remove as well
                        if (isDashing)
                        {
                            //dont update vector
                        }
                        else
                        {
                            if (!wallSliding) {
                                newPosition -= new Vector2(0, 6);
                            }
                        }
                    }

                    //tracks dashing modifiers, if dashing and within the duration modifies the position accordingly
                    //else it's outside duration and sets dashing to false, but that it's been exhausted, the exhaustedDash
                    //needs to be reset by collision objects otherwise it breaks
                    if (isDashing && !wallSliding)
                    {
                        if (dashTimer < dashTimerMax &&
                            exhaustedDash == false)
                        {
                            //if you want to be able to dash without having to move change these to "facingLeft" and "facingRight"
                            if (facingLeft)
                            {
                                newPosition.X -= 10f;
                            }
                            if (facingRight)
                            {
                                newPosition.X += 10f;
                            }
                        }
                        else
                        {
                            isDashing = false;
                            exhaustedDash = true;
                            if ((movingLeft || movingRight) && (!isFalling && !isFallingFromGravity && !isJumping))
                            {
                                playerImageSheet.setFrameConfiguration(18, 18, 25);
                                playerImageSheet.frameTimeLimit = defaultFrameTimeLimit;
                            }
                            else
                            {
                                if (isFalling)
                                {
                                    playerImageSheet.setFrameConfiguration(72, 72, 72);
                                    playerImageSheet.frameTimeLimit = defaultFrameTimeLimit;
                                }
                                else if (isFallingFromGravity)
                                {
                                    playerImageSheet.setFrameConfiguration(73, 73, 73);
                                    playerImageSheet.frameTimeLimit = defaultFrameTimeLimit;
                                }
                                else if (isJumping)
                                {
                                    playerImageSheet.setFrameConfiguration(36, 36, 42);
                                    playerImageSheet.frameTimeLimit = defaultFrameTimeLimit;
                                }
                            }
                        }
                        dashTimer++;
                    }

                    //adds environment modifying vectors
                    if (isJumping || isDashing)
                    {
                         newPosition -= gravity;                       
                    }

                    if (wallSliding)
                    {
                        newPosition.Y += (gravity.Y*.5f);
                    }
                    else {
                        newPosition += gravity;
                    }
                    

                    newPosition += momentum;
                    momentum.X = (float)(momentum.X * .9);
                    momentum.Y = (float)(momentum.Y * .9);

                    return newPosition;
                }
            }
            else {
                return newPosition;
            }
        }
    }
}
