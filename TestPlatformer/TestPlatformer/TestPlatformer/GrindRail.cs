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
    class GrindRail
    {
        ContentManager cmReference;
        GraphicsDeviceManager gdmReference;
        ImageLibrary imageLibrary;
        SpriteBatch sbReference;

        public Line platformSlope;

        public Platform boundingPlatform;

        public AnimatedImage slopeTexture;

        float layerDepth;

        public int platformBodyHeight;

        public GrindRail(ContentManager cm, GraphicsDeviceManager gdm, SpriteBatch sb, ImageLibrary il, Vector2 startSlopePos, Vector2 endSlopePos, int platBodyHeight, float lDepth)
        {
            cmReference = cm;
            gdmReference = gdm;
            imageLibrary = il;
            sbReference = sb;

            platformSlope = new Line(startSlopePos, endSlopePos);
            slopeTexture = il.testRail;
            //platformSlope = new Line(new Vector2(0, 1000), new Vector2(200, 800));

            if (platformSlope.pt1.Y < platformSlope.pt2.Y)
            {
                boundingPlatform = new Platform(cmReference, sbReference, gdmReference, imageLibrary, new Vector2(((platformSlope.pt2.Y - platformSlope.pt1.Y) / 16), (platformSlope.getBoundingRectangle.Height / 16)), new Vector2(platformSlope.getBoundingRectangle.Left, platformSlope.getBoundingRectangle.Top), true, true, true, false, false, true);
            }
            else
            {
                boundingPlatform = new Platform(cmReference, sbReference, gdmReference, imageLibrary, new Vector2(((platformSlope.pt1.Y - platformSlope.pt2.Y) / 16), (platformSlope.getBoundingRectangle.Height / 16)), new Vector2(platformSlope.getBoundingRectangle.Left, platformSlope.getBoundingRectangle.Top), true, true, false, true, false, true);
            }

            layerDepth = lDepth;

            platformBodyHeight = platBodyHeight * 16;
        }

        public void update(GameTime gameTime)
        {
        }

        public void drawAltPosition(Vector2 startSlopePosition, Vector2 endSlopePosition)
        {
            //sbReference.Draw(slopeTexture, new Rectangle((int)(altPos.X + xTracker), (int)(altLine.yAtX(altPos.X-xTracker)), (int)slopeTexture.Width, (int)slopeTexture.Height), null, Color.White, (float)(rotation * (Math.PI / 180)), Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
            Rectangle tempRectangle = platformSlope.getAltBoundingRectangle(startSlopePosition);
            if (startSlopePosition.Y > endSlopePosition.Y)
            {
                tempRectangle = platformSlope.getAltBoundingRectangle(new Vector2(startSlopePosition.X, endSlopePosition.Y));
            }

            Line altLine = new Line(startSlopePosition, endSlopePosition);
            float slope = Math.Abs(altLine.getSlope());
            float rotation = (float)(180 / Math.PI * Math.Atan((platformSlope.pt2.Y - platformSlope.pt1.Y) / (platformSlope.pt2.X - platformSlope.pt1.X)));

            //System.Diagnostics.Debug.WriteLine(rotation);
            float xTracker = 0;

            //System.Diagnostics.Debug.WriteLine("Start X: " + platformSlope.getBoundingRectangle.X);
            //System.Diagnostics.Debug.WriteLine("Start Y: " + startYPos);
            //System.Diagnostics.Debug.WriteLine("Alt Y: " + altPos.Y);
            //System.Diagnostics.Debug.WriteLine("Width: " + platformSlope.getBoundingRectangle.Width);
            //System.Diagnostics.Debug.WriteLine(altLine.getSlope());

            while (tempRectangle.Left + xTracker < tempRectangle.Right)
            {
                if (xTracker == 0)
                {
                   slopeTexture.setFrameConfiguration(1, 1, 1);
                }
                else if (tempRectangle.Left + xTracker != tempRectangle.Right - 16)
                {
                    slopeTexture.setFrameConfiguration(1, 1, 1);
                }
                else if (tempRectangle.Left + xTracker > tempRectangle.Right - 16)
                {
                     slopeTexture.setFrameConfiguration(1, 1, 1);
                }

                if (tempRectangle.Left + xTracker > tempRectangle.Right - 16)
                {
                    slopeTexture.DrawAltPosition(new Vector2(tempRectangle.Right - 16, altLine.yAtX(tempRectangle.Right-16)), false, rotation, layerDepth + .01f);
                }
                else
                {
                    slopeTexture.DrawAltPosition(new Vector2(tempRectangle.Left + xTracker, altLine.yAtX(tempRectangle.Left + xTracker)), false, rotation, layerDepth);
                }


                //draws the platform slope texture                               

                //-------------------------------

                float xTrackerModifier = (float)Math.Abs(Math.Cos(rotation) * 16);
                //System.Diagnostics.Debug.WriteLine(xTrackerModifier);
                if (xTrackerModifier > 16)
                {
                    xTracker += 16;
                }
                else
                {
                    if (slope < 1)
                    {
                        xTracker += xTrackerModifier;
                    }
                    else {
                        xTracker += 8;
                    }
                    
                }
            }
        }

        public Vector2 checkAndFixGrindRailCollision(Vector2 newPos, Player player)
        {
            int heightBuffer = player.height-12;

            Rectangle curRect = new Rectangle((int)newPos.X, (int)(newPos.Y + heightBuffer), (int)player.width, 1);
            Rectangle prevRec = new Rectangle((int)player.previousWorldPosition.X, (int)(player.previousWorldPosition.Y + heightBuffer), (int)player.width, 1);
            Rectangle checkRectangle = new Rectangle((int)newPos.X, (int)(newPos.Y + heightBuffer), (int)player.width, 1);
            Rectangle returnRect;

            returnRect = platformSlope.verticalCollisionResponse(curRect, prevRec);

            if (returnRect.X != checkRectangle.X ||
                returnRect.Y != checkRectangle.Y)
            {
                if (player.actionButtonBeingPressed)
                {
                    //set player state to isGrinding here and have it animate it differently
                    player.isJumping = false;
                    player.jumpExhausted = false;
                    player.isFalling = false;
                    player.isFallingFromGravity = false;
                    player.isDashing = false;
                    player.exhaustedDash = false;

                    if (!player.isGrinding)
                    {
                        player.grindRailReference = this;
                        player.isGrinding = true;
                        if (player.movingLeft || player.movingRight)
                        {
                            player.playerImageSheet.setFrameConfiguration( 56, 56, 56);
                        }
                        else
                        {
                            player.playerImageSheet.setFrameConfiguration(55, 55, 55);
                        }
                    }
                }                
            }
            else
            {
                if (player.grindRailReference != null)
                {
                    if (player.grindRailReference == this)
                    {
                        player.isGrinding = false;
                        player.grindRailReference = null;

                        if (player.isFalling)
                        {
                            if (!player.isDashing) {
                                player.playerImageSheet.setFrameConfiguration(72, 72, 72);
                                player.playerImageSheet.frameTimeLimit = player.defaultFrameTimeLimit;
                            }
                        }
                        else if (player.isFallingFromGravity)
                        {
                            if (!player.isDashing)
                            {
                                player.playerImageSheet.setFrameConfiguration(73, 73, 73);
                                player.playerImageSheet.frameTimeLimit = player.defaultFrameTimeLimit;
                            }
                        }
                        else
                        {
                            if (!player.isJumping)
                            {
                                player.playerImageSheet.setFrameConfiguration(18, 18, 28);
                                player.playerImageSheet.frameTimeLimit = player.defaultFrameTimeLimit;
                            }
                        }
                    }
                }
            }

            if (player.actionButtonBeingPressed)
            {
                return new Vector2(returnRect.X, returnRect.Y - heightBuffer);
            }
            else {
                return newPos;
            }
            
        }
    }
}
