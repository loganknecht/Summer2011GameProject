using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TestPlatformer
{
    class ScreenManager
    {
        Screen previousScreen;
        Screen currentScreen;
        Stack<Screen> screens;

        public ScreenManager() {
            previousScreen = null;
            currentScreen = null;
            screens = new Stack<Screen>();
        }

        public int size {
            get {
                return screens.Count;
            }
        }
        
        public Screen getCurrentScreen {
            get {
                return currentScreen;
            }
        }

        public Screen getPreviousScreen
        {
            get
            {
                return previousScreen;
            }
        }

        public void initialize() { 
        }

        public void update(GameTime gameTimer) {
            if (currentScreen != null) {
                currentScreen.updateScreen(gameTimer);
            }
            else {
                System.Diagnostics.Debug.WriteLine("There is no current screen and the update method was run.");
            }
        }
        
        public void drawCurrentScreen(GameTime gameTimer) {
            if (currentScreen != null) {
                currentScreen.draw(gameTimer);
            }
            else {
                System.Diagnostics.Debug.WriteLine("There is no current screen and the draw method was run.");
            }
        }

        //adds the new screen to the stack and immediatley sets the currentScreen to the item at the top of the stack
        public void addScreen(Screen newScreen) {
            if (screens.Count > 0) {
                previousScreen = currentScreen;
            }
            screens.Push(newScreen);
            currentScreen = screens.Peek();
        }

        //returns the screen from the top of the stack if the size is greater than 0
        //otherise it returns null
        public Screen removeScreen() {
            Screen returnScreen = null;

            if (screens.Count > 0) {
                returnScreen = screens.Pop();
                if (screens.Count > 0)
                {
                    currentScreen = screens.Peek();
                }
                else {
                    currentScreen = null;
                }
                return returnScreen;
            }
            else {
                return returnScreen;
            }
        }
    }
}
