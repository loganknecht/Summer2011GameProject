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
        public GraphicsDeviceManager gdmReference;
        public SpriteBatch sbReference;

        public Screen(ContentManager cm, GraphicsDeviceManager gdm, SpriteBatch sb) {
            cmReference = cm;
            gdmReference = gdm;
            sbReference = sb;
        }

        public virtual void initializeScreen() { }

        public virtual void unloadScreenContent() { }

        public virtual void updateScreen(GameTime gametime) { }

        public virtual void draw(GameTime gameTime) { }
    }
}
