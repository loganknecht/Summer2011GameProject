using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace TestPlatformer
{
    class TestScreen : Screen
    {
        Color screenColor;

        public TestScreen(ContentManager cReference, GraphicsDeviceManager gdmReference, SpriteBatch sbReference, Color sColor) : base(cReference, gdmReference, sbReference) {
            screenColor = sColor;
        }

        public override void updateScreen(GameTime gameTimer) {
        }

        public override void draw(GameTime gameTimer) {
            gdmReference.GraphicsDevice.Clear(screenColor);
        }
    }
}
