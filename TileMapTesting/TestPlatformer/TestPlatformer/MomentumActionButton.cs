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
    class MomentumActionButton
    {
        ContentManager cmReference;
        GraphicsDeviceManager gdmReference;
        SpriteBatch sbReference;

        AnimatedImage imageSheet;
        public Vector2 worldPosition;
        public Vector2 momentumToImpart;

        public int width
        {
            get
            {
                return (int)imageSheet.frameSize.X;
            }
        }

        public int height
        {
            get
            {
                return (int)imageSheet.frameSize.Y;
            }
        }

        public Rectangle getBoundingBox
        {
            get
            {
                return new Rectangle((int)worldPosition.X, (int)worldPosition.Y, width, height);
            }
        }
        public MomentumActionButton(ContentManager cm, GraphicsDeviceManager gdm, SpriteBatch sb, Vector2 momentumTransferred)
        {
            cmReference = cm;
            gdmReference = gdm;
            sbReference = sb;

            momentumToImpart = momentumTransferred;

            imageSheet = new AnimatedImage(cm, sb, gdmReference.GraphicsDevice, "ActionButton", new Vector2(30, 30), new Vector2(1, 1));

            worldPosition = Vector2.Zero;
        }

        public void update(GameTime gameTime, Camera camera)
        {
            imageSheet.position = worldPosition;
            imageSheet.Update(gameTime);
        }

        public void draw(Camera camera)
        {
            imageSheet.Draw();
        }

        public void drawAltPosition(Vector2 altPos)
        {
            imageSheet.DrawAltPosition(altPos);
        }

        //this triggers the players action state for the hurdle and because of that sets the player on course to finish a scripted interaction with the object
        public void checkForAndTriggerPlayerState(Player player)
        {
            if (getBoundingBox.Intersects(player.boundingRectangle))
            {
                if (player.actionButtonBeingPressed)
                {
                    player.momentum = momentumToImpart;
                }
            }
        }
    }
}
