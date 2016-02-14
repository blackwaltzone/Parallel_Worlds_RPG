using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;


namespace Parallel_Worlds
{
    public static class ScreenManager //: DrawableGameComponent
    {
        #region Fields


        private static List<Screen> Stack = new List<Screen>();
        private static List<Screen> screensToUpdate = new List<Screen>();
        private static bool isInitialized;
        private static bool traceEnabled = false;
        public static SpriteFont Font;
        private static SpriteBatch spriteBatch;
        private static Texture2D fadeTexture;
        public static ContentManager Content;
        public static GraphicsDevice GraphicsDevice;
        public static Game Game;


        #endregion


        #region Properties


        public static SpriteBatch SpriteBatch
        {
            get { return spriteBatch; }
        }

        public static bool TraceEnabled
        {
            get { return traceEnabled; }
            set { traceEnabled = value; }
        }

        public static Screen[] GetScreens()
        {
            return Stack.ToArray();
        }


        #endregion


        #region Initialize


        public static void Initialize()
        {
            //base.Initialize();

            isInitialized = true;
        }

        public static void LoadContent(Game g)
        {
            Game = g;
            Content = g.Content;
            GraphicsDevice = g.GraphicsDevice;
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Font = Content.Load<SpriteFont>("Arial");
            fadeTexture = Content.Load<Texture2D>("Textures/FadeScreen");

            foreach (Screen s in Stack)
                s.LoadContent();

            //Push(new SplashScreen());
        }


        #endregion


        #region Update


        public static void Update(GameTime gameTime)
        {
            screensToUpdate.Clear();

            foreach (Screen s in Stack)
                screensToUpdate.Add(s);

            bool otherScreenHasFocus = !Game.IsActive;
            bool coveredByOtherScreen = false;

            while (screensToUpdate.Count > 0)
            {
                Screen screen = screensToUpdate[screensToUpdate.Count - 1];
                screensToUpdate.RemoveAt(screensToUpdate.Count - 1);

                screen.Update(gameTime,
                    otherScreenHasFocus,
                    coveredByOtherScreen);

                if (screen.State == ScreenState.TransitionOn ||
                    screen.State == ScreenState.Active)
                {
                    if (!otherScreenHasFocus)
                    {
                        screen.HandleInput();
                        otherScreenHasFocus = true;
                    }

                    if (!screen.IsPopup)
                        coveredByOtherScreen = true;
                }
            }

            if (traceEnabled)
                TraceScreens();
        }

        private static void TraceScreens()
        {
            List<string> screens = new List<string>();

            foreach (Screen s in Stack)
                screens.Add(s.GetType().Name);

            Trace.WriteLine(string.Join(", ", screens.ToArray()));
        }


        #endregion


        #region Draw


        public static void Draw(GameTime gameTime)
        {
            foreach (Screen s in Stack)
            {
                if (s.State == ScreenState.Hidden)
                    return;

                s.Draw(gameTime);
            }
        }

        public static void FadeBackBufferToBlack(float alpha)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(fadeTexture, GraphicsDevice.Viewport.Bounds, Color.Black * alpha);
            spriteBatch.End();
        }


        #endregion


        #region Modify Stack


        public static void Push(Screen name)
        {
            //name.ScreenManager = this;
            name.IsExiting = false;

            if (isInitialized)
                name.LoadContent();

            Stack.Add(name);
        }

        public static void Pop(Screen name)
        {
            if (isInitialized)
                name.UnloadContent();

            Stack.Remove(name);
            screensToUpdate.Remove(name);
        }

        public static void Switch(Screen name)
        {
            if (Stack.Count > 0)
            {
                Pop(name);

                Push(name);
            }
        }


        #endregion
    }
}