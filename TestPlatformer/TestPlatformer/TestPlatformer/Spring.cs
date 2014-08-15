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
    class Spring
    {
        
        ContentManager cmReference;
        GraphicsDeviceManager gdmReference;
        SpriteBatch sbReference;

        AnimatedImage imageSheet;

        public Vector2 worldPosition;
        public Vector2 momentumImparted;

        public float layerDepth;
        public float rotation;

        public int width {
            get {
                return (int)imageSheet.frameSize.X;
            }
        }

        public int height {
            get {
                return (int)imageSheet.frameSize.Y;
            }
        }

        public Rectangle getBoundingBox {
            get {
                return new Rectangle((int)worldPosition.X, (int)worldPosition.Y, width, height);
            }
        }
        public Spring(ContentManager cm, GraphicsDeviceManager gdm, SpriteBatch sb, ImageLibrary il, Vector2 worldPos, Vector2 momentumToImpart, float springRotation)
        {
            cmReference = cm;
            gdmReference = gdm;
            sbReference = sb;

            //probably going to have to have each spring load its own separate texture because this will make it do weird drawing for multiple springs
            imageSheet = new AnimatedImage(cm, sb, gdm.GraphicsDevice, "GameObjects/Springs/SonicSpring", new Vector2(190, 38), new Vector2(5, 1));
            imageSheet.setFrameConfiguration(0, 0, 3);
            imageSheet.frameTimeLimit = 1;
            imageSheet.isAnimating = false;

            layerDepth = .5f;
            rotation = springRotation;

            worldPosition = worldPos;
            momentumImparted = momentumToImpart;
        }

        public void update(GameTime gameTime, Camera camera) {
            imageSheet.Update(gameTime);
        }

        public void draw(Camera camera) {
            imageSheet.Draw();
        }

        public void drawAltPosition(Vector2 altPos) {
            imageSheet.DrawAltPosition(altPos, false, rotation, layerDepth);
        }

        public void checkToActivateSpring(Player playerOne) {
            if (getBoundingBox.Intersects(playerOne.boundingRectangle))
            {
                playerOne.isFallingFromGravity = true;
                playerOne.momentum = momentumImparted;
                imageSheet.animateOnce = true;
                playerOne.isDashing = false;
                playerOne.exhaustedDash = false;
            }
        }
    }
}
