using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Parallel_Worlds
{
    public class MainMenu : Screen
    {
        public override void Update(GameTime gameTime)
        {
            //KeyboardState input = Keyboard.GetState();

            //if (input.IsKeyDown(Keys.Tab))
            //{
            //    GameScreen gameScreen = new GameScreen();
            //    ScreenManager.Push(gameScreen);
            //}
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(Font, "Main menu screen", new Vector2(50, 100), Color.White);
            spriteBatch.End();
        }

        public override void OnEnter()
        {
        }

        public override void OnExit()
        {
        }
    }
}
