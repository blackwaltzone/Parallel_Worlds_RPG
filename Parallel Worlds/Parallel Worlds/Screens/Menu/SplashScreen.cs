using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Parallel_Worlds
{
    public class SplashScreen : Screen
    {
        public SplashScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(1.5);
        }


        public override void Update(GameTime gameTime,
            bool focus,
            bool covered)
        {
            base.Update(gameTime, focus, covered);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            spriteBatch.Begin();
            Color color = Color.White * TransitionAlpha;
            spriteBatch.DrawString(ScreenManager.Font, "Splash screen", new Vector2(50, 50), color);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        public override void HandleInput()
        {
            if (InputManager.IsActionTriggered(InputManager.Action.Select))
            {
                ExitScreen();
                ScreenManager.Push(new MenuScreen());
            }
            else if (InputManager.IsActionTriggered(InputManager.Action.Exit))
            {
                string message = "Are you sure you want to exit?";
                MessageBox exitBox = new MessageBox(message, true);
                exitBox.Accepted += ConfirmAccepted;
                ScreenManager.Push(exitBox);
            }
        }

        private void ConfirmAccepted(object sender, EventArgs e)
        {
            ScreenManager.Game.Exit();
        }
    }
}
