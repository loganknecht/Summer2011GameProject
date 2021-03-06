﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ScreenTest
{
    public class Screen
    {
        public GraphicsDeviceManager graphicsDeviceManagerReference;
        public SpriteBatch spriteBatchReference;

        public Screen(GraphicsDeviceManager gdmReference, SpriteBatch sbReference) {
            graphicsDeviceManagerReference = gdmReference;
            spriteBatchReference = sbReference;
        }

        public virtual void initializeScreen() { }

        public virtual void unloadScreenContent() { }

        public virtual void updateScreen(GameTime gametime) { }

        public virtual void draw(GameTime gameTime) { }
    }
}
