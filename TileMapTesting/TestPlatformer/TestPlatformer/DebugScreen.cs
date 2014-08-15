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
    class DebugScreen : Screen
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

        public DebugScreen(ContentManager cm, GraphicsDeviceManager gdm, SpriteBatch sb, Color sColor) : base(cm, gdm, sb)
        {
            //configures default map
            //TO DO: fix tile map to be instan
            map = new TileMap(cm, gdm, sb, new Vector2(160, 45), new Vector2(16, 16));

            playerOneCamera = new Camera(cm, gdm, sb, new Vector2(0, 0), graphicsDeviceManagerReference.GraphicsDevice.Viewport.TitleSafeArea.Width, graphicsDeviceManagerReference.GraphicsDevice.Viewport.TitleSafeArea.Height);
            //playerTwoCamera = new Camera(cm, gdm, sb, new Vector2(0, 0), graphicsDeviceManagerReference.GraphicsDevice.Viewport.TitleSafeArea.Width / 2, graphicsDeviceManagerReference.GraphicsDevice.Viewport.TitleSafeArea.Height);
            //playerTwoCamera.screenPosition = new Vector2(graphicsDeviceManagerReference.GraphicsDevice.Viewport.TitleSafeArea.Width / 2, 0);

            screenColor = sColor;

            playerOne = new Player(cm, sb, gdm);
            //playerTwo = new Player(cm, sb, gdm);
            //playerTwo.currentWorldPosition = playerTwoCamera.worldPosition;

            platformOne = new TiledPlatform(cm, sb, gdm, true, true, true, true);
            platformOne.platformSize = new Vector2(20, 5);
            platformOne.position = new Vector2(map.getTileMapWidthInPixels / 2 - platformOne.platformWidth / 2, map.getTileMapHeightInPixels - platformOne.platformHeight);

            hurdleOne = new Hurdle(cm, gdm, sb);
            hurdleOne.worldPosition = new Vector2(map.getTileMapWidthInPixels / 4, map.getTileMapHeightInPixels - hurdleOne.height);

            hurdleTwo = new Hurdle(cm, gdm, sb);
            hurdleTwo.worldPosition = new Vector2(map.getTileMapWidthInPixels / 2 - hurdleTwo.width / 2, map.getTileMapHeightInPixels - platformOne.platformHeight - hurdleTwo.height);

            momentumButtonOne = new MomentumActionButton(cm, gdm, sb, new Vector2(0, 25));
            momentumButtonOne.worldPosition = new Vector2(map.getTileMapWidthInPixels / 2 - momentumButtonOne.width / 2, platformOne.position.Y - 40);

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
            //playerTwoNewPosition = platformOne.checkAndFixTiledPlatformCollision(playerTwoNewPosition, playerTwo);

            playerOneNewPosition = map.fixPlayerCollisionWithMapBoundings(playerOneNewPosition, playerOne);
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
