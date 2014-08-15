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
    class SwingRope
    {
        //TO DO make it so that the circle is drawn based off pythagoream theory
        ContentManager cmReference;
        GraphicsDeviceManager gdmReference;
        SpriteBatch sbReference;

        public int rotation;
        public int ropeLength;

        public float layerDepth;

        public Circle grappleCircle;
        public Vector2 ropePosition;
        public Vector2 swingVelocity;
        public Texture2D ropeTexture;
        public Texture2D grapplePointTexture;
        
        public SwingRope(ContentManager cm, GraphicsDeviceManager gdm, SpriteBatch sb, Vector2 pos, int rLength) {
            cmReference = cm;
            gdmReference = gdm;
            sbReference = sb;

            ropePosition = pos;
            ropeLength = rLength;
            rotation = 0;
            swingVelocity = Vector2.Zero;
            layerDepth = .5f;

            ropeTexture = cm.Load<Texture2D>("SwingRope/RopeTexture");
            grapplePointTexture = cm.Load<Texture2D>("SwingRope/RopeGrapplePoint");

            grappleCircle = new Circle(new Vector2(ropePosition.X + ropeTexture.Width / 2, ropePosition.Y + ropeLength), 10);
        }

        public void acceptPlayerInput(Player playerOne) {
            if (playerOne.swingRopeActionStateActive) {
                if (playerOne.movingLeft)
                {
                    if (playerOne.isDashing) {
                        rotation += 7;
                    }
                    else
                    {
                        rotation+=4;
                    }
                }
                if (playerOne.movingRight)
                {
                    if (playerOne.isDashing)
                    {
                        rotation -= 7;
                    }
                    else {
                        rotation-=4;
                    }
                }

                //resets the position of the rope when the playerOne isn't moving
                if (!playerOne.movingLeft && !playerOne.movingRight && !playerOne.isDashing) {
                    if (rotation < 180)
                    {
                        rotation -= 4;
                    }
                    else {
                        rotation += 4;
                    }
                }
                
                if (rotation < 0)
                {
                    rotation = 360;
                }
                if (rotation > 360)
                {
                    rotation = 0;
                }
                if (rotation > 75 && rotation < 180) {
                    rotation = 75;
                }
                if (rotation < 300 && rotation > 180)
                {
                    rotation = 300;
                }
                //System.Diagnostics.Debug.WriteLine(rotation);

                //grappleCircle.center.X = (float)Math.Cos(rotation) * ropeLength;
                //grappleCircle.center.Y = (float)Math.Sin(rotation) * ropeLength;

                grappleCircle.center.X = (int)(Math.Cos(rotation * (Math.PI / 180) + (Math.PI / 2)) * (ropeLength - grappleCircle.radius)) + ropePosition.X - ropeTexture.Width / 2;
                grappleCircle.center.Y = (int)(Math.Sin(rotation * (Math.PI / 180) + (Math.PI / 2)) * (ropeLength - grappleCircle.radius)) + ropePosition.Y + (int)(grappleCircle.radius / 2);
            }
            //resets the rope when the playerOne isn't on it
            else if (!playerOne.swingRopeActionStateActive) {
                if (rotation != 0)
                {
                    if (rotation > 5 && rotation < 180)
                    {
                        rotation -= 4;
                    }
                    else if(rotation > 180 && rotation < 355) {
                        rotation += 4;
                    }
                    else if (rotation <= 5 || rotation >= 355) {
                        rotation = 0;
                    }
                }
                grappleCircle.center.X = (int)(Math.Cos(rotation * (Math.PI / 180) + (Math.PI / 2)) * (ropeLength - grappleCircle.radius)) + ropePosition.X - ropeTexture.Width / 2;
                grappleCircle.center.Y = (int)(Math.Sin(rotation * (Math.PI / 180) + (Math.PI / 2)) * (ropeLength - grappleCircle.radius)) + ropePosition.Y + (int)(grappleCircle.radius / 2);
            }

            if (playerOne.ejectPlayerFromRope) {
                if (rotation <= 90 && rotation > 45) {
                    if (playerOne.movingLeft)
                    {
                        playerOne.momentum.X = -20;
                        playerOne.momentum.Y = -10;
                    }
                    if (playerOne.movingRight)
                    {
                        playerOne.momentum.X = 20;
                        playerOne.momentum.Y = -10;
                    }
                }
                else if (rotation >= 290 && rotation < 335)
                {
                    if (playerOne.movingLeft)
                    {
                        playerOne.momentum.X = -20;
                        playerOne.momentum.Y = -10;
                    }
                    if (playerOne.movingRight)
                    {
                        playerOne.momentum.X = 20;
                        playerOne.momentum.Y = -10;
                    }
                }
                else if (rotation >= 335 && rotation < 350)
                {
                    if (playerOne.movingLeft)
                    {
                        playerOne.momentum.X = -10;
                        playerOne.momentum.Y = -5;
                    }
                    if (playerOne.movingRight)
                    {
                        playerOne.momentum.X = 10;
                        playerOne.momentum.Y = -5;
                    }
                }
                playerOne.ejectPlayerFromRope = false;
            }
        }

        public void updatePlayerPositionWithGrappleCircle(Player playerOne) {
            if (playerOne.swingRopeActionStateActive)
            {
                playerOne.currentWorldPosition.X = grappleCircle.center.X - playerOne.width / 2;
                playerOne.currentWorldPosition.Y = grappleCircle.center.Y + grappleCircle.radius / 2;
            }
        }

        public void drawAltPosition(Vector2 ropePosition, Vector2 ballPosition) {
            sbReference.Draw(ropeTexture, new Rectangle((int)ropePosition.X, (int)ropePosition.Y, (int)ropeTexture.Width, ropeLength), new Rectangle(0, 0, ropeTexture.Width, ropeTexture.Height), Color.White, (float)(rotation * (Math.PI / 180)), new Vector2(ropeTexture.Width / 2, 0), SpriteEffects.None, layerDepth);
            //sbReference.Draw(grapplePointTexture, new Rectangle((int)(newPositon.X - ropeTexture.Width / 2), (int)(newPositon.Y + ropeLength), (int)(grappleCircle.radius * 2), (int)(grappleCircle.radius * 2)), new Rectangle(0, 0, (int)(grappleCircle.radius * 2), (int)(grappleCircle.radius * 2)), Color.White, (float)(rotation*(Math.PI / 180)), new Vector2(ropeTexture.Width / 2, 0), SpriteEffects.None, 1.0f);
            sbReference.Draw(grapplePointTexture, new Rectangle((int)ballPosition.X, (int)ballPosition.Y, (int)(grappleCircle.radius * 2), (int)(grappleCircle.radius * 2)), new Rectangle(0, 0, (int)(grappleCircle.radius * 2), (int)(grappleCircle.radius * 2)), Color.White, (float)(rotation * (Math.PI / 180)), new Vector2(ropeTexture.Width / 2, 0), SpriteEffects.None, layerDepth);
        }

        public void checkForAndTriggerPlayerActionState(Player playerOne)
        {
            if (!playerOne.swingRopeActionStateActive)
            {
                Rectangle modifiedPlayerRectangle = new Rectangle((int)playerOne.currentWorldPosition.X + 20, (int)playerOne.currentWorldPosition.Y + 10, (int)playerOne.width - 40, (int)(playerOne.height/3));
                if (grappleCircle.intersects(modifiedPlayerRectangle))
                {
                    if (playerOne.actionButtonBeingPressed)
                    {
                        playerOne.currentWorldPosition.X = grappleCircle.center.X - playerOne.width / 2;
                        playerOne.currentWorldPosition.Y = grappleCircle.center.Y + grappleCircle.radius / 2;
                        playerOne.actionStateIsActive = true;
                        playerOne.swingRopeActionStateActive = true;
                    }
                }
            }
        }
    }
}