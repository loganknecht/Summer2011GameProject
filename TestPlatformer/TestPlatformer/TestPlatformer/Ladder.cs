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
    class Ladder
    {

        ContentManager cmReference;
        GraphicsDeviceManager gdmReference;
        SpriteBatch sbReference;

        Texture2D ladderLeft;
        Texture2D ladderRight;
        Texture2D ladderRung;

        public Vector2 worldPosition;
        public Platform ladderTop;

        public float layerDepth;

        public int width;
        public int height;

        public Rectangle getBoundingBox
        {
            get
            {
                return new Rectangle((int)worldPosition.X, (int)worldPosition.Y, width, height);
            }
        }
        public Ladder(ContentManager cm, GraphicsDeviceManager gdm, SpriteBatch sb, ImageLibrary il, Vector2 worldPos, Vector2 ladderWidthHeight)
        {
            cmReference = cm;
            gdmReference = gdm;
            sbReference = sb;

            ladderLeft = il.TestLadderLeft;
            ladderRight = il.TestLadderRight;
            ladderRung = il.TestLadderRung;

            layerDepth = .55f;

            worldPosition = worldPos;

            width = (int)ladderWidthHeight.X;
            height = (int)ladderWidthHeight.Y;

            ladderTop = new Platform(cm, sb, gdm, il, new Vector2(width / 16, 1), new Vector2(worldPosition.X, worldPosition.Y), true, true, false, false, true, false);
        }

        public void update(GameTime gameTime, Camera camera)
        {
        }

        public void drawAltPosition(Vector2 altPos)
        {
            sbReference.Draw(ladderRung, new Rectangle((int)(altPos.X), (int)altPos.Y, (int)width, (int)height), new Rectangle(0, 0, ladderRung.Width, height), Color.White, 0f, new Vector2(0, 0), SpriteEffects.None, layerDepth);
            sbReference.Draw(ladderLeft, new Rectangle((int)altPos.X, (int)altPos.Y, (int)ladderLeft.Width, (int)height), new Rectangle(0, 0, ladderLeft.Width, ladderLeft.Height), Color.White, 0f, new Vector2(0, 0), SpriteEffects.None, layerDepth);
            sbReference.Draw(ladderRight, new Rectangle((int)(altPos.X + width - ladderRight.Width), (int)altPos.Y, ladderRight.Width, height), new Rectangle(0, 0, ladderRight.Width, ladderRight.Height), Color.White, 0f, new Vector2(0, 0), SpriteEffects.None, layerDepth);
        }

        public void checkToActivateActionState(Player player)
        {
            Rectangle playerBoundingRect = new Rectangle((int)(player.currentWorldPosition.X + player.width/2), (int)player.currentWorldPosition.Y, 1, (int)player.height);
            Rectangle ladderRectangle = new Rectangle((int)worldPosition.X, (int)worldPosition.Y, width, height);

            if (ladderRectangle.Intersects(playerBoundingRect))
            { 
                if(player.movingUp || player.movingDown && (!player.ladderActionStateActive)) {
                    player.currentWorldPosition.X = worldPosition.X + width / 2 - player.width / 2;

                    player.actionStateIsActive = true;
                    player.ladderActionStateActive = true;

                    player.ladderReference = this;

                    if (player.movingUp) {
                        player.movingUp = false;
                        player.climbingLadder = true;

                        player.playerImageSheet.setFrameConfiguration(91, 91, 94);
                        player.playerImageSheet.frameTimeLimit = 8;
                    }
                    else if(player.movingDown) {
                        player.playerImageSheet.setFrameConfiguration(92, 92, 92);
                        player.playerImageSheet.frameTimeLimit = 8;

                        player.movingDown = false;
                        player.slidingDownLadder = true;
                    }
                }
                else if (player.boundingRectangle.Top >= ladderTop.boundingRectangle.Top - player.height && !player.ladderActionStateActive)
                {
                    player.currentWorldPosition = ladderTop.checkAndFixPlatformCollision(player.currentWorldPosition, player);
                }
            }

            else if (player.ladderReference == this && !ladderRectangle.Intersects(playerBoundingRect))
            {
                player.climbingLadder = false;
                player.slidingDownLadder = false;
                player.holdingLadder = false;
                player.ladderActionStateActive = false;
                player.ladderReference = null;
            }

        }
    }
}
