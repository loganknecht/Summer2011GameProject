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
    class SplashScreen : Screen
    {
        Texture2D splashScreenImage;

        public SplashScreen(ContentManager cManager, GraphicsDeviceManager gdmReference, SpriteBatch sbReference) : base(cManager, gdmReference, sbReference) {
            splashScreenImage = cmReference.Load<Texture2D>("SplashScreens/SplashScreen");
        }

        public override void updateScreen(GameTime gameTimer) {
        }

        public override void draw(GameTime gameTimer) {
            spriteBatchReference.Begin();
            spriteBatchReference.Draw(splashScreenImage, new Vector2(0, 0), Color.White);
            spriteBatchReference.End();
        }
    }
}
