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
        List<Ladder> ladders;
        List<Hurdle> hurdles;
        List<BearTrap> bearTraps;

        List<Treasure> worldTreasure;
        List<Treasure> worldTreasureToRemove;

        public EventTrigger endOfLevelTrigger;

        //keeps track of previous and current update screen presses
        KeyboardState previousKeyboardState;
        KeyboardState currentKeyboardState;

        GamePadState playerPreviousGamePadState;
        GamePadState playerCurrentGamePadState;

        //tracks gravity, want it to be an environment class eventually
        public Vector2 gravity;

        //The "world"/tilemap that the playerOne exists in. If it's not located around the playerOne it automatically draws to the playerOne's bounding, provided you call the method of course
        public LevelMap map;
        public ImageLibrary imageLibrary;

        public SpriteFont scoreFont;

        public int deathWatchTimer;
        public int deathWatchTimerTimerLimit;

        public RegalFalls(ContentManager cm, GraphicsDeviceManager gdm, SpriteBatch sb, ImageLibrary iLibrary, Player pOne)
            : base(cm, gdm, sb)
        {
            nextLevel = NextLevelPointer.WorldOneLevelTwo;

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
            ladders = new List<Ladder>();
            hurdles = new List<Hurdle>();
            bearTraps = new List<BearTrap>();

            worldTreasure = new List<Treasure>();
            worldTreasureToRemove = new List<Treasure>();

            endOfLevelTrigger = new EventTrigger();

            //System.Diagnostics.Debug.WriteLine(testPlatform.platformSlope.getSlope());

            //configures default map
            map = new LevelMap(cm, gdm, sb, new Vector2(344, 264), new Vector2(16, 16));
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
            configureWorldLadders();
            configureWorldHurdles();
            configureWorldBearTraps();
            configureWorldTreasure();

            gravity = new Vector2(0, 9);

            scoreFont = cmReference.Load<SpriteFont>("Fonts/PlaceholderFont");

            deathWatchTimer = 0;
            deathWatchTimerTimerLimit = 60;
        }

        public override void updateScreen(GameTime gameTime)
        {
            //checks to move state
            checkToActivateEndOfLevel(playerOne);

            ///--------------------------------------------------
            /// sets previous state to current, then grabs the now current state
            ///--------------------------------------------------
            previousKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();

            if (playerOne.controllerBeingUsed == 1)
            {
                playerPreviousGamePadState = GamePad.GetState(PlayerIndex.One);
                playerCurrentGamePadState = GamePad.GetState(PlayerIndex.One);
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
            /// - take the playerNewPosition that you got from calculateplayerNewPosition and put it into the collision checking of the objects you're concerned about (map bounding, camera bounding, collisions etc)
            /// - update the playerOne's currentWorldPosition to the playerNewPosition, 
            /// - call the playerOne's update command
            ///--------------------------------------------------
            Vector2 playerNewPosition = playerOne.calculateNewPosition(gravity);
            playerOne.previousWorldPosition = playerOne.currentWorldPosition;
            //Vector2 playerTwoNewPosition = playerTwo.calculateNewPosition(gravity);
            foreach (Platform tp in platforms)
            {
                if (playerOneCamera.getWorldBoundingBox.Intersects(tp.boundingRectangle))
                {
                    if (!playerOne.ladderActionStateActive)
                    {
                        playerNewPosition = tp.checkAndFixPlatformCollision(playerNewPosition, playerOne);
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

            playerNewPosition = map.fixPlayerCollisionWithMapBoundings(playerNewPosition, playerOne);
            playerNewPosition = map.checkForAndFixCollidableTileCollisionWithPlayer(playerNewPosition, playerOne, playerOneCamera);

            //playerTwoNewPosition = map.fixPlayerCollisionWithMapBoundings(playerTwoNewPosition, playerTwo);
            //playerNewPosition = playerOneCamera.fixPlayerCollisionWithCameraBounds(playerNewPosition, playerOne);

            foreach (SlopedPlatform sp in slopedPlatforms)
            {
                playerNewPosition = sp.checkAndFixPlatformCollision(playerNewPosition, playerOne);
            }

            // Start of rail grinds collision checking
            Rectangle checkRect = new Rectangle((int)playerNewPosition.X, (int)playerNewPosition.Y, playerOne.width, playerOne.height);

            foreach (GrindRail gr in grindRails)
            {
                if (playerOneCamera.getWorldBoundingBox.Intersects(gr.platformSlope.getBoundingRectangle))
                {
                    playerNewPosition = gr.checkAndFixGrindRailCollision(playerNewPosition, playerOne);
                    //System.Diagnostics.Debug.WriteLine("Rail Start X: " + gr.platformSlope.getBoundingRectangle.X);
                    //System.Diagnostics.Debug.WriteLine("Rail Start Y: " + gr.platformSlope.getBoundingRectangle.Y);
                    //System.Diagnostics.Debug.WriteLine("Rail Slope: " + gr.platformSlope.getSlope());
                    //System.Diagnostics.Debug.WriteLine("playerOne Start X: " + playerOne.currentWorldPosition.X);
                    //System.Diagnostics.Debug.WriteLine("playerOne End X: " + (playerOne.currentWorldPosition.X+playerOne.width));
                    //System.Diagnostics.Debug.WriteLine("playerOne Start Y: " + playerOne.currentWorldPosition.Y);
                    //System.Diagnostics.Debug.WriteLine("playerOne End Y: " + (playerOne.currentWorldPosition.Y + playerOne.height));
                    gr.update(gameTime);
                }
            }
            if (playerNewPosition.X != checkRect.X ||
                playerNewPosition.Y != checkRect.Y)
            {
                // a collision occurred from at least one of the rails
            }
            else
            {
                //a collision for grinding did not occur, reset grind state
                playerOne.grindRailReference = null;
            }
            // End of rail grinds collision checking

            playerOne.currentWorldPosition = playerNewPosition;

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
            
            //LADDERS
            foreach (Ladder ld in ladders) {
                ld.update(gameTime, playerOneCamera);
                ld.checkToActivateActionState(playerOne);
            }
            
            //HURDLES
            foreach (Hurdle hl in hurdles)
            {
                if (playerOneCamera.getWorldBoundingBox.Intersects(hl.getBoundingBox))
                {
                    hl.update(gameTime, playerOneCamera);
                    hl.checkForAndTriggerPlayerActionState(playerOne);
                }
            }

            //BEAR TRAPS
            foreach (BearTrap tr in bearTraps)
            {
                if (playerOneCamera.getWorldBoundingBox.Intersects(tr.getBoundingBox))
                {
                    tr.update(gameTime);
                    tr.checkToActivate(playerOne);
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

            //if the player was killed
            if (playerOne.wasKilled) {

                //this is where the animation would occur, after the animation and the timer for it finishes the position gets set and the player has their killed status set to false
                if (deathWatchTimer < deathWatchTimerTimerLimit)
                {
                    if (deathWatchTimer == 0)
                    {
                        //handles the players death animation
                        playerOne.playerImageSheet.setFrameConfiguration(75, 74, 77);
                        playerOne.playerImageSheet.isAnimating = false;
                        playerOne.playerImageSheet.animateOnce = true;
                        //System.Diagnostics.Debug.WriteLine("Killed");
                    }
                    //do nothing but increment
                    deathWatchTimer++;
                }
                else {
                    //handles the players position reset to the death checkpoint
                    playerOne.currentWorldPosition = new Vector2(0, 640- playerOne.height);
                    //resets the camera position
                    playerOneCamera.currentWorldPosition = new Vector2(0, 0);
                    //resets the player's state
                    playerOne.playerImageSheet.setFrameConfiguration(0, 0, 4);
                    playerOne.facingLeft = false;
                    playerOne.facingRight = true;
                    playerOne.movingLeft = false;
                    playerOne.movingRight = false;
                    playerOne.wasKilled = false;
                    playerOne.isJumping = false;
                    playerOne.isDashing = false;
                    playerOne.exhaustedDash = false;
                    playerOne.isFalling = false;
                    playerOne.isFallingFromGravity = false;
                    playerOne.playerImageSheet.isAnimating = true;
                    playerOne.playerImageSheet.animateOnce = false;
                    deathWatchTimer = 0;
                }
                
            }
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
                sp.drawAltPosition(map.getWorldToScreenCoord(new Vector2(sp.platformSlope.pt1.X, sp.platformSlope.pt1.Y), playerOneCamera), map.getWorldToScreenCoord(new Vector2(sp.platformSlope.pt2.X, sp.platformSlope.pt2.Y), playerOneCamera));;
            }

            foreach (GrindRail gr in grindRails)
            {
                if (playerOneCamera.getWorldBoundingBox.Intersects(gr.platformSlope.getBoundingRectangle))
                {
                    gr.drawAltPosition(map.getWorldToScreenCoord(new Vector2(gr.platformSlope.pt1.X, gr.platformSlope.pt1.Y), playerOneCamera), map.getWorldToScreenCoord(new Vector2(gr.platformSlope.pt2.X, gr.platformSlope.pt2.Y), playerOneCamera)); ;
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

            //Ladders
            foreach (Ladder ld in ladders)
            {
                ld.drawAltPosition(map.getWorldToScreenCoord(ld.worldPosition, playerOneCamera));;
            }

            //Hurdles
            foreach (Hurdle hl in hurdles)
            {
                if (playerOneCamera.getWorldBoundingBox.Intersects(hl.getBoundingBox))
                {
                    hl.drawAltPosition(map.getWorldToScreenCoord(hl.worldPosition, playerOneCamera));
                }
            }

            //Bear Traps
            foreach (BearTrap tr in bearTraps)
            {
                if (playerOneCamera.getWorldBoundingBox.Intersects(tr.getBoundingBox))
                {
                    tr.drawAltPosition(map.getWorldToScreenCoord(tr.worldPosition, playerOneCamera));
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
            drawTreasureInfo(new Vector2(0, 0), playerOne);

            playerOne.DrawAltPosition(map.getWorldToScreenCoord(playerOne.currentWorldPosition, playerOneCamera));
            //playerOne.drawBoundingBox(map.getWorldToScreenCoord(playerOne.currentWorldPosition, playerOneCamera));
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
            //platforms.Add(new ResistantWaterPlatform(cmReference, sbReference, gdmReference, imageLibrary, new Vector2(20, 4), new Vector2(0, 1536), -5, true, true, true, true));
            //platforms.Add(new DisappearingPlatform(cmReference, sbReference, gdmReference, imageLibrary, new Vector2(50, 2), new Vector2(320, 1100), true, true, true, true));
            //platforms.Add(new GrassPlatform(cmReference, sbReference, gdmReference, imageLibrary, new Vector2(328, 8), new Vector2(0, 640), true, true, true, true));
            //platforms.Add(new GoalPlatform(cmReference, sbReference, gdmReference, imageLibrary, new Vector2(20, 8), new Vector2(5248, 2176), true, true, true, true));
            //platforms.Add(new SpikedPlatform(cmReference, sbReference, gdmReference, imageLibrary, new Vector2(16, 4), new Vector2(1216, 2112), false, false, true, false, false, false, true, false));
            //platforms.Add(new MovingPlatform(cmReference, sbReference, gdmReference, imageLibrary, new Vector2(4, 4), new Vector2(0, 400), new Vector2(400, 400), new Vector2(3, 3), true, true, true, true));
            
            
        }

        public void configureWorldSlopePlatforms()
        {   
            //slope 1
            //slopedPlatforms.Add(new SlopedPlatform(cmReference, gdmReference, sbReference, imageLibrary, new Vector2(320, 640), new Vector2(1536, 384), 0, .8f, false, false));
        }

        public void configureWorldGrindRails()
        {
            //grindRails.Add(new GrindRail(cmReference, gdmReference, sbReference, imageLibrary, new Vector2(2944, 1232), new Vector2(4288, 1104), 0, .75f));
        }

        public void configureWorldSprings()
        {
            // EXAMPLE
            //springs.Add(new Spring(cmReference, gdmReference, sbReference, imageLibrary, new Vector2(2768, 1920 - 38 / 2), new Vector2(0, -50), 0f));
        }

        public void configureWorldLadders() {
            //ladders.Add(new Ladder(cmReference, gdmReference, sbReference, imageLibrary, new Vector2(256, 1400), new Vector2(64, 776)));
        }

        public void configureWorldHurdles() {
            //hurdles.Add(new Hurdle(cmReference, gdmReference, sbReference, new Vector2(2304, 384-61)));
        }

        public void configureWorldBearTraps() {
            //bearTraps.Add(new BearTrap(cmReference, gdmReference, sbReference, new Vector2(1536, 364)));
        }

        public void configureParallaxBackgrounds()
        {
            //EXAMPLE
            //parallaxBackgrounds.Add(new ParallaxBackground(cmReference, gdmReference, sbReference, imageLibrary.parallaxClouds, .15f, new Vector2(0, 0), new Vector2(gdmReference.GraphicsDevice.Viewport.TitleSafeArea.Width, 1500), true, false, true, false));
        }

        public void configureWorldTreasure()
        {
            /*
            addTreasureBlockSlope(320, 1500, 580, 340, 30, .21f, 1);

            addTreasureBlock(1520, 2304, 334, 334, 30, 30, 1);
            addAlternatingTreasureBlock(1520, 2304, 300, 300, 30, 30, 2);
            addTreasureBlock(1520, 2304, 266, 266, 30, 30, 1);

            worldTreasure.Add(new GreenJewel(cmReference, gdmReference, sbReference, imageLibrary, new Vector2(2496, 512), 0f));
            worldTreasure.Add(new GreenJewel(cmReference, gdmReference, sbReference, imageLibrary, new Vector2(2560, 448), 0f));
            worldTreasure.Add(new GreenJewel(cmReference, gdmReference, sbReference, imageLibrary, new Vector2(2624, 384), 0f));

            addTreasureBlock(2368, 2688, 256, 256, 30, 30, 3);
            addTreasureBlock(2368, 2688, 220, 220, 30, 30, 3);
            addTreasureBlock(2720, 3328, 320, 320, 30, 30, 2);

            addTreasureBlock(3456, 3520, 384, 384, 30, 30, 2);
            addAlternatingPairTreasureBlock(3648, 3968, 270, 310, 40, 30, 1, 2);

            addTreasureBlockSlope(4030, 5248, 400, 600, 30, .15f, 1);

            worldTreasure.Add(new GreenJewel(cmReference, gdmReference, sbReference, imageLibrary, new Vector2(3776, 512), 0f));
            worldTreasure.Add(new GreenJewel(cmReference, gdmReference, sbReference, imageLibrary, new Vector2(3840, 448), 0f));
            worldTreasure.Add(new GreenJewel(cmReference, gdmReference, sbReference, imageLibrary, new Vector2(3904, 384), 0f));

            addAlternatingTreasureBlock(5312, 5420, 640, 1080, 30, 30, 3);

            addTreasureBlockSlope(5088, 5384, 1340, 1080, 30, .63f, 2);
            
            addAlternatingPairTreasureBlock(4672, 5024, 1270, 1376, 40, 30, 2, 3);
             */
        }


        public void checkToActivateEndOfLevel(Player player)
        {

            Rectangle endOfLevel = new Rectangle(5248, 2076, 320, 100);
            if (endOfLevel.Contains(player.boundingRectangle) && !player.isJumping)
            {
                //player.wasKilled = true;
                endOfLevelTrigger.isTriggerActivated = true;
            }
        }

        public void drawTreasureInfo(Vector2 position, Player playerOne)
        {
            //red jewel playerOne tracker
            imageLibrary.redJewelImage.DrawAltPosition(position + (new Vector2(5, 0)), false, 0f);
            sbReference.DrawString(scoreFont, "" + playerOne.redJewelLevelTracker, position + new Vector2(30, 5), Color.Black, 0f, Vector2.Zero, .25f, SpriteEffects.None, 0f);

            //green jewel playerOne tracker
            imageLibrary.greenJewelImage.DrawAltPosition(position + (new Vector2(0, 30)), false, 0f);
            sbReference.DrawString(scoreFont, "" + playerOne.greenJewelLevelTracker, position + new Vector2(30, 30), Color.Black, 0f, Vector2.Zero, .25f, SpriteEffects.None, 0f);

            //blue jewel playerOne tracker
            imageLibrary.blueJewelImage.DrawAltPosition(position + (new Vector2(5, 60)), false, 0f);
            sbReference.DrawString(scoreFont, "" + playerOne.blueJewelLevelTracker, position + new Vector2(30, 60), Color.Black, 0f, Vector2.Zero, .25f, SpriteEffects.None, 0f);
        }

        public void addTreasureBlockSlope(float startX ,float endX, float startY, float endY, float xDistanceApart, float slope, int jewelType) {
            if(startY >= endY){
                while (startY > endY)
                {
                    if (jewelType == 1)
                    {
                        worldTreasure.Add(new RedJewel(cmReference, gdmReference, sbReference, imageLibrary, new Vector2(startX, startY), 0f));
                    }
                    else if (jewelType == 2)
                    {
                        worldTreasure.Add(new GreenJewel(cmReference, gdmReference, sbReference, imageLibrary, new Vector2(startX, startY), 0f));
                    }
                    else if (jewelType == 3)
                    {
                        worldTreasure.Add(new BlueJewel(cmReference, gdmReference, sbReference, imageLibrary, new Vector2(startX, startY), 0f));
                    }
                    startX += xDistanceApart;
                    startY -= xDistanceApart*slope;
                }
            }
            else {
                while (startY < endY)
                {
                    if (jewelType == 1)
                    {
                        worldTreasure.Add(new RedJewel(cmReference, gdmReference, sbReference, imageLibrary, new Vector2(startX, startY), 0f));
                    }
                    else if (jewelType == 2)
                    {
                        worldTreasure.Add(new GreenJewel(cmReference, gdmReference, sbReference, imageLibrary, new Vector2(startX, startY), 0f));
                    }
                    else if (jewelType == 3)
                    {
                        worldTreasure.Add(new BlueJewel(cmReference, gdmReference, sbReference, imageLibrary, new Vector2(startX, startY), 0f));
                    }
                    startX += xDistanceApart;
                    startY += xDistanceApart*slope;
                }
            }
        }

        public void addTreasureBlock(float startX ,float endX, float startY, float endY, float xDistanceApart, float yDistanceApart, int jewelType) {
            //1 = red, 2 = green, 3 = blue
            float tempStartX = startX;
            while (startY <= endY)
            {
                startX = tempStartX;
                while (startX < endX)
                {
                    if (jewelType == 1)
                    {
                        worldTreasure.Add(new RedJewel(cmReference, gdmReference, sbReference, imageLibrary, new Vector2(startX, startY), 0f));
                    }
                    else if (jewelType == 2)
                    {
                        worldTreasure.Add(new GreenJewel(cmReference, gdmReference, sbReference, imageLibrary, new Vector2(startX, startY), 0f));
                    }
                    else if (jewelType == 3)
                    {
                        worldTreasure.Add(new BlueJewel(cmReference, gdmReference, sbReference, imageLibrary, new Vector2(startX, startY), 0f));
                    }
                    startX += xDistanceApart;
                }
                startY += yDistanceApart;
            }
        }         
        public void addAlternatingTreasureBlock(float startX ,float endX, float startY, float endY, float xDistanceApart, float yDistanceApart, int startJewelType) {
            float tempStartX = startX;
            while (startY <= endY)
            {
                startX = tempStartX;
                while (startX < endX)
                {
                    if (startJewelType == 1)
                    {
                        worldTreasure.Add(new RedJewel(cmReference, gdmReference, sbReference, imageLibrary, new Vector2(startX, startY), 0f));
                    }
                    else if (startJewelType == 2)
                    {
                        worldTreasure.Add(new GreenJewel(cmReference, gdmReference, sbReference, imageLibrary, new Vector2(startX, startY), 0f));
                    }
                    else if (startJewelType == 3)
                    {
                        worldTreasure.Add(new BlueJewel(cmReference, gdmReference, sbReference, imageLibrary, new Vector2(startX, startY), 0f));
                    }
                    startJewelType++;
                    if (startJewelType > 3)
                    {
                        startJewelType = 1;
                    }
                    startX += xDistanceApart;
                }
                startY += yDistanceApart;
            }
        }
        public void addAlternatingPairTreasureBlock(float startX, float endX, float startY, float endY, float xDistanceApart, float yDistanceApart, int firstJewelType, int secondJewelType)
        {
            int jewelTracker = firstJewelType;
            int rowTracker = 0;

            float tempStartX = startX;

            while (startY <= endY)
            {
                if (rowTracker%2 == 0)
                {
                    jewelTracker = firstJewelType;    
                }
                else
                {
                    jewelTracker = secondJewelType;
                }
                while (startX < endX)
                {
                    if (jewelTracker == 1)
                    {
                        worldTreasure.Add(new RedJewel(cmReference, gdmReference, sbReference, imageLibrary, new Vector2(startX, startY), 0f));
                    }
                    else if (jewelTracker == 2)
                    {
                        worldTreasure.Add(new GreenJewel(cmReference, gdmReference, sbReference, imageLibrary, new Vector2(startX, startY), 0f));
                    }
                    else if (jewelTracker == 3)
                    {
                        worldTreasure.Add(new BlueJewel(cmReference, gdmReference, sbReference, imageLibrary, new Vector2(startX, startY), 0f));
                    }

                    startX += xDistanceApart;
                    if (jewelTracker == firstJewelType) {
                        jewelTracker = secondJewelType;
                    }
                    else if (jewelTracker == secondJewelType)
                    {
                        jewelTracker = firstJewelType;
                    }
                }
                rowTracker++;
                startY += yDistanceApart;
                startX = tempStartX;
            }
        }
    }
}
