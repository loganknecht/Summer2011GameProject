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
    class SlopedPlatform
    {
        ContentManager cmReference;
        GraphicsDeviceManager gdmReference;
        ImageLibrary imageLibrary;
        SpriteBatch sbReference;

        public Line platformSlope;

        public Platform boundingPlatform;

        public AnimatedImage slopeTexture;

        float layerDepth;

        int platformBodyHeight;

        public bool collidableOnLeftSide;
        public bool collidableOnRightSide;
        public bool collidableOnBottom;

        public SlopedPlatform(ContentManager cm, GraphicsDeviceManager gdm, SpriteBatch sb, ImageLibrary il, Vector2 startSlopePos, Vector2 endSlopePos, int platBodyHeight, float lDepth, bool slidable, bool cOnLeftSide, bool cOnRightSide, bool cOnBottom)
        {
            cmReference = cm;
            gdmReference = gdm;
            imageLibrary = il;
            sbReference = sb;

            platformSlope = new Line(startSlopePos, endSlopePos);
            slopeTexture = il.smwOutsideGrass;
            //platformSlope = new Line(new Vector2(0, 1000), new Vector2(200, 800));

            if (platformSlope.pt1.Y < platformSlope.pt2.Y)
            {
                boundingPlatform = new Platform(cmReference, sbReference, gdmReference, imageLibrary, new Vector2(((platformSlope.pt2.Y - platformSlope.pt1.Y) / 16), (platformSlope.getBoundingRectangle.Height / 16)), new Vector2(platformSlope.getBoundingRectangle.Left, platformSlope.getBoundingRectangle.Top), slidable, true, true, false, false, true);
            }
            else
            {
                boundingPlatform = new Platform(cmReference, sbReference, gdmReference, imageLibrary, new Vector2(((platformSlope.pt1.Y - platformSlope.pt2.Y) / 16), (platformSlope.getBoundingRectangle.Height / 16)), new Vector2(platformSlope.getBoundingRectangle.Left, platformSlope.getBoundingRectangle.Top), slidable, true, false, true, false, true);
            }

            layerDepth = lDepth;

            platformBodyHeight = platBodyHeight * 16;

            collidableOnLeftSide = cOnLeftSide;
            collidableOnRightSide = cOnRightSide;
            collidableOnBottom = cOnBottom;
        }

        public void update(GameTime gameTime)
        {
        }

        public void drawAltPosition(Vector2 startSlopePosition, Vector2 endSlopePosition)
        {
            //sbReference.Draw(slopeTexture, new Rectangle((int)(altPos.X + xTracker), (int)(altLine.yAtX(altPos.X-xTracker)), (int)slopeTexture.Width, (int)slopeTexture.Height), null, Color.White, (float)(rotation * (Math.PI / 180)), Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
            Rectangle tempRectangle = platformSlope.getAltBoundingRectangle(startSlopePosition);
            if (startSlopePosition.Y > endSlopePosition.Y) {
                tempRectangle = platformSlope.getAltBoundingRectangle(new Vector2(startSlopePosition.X, endSlopePosition.Y));
            }

            Line altLine = new Line(startSlopePosition, endSlopePosition);
            float slope = Math.Abs(altLine.getSlope());
            float rotation = (float)(180 / Math.PI * Math.Atan((platformSlope.pt2.Y - platformSlope.pt1.Y) / (platformSlope.pt2.X - platformSlope.pt1.X)));

            //System.Diagnostics.Debug.WriteLine(rotation);
            float xTracker = 0;
            float yTracker = 0;

            //System.Diagnostics.Debug.WriteLine("Start X: " + platformSlope.getBoundingRectangle.X);
            //System.Diagnostics.Debug.WriteLine("Start Y: " + startYPos);
            //System.Diagnostics.Debug.WriteLine("Alt Y: " + altPos.Y);
            //System.Diagnostics.Debug.WriteLine("Width: " + platformSlope.getBoundingRectangle.Width);
            //System.Diagnostics.Debug.WriteLine(altLine.getSlope());
            
            while (tempRectangle.Left + xTracker < tempRectangle.Right)
            {
                if (xTracker == 0) {
                    if (collidableOnLeftSide)
                    {
                        slopeTexture.setFrameConfiguration(0, 0, 0);
                    }
                    else {
                        slopeTexture.setFrameConfiguration(1, 1, 1);
                    }
                }
                else if (tempRectangle.Left + xTracker != tempRectangle.Right-16) {
                    slopeTexture.setFrameConfiguration(1, 1, 1);
                }
                else if (tempRectangle.Left + xTracker > tempRectangle.Right - 16)
                {
                    if (collidableOnRightSide)
                    {
                        slopeTexture.setFrameConfiguration(2, 2, 2);
                    }
                    else
                    {
                        slopeTexture.setFrameConfiguration(1, 1, 1);
                    }
                }

                if (tempRectangle.Left + xTracker > tempRectangle.Right-16)
                {
                    slopeTexture.DrawAltPosition(new Vector2(tempRectangle.Right-16, altLine.yAtX(tempRectangle.Right)), false, 0, layerDepth + .01f);
                }
                else
                {
                    slopeTexture.DrawAltPosition(new Vector2(tempRectangle.Left + xTracker, altLine.yAtX(tempRectangle.Left + xTracker)), false, rotation, layerDepth);
                }
                

                //draws the platform slope texture                               

                //-------------------------------
                //System.Diagnostics.Debug.WriteLine("slope: " + altLine.getSlope());
                float xTrackerModifier = (float)Math.Abs(Math.Cos(rotation) * 16);
                //System.Diagnostics.Debug.WriteLine(xTrackerModifier);
                if (xTrackerModifier > 16)
                {
                    xTracker += 16;
                }
                else {
                    xTracker += xTrackerModifier;
                }
            }

            //------------------------ draw body start -----------------------
            
            //DRAW PLATFORM BODY BELOW
            xTracker = 0;
            yTracker = 16;

            float startYPos; 

            while (xTracker < tempRectangle.Width)
            {
                startYPos = altLine.yAtX(tempRectangle.Left + xTracker);
                
                if (slope >= 0 && slope < .1) { 
                }
                else if (slope >= .1 && slope < .2)
                {
                    startYPos -= 1;
                }
                else if (slope >= .2 && slope < .3)
                {
                    startYPos -= 2;
                }
                else if(slope >= .3 && slope < .4) {
                    startYPos -= 2;
                }
                else if (slope >= .4 && slope < .5)
                {
                    startYPos -= 4;
                }
                else if (slope >= .6 && slope < .7)
                {
                    startYPos -= 4;
                }

                yTracker = 16;

                while (startYPos <= platformBodyHeight + tempRectangle.Bottom- yTracker)
                {
                    //left side
                    if (xTracker == 0)
                    {
                        //bottom left
                        if (yTracker == 16)
                        {
                            if (collidableOnLeftSide && collidableOnBottom)
                            {
                                slopeTexture.setFrameConfiguration(6, 6, 6);
                            }
                            else if (collidableOnBottom && !collidableOnLeftSide)
                            {
                                slopeTexture.setFrameConfiguration(7, 7, 7);
                            }
                            else if (collidableOnLeftSide && !collidableOnBottom)
                            {
                                slopeTexture.setFrameConfiguration(3, 3, 3);
                            }
                            else
                            {
                                slopeTexture.setFrameConfiguration(4, 4, 4);
                            }
                        }
                        //right side
                        else
                        {
                            if (collidableOnLeftSide)
                            {
                                slopeTexture.setFrameConfiguration(3, 3, 3);
                            }
                            else
                            {
                                slopeTexture.setFrameConfiguration(4, 4, 4);
                            }
                        }
                    }
                    //between left and right side                   
                    else if (xTracker > 0 && xTracker < tempRectangle.Width - 16 && tempRectangle.Bottom + platformBodyHeight - yTracker > altLine.yAtX(tempRectangle.Left + xTracker + 16))
                    {
                        //bottom
                        if (yTracker == 16)
                        {
                            if (collidableOnBottom)
                            {
                                slopeTexture.setFrameConfiguration(7, 7, 7);
                            }
                            else
                            {
                                slopeTexture.setFrameConfiguration(4, 4, 4);
                            }
                        }
                        //center
                        else
                        {
                            slopeTexture.setFrameConfiguration(4, 4, 4);
                        }

                    }
                    //right side
                    else if (xTracker > tempRectangle.Width - 16)
                    {
                        if (yTracker == 16)
                        {
                            if (collidableOnRightSide && collidableOnBottom)
                            {
                                slopeTexture.setFrameConfiguration(8, 8, 8);
                            }
                            else if (collidableOnBottom && !collidableOnRightSide)
                            {
                                slopeTexture.setFrameConfiguration(7, 7, 7);
                            }
                            else if (collidableOnRightSide && !collidableOnBottom)
                            {
                                slopeTexture.setFrameConfiguration(5, 5, 5);
                            }
                            else
                            {
                                slopeTexture.setFrameConfiguration(4, 4, 4);
                            }
                        }
                        else
                        {
                            if (collidableOnRightSide)
                            {
                                slopeTexture.setFrameConfiguration(5, 5, 5);
                            }
                            else
                            {
                                slopeTexture.setFrameConfiguration(4, 4, 4);
                            }
                        }
                    }
                    if (xTracker > tempRectangle.Width - 16)
                    {
                        slopeTexture.DrawAltPosition(new Vector2(tempRectangle.Right-16, tempRectangle.Bottom + platformBodyHeight - yTracker), false, 0, layerDepth + .01f);
                    }
                    else
                    {
                        slopeTexture.DrawAltPosition(new Vector2(tempRectangle.Left + xTracker, tempRectangle.Bottom + platformBodyHeight - yTracker), false, 0, layerDepth + .01f);
                    }
                    yTracker += 16;
                }
                xTracker += 16;

                //modifier helps account for slight discrepancies with drawing
                //-------------------------------------------------------------------------draw body end --------------------
            }
        }

        public Vector2 checkAndFixPlatformCollision(Vector2 newPos, Player player)
        {
            //System.Diagnostics.Debug.WriteLine("player X: " + newPos.X);
            //System.Diagnostics.Debug.WriteLine("player Y: " + newPos.Y);
            //System.Diagnostics.Debug.WriteLine("player Height: " + (int)player.height);
            //System.Diagnostics.Debug.WriteLine("player Bottom Y: " + (newPos.Y + (int)player.height));

            //System.Diagnostics.Debug.WriteLine("Bounding Platform Right: " + boundingPlatform.boundingRectangle.Right);
            //System.Diagnostics.Debug.WriteLine("Bounding Platform Top: " + boundingPlatform.boundingRectangle.Top);
            //System.Diagnostics.Debug.WriteLine("Bounding Platform Bottom: " + boundingPlatform.boundingRectangle.Bottom);

            //Vector2 tempPos = boundingPlatform.checkAndFixPlatformCollision(newPos, player);

            //Rectangle tempRectangle = platformSlope.verticalCollisionResponse(new Rectangle((int)tempPos.X, (int)tempPos.Y, (int)player.width, (int)player.height), new Rectangle((int)player.previousWorldPosition.X, (int)player.previousWorldPosition.Y, (int)player.width, (int)player.height));
            int heightBuffer = player.height-10;

            Rectangle curRect = new Rectangle((int)newPos.X, (int)(newPos.Y + heightBuffer), (int)player.width, 1);
            Rectangle prevRec = new Rectangle((int)player.previousWorldPosition.X, (int)(player.previousWorldPosition.Y + heightBuffer), (int)player.width, 1);
            Rectangle checkRectangle = new Rectangle((int)newPos.X, (int)(newPos.Y + heightBuffer), (int)player.width, 1);
            Rectangle returnRect;


            returnRect = platformSlope.verticalCollisionResponse(curRect, prevRec);

            if (returnRect.X != checkRectangle.X ||
                returnRect.Y != checkRectangle.Y)
            {
                if (!player.wallSliding && !player.isDashing) {
                    if ((player.movingRight || player.movingLeft) && (player.playerImageSheet.currentFrame <= 18 || player.playerImageSheet.currentFrame >= 28))
                    {
                        player.playerImageSheet.setFrameConfiguration(18, 18, 28);
                        player.playerImageSheet.frameTimeLimit = player.defaultFrameTimeLimit;
                    }
                    else if (player.standing && player.playerImageSheet.currentFrame <= 0 && player.playerImageSheet.currentFrame >= 4)
                    {
                        player.playerImageSheet.setFrameConfiguration(0, 0, 4);
                        player.playerImageSheet.frameTimeLimit = player.defaultFrameTimeLimit;
                    }
                }
                
                if (player.exhaustedDash)
                {
                    player.exhaustedDash = false;
                }

                player.isJumping = false;
                player.jumpExhausted = false;
                player.isFalling = false;
                player.isFallingFromGravity = false;
                player.exhaustedDash = false;
            }

            //System.Diagnostics.Debug.WriteLine("player X: " + tempRectangle.X);
            //System.Diagnostics.Debug.WriteLine("player Y: " + tempRectangle.Y);

            //System.Diagnostics.Debug.WriteLine("Side Start X: " + tempLine.pt1.X);
            //System.Diagnostics.Debug.WriteLine("Side End X: " + tempLine.pt2.X);

            //System.Diagnostics.Debug.WriteLine("Point 1 Y: " + tempLine.pt1.Y);
            //System.Diagnostics.Debug.WriteLine("Point 2 Y: " + tempLine.pt2.Y);

            return new Vector2(returnRect.X, returnRect.Y - heightBuffer);
        }
    }
}
