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
    class TightRope
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

        public TightRope(ContentManager cm, GraphicsDeviceManager gdm, SpriteBatch sb, ImageLibrary il, Vector2 startSlopePos, Vector2 endSlopePos, int platBodyHeight, float lDepth)
        {
            cmReference = cm;
            gdmReference = gdm;
            imageLibrary = il;
            sbReference = sb;

            platformSlope = new Line(startSlopePos, endSlopePos);
            slopeTexture = il.testTightRope;
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
                    slopeTexture.DrawAltPosition(new Vector2(tempRectangle.Right - 16, altLine.yAtX(tempRectangle.Right - 16)), false, rotation, layerDepth + .01f);
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
                else
                {
                    xTracker += xTrackerModifier;
                }
            }
        }

        public Vector2 checkAndFixTightRopeCollision(Vector2 newPos, Player player)
        {
            int heightBuffer;
            if (player.actionButtonBeingPressed)
            {
                if (platformSlope.getSlope() == 0)
                {
                    heightBuffer = player.height - 12;
                }
                else {
                    heightBuffer = player.height - 20;
                }
            }
            else {
                heightBuffer = 0;
            }

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

                    player.isJumping = false;
                    player.isFalling = false;
                    player.isFallingFromGravity = false;
                    player.exhaustedDash = false;
                }
                else
                {
                    //set player state to isHanging here and have it animate it differently
                    player.isJumping = false;
                    player.isFalling = false;
                    player.isFallingFromGravity = false;
                    player.exhaustedDash = false;

                    if (!player.isHanging)
                    {
                        player.tightRopeReference = this;
                        player.isHanging = true;
                        if (player.movingLeft || player.movingRight)
                        {
                            player.playerImageSheet.setFrameConfiguration(91, 91, 94);
                        }
                        else {
                            player.playerImageSheet.setFrameConfiguration(91, 91, 91);
                        }
                        
                    }
                }
            }
            else {
                if (player.tightRopeReference != null) {
                    if (player.tightRopeReference == this) {
                        player.isHanging = false;
                        player.tightRopeReference = null;
                        player.isFallingFromGravity = true;

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
                            if (player.isDashing) {
                                player.playerImageSheet.setFrameConfiguration(18, 18, 28);
                                player.playerImageSheet.frameTimeLimit = player.defaultFrameTimeLimit;
                            }
                        }
                    }
                }
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
