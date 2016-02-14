using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine
{
    public class Block : GameObject
    {
        public bool IsMoveable = false;
        public bool IsMoving = false;
        private const float Speed = 1f;

        public Vector2 NewPosition;
        private Vector2 autoMovement = Vector2.Zero;

        public void PushUp()
        {
            NewPosition = this.Position + new Vector2(0, -32);
            IsMoving = true;
        }

        public void PushDown()
        {
            NewPosition = this.Position + new Vector2(0, 32);
            IsMoving = true;
        }

        public void PushLeft()
        {
            NewPosition = this.Position + new Vector2(-32, 0);
            IsMoving = true;
        }

        public void PushRight()
        {
            NewPosition = this.Position + new Vector2(32, 0);
            IsMoving = true;
        }

        public void Push(Vector2 dir)
        {
            NewPosition = Position + dir;
            IsMoving = true;
        }

        public void Update(GameTime gameTime)
        {
            if (IsMoving)
            {
                if (Position != NewPosition)
                {
                    Position = NewPosition;
                    IsMoving = false;
                }
                else
                    IsMoving = false;
            }
        }

        public override void Draw(SpriteBatch spriteBatch, Texture2D blockTexture)
        {
            base.Draw(spriteBatch, blockTexture);
        }
    }
}
