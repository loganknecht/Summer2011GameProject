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
    class BearTrap
    {
        public ContentManager cmReference;
        public GraphicsDeviceManager gdmReference;
        public SpriteBatch sbReference;

        public bool wasActivated;

        public float layerDepth;

        public AnimatedImage bearTrapImage;

        public Vector2 worldPosition;

        public Rectangle getBoundingBox {
            get {
                return new Rectangle((int)worldPosition.X, (int)worldPosition.Y, (int)bearTrapImage.frameSize.X, (int)bearTrapImage.frameSize.Y);
            }
        }

        public BearTrap(ContentManager cm, GraphicsDeviceManager gdm, SpriteBatch sb, Vector2 worldPos) {
            cmReference = cm;
            gdmReference = gdm;
            sbReference = sb;

            bearTrapImage = new AnimatedImage(cmReference, sbReference, gdmReference.GraphicsDevice, "GameObjects/Traps/BearTrapPlaceholder", new Vector2(120, 20), new Vector2(3, 1));
            bearTrapImage.isAnimating = false;
            bearTrapImage.animateOnce = false;
            bearTrapImage.frameTimeLimit = 4;
            bearTrapImage.setFrameConfiguration(0, 0, 2);

            worldPosition = worldPos;

            layerDepth = .5f;

            wasActivated = false;
        }

        public void update(GameTime gameTime) {
            bearTrapImage.Update(gameTime);
        }

        public void drawAltPosition(Vector2 altPos)
        {
            bearTrapImage.DrawAltPosition(altPos, false, layerDepth);
        }

        public void checkToActivate(Player player) {
            if (!player.wasKilled)
            {
                if (wasActivated)
                {
                }
                //if trap hasn't been activated
                else
                {
                    Rectangle playerRect = new Rectangle(player.boundingRectangle.Left, player.boundingRectangle.Bottom, player.boundingRectangle.Width, 1);
                    if (getBoundingBox.Intersects(playerRect))
                    {
                        bearTrapImage.animateOnce = true;
                        player.wasKilled = true;
                    }
                }
            }
        }
    }
}
