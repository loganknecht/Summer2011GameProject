using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TestPlatformer
{
    /// <summary>
    /// This is the background tile class, its width and height are derived from the animated image it tracks, the position vector is passed on
    /// to the base class' constructor so it can set the default position vector, the draw and update are just the starndard updates that are overridded to draw the screen position of their modified world position
    /// This is meant to be a boring class with simple functionality so that I can extend it to other classes and just have them update based on certain configurations
    /// </summary>
    class BackgroundTile : BaseTile
    {
        public int getTileWidth {
            get {
                return (int)baseTileImage.frameSize.X;
            }
        }

        public int getTileHeight
        {
            get
            {
                return (int)baseTileImage.frameSize.Y;
            }
        }

        public Rectangle getBoundingBox {
            get {
                return new Rectangle((int)position.X, (int)position.Y, getTileWidth, getTileHeight);
            }
        }
        public BackgroundTile(ContentManager cm, GraphicsDeviceManager gdm, SpriteBatch sb, Vector2 pos, AnimatedImage imageName) : base(cm, gdm, sb, pos, imageName)
        {
        }

        public BackgroundTile(ContentManager cm, GraphicsDeviceManager gdm, SpriteBatch sb, Vector2 pos, Vector2 sourceImageSize, Vector2 sourceImageColumnAndRowSize, String imageName, bool cOnLeft, bool cOnRight, bool cOnTop, bool cOnBottom) : base(cm, gdm, sb, pos, sourceImageSize, sourceImageColumnAndRowSize, imageName, cOnLeft, cOnRight, cOnTop, cOnBottom)
        {
        }
    }
}
