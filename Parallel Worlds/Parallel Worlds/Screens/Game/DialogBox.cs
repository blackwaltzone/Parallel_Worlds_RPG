using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace Parallel_Worlds
{
    public class DialogBox : Screen
    {
        #region Fields


        private string message;
        private string displayedMessage;
        private Vector2 messagePos;
        private Vector2 messageSize;
        private Vector2 messageOffset;
        private Rectangle bgRect;

        private Texture2D texture;
        private TimeSpan time = TimeSpan.Zero;


        #endregion


        #region Events


        public event EventHandler<EventArgs> Accepted;
        // Other options?
        //public event EventHandler<EventArgs> Option1;
        //public event EventHandler<EventArgs> Option2;


        #endregion


        #region Initialization


        public DialogBox(string message, string[] options)
        {
            this.message = message;
            displayedMessage = "";

            this.message = Fonts.BreakTextIntoLines(this.message, 100);

            IsPopup = true;

            TransitionOnTime = TimeSpan.FromSeconds(0.1);
            TransitionOffTime = TimeSpan.FromSeconds(0.1);
        }

        public override void LoadContent()
        {
            ContentManager content = ScreenManager.Game.Content;

            messagePos = new Vector2(20, 25);
            messageOffset = new Vector2(32, 16);
            texture = content.Load<Texture2D>("Textures/DialogBox");
        }


        #endregion


        #region Input


        public override void HandleInput()
        {
            if (InputManager.IsActionTriggered(InputManager.Action.Select))
            {
                if (Accepted != null)
                    Accepted(this, EventArgs.Empty);

                ExitScreen();
            }
            //else if (InputManager.IsActionTriggered(InputManager.Action.CursorUp))
            //else if (InputManager.IsActionTriggered(InputManager.Action.CursorDown))
        }


        #endregion


        #region Update and Draw


        public override void Update(GameTime gameTime, bool focus, bool covered)
        {
            base.Update(gameTime, focus, covered);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont font = ScreenManager.Font;
            Vector2 viewportSize = new Vector2(
                ScreenManager.GraphicsDevice.Viewport.Width,
                ScreenManager.GraphicsDevice.Viewport.Height);
            messageSize = font.MeasureString(message);
            bgRect = new Rectangle(
                (int)(0),
                (int)(viewportSize.Y - messageSize.Y - (messageOffset.Y * 2)),
                (int)(viewportSize.X),
                (int)(messageSize.Y + messageOffset.Y * 2));
            messagePos = new Vector2(bgRect.X + messageOffset.X, bgRect.Y + messageOffset.Y);
            Color color = Color.White * TransitionAlpha;

            spriteBatch.Begin(
                SpriteSortMode.Immediate,
                BlendState.AlphaBlend);

            // *****************
            // TODO animate text box
            // *****************
            spriteBatch.Draw(texture, bgRect, color);

            // draw animated text
            spriteBatch.DrawString(font, GetLetter(gameTime), messagePos, color);

            spriteBatch.End();
        }


        private string GetLetter(GameTime gameTime)
        {
            if (displayedMessage != message)
            {
                if (gameTime.ElapsedGameTime > TimeSpan.FromMilliseconds(0.1))
                {
                    displayedMessage += message[displayedMessage.Count()];
                }
            }
            return displayedMessage;
        }


        #endregion
    }
}
