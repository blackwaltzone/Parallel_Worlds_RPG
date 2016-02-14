using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace Engine
{
    public class Camera
    {
        public Vector2 Position = Vector2.Zero;
        public Vector2 Target = Vector2.Zero;
        public Vector2 Size = Vector2.Zero;
        public float Speed = 0.0f;

        public Camera()
        {

        }

        public Camera(int w, int h, float s)
        {
            Size.X = w;
            Size.Y = h;

            if (s < 0.0)
                this.Speed = 0.0f;
            if (s > 1.0)
                this.Speed = 1.0f;
        }

        public void Move(int x, int y)
        {
            Position.X = (float)x;
            Position.Y = (float)y;
            Target.X = (float)x;
            Target.Y = (float)y;
        }

        public void MoveCenter(int x, int y)
        {
            x = x - (int)(Size.X / 2);
            y = y - (int)(Size.Y / 2);

            Position.X = (float)x;
            Position.Y = (float)y;
            Target.X = (float)x;
            Target.Y = (float)y;
        }

        public void GoTo(int x, int y)
        {
            Target.X = (float)x;
            Target.Y = (float)y;
        }

        public void GoToCenter(int x, int y)
        {
            x = x - (int)(Size.X / 2);
            y = y - (int)(Size.Y / 2);

            Target.X = (float)x;
            Target.Y = (float)y;
        }

        // allows us to do a camera scrolling effect by moving towards
        // a target position over time
        public void Update()
        {
            // x distance to target, y distance to target, and Euclidean distance
            float x, y, d;

            // velocity magnitudes
            float vx, vy, v;

            // find x and y
            x = (float)(Target.X - Position.X);
            y = (float)(Target.Y - Position.Y);

            // if within 1 pixel, snap to target
            if ((x * x + y * y) <= 1)
            {
                Position.X = Target.X;
                Position.Y = Target.Y;
            }
            else
            {
                // distance formula
                d = (float)Math.Sqrt((x * x + y * y));

                // set velocity to move 1/60th distance to target
                // intend to run 60 times/s
                // allow user to change camera speed
                v = (d * Speed) / 60;

                // keep v above 1 pixel per update, otherwise may never get
                // to target. v is absolute value (squaring of x)
                if (v < 1.0f)
                    v = 1.0f;

                // similar triangles to get vx and vy
                vx = x * (v / d);
                vy = y * (v / d);

                // update camera's position
                Position.X += vx;
                Position.Y += vy;
            }
        }

        // keep camera from moving past map boundaries
        public void ClampToArea(int width, int height)
        {
            // make sure camera doesn't scroll off bottom or right
            if (Position.X > width)
                Position.X = width;
            if (Position.Y > height)
                Position.Y = height;

            // make sure camera doesn't scroll off top or left
            if (Position.X < 0)
                Position.X = 0;
            if (Position.Y < 0)
                Position.Y = 0;
        }

        public void LockToTarget(Vector2 pos, int screenWidth, int screenHeight)
        {
            // set camera position based on sprite position
            Position.X = pos.X/* +
                (sprite.CurrentAnimation.CurrentRectangle.Width / 2) -
                (screenWidth / 2)*/;
            Position.Y = pos.Y/* +
                (sprite.CurrentAnimation.CurrentRectangle.Height / 2) -
                (screenHeight / 2)*/;
        }
    }
}
