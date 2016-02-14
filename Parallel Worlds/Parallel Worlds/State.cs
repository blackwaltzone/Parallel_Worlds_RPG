using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace Parallel_Worlds
{
    public interface IState
    {
        public virtual void Update(GameTime gameTime);
        public virtual void Draw();
        public virtual void OnEnter();
        public virtual void OnExit();
    }

    public class State : IState
    {
        public void Update(GameTime gameTime)
        {
        }

        public void Draw()
        {
        }

        public void OnEnter()
        {
        }

        public void OnExit()
        {
        }
    }
}