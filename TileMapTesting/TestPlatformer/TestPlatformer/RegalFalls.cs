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
    class RegalFalls : Screen
    {
        //camera for the player
        Camera playerOneCamera;
        //Camera playerTwoCamera;

        Color screenColor;

        //player one
        Player playerOne;
        //player two
        //Player playerTwo;

        //test platform
        TiledPlatform platformOne;
        TiledPlatform platformTwo;
        TiledPlatform platformThree;
        TiledPlatform platformFour;
        TiledPlatform platformFive;

        //Hurdles
        Hurdle hurdleOne;
        Hurdle hurdleTwo;

        //Momentum Action Button Test
        MomentumActionButton momentumButtonOne;

        //keeps track of previous and current update screen presses
        KeyboardState previousKeyboardState;
        KeyboardState currentKeyboardState;

        //tracks gravity, want it to be an environment class eventually
        public Vector2 gravity;

        //The "world"/tilemap that the player exists in. If it's not located around the player it automatically draws to the player's bounding, provided you call the method of course
        public TileMap map;

        public RegalFalls(ContentManager cm, GraphicsDeviceManager gdm, SpriteBatch sb, Color sColor) : base(cm, gdm, sb)
        {
            //configures default map
            map = new TileMap(cm, gdm, sb, new Vector2(160, 45), new Vector2(16, 16));
            //----------------------------------------------------------Start Map Configuration----------------------------------------
            int tileTrackerX = 0;
            int tileTrackerY = 0;

            //iterates through row by column and initializes based on some arbitrary distinctions
            while (tileTrackerY < map.tileMapHeight)
            {
                while (tileTrackerX < map.tileMapWidth)
                {
                    /*
                    if (tileTrackerX == map.tileMapWidth / 2)
                    {
                        map.tiles[tileTrackerX, tileTrackerY] = new BackgroundTile(cm, gdm, sb, new Vector2(map.tilePositionX, map.tilePositionY), new Vector2(80, 16), new Vector2(5, 1), "TileMapImages/AltBasicTileSet", true, true, false, false);
                        map.tiles[tileTrackerX, tileTrackerY].isCollidable = true;
                        map.tiles[tileTrackerX, tileTrackerY].baseTileImage.setFrameConfiguration(4, 4, 4);
                    }
                    else if (tileTrackerY == map.tileMapHeight / 2)
                    {
                        map.tiles[tileTrackerX, tileTrackerY] = new BackgroundTile(cm, gdm, sb, new Vector2(map.tilePositionX, map.tilePositionY), new Vector2(80, 16), new Vector2(5, 1), "TileMapImages/AltBasicTileSet", false, false, true, true);
                        map.tiles[tileTrackerX, tileTrackerY].isCollidable = true;
                        map.tiles[tileTrackerX, tileTrackerY].baseTileImage.setFrameConfiguration(4, 4, 4);
                    }*/
                    //else {
                    map.tiles[tileTrackerX, tileTrackerY] = new BackgroundTile(cm, gdm, sb, new Vector2(map.tilePositionX, map.tilePositionY), new Vector2(80, 16), new Vector2(5, 1), "TileMapImages/AltBasicTileSet", false, false, false, false);

                    //bottom
                    if (tileTrackerX > map.tileMapWidth / 2)
                    {
                        //bottom right
                        if (tileTrackerY > map.tileMapHeight / 2)
                        {
                            map.tiles[tileTrackerX, tileTrackerY].baseTileImage.setFrameConfiguration(3, 3, 3);
                        }
                        //bottom left
                        else
                        {
                            map.tiles[tileTrackerX, tileTrackerY].baseTileImage.setFrameConfiguration(2, 2, 2);
                        }
                    }
                    //top
                    else
                    {
                        //top right
                        if (tileTrackerY > map.tileMapHeight / 2)
                        {
                            map.tiles[tileTrackerX, tileTrackerY].baseTileImage.setFrameConfiguration(1, 1, 1);
                        }
                        //top left
                        else
                        {
                            map.tiles[tileTrackerX, tileTrackerY].baseTileImage.setFrameConfiguration(0, 0, 0);
                        }
                    }
                    //}
                    //tiles[tileTrackerX, tileTrackerY] = new BackgroundTile(cm, gdm, sb, new Vector2(tilePositionX, tilePositionY), new Vector2(80, 16), new Vector2(5, 1), "TileMapImages/BasicTileSet");
                    
                    //tiles[tileTrackerX, tileTrackerY] = new BackgroundTile(cm, gdm, sb, new Vector2(tilePositionX, tilePositionY), imageLibrary.basicTileSet);
                    map.tilePositionX += map.defaultTileWidth;

                    tileTrackerX++;
                }
                map.tilePositionX = 0;
                tileTrackerX = 0;
                map.tilePositionY += map.defaultTileHeight;
                tileTrackerY++;
            }
            //----------------------------------------------------------End Map Configuration----------------------------------------
            playerOneCamera = new Camera(cm, gdm, sb, new Vector2(0, 0), graphicsDeviceManagerReference.GraphicsDevice.Viewport.TitleSafeArea.Width, graphicsDeviceManagerReference.GraphicsDevice.Viewport.TitleSafeArea.Height);
            //playerTwoCamera = new Camera(cm, gdm, sb, new Vector2(0, 0), graphicsDeviceManagerReference.GraphicsDevice.Viewport.TitleSafeArea.Width / 2, graphicsDeviceManagerReference.GraphicsDevice.Viewport.TitleSafeArea.Height);
            //playerTwoCamera.screenPosition = new Vector2(graphicsDeviceManagerReference.GraphicsDevice.Viewport.TitleSafeArea.Width / 2, 0);

            screenColor = sColor;

            playerOne = new Player(cm, sb, gdm);
            playerOne.previousWorldPosition = new Vector2(0, map.getTileMapHeightInPixels - playerOne.height);
            playerOne.currentWorldPosition = new Vector2(0, map.getTileMapHeightInPixels - playerOne.height);
            //playerTwo = new Player(cm, sb, gdm);
            //playerTwo.currentWorldPosition = playerTwoCamera.worldPosition;

            platformOne = new TiledPlatform(cm, sb, gdm, true, true, false, false);
            platformOne.platformSize = new Vector2(5, 5);
            platformOne.position = new Vector2(map.getTileMapWidthInPixels/2-platformOne.platformWidth/2, map.getTileMapHeightInPixels - platformOne.platformHeight);

            platformTwo = new TiledPlatform(cm, sb, gdm, true, true, false, false);
            platformTwo.platformSize = new Vector2(5, 5);
            platformTwo.position = new Vector2(map.getTileMapWidthInPixels / 2 - platformOne.platformWidth / 2, map.getTileMapHeightInPixels - platformOne.platformHeight - platformTwo.platformHeight);

            platformThree = new TiledPlatform(cm, sb, gdm, false, false, true, false);
            platformThree.platformSize = new Vector2(5, 5);
            platformThree.position = new Vector2(map.getTileMapWidthInPixels / 2 - platformOne.platformWidth / 2, map.getTileMapHeightInPixels - platformOne.platformHeight - platformTwo.platformHeight-platformThree.platformHeight);

            platformFour = new TiledPlatform(cm, sb, gdm, true, false, true, true);
            platformFour.platformSize = new Vector2(5, 5);
            platformFour.position = new Vector2(map.getTileMapWidthInPixels / 2 - platformOne.platformWidth / 2 - platformThree.platformWidth, map.getTileMapHeightInPixels - platformOne.platformHeight - platformTwo.platformHeight - platformThree.platformHeight);

            platformFive = new TiledPlatform(cm, sb, gdm, false, true, true, true);
            platformFive.platformSize = new Vector2(5, 5);
            platformFive.position = new Vector2(map.getTileMapWidthInPixels / 2 - platformOne.platformWidth / 2 + platformThree.platformWidth, map.getTileMapHeightInPixels - platformOne.platformHeight - platformTwo.platformHeight - platformThree.platformHeight);

            hurdleOne = new Hurdle(cm, gdm, sb);
            hurdleOne.worldPosition = new Vector2(map.getTileMapWidthInPixels / 4, map.getTileMapHeightInPixels - hurdleOne.height);

            hurdleTwo = new Hurdle(cm, gdm, sb);
            hurdleTwo.worldPosition = new Vector2(map.getTileMapWidthInPixels/2 - 400, map.getTileMapHeightInPixels - platformOne.platformHeight - hurdleTwo.height);

            momentumButtonOne = new MomentumActionButton(cm, gdm, sb, new Vector2(15, -25));
            momentumButtonOne.worldPosition = new Vector2(map.getTileMapWidthInPixels / 2 - momentumButtonOne.width / 2, platformOne.position.Y - 200);

            gravity = new Vector2(0, 9);
        }

        public override void updateScreen(GameTime gameTime)
        {
            ///--------------------------------------------------
            /// sets previous state to current, then grabs the now current state
            ///--------------------------------------------------
            previousKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();
            
            ///--------------------------------------------------
            ///update based of key press phase
            ///--------------------------------------------------
            //playerOneCamera.acceptKeyboardInput(previousKeyboardState, currentKeyboardState);
            //playerTwoCamera.acceptKeyboardInput(previousKeyboardState, currentKeyboardState);

            playerOne.checkForPlayerInput(previousKeyboardState, currentKeyboardState);
            //playerTwo.checkForPlayerInput(previousKeyboardState, currentKeyboardState);

            ///---------------------------------------------------
            /// Update player position phase
            /// - grab the calculatedPosition from the player, this calculates the next position for the player and returns the vector2
            /// - take the playerOneNewPosition that you got from calculateplayerOneNewPosition and put it into the collision checking of the objects you're concerned about (map bounding, camera bounding, collisions etc)
            /// - update the player's currentWorldPosition to the playerOneNewPosition, 
            /// - call the player's update command
            ///--------------------------------------------------
            Vector2 playerOneNewPosition = playerOne.calculateNewPosition(gravity);
            //Vector2 playerTwoNewPosition = playerTwo.calculateNewPosition(gravity);

            playerOneNewPosition = platformOne.checkAndFixTiledPlatformCollision(playerOneNewPosition, playerOne);
            playerOneNewPosition = platformTwo.checkAndFixTiledPlatformCollision(playerOneNewPosition, playerOne);
            playerOneNewPosition = platformThree.checkAndFixTiledPlatformCollision(playerOneNewPosition, playerOne);
            playerOneNewPosition = platformFour.checkAndFixTiledPlatformCollision(playerOneNewPosition, playerOne);
            playerOneNewPosition = platformFive.checkAndFixTiledPlatformCollision(playerOneNewPosition, playerOne);
            //playerTwoNewPosition = platformOne.checkAndFixTiledPlatformCollision(playerTwoNewPosition, playerTwo);

            playerOneNewPosition = map.fixPlayerCollisionWithMapBoundings(playerOneNewPosition, playerOne);
            playerOneNewPosition = map.checkForAndFixCollidableTileCollisionWithPlayer(playerOneNewPosition, playerOne, playerOneCamera);

            //playerTwoNewPosition = map.fixPlayerCollisionWithMapBoundings(playerTwoNewPosition, playerTwo);
            //playerOneNewPosition = playerOneCamera.fixPlayerCollisionWithCameraBounds(playerOneNewPosition, playerOne);
            
            playerOne.currentWorldPosition = playerOneNewPosition;
            //playerTwo.currentWorldPosition = playerTwoNewPosition;
            //This is only used to contain the player inside the drawing window, not needed if camera bounds and map bounding are handled
            //playerOne.checkAndFixWindowCollision();
            
            playerOne.Update(gameTime);
            //playerTwo.Update(gameTime);

            ///--------------------------------------------------
            /// Update world object positions
            /// This is the area where the world objects will perform their variations of updating, in some cirucmstances that may involve calculating a new position
            /// In other circumastances it may just involve updating
            ///--------------------------------------------------
            platformOne.Update(gameTime, gravity);
            platformTwo.Update(gameTime, gravity);
            platformThree.Update(gameTime, gravity);
            platformFour.Update(gameTime, gravity);
            platformFive.Update(gameTime, gravity);

            //Hurdle
            hurdleOne.update(gameTime, playerOneCamera);
            hurdleOne.checkForAndTriggerPlayerActionState(playerOne);

            hurdleTwo.update(gameTime, playerOneCamera);
            hurdleTwo.checkForAndTriggerPlayerActionState(playerOne);

            momentumButtonOne.update(gameTime, playerOneCamera);
            momentumButtonOne.checkForAndTriggerPlayerState(playerOne);

            ///--------------------------------------------------
            /// Update the camera's logic here last
            /// - updateWorldPositionBasedOnPlayer updates the camera so it orients around the player passed in
            /// - fixCameraCollisionWithTileMapBounds makes it so that the camera will never draw over the boundaries of the tile map, if it exceeds the dimensions the map is adjusted accordingly
            ///--------------------------------------------------
            playerOneCamera.updateWorldPositionBasedOnPlayer(playerOne);
            playerOneCamera.fixCameraCollisionWithTileMapBounds(map);

            //playerTwoCamera.updateWorldPositionBasedOnPlayer(playerTwo);
            //playerTwoCamera.fixCameraCollisionWithTileMapBounds(map);

            ///--------------------------------------------------
            ///  Update the map accordingly
            ///--------------------------------------------------
            map.update(gameTime, playerOneCamera);
            //map.update(gameTime, playerTwoCamera);
        }

        /// The draw order will be 
        /// - 1 Map
        /// - 2 Objects
        /// - 3 Enemies
        /// - 4 Players
        public override void draw(GameTime gameTime)
        {
            RasterizerState rState = new RasterizerState();
            rState.CullMode = CullMode.None;
            rState.ScissorTestEnable = true;

            //graphicsDeviceManagerReference.GraphicsDevice.Clear(screenColor);

            spriteBatchReference.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearWrap, DepthStencilState.None, rState);
            graphicsDeviceManagerReference.GraphicsDevice.ScissorRectangle = playerOneCamera.getScreenBoundingBox; 

            //playerOne
            map.draw(playerOneCamera);
            playerOne.DrawAltPosition(map.getWorldToScreenCoord(playerOne.currentWorldPosition, playerOneCamera));
            playerOne.drawBoundingBox(map.getWorldToScreenCoord(playerOne.currentWorldPosition, playerOneCamera));

            platformOne.DrawAltPosition(map.getWorldToScreenCoord(platformOne.position, playerOneCamera));
            platformTwo.DrawAltPosition(map.getWorldToScreenCoord(platformTwo.position, playerOneCamera));
            platformThree.DrawAltPosition(map.getWorldToScreenCoord(platformThree.position, playerOneCamera));
            platformFour.DrawAltPosition(map.getWorldToScreenCoord(platformFour.position, playerOneCamera));
            platformFive.DrawAltPosition(map.getWorldToScreenCoord(platformFive.position, playerOneCamera));

            hurdleOne.drawAltPosition(map.getWorldToScreenCoord(hurdleOne.worldPosition, playerOneCamera));
            hurdleTwo.drawAltPosition(map.getWorldToScreenCoord(hurdleTwo.worldPosition, playerOneCamera));
            momentumButtonOne.drawAltPosition(map.getWorldToScreenCoord(momentumButtonOne.worldPosition, playerOneCamera));
            spriteBatchReference.End();


            //player 2 draw section
            //spriteBatchReference.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearWrap, DepthStencilState.None, rState);
            //graphicsDeviceManagerReference.GraphicsDevice.ScissorRectangle = playerTwoCamera.getScreenBoundingBox; 
            //playerTwo
            //map.draw(playerTwoCamera);
            //playerTwo.DrawAltPosition(map.getWorldToScreenCoord(playerTwo.currentWorldPosition, playerTwoCamera));
            //platformOne.DrawAltPosition(map.getWorldToScreenCoord(platformOne.position, playerTwoCamera));
            //spriteBatchReference.End();
        }
    }
}
