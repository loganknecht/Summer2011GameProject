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
    class ParallaxBackground
    {
        ContentManager cmReference;
        GraphicsDeviceManager gdmReference;
        SpriteBatch sbReference;

        bool moveInXDirection;
        bool moveInYDirection;

        float layerDepth;

        int parallaxBackgroundDrawWidth;
        int parallaxBackgroundDrawHeight;

        int parallaxMovementTimer;
        int parallaxMovementTimerMax;

        Texture2D backgroundParallaxImage;
        
        public Rectangle sourceRectangle;

        public Vector2 worldPosition;
        
        Vector2 backgroundSpeed;

        public ParallaxBackground(ContentManager cm, GraphicsDeviceManager gdm, SpriteBatch sb, Texture2D image, int pMovementTimerMax, Vector2 wPosition, Vector2 screenSize, bool moveInX, bool moveInY, float lDepth) {
            cmReference = cm;
            gdmReference = gdm;
            sbReference = sb;

            backgroundParallaxImage = image;

            moveInXDirection = moveInX;
            moveInYDirection = moveInY;

            backgroundSpeed = new Vector2(1, 1);
            layerDepth = lDepth;

            parallaxBackgroundDrawWidth = (int)screenSize.X;
            parallaxBackgroundDrawHeight = (int)screenSize.Y;
            
            parallaxMovementTimer = 0;
            parallaxMovementTimerMax = pMovementTimerMax;

            sourceRectangle = new Rectangle(0, 0, parallaxBackgroundDrawWidth, parallaxBackgroundDrawHeight);

            worldPosition = wPosition;
        }

        //updates based on the camera change from its last work update
        public void updateBasedOnCamera(Camera camera)
        {
            if (parallaxMovementTimer < parallaxMovementTimerMax)
            {
                parallaxMovementTimer++;
            }
            else
            {

                if (moveInXDirection) {
                    sourceRectangle.X += (int)backgroundSpeed.X;
                }
                if (moveInYDirection) {
                    sourceRectangle.Y += (int)backgroundSpeed.Y;
                }

                //X OVERFLOW HANDLING
                if (sourceRectangle.X > backgroundParallaxImage.Width)
                {
                    sourceRectangle.X = 0;
                }
                else if (sourceRectangle.X < 0 - backgroundParallaxImage.Width)
                {
                    sourceRectangle.X = 0;
                }

                //Y OVERFLOW HANDLING
                if (sourceRectangle.Y > backgroundParallaxImage.Height)
                {
                    sourceRectangle.Y = 0;
                }
                else if (sourceRectangle.Y < 0 - backgroundParallaxImage.Height)
                {
                    sourceRectangle.Y = 0;
                }

                parallaxMovementTimer = 0;
            }
        }

        public void drawAltPosition(Vector2 altPosition)
        {
            //sbReference.Draw(backgroundParallaxImage, new Rectangle((int)altPosition.X, (int)altPosition.Y, drawRectangle.Width, drawRectangle.Height), sourceRectangle, Color.White);
            sbReference.Draw(backgroundParallaxImage, new Rectangle((int)altPosition.X, (int)altPosition.Y, parallaxBackgroundDrawWidth, parallaxBackgroundDrawHeight), sourceRectangle, Color.White, 0.0f, new Vector2(0, 0), SpriteEffects.None, layerDepth);
        }
    }
}
