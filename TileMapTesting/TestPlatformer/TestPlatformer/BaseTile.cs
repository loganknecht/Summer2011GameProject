using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TestPlatformer
{
    /// <summary>
    /// This class serves as the base reference for the tiles in tile maps
    /// I'm creating this base class so that I can track them easier in container classes
    /// The base tile clase is to only serve for base parameters, ever other tile will be implemented differently
    /// POSSIBLE IMPLEMENTATION: Remember that it may be easier to set up an int parameter that determines the type of tile to draw
    /// if further drawn on you could create a type parameter, the tile image to draw (and possibly configure it to be procedural for corners), and then if it's collidable
    /// Again, probably way later in the implementation
    /// </summary>
    abstract class BaseTile : GameObject
    {
        public AnimatedImage baseTileImage;

        ContentManager cmReference;
        GraphicsDeviceManager gdmReference;
        SpriteBatch sbReference;

        public bool isCollidable;
        public bool collidableOnLeft;
        public bool collidableOnRight;
        public bool collidableOnTop;
        public bool collidableOnBottom;

        public Rectangle boundingRectangle {
            get {
                return new Rectangle((int)position.X, (int)position.Y, (int)baseTileImage.frameSize.X, (int)baseTileImage.frameSize.Y);
            }
        }
        public BaseTile(ContentManager cm, GraphicsDeviceManager gdm, SpriteBatch sb, Vector2 pos, AnimatedImage imageName) : base(pos)
        {
            cmReference = cm;
            gdmReference = gdm;
            sbReference = sb;

            baseTileImage = imageName;
            baseTileImage.position = pos;
            baseTileImage.setFrameConfiguration(0, 0, 0);

            isCollidable = false;
        }

        public BaseTile(ContentManager cm, GraphicsDeviceManager gdm, SpriteBatch sb, Vector2 pos, Vector2 sourceImageSize, Vector2 sourceImageColumnAndRowSize, String imageName, bool cOnLeft, bool cOnRight, bool cOnTop, bool cOnBottom) : base(pos)
        {
            cmReference = cm;
            gdmReference = gdm;
            sbReference = sb;

            baseTileImage = new AnimatedImage(cmReference, sbReference, gdmReference.GraphicsDevice, imageName, sourceImageSize, sourceImageColumnAndRowSize);
            baseTileImage.position = pos;
            baseTileImage.setFrameConfiguration(0, 0, 0);

            isCollidable = false;
            collidableOnLeft = cOnLeft;
            collidableOnRight = cOnRight;
            collidableOnTop = cOnTop;
            collidableOnBottom = cOnBottom;
        }

        public void update(GameTime gameTime)
        {
            baseTileImage.Update(gameTime);
        }

        public void draw()
        {
            baseTileImage.Draw();
        }

        public void drawAltPosition(Vector2 altPosition)
        {
            baseTileImage.DrawAltPosition(altPosition);
        }

        //not really sure how I want to implement this, I need to refine a binary search for the correct placement....
        public Vector2 checkAndFixTileCollisionWithPlayer(Vector2 newPos, Player player)
        {
            //------------
            //First phase is to check for a collision and to mark collisions accordingly
            // will operate on the basis that the values returned for the collision are against the players sides/top/bottom and the rectangle of the tile platform
            //------------

            BaseTile tp = this;

            //makes the current tileplatform rectangle reference, not sure if this is done via call by reference
            Rectangle previousTileComparisionRect = tp.boundingRectangle;
            Rectangle currentTileComparisionRect = tp.boundingRectangle;
            //player left side
            Rectangle tpLeftRectSide = new Rectangle(currentTileComparisionRect.X, currentTileComparisionRect.Y, 1, currentTileComparisionRect.Height);
            //player right side, has a -1 offset to the x value so that it's overlapping correctly for comparison
            Rectangle tpRightRectSide = new Rectangle(currentTileComparisionRect.Right, currentTileComparisionRect.Y, 1, currentTileComparisionRect.Height);
            //player top side
            Rectangle tpTopRectSide = new Rectangle(currentTileComparisionRect.X, currentTileComparisionRect.Y, currentTileComparisionRect.Width, 1);
            //player bottom side, has a -1 offset to the y value so that it's overlapping correctly for comparison
            Rectangle tpBottomRectSide = new Rectangle(currentTileComparisionRect.X, currentTileComparisionRect.Bottom, currentTileComparisionRect.Width, 1);

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

            //right tileplatform hit, left side of player hit
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
            //this is where dash and jump and falling are reset
            if (currentPlayerComparisionRect.Intersects(tpTopRectSide) && collidableOnTop)
            {
                player.collisionOnPlayerBottom = true;
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
                    if (playerLeftRectSide.X < tpRightRectSide.X - 5 && playerLeftRectSide.X > tpRightRectSide.X - 10)
                    {

                    }
                    else
                    {
                        player.collisionOnPlayerLeft = false;
                    }
                }
                if (player.collisionOnPlayerRight)
                {
                    if (playerRightRectSide.X > tpLeftRectSide.X + 5 && playerRightRectSide.X < tpLeftRectSide.X + 10)
                    {

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
                    if (playerLeftRectSide.X < tpRightRectSide.X - 5 && playerLeftRectSide.X > tpRightRectSide.X - 10)
                    {
                        //collision on the left side occurred
                    }
                    else
                    {
                        player.collisionOnPlayerLeft = false;
                    }
                }
                if (player.collisionOnPlayerRight)
                {
                    if (playerRightRectSide.X > tpLeftRectSide.X + 5 && playerRightRectSide.X < tpLeftRectSide.X + 10)
                    {
                        //collision on right side occurred
                    }
                    else
                    {
                        player.collisionOnPlayerRight = false;
                    }
                }
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
                        float widthMidpointDistance = (previousPlayerComparisionRect.Right - tpLeftRectSide.X) / 2;

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
                                player.playerImageSheet.frameTimeLimit = 8;
                            }
                        }
                        if (player.isFallingFromGravity)
                        {
                            player.isFallingFromGravity = false;
                            if (player.movingRight || player.movingLeft)
                            {
                                player.playerImageSheet.setFrameConfiguration(18, 18, 28);
                                player.playerImageSheet.frameTimeLimit = 8;
                            }
                            else if (player.standing)
                            {
                                player.playerImageSheet.setFrameConfiguration(0, 0, 4);
                                player.playerImageSheet.frameTimeLimit = 8;
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
