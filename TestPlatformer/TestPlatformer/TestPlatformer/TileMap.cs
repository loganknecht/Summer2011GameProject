using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TestPlatformer
{
    class TileMap
    {
        ContentManager cmReference;
        GraphicsDeviceManager gdmReference;
        SpriteBatch sbReference;

        public int tilePositionX;
        public int tilePositionY;
        public int defaultTileWidth;
        public int defaultTileHeight;
        public int tileMapWidth;
        public int tileMapHeight;

        public BaseTile[,] tiles;

        public Rectangle getBoundingBox {
            get {
                return new Rectangle(0, 0, getTileMapWidthInPixels, getTileMapHeightInPixels);
            }
        }

        public int getTileMapWidthInPixels {
            get {
                return tileMapWidth * defaultTileWidth;
            }
        }

        public int getTileMapHeightInPixels
        {
            get
            {
                return tileMapHeight * defaultTileHeight;
            }
        }

        /// <summary>
        /// Tile map will be a X b Y multi dimensional array that is uniform in their dimensions
        /// </summary>
        /// <param name="widthAndHeightInTiles">Sets the size of the tile map via width in tile and height in tiles</param>
        public TileMap(ContentManager cm, GraphicsDeviceManager gdm, SpriteBatch sb, Vector2 tileMapWidthAndHeight, Vector2 defaultTileWidthAndHeight) {
            tileMapWidth = (int)tileMapWidthAndHeight.X;
            tileMapHeight = (int)tileMapWidthAndHeight.Y;

            tiles = new BaseTile[tileMapWidth, tileMapHeight];

            tilePositionX = 0;
            tilePositionY = 0;
            defaultTileWidth = (int)defaultTileWidthAndHeight.X;
            defaultTileHeight = (int)defaultTileWidthAndHeight.Y;

            cmReference = cm;
            gdmReference = gdm;
            sbReference = sb;
        }

        public void update(GameTime gameTime) { 
            //to do update for each in tiles 
            foreach (BackgroundTile tile in tiles) {
                tile.update(gameTime);
            }
        }

        public void update(GameTime gameTime, Camera camera) {
            int startQuadrantX = (int)(camera.currentWorldPosition.X/defaultTileWidth);
            int startQuadrantY = (int)(camera.currentWorldPosition.Y / defaultTileHeight);

            int endQuadrantX;
            int endQuadrantY;

            int widthTracker = startQuadrantX;
            int heightTracker = startQuadrantY;

            //checks if the last tile hasn't been passed and if it hasn't increases the tiles being drawn by 1 in the X direction
            if (camera.getWorldBoundingBox.Right < getBoundingBox.Right) {
                endQuadrantX = (int)(camera.getWorldBoundingBox.Right / defaultTileWidth + 1);
            }
            else {
                endQuadrantX = (int)(camera.getWorldBoundingBox.Right / defaultTileWidth);
            }
            //checks if the last tile hasn't been passed and if it hasn't increases the tiles being drawn by 1 in the Y direction
            if (camera.getWorldBoundingBox.Bottom < getBoundingBox.Bottom) {
                endQuadrantY = (int)(camera.getWorldBoundingBox.Bottom / defaultTileHeight + 1);
            }
            else {
                endQuadrantY = (int)(camera.getWorldBoundingBox.Bottom / defaultTileHeight);
            }

            while (heightTracker < endQuadrantY) {
                while (widthTracker < endQuadrantX) {
                    tiles[widthTracker, heightTracker].update(gameTime);
                    widthTracker++;
                }
                widthTracker = startQuadrantX;
                heightTracker++;
            }
        }

        public void draw() {
            //to do draw for each in tiles, eventually take in a camera that determines what part of the map to draw, and where to draw it on graphics device/spritebatch
            foreach (BackgroundTile tile in tiles) {
                tile.draw();
            }
        }

        public void draw(Camera camera)
        {
            int startQuadrantX = (int)(camera.currentWorldPosition.X / defaultTileWidth);
            int startQuadrantY = (int)(camera.currentWorldPosition.Y / defaultTileHeight);
            
            int endQuadrantX;
            int endQuadrantY;

            //checks if the last tile hasn't been passed and if it hasn't increases the tiles being drawn by 1 in the X direction
            if (camera.getWorldBoundingBox.Right < getBoundingBox.Right) {
                endQuadrantX = (int)(camera.getWorldBoundingBox.Right / defaultTileWidth+1);
            }
            else {
                endQuadrantX = (int)(camera.getWorldBoundingBox.Right / defaultTileWidth);
            }
            //checks if the last tile hasn't been passed and if it hasn't increases the tiles being drawn by 1 in the Y direction
            if (camera.getWorldBoundingBox.Bottom < getBoundingBox.Bottom) {
                endQuadrantY = (int)(camera.getWorldBoundingBox.Bottom / defaultTileHeight+1);
            }
            else {
                endQuadrantY = (int)(camera.getWorldBoundingBox.Bottom / defaultTileHeight);
            }
            

            int widthTracker = startQuadrantX;
            int heightTracker = startQuadrantY;

            while (heightTracker < endQuadrantY)
            {
                while (widthTracker < endQuadrantX)
                {
                    tiles[widthTracker, heightTracker].drawAltPosition(getWorldToScreenCoord(tiles[widthTracker, heightTracker].position, camera));
                    widthTracker++;
                }
                widthTracker = startQuadrantX;
                heightTracker++;
            }
        }

        public Vector2 checkForAndFixCollidableTileCollisionWithPlayer(Vector2 expectedPosition, Player playerOne, Camera playerCamera) {
            int startQuadrantX = (int)(playerCamera.currentWorldPosition.X / defaultTileWidth);
            int startQuadrantY = (int)(playerCamera.currentWorldPosition.Y / defaultTileHeight);

            int endQuadrantX;
            int endQuadrantY;

            int widthTracker = startQuadrantX;
            int heightTracker = startQuadrantY;

            //checks if the last tile hasn't been passed and if it hasn't increases the tiles being drawn by 1 in the X direction
            if (playerCamera.getWorldBoundingBox.Right < getBoundingBox.Right)
            {
                endQuadrantX = (int)(playerCamera.getWorldBoundingBox.Right / defaultTileWidth + 1);
            }
            else
            {
                endQuadrantX = (int)(playerCamera.getWorldBoundingBox.Right / defaultTileWidth);
            }
            //checks if the last tile hasn't been passed and if it hasn't increases the tiles being drawn by 1 in the Y direction
            if (playerCamera.getWorldBoundingBox.Bottom < getBoundingBox.Bottom)
            {
                endQuadrantY = (int)(playerCamera.getWorldBoundingBox.Bottom / defaultTileHeight + 1);
            }
            else
            {
                endQuadrantY = (int)(playerCamera.getWorldBoundingBox.Bottom / defaultTileHeight);
            }

            while (heightTracker < endQuadrantY)
            {
                while (widthTracker < endQuadrantX)
                {
                    //System.Diagnostics.Debug.WriteLine("Width Tracker: " + widthTracker);
                    //System.Diagnostics.Debug.WriteLine("Width Tracker: " + heightTracker);
                    if (tiles[widthTracker, heightTracker].isCollidable)
                    {
                        expectedPosition = tiles[widthTracker, heightTracker].checkAndFixTileCollisionWithPlayer(expectedPosition, playerOne);
                    }
                    
                    widthTracker++;
                }
                widthTracker = startQuadrantX;
                heightTracker++;
            }
            return expectedPosition;
        }

        public Vector2 fixPlayerCollisionWithMapBoundings(Vector2 newPosition, Player playerOne) {
            if (newPosition.X < getBoundingBox.Left)
            {
                newPosition.X = getBoundingBox.Left;
            }
            if (newPosition.Y < getBoundingBox.Top)
            {
                newPosition.Y = getBoundingBox.Top;
            }
            if (newPosition.X + playerOne.width > getBoundingBox.Right)
            {
                newPosition.X = getBoundingBox.Right - playerOne.width;
            }
            if (newPosition.Y + playerOne.height > getBoundingBox.Bottom)
            {
                newPosition.Y = getBoundingBox.Bottom - playerOne.height;
                
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
            }

            return newPosition;
        }

        //assumes you're just giving straight world coordinates in pixel coords
        public Vector2 getWorldToScreenCoord(Vector2 world, Camera camera) {
            return camera.screenPosition + world - camera.currentWorldPosition;
        }
    }
}