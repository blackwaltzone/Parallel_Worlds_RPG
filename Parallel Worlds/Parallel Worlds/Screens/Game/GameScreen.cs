using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using Engine;

namespace Parallel_Worlds
{
    public class GameScreen : Screen
    {
        #region Fields


        public static Texture2D boxTexture;
        public static bool setBoundingBoxes = true;
        //public static World World;
        public static float AlphaStep;


        #endregion


        #region Constructor and Loading


        public GameScreen()
            : base()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(1.5);
        }

        public override void LoadContent()
        {
            World.Load(ScreenManager.Content);// (ScreenManager.Game.Content);
        }


        #endregion


        #region Update


        public override void Update(GameTime gameTime,
            bool focus,
            bool covered)
        {
            base.Update(gameTime, focus, covered);

            World.Update(gameTime);

            if (covered)
                AlphaStep = Math.Min(AlphaStep + 1f / 32, 1);
            else
                AlphaStep = Math.Max(AlphaStep - 1f / 32, 0);
        }


        #endregion


        #region Draw


        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            World.Draw(spriteBatch);

            if (TransitionPosition > 0 || AlphaStep > 0)
            {
                float a = MathHelper.Lerp(1f - TransitionAlpha, 1f, AlphaStep / 2);
                ScreenManager.FadeBackBufferToBlack(a);
            }
        }


        #endregion


        #region Input


        public override void HandleInput()
        {
            if (InputManager.IsActionTriggered(InputManager.Action.Exit))
            {
                // add a confirmation message box
                const string message =
                    "Are you sure you want to exit?  All unsaved progress will be lost." +
                    "\n\n <Esc>   : cancel " +
                    "\n <Enter> : confirm";
                MessageBox exitBox = new MessageBox(message, true);
                exitBox.Accepted += ConfirmExitAccepted;
                ScreenManager.Push(exitBox);
                //return;
            }
        }

        /// <summary>
        /// Event handler for when the user selects Yes 
        /// on the "Are you sure?" message box.
        /// </summary>
        void ConfirmExitAccepted(object sender, EventArgs e)
        {
            //ExitScreen();
            //ScreenManager.Push(new MenuScreen());
            LoadingScreen.Load(
                /*ScreenManager,*/
                false,
                null,
                new BackgroundScreen(), new MenuScreen());
        }


        #endregion
    }
}
