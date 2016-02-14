using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Parallel_Worlds
{
    public class MenuItem
    {
        #region Fields

        /// <summary>
        /// The text rendered for this entry.
        /// </summary>
        string text;


        /// <summary>
        /// The font used for this menu item.
        /// </summary>
        SpriteFont spriteFont;


        /// <summary>
        /// The position of this menu item on the screen.
        /// </summary>
        Vector2 position;


        /// <summary>
        /// A description of the function of the button.
        /// </summary>
        private string description;


        /// <summary>
        /// An optional texture drawn with the text.
        /// </summary>
        /// <remarks>If present, the text will be centered on the texture.</remarks>
        private Texture2D texture;


        #endregion


        #region Properties


        /// <summary>
        /// Gets or sets the text of this menu entry.
        /// </summary>
        public string Text
        {
            get { return text; }
            set { text = value; }
        }


        /// <summary>
        /// Gets or sets the font used to draw this menu entry.
        /// </summary>
        public SpriteFont Font
        {
            get { return spriteFont; }
            set { spriteFont = value; }
        }


        /// <summary>
        /// Gets or sets the position of this menu entry.
        /// </summary>
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }


        /// <summary>
        /// A description of the function of the button.
        /// </summary>
        public string Description
        {
            get { return description; }
            set { description = value; }
        }


        /// <summary>
        /// An optional texture drawn with the text.
        /// </summary>
        /// <remarks>If present, the text will be centered on the texture.</remarks>
        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }


        #endregion


        #region Events


        /// <summary>
        /// Event raised when the menu entry is selected.
        /// </summary>
        public event EventHandler<EventArgs> Selected;


        /// <summary>
        /// Method for raising the Selected event.
        /// </summary>
        protected internal virtual void OnSelectEntry()
        {
            if (Selected != null)
                Selected(this, EventArgs.Empty);
        }


        #endregion


        #region Initialization


        /// <summary>
        /// Constructs a new menu entry with the specified text.
        /// </summary>
        public MenuItem(string text)
        {
            this.text = text;
        }


        #endregion


        #region Update and Draw


        /// <summary>
        /// Updates the menu entry.
        /// </summary>
        public virtual void Update(Menu screen, bool isSelected, GameTime gameTime)
        { }


        /// <summary>
        /// Draws the menu entry. This can be overridden to customize the appearance.
        /// </summary>
        public virtual void Draw(Menu screen, bool isSelected, GameTime gameTime)
        {
            // Draw the selected entry in yellow, otherwise white.
            Color color = isSelected ? Fonts.MenuSelectedColor : Fonts.TitleColor;
            color *= screen.TransitionAlpha;

            // Draw text, centered on the middle of each line.
            ScreenManager screenManager = screen.ScreenManager;
            SpriteBatch spriteBatch = screenManager.SpriteBatch;
            spriteFont = screenManager.Font;

            if (texture != null)
            {
                spriteBatch.Draw(texture, position, Color.White);
                if ((spriteFont != null) && !String.IsNullOrEmpty(text))
                {
                    Vector2 textSize = spriteFont.MeasureString(text);
                    Vector2 textPosition = position + new Vector2(
                        (float)Math.Floor((texture.Width - textSize.X) / 2),
                        (float)Math.Floor((texture.Height - textSize.Y) / 2));
                    spriteBatch.DrawString(spriteFont, text, textPosition, color);
                }
            }
            else if ((spriteFont != null) && !String.IsNullOrEmpty(text))
            {
                spriteBatch.DrawString(spriteFont, text, position, color);
            }
        }


        /// <summary>
        /// Queries how much space this menu entry requires.
        /// </summary>
        public virtual int GetHeight(Menu screen)
        {
            return Font.LineSpacing;
        }


        #endregion
    }
}
