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
    class Hurdle
    {
        ContentManager cmReference;
        GraphicsDeviceManager gdmReference;
        SpriteBatch sbReference;

        AnimatedImage imageSheet;
        public Vector2 worldPosition;

        public float layerDepth;

        public int width {
            get {
                return (int)imageSheet.frameSize.X;
            }
        }

        public int height {
            get {
                return (int)imageSheet.frameSize.Y;
            }
        }

        public Rectangle getBoundingBox {
            get {
                return new Rectangle((int)worldPosition.X, (int)worldPosition.Y, width, height);
            }
        }
        public Hurdle(ContentManager cm, GraphicsDeviceManager gdm, SpriteBatch sb, Vector2 worldPos) {
            cmReference = cm;
            gdmReference = gdm;
            sbReference = sb;

            worldPosition = worldPos;
            imageSheet = new AnimatedImage(cm, sb,gdmReference.GraphicsDevice, "GameObjects/Hurdles/Hurdle", new Vector2(28, 61), new Vector2(1,1));
            layerDepth = .5f;
        }

        public void update(GameTime gameTime, Camera camera) {
            imageSheet.position = worldPosition;
            imageSheet.Update(gameTime);
        }

        public void draw(Camera camera) {
            imageSheet.Draw();
        }

        public void drawAltPosition(Vector2 altPos) {
            imageSheet.DrawAltPosition(altPos, false, layerDepth);
        }

        //this triggers the players action state for the hurdle and because of that sets the player on course to finish a scripted interaction with the object
        public void checkForAndTriggerPlayerActionState(Player player) {
            if (getBoundingBox.Intersects(player.boundingRectangle)) {
                Rectangle playerLeftSide = new Rectangle((int)player.currentWorldPosition.X, (int)player.currentWorldPosition.Y, 1, (int)player.height);
                Rectangle playerRightSide = new Rectangle((int)(player.currentWorldPosition.X + player.width), (int)player.currentWorldPosition.Y, 1, (int)player.height);

                Rectangle hurdleLeftSide = new Rectangle((int)worldPosition.X, (int)worldPosition.Y, width/2, (int)height);
                Rectangle hurdleRightSide = new Rectangle((int)worldPosition.X + width/2, (int)worldPosition.Y, width/2, (int)height);

                if ((hurdleLeftSide.Intersects(playerRightSide) && player.movingRight) ||
                    (hurdleRightSide.Intersects(playerLeftSide) && player.movingLeft))
                {
                    if (player.actionButtonBeingPressed)
                    {
                        if (!player.actionStateIsActive)
                        {
                            //System.Diagnostics.Debug.WriteLine("Hurdle and player state activated");
                            player.actionStateIsActive = true;
                            player.hurdleActionStateActive = true;
                            player.currentActionHurdleReference = this;
                            player.hurdleTimer = 0;
                            player.hurdleTimerMax = 5;
                            player.upTheHurdle = true;
                            if (player.movingLeft)
                            {
                                player.hurdleDistance.X = getBoundingBox.Right - player.boundingRectangle.Left;
                            }
                            if (player.movingRight)
                            {
                                player.hurdleDistance.X = getBoundingBox.Left - player.boundingRectangle.Right;
                            }
                            player.hurdleDistance.Y = (player.boundingRectangle.Bottom - getBoundingBox.Top);
                        }
                        else
                        {
                            if (!player.hurdleActionStateActive)
                            {
                                //System.Diagnostics.Debug.WriteLine("Hurdle state activated");
                                player.hurdleActionStateActive = true;
                                player.currentActionHurdleReference = this;
                                player.hurdleTimer = 0;
                                player.hurdleTimerMax = 5;
                                player.upTheHurdle = true;
                                if (player.movingLeft)
                                {
                                    player.hurdleDistance.X = getBoundingBox.Right - player.boundingRectangle.Left;
                                }
                                if (player.movingRight)
                                {
                                    player.hurdleDistance.X = getBoundingBox.Left - player.boundingRectangle.Right;
                                }
                                player.hurdleDistance.Y = (player.boundingRectangle.Bottom - getBoundingBox.Top);
                            }
                        }
                    }
                }
            }
        }
    }
}
