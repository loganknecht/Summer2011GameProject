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
    class Camera
    {
        public ContentManager cmReference;
        public GraphicsDeviceManager gdmReference;
        public SpriteBatch sbReference;

        public Texture2D boundingRectangleColor;

        public Vector2 previousWorldPosition;
        public Vector2 currentWorldPosition;
        public Vector2 screenPosition;
        public int cameraWidth;
        public int cameraHeight;

        public Rectangle getScreenBoundingBox {
            get {
                return new Rectangle((int)screenPosition.X, (int)screenPosition.Y, cameraWidth, cameraHeight);
            }
        }

        public Rectangle getWorldBoundingBox
        {
            get
            {
                return new Rectangle((int)currentWorldPosition.X, (int)currentWorldPosition.Y, cameraWidth, cameraHeight);
            }
        }

        public Camera(ContentManager cm, GraphicsDeviceManager gdm, SpriteBatch sb, Vector2 pos, int w, int h)
        {
            cmReference = cm;
            gdmReference = gdm;
            sbReference = sb;
            currentWorldPosition = pos;
            screenPosition = new Vector2(0, 0);
            cameraWidth = w;
            cameraHeight = h;

            boundingRectangleColor = cmReference.Load<Texture2D>("CollisionColor");
        }

        public void acceptKeyboardInput(KeyboardState previousKBState, KeyboardState currentKBState) { 
            if(currentKBState.IsKeyDown(Keys.I)) {
                currentWorldPosition.Y -= 5;
            }
            if (currentKBState.IsKeyDown(Keys.J))
            {
                currentWorldPosition.X -= 5;
            }
            if (currentKBState.IsKeyDown(Keys.K))
            {
                currentWorldPosition.Y += 5;
            }
            if (currentKBState.IsKeyDown(Keys.L))
            {
                currentWorldPosition.X += 5;
            }
        }

        public void updateWorldPositionBasedOnPlayer(Vector2 playerMoveDifference)
        {
            currentWorldPosition += playerMoveDifference;
        }

        public void updateWorldPositionBasedOnPlayer(Player playerOne) {
            previousWorldPosition = currentWorldPosition;
            //left
            if (playerOne.boundingRectangle.Left < getWorldBoundingBox.Left + getWorldBoundingBox.Width * .3)
            {
                currentWorldPosition.X = playerOne.currentWorldPosition.X - (int)(getWorldBoundingBox.Width * .3);
            }
            //top
            if (playerOne.boundingRectangle.Top < getWorldBoundingBox.Top + getWorldBoundingBox.Height * .2)
            {
                currentWorldPosition.Y = playerOne.currentWorldPosition.Y - (int)(getWorldBoundingBox.Height * .2);
            }
            //right
            if (playerOne.boundingRectangle.Right > getWorldBoundingBox.Right - getWorldBoundingBox.Width * .3)
            {
                currentWorldPosition.X = playerOne.boundingRectangle.Right - (int)(getWorldBoundingBox.Width - (getWorldBoundingBox.Width * .3));
            }
            //bottom
            if (playerOne.boundingRectangle.Bottom > getWorldBoundingBox.Bottom - getWorldBoundingBox.Height * .2)
            {
                currentWorldPosition.Y = playerOne.boundingRectangle.Bottom - (int)(getWorldBoundingBox.Height - (getWorldBoundingBox.Height * .2));
            }
        }

        public void updateWorldPositionBasedOnPlayer(Vector2 prevPosition, Vector2 currPosition)
        {
            //System.Diagnostics.Debug.WriteLine("prev X: " + prevPosition.X + ", prev Y: " + prevPosition.Y);
            //System.Diagnostics.Debug.WriteLine("cur X: " + currPosition.X + ", cur Y: " + currPosition.Y);
            currentWorldPosition.X -= prevPosition.X - currPosition.X;
            currentWorldPosition.Y -= prevPosition.Y - currPosition.Y;
        }
        
        //fixes the players position inside the screens boundings so that the playerOne never leaves the drawing area
        public Vector2 fixPlayerCollisionWithCameraBounds(Vector2 position, Player playerOne) {
            if (position.X < getScreenBoundingBox.Left)
            {
                position.X = getScreenBoundingBox.Left;
            }
            if (position.Y < getScreenBoundingBox.Top)
            {
                position.Y = getScreenBoundingBox.Top;
            }
            if (position.X + playerOne.width > getScreenBoundingBox.Right)
            {
                position.X = getScreenBoundingBox.Right - playerOne.width;
            }
            if (position.Y + playerOne.height > getScreenBoundingBox.Bottom)
            {
                //this is set to false so that when a character lands they can jump again
                //this needs to occur in every bottom bounded collision
                if (playerOne.isFalling)
                {
                    playerOne.isFalling = false;
                    if (playerOne.movingRight || playerOne.movingLeft)
                    {
                        playerOne.playerImageSheet.setFrameConfiguration(18, 18, 28);
                        playerOne.playerImageSheet.frameTimeLimit = 8;
                    }
                }
                if (playerOne.isFallingFromGravity)
                {
                    playerOne.isFallingFromGravity = false;
                    if (playerOne.movingRight || playerOne.movingLeft)
                    {
                        playerOne.playerImageSheet.setFrameConfiguration(18, 18, 28);
                        playerOne.playerImageSheet.frameTimeLimit = 8;
                    }
                    else if (playerOne.standing)
                    {
                        playerOne.playerImageSheet.setFrameConfiguration(0, 0, 4);
                        playerOne.playerImageSheet.frameTimeLimit = 8;
                    }
                }
                if (playerOne.exhaustedDash)
                {
                    playerOne.exhaustedDash = false;
                }
                position.Y = getScreenBoundingBox.Bottom - playerOne.height;
            }
            return position;
        }

        //rectifies collisions between the camera and the edge of the tile map, it will reset the tile map so that it doesn't 
        //draw outside the tile map, keeping everything contained in the camera
        public void fixCameraCollisionWithTileMapBounds(TileMap tm) {
            if (getWorldBoundingBox.Left < tm.getBoundingBox.Left)
            {
                currentWorldPosition.X = tm.getBoundingBox.Left;
            }
            if (getWorldBoundingBox.Top < tm.getBoundingBox.Top)
            {
                currentWorldPosition.Y = tm.getBoundingBox.Top;
            }
            if (getWorldBoundingBox.Right > tm.getBoundingBox.Right)
            {
                currentWorldPosition.X = tm.getBoundingBox.Right-cameraWidth;
            }
            if (getWorldBoundingBox.Bottom > tm.getBoundingBox.Bottom)
            {
                currentWorldPosition.Y = tm.getBoundingBox.Bottom - cameraHeight;
            }
        }

        //rectifies collisions between the camera and the edge of the tile map, it will reset the tile map so that it doesn't 
        //draw outside the tile map, keeping everything contained in the camera
        public void fixCameraCollisionWithTileMapBounds(LevelMap tm)
        {
            if (getWorldBoundingBox.Left < tm.getBoundingBox.Left)
            {
                currentWorldPosition.X = tm.getBoundingBox.Left;
            }
            if (getWorldBoundingBox.Top < tm.getBoundingBox.Top)
            {
                currentWorldPosition.Y = tm.getBoundingBox.Top;
            }
            if (getWorldBoundingBox.Right > tm.getBoundingBox.Right)
            {
                currentWorldPosition.X = tm.getBoundingBox.Right - cameraWidth;
            }
            if (getWorldBoundingBox.Bottom > tm.getBoundingBox.Bottom)
            {
                currentWorldPosition.Y = tm.getBoundingBox.Bottom - cameraHeight;
            }
        }
    }
}
