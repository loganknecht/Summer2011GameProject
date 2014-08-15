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
    class TiledPlatform {
        //steps to call the update in are
        //1: updatePlatformState()
        //2: updatePosition()
        //these are to be called in the update function
        //06/22/2011 remember that the gravity vector will be replaced by an environment class that passes in the appropriate information
        //so that you can have the platform react to the environment.... if that's your thing
        public AnimatedImage imageSheet;
        SpriteBatch sbReference;
        ContentManager cmReference;
        GraphicsDeviceManager gdmReference;

        public Vector2 position;
        //tracks the length and width in the frame height and size, so 5x10 would mean 5*framewidth is 
        //the total pixel width of platform, and 10*frameheight is the total pixel height of platforam
        public Vector2 platformSize;

        public bool movingLeft;
        public bool movingRight;
        public bool movingUp;
        public bool movingDown;

        public bool collidableOnLeft;
        public bool collidableOnRight;
        public bool collidableOnTop;
        public bool collidableOnBottom;

        public bool notMoving;

        public bool facingLeft;
        public bool facingRight;

        //best name I could think for this, this represents the number that will modify the playerOne's velocity in both directions
        //... really though it's initially being used just for the x value so that we have ice, grass, concrete, and eventually 
        //implement a rail tile
        public Vector2 terrainModifier;

        public float layerDepth;

        public float rightBounding {
            get {
                return position.X + platformWidth;
            }
        }

        public float bottomBounding {
            get {
                return position.Y + platformHeight;
            }
        }


        //the actual width of the tile that is being wrapped to draw
        public float tileWidth {
            get {
                return imageSheet.frameSize.X;
            }
        }

        //the actual height of the tile that is being wrapped to draw
        public float tileHeight {
            get {
                return imageSheet.frameSize.Y;
            }
        }

        //gives the total tiles of the platform in the x direction times the tileWidth, thus the total width
        public float platformWidth {
            get {
                return tileWidth * platformSize.X;
            }
        }

        //gives the total tiles of the platform in the y direction times the tileHeight, thus the total height
        public float platformHeight {
            get {
                return tileHeight * platformSize.Y;
            }
        }
    
        /// 
        /// tile platform has to have a special bounding rectangle on account that its texture is image wrapped multiple times
        /// making it impossible to guarantee that the frame size it receives is an accurate representation of the game objects size
        /// to rectify this I just create the actual dimensions using the appropriate modifiers to account for the block width and height
        /// 
        public Rectangle boundingRectangle {
            get {
                Rectangle r = new Rectangle((int)position.X, (int)position.Y, (int)platformWidth, (int)platformHeight);
                return r;
            }
        }

        public TiledPlatform(ContentManager cm, SpriteBatch sb, GraphicsDeviceManager gdm, bool cOnLeft, bool cOnRight, bool cOnTop, bool cOnBottom)
        {
            //configures the image sheet associated with the playerOne
            imageSheet = new AnimatedImage(cm, sb, gdm.GraphicsDevice, "PlatformTextures/Square", new Vector2(16, 16), new Vector2(1, 1));
            imageSheet.frameTimeLimit = 8;
            imageSheet.setFrameConfiguration(0, 0, 0);
            //imageSheet.isAnimating = false;

            //sets the references to the spritebatch and content manager so this class can define items in the content pipeline and draw to the same area
            sbReference = sb;
            cmReference = cm;
            gdmReference = gdm;

            //environment variables
            position = Vector2.Zero;
            platformSize = new Vector2(1, 1);

            //will multiply the playerOne's velocity by the corresponding vectors
            terrainModifier = new Vector2(1.0f, 1.0f);

            movingLeft = false;
            movingRight = false;
            movingUp = false;
            movingDown = false;

            collidableOnLeft = cOnLeft;
            collidableOnRight = cOnRight;
            collidableOnTop = cOnTop;
            collidableOnBottom = cOnBottom;

            //equivilant of the playerOne class is the boolean "standing", basically default state
            notMoving = true;

            //Pretty sure I won't need these and they'll be deprecated for the platform
            facingLeft = false;
            facingRight = true;

            layerDepth = .75f;
        }
        //----------------------------------------------------------------------------------------------------------------------------------------------------------
        public TiledPlatform(ContentManager cm, SpriteBatch sb, GraphicsDeviceManager gdm, Vector2 platSize, Vector2 pos, bool cOnLeft, bool cOnRight, bool cOnTop, bool cOnBottom)
        {
            //configures the image sheet associated with the playerOne
            imageSheet = new AnimatedImage(cm, sb, gdm.GraphicsDevice, "PlatformTextures/Square", new Vector2(16, 16), new Vector2(1, 1));
            imageSheet.frameTimeLimit = 8;
            imageSheet.setFrameConfiguration(0, 0, 0);
            //imageSheet.isAnimating = false;

            //sets the references to the spritebatch and content manager so this class can define items in the content pipeline and draw to the same area
            sbReference = sb;
            cmReference = cm;
            gdmReference = gdm;

            //environment variables
            position = Vector2.Zero;
            platformSize = new Vector2(1, 1);

            //will multiply the playerOne's velocity by the corresponding vectors
            terrainModifier = new Vector2(1.0f, 1.0f);

            movingLeft = false;
            movingRight = false;
            movingUp = false;
            movingDown = false;

            collidableOnLeft = cOnLeft;
            collidableOnRight = cOnRight;
            collidableOnTop = cOnTop;
            collidableOnBottom = cOnBottom;

            //equivilant of the playerOne class is the boolean "standing", basically default state
            notMoving = true;

            //Pretty sure I won't need these and they'll be deprecated for the platform
            facingLeft = false;
            facingRight = true;

            platformSize = platSize;
            position = pos;
        }
        //----------------------------------------------------------------------------------------------------------------------------------------------------------

        //this is where you will configure the platform's length and width
        public void Initialize() {
        }

        public void Update(GameTime gameTime, Vector2 gravity) {
            updatePlatformState();
            updatePosition(gravity);
            imageSheet.position = position;
            imageSheet.Update(gameTime);
        }

        public void DrawAltPosition(Vector2 altPos)
        {
            imageSheet.DrawAltPosition(altPos, false, platformSize, layerDepth);
        }
        //The playerOne equivilant of this would be the check for input method, however rarely will a platform interact with someone
        //in a manner that wouldn't require an extended or new class altogether, that said this is to update state changes that are 
        //caused by the environment playerOne some how, and to subsequently handle any... miscellaneous state information that may not
        //need to be handled in the update position method
        public void updatePlatformState() {
            if (movingLeft ||
                movingRight ||
                movingUp ||
                movingDown) {
                notMoving = false;
            }
            else {
                notMoving = true;
            }
        }
        public void updatePosition(Vector2 gravity) {
            //adds effects of the environment
            position.Y += gravity.Y;
            //side steps gravity because it's a god damn platform
            //this may be deprecated later, more of a nuance of thought than needed
            //plus this is like constant time so reehh
            position.Y -= gravity.Y;

        }

        //not really sure how I want to implement this, I need to refine a binary search for the correct placement....
        public Vector2 checkAndFixTiledPlatformCollision(Vector2 newPos, Player playerOne)
        {
            //------------
            //First phase is to check for a collision and to mark collisions accordingly
            // will operate on the basis that the values returned for the collision are against the players sides/top/bottom and the rectangle of the tile platform
            //------------

            TiledPlatform tp = this;

            //makes the current tileplatform rectangle reference, not sure if this is done via call by reference
            Rectangle previousTileComparisionRect = tp.boundingRectangle;
            Rectangle currentTileComparisionRect = tp.boundingRectangle;
            //playerOne left side
            Rectangle tpLeftRectSide = new Rectangle(currentTileComparisionRect.X, currentTileComparisionRect.Y, 1, currentTileComparisionRect.Height);
            //playerOne right side, has a -1 offset to the x value so that it's overlapping correctly for comparison
            Rectangle tpRightRectSide = new Rectangle(currentTileComparisionRect.Right, currentTileComparisionRect.Y, 1, currentTileComparisionRect.Height);
            //playerOne top side
            Rectangle tpTopRectSide = new Rectangle(currentTileComparisionRect.X, currentTileComparisionRect.Y, currentTileComparisionRect.Width, 1);
            //playerOne bottom side, has a -1 offset to the y value so that it's overlapping correctly for comparison
            Rectangle tpBottomRectSide = new Rectangle(currentTileComparisionRect.X, currentTileComparisionRect.Bottom, currentTileComparisionRect.Width, 1);

            //makes playerOne rectangle reference
            Rectangle previousPlayerComparisionRect = new Rectangle((int)newPos.X, (int)newPos.Y, (int)playerOne.width, (int)playerOne.height);
            Rectangle currentPlayerComparisionRect = new Rectangle((int)newPos.X, (int)newPos.Y, (int)playerOne.width, (int)playerOne.height);
            //playerOne left side
            Rectangle playerLeftRectSide = new Rectangle(currentPlayerComparisionRect.X, currentPlayerComparisionRect.Y, 1, currentPlayerComparisionRect.Height);
            //playerOne right side, has a -1 offset to the x value so that it's overlapping correctly for comparison
            Rectangle playerRightRectSide = new Rectangle(currentPlayerComparisionRect.Right, currentPlayerComparisionRect.Y, 1, currentPlayerComparisionRect.Height);
            //playerOne top side
            Rectangle playerTopRectSide = new Rectangle(currentPlayerComparisionRect.X, currentPlayerComparisionRect.Y, currentPlayerComparisionRect.Width, 1);
            //playerOne bottom side, has a -1 offset to the y value so that it's overlapping correctly for comparison
            Rectangle playerBottomRectSide = new Rectangle(currentPlayerComparisionRect.X, currentPlayerComparisionRect.Bottom, currentPlayerComparisionRect.Width, 1);
            //System.Diagnostics.Debug.WriteLine("Current Collision Box X: " + currentPlayerComparisionRect.X);
            //System.Diagnostics.Debug.WriteLine("Current Collision Box Y: " + currentPlayerComparisionRect.Y);

            //System.Diagnostics.Debug.WriteLine("Current Collision Box X: " + currentTileComparisionRect.X);
            //System.Diagnostics.Debug.WriteLine("Current Collision Box Y: " + currentTileComparisionRect.Y);

            //right tileplatform hit, left side of playerOne hit
            if (currentPlayerComparisionRect.Intersects(tpRightRectSide) && collidableOnRight)
            {
                playerOne.collisionOnPlayerLeft = true;
                //System.Diagnostics.Debug.WriteLine("Left Collision");
            }
            //left tile platform hit, right side of playerOne hit
            if (currentPlayerComparisionRect.Intersects(tpLeftRectSide) && collidableOnLeft)
            {
                playerOne.collisionOnPlayerRight = true;
                //System.Diagnostics.Debug.WriteLine("Right Collision");
            }
            //bottom of platform hit, top of playerOne hit
            if (currentPlayerComparisionRect.Intersects(tpBottomRectSide) && collidableOnBottom)
            {
                playerOne.collisionOnPlayerTop = true;
                //System.Diagnostics.Debug.WriteLine("Top Collision");
            }
            //top of platform hit, bottom of playerOne hit
            //this is where dash and jump and falling are reset
            if (currentPlayerComparisionRect.Intersects(tpTopRectSide) && collidableOnTop)
            {
                playerOne.collisionOnPlayerBottom = true;
                //System.Diagnostics.Debug.WriteLine("Bottom Collision");
            }
            //*****
            //I'm pretty positive that this may be a problem in the future. I don't know.... I've never implemented collision like this...
            //this nested if statement basically says don't fix any collision on bottom if jumping
            //I'm pretty sure this will mess me up later, but at the moment I'm letting it slide because wow, it really fixes the
            //controls, if you can in the future fix this to work around any bugs. Try to keep this for the logic.
            if (playerOne.collisionOnPlayerBottom && playerOne.isJumping)
            {
                playerOne.collisionOnPlayerBottom = false;
            }

            //this is to fix landing collision so that if there is a bottom collision as long as you're not falling left and right are false
            if (playerOne.collisionOnPlayerBottom) {
                if (playerOne.collisionOnPlayerLeft) {
                    if (tpRightRectSide.X - playerLeftRectSide.X < playerBottomRectSide.Y - tpTopRectSide.Y)
                    {
                        playerOne.collisionOnPlayerBottom = false;
                    }
                    else
                    {
                        playerOne.collisionOnPlayerLeft = false;
                    }
                }
                if (playerOne.collisionOnPlayerRight) {
                    if (playerRightRectSide.X - tpLeftRectSide.X < playerBottomRectSide.Y - tpTopRectSide.Y)
                    {
                        playerOne.collisionOnPlayerBottom = false;
                    }
                    else
                    {
                        playerOne.collisionOnPlayerRight = false;
                    }
                }
            }

            //top collision
            if (playerOne.collisionOnPlayerTop)
            {
                if (playerOne.collisionOnPlayerLeft)
                {
                    if (tpRightRectSide.X - playerLeftRectSide.X < tpBottomRectSide.Y - playerTopRectSide.Y)
                    {
                        playerOne.collisionOnPlayerTop = false;
                    }
                    else
                    {
                        playerOne.collisionOnPlayerLeft = false;
                    }
                }
                if (playerOne.collisionOnPlayerRight)
                {
                    if (playerRightRectSide.X - tpLeftRectSide.X < tpBottomRectSide.Y - playerTopRectSide.Y)
                    {
                        playerOne.collisionOnPlayerBottom = false;
                    }
                    else
                    {
                        playerOne.collisionOnPlayerRight = false;
                    }
                }
            }

            //if any collision occured then update the flag so that a general marker is known
            if (playerOne.collisionOnPlayerLeft ||
                playerOne.collisionOnPlayerRight ||
                playerOne.collisionOnPlayerTop ||
                playerOne.collisionOnPlayerBottom)
            {
                playerOne.playerCollisionOccurred = true;
            }

            //------------
            //second phase is to respond accordingly
            //------------

            if (playerOne.playerCollisionOccurred)
            {
                //left was hit
                if (playerOne.collisionOnPlayerLeft)
                {
                    while (currentPlayerComparisionRect.Intersects(tpRightRectSide))
                    {
                        ///
                        /// resets the values hopefully using a call by value here to that we're getting the numbers and not the reference to previous position
                        /// This is an important nuance on account that it makes it so all our collision checking is done through rectangled abstraction. This means
                        /// we're just using the values to initialze the rectangles and then updating the collision from there, at the end when the fix is to exit the
                        /// rectangle will set the playerOne position appropriately
                        /// 
                        float widthMidpointDistance = (previousPlayerComparisionRect.X - tpRightRectSide.X) / 2;

                        //update previous and current rectangle reference before modifying their values
                        previousPlayerComparisionRect = currentPlayerComparisionRect;
                        currentPlayerComparisionRect.X -= (int)widthMidpointDistance;

                        playerLeftRectSide.Location = currentPlayerComparisionRect.Location;

                        //checks to see if the midpoint modifying the position causes it to hit the tile platform
                        //if it does the rectangle is set to the previous rectangle and 
                        if (currentPlayerComparisionRect.Intersects(tpRightRectSide))
                        {
                            currentPlayerComparisionRect.X = currentTileComparisionRect.Right + (int)widthMidpointDistance + 1;
                            playerLeftRectSide.Location = currentPlayerComparisionRect.Location;
                        }
                    }
                    newPos.X = currentPlayerComparisionRect.X;
                }
                //right was hit
                //pretty questionable when it feels like it seems to zoom through the block, i think I've rectified the issue but the dash
                //causes it to look a little more aggressive on its position modification.... possible bug here
                if (playerOne.collisionOnPlayerRight)
                {
                    while (currentPlayerComparisionRect.Intersects(tpLeftRectSide))
                    {
                        ///
                        /// resets the values hopefully using a call by value here to that we're getting the numbers and not the reference to previous position
                        /// This is an important nuance on account that it makes it so all our collision checking is done through rectangled abstraction. This means
                        /// we're just using the values to initialze the rectangles and then updating the collision from there, at the end when the fix is to exit the
                        /// rectangle will set the playerOne position appropriately
                        /// 
                        float widthMidpointDistance = (tpLeftRectSide.X - previousPlayerComparisionRect.Right) / 2;

                        //update previous and current rectangle reference before modifying their values
                        previousPlayerComparisionRect = currentPlayerComparisionRect;
                        currentPlayerComparisionRect.X += (int)widthMidpointDistance;

                        playerRightRectSide.X = currentPlayerComparisionRect.Right;

                        //checks to see if the midpoint modifying the position causes it to hit the tile platform
                        //if it does the rectangle is set to the previous rectangle and 
                        if (currentPlayerComparisionRect.Intersects(tpLeftRectSide))
                        {
                            currentPlayerComparisionRect.X = currentTileComparisionRect.Left - (int)widthMidpointDistance - currentPlayerComparisionRect.Width - 1;
                            playerRightRectSide.X = currentPlayerComparisionRect.Right;
                        }
                    }
                    newPos.X = currentPlayerComparisionRect.X;
                }
                //top was hit
                if (playerOne.collisionOnPlayerTop)
                {

                    while (currentPlayerComparisionRect.Intersects(tpBottomRectSide))
                    {
                        ///
                        /// resets the values hopefully using a call by value here to that we're getting the numbers and not the reference to previous position
                        /// This is an important nuance on account that it makes it so all our collision checking is done through rectangled abstraction. This means
                        /// we're just using the values to initialze the rectangles and then updating the collision from there, at the end when the fix is to exit the
                        /// rectangle will set the playerOne position appropriately
                        /// 

                        float heightMidpointDistance = (currentPlayerComparisionRect.Top - tpBottomRectSide.Y) / 2;

                        //update previous and current rectangle reference before modifying their values
                        previousPlayerComparisionRect = currentPlayerComparisionRect;
                        currentPlayerComparisionRect.Y += (int)heightMidpointDistance;

                        playerTopRectSide.Y = currentPlayerComparisionRect.Top;

                        //checks to see if the midpoint modifying the position causes it to hit the tile platform
                        //if it does the rectangle is set to the previous rectangle and 
                        if (currentPlayerComparisionRect.Intersects(tpBottomRectSide))
                        {
                            currentPlayerComparisionRect.Y = currentTileComparisionRect.Bottom + (int)heightMidpointDistance + 1;
                            playerTopRectSide.Y = currentPlayerComparisionRect.Top;
                        }
                    }
                    newPos.Y = currentPlayerComparisionRect.Y;
                    //playerOne.isJumping = false;
                    //playerOne.isFalling = true;
                }

                //bottom was hit
                if (playerOne.collisionOnPlayerBottom)
                {
                    while (currentPlayerComparisionRect.Intersects(tpTopRectSide))
                    {
                        ///
                        /// resets the values hopefully using a call by value here to that we're getting the numbers and not the reference to previous position
                        /// This is an important nuance on account that it makes it so all our collision checking is done through rectangled abstraction. This means
                        /// we're just using the values to initialze the rectangles and then updating the collision from there, at the end when the fix is to exit the
                        /// rectangle will set the playerOne position appropriately
                        /// 

                        float heightMidpointDistance = (tpTopRectSide.Y - playerBottomRectSide.Y) / 2;

                        //update previous and current rectangle reference before modifying their values
                        previousPlayerComparisionRect = currentPlayerComparisionRect;
                        currentPlayerComparisionRect.Y += (int)heightMidpointDistance;

                        playerBottomRectSide.Y = currentPlayerComparisionRect.Bottom;

                        //checks to see if the midpoint modifying the position causes it to hit the tile platform
                        //if it does the rectangle is set to the previous rectangle and 
                        if (currentPlayerComparisionRect.Intersects(tpTopRectSide))
                        {
                            currentPlayerComparisionRect.Y = currentTileComparisionRect.Y - (int)heightMidpointDistance - (int)playerOne.height - 2;
                            playerBottomRectSide.Y = currentPlayerComparisionRect.Bottom;
                        }
                    }
                    
                    //fixes falling collision for gravity falls with platform
                    if (playerOne.isFalling || playerOne.isFallingFromGravity) {
                        if (playerOne.isFalling)
                        {
                            playerOne.isFalling = false;
                            if (playerOne.movingRight || playerOne.movingLeft)
                            {
                                playerOne.playerImageSheet.setFrameConfiguration(18, 18, 28);
                                playerOne.playerImageSheet.frameTimeLimit = 8;
                            }
                        }
                        if (playerOne.isFallingFromGravity)
                        {
                            playerOne.isFallingFromGravity = false;
                            if (playerOne.movingRight || playerOne.movingLeft)
                            {
                                playerOne.playerImageSheet.setFrameConfiguration(18, 18, 28);
                                playerOne.playerImageSheet.frameTimeLimit = 8;
                            }
                            else if (playerOne.standing)
                            {
                                playerOne.playerImageSheet.setFrameConfiguration(0, 0, 4);
                                playerOne.playerImageSheet.frameTimeLimit = 8;
                            }
                        }
                        if (playerOne.exhaustedDash)
                        {
                            playerOne.exhaustedDash = false;
                        }
                    }

                    newPos.Y = currentPlayerComparisionRect.Y;
                    playerOne.isJumping = false;
                    playerOne.isFalling = false;
                    playerOne.isFallingFromGravity = false;
                    playerOne.isDashing = false;
                    playerOne.exhaustedDash = false;
                }

                if (playerOne.playerCollisionOccurred)
                {
                    playerOne.playerCollisionOccurred = false;
                    playerOne.collisionOnPlayerLeft = false;
                    playerOne.collisionOnPlayerRight = false;
                    playerOne.collisionOnPlayerTop = false;
                    playerOne.collisionOnPlayerBottom = false;
                }

                return newPos;
            }
            else
            {
                if (playerOne.playerCollisionOccurred)
                {
                    playerOne.playerCollisionOccurred = false;
                    playerOne.collisionOnPlayerLeft = false;
                    playerOne.collisionOnPlayerRight = false;
                    playerOne.collisionOnPlayerTop = false;
                    playerOne.collisionOnPlayerBottom = false;
                }

                return newPos;
            }

            //TO DO: finish the collision code so that it does corners and sides with out issues            

            //------------
            //third phase is to reset flags
            //------------

        }
    }
}
