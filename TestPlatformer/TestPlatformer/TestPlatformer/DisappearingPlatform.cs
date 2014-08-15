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
    class DisappearingPlatform : Platform
    {
        public bool isVanishing;
        
        public enum VanishDirection { leftToRight, rightToLeft }
        public VanishDirection vanishDirection;

        public int vanishTimer;
        public int vanishTimerMax;

        //----------------------------------------------------------------------------------------------------------------------------------------------------------
        public DisappearingPlatform(ContentManager cm, SpriteBatch sb, GraphicsDeviceManager gdm, ImageLibrary il, Vector2 platSize, Vector2 pos, bool slidableOnL, bool slidableOnR, bool cOnLeft, bool cOnRight, bool cOnTop, bool cOnBottom)
            : base(cm, sb, gdm, il, platSize, pos, slidableOnL, slidableOnR, cOnLeft, cOnRight, cOnTop, cOnBottom)
        {
            isVanishing = false;
            vanishDirection = VanishDirection.rightToLeft;

            vanishTimer = 0;
            vanishTimerMax = 0;

        }

        public override void updatePlatformState() {
            if (isVanishing)
            {
                if (vanishTimer >= vanishTimerMax)
                {
                    //left to right
                    if (vanishDirection == VanishDirection.leftToRight)
                    {
                        if (platformSize.X > 0)
                        {
                            platformSize.X -= 1;
                            position.X += tileWidth;
                        }
                    }
                    //right to left
                    if (vanishDirection == VanishDirection.rightToLeft)
                    {
                        if (platformSize.X > 0)
                        {
                            platformSize.X -= 1;
                        }
                    }
                    vanishTimer = 0;
                }
                else
                {
                    vanishTimer++;
                }
            }
        }

        public override Vector2 checkAndFixPlatformCollision(Vector2 newPos, Player player) {
            Platform tp = this;

            //makes the current platform rectangle reference, not sure if this is done via call by reference
            Rectangle previousTileComparisionRect = tp.boundingRectangle;
            Rectangle currentTileComparisionRect = tp.boundingRectangle;
            Rectangle topTileBorder = new Rectangle((int)(currentTileComparisionRect.Left), currentTileComparisionRect.Top, (int)(currentTileComparisionRect.Width), currentTileComparisionRect.Height);

            //makes player rectangle reference
            Rectangle previousPlayerComparisionRect = new Rectangle((int)newPos.X, (int)newPos.Y, (int)player.width, (int)player.height);
            Rectangle currentPlayerComparisionRect = new Rectangle((int)newPos.X, (int)newPos.Y, (int)player.width, (int)player.height);

            if (topTileBorder.Contains(new Rectangle(currentPlayerComparisionRect.X, (int)currentPlayerComparisionRect.Bottom, currentPlayerComparisionRect.Width, 1)) && !isVanishing && !player.isJumping)
            {
                isVanishing = true;
            }

            return base.checkAndFixPlatformCollision(newPos, player);
        }
    }
}
