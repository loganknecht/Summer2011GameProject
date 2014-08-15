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
    class GrassPlatform : Platform
    {
        //----------------------------------------------------------------------------------------------------------------------------------------------------------
        public GrassPlatform(ContentManager cm, SpriteBatch sb, GraphicsDeviceManager gdm, ImageLibrary il, Vector2 platSize, Vector2 pos, bool slidableOnL, bool slidableOnR, bool cOnLeft, bool cOnRight, bool cOnTop, bool cOnBottom)
            : base(cm, sb, gdm, il, platSize, pos, slidableOnL, slidableOnR, cOnLeft, cOnRight, cOnTop, cOnBottom)
        {
            completeOutsidePlatformImage = il.smwOutsideGrass;
        }
    }
}
