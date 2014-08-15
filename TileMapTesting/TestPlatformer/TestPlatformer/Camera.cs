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

        public Vector2 worldPosition;
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
                return new Rectangle((int)worldPosition.X, (int)worldPosition.Y, cameraWidth, cameraHeight);
            }
        }

        public Camera(ContentManager cm, GraphicsDeviceManager gdm, SpriteBatch sb, Vector2 pos, int w, int h)
        {
            cmReference = cm;
            gdmReference = gdm;
            sbReference = sb;
            worldPosition = pos;
            screenPosition = new Vector2(0, 0);
            cameraWidth = w;
            cameraHeight = h;

            boundingRectangleColor = cmReference.Load<Texture2D>("CollisionColor");
        }

        public void acceptKeyboardInput(KeyboardState previousKBState, KeyboardState currentKBState) { 
            if(currentKBState.IsKeyDown(Keys.I)) {
                worldPosition.Y -= 5;
            }
            if (currentKBState.IsKeyDown(Keys.J))
            {
                worldPosition.X -= 5;
            }
            if (currentKBState.IsKeyDown(Keys.K))
            {
                worldPosition.Y += 5;
            }
            if (currentKBState.IsKeyDown(Keys.L))
            {
                worldPosition.X += 5;
            }
        }

        public void updateWorldPositionBasedOnPlayer(Vector2 playerMoveDifference)
        {
            worldPosition += playerMoveDifference;
        }

        public void updateWorldPositionBasedOnPlayer(Player player) {
            //left
            if (player.boundingRectangle.Left < getWorldBoundingBox.Left + getWorldBoundingBox.Width * .3)
            {
                worldPosition.X = player.currentWorldPosition.X - (int)(getWorldBoundingBox.Width * .3);
            }
            //top
            if (player.boundingRectangle.Top < getWorldBoundingBox.Top + getWorldBoundingBox.Height * .1)
            {
                worldPosition.Y = player.currentWorldPosition.Y - (int)(getWorldBoundingBox.Height * .1);
            }
            //right
            if (player.boundingRectangle.Right > getWorldBoundingBox.Right - getWorldBoundingBox.Width * .3)
            {
                worldPosition.X = player.boundingRectangle.Right - (int)(getWorldBoundingBox.Width - (getWorldBoundingBox.Width * .3));
            }
            //bottom
            if (player.boundingRectangle.Bottom > getWorldBoundingBox.Bottom - getWorldBoundingBox.Height * .1)
            {
                worldPosition.Y = player.boundingRectangle.Bottom - (int)(getWorldBoundingBox.Height - (getWorldBoundingBox.Height * .1));
            }
        }

        public void updateWorldPositionBasedOnPlayer(Vector2 prevPosition, Vector2 currPosition)
        {
            System.Diagnostics.Debug.WriteLine("prev X: " + prevPosition.X + ", prev Y: " + prevPosition.Y);
            System.Diagnostics.Debug.WriteLine("cur X: " + currPosition.X + ", cur Y: " + currPosition.Y);
            worldPosition.X -= prevPosition.X - currPosition.X;
            worldPosition.Y -= prevPosition.Y - currPosition.Y;
        }
        
        //fixes the players position inside the screens boundings so that the player never leaves the drawing area
        public Vector2 fixPlayerCollisionWithCameraBounds(Vector2 position, Player player) {
            if (position.X < getScreenBoundingBox.Left)
            {
                position.X = getScreenBoundingBox.Left;
            }
            if (position.Y < getScreenBoundingBox.Top)
            {
                position.Y = getScreenBoundingBox.Top;
            }
            if (position.X + player.width > getScreenBoundingBox.Right)
            {
                position.X = getScreenBoundingBox.Right - player.width;
            }
            if (position.Y + player.height > getScreenBoundingBox.Bottom)
            {
                //this is set to false so that when a character lands they can jump again
                //this needs to occur in every bottom bounded collision
                if (player.isFalling)
                {
                    player.isFalling = false;
                    if (player.movingRight || player.movingLeft)
                    {
                        player.playerImageSheet.setFrameConfiguration(18, 18, 28);
                        player.playerImageSheet.frameTimeLimit = 8;
                    }
                }
                if (player.isFallingFromGravity)
                {
                    player.isFallingFromGravity = false;
                    if (player.movingRight || player.movingLeft)
                    {
                        player.playerImageSheet.setFrameConfiguration(18, 18, 28);
                        player.playerImageSheet.frameTimeLimit = 8;
                    }
                    else if (player.standing)
                    {
                        player.playerImageSheet.setFrameConfiguration(0, 0, 4);
                        player.playerImageSheet.frameTimeLimit = 8;
                    }
                }
                if (player.exhaustedDash)
                {
                    player.exhaustedDash = false;
                }
                position.Y = getScreenBoundingBox.Bottom - player.height;
            }
            return position;
        }

        //rectifies collisions between the camera and the edge of the tile map, it will reset the tile map so that it doesn't 
        //draw outside the tile map, keeping everything contained in the camera
        public void fixCameraCollisionWithTileMapBounds(TileMap tm) {
            if (getWorldBoundingBox.Left < tm.getBoundingBox.Left)
            {
                worldPosition.X = tm.getBoundingBox.Left;
            }
            if (getWorldBoundingBox.Top < tm.getBoundingBox.Top)
            {
                worldPosition.Y = tm.getBoundingBox.Top;
            }
            if (getWorldBoundingBox.Right > tm.getBoundingBox.Right)
            {
                worldPosition.X = tm.getBoundingBox.Right-cameraWidth;
            }
            if (getWorldBoundingBox.Bottom > tm.getBoundingBox.Bottom)
            {
                worldPosition.Y = tm.getBoundingBox.Bottom - cameraHeight;
            }
        }
    }
}
