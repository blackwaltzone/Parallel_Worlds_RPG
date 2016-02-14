using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Parallel_Worlds
{
    public class MenuScreen : Menu
    {
        #region Graphics


        #endregion


        #region Menu Items


        MenuItem startItem, exitItem;
        MenuItem controlsItem, helpItem;


        #endregion


        #region Initialization


        public MenuScreen()
            : base()
        {
            startItem = new MenuItem("New Game");
            startItem.Description = "Start a new game.";
            startItem.Position = new Vector2(50, 150);
            startItem.Selected += StartMenuItemSelected;
            MenuItems.Add(startItem);

            exitItem = new MenuItem("Exit");
            exitItem.Description = "Exit game.";
            exitItem.Position = new Vector2(50, 175);
            exitItem.Selected += ExitMenuItemSelected;
            MenuItems.Add(exitItem);

            controlsItem = new MenuItem("Controls");
            controlsItem.Description = "View game controls.";
            controlsItem.Position = new Vector2(50, 200);
            controlsItem.Selected += ControlsMenuItemSelected;
            MenuItems.Add(controlsItem);

            helpItem = new MenuItem("Help");
            helpItem.Description = "About the game";
            helpItem.Position = new Vector2(50, 225);
            helpItem.Selected += HelpMenuItemSelected;
            MenuItems.Add(helpItem);
        }

        public override void LoadContent()
        {
            ContentManager content = ScreenManager.Game.Content;


        }


        #endregion


        #region Update


        public override void HandleInput()
        {
            if (InputManager.IsActionTriggered(InputManager.Action.Back))
            {
                ExitScreen();
                ScreenManager.Push(new SplashScreen());
                return;
            }

            base.HandleInput();
        }

        private void StartMenuItemSelected(object sender, EventArgs e)
        {
            if (this.IsActive)
            {
                ExitScreen();
            }

            LoadingScreen.Load(/*ScreenManager, */true, new GameScreen());
        }

        private void ExitMenuItemSelected(object sender, EventArgs e)
        {
            if (this.IsActive)
            {
                ExitScreen();
            }

            ScreenManager.Switch(new SplashScreen());
        }

        private void ControlsMenuItemSelected(object sender, EventArgs e)
        {

        }

        private void HelpMenuItemSelected(object sender, EventArgs e)
        {

        }

        protected override void OnCancel()
        {
            string message = String.Empty;
            if (this.IsActive)
            {
                message = "Are you sure you want to exit?" +
                    "All unsaved progress will be lost.";
            }
            else
            {
                message = "Are you sure you want to exit?";
            }

            
             MessageBox confirmExitBox = new MessageBox(message, true);
             confirmExitBox.Accepted += ConfirmAccepted;
             ScreenManager.Push(confirmExitBox);             
        }

        private void ConfirmAccepted(object sender, EventArgs e)
        {
            ScreenManager.Game.Exit();
        }


        #endregion
    }
}
