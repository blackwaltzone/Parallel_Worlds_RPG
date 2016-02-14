using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Parallel_Worlds
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

    public abstract class Screen
    {
        #region Fields


        public event EventHandler Exiting;
        private bool isExiting = false;
        private ScreenState state = ScreenState.TransitionOn;
        private bool isPopup = false;
        private ScreenManager screenManager;
        private TimeSpan transitionOnTime = TimeSpan.Zero;
        private TimeSpan transitionOffTime = TimeSpan.Zero;
        private float transitionPos = 1;
        private bool otherScreenHasFocus;


        #endregion


        #region Properties


        public bool IsExiting
        {
            get { return isExiting; }
            protected internal set 
            {
                bool fireEvent = !isExiting && value;
                isExiting = value;
                if (fireEvent && (Exiting != null))
                {
                    Exiting(this, EventArgs.Empty);
                }
            }
        }

        public ScreenState State
        {
            get { return state; }
            protected set { state = value; }
        }

        /// <summary>
        /// Gets the manager that this screen belongs to.
        /// </summary>
        public ScreenManager ScreenManager
        {
            get { return screenManager; }
            internal set { screenManager = value; }
        }

        public bool IsPopup
        {
            get { return isPopup; }
            protected set { isPopup = value; }
        }

        public TimeSpan TransitionOnTime
        {
            get { return transitionOnTime; }
            protected set { transitionOnTime = value; }
        }

        public TimeSpan TransitionOffTime
        {
            get { return transitionOffTime; }
            protected set { transitionOffTime = value; }
        }

        public float TransitionPosition
        {
            get { return transitionPos; }
            protected set { transitionPos = value; }
        }
        
        public float TransitionAlpha
        {
            get { return (1f - TransitionPosition); }
        }

        public bool IsActive
        {
            get
            {
                return !otherScreenHasFocus &&
                        (state == ScreenState.TransitionOn ||
                        state == ScreenState.Active);
            }
        }


        #endregion


        #region Initialize


        public virtual void LoadContent()
        {
        }

        public virtual void UnloadContent() 
        {
        }


        #endregion


        #region Update


        public virtual void Update(GameTime gameTime,
            bool focus,
            bool covered)
        {
            this.otherScreenHasFocus = focus;

            if (isExiting)
            {
                // If screen is going away, should transition off
                state = ScreenState.TransitionOff;

                if (!UpdateTransition(gameTime, transitionOffTime, 1))
                {
                    // when transition finishes, remove screen
                    screenManager.Pop(this);
                }
            }
            else if (covered)
            {
                // if screen is covered by another, transition off
                if (UpdateTransition(gameTime, transitionOffTime, 1))
                {
                    // still transitioning
                    state = ScreenState.TransitionOff;
                }
                else
                {
                    // transition finished
                    state = ScreenState.Hidden;
                }
            }
            else
            {
                // otherwise screen should transition on and become active
                if (UpdateTransition(gameTime, transitionOnTime, -1))
                {
                    // still transitioning
                    state = ScreenState.TransitionOn;
                }
                else
                {
                    // transition finished
                    state = ScreenState.Active;
                }
            }
        }

        private bool UpdateTransition(GameTime gameTime,
            TimeSpan time,
            int direction)
        {
            // how much to move by
            float delta;

            if (time == TimeSpan.Zero)
                delta = 1;
            else
                delta = (float)(gameTime.ElapsedGameTime.TotalMilliseconds /
                                time.TotalMilliseconds);

            // update position
            transitionPos += delta * direction;

            // end of transition?
            if (((direction < 0) && (transitionPos <= 0) ||
                 (direction > 0) && (transitionPos >= 1)))
            {
                transitionPos = MathHelper.Clamp(transitionPos, 0, 1);
                return false;
            }

            // still transitioning
            return true;
        }


        #endregion


        #region Draw


        public virtual void Draw(GameTime gameTime)
        {

        }


        #endregion


        #region Exit Screen / Input


        public virtual void ExitScreen()
        {

            // if screen has zero transition time, remove it
            if (TransitionOffTime == TimeSpan.Zero)
            {
                // If the screen has a zero transition time, remove it immediately.
                ScreenManager.Pop(this);
            }
            else
            {
                // Otherwise flag that it should transition off and then exit.
                isExiting = true;
            }
        }

        public virtual void HandleInput()
        {

        }


        #endregion
    }
}