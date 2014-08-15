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
    class BasicLevel : Screen
    {
        public NextLevelPointer nextLevel;

        //camera for the playerOne
        Camera playerOneCamera;
        //Camera playerTwoCamera;

        //playerOne one
        public Player playerOne;
        //playerOne two
        //playerOne playerTwo;

        List<ParallaxBackground> parallaxBackgrounds;

        List<Platform> platforms;
        List<Platform> platformsToRemove;

        List<SlopedPlatform> slopedPlatforms;

        List<GrindRail> grindRails;

        List<Spring> springs;

        List<Treasure> worldTreasure;
        List<Treasure> worldTreasureToRemove;

        public EventTrigger endOfLevelTrigger;

        //keeps track of previous and current update screen presses
        KeyboardState previousKeyboardState;
        KeyboardState currentKeyboardState;

        GamePadState playerOnePreviousGamePadState;
        GamePadState playerOneCurrentGamePadState;

        //tracks gravity, want it to be an environment class eventually
        public Vector2 gravity;

        //The "world"/tilemap that the playerOne exists in. If it's not located around the playerOne it automatically draws to the playerOne's bounding, provided you call the method of course
        public LevelMap map;
        public ImageLibrary imageLibrary;

        public SpriteFont scoreFont;

        public BasicLevel(ContentManager cm, GraphicsDeviceManager gdm, SpriteBatch sb, ImageLibrary iLibrary, Player pOne)
            : base(cm, gdm, sb)
        {
            nextLevel = NextLevelPointer.WorldOneLevelOne;

            cmReference = cm;
            gdmReference = gdm;
            sbReference = sb;

            imageLibrary = iLibrary;

            parallaxBackgrounds = new List<ParallaxBackground>();

            platforms = new List<Platform>();
            platformsToRemove = new List<Platform>();

            slopedPlatforms = new List<SlopedPlatform>();

            grindRails = new List<GrindRail>();

            springs = new List<Spring>();

            worldTreasure = new List<Treasure>();
            worldTreasureToRemove = new List<Treasure>();

            endOfLevelTrigger = new EventTrigger();

            //System.Diagnostics.Debug.WriteLine(testPlatform.platformSlope.getSlope());

            //configures default map
            map = new LevelMap(cm, gdm, sb, new Vector2(80, 45), new Vector2(16, 16));
            //----------------------------------------------------------Start Map Configuration----------------------------------------
            int tileTrackerX = 0;
            int tileTrackerY = 0;

            //iterates through row by column and initializes based on some arbitrary distinctions
            while (tileTrackerY < map.tileMapHeight)
            {
                while (tileTrackerX < map.tileMapWidth)
                {
                    map.tiles[tileTrackerX, tileTrackerY] = new BackgroundTile(cm, gdm, sb, new Vector2(map.tilePositionX, map.tilePositionY), new Vector2(16, 16), new Vector2(1, 1), imageLibrary.basicBlueTile, false, false, false, false);
                    map.tilePositionX += map.defaultTileWidth;
                    tileTrackerX++;
                }
                map.tilePositionX = 0;
                tileTrackerX = 0;
                map.tilePositionY += map.defaultTileHeight;
                tileTrackerY++;
            }
            //----------------------------------------------------------End Map Configuration----------------------------------------
            playerOneCamera = new Camera(cmReference, gdmReference, sbReference, new Vector2(0, 0), gdmReference.GraphicsDevice.Viewport.TitleSafeArea.Width, gdmReference.GraphicsDevice.Viewport.TitleSafeArea.Height);
            //playerTwoCamera = new Camera(cm, gdm, sb, new Vector2(0, 0), graphicsDeviceManagerReference.GraphicsDevice.Viewport.TitleSafeArea.Width / 2, graphicsDeviceManagerReference.GraphicsDevice.Viewport.TitleSafeArea.Height);
            //playerTwoCamera.screenPosition = new Vector2(graphicsDeviceManagerReference.GraphicsDevice.Viewport.TitleSafeArea.Width / 2, 0);

            playerOne = pOne;
            playerOne.previousWorldPosition = new Vector2(0, 0);
            playerOne.currentWorldPosition = new Vector2(0, 0);

            //playerTwo = new playerOne(cm, sb, gdm);
            //playerTwo.currentWorldPosition = playerTwoCamera.worldPosition;

            //Configures the lists
            configureParallaxBackgrounds();
            configureWorldPlatforms();
            configureWorldSlopePlatforms();
            configureWorldGrindRails();
            configureWorldSprings();
            configureWorldTreasure();

            gravity = new Vector2(0, 9);

            scoreFont = cmReference.Load<SpriteFont>("Fonts/PlaceholderFont");
        }

        public override void updateScreen(GameTime gameTime)
        {
            //checks to move state
            checkToActivateEndOfLevel();

            ///--------------------------------------------------
            /// sets previous state to current, then grabs the now current state
            ///--------------------------------------------------
            previousKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();

            if (playerOne.controllerBeingUsed == 1)
            {
                playerOnePreviousGamePadState = GamePad.GetState(PlayerIndex.One);
                playerOneCurrentGamePadState = GamePad.GetState(PlayerIndex.One);
            }
            ///--------------------------------------------------
            ///update based of key press phase
            ///--------------------------------------------------
            //playerOneCamera.acceptKeyboardInput(previousKeyboardState, currentKeyboardState);
            //playerTwoCamera.acceptKeyboardInput(previousKeyboardState, currentKeyboardState);
            playerOne.checkForPlayerKeyboardInput(previousKeyboardState, currentKeyboardState);
            //playerTwo.checkForPlayerInput(previousKeyboardState, currentKeyboardState);

            ///---------------------------------------------------
            /// Update playerOne position phase
            /// - grab the calculatedPosition from the playerOne, this calculates the next position for the playerOne and returns the vector2
            /// - take the playerOneNewPosition that you got from calculateplayerOneNewPosition and put it into the collision checking of the objects you're concerned about (map bounding, camera bounding, collisions etc)
            /// - update the playerOne's currentWorldPosition to the playerOneNewPosition, 
            /// - call the playerOne's update command
            ///--------------------------------------------------
            Vector2 playerOneNewPosition = playerOne.calculateNewPosition(gravity);
            playerOne.previousWorldPosition = playerOne.currentWorldPosition;
            //Vector2 playerTwoNewPosition = playerTwo.calculateNewPosition(gravity);
            foreach (Platform tp in platforms)
            {
                if (playerOneCamera.getWorldBoundingBox.Intersects(tp.boundingRectangle))
                {
                    if (!playerOne.ladderActionStateActive)
                    {
                        playerOneNewPosition = tp.checkAndFixPlatformCollision(playerOneNewPosition, playerOne);
                    }
                }
                if (tp is DisappearingPlatform)
                {
                    if (tp.platformSize.X == 0)
                    {
                        platformsToRemove.Add(tp);
                    }
                }
                tp.Update(gameTime, gravity);
            }
            foreach (Platform tp in platformsToRemove)
            {
                platforms.Remove(tp);
            }
            platformsToRemove.Clear();

            //playerTwoNewPosition = platformOne.checkAndFixTiledPlatformCollision(playerTwoNewPosition, playerTwo);

            playerOneNewPosition = map.fixPlayerCollisionWithMapBoundings(playerOneNewPosition, playerOne);
            playerOneNewPosition = map.checkForAndFixCollidableTileCollisionWithPlayer(playerOneNewPosition, playerOne, playerOneCamera);

            //playerTwoNewPosition = map.fixPlayerCollisionWithMapBoundings(playerTwoNewPosition, playerTwo);
            //playerOneNewPosition = playerOneCamera.fixPlayerCollisionWithCameraBounds(playerOneNewPosition, playerOne);

            foreach (SlopedPlatform sp in slopedPlatforms)
            {
                playerOneNewPosition = sp.checkAndFixPlatformCollision(playerOneNewPosition, playerOne);
            }

            // Start of rail grinds collision checking
            Rectangle checkRect = new Rectangle((int)playerOneNewPosition.X, (int)playerOneNewPosition.Y, playerOne.width, playerOne.height);

            foreach (GrindRail gr in grindRails)
            {
                if (playerOneCamera.getWorldBoundingBox.Intersects(gr.platformSlope.getBoundingRectangle))
                {
                    playerOneNewPosition = gr.checkAndFixGrindRailCollision(playerOneNewPosition, playerOne);
                    //System.Diagnostics.Debug.WriteLine("Rail Start X: " + gr.platformSlope.getBoundingRectangle.X);
                    //System.Diagnostics.Debug.WriteLine("Rail Start Y: " + gr.platformSlope.getBoundingRectangle.Y);
                    //System.Diagnostics.Debug.WriteLine("Rail Slope: " + gr.platformSlope.getSlope());
                    //System.Diagnostics.Debug.WriteLine("Player Start X: " + playerOne.currentWorldPosition.X);
                    //System.Diagnostics.Debug.WriteLine("Player End X: " + (playerOne.currentWorldPosition.X+playerOne.width));
                    //System.Diagnostics.Debug.WriteLine("Player Start Y: " + playerOne.currentWorldPosition.Y);
                    //System.Diagnostics.Debug.WriteLine("Player End Y: " + (playerOne.currentWorldPosition.Y + playerOne.height));
                    gr.update(gameTime);
                }
            }
            if (playerOneNewPosition.X != checkRect.X ||
                playerOneNewPosition.Y != checkRect.Y)
            {
                // a collision occurred from at least one of the rails
            }
            else
            {
                //a collision for grinding did not occur, reset grind state
                playerOne.grindRailReference = null;
            }
            // End of rail grinds collision checking

            playerOne.currentWorldPosition = playerOneNewPosition;

            //playerTwo.currentWorldPosition = playerTwoNewPosition;
            //This is only used to contain the playerOne inside the drawing window, not needed if camera bounds and map bounding are handled
            //playerOne.checkAndFixWindowCollision();

            playerOne.Update(gameTime);
            //playerTwo.Update(gameTime);

            ///--------------------------------------------------
            /// Update world object positions
            /// This is the area where the world objects will perform their variations of updating, in some cirucmstances that may involve calculating a new position
            /// In other circumastances it may just involve updating
            ///--------------------------------------------------
            foreach (Spring sp in springs)
            {
                if (playerOneCamera.getWorldBoundingBox.Intersects(sp.getBoundingBox))
                {
                    sp.update(gameTime, playerOneCamera);
                    sp.checkToActivateSpring(playerOne);
                }
            }

            //------------ treasure checks --------------
            Treasure tempTreasure = null;
            foreach (Treasure treasure in worldTreasure)
            {
                if (playerOneCamera.getWorldBoundingBox.Intersects(treasure.getBoundingBox))
                {
                    treasure.update(gameTime, playerOneCamera);
                    tempTreasure = treasure.checkToActivate(playerOne);
                    if (tempTreasure != null)
                    {
                        worldTreasureToRemove.Add(tempTreasure);
                    }
                }
            }
            foreach (Treasure t in worldTreasureToRemove)
            {
                worldTreasure.Remove(t);
            }
            worldTreasureToRemove.Clear();

            ///--------------------------------------------------
            /// Update the camera's logic here last
            /// - updateWorldPositionBasedOnPlayer updates the camera so it orients around the playerOne passed in
            /// - fixCameraCollisionWithTileMapBounds makes it so that the camera will never draw over the boundaries of the tile map, if it exceeds the dimensions the map is adjusted accordingly
            ///--------------------------------------------------
            playerOneCamera.updateWorldPositionBasedOnPlayer(playerOne);
            playerOneCamera.fixCameraCollisionWithTileMapBounds(map);

            foreach (ParallaxBackground pBackground in parallaxBackgrounds)
            {
                pBackground.updateBasedOnCamera(playerOneCamera);
            }
            //playerTwoCamera.updateWorldPositionBasedOnPlayer(playerTwo);
            //playerTwoCamera.fixCameraCollisionWithTileMapBounds(map);

            ///--------------------------------------------------
            ///  Update the map accordingly
            ///--------------------------------------------------
            //System.Diagnostics.Debug.WriteLine(map.getWorldToScreenCoord(new Vector2(playerOne.currentWorldPosition.X + playerOne.width / 2, playerOne.boundingRectangle.Top), playerOneCamera));
            map.update(gameTime, playerOneCamera);
            //map.update(gameTime, playerTwoCamera);
        }

        /// The draw order is determined by layer depth 
        public override void draw(GameTime gameTime)
        {
            RasterizerState rState = new RasterizerState();
            rState.CullMode = CullMode.None;
            rState.ScissorTestEnable = true;

            //graphicsDeviceManagerReference.GraphicsDevice.Clear(screenColor);

            sbReference.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.LinearWrap, DepthStencilState.None, rState);
            gdmReference.GraphicsDevice.ScissorRectangle = playerOneCamera.getScreenBoundingBox;

            //playerOne
            map.draw(playerOneCamera);

            foreach (ParallaxBackground pBackground in parallaxBackgrounds)
            {
                pBackground.drawAltPosition(map.getWorldToScreenCoord(new Vector2(pBackground.worldPosition.X, pBackground.worldPosition.Y), playerOneCamera));
            }

            //Sloped Platforms
            foreach (SlopedPlatform sp in slopedPlatforms)
            {
                sp.drawAltPosition(map.getWorldToScreenCoord(new Vector2(sp.platformSlope.pt1.X, sp.platformSlope.pt1.Y), playerOneCamera), map.getWorldToScreenCoord(new Vector2(sp.platformSlope.pt2.X, sp.platformSlope.pt2.Y), playerOneCamera));
            }

            foreach (GrindRail gr in grindRails)
            {
                if (playerOneCamera.getWorldBoundingBox.Intersects(gr.platformSlope.getBoundingRectangle))
                {
                    gr.drawAltPosition(map.getWorldToScreenCoord(new Vector2(gr.platformSlope.pt1.X, gr.platformSlope.pt1.Y), playerOneCamera), map.getWorldToScreenCoord(new Vector2(gr.platformSlope.pt2.X, gr.platformSlope.pt2.Y), playerOneCamera));
                }
            }

            //Platforms
            foreach (Platform tp in platforms)
            {
                if (playerOneCamera.getWorldBoundingBox.Intersects(tp.boundingRectangle))
                {
                    tp.DrawAltPosition(map.getWorldToScreenCoord(tp.position, playerOneCamera));
                }

            }

            //Springs
            foreach (Spring sp in springs)
            {
                if (playerOneCamera.getWorldBoundingBox.Intersects(sp.getBoundingBox))
                {
                    sp.drawAltPosition(map.getWorldToScreenCoord(sp.worldPosition, playerOneCamera));
                }

            }

            //Treasure
            foreach (Treasure treasure in worldTreasure)
            {
                if (playerOneCamera.getWorldBoundingBox.Intersects(treasure.getBoundingBox))
                {
                    treasure.drawAltPosition(map.getWorldToScreenCoord(treasure.centerPosition, playerOneCamera));
                    //treasure.checkToActivateSpring(playerOne);
                }
            }
            drawTreasureInfo(new Vector2(0,0), playerOne);
            playerOne.DrawAltPosition(map.getWorldToScreenCoord(playerOne.currentWorldPosition, playerOneCamera));
            playerOne.drawBoundingBox(map.getWorldToScreenCoord(playerOne.currentWorldPosition, playerOneCamera));

            sbReference.End();


            //playerOne 2 draw section
            //sbReference.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearWrap, DepthStencilState.None, rState);
            //graphicsDeviceManagerReference.GraphicsDevice.ScissorRectangle = playerTwoCamera.getScreenBoundingBox; 
            //playerTwo
            //map.draw(playerTwoCamera);
            //playerTwo.DrawAltPosition(map.getWorldToScreenCoord(playerTwo.currentWorldPosition, playerTwoCamera));
            //platformOne.DrawAltPosition(map.getWorldToScreenCoord(platformOne.position, playerTwoCamera));
            //sbReference.End();
        }

        public void configureWorldPlatforms()
        {
            //EXAMPLE
            //platforms.Add(new ContainerPlatform(cmReference, sbReference, gdmReference, imageLibrary, new Vector2(15, 15), new Vector2(0, 0), true, false, true, true));
            // EXAMPLE
            //platforms.Add(new ResistantWaterPlatform(cmReference, sbReference, gdmReference, imageLibrary, new Vector2(20, 4), new Vector2(0, 1536), -5, true, true, true, true));
            // EXAMPLE
            //platforms.Add(new DisappearingPlatform(cmReference, sbReference, gdmReference, imageLibrary, new Vector2(50, 2), new Vector2(320, 1100), true, true, true, true));
            // EXAMPLE
            //platforms.Add(new Platform(cmReference, sbReference, gdmReference, imageLibrary, new Vector2(12, 4), new Vector2(640, 1408), false, false, true, false));
            // EXAMPLE
            //platforms.Add(new GrassPlatform(cmReference, sbReference, gdmReference, imageLibrary, new Vector2(4, 8), new Vector2(2272, 768), true, true, true, false));

            platforms.Add(new GrassPlatform(cmReference, sbReference, gdmReference, imageLibrary, new Vector2(map.tileMapWidth - 2, 4), new Vector2(0, map.getTileMapHeightInPixels - (4 * 16)), true, true, true, true, true, false));
            platforms.Add(new GoalPlatform(cmReference, sbReference, gdmReference, imageLibrary, new Vector2(2, 4), new Vector2((map.getTileMapWidthInPixels - (2 * 16)), map.getTileMapHeightInPixels - (4 * 16)), true, true, true, true, true, false));
        }

        public void configureWorldSlopePlatforms()
        {
            // EXAMPLE
            //slopedPlatforms.Add(new SlopedPlatform(cmReference, gdmReference, sbReference, imageLibrary, new Vector2(200, 1400), new Vector2(1200, 400), 0));
        }

        public void configureWorldGrindRails()
        {
            //slope 1
            //grindRails.Add(new GrindRail(cmReference, gdmReference, sbReference, imageLibrary, new Vector2(400, 0), new Vector2(800, 400), 6, .75f));
            //slope 2
            //grindRails.Add(new GrindRail(cmReference, gdmReference, sbReference, imageLibrary, new Vector2(0, 800), new Vector2(500, 400), 6, .75f));
            //slope3
            //grindRails.Add(new GrindRail(cmReference, gdmReference, sbReference, imageLibrary, new Vector2(800, 0), new Vector2(1000, 400), 6, .75f));
            //slope4
            //grindRails.Add(new GrindRail(cmReference, gdmReference, sbReference, imageLibrary, new Vector2(400, 800), new Vector2(500, 400), 6, .75f));
        }

        public void configureWorldSprings()
        {
            // EXAMPLE
            //springs.Add(new Spring(cmReference, gdmReference, sbReference, imageLibrary, new Vector2(2768, 1920 - 38 / 2), new Vector2(0, -50), 0f));
        }

        public void configureParallaxBackgrounds()
        {
            //EXAMPLE
            //parallaxBackgrounds.Add(new ParallaxBackground(cmReference, gdmReference, sbReference, imageLibrary.parallaxClouds, .15f, new Vector2(0, 0), new Vector2(gdmReference.GraphicsDevice.Viewport.TitleSafeArea.Width, 1500), true, false, true, false));
        }

        public void configureWorldTreasure()
        {
            /*-------------EXAMPLE-------------------
            int startX = 0;
            int xTracker = 0;
            int endX = 0;
            int startY = 0;
            int endY = 0;
            int xDistanceApart = 40;

            //line loop
            startX = 340;
            endX = 540;
            startY = 1500;
            while (startX < endX)
            {
                worldTreasure.Add(new RedJewel(cmReference, gdmReference, sbReference, imageLibrary, new Vector2(startX, startY), 0f));
                startX += xDistanceApart;
            }
             ----------------------------------------*/
            int startX = 0;
            int endX = 0;
            int startY = 0;
            int xDistanceApart = 40;

            //line loop
            startX = 50;
            endX = map.getTileMapWidthInPixels-50;
            startY = map.getTileMapHeightInPixels - 100;
            while (startX < endX)
            {
                worldTreasure.Add(new RedJewel(cmReference, gdmReference, sbReference, imageLibrary, new Vector2(startX, startY), 0f));
                startX += xDistanceApart;
            }
        }

        public void checkToActivateEndOfLevel() {
            if (playerOne.boundingRectangle.Right > map.getTileMapWidthInPixels - 32) {
                endOfLevelTrigger.isTriggerActivated = true;
            }
        }

        public void drawTreasureInfo(Vector2 position, Player player) {
            //red jewel player tracker
            imageLibrary.redJewelImage.DrawAltPosition(position+(new Vector2(5, 0)), false, 0f);
            sbReference.DrawString(scoreFont, "" + player.redJewelLevelTracker, position + new Vector2(30, 5), Color.White, 0f, Vector2.Zero, .25f, SpriteEffects.None, 0f);
            
            //green jewel player tracker
            imageLibrary.greenJewelImage.DrawAltPosition(position+(new Vector2(0,30)), false, 0f);
            sbReference.DrawString(scoreFont, "" + player.greenJewelLevelTracker, position + new Vector2(30, 30), Color.White, 0f, Vector2.Zero, .25f, SpriteEffects.None, 0f);
            
            //blue jewel player tracker
            imageLibrary.blueJewelImage.DrawAltPosition(position+(new Vector2(5,60)), false, 0f);
            sbReference.DrawString(scoreFont, "" + player.blueJewelLevelTracker, position + new Vector2(30, 60), Color.White, 0f, Vector2.Zero, .25f, SpriteEffects.None, 0f);
        }
    }
}
