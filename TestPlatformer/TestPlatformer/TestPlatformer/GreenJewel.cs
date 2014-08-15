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
    class GreenJewel : Treasure
    {
        public GreenJewel(ContentManager cm, GraphicsDeviceManager gdm, SpriteBatch sb, ImageLibrary il, Vector2 startCenterPos, float treasureRotation)
            : base(cm, gdm, sb, il, startCenterPos, treasureRotation)
        {
            imageSheet = il.greenJewelImage;
        }

        public override Treasure checkToActivate(Player player)
        {
            if (player.boundingRectangle.Intersects(getBoundingBox))
            {
                player.cumulativeScore += 100;
                player.greenJewelLevelTracker++;
                return this;
            }
            else
            {
                return null;
            }

        }
    }
}
