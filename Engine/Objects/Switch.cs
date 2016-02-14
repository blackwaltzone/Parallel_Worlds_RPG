using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine
{
    public class Switch : GameObject
    {
        private bool switchOn;
        private float rotation;
        private Color color;

        public Switch()
        {
            switchOn = false;
            rotation = 0f;
            color = Color.Red;
        }

        public void Activate()
        {
            switchOn = !switchOn;

            if (switchOn)
                color = Color.Green;
            else
                color = Color.Red;
        }

        public void Reset()
        {
            switchOn = false;
            color = Color.Red;
        }

        public bool Status()
        {
            return switchOn;
        }

        public void Rotate(int r)
        {
            rotation = r;
        }

        public void RotateRight()
        {
            rotation += 90f;
        }

        public void RotateLeft()
        {
            rotation -= 90f;
        }

        public override void Draw(SpriteBatch spriteBatch, Texture2D Texture)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(
                Texture,
                new Rectangle(
                    (int)PixelPosition.X,
                    (int)PixelPosition.Y,
                    (int)Size.X,
                    (int)Size.Y),
                color);
            spriteBatch.End();
        }
    }
}
