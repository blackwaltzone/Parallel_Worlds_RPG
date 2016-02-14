using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Parallel_Worlds
{
    public interface IScreen
    {
        //public SpriteFont Font;
        void Update(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch);
        void OnEnter();
        void OnExit();
    }

    public class Screen : IScreen
    {
        /// <summary>
        /// Enum describes the screen transition state.
        /// </summary>
        public enum ScreenState
        {
            TransitionOn,
            Active,
            TransitionOff,
            Hidden,
        }

        public SpriteFont Font;

        public bool IsExiting
        {
            get { return isExiting; }
            set { isExiting = value; }
        }

        private bool isExiting = false;

        public ScreenState State
        {
            get { return state; }
            set { state = value; }
        }

        private ScreenState state = ScreenState.TransitionOn;

        /// <summary>
        /// Gets the manager that this screen belongs to.
        /// </summary>
        public ScreenManager ScreenManager
        {
            get { return screenManager; }
            internal set { screenManager = value; }
        }

        ScreenManager screenManager;

        public virtual void Update(GameTime gameTime)
        {
            if (isExiting)
            {
                // If screen is going away, should transition off
                state = ScreenState.TransitionOff;
            }
            else
            {
                state = ScreenState.Active;
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
        }

        public virtual void OnEnter()
        {
        }

        public virtual void OnExit()
        {
            //if (TransitionOffTime == TimeSpan.Zero)
            //{
            //    // If the screen has a zero transition time, remove it immediately.
            //ScreenManager.Pop();
            //}
            //else
            //{
                // Otherwise flag that it should transition off and then exit.
                isExiting = true;
            //}
        }

        public void HandleInput()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                OnExit();
        }
    }
}