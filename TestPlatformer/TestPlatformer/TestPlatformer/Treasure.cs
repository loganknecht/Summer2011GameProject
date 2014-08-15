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
    class Treasure
    {

        ContentManager cmReference;
        GraphicsDeviceManager gdmReference;
        SpriteBatch sbReference;

        public AnimatedImage imageSheet;

        public Vector2 centerPosition;
        public Vector2 startCenterPosition;

        public float layerDepth;
        public float rotation;

        public bool animatingUp;
        public bool animatingDown;

        public int floatIncrementTimer;
        public int floatIncrementTimerMax;

        public int width
        {
            get
            {
                return (int)imageSheet.frameSize.X;
            }
        }

        public int height
        {
            get
            {
                return (int)imageSheet.frameSize.Y;
            }
        }

        public Rectangle getBoundingBox
        {
            get
            {
                return new Rectangle((int)centerPosition.X-width/2, (int)centerPosition.Y-height/2, width, height);
            }
        }
        public Treasure(ContentManager cm, GraphicsDeviceManager gdm, SpriteBatch sb, ImageLibrary il, Vector2 startCenterPos, float treasureRotation)
        {
            cmReference = cm;
            gdmReference = gdm;
            sbReference = sb;

            //probably going to have to have each spring load its own separate texture because this will make it do weird drawing for multiple springs
            imageSheet = il.goldJewelImage;
            imageSheet.setFrameConfiguration(0, 0, 4);
            imageSheet.isAnimating = false;

            layerDepth = .5f;
            rotation = treasureRotation;

            centerPosition = startCenterPos;
            startCenterPosition = startCenterPos;

            animatingUp = false;
            animatingDown = true;

            floatIncrementTimer = 0;
            floatIncrementTimerMax = 3;
        }

        public void update(GameTime gameTime, Camera camera)
        {
            floatIncrementTimer++;

            if (animatingDown)
            {
                if (floatIncrementTimer > floatIncrementTimerMax)
                {
                    centerPosition.Y += 1;
                    floatIncrementTimer = 0;
                }
                if (centerPosition.Y > startCenterPosition.Y + 3)
                {
                    animatingUp = true;
                    animatingDown = false;
                }
            }
            else if (animatingUp)
            {
                if (floatIncrementTimer > floatIncrementTimerMax)
                {
                    centerPosition.Y -= 1;
                    floatIncrementTimer = 0;
                }
                if (centerPosition.Y < startCenterPosition.Y - 3)
                {
                    animatingDown = true;
                    animatingUp = false;
                }
            }
            imageSheet.Update(gameTime);
        }

        public void drawAltPosition(Vector2 altPos)
        {
            imageSheet.DrawAltPosition(altPos, false, rotation, layerDepth);
        }

        public virtual Treasure checkToActivate(Player player)
        {
            if(player.boundingRectangle.Intersects(getBoundingBox)) {
                return this;
            }
            else {
                return null;
            }
            
        }
    }
}
