using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine
{
    public class Chest : GameObject
    {
        private int gold;
        private bool opened;

        public Chest()
        {
            gold = 0;
            opened = false;
        }

        public void SetSpoils(int g)
        {
            gold = g;
        }

        public int Open()
        {
            opened = true;
            return gold;
        }

        public bool IsOpen()
        {
            return opened;
        }

        public void Reset()
        {
            opened = false;
        }

        public override void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {
            base.Draw(spriteBatch, texture);
        }
    }
}
