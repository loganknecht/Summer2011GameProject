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
        public SpriteBatch sbReference;
        public ContentManager cmReference;
        public GraphicsDeviceManager gdmReference;

        public Texture2D boundingRectangleColor;

        public Vector2 previousWorldPosition;
        public Vector2 currentWorldPosition;
        public Vector2 velocity;
        public Vector2 momentum;

        //tracks the movement in each direction
        public bool movingLeft;
        public bool movingRight;
        public bool movingUp;
        public bool movingDown;

        //default no key press state
        public bool standing;

        //movement states the player can have, these will overide the standing flag and set it to false so that the appropriate animation is played
        public bool isJumping;
        public bool isFalling;
        public bool isFallingFromGravity;
        public bool isDashing;
        public bool exhaustedDash;

        //action states
        public Hurdle currentActionHurdleReference;
        public bool actionButtonBeingPressed;
        public bool actionStateIsActive;
        public bool hurdleActionStateActive;
        public bool upTheHurdle;
        public bool overTheHurdle;
        public int hurdleTimer;
        public int hurdleTimerMax;
        public Vector2 hurdleDistance;

        //tracker for jump duration
        public int jumpTimer;
        //static number that is the bounds for the jumpTimer
        public int jumpTimerMax;

        //tracks the time for dash
        public int dashTimer;
        //tracks the max time allowed for dashing
        public int dashTimerMax;

        //accounts for players facing
        public bool facingLeft;
        public bool facingRight;

        public bool playerCollisionOccurred;
        public bool collisionOnPlayerLeft;
        public bool collisionOnPlayerRight;
        public bool collisionOnPlayerTop;
        public bool collisionOnPlayerBottom;

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

        public float width
        {
            get
            {
                return playerImageSheet.frameSize.X;
            }
        }

        public float height
        {
            get
            {
                return playerImageSheet.frameSize.Y;
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

        public Player(ContentManager cm, SpriteBatch sb, GraphicsDeviceManager gdm)
        {
            //configures the image sheet associated with the player
            playerImageSheet = new AnimatedImage(cm, sb, gdm.GraphicsDevice, "PlayerImages/PlayerPlaceholderRefined", new Vector2(1372, 469), new Vector2(18, 5));
            playerImageSheet.frameTimeLimit = 8;
            playerImageSheet.Initialize();
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
            velocity = new Vector2(8, 25);
            momentum = new Vector2(0,0);

            movingLeft = false;
            movingRight = false;
            movingUp = false;
            movingDown = false;

            standing = true;

            isJumping = false;
            isFalling = false;
            isFallingFromGravity = false;
            isDashing = false;
            exhaustedDash = false;

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

            facingLeft = false;
            facingRight = true;

            playerCollisionOccurred = false;
            collisionOnPlayerLeft = false;
            collisionOnPlayerRight = false;
            collisionOnPlayerTop = false;
            collisionOnPlayerBottom = false;
        }

        public void Initialize()
        {
        }

        public void Draw()
        {
            if (facingRight)
            {
                playerImageSheet.Draw(false);
            }
            else if (facingLeft)
            {
                playerImageSheet.Draw(true);
            }
        }
        //draws at different position
        public void DrawAltPosition(Vector2 position)
        {
            if (facingRight)
            {
                playerImageSheet.DrawAltPosition(position, false);
            }
            else if (facingLeft)
            {
                playerImageSheet.DrawAltPosition(position, true);
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
            playerImageSheet.position = currentWorldPosition;
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
                        playerImageSheet.frameTimeLimit = 8;
                    }
                }
                if (isFallingFromGravity)
                {
                    isFallingFromGravity = false;
                    if (movingRight || movingLeft)
                    {
                        playerImageSheet.setFrameConfiguration(18, 18, 28);
                        playerImageSheet.frameTimeLimit = 8;
                    }
                    else if (standing)
                    {
                        playerImageSheet.setFrameConfiguration(0, 0, 4);
                        playerImageSheet.frameTimeLimit = 8;
                    }
                }
                if (exhaustedDash)
                {
                    exhaustedDash = false;
                }
            }
        }

        public void checkForPlayerInput(KeyboardState previousKBState, KeyboardState currentKBState)
        {
            if (actionStateIsActive) {
                if (previousKBState.IsKeyUp(Keys.V) && currentKBState.IsKeyDown(Keys.V))
                {
                    actionButtonBeingPressed = true;
                }
                if (previousKBState.IsKeyDown(Keys.V) && currentKBState.IsKeyDown(Keys.V))
                {
                    actionButtonBeingPressed = true;
                }
                if (previousKBState.IsKeyDown(Keys.V) && currentKBState.IsKeyUp(Keys.V))
                {
                    actionButtonBeingPressed = false;
                }
            }
            else {
                //checks if keys are down
                if (currentKBState.IsKeyDown(Keys.A))
                {
                    if (movingLeft)
                    {
                        //do nothing
                    }
                    else
                    {
                        //fixes animations for the character's fall
                        if (isFalling)
                        {
                            playerImageSheet.setFrameConfiguration(72, 72, 72);
                            playerImageSheet.frameTimeLimit = 8;
                        }
                        else if (isFallingFromGravity)
                        {
                            playerImageSheet.setFrameConfiguration(73, 73, 73);
                            playerImageSheet.frameTimeLimit = 8;
                        }
                        else
                        {
                            playerImageSheet.setFrameConfiguration(18, 18, 28);
                            playerImageSheet.frameTimeLimit = 8;
                        }
                    }
                    //checks if dash was pressed
                    if (previousKBState.IsKeyUp(Keys.W) &&
                        currentKBState.IsKeyDown(Keys.W))
                    {
                        if (exhaustedDash == false)
                        {
                            isDashing = true;
                            System.Diagnostics.Debug.WriteLine("isDashing: " + isDashing);
                            dashTimer = 0;
                            playerImageSheet.setFrameConfiguration(54, 54, 54);
                            playerImageSheet.frameTimeLimit = 8;
                        }
                    }
                    facingLeft = true;
                    facingRight = false;
                    movingLeft = true;
                }
                if (currentKBState.IsKeyDown(Keys.D))
                {
                    if (movingRight)
                    {
                        //do nothing
                    }
                    else
                    {
                        //fixes animations for the character's fall
                        if (isFalling)
                        {
                            playerImageSheet.setFrameConfiguration(72, 72, 72);
                            playerImageSheet.frameTimeLimit = 8;

                        }
                        else if (isFallingFromGravity)
                        {
                            playerImageSheet.setFrameConfiguration(73, 73, 73);
                            playerImageSheet.frameTimeLimit = 8;
                        }
                        else
                        {
                            playerImageSheet.setFrameConfiguration(18, 18, 28);
                            playerImageSheet.frameTimeLimit = 8;

                        }
                    }
                //checks if dash was pressed
                if (previousKBState.IsKeyUp(Keys.W) &&
                    currentKBState.IsKeyDown(Keys.W))
                {
                    if (exhaustedDash == false)
                    {
                        isDashing = true;
                        dashTimer = 0;
                        playerImageSheet.setFrameConfiguration(54, 54, 54);
                        playerImageSheet.frameTimeLimit = 8;
                    }
                }
                    movingRight = true;
                    facingRight = true;
                    facingLeft = false;
                }
                if (currentKBState.IsKeyDown(Keys.S)) {
                }

                //checks if keys are up
                if (currentKBState.IsKeyUp(Keys.A))
                {
                    movingLeft = false;
                }
                if (currentKBState.IsKeyUp(Keys.D))
                {
                    movingRight = false;
                }

                //checks if dash was released
                if (previousKBState.IsKeyDown(Keys.W) && currentKBState.IsKeyUp(Keys.W))
                {
                    //if you are jumping or falling or falling from gravity the exhausted dash is triggered
                    //if you aren't jumping/falling/falling from gravity it's safe to assume you're probably moving or standing on 
                    //surface, because of this the exhausted dash won't be flagged and you can dash again
                    if (isJumping || isFalling || isFallingFromGravity)
                    {
                        isDashing = false;
                        exhaustedDash = true;

                        if (isFalling)
                        {
                            playerImageSheet.setFrameConfiguration(72, 72, 72);
                            playerImageSheet.frameTimeLimit = 8;
                        }
                        else if (isFallingFromGravity)
                        {
                            playerImageSheet.setFrameConfiguration(73, 73, 73);
                            playerImageSheet.frameTimeLimit = 8;
                        }
                        else if (isJumping)
                        {
                            playerImageSheet.setFrameConfiguration(36, 36, 42);
                            playerImageSheet.frameTimeLimit = 6;
                        }

                    }
                    else
                    {
                        isDashing = false;
                    }
                }

                //jump
                if (previousKBState.IsKeyUp(Keys.Space) &&
                    currentKBState.IsKeyDown(Keys.Space))
                {
                    if (isJumping || isFalling || isFallingFromGravity)
                    {
                    }
                    else
                    {
                        isJumping = true;
                        jumpTimer = 0;
                        playerImageSheet.setFrameConfiguration(36, 36, 42);
                        playerImageSheet.frameTimeLimit = 6;
                    }
                }

                //jump
                if (previousKBState.IsKeyDown(Keys.Space) && currentKBState.IsKeyUp(Keys.Space))
                {
                    //if you want to enable double jumping just remove the if check and set 
                    //isJumping to false and remove isFalling
                    if (isJumping)
                    {
                        isJumping = false;
                        isFalling = true;
                        playerImageSheet.setFrameConfiguration(72, 72, 72);
                        playerImageSheet.frameTimeLimit = 8;
                    }
                }

                if (previousKBState.IsKeyUp(Keys.V) && currentKBState.IsKeyDown(Keys.V))
                {
                    actionButtonBeingPressed = true;
                }
                if (previousKBState.IsKeyDown(Keys.V) && currentKBState.IsKeyDown(Keys.V))
                {
                    actionButtonBeingPressed = true;
                }
                if (previousKBState.IsKeyDown(Keys.V) && currentKBState.IsKeyUp(Keys.V))
                {
                    actionButtonBeingPressed = false;
                }

                //if you're moving/falling/fallingFromGravity/jumping standing is set to false
                //if not standing is set to true and the animation strip is updated
                if (movingDown ||
                    movingLeft ||
                    movingRight ||
                    movingUp ||
                    isJumping ||
                    isFalling ||
                    isFallingFromGravity ||
                    isDashing
                    )
                {
                    standing = false;
                }
                else
                {
                    standing = true;
                    playerImageSheet.setFrameConfiguration(0, 0, 4);
                    playerImageSheet.frameTimeLimit = 8;
                }

                //checks to see if you're falling and the key was released, if it does gravity takes over
                if (isFalling && currentKBState.IsKeyUp(Keys.Space))
                {
                    isFalling = false;
                    isFallingFromGravity = true;
                    playerImageSheet.setFrameConfiguration(73, 73, 73);
                    playerImageSheet.frameTimeLimit = 8;
                }
                //checks to see if you're falling and press the key to slow the fall
                if (isFallingFromGravity && isFalling == false && currentKBState.IsKeyDown(Keys.Space))
                {
                    isFalling = true;
                    isFallingFromGravity = false;
                    playerImageSheet.setFrameConfiguration(72, 72, 72);
                    playerImageSheet.frameTimeLimit = 8;
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

            if (actionStateIsActive)
            {
                if (hurdleActionStateActive) {
                    if (currentActionHurdleReference != null) {
                        //TO DO: have it run through the hurdle's action state sequence here
                        if (upTheHurdle) {
                            hurdleTimer++;
                            if (movingLeft) {
                                newPosition.X += hurdleDistance.X/hurdleTimerMax;
                            }
                            if (movingRight) {
                                newPosition.X -= hurdleDistance.X / hurdleTimerMax;
                            }
                            newPosition.Y -= hurdleDistance.Y/hurdleTimerMax;

                            if (hurdleTimer > hurdleTimerMax) {
                                upTheHurdle = false;
                                overTheHurdle = true;
                                if (movingLeft) {
                                    hurdleDistance.X = currentActionHurdleReference.getBoundingBox.Left - boundingRectangle.Right;
                                }
                                if (movingRight) {
                                    hurdleDistance.X = currentActionHurdleReference.getBoundingBox.Right - boundingRectangle.Left;
                                }
                                hurdleDistance.Y = currentActionHurdleReference.getBoundingBox.Top - boundingRectangle.Bottom;
                                hurdleTimer = 0;
                                hurdleTimerMax = 10;
                            }
                        }

                        if (overTheHurdle) {
                            hurdleTimer++;
                            newPosition.X += hurdleDistance.X / hurdleTimerMax;
                            newPosition.Y -= hurdleDistance.Y / hurdleTimerMax;
                            if (hurdleTimer > hurdleTimerMax) {
                                overTheHurdle = false;
                                if (movingLeft) {
                                    momentum.X = -50;
                                }
                                if (movingRight) {
                                    momentum.X = 50;
                                }
                            }
                        }

                        if (!upTheHurdle && !overTheHurdle) {
                            hurdleActionStateActive = false;
                        }
                    }
                    else {
                        hurdleActionStateActive = false;
                    }

                    //resets the action state flag if nothing is pressed
                    if (hurdleActionStateActive) {
                        actionStateIsActive = true;
                    }
                    else {
                        currentActionHurdleReference = null;
                        actionStateIsActive = false;
                    }
                }
                return newPosition;
            }
            else {
                //accounts for movement in these directions
                if (movingLeft)
                {
                    newPosition.X -= velocity.X;
                }
                if (movingRight)
                {
                    newPosition.X += velocity.X;
                }

                //accounts for the action states, standing, jumping, falling, dashing
                if (isJumping)
                {
                    if (jumpTimer < jumpTimerMax)
                    {
                        //This basically ups the velocity of the player and diminishes it based on the duration of the jump, the longer the jump the less high you will go
                        //Abe:  If you want to do it based on progress, use a parabolic velocity function - e.g. x(t) = v * t * cos(angle), y(t) = v * t * sin(angle) - 0.5 * g * t^2 
                        if (isDashing)
                        {
                            //don't update player jump values
                        }
                        else
                        {
                            newPosition.Y -= velocity.Y - (velocity.Y * ((float)jumpTimer / (float)jumpTimerMax));
                        }
                    }
                    else
                    {
                        isJumping = false;
                        //possibly set the falling animation here and then on any collision have the player check if they're falling and set the animation accordingly
                        isFalling = true;
                        playerImageSheet.setFrameConfiguration(72, 72, 72);
                        playerImageSheet.frameTimeLimit = 8;
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
                        newPosition -= new Vector2(0, 6);
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
                            playerImageSheet.frameTimeLimit = 8;
                        }
                        else
                        {
                            if (isFalling)
                            {
                                playerImageSheet.setFrameConfiguration(72, 72, 72);
                                playerImageSheet.frameTimeLimit = 8;
                            }
                            else if (isFallingFromGravity)
                            {
                                playerImageSheet.setFrameConfiguration(73, 73, 73);
                                playerImageSheet.frameTimeLimit = 8;
                            }
                            else if (isJumping)
                            {
                                playerImageSheet.setFrameConfiguration(36, 36, 42);
                                playerImageSheet.frameTimeLimit = 6;
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

                newPosition += gravity;

                newPosition += momentum;
                momentum.X = (float)(momentum.X *.9);
                momentum.Y = (float)(momentum.Y*.9);

                return newPosition;
            }

        }
    }
}
