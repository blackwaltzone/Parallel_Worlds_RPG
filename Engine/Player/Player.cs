using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Engine
{
    public class Player
    {
        public int Gold;

        private Texture2D sprite;
        public Texture2D Sprite
        {
            get { return sprite; }
            set { sprite = value; }
        }

        //private Vector2 position;
        public Vector2 Position = Vector2.Zero;
        //public Vector2 Position
        //{
        //    get { return position; }
        //    set
        //    {
        //        position.X = value.X;
        //        position.Y = value.Y;
        //    }
        //}

        private Vector2 size;
        public Vector2 Size
        {
            get { return size; }
            set { size = value; }
        }

        private Vector2 center;
        public Vector2 Center
        {
            get
            {
                center = new Vector2(
                          Position.X + (size.X / 2),
                          Position.Y + (size.Y / 2));
                return center;
            }
            set { center = value; }
        }

        public Vector2 MapPosition
        {
            get { return new Vector2(Position.X / 32, Position.Y / 32); }
        }

        private Rectangle rect;
        public Rectangle Rectangle
        {
            get
            {
                rect = new Rectangle(
                      (int)Position.X,
                      (int)Position.Y,
                      (int)size.X,
                      (int)size.Y);
                return rect;
            }
            set { rect = value; }
        }

        /// <summary>
        /// The speed of the party leader, in units per second.
        /// </summary>
        /// <remarks>
        /// The movementCollisionTolerance constant should be a multiple of this number.
        /// </remarks>
        private const float movementSpeed = 2.0f;
        private const float autoMovementSpeed = 1.0f;

        /// <summary>
        /// The automatic movement remaining for the party leader.
        /// </summary>
        /// <remarks>
        /// This is typically used for automatic movement when spawning on a map.
        /// </remarks>
        private static Vector2 autoMovement = Vector2.Zero;
        public void SetAutoMovement(Vector2 target)
        {
            autoMovement = target;
        }

        public Vector2 GetAutoMovement()
        {
            return autoMovement;
        }

        /// <summary>
        /// Updates the automatic movement of the party.
        /// </summary>
        /// <returns>The automatic movement for this update.</returns>
        public static Vector2 UpdateAutoMovement(GameTime gameTime)
        {
            // check for any remaining auto-movement
            if (autoMovement == Vector2.Zero)
            {
                return Vector2.Zero;
            }

            // get the remaining-movement direction
            Vector2 autoMovementDirection = Vector2.Normalize(autoMovement);

            // calculate the potential movement vector
            Vector2 movement = Vector2.Multiply(autoMovementDirection,
                autoMovementSpeed);

            // limit the potential movement vector by the remaining auto-movement
            movement.X = Math.Sign(movement.X) * MathHelper.Min(Math.Abs(movement.X),
                Math.Abs(autoMovement.X));
            movement.Y = Math.Sign(movement.Y) * MathHelper.Min(Math.Abs(movement.Y),
                Math.Abs(autoMovement.Y));

            // remove the movement from the total remaining auto-movement
            autoMovement -= movement;

            return movement;
        }

        public void Load(ContentManager content, string file)
        {
            sprite = content.Load<Texture2D>(file);
            Size = new Vector2(32, 32);
            rect = new Rectangle(
                        (int)Position.X,
                        (int)Position.Y,
                        (int)size.X,
                        (int)size.Y);
        }

        public void Update(GameTime gameTime, Vector2 direction)
        {
            Vector2 auto = UpdateAutoMovement(gameTime);

            // update position in regards to motion
            Position += auto + direction * movementSpeed;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(sprite, Rectangle, Color.White);
            spriteBatch.End();
        }

        // keep player sprite within map bounds
        public void Clamp(int width, int height)
        {
            // sprite can't walk off upper or left border
            if (Position.X < 0)
                Position.X = 0;
            if (Position.Y < 0)
                Position.Y = 0;

            // sprite can't walk off lower or right border
            if (Position.X >
                width - 32)
                //CurrentAnimation.CurrentRectangle.Width)
            {
                Position.X =
                    width - 32;
                    //CurrentAnimation.CurrentRectangle.Width;
            }
            if (Position.Y >
                height - 32)
                //CurrentAnimation.CurrentRectangle.Height)
            {
                Position.Y =
                    height - 32;
                    //CurrentAnimation.CurrentRectangle.Height;
            }
        }
    }
}
