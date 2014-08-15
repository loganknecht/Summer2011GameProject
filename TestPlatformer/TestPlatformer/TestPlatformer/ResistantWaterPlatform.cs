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
    class ResistantWaterPlatform : WaterPlatform
    {
        float resistance;

        public ResistantWaterPlatform(ContentManager cm, SpriteBatch sb, GraphicsDeviceManager gdm, ImageLibrary il, Vector2 platSize, Vector2 pos, float resist, bool slidableOnL, bool slidableOnR, bool cOnLeft, bool cOnRight, bool cOnTop, bool cOnBottom)
            : base(cm, sb, gdm, il, platSize, pos, slidableOnL, slidableOnR, cOnLeft, cOnRight, cOnTop, cOnBottom)
        {
            topWater.imageSheet = il.resisitantWaterCrestTile.imageSheet;
            
            //moves playerOne to the left
            if (resist < 0)
            {
                flipTopWaterCrest = true;
            }
            else {
                flipTopWaterCrest = false;
            }

            resistance = resist;
        }

        public override Vector2 checkAndFixPlatformCollision(Vector2 newPos, Player playerOne)
        {
            if(boundingRectangle.Intersects(playerOne.boundingRectangle)) {
                if (playerOne.boundingRectangle.Bottom < boundingRectangle.Top+10 && playerOne.boundingRectangle.Bottom >  boundingRectangle.Top - 10)
                {
                    playerOne.momentum.X = 0;
                }
                else {
                    playerOne.momentum.X = resistance;
                }
            }

            return base.checkAndFixPlatformCollision(newPos, playerOne);
        }
    }
}
