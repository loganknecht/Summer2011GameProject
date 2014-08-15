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
    class ContainerPlatform : Platform
    {
        //tenatively completed
        public AnimatedImage completeInsideImage;

        public bool playerIsContained;

        public ContainerPlatform(ContentManager cm, SpriteBatch sb, GraphicsDeviceManager gdm, ImageLibrary il, Vector2 platSize, Vector2 pos, bool slidableOnL, bool slidableOnR, bool cOnLeft, bool cOnRight, bool cOnTop, bool cOnBottom)
            : base(cm, sb, gdm, il, platSize, pos, slidableOnL, slidableOnR, cOnLeft, cOnRight, cOnTop, cOnBottom)
        {
            //Outside image
            completeOutsidePlatformImage = il.smwOutsideGrass;
            completeInsideImage = il.completeInsideGrassPlatform;

            playerIsContained = true;
        }

        public override void DrawAltPosition(Vector2 altPos)
        {
            if (platformSize.X < 3 || platformSize.Y < 3)
            {
            }
            else
            {
                if (playerIsContained)
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
                            //top left
                            if (xTracker == 0 && yTracker == 0)
                            {
                                completeInsideImage.setFrameConfiguration(0, 0, 0);
                            }
                            //top
                            else if (yTracker == 0 && xTracker > 0 && xTracker < platformSize.X - 1)
                            {
                                completeInsideImage.setFrameConfiguration(1, 1, 1);
                            }
                            //top right
                            else if (yTracker == 0 && xTracker == platformSize.X - 1)
                            {
                                completeInsideImage.setFrameConfiguration(2, 2, 2);
                            }
                            //left
                            else if (yTracker > 0 && yTracker < platformSize.Y - 1 && xTracker == 0)
                            {
                                completeInsideImage.setFrameConfiguration(3, 3, 3);
                            }
                            //center
                            else if (yTracker > 0 && yTracker < platformSize.Y - 1 && xTracker > 0 && xTracker < platformSize.X - 1)
                            {
                                completeInsideImage.setFrameConfiguration(4, 4, 4);
                            }
                            //right
                            else if (yTracker > 0 && yTracker < platformSize.Y - 1 && xTracker == platformSize.X - 1)
                            {
                                completeInsideImage.setFrameConfiguration(5, 5, 5);
                            }
                            //bottom left
                            else if (yTracker == platformSize.Y - 1 && xTracker == 0)
                            {
                                completeInsideImage.setFrameConfiguration(6, 6, 6);
                            }
                            //bottom
                            else if (yTracker == platformSize.Y - 1 && xTracker > 0 && xTracker < platformSize.X - 1)
                            {
                                completeInsideImage.setFrameConfiguration(7, 7, 7);
                            }
                            //bottom right
                            else if (yTracker == platformSize.Y - 1 && xTracker == platformSize.X - 1)
                            {
                                completeInsideImage.setFrameConfiguration(8, 8, 8);
                            }

                            completeInsideImage.DrawAltPosition(new Vector2(xPosition, yPosition), false, layerDepth);

                            xPosition += (int)completeInsideImage.frameSize.X;
                            xTracker++;
                        }
                        yTracker++;
                        yPosition += (int)(completeInsideImage.frameSize.Y);
                    }
                }
                else {
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
                            //top left
                            if (xTracker == 0 && yTracker == 0)
                            {
                                completeOutsidePlatformImage.setFrameConfiguration(0, 0, 0);
                            }
                            //top
                            else if (yTracker == 0 && xTracker > 0 && xTracker < platformSize.X - 1)
                            {
                                completeOutsidePlatformImage.setFrameConfiguration(1, 1, 1);
                            }
                            //top right
                            else if (yTracker == 0 && xTracker == platformSize.X - 1)
                            {
                                completeOutsidePlatformImage.setFrameConfiguration(2, 2, 2);
                            }
                            //left
                            else if (yTracker > 0 && yTracker < platformSize.Y - 1 && xTracker == 0)
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

                            xPosition += (int)completeOutsidePlatformImage.frameSize.X;
                            xTracker++;
                        }
                        yTracker++;
                        yPosition += (int)(completeOutsidePlatformImage.frameSize.Y);
                    }
                }
            }
        }

        //not really sure how I want to implement this, I need to refine a binary search for the correct placement....
        public override Vector2 checkAndFixPlatformCollision(Vector2 newPos, Player playerOne)    {
            if (boundingRectangle.Intersects(playerOne.boundingRectangle)) {
                playerIsContained = true;
            }
            else {
                playerIsContained = false;
            }

            //------------
            //First phase is to check for a collision and to mark collisions accordingly
            // will operate on the basis that the values returned for the collision are against the players sides/top/bottom and the rectangle of the tile platform
            //------------

            ContainerPlatform tp = this;

            //makes the current platform rectangle reference, not sure if this is done via call by reference
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

            //right platform hit, left side of playerOne hit
            if (currentPlayerComparisionRect.Intersects(tpRightRectSide) && collidableOnRight)
            {
                if (currentPlayerComparisionRect.Left < tpRightRectSide.X + 20 && currentPlayerComparisionRect.Left > tpRightRectSide.X - 20)
                {
                    playerOne.collisionOnPlayerLeft = true;
                }
                else if (currentPlayerComparisionRect.Right < tpRightRectSide.X + 20 && currentPlayerComparisionRect.Right > tpRightRectSide.X - 20)
                {
                    playerOne.collisionOnPlayerRight = true;
                }
                //System.Diagnostics.Debug.WriteLine("Left Collision");
            }
            //left tile platform hit, right side of playerOne hit
            if (currentPlayerComparisionRect.Intersects(tpLeftRectSide) && collidableOnLeft)
            {
                if (currentPlayerComparisionRect.Left < tpLeftRectSide.X + 20 && currentPlayerComparisionRect.Left > tpLeftRectSide.X - 20)
                {
                    playerOne.collisionOnPlayerLeft = true;
                }
                else if (currentPlayerComparisionRect.Right < tpLeftRectSide.X + 20 && currentPlayerComparisionRect.Right > tpLeftRectSide.X - 20)
                {
                    playerOne.collisionOnPlayerRight = true;
                }
                //System.Diagnostics.Debug.WriteLine("Right Collision");
            }
            //bottom of platform hit, top of playerOne hit
            if (currentPlayerComparisionRect.Intersects(tpBottomRectSide) && collidableOnBottom)
            {
                //if the bottom bounding of the playerOne isn't within 40 pixels of the top of the platform the collision doesn't occur for this type of block
                if (currentPlayerComparisionRect.Bottom < tpBottomRectSide.Y + 20 && currentPlayerComparisionRect.Bottom > tpBottomRectSide.Y - 20)
                {
                    playerOne.collisionOnPlayerBottom = true;
                }
                else if (currentPlayerComparisionRect.Top < tpBottomRectSide.Y + 20 && currentPlayerComparisionRect.Top > tpBottomRectSide.Y - 20)
                {
                    //it's false
                    playerOne.collisionOnPlayerTop = true;
                    //System.Diagnostics.Debug.WriteLine("Top Collision");
                }  
                
            }
            //top of platform hit, bottom of playerOne hit
            if (currentPlayerComparisionRect.Intersects(tpTopRectSide) && collidableOnTop)
            {
                //if the bottom bounding of the playerOne isn't within 40 pixels of the top of the platform the collision doesn't occur for this type of block
                if (currentPlayerComparisionRect.Bottom < tpTopRectSide.Y + 20 && currentPlayerComparisionRect.Bottom > tpTopRectSide.Y - 20)
                {
                    playerOne.collisionOnPlayerBottom = true;
                }
                else if (currentPlayerComparisionRect.Top < tpTopRectSide.Y + 20 && currentPlayerComparisionRect.Top > tpTopRectSide.Y - 20)
                {
                    //it's false
                    playerOne.collisionOnPlayerTop = true;
                }   
                //System.Diagnostics.Debug.WriteLine("Bottom Collision");
            }

            if (!boundingRectangle.Contains(playerOne.boundingRectangle))
            {
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
                    if(currentPlayerComparisionRect.Intersects(tpRightRectSide)) {                    
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
                    else if (currentPlayerComparisionRect.Intersects(tpLeftRectSide)) { 
                        while (currentPlayerComparisionRect.Intersects(tpLeftRectSide))
                        {
                            ///
                            /// resets the values hopefully using a call by value here to that we're getting the numbers and not the reference to previous position
                            /// This is an important nuance on account that it makes it so all our collision checking is done through rectangled abstraction. This means
                            /// we're just using the values to initialze the rectangles and then updating the collision from there, at the end when the fix is to exit the
                            /// rectangle will set the playerOne position appropriately
                            /// 
                            float widthMidpointDistance = (tpLeftRectSide.X - previousPlayerComparisionRect.Left) / 2;

                            //update previous and current rectangle reference before modifying their values
                            previousPlayerComparisionRect = currentPlayerComparisionRect;
                            currentPlayerComparisionRect.X += (int)widthMidpointDistance;

                            playerLeftRectSide.X = currentPlayerComparisionRect.Left;

                            //checks to see if the midpoint modifying the position causes it to hit the tile platform
                            //if it does the rectangle is set to the previous rectangle and 
                            if (currentPlayerComparisionRect.Intersects(tpLeftRectSide))
                            {
                                currentPlayerComparisionRect.X = currentTileComparisionRect.Left + (int)widthMidpointDistance + 1;
                                playerLeftRectSide.X = currentPlayerComparisionRect.Left;
                            }
                        }
                        newPos.X = currentPlayerComparisionRect.X;
                    }
                }
                //right was hit
                //pretty questionable when it feels like it seems to zoom through the block, i think I've rectified the issue but the dash
                //causes it to look a little more aggressive on its position modification.... possible bug here
                else if (playerOne.collisionOnPlayerRight)
                {
                    if (currentPlayerComparisionRect.Intersects(tpLeftRectSide))
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
                    else if (currentPlayerComparisionRect.Intersects(tpRightRectSide)) {
                        while (currentPlayerComparisionRect.Intersects(tpRightRectSide))
                        {
                            ///
                            /// resets the values hopefully using a call by value here to that we're getting the numbers and not the reference to previous position
                            /// This is an important nuance on account that it makes it so all our collision checking is done through rectangled abstraction. This means
                            /// we're just using the values to initialze the rectangles and then updating the collision from there, at the end when the fix is to exit the
                            /// rectangle will set the playerOne position appropriately
                            /// 
                            float widthMidpointDistance = (tpRightRectSide.X - previousPlayerComparisionRect.Right) / 2;

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
                }
                //top was hit
                else if (playerOne.collisionOnPlayerTop)
                {   
                    //System.Diagnostics.Debug.WriteLine("top hit");
                    if (currentPlayerComparisionRect.Intersects(tpBottomRectSide))
                    {
                        while (currentPlayerComparisionRect.Intersects(tpBottomRectSide))
                        {
                            ///
                            /// resets the values hopefully using a call by value here to that we're getting the numbers and not the reference to previous position
                            /// This is an important nuance on account that it makes it so all our collision checking is done through rectangled abstraction. This means
                            /// we're just using the values to initialze the rectangles and then updating the collision from there, at the end when the fix is to exit the
                            /// rectangle will set the playerOne position appropriately
                            /// 

                            float heightMidpointDistance = (tpBottomRectSide.Y - currentPlayerComparisionRect.Top) / 2;
                            //System.Diagnostics.Debug.WriteLine(heightMidpointDistance);

                            //update previous and current rectangle reference before modifying their values
                            previousPlayerComparisionRect = currentPlayerComparisionRect;
                            currentPlayerComparisionRect.Y -= (int)heightMidpointDistance;

                            playerTopRectSide.Y = currentPlayerComparisionRect.Top;

                            //checks to see if the midpoint modifying the position causes it to hit the tile platform
                            //if it does the rectangle is set to the previous rectangle and 
                            if (currentPlayerComparisionRect.Intersects(tpBottomRectSide))
                            {
                                currentPlayerComparisionRect.Y = currentTileComparisionRect.Bottom - (int)heightMidpointDistance + 1;
                                playerTopRectSide.Y = currentPlayerComparisionRect.Top;
                            }
                        }                        
                        newPos.Y = currentPlayerComparisionRect.Y;
                    }
                    else if (currentPlayerComparisionRect.Intersects(tpTopRectSide)) {
                        while (currentPlayerComparisionRect.Intersects(tpTopRectSide))
                        {
                            ///
                            /// resets the values hopefully using a call by value here to that we're getting the numbers and not the reference to previous position
                            /// This is an important nuance on account that it makes it so all our collision checking is done through rectangled abstraction. This means
                            /// we're just using the values to initialze the rectangles and then updating the collision from there, at the end when the fix is to exit the
                            /// rectangle will set the playerOne position appropriately
                            /// 
                            float widthMidpointDistance = (tpTopRectSide.Y - previousPlayerComparisionRect.Top) / 2;

                            //update previous and current rectangle reference before modifying their values
                            previousPlayerComparisionRect = currentPlayerComparisionRect;
                            currentPlayerComparisionRect.Y += (int)widthMidpointDistance;

                            playerTopRectSide.Y = currentPlayerComparisionRect.Top;

                            //checks to see if the midpoint modifying the position causes it to hit the tile platform
                            //if it does the rectangle is set to the previous rectangle and 
                            if (currentPlayerComparisionRect.Intersects(tpTopRectSide))
                            {
                                currentPlayerComparisionRect.Y = currentTileComparisionRect.Top + (int)widthMidpointDistance + 1;
                                playerTopRectSide.Y = currentPlayerComparisionRect.Top;
                            }
                        }
                        newPos.Y = currentPlayerComparisionRect.Y;
                    }
                    playerOne.isJumping = false;
                    playerOne.isFalling = true;
                    playerOne.playerImageSheet.setFrameConfiguration(72, 72, 72);
                    playerOne.playerImageSheet.frameTimeLimit = playerOne.defaultFrameTimeLimit;
                }

                //bottom was hit
                else if (playerOne.collisionOnPlayerBottom)
                {
                    if (currentPlayerComparisionRect.Intersects(tpTopRectSide))
                    {
                        while (currentPlayerComparisionRect.Intersects(tpTopRectSide))
                        {
                            ///
                            /// resets the values hopefully using a call by value here to that we're getting the numbers and not the reference to previous position
                            /// This is an important nuance on account that it makes it so all our collision checking is done through rectangled abstraction. This means
                            /// we're just using the values to initialze the rectangles and then updating the collision from there, at the end when the fix is to exit the
                            /// rectangle will set the playerOne position appropriately
                            /// 
                            float widthMidpointDistance = (tpTopRectSide.Y - previousPlayerComparisionRect.Bottom) / 2;

                            //update previous and current rectangle reference before modifying their values
                            previousPlayerComparisionRect = currentPlayerComparisionRect;
                            currentPlayerComparisionRect.Y += (int)widthMidpointDistance;

                            playerBottomRectSide.Y = currentPlayerComparisionRect.Bottom;

                            //checks to see if the midpoint modifying the position causes it to hit the tile platform
                            //if it does the rectangle is set to the previous rectangle and 
                            if (currentPlayerComparisionRect.Intersects(tpTopRectSide))
                            {
                                currentPlayerComparisionRect.Y = currentTileComparisionRect.Top - (int)widthMidpointDistance - currentPlayerComparisionRect.Height - 1;
                                playerBottomRectSide.Y = currentPlayerComparisionRect.Bottom;
                            }
                        }
                        newPos.Y = currentPlayerComparisionRect.Y;
                    }
                    else if (currentPlayerComparisionRect.Intersects(tpBottomRectSide))
                    {
                        while (currentPlayerComparisionRect.Intersects(tpBottomRectSide))
                        {
                            ///
                            /// resets the values hopefully using a call by value here to that we're getting the numbers and not the reference to previous position
                            /// This is an important nuance on account that it makes it so all our collision checking is done through rectangled abstraction. This means
                            /// we're just using the values to initialze the rectangles and then updating the collision from there, at the end when the fix is to exit the
                            /// rectangle will set the playerOne position appropriately
                            /// 
                            float widthMidpointDistance = (tpBottomRectSide.Y - previousPlayerComparisionRect.Bottom) / 2;
                            if (widthMidpointDistance > 0) {
                                widthMidpointDistance = widthMidpointDistance * -1;
                            }
                            else if (widthMidpointDistance == 0) {
                                widthMidpointDistance = -1;
                            }
                            //System.Diagnostics.Debug.WriteLine(widthMidpointDistance);

                            //update previous and current rectangle reference before modifying their values
                            previousPlayerComparisionRect = currentPlayerComparisionRect;
                            currentPlayerComparisionRect.Y += (int)widthMidpointDistance;

                            playerBottomRectSide.Y = currentPlayerComparisionRect.Bottom;

                            //checks to see if the midpoint modifying the position causes it to hit the tile platform
                            //if it does the rectangle is set to the previous rectangle and 
                            if (currentPlayerComparisionRect.Intersects(tpTopRectSide))
                            {
                                currentPlayerComparisionRect.Y = currentTileComparisionRect.Bottom - (int)widthMidpointDistance - currentPlayerComparisionRect.Height - 1;
                                playerBottomRectSide.Y = currentPlayerComparisionRect.Bottom;
                            }
                        }
                        newPos.Y = currentPlayerComparisionRect.Y;
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
                                playerOne.playerImageSheet.frameTimeLimit = playerOne.defaultFrameTimeLimit;
                            }
                        }
                        if (playerOne.isFallingFromGravity)
                        {
                            playerOne.isFallingFromGravity = false;
                            if (playerOne.movingRight || playerOne.movingLeft)
                            {
                                playerOne.playerImageSheet.setFrameConfiguration(18, 18, 28);
                                playerOne.playerImageSheet.frameTimeLimit = playerOne.defaultFrameTimeLimit;
                            }
                            else if (playerOne.standing)
                            {
                                playerOne.playerImageSheet.setFrameConfiguration(0, 0, 4);
                                playerOne.playerImageSheet.frameTimeLimit = playerOne.defaultFrameTimeLimit;
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
                return checkAndFixPlatformCollision(newPos, playerOne); ;
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
        }
    }
}
