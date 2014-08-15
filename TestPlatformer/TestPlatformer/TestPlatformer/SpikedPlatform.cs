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
    class SpikedPlatform : Platform
    {
        AnimatedImage spikedPlatformImage;

        bool spikedOnLeft;
        bool spikedOnRight;
        bool spikedOnTop;
        bool spikedOnBottom;

        public SpikedPlatform(ContentManager cm, SpriteBatch sb, GraphicsDeviceManager gdm, ImageLibrary il, Vector2 platSize, Vector2 pos, bool slidableOnL, bool slidableOnR, bool spOnLeft, bool spOnRight, bool spOnTop, bool spOnBottom, bool cOnLeft, bool cOnRight, bool cOnTop, bool cOnBottom)
            : base(cm, sb, gdm, il, platSize, pos, slidableOnL, slidableOnR, cOnLeft, cOnRight, cOnTop, cOnBottom)
        {
            spikedOnLeft = spOnLeft;
            spikedOnRight = spOnRight;
            spikedOnTop = spOnTop;
            spikedOnBottom = spOnBottom;

            completeOutsidePlatformImage = il.smwOutsideGrass;
            spikedPlatformImage = il.spikePlatform;
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
                    //top left
                    if (xTracker == 0 && yTracker == 0)
                    {
                        if (spikedOnTop)
                        {
                            if (spikedOnLeft)
                            {
                                spikedPlatformImage.setFrameConfiguration(0, 0, 0);
                                spikedPlatformImage.DrawAltPosition(new Vector2(xPosition, yPosition), false, layerDepth);
                            }
                            else {
                                spikedPlatformImage.setFrameConfiguration(1, 1, 1);
                                spikedPlatformImage.DrawAltPosition(new Vector2(xPosition, yPosition), false, layerDepth);
                            }
                        }
                        else {
                            if (spikedOnLeft)
                            {
                                spikedPlatformImage.setFrameConfiguration(3, 3, 3);
                                spikedPlatformImage.DrawAltPosition(new Vector2(xPosition, yPosition), false, layerDepth);
                            }
                            else {
                                if (collidableOnTop)
                                {
                                    if (collidableOnLeft)
                                    {
                                        completeOutsidePlatformImage.setFrameConfiguration(0, 0, 0);
                                        completeOutsidePlatformImage.DrawAltPosition(new Vector2(xPosition, yPosition), false, layerDepth);
                                    }
                                    else
                                    {
                                        completeOutsidePlatformImage.setFrameConfiguration(1, 1, 1);
                                        completeOutsidePlatformImage.DrawAltPosition(new Vector2(xPosition, yPosition), false, layerDepth);
                                    }
                                }
                                else {
                                    if (collidableOnLeft)
                                    {
                                        completeOutsidePlatformImage.setFrameConfiguration(3, 3, 3);
                                        completeOutsidePlatformImage.DrawAltPosition(new Vector2(xPosition, yPosition), false, layerDepth);
                                    }
                                    else {
                                        completeOutsidePlatformImage.setFrameConfiguration(4, 4, 4);
                                        completeOutsidePlatformImage.DrawAltPosition(new Vector2(xPosition, yPosition), false, layerDepth);
                                    }
                                }
                            }
                        }
                    }
                    //top
                    else if (yTracker == 0 && xTracker > 0 && xTracker < platformSize.X - 1)
                    {
                        if (spikedOnTop)
                        {
                            spikedPlatformImage.setFrameConfiguration(1, 1, 1);
                            spikedPlatformImage.DrawAltPosition(new Vector2(xPosition, yPosition), false, layerDepth);
                        }
                        else {
                            if (collidableOnTop)
                            {
                                completeOutsidePlatformImage.setFrameConfiguration(1, 1, 1);
                                completeOutsidePlatformImage.DrawAltPosition(new Vector2(xPosition, yPosition), false, layerDepth);
                            }
                            else {
                                completeOutsidePlatformImage.setFrameConfiguration(4, 4, 4);
                                completeOutsidePlatformImage.DrawAltPosition(new Vector2(xPosition, yPosition), false, layerDepth);
                            }
                        }
                    }
                    //top right
                    else if (yTracker == 0 && xTracker == platformSize.X - 1) {
                        if (spikedOnTop)
                        {
                            if (spikedOnRight)
                            {
                                spikedPlatformImage.setFrameConfiguration(2, 2, 2);
                                spikedPlatformImage.DrawAltPosition(new Vector2(xPosition, yPosition), false, layerDepth);
                            }
                            else {
                                spikedPlatformImage.setFrameConfiguration(1, 1, 1);
                                spikedPlatformImage.DrawAltPosition(new Vector2(xPosition, yPosition), false, layerDepth);
                            }
                        }
                        else {
                            if (spikedOnRight)
                            {
                                spikedPlatformImage.setFrameConfiguration(5, 5, 5);
                                spikedPlatformImage.DrawAltPosition(new Vector2(xPosition, yPosition), false, layerDepth);
                            }
                            else {
                                if (collidableOnTop)
                                {
                                    if (collidableOnRight)
                                    {
                                        completeOutsidePlatformImage.setFrameConfiguration(2, 2, 2);
                                        completeOutsidePlatformImage.DrawAltPosition(new Vector2(xPosition, yPosition), false, layerDepth);
                                    }
                                    else
                                    {
                                        completeOutsidePlatformImage.setFrameConfiguration(1, 1, 1);
                                        completeOutsidePlatformImage.DrawAltPosition(new Vector2(xPosition, yPosition), false, layerDepth);
                                    }
                                }
                                else {
                                    if (collidableOnRight)
                                    {
                                        completeOutsidePlatformImage.setFrameConfiguration(5, 5, 5);
                                        completeOutsidePlatformImage.DrawAltPosition(new Vector2(xPosition, yPosition), false, layerDepth);
                                    }
                                    else {
                                        completeOutsidePlatformImage.setFrameConfiguration(4, 4, 4);
                                        completeOutsidePlatformImage.DrawAltPosition(new Vector2(xPosition, yPosition), false, layerDepth);
                                    }
                                }
                            }
                        }
                    }
                    //left
                    else if (yTracker > 0 && yTracker < platformSize.Y - 1 && xTracker == 0)
                    {
                        if (spikedOnLeft)
                        {
                            spikedPlatformImage.setFrameConfiguration(3, 3, 3);
                            spikedPlatformImage.DrawAltPosition(new Vector2(xPosition, yPosition), false, layerDepth);
                        }
                        else
                        {
                            if (collidableOnLeft)
                            {
                                completeOutsidePlatformImage.setFrameConfiguration(3, 3, 3);
                                completeOutsidePlatformImage.DrawAltPosition(new Vector2(xPosition, yPosition), false, layerDepth);
                            }
                            else
                            {
                                completeOutsidePlatformImage.setFrameConfiguration(4, 4, 4);
                                completeOutsidePlatformImage.DrawAltPosition(new Vector2(xPosition, yPosition), false, layerDepth);
                            }
                        }
                    }
                    //center
                    else if (yTracker > 0 && yTracker < platformSize.Y - 1 && xTracker > 0 && xTracker < platformSize.X - 1)
                    {
                        completeOutsidePlatformImage.setFrameConfiguration(4, 4, 4);
                        completeOutsidePlatformImage.DrawAltPosition(new Vector2(xPosition, yPosition), false, layerDepth);
                    }
                    //right
                    else if (yTracker > 0 && yTracker < platformSize.Y - 1 && xTracker == platformSize.X - 1)
                    {
                        if (spikedOnRight)
                        {
                            spikedPlatformImage.setFrameConfiguration(5, 5, 5);
                            spikedPlatformImage.DrawAltPosition(new Vector2(xPosition, yPosition), false, layerDepth);
                        }
                        else
                        {
                            if (collidableOnRight)
                            {
                                completeOutsidePlatformImage.setFrameConfiguration(5, 5, 5);
                                completeOutsidePlatformImage.DrawAltPosition(new Vector2(xPosition, yPosition), false, layerDepth);
                            }
                            else
                            {
                                completeOutsidePlatformImage.setFrameConfiguration(4, 4, 4);
                                completeOutsidePlatformImage.DrawAltPosition(new Vector2(xPosition, yPosition), false, layerDepth);
                            }
                        }
                    }
                    //bottom left
                    else if (yTracker == platformSize.Y - 1 && xTracker == 0)
                    {
                        if (spikedOnBottom)
                        {
                            if (spikedOnLeft)
                            {
                                spikedPlatformImage.setFrameConfiguration(6, 6, 6);
                                spikedPlatformImage.DrawAltPosition(new Vector2(xPosition, yPosition), false, layerDepth);
                            }
                            else
                            {
                                spikedPlatformImage.setFrameConfiguration(7, 7, 7);
                                spikedPlatformImage.DrawAltPosition(new Vector2(xPosition, yPosition), false, layerDepth);
                            }
                        }
                        else
                        {
                            if (spikedOnLeft)
                            {
                                spikedPlatformImage.setFrameConfiguration(3, 3, 3);
                                spikedPlatformImage.DrawAltPosition(new Vector2(xPosition, yPosition), false, layerDepth);
                            }
                            else
                            {
                                if (collidableOnBottom)
                                {
                                    if (collidableOnLeft)
                                    {
                                        completeOutsidePlatformImage.setFrameConfiguration(6, 6, 6);
                                        completeOutsidePlatformImage.DrawAltPosition(new Vector2(xPosition, yPosition), false, layerDepth);
                                    }
                                    else
                                    {
                                        completeOutsidePlatformImage.setFrameConfiguration(7, 7, 7);
                                        completeOutsidePlatformImage.DrawAltPosition(new Vector2(xPosition, yPosition), false, layerDepth);
                                    }
                                }
                                else
                                {
                                    if (collidableOnLeft)
                                    {
                                        completeOutsidePlatformImage.setFrameConfiguration(3, 3, 3);
                                        completeOutsidePlatformImage.DrawAltPosition(new Vector2(xPosition, yPosition), false, layerDepth);
                                    }
                                    else
                                    {
                                        completeOutsidePlatformImage.setFrameConfiguration(4, 4, 4);
                                        completeOutsidePlatformImage.DrawAltPosition(new Vector2(xPosition, yPosition), false, layerDepth);
                                    }
                                }
                            }
                        }
                    }
                    //bottom
                    else if (yTracker == platformSize.Y - 1 && xTracker > 0 && xTracker < platformSize.X - 1)
                    {
                        if (spikedOnBottom)
                        {
                            spikedPlatformImage.setFrameConfiguration(7, 7, 7);
                            spikedPlatformImage.DrawAltPosition(new Vector2(xPosition, yPosition), false, layerDepth);
                        }
                        else
                        {
                            if (collidableOnBottom)
                            {
                                completeOutsidePlatformImage.setFrameConfiguration(7, 7, 7);
                                completeOutsidePlatformImage.DrawAltPosition(new Vector2(xPosition, yPosition), false, layerDepth);
                            }
                            else
                            {
                                completeOutsidePlatformImage.setFrameConfiguration(4, 4, 4);
                                completeOutsidePlatformImage.DrawAltPosition(new Vector2(xPosition, yPosition), false, layerDepth);
                            }
                        }
                    }
                    //bottom right
                    else if (yTracker == platformSize.Y - 1 && xTracker == platformSize.X - 1)
                    {
                        if (spikedOnBottom)
                        {
                            if (spikedOnRight)
                            {
                                spikedPlatformImage.setFrameConfiguration(8, 8, 8);
                                spikedPlatformImage.DrawAltPosition(new Vector2(xPosition, yPosition), false, layerDepth);
                            }
                            else
                            {
                                spikedPlatformImage.setFrameConfiguration(7, 7, 7);
                                spikedPlatformImage.DrawAltPosition(new Vector2(xPosition, yPosition), false, layerDepth);
                            }
                        }
                        else
                        {
                            if (spikedOnRight)
                            {
                                spikedPlatformImage.setFrameConfiguration(5, 5, 5);
                                spikedPlatformImage.DrawAltPosition(new Vector2(xPosition, yPosition), false, layerDepth);
                            }
                            else
                            {
                                if (collidableOnBottom)
                                {
                                    if (collidableOnRight)
                                    {
                                        completeOutsidePlatformImage.setFrameConfiguration(8, 8, 8);
                                        completeOutsidePlatformImage.DrawAltPosition(new Vector2(xPosition, yPosition), false, layerDepth);
                                    }
                                    else
                                    {
                                        completeOutsidePlatformImage.setFrameConfiguration(7, 7, 7);
                                        completeOutsidePlatformImage.DrawAltPosition(new Vector2(xPosition, yPosition), false, layerDepth);
                                    }
                                }
                                else
                                {
                                    if (collidableOnRight)
                                    {
                                        completeOutsidePlatformImage.setFrameConfiguration(5, 5, 5);
                                        completeOutsidePlatformImage.DrawAltPosition(new Vector2(xPosition, yPosition), false, layerDepth);
                                    }
                                    else
                                    {
                                        completeOutsidePlatformImage.setFrameConfiguration(4, 4, 4);
                                        completeOutsidePlatformImage.DrawAltPosition(new Vector2(xPosition, yPosition), false, layerDepth);
                                    }
                                }
                            }
                        }
                    }

                    xPosition += (int)completeOutsidePlatformImage.frameSize.X;
                    xTracker++;
                }
                yTracker++;
                yPosition += (int)(completeOutsidePlatformImage.frameSize.Y);
            }
        }

        //not really sure how I want to implement this, I need to refine a binary search for the correct placement....
        public override Vector2 checkAndFixPlatformCollision(Vector2 newPos, Player player)
        {
            //------------
            //First phase is to check for a collision and to mark collisions accordingly
            // will operate on the basis that the values returned for the collision are against the players sides/top/bottom and the rectangle of the tile platform
            //------------

            Platform tp = this;

            //makes the current platform rectangle reference, not sure if this is done via call by reference
            Rectangle previousTileComparisionRect = tp.boundingRectangle;
            Rectangle currentTileComparisionRect = tp.boundingRectangle;

            //please not that the +10 and -20 modifiers are used to alter the player's collision with the platform in a smaller section so that he doesn't detect hits when falling down
            //if this becomes an issue replace the modifiers with variables that are only used if a default boolean in the constructor is set to true so this can differentiate platforms that
            //are stacked
            //player left side
            Rectangle tpLeftRectSide = new Rectangle(currentTileComparisionRect.X, currentTileComparisionRect.Y, 1, currentTileComparisionRect.Height);
            //player right side, has a -1 offset to the x value so that it's overlapping correctly for comparison
            Rectangle tpRightRectSide = new Rectangle(currentTileComparisionRect.Right, currentTileComparisionRect.Y, 1, currentTileComparisionRect.Height);
            //player top side
            Rectangle tpTopRectSide = new Rectangle(currentTileComparisionRect.X + 16, currentTileComparisionRect.Y, currentTileComparisionRect.Width - 32, 1);
            //player bottom side, has a -1 offset to the y value so that it's overlapping correctly for comparison
            Rectangle tpBottomRectSide = new Rectangle(currentTileComparisionRect.X + 16, currentTileComparisionRect.Bottom, currentTileComparisionRect.Width - 32, 1);

            //makes player rectangle reference
            Rectangle previousPlayerComparisionRect = new Rectangle((int)newPos.X, (int)newPos.Y, (int)player.width, (int)player.height);
            Rectangle currentPlayerComparisionRect = new Rectangle((int)newPos.X, (int)newPos.Y, (int)player.width, (int)player.height);
            //player left side
            Rectangle playerLeftRectSide = new Rectangle(currentPlayerComparisionRect.X, currentPlayerComparisionRect.Y, 1, currentPlayerComparisionRect.Height);
            //player right side, has a -1 offset to the x value so that it's overlapping correctly for comparison
            Rectangle playerRightRectSide = new Rectangle(currentPlayerComparisionRect.Right, currentPlayerComparisionRect.Y, 1, currentPlayerComparisionRect.Height);
            //player top side
            Rectangle playerTopRectSide = new Rectangle(currentPlayerComparisionRect.X, currentPlayerComparisionRect.Y, currentPlayerComparisionRect.Width, 1);
            //player bottom side, has a -1 offset to the y value so that it's overlapping correctly for comparison
            Rectangle playerBottomRectSide = new Rectangle(currentPlayerComparisionRect.X, currentPlayerComparisionRect.Bottom, currentPlayerComparisionRect.Width, 1);
            //System.Diagnostics.Debug.WriteLine("Current Collision Box X: " + currentPlayerComparisionRect.X);
            //System.Diagnostics.Debug.WriteLine("Current Collision Box Y: " + currentPlayerComparisionRect.Y);

            //System.Diagnostics.Debug.WriteLine("Current Collision Box X: " + currentTileComparisionRect.X);
            //System.Diagnostics.Debug.WriteLine("Current Collision Box Y: " + currentTileComparisionRect.Y);

            //right platform hit, left side of player hit
            if (currentPlayerComparisionRect.Intersects(tpRightRectSide) && collidableOnRight)
            {
                player.collisionOnPlayerLeft = true;
                //System.Diagnostics.Debug.WriteLine("Left Collision");
            }
            //left tile platform hit, right side of player hit
            if (currentPlayerComparisionRect.Intersects(tpLeftRectSide) && collidableOnLeft)
            {
                player.collisionOnPlayerRight = true;
                //System.Diagnostics.Debug.WriteLine("Right Collision");
            }
            //bottom of platform hit, top of player hit
            if (currentPlayerComparisionRect.Intersects(tpBottomRectSide) && collidableOnBottom)
            {
                player.collisionOnPlayerTop = true;
                //System.Diagnostics.Debug.WriteLine("Top Collision");
            }
            //top of platform hit, bottom of player hit
            if (currentPlayerComparisionRect.Intersects(tpTopRectSide) && collidableOnTop)
            {
                //this checks to see if it needs to do anything special, just in case only the bottom bounding is collidable
                if (!collidableOnLeft && !collidableOnRight && !collidableOnBottom)
                {
                    //if the bottom bounding of the player isn't within 40 pixels of the top of the platform the collision doesn't occur for this type of block
                    if (player.bottomBounding < tpTopRectSide.Y + 20 && player.bottomBounding > tpTopRectSide.Y - 20)
                    {
                        player.collisionOnPlayerBottom = true;
                    }
                    else
                    {
                        //it's false
                    }
                }
                else
                {
                    player.collisionOnPlayerBottom = true;
                }
                //System.Diagnostics.Debug.WriteLine("Bottom Collision");
            }
            //*****
            //I'm pretty positive that this may be a problem in the future. I don't know.... I've never implemented collision like this...
            //this nested if statement basically says don't fix any collision on bottom if jumping
            //I'm pretty sure this will mess me up later, but at the moment I'm letting it slide because wow, it really fixes the
            //controls, if you can in the future fix this to work around any bugs. Try to keep this for the logic.
            if (player.collisionOnPlayerBottom && player.isJumping)
            {
                player.collisionOnPlayerBottom = false;
            }

            //this is to fix landing collision so that if there is a bottom collision as long as you're not falling left and right are false
            if (player.collisionOnPlayerBottom)
            {
                if (player.collisionOnPlayerLeft)
                {
                    if (tpRightRectSide.X - playerLeftRectSide.X < playerBottomRectSide.Y - tpTopRectSide.Y)
                    {
                        player.collisionOnPlayerBottom = false;
                    }
                    else
                    {
                        player.collisionOnPlayerLeft = false;
                    }
                }
                if (player.collisionOnPlayerRight)
                {
                    if (playerRightRectSide.X - tpLeftRectSide.X < playerBottomRectSide.Y - tpTopRectSide.Y)
                    {
                        player.collisionOnPlayerBottom = false;
                    }
                    else
                    {
                        player.collisionOnPlayerRight = false;
                    }
                }
            }

            //top collision
            if (player.collisionOnPlayerTop)
            {
                if (player.collisionOnPlayerLeft)
                {
                    if (tpRightRectSide.X - playerLeftRectSide.X < tpBottomRectSide.Y - playerTopRectSide.Y)
                    {
                        player.collisionOnPlayerTop = false;
                    }
                    else
                    {
                        player.collisionOnPlayerLeft = false;
                    }
                }
                if (player.collisionOnPlayerRight)
                {
                    if (playerRightRectSide.X - tpLeftRectSide.X < tpBottomRectSide.Y - playerTopRectSide.Y)
                    {
                        player.collisionOnPlayerBottom = false;
                    }
                    else
                    {
                        player.collisionOnPlayerRight = false;
                    }
                }
            }

            //left death condition
            if (player.collisionOnPlayerLeft && spikedOnRight) {
                player.wasKilled = true;
            }
            //right death condition
            if (player.collisionOnPlayerRight && spikedOnLeft)
            {
                player.wasKilled = true;
            }
            //top death condition
            if (player.collisionOnPlayerTop && spikedOnBottom)
            {
                player.wasKilled = true;
            }
            //bottom death condition
            if (player.collisionOnPlayerBottom && spikedOnTop)
            {
                player.wasKilled = true;
            }

            //if any collision occured then update the flag so that a general marker is known
            if (player.collisionOnPlayerLeft ||
                player.collisionOnPlayerRight ||
                player.collisionOnPlayerTop ||
                player.collisionOnPlayerBottom)
            {
                player.playerCollisionOccurred = true;
            }

            //------------
            //second phase is to respond accordingly
            //------------

            if (player.playerCollisionOccurred)
            {
                //left was hit
                if (player.collisionOnPlayerLeft)
                {
                    while (currentPlayerComparisionRect.Intersects(tpRightRectSide))
                    {
                        ///
                        /// resets the values hopefully using a call by value here to that we're getting the numbers and not the reference to previous position
                        /// This is an important nuance on account that it makes it so all our collision checking is done through rectangled abstraction. This means
                        /// we're just using the values to initialze the rectangles and then updating the collision from there, at the end when the fix is to exit the
                        /// rectangle will set the player position appropriately
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
                if (player.collisionOnPlayerRight)
                {
                    while (currentPlayerComparisionRect.Intersects(tpLeftRectSide))
                    {
                        ///
                        /// resets the values hopefully using a call by value here to that we're getting the numbers and not the reference to previous position
                        /// This is an important nuance on account that it makes it so all our collision checking is done through rectangled abstraction. This means
                        /// we're just using the values to initialze the rectangles and then updating the collision from there, at the end when the fix is to exit the
                        /// rectangle will set the player position appropriately
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
                if (player.collisionOnPlayerTop)
                {

                    while (currentPlayerComparisionRect.Intersects(tpBottomRectSide))
                    {
                        ///
                        /// resets the values hopefully using a call by value here to that we're getting the numbers and not the reference to previous position
                        /// This is an important nuance on account that it makes it so all our collision checking is done through rectangled abstraction. This means
                        /// we're just using the values to initialze the rectangles and then updating the collision from there, at the end when the fix is to exit the
                        /// rectangle will set the player position appropriately
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
                    //player.isJumping = false;
                    //player.isFalling = true;
                }

                //bottom was hit
                if (player.collisionOnPlayerBottom)
                {
                    while (currentPlayerComparisionRect.Intersects(tpTopRectSide))
                    {
                        ///
                        /// resets the values hopefully using a call by value here to that we're getting the numbers and not the reference to previous position
                        /// This is an important nuance on account that it makes it so all our collision checking is done through rectangled abstraction. This means
                        /// we're just using the values to initialze the rectangles and then updating the collision from there, at the end when the fix is to exit the
                        /// rectangle will set the player position appropriately
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
                            currentPlayerComparisionRect.Y = currentTileComparisionRect.Y - (int)heightMidpointDistance - (int)player.height - 2;
                            playerBottomRectSide.Y = currentPlayerComparisionRect.Bottom;
                        }
                    }

                    //fixes falling collision for gravity falls with platform
                    if (player.isFalling || player.isFallingFromGravity)
                    {
                        if (player.isFalling)
                        {
                            player.isFalling = false;
                            if (player.movingRight || player.movingLeft)
                            {
                                player.playerImageSheet.setFrameConfiguration(18, 18, 28);
                                player.playerImageSheet.frameTimeLimit = player.defaultFrameTimeLimit;
                            }
                        }
                        if (player.isFallingFromGravity)
                        {
                            player.isFallingFromGravity = false;
                            if (player.movingRight || player.movingLeft)
                            {
                                player.playerImageSheet.setFrameConfiguration(18, 18, 28);
                                player.playerImageSheet.frameTimeLimit = player.defaultFrameTimeLimit;
                            }
                            else if (player.standing)
                            {
                                player.playerImageSheet.setFrameConfiguration(0, 0, 4);
                                player.playerImageSheet.frameTimeLimit = player.defaultFrameTimeLimit;
                            }
                        }
                        if (player.exhaustedDash)
                        {
                            player.exhaustedDash = false;
                        }
                    }

                    newPos.Y = currentPlayerComparisionRect.Y;
                    player.isJumping = false;
                    player.isFalling = false;
                    player.isFallingFromGravity = false;
                    player.isDashing = false;
                    player.exhaustedDash = false;
                }

                if (player.playerCollisionOccurred)
                {
                    player.playerCollisionOccurred = false;
                    player.collisionOnPlayerLeft = false;
                    player.collisionOnPlayerRight = false;
                    player.collisionOnPlayerTop = false;
                    player.collisionOnPlayerBottom = false;
                }

                return newPos;
            }
            else
            {
                if (player.playerCollisionOccurred)
                {
                    player.playerCollisionOccurred = false;
                    player.collisionOnPlayerLeft = false;
                    player.collisionOnPlayerRight = false;
                    player.collisionOnPlayerTop = false;
                    player.collisionOnPlayerBottom = false;
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
