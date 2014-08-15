using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TestPlatformer
{
    public class Screen
    {
        public ContentManager cmReference;
        public GraphicsDeviceManager graphicsDeviceManagerReference;
        public SpriteBatch spriteBatchReference;

        public Screen(ContentManager contentReference, GraphicsDeviceManager gdmReference, SpriteBatch sbReference) {
            cmReference = contentReference;
            graphicsDeviceManagerReference = gdmReference;
            spriteBatchReference = sbReference;
        }

        public virtual void initializeScreen() { }

        public virtual void unloadScreenContent() { }

        public virtual void updateScreen(GameTime gametime) { }

        public virtual void draw(GameTime gameTime) { }
    }
}
