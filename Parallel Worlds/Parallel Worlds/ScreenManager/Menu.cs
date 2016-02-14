using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Parallel_Worlds
{
    public class Menu : Screen
    {
        #region Fields

        
        private List<MenuItem> menuItems = new List<MenuItem>();
        protected int selectedItem = 0;

        
        #endregion


        #region Properties


        public List<MenuItem> MenuItems
        {
            get { return menuItems; }
        }

        public MenuItem SelectedItem
        {
            get
            {
                if (selectedItem < 0 || selectedItem >= menuItems.Count)
                    return null;
                return menuItems[selectedItem];
            }
        }


        #endregion


        public Menu()
        {
            TransitionOnTime = TimeSpan.FromSeconds(0.75);
            TransitionOffTime = TimeSpan.FromSeconds(0.1);
        }

        public override void LoadContent()
        {

        }

        public override void UnloadContent()
        {

        }


        #region Input


        public override void HandleInput()
        {
            int oldSelection = selectedItem;

            if (InputManager.IsActionTriggered(InputManager.Action.CursorUp))
            {
                selectedItem--;
                if (selectedItem < 0)
                    selectedItem = menuItems.Count - 1;
            }
            if (InputManager.IsActionTriggered(InputManager.Action.CursorDown))
            {
                selectedItem++;
                if (selectedItem >= menuItems.Count)
                    selectedItem = 0;
            }
            if (InputManager.IsActionTriggered(InputManager.Action.Select))
            {
                OnSelection(selectedItem);
            }
            if (InputManager.IsActionTriggered(InputManager.Action.Exit))
            {
                OnCancel();
            }
            if (selectedItem != oldSelection)
            {
                // play menu noise
            }
        }

        protected virtual void OnCancel()
        {
            ExitScreen();
        }

        protected void OnCancel(object sender, EventArgs e)
        {
            OnCancel();
        }

        protected virtual void OnSelection(int selectedItem)
        {
            menuItems[selectedItem].OnSelectEntry();
        }


        #endregion


        #region Update and Draw


        public override void Update(GameTime gameTime,
            bool focus,
            bool covered)
        {
            base.Update(gameTime, focus, covered);

            for (int i = 0; i < menuItems.Count; i++)
            {
                bool isSelected = IsActive && (i == selectedItem);

                menuItems[i].Update(this, isSelected, gameTime);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            spriteBatch.Begin();

            for (int i = 0; i < menuItems.Count; i++)
            {
                MenuItem item = menuItems[i];
                bool isSelected = IsActive && (i == selectedItem);
                item.Draw(this, isSelected, gameTime);
            }

            Color titleColor = Color.White * TransitionAlpha;

            spriteBatch.DrawString(ScreenManager.Font,
                "Menu Title",
                new Vector2(20, 20),
                titleColor);

            spriteBatch.End();
        }


        #endregion
    }
}
