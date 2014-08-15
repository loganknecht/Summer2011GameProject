using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TestPlatformer
{
    class Images
    {
        public AnimatedImage basicTileSet;

        ContentManager cmReference;
        GraphicsDeviceManager gdmReference;
        SpriteBatch sbReference;

        public Images(ContentManager cm, SpriteBatch sb, GraphicsDeviceManager gdm) {
            cmReference = cm;
            gdmReference = gdm;
            sbReference = sb;
            basicTileSet = new AnimatedImage(cmReference, sb, gdm.GraphicsDevice, "TileMapImages/BasicTileSet", new Vector2(80, 16), new Vector2(5, 1));
        }
    }
}
