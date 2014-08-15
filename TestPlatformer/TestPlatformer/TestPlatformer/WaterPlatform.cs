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
    class WaterPlatform : Platform
    {
        public AnimatedImage topWater;

        public bool flipTopWaterCrest;
        //----------------------------------------------------------------------------------------------------------------------------------------------------------
        public WaterPlatform(ContentManager cm, SpriteBatch sb, GraphicsDeviceManager gdm, ImageLibrary il, Vector2 platSize, Vector2 pos, bool slidableOnL, bool slidableOnR, bool cOnLeft, bool cOnRight, bool cOnTop, bool cOnBottom)
            : base(cm, sb, gdm, il, platSize, pos, slidableOnL, slidableOnR, cOnLeft, cOnRight, cOnTop, cOnBottom) {
            topWater = il.waterCrestTile;
            topWater.setFrameConfiguration(0, 0, 1);
            topWater.frameTimeLimit = 15;

            completeOutsidePlatformImage = il.waterBodyTile;

            layerDepth = .25f;

            flipTopWaterCrest = false;
        }

        public override void Update(GameTime gameTime, Vector2 gravity)
        {
            topWater.Update(gameTime);
            
            updatePlatformState();
            updatePosition(gravity);
        }
    
        public override void DrawAltPosition(Vector2 altPos)
        {
            int xTracker = 0;
            int xPosition = (int)altPos.X;
            int yTracker = 0;
            int yPosition = (int)altPos.Y;

            //expects you to pass in the converted position variable
            yTracker = 0;
            yPosition = (int)altPos.Y;

            while (yTracker < platformSize.Y)
            {
                xTracker = 0;
                xPosition = (int)(altPos.X);

                while (xTracker < platformSize.X)
                {
                    if (yTracker == 0)
                    {
                        topWater.DrawAltPosition(new Vector2(xPosition, altPos.Y), flipTopWaterCrest, layerDepth);
                    }
                    else { 
                        //left
                        if (yTracker > 0 && yTracker < platformSize.Y - 1 && xTracker == 0)
                        {
                            completeOutsidePlatformImage.setFrameConfiguration(3, 3, 3);
                        }
                        //center
                        else if (yTracker > 0 && yTracker < platformSize.Y - 1 && xTracker > 0 && xTracker < platformSize.X - 1)
                        {
                            completeOutsidePlatformImage.setFrameConfiguration(4, 4, 4);
                        }
                        //right
                        else if (yTracker > 0 && yTracker < platformSize.Y - 1 && xTracker == platformSize.X - 1)
                        {
                            completeOutsidePlatformImage.setFrameConfiguration(5, 5, 5);
                        }
                        //bottom left
                        else if (yTracker == platformSize.Y - 1 && xTracker == 0)
                        {
                            completeOutsidePlatformImage.setFrameConfiguration(6, 6, 6);
                        }
                        //bottom
                        else if (yTracker == platformSize.Y - 1 && xTracker > 0 && xTracker < platformSize.X - 1)
                        {
                            completeOutsidePlatformImage.setFrameConfiguration(7, 7, 7);
                        }
                        //bottom right
                        else if (yTracker == platformSize.Y - 1 && xTracker == platformSize.X - 1)
                        {
                            completeOutsidePlatformImage.setFrameConfiguration(8, 8, 8);
                        }

                        completeOutsidePlatformImage.DrawAltPosition(new Vector2(xPosition, yPosition), false, layerDepth);
                    }
                    

                    xPosition += (int)completeOutsidePlatformImage.frameSize.X;
                    xTracker++;
                }
                yTracker++;
                yPosition += (int)(completeOutsidePlatformImage.frameSize.Y);
            }
        }

        //not really sure how I want to implement this, I need to refine a binary search for the correct placement....
        public override Vector2 checkAndFixPlatformCollision(Vector2 newPos, Player playerOne)
        {
            //------------
            //First phase is to check for a collision and to mark collisions accordingly
            // will operate on the basis that the values returned for the collision are against the players sides/top/bottom and the rectangle of the tile platform
            //------------

            WaterPlatform tp = this;

            //makes the current platform rectangle reference, not sure if this is done via call by reference
            Rectangle previousTileComparisionRect = new Rectangle(tp.boundingRectangle.X, tp.boundingRectangle.Y+30, tp.boundingRectangle.Width, tp.boundingRectangle.Height-30);
            Rectangle currentTileComparisionRect = new Rectangle(tp.boundingRectangle.X, tp.boundingRectangle.Y+30, tp.boundingRectangle.Width, tp.boundingRectangle.Height-30);

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

            //right platform hit, left side of playerOne hit
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
            if (playerOne.collisionOnPlayerBottom)
            {
                if (playerOne.collisionOnPlayerLeft)
                {
                    if (tpRightRectSide.X - playerLeftRectSide.X < playerBottomRectSide.Y - tpTopRectSide.Y)
                    {
                        playerOne.collisionOnPlayerBottom = false;
                    }
                    else
                    {
                        playerOne.collisionOnPlayerLeft = false;
                    }
                }
                if (playerOne.collisionOnPlayerRight)
                {
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
                    if (playerOne.isFalling || playerOne.isFallingFromGravity)
                    {
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
