using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace TestPlatformer
{
    class PauseScreen : Screen
    {
        Screen previousScreen;

        //tracks state in this screen
        KeyboardState previousKeyboardState;
        KeyboardState currentKeyboardState;

        //this is for the overlay that will designate inactive screens, it's just a transparent gray pixel stretched over the tile safe area
        Texture2D transparentOverlay;
        Vector2 overlayPosition;
        Vector2 overlaySize;

        //menu overlay for pause screen
        Texture2D menuBackground;
        Rectangle menuRectangle;

        //handles menu font
        SpriteFont menuFont;
        int menuHoveringSelection;

        public int getHoveringSelectedMenuItem {
            get {
                return menuHoveringSelection;
            }
        }

        public PauseScreen(ContentManager cReference, GraphicsDeviceManager gdmReference, SpriteBatch sbReference, Screen prevScreen) : base(cReference, gdmReference, sbReference)
        {
            transparentOverlay = cmReference.Load<Texture2D>("Pixels/TransparentPixel");
            overlayPosition = new Vector2(gdmReference.GraphicsDevice.Viewport.TitleSafeArea.X, gdmReference.GraphicsDevice.Viewport.TitleSafeArea.Y);
            overlaySize = new Vector2(gdmReference.GraphicsDevice.Viewport.TitleSafeArea.Width, gdmReference.GraphicsDevice.Viewport.TitleSafeArea.Height);
            previousScreen = prevScreen;

            menuRectangle = new Rectangle(gdmReference.GraphicsDevice.Viewport.TitleSafeArea.Center.X - gdmReference.GraphicsDevice.Viewport.TitleSafeArea.Width / 4,
                                          gdmReference.GraphicsDevice.Viewport.TitleSafeArea.Center.Y - gdmReference.GraphicsDevice.Viewport.TitleSafeArea.Height / 4,
                                          gdmReference.GraphicsDevice.Viewport.TitleSafeArea.Width / 2,
                                          gdmReference.GraphicsDevice.Viewport.TitleSafeArea.Height / 2);

            menuBackground = cmReference.Load<Texture2D>("Pixels/WhitePixel");
            menuFont = cmReference.Load<SpriteFont>("Fonts/PlaceholderFont");
            menuHoveringSelection = 1;
        }

        public override void updateScreen(GameTime gameTimer) {
            checkForUserInput();
        }

        public override void draw(GameTime gameTimer) {
            
            previousScreen.draw(gameTimer);

            sbReference.Begin();

            sbReference.Draw(transparentOverlay, gdmReference.GraphicsDevice.Viewport.TitleSafeArea, Color.White);

            sbReference.Draw(menuBackground, menuRectangle, Color.Black);

            Vector2 menuHeading = new Vector2((int)(menuRectangle.X*1.4), (int)(menuRectangle.Y*1.1));
            Vector2 beginningMenuOption = new Vector2((int)(menuRectangle.X*1.6), (int)(menuRectangle.Y*1.9));
            sbReference.DrawString(menuFont, "Pause Menu", menuHeading, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            sbReference.DrawString(menuFont, "Return To Game", beginningMenuOption, Color.White, 0f, Vector2.Zero, .5f, SpriteEffects.None, 0f);
            sbReference.DrawString(menuFont, "Exit to Title Screen", new Vector2(beginningMenuOption.X-40, beginningMenuOption.Y + menuRectangle.Height*.15f), Color.White, 0f, Vector2.Zero, .5f, SpriteEffects.None, 0f);
            if (menuHoveringSelection == 1) {
                //System.Diagnostics.Debug.WriteLine("Line 1");
                sbReference.DrawString(menuFont, ".", new Vector2(beginningMenuOption.X-20, beginningMenuOption.Y - menuRectangle.Height*.10f), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }
            else if (menuHoveringSelection == 2)
            {
                //System.Diagnostics.Debug.WriteLine("Line 2");
                sbReference.DrawString(menuFont, ".", new Vector2(beginningMenuOption.X - 60, beginningMenuOption.Y + menuRectangle.Height * .06f), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }
            

            sbReference.End();
        }

        public void checkForUserInput()
        {
            previousKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();
            
            //System.Diagnostics.Debug.WriteLine(menuItemSelected);

            if (previousKeyboardState.IsKeyUp(Keys.Up) && currentKeyboardState.IsKeyDown(Keys.Up)) {
                if (menuHoveringSelection == 1) {
                    menuHoveringSelection = 2;
                }
                else if (menuHoveringSelection == 2) {
                    menuHoveringSelection = 1;
                }
            }
            if (previousKeyboardState.IsKeyUp(Keys.Down) && currentKeyboardState.IsKeyDown(Keys.Down))
            {
                if (menuHoveringSelection == 1)
                {
                    menuHoveringSelection = 2;
                }
                else if (menuHoveringSelection == 2)
                {
                    menuHoveringSelection = 1;
                }
            }
        }
    }
}
