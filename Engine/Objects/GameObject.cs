using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine
{
    public class GameObject
    {
        public string Name;
        public Vector2 Position;

        public Vector2 PixelPosition
        {
            get { return Position * 32; }
        }

        public Vector2 Size;
        //public Texture2D Texture;

        public virtual void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(
                texture,
                new Rectangle(
                    (int)PixelPosition.X,
                    (int)PixelPosition.Y,
                    (int)Size.X,
                    (int)Size.Y),
                Color.White);
            spriteBatch.End();
        }
    }
}
