using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ScreenTest
{
    class TestScreen : Screen
    {
        Color screenColor;

        public TestScreen(GraphicsDeviceManager gdmReference, SpriteBatch sbReference, Color sColor) : base(gdmReference, sbReference) {
            screenColor = sColor;
        }

        public override void updateScreen(GameTime gameTimer) {
            graphicsDeviceManagerReference.GraphicsDevice.Clear(screenColor);
        }

        public override void draw(GameTime gameTimer) {
        }
    }
}
