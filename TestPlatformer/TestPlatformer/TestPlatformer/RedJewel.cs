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
    class RedJewel : Treasure
    {
        public RedJewel(ContentManager cm, GraphicsDeviceManager gdm, SpriteBatch sb, ImageLibrary il, Vector2 startCenterPos, float treasureRotation) : base(cm, gdm, sb, il, startCenterPos, treasureRotation) {
            imageSheet = il.redJewelImage;
        }

        public override Treasure checkToActivate(Player player)
        {
            if (player.boundingRectangle.Intersects(getBoundingBox))
            {
                player.cumulativeScore += 100;
                player.redJewelLevelTracker++;
                return this;
            }
            else
            {
                return null;
            }

        }
    }
}
