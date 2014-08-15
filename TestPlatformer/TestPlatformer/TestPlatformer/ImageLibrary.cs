using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TestPlatformer
{
    class ImageLibrary
    {

        //-------------- Character Select Screen Images -----------------
        public Texture2D characterSelector;
        //-------------------------------------------

        //--------------Basic Tiles-----------------
        public AnimatedImage basicTileSet;

        public AnimatedImage basicRedTile;
        public AnimatedImage basicOrangeTile;
        public AnimatedImage basicYellowTile;
        public AnimatedImage basicGreenTile;
        public AnimatedImage basicBlueTile;

        public AnimatedImage testPlatformTile;

        public AnimatedImage smwOutsideGrass;

        public AnimatedImage waterCrestTile;
        public AnimatedImage resisitantWaterCrestTile;
        public AnimatedImage waterBodyTile;
        //-------------------------------------------

        //----------------Platform Tiles----------------
        public AnimatedImage completeOutsideGrassPlatform;
        public AnimatedImage completeInsideGrassPlatform;

        public AnimatedImage spikePlatform;

        public AnimatedImage goalTile;
        //-------------------------------------------

        //----------------Parallax Images----------------
        public Texture2D parallaxClouds;
        public Texture2D treeTopTest;
        public Texture2D treeTrunkTest;
        public Texture2D treeTrunkDarkTest;
        public Texture2D testParallax;
        public Texture2D testParallaxBottom;

        public Texture2D newSMWCloudSingle;
        public Texture2D newSMWCloudSmall;
        public Texture2D newSMWCloudMedium;
        //-------------------------------------------

        //--------------Character Images----------------
        public AnimatedImage characterOneSpriteSheet;
        public AnimatedImage characterTwoSpriteSheet;
        //-------------------------------------------

        //--------------Object Images----------------
        public Texture2D TestLadderLeft;
        public Texture2D TestLadderRight;
        public Texture2D TestLadderRung;

        public Texture2D testBridgeTexture;

        public AnimatedImage sonicSpringImage;

        //Rails
        public AnimatedImage testRail;

        //Tight Ropes
        public AnimatedImage testTightRope;
        //-------------------------------------------

        //--------------Treasure Images----------------
        public AnimatedImage redJewelImage;
        public AnimatedImage greenJewelImage;
        public AnimatedImage blueJewelImage;
        public AnimatedImage goldJewelImage;
        //-------------------------------------------

        ContentManager cmReference;
        GraphicsDeviceManager gdmReference;
        SpriteBatch sbReference;

        public ImageLibrary(ContentManager cm, SpriteBatch sb, GraphicsDeviceManager gdm) {
            cmReference = cm;
            gdmReference = gdm;
            sbReference = sb;
            basicTileSet = new AnimatedImage(cmReference, sb, gdm.GraphicsDevice, "TileMapImages/AltBasicTileSet", new Vector2(80, 16), new Vector2(5, 1));
            
            basicRedTile = new AnimatedImage(cmReference, sb, gdm.GraphicsDevice, "TileMapImages/basicRedTile", new Vector2(80, 16), new Vector2(5, 1));
            basicOrangeTile = new AnimatedImage(cmReference, sb, gdm.GraphicsDevice, "TileMapImages/basicOrangeTile", new Vector2(80, 16), new Vector2(5, 1));
            basicYellowTile = new AnimatedImage(cmReference, sb, gdm.GraphicsDevice, "TileMapImages/basicYellowTile", new Vector2(80, 16), new Vector2(5, 1));
            basicGreenTile = new AnimatedImage(cmReference, sb, gdm.GraphicsDevice, "TileMapImages/basicGreenTile", new Vector2(80, 16), new Vector2(5, 1));
            basicBlueTile = new AnimatedImage(cmReference, sb, gdm.GraphicsDevice, "TileMapImages/basicBlueTile", new Vector2(80, 16), new Vector2(5, 1));
            
            //-------------- Character Select Screen Images -----------------
            characterSelector = cmReference.Load<Texture2D>("CharacterSelectScreen/CharacterSelector");
            //---------------------------------------------------------------

            //Water Platform
            waterCrestTile = new AnimatedImage(cmReference, sbReference, gdm.GraphicsDevice, "WaterTiles/WaterCrest", new Vector2(32, 16), new Vector2(2, 1));

            resisitantWaterCrestTile = new AnimatedImage(cmReference, sbReference, gdm.GraphicsDevice, "WaterTiles/ResistantCrest", new Vector2(32, 16), new Vector2(2, 1));

            waterBodyTile = new AnimatedImage(cmReference, sbReference, gdm.GraphicsDevice, "WaterTiles/WaterBody", new Vector2(16, 16), new Vector2(1, 1));
            //--------------------------

            //Grass Platform
            smwOutsideGrass = new AnimatedImage(cmReference, sbReference, gdm.GraphicsDevice, "PlatformTextures/GrassPlatform/SMWOutsideGrass", new Vector2(48, 48), new Vector2(3, 3));

            completeOutsideGrassPlatform = new AnimatedImage(cmReference, sbReference, gdm.GraphicsDevice, "PlatformTextures/GrassPlatform/OutsideGrass/CompleteOutsideGrass", new Vector2(48, 48), new Vector2(3, 3));
            completeInsideGrassPlatform = new AnimatedImage(cmReference, sbReference, gdm.GraphicsDevice, "PlatformTextures/GrassPlatform/InsideGrass/CompleteInsideGrass", new Vector2(48, 48), new Vector2(3, 3));

            goalTile = new AnimatedImage(cmReference, sbReference, gdm.GraphicsDevice, "PlatformTextures/GoalPlatform/GoalTile", new Vector2(16, 16), new Vector2(1, 1));

            spikePlatform = new AnimatedImage(cmReference, sbReference, gdmReference.GraphicsDevice, "PlatformTextures/SpikePlatform/SpikePlatform", new Vector2(48, 48), new Vector2(3, 3));
            //--------------------------

            testPlatformTile = new AnimatedImage(cm, sbReference, gdm.GraphicsDevice, "PlatformTextures/Square", new Vector2(16, 16), new Vector2(1, 1));

            //Parallax
            parallaxClouds = cmReference.Load<Texture2D>("ParallaxImages/Clouds");
            treeTopTest = cmReference.Load<Texture2D>("ParallaxImages/TreetopTest");
            treeTrunkTest = cmReference.Load<Texture2D>("ParallaxImages/TreeTrunkPixel");
            treeTrunkDarkTest = cmReference.Load<Texture2D>("ParallaxImages/TreeTrunkDarkPixel");
            testParallax = cmReference.Load<Texture2D>("ParallaxImages/TestParallax");
            testParallaxBottom = cmReference.Load<Texture2D>("ParallaxImages/TestParallaxBottom");

            newSMWCloudSingle = cmReference.Load<Texture2D>("ParallaxImages/SingleCloud"); ;
            newSMWCloudSmall = cmReference.Load<Texture2D>("ParallaxImages/DoubleCloudStripSmall"); ;
            newSMWCloudMedium = cmReference.Load<Texture2D>("ParallaxImages/DoubleCloudStripMedium"); ;
            //-------------------------

            //player
            characterOneSpriteSheet = new AnimatedImage(cm, sbReference, gdm.GraphicsDevice, "PlayerImages/CharacterOnePlaceholderRefined", new Vector2(1372, 563), new Vector2(18, 6));
            characterTwoSpriteSheet = new AnimatedImage(cm, sbReference, gdm.GraphicsDevice, "PlayerImages/CharacterTwoPlaceholderRefined", new Vector2(1372, 563), new Vector2(18, 6));
            //--------------------------

            //Objects
            TestLadderLeft = cmReference.Load<Texture2D>("GameObjects/Ladders/TestLadder/TestLadderLeft");
            TestLadderRight = cmReference.Load<Texture2D>("GameObjects/Ladders/TestLadder/TestLadderRight");
            TestLadderRung = cmReference.Load<Texture2D>("GameObjects/Ladders/TestLadder/TestLadderRung");

            sonicSpringImage = new AnimatedImage(cm, sb, gdm.GraphicsDevice, "GameObjects/Springs/SonicSpring", new Vector2(190, 38), new Vector2(5, 1));
            sonicSpringImage.frameTimeLimit = 5;

            testBridgeTexture = cmReference.Load<Texture2D>("GameObjects/Bridges/BridgeTexture");

            //rails
            testRail = new AnimatedImage(cm, sb, gdm.GraphicsDevice, "GameObjects/Railings/TestRailing", new Vector2(48, 48), new Vector2(3, 3));

            //tight ropes
            testTightRope = new AnimatedImage(cm, sb, gdm.GraphicsDevice, "GameObjects/TightRopes/TestTightRope", new Vector2(48, 48), new Vector2(3, 3));
            //--------------------------

            //Treasure
            redJewelImage = new AnimatedImage(cm, sb, gdm.GraphicsDevice, "Treasure/TestTreasure/RedJewel", new Vector2(17, 24), new Vector2(1, 1)); ;
            greenJewelImage = new AnimatedImage(cm, sb, gdm.GraphicsDevice, "Treasure/TestTreasure/GreenJewel", new Vector2(27, 22), new Vector2(1, 1)); ;
            blueJewelImage = new AnimatedImage(cm, sb, gdm.GraphicsDevice, "Treasure/TestTreasure/BlueJewel", new Vector2(16, 20), new Vector2(1, 1)); ;
            goldJewelImage = new AnimatedImage(cm, sb, gdm.GraphicsDevice, "Treasure/TestTreasure/GoldPearl", new Vector2(17, 17), new Vector2(1, 1)); ;
            //--------------------------
        }
    }
}
