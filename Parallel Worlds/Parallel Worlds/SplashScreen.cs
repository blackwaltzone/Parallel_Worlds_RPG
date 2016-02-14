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
        public override void Update(GameTime gameTime)
        {
            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(Font, "Splash screen", new Vector2(50, 50), Color.White);
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
