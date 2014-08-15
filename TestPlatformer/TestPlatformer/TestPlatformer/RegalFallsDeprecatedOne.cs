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
    class RegalFallsDeprecatedOne : Screen
    {
        //camera for the playerOne
        Camera playerOneCamera;
        //Camera playerTwoCamera;

        //playerOne one
        Player playerOne;
        //playerOne two
        //playerOne playerTwo;

        List<ParallaxBackground> parallaxBackgrounds;

        List<Platform> platforms;
        List<Platform> platformsToRemove;

        List<SlopedPlatform> slopedPlatforms;

        List<Spring> springs;

        List<Treasure> worldTreasure;
        List<Treasure> worldTreasureToRemove;

        //Hurdles
        Hurdle hurdleOne;
        Hurdle hurdleTwo;

        //Rope Testing
        SwingRope testRope;

        Ladder testLadder;

        //keeps track of previous and current update screen presses
        KeyboardState previousKeyboardState;
        KeyboardState currentKeyboardState;

        GamePadState playerOnePreviousGamePadState;
        GamePadState playerOneCurrentGamePadState;

        //tracks gravity, want it to be an environment class eventually
        public Vector2 gravity;

        //The "world"/tilemap that the playerOne exists in. If it's not located around the playerOne it automatically draws to the playerOne's bounding, provided you call the method of course
        public TileMap map;
        public ImageLibrary imageLibrary;

        public RegalFallsDeprecatedOne(ContentManager cm, GraphicsDeviceManager gdm, SpriteBatch sb, ImageLibrary iLibrary, Player pOne) : base(cm, gdm, sb)
        {
            cmReference = cm;
            gdmReference = gdm;
            sbReference = sb;

            imageLibrary = iLibrary;

            parallaxBackgrounds = new List<ParallaxBackground>();

            platforms = new List<Platform>();
            platformsToRemove = new List<Platform>();

            slopedPlatforms = new List<SlopedPlatform>();

            springs = new List<Spring>();

            worldTreasure = new List<Treasure>();
            worldTreasureToRemove = new List<Treasure>();

            //System.Diagnostics.Debug.WriteLine(testPlatform.platformSlope.getSlope());

            //configures default map
            map = new TileMap(cm, gdm, sb, new Vector2(500, 400), new Vector2(16, 16));
            //----------------------------------------------------------Start Map Configuration----------------------------------------
            int tileTrackerX = 0;
            int tileTrackerY = 0;

            //iterates through row by column and initializes based on some arbitrary distinctions
            while (tileTrackerY < map.tileMapHeight)
            {
                while (tileTrackerX < map.tileMapWidth)
                {
                    /*
                    if (tileTrackerX > map.tileMapWidth / 2)
                    {
                        //bottom right
                        if (tileTrackerY > map.tileMapHeight / 2)
                        {
                            map.tiles[tileTrackerX, tileTrackerY] = new BackgroundTile(cm, gdm, sb, new Vector2(map.tilePositionX, map.tilePositionY), new Vector2(16, 16), new Vector2(1, 1), imageLibrary.basicGreenTile, false, false, false, false);
                        }
                        //bottom left
                        else
                        {
                            map.tiles[tileTrackerX, tileTrackerY] = new BackgroundTile(cm, gdm, sb, new Vector2(map.tilePositionX, map.tilePositionY), new Vector2(16, 16), new Vector2(1, 1), imageLibrary.basicOrangeTile, false, false, false, false);
                        }
                    }
                    //top
                    else
                    {
                        //top right
                        if (tileTrackerY > map.tileMapHeight / 2)
                        {
                            map.tiles[tileTrackerX, tileTrackerY] = new BackgroundTile(cm, gdm, sb, new Vector2(map.tilePositionX, map.tilePositionY), new Vector2(16, 16), new Vector2(1, 1), imageLibrary.basicYellowTile , false, false, false, false);
                        }
                        //top left
                        else
                        {
                            map.tiles[tileTrackerX, tileTrackerY] = new BackgroundTile(cm, gdm, sb, new Vector2(map.tilePositionX, map.tilePositionY), new Vector2(16, 16), new Vector2(1, 1), imageLibrary.basicRedTile, false, false, false, false);
                        }
                    }
                    */
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
            configureWorldSprings();
            configureWorldTreasure();

            hurdleOne = new Hurdle(cmReference, gdmReference, sbReference, (new Vector2(4288, 1280 - hurdleOne.height)));

            hurdleTwo = new Hurdle(cmReference, gdmReference, sbReference, (new Vector2(map.getTileMapWidthInPixels/2 - 400, map.getTileMapHeightInPixels - hurdleTwo.height)));

            testRope = new SwingRope(cmReference, gdmReference, sbReference, new Vector2(2304, 896), 200);

            testLadder = new Ladder(cmReference, gdmReference, sbReference, imageLibrary, new Vector2(2944 - playerOne.width, 1280), new Vector2(playerOne.width, 500));

            gravity = new Vector2(0, 9);
        }

        public override void updateScreen(GameTime gameTime)
        {
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
                if (tp is DisappearingPlatform) {
                    if (tp.platformSize.X == 0) {
                        platformsToRemove.Add(tp);
                    }
                }
                tp.Update(gameTime, gravity);
            }
            foreach (Platform tp in platformsToRemove) {
                platforms.Remove(tp);
            }
            platformsToRemove.Clear();

            //playerTwoNewPosition = platformOne.checkAndFixTiledPlatformCollision(playerTwoNewPosition, playerTwo);

            playerOneNewPosition = map.fixPlayerCollisionWithMapBoundings(playerOneNewPosition, playerOne);
            playerOneNewPosition = map.checkForAndFixCollidableTileCollisionWithPlayer(playerOneNewPosition, playerOne, playerOneCamera);

            //playerTwoNewPosition = map.fixPlayerCollisionWithMapBoundings(playerTwoNewPosition, playerTwo);
            //playerOneNewPosition = playerOneCamera.fixPlayerCollisionWithCameraBounds(playerOneNewPosition, playerOne);

            foreach (SlopedPlatform sp in slopedPlatforms) {
                playerOneNewPosition = sp.checkAndFixPlatformCollision(playerOneNewPosition, playerOne);
            }
            
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

            //Hurdle
            hurdleOne.update(gameTime, playerOneCamera);
            hurdleOne.checkForAndTriggerPlayerActionState(playerOne);

            hurdleTwo.update(gameTime, playerOneCamera);
            hurdleTwo.checkForAndTriggerPlayerActionState(playerOne);

            foreach (Spring sp in springs) {
                if (playerOneCamera.getWorldBoundingBox.Intersects(sp.getBoundingBox)) {
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
                    if (tempTreasure != null) {
                        worldTreasureToRemove.Add(tempTreasure);
                    }
                }
            }
            foreach(Treasure t in worldTreasureToRemove)
            {
                worldTreasure.Remove(t);
            }
            worldTreasureToRemove.Clear();

            testRope.acceptPlayerInput(playerOne);
            testRope.updatePlayerPositionWithGrappleCircle(playerOne);
            testRope.checkForAndTriggerPlayerActionState(playerOne);

            testLadder.update(gameTime, playerOneCamera);
            testLadder.checkToActivateActionState(playerOne);

            ///--------------------------------------------------
            /// Update the camera's logic here last
            /// - updateWorldPositionBasedOnPlayer updates the camera so it orients around the playerOne passed in
            /// - fixCameraCollisionWithTileMapBounds makes it so that the camera will never draw over the boundaries of the tile map, if it exceeds the dimensions the map is adjusted accordingly
            ///--------------------------------------------------
            playerOneCamera.updateWorldPositionBasedOnPlayer(playerOne);
            playerOneCamera.fixCameraCollisionWithTileMapBounds(map);

            foreach (ParallaxBackground pBackground in parallaxBackgrounds) {
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

            playerOne.DrawAltPosition(map.getWorldToScreenCoord(playerOne.currentWorldPosition, playerOneCamera));
            playerOne.drawBoundingBox(map.getWorldToScreenCoord(playerOne.currentWorldPosition, playerOneCamera));

            //Platforms
            foreach (Platform tp in platforms)
            {
                if (playerOneCamera.getWorldBoundingBox.Intersects(tp.boundingRectangle)) {
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

            hurdleOne.drawAltPosition(map.getWorldToScreenCoord(hurdleOne.worldPosition, playerOneCamera));
            hurdleTwo.drawAltPosition(map.getWorldToScreenCoord(hurdleTwo.worldPosition, playerOneCamera));

            testRope.drawAltPosition(map.getWorldToScreenCoord(testRope.ropePosition, playerOneCamera), map.getWorldToScreenCoord(testRope.grappleCircle.center, playerOneCamera));

            testLadder.drawAltPosition(map.getWorldToScreenCoord(testLadder.worldPosition, playerOneCamera));

            foreach (SlopedPlatform sp in slopedPlatforms)
            {
                sp.drawAltPosition(map.getWorldToScreenCoord(new Vector2(sp.platformSlope.pt1.X, sp.platformSlope.pt1.Y), playerOneCamera), map.getWorldToScreenCoord(new Vector2(sp.platformSlope.pt2.X, sp.platformSlope.pt2.Y), playerOneCamera));;
            }
            
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

        public void configureWorldPlatforms() {
            /*
            tempPlatform = new BetaPlatform(cmReference, sbReference, gdmReference, new Vector2(10,4), new Vector2(0, 1536), true, true, true, true);
            tempPlatform.configureImages(imageLibrary.topLeftGrassPlatform, imageLibrary.topGrassPlatform, imageLibrary.topRightGrassPlatform, imageLibrary.leftGrassPlatform, imageLibrary.centerGrassPlatform, imageLibrary.rightGrassPlatform, imageLibrary.bottomLeftGrassPlatform, imageLibrary.bottomGrassPlatform, imageLibrary.bottomRightGrassPlatform);
            platforms.Add(tempPlatform);

            tempPlatform = new BetaPlatform(cmReference, sbReference, gdmReference, new Vector2((map.getTileMapWidthInPixels / 16) - 10, 4), new Vector2(0, 1920), true, true, true, true);
            tempPlatform.configureImages(imageLibrary.topLeftGrassPlatform, imageLibrary.topGrassPlatform, imageLibrary.topRightGrassPlatform, imageLibrary.leftGrassPlatform, imageLibrary.centerGrassPlatform, imageLibrary.rightGrassPlatform, imageLibrary.bottomLeftGrassPlatform, imageLibrary.bottomGrassPlatform, imageLibrary.bottomRightGrassPlatform);
            platforms.Add(tempPlatform);
            */

            //This is the place holder for the Regal Falls Bar
            platforms.Add(new ContainerPlatform(cmReference, sbReference, gdmReference, imageLibrary, new Vector2(15, 15), new Vector2(0, 0), true, true, true, false, true, true));

            platforms.Add(new ResistantWaterPlatform(cmReference, sbReference, gdmReference, imageLibrary, new Vector2(20, 4), new Vector2(0, 1536), -5, true, true, true, true, true, true));

            //GrassPlatform that you land on in the beginning
            platforms.Add(new GrassPlatform(cmReference, sbReference, gdmReference, imageLibrary, new Vector2(12, 4), new Vector2(320, 1536), true, true, true, true, true, true));
            
            //test disappearing platform
            platforms.Add(new DisappearingPlatform(cmReference, sbReference, gdmReference, imageLibrary, new Vector2(50, 2), new Vector2(320, 1100), true, true, true, true, true, true));

            //corresponding GrassPlatform obstacles the large obstacle
            platforms.Add(new Platform(cmReference, sbReference, gdmReference, imageLibrary, new Vector2(12, 4), new Vector2(640, 1408), true, true, false, false, true, false));
            platforms.Add(new Platform(cmReference, sbReference, gdmReference, imageLibrary, new Vector2(20, 4), new Vector2(640, 1728), true, true, false, false, true, false));
            platforms.Add(new Platform(cmReference, sbReference, gdmReference, imageLibrary, new Vector2(52, 4), new Vector2(1088, 1280), true, true, true, true, true, true));
            platforms.Add(new Platform(cmReference, sbReference, gdmReference, imageLibrary, new Vector2(4, 19), new Vector2(1664, 1280), true, true, true, true, false, true));
            platforms.Add(new Platform(cmReference, sbReference, gdmReference, imageLibrary, new Vector2(4, 8), new Vector2(1664, 1792), true, true, true, true, true, false));

            //swing rope GrassPlatform
            platforms.Add(new GrassPlatform(cmReference, sbReference, gdmReference, imageLibrary, new Vector2(4, 8), new Vector2(2272, 768), true, true, true, true, true, false));

            //right platform of swing rope
            platforms.Add(new GrassPlatform(cmReference, sbReference, gdmReference, imageLibrary, new Vector2(44, 4), new Vector2(2944, 1280), true, true, false, false, true, false));

            //platform next to the spring
            platforms.Add(new GrassPlatform(cmReference, sbReference, gdmReference, imageLibrary, new Vector2(4, 12), new Vector2(2800, 1728), true, true, true, true, true, false));

            //platform 10
            platforms.Add(new GrassPlatform(cmReference, sbReference, gdmReference, imageLibrary, new Vector2(40, 4), new Vector2(3072, 1664), true, true, false, false, true, false));

            //platform 11
            platforms.Add(new GrassPlatform(cmReference, sbReference, gdmReference, imageLibrary, new Vector2(32, 4), new Vector2(3840, 1280), true, true, false, false, true, false));

            //platform 12
            platforms.Add(new GrassPlatform(cmReference, sbReference, gdmReference, imageLibrary, new Vector2(32, 4), new Vector2(4608, 1280), true, true, false, false, true, false));

            //platform 13
            platforms.Add(new GrassPlatform(cmReference, sbReference, gdmReference, imageLibrary, new Vector2(4, 12), new Vector2(4352, 1728), true, true, true, true, true, false));
            //platform 14
            platforms.Add(new GrassPlatform(cmReference, sbReference, gdmReference, imageLibrary, new Vector2(4, 12), new Vector2(4736, 1728), true, true, true, true, true, false));
            //platform 15
            platforms.Add(new GrassPlatform(cmReference, sbReference, gdmReference, imageLibrary, new Vector2(4, 12), new Vector2(5184, 1728), true, true, true, true, true, false));

            //top level GrassPlatform holder
            platforms.Add(new GrassPlatform(cmReference, sbReference, gdmReference, imageLibrary, new Vector2((map.getTileMapWidthInPixels / 16) - 10, 4), new Vector2(0, 1920), true, true, true, true, true, true));

            //placeholder for the bottom of the jungle
            platforms.Add(new GrassPlatform(cmReference, sbReference, gdmReference, imageLibrary, new Vector2(map.getTileMapWidthInPixels / 16, 4), new Vector2(0, map.getTileMapHeightInPixels - 64), true, true, true, true, true, true));
        }

        public void configureWorldSlopePlatforms() {
            slopedPlatforms.Add(new SlopedPlatform(cmReference, gdmReference, sbReference, imageLibrary, new Vector2(200, 1400), new Vector2(1200, 400), 0, .8f, true, true, true, true));
        }

        public void configureWorldSprings() {
            springs.Add(new Spring(cmReference, gdmReference, sbReference, imageLibrary, new Vector2(2768, 1920 - 38 / 2), new Vector2(0,-50), 0f));
            springs.Add(new Spring(cmReference, gdmReference, sbReference, imageLibrary, new Vector2(3648, 1664 - 38 / 2), new Vector2(0,-50), 0f));
            springs.Add(new Spring(cmReference, gdmReference, sbReference, imageLibrary, new Vector2(3666, 1344 - 38 / 2), new Vector2(40,0), 90f));
        }
        
        public void configureParallaxBackgrounds() {
            //clouds
            parallaxBackgrounds.Add(new ParallaxBackground(cmReference, gdmReference, sbReference, imageLibrary.parallaxClouds, 4, new Vector2(0, 0), new Vector2(gdmReference.GraphicsDevice.Viewport.TitleSafeArea.Width, 1500), true, false, .9f));
            //treetops
            parallaxBackgrounds.Add(new ParallaxBackground(cmReference, gdmReference, sbReference, imageLibrary.treeTopTest, 4, new Vector2(0, 1500), new Vector2(gdmReference.GraphicsDevice.Viewport.TitleSafeArea.Width, imageLibrary.treeTopTest.Height), true, false, .9f));
            //tree trunk light gradient
            parallaxBackgrounds.Add(new ParallaxBackground(cmReference, gdmReference, sbReference, imageLibrary.testParallax, 4, new Vector2(0, 1500 + imageLibrary.treeTopTest.Height), new Vector2(gdmReference.GraphicsDevice.Viewport.TitleSafeArea.Width, (map.getTileMapHeightInPixels - (1500 + imageLibrary.treeTopTest.Height + 80))), true, false, .9f));
            //tree trunk dark gradient
            parallaxBackgrounds.Add(new ParallaxBackground(cmReference, gdmReference, sbReference, imageLibrary.testParallaxBottom, 4, new Vector2(0, (map.getTileMapHeightInPixels - 80)), new Vector2(gdmReference.GraphicsDevice.Viewport.TitleSafeArea.Width, 16), true, false, .9f));
        }

        public void configureWorldTreasure() {
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
            while (startX < endX) {
                worldTreasure.Add(new RedJewel(cmReference, gdmReference, sbReference, imageLibrary, new Vector2(startX, startY), 0f));
                startX += xDistanceApart;
            }

            //line loop
            startX = 690;
            endX = 930;
            startY = 1380;
            while (startX < endX)
            {
                worldTreasure.Add(new GreenJewel(cmReference, gdmReference, sbReference, imageLibrary, new Vector2(startX, startY), 0f));
                startX += xDistanceApart;
            }

            //line loop
            startX = 700;
            endX = 940;
            startY = 1690;
            while (startX < endX)
            {
                worldTreasure.Add(new BlueJewel(cmReference, gdmReference, sbReference, imageLibrary, new Vector2(startX, startY), 0f));
                startX += xDistanceApart;
            }
            platforms.Add(new GrassPlatform(cmReference, sbReference, gdmReference, imageLibrary, new Vector2(52, 4), new Vector2(1088, 1280), true, true, true, true, true, true));

            //pyramid loop
            startX = 1215;
            xTracker = startX;
            endX = 1800;
            startY = 1280-160;
            endY = 1280 - 40;

            while (startY < endY)
            {
                while (xTracker < endX)
                {
                    worldTreasure.Add(new GoldCoin(cmReference, gdmReference, sbReference, imageLibrary, new Vector2(xTracker, startY), 0f));
                    xTracker += xDistanceApart;
                }
                startY += 40;

                startX -= 40;
                xTracker = startX;
                endX += 40;
            }
        }
    }
}
