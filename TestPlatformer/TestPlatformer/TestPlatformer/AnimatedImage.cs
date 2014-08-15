using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace TestPlatformer {
    class AnimatedImage {
        public Texture2D imageSheet;
        public Vector2 position;
        public Vector2 frameSize;
        public Vector2 sourceImageSize;

        //so I can just keep track of everything in the class itself
        public SpriteBatch spriteBatchReference;

        //to modify the tile wrapping I have to keep track of the graphics device
        GraphicsDevice graphicsDevice;

        public int columns;
        public int rows;

        public int startFrame;
        public int endFrame;
        public int currentFrame;

        public int frameTimer;
        public int frameTimeLimit;

        public bool isAnimating;
        public bool animateOnce;

        public Rectangle imageBounding {
            get {
                Rectangle r = new Rectangle((int)position.X, (int)position.Y, (int)frameSize.X, (int)frameSize.Y);
                return r;
            }
        }

        public AnimatedImage(ContentManager cm, SpriteBatch sb, GraphicsDevice gd, String imageName, Vector2 sImageSize, Vector2 columnsAndRows) {
            imageSheet = cm.Load<Texture2D>(imageName);
            spriteBatchReference = sb;
            graphicsDevice = gd;

            position = Vector2.Zero;
            frameSize = new Vector2((sImageSize.X/columnsAndRows.X), (sImageSize.Y/columnsAndRows.Y));
            sourceImageSize = sImageSize;

            columns = (int)(columnsAndRows.X);
            rows = (int)(columnsAndRows.Y);
            isAnimating = true;
            animateOnce = false;

            currentFrame = 0;
            startFrame = 0;
            endFrame = 0;

            frameTimer = 0;
            frameTimeLimit = 8;
        }

        //operates on the assumption this is called every 1/60th of a second
        public void Update(GameTime gameTime) {
            if (isAnimating || animateOnce) {
                frameTimer++;
                if (frameTimer > frameTimeLimit) {
                    currentFrame++;
                    if (currentFrame > endFrame) {
                        currentFrame = startFrame;
                        animateOnce = false;
                    }
                    frameTimer = 0;
                }
            }
            else { 
                //don't animate
            }
        }

        public void Draw() {
            Rectangle drawRectangle = new Rectangle();
            drawRectangle.X = (currentFrame % columns) * (int)frameSize.X;
            drawRectangle.Y = (currentFrame / columns) * (int)frameSize.Y;
            drawRectangle.Width = (int)frameSize.X;
            drawRectangle.Height = (int)frameSize.Y;

            
            spriteBatchReference.Draw(imageSheet, position, drawRectangle, Color.White);  
        }

        //this is a generic draw that allows for a horizontal flip overide
        public void DrawAltPosition(Vector2 position, bool fHorizontal, float layerDepth)
        {
            Rectangle drawRectangle = new Rectangle();
            drawRectangle.X = (currentFrame % columns) * (int)frameSize.X;
            drawRectangle.Y = (currentFrame / columns) * (int)frameSize.Y;
            drawRectangle.Width = (int)frameSize.X;
            drawRectangle.Height = (int)frameSize.Y;
            

            if (fHorizontal)
            {
                spriteBatchReference.Draw(imageSheet, new Rectangle((int)position.X, (int)position.Y, (int)frameSize.X, (int)frameSize.Y), drawRectangle, Color.White, 0.0f, new Vector2(0, 0), SpriteEffects.FlipHorizontally, layerDepth);
            }
            else
            {
                spriteBatchReference.Draw(imageSheet, new Rectangle((int)position.X, (int)position.Y, (int)frameSize.X, (int)frameSize.Y), drawRectangle, Color.White, 0.0f, new Vector2(0, 0), SpriteEffects.None, layerDepth);
            }
        }

        //this is a generic draw that allows for a horizontal flip overide
        public void DrawAltPosition(Vector2 position, bool fHorizontal, float rotation, float layerDepth)
        {
            Rectangle drawRectangle = new Rectangle();
            drawRectangle.X = (currentFrame % columns) * (int)frameSize.X;
            drawRectangle.Y = (currentFrame / columns) * (int)frameSize.Y;
            drawRectangle.Width = (int)frameSize.X;
            drawRectangle.Height = (int)frameSize.Y;


            if (fHorizontal)
            {
                spriteBatchReference.Draw(imageSheet, new Rectangle((int)position.X, (int)position.Y, (int)frameSize.X, (int)frameSize.Y), drawRectangle, Color.White, (float)(Math.PI/180*rotation), new Vector2(0,0), SpriteEffects.FlipHorizontally, layerDepth);
            }
            else
            {
                spriteBatchReference.Draw(imageSheet, new Rectangle((int)position.X, (int)position.Y, (int)frameSize.X, (int)frameSize.Y), drawRectangle, Color.White, (float)(Math.PI / 180 * rotation), new Vector2(0,0), SpriteEffects.None, layerDepth);
            }
        }

        //this is a specific implementation of draw being used for tiled images, primarirly the tile platform
        //you must specifcy the horizontal orientation as well
        public void DrawAltPosition(Vector2 position, bool fHorizontal, Vector2 tileLengthHeight, float layerDepth)
        {
            Rectangle drawRectangle = new Rectangle();
            drawRectangle.X = (currentFrame % columns) * (int)frameSize.X;
            drawRectangle.Y = (currentFrame / columns) * (int)frameSize.Y;
            drawRectangle.Width = (int)frameSize.X * (int)tileLengthHeight.X;
            drawRectangle.Height = (int)frameSize.Y * (int)tileLengthHeight.Y;

            //spriteBatchReference.End();
            //spriteBatchReference.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearWrap, DepthStencilState.None, RasterizerState.CullCounterClockwise);
            if (fHorizontal) {
                spriteBatchReference.Draw(imageSheet, position, drawRectangle, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.FlipHorizontally, layerDepth);
            }
            else {
                spriteBatchReference.Draw(imageSheet, position, drawRectangle, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, layerDepth);
            }
            
            //spriteBatchReference.End();
            //spriteBatchReference.Begin();
            
        }

        public void setFrameConfiguration(int cFrame, int sFrame, int eFrame) {
            currentFrame = cFrame;
            startFrame = sFrame;
            endFrame = eFrame;
        }

        public void setFrameConfiguration(int cFrame, int sFrame, int eFrame, int timeLimit)
        {
            currentFrame = cFrame;
            startFrame = sFrame;
            endFrame = eFrame;
            frameTimeLimit = timeLimit;
        }
    }
}
