using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

using Engine;

namespace Parallel_Worlds
{
    public class MapEngine
    {
        #region Fields


        public Texture2D boxTexture;
        public bool setBoundingBoxes = true;
        public static Game MapGame;

        SpriteFont font;

        //static Camera camera;
        private static Vector2 prevPos = Vector2.Zero;

        private static Map map = null;
        private static Vector2 motion = Vector2.Zero;


        #endregion


        #region Properties


        /// <summary>
        /// Map being used by the tile engine
        /// </summary>
        public static Map Map
        {
            get { return map; }
        }


        #endregion


        #region Loading / Initializing


        public void Load(ContentManager content)
        {
            //MapGame = game;

            // load map
            //map = content.Load<Map>("Maps/Start");
            map.Texture = content.Load<Texture2D>("Textures/tileSet");
            font = content.Load<SpriteFont>("Arial");

            map.Load();
        }

        public void Initialize()
        {
            // TODO
            map = new Map(5, 5);
        }


        #endregion


        #region Change Map


        // TODO


        #endregion


        #region Collision


        // TODO


        #endregion


        #region Update


        public void Update(GameTime gameTime, int width, int height)
        {
            //InputManager.Update();
            motion = Vector2.Zero;

            // do input stuff

            // do motion stuff

            // clamp to area

            // get screen height/width in pixels

            // keep camera centered on player

            // keep camera from going past map bounds

            // add code to scroll camera
        }

        // get input


        #endregion


        #region Draw


        public void Draw(SpriteBatch spriteBatch)
        {
            map.Draw(spriteBatch);

            spriteBatch.Begin();
            spriteBatch.DrawString(font,
                "Texture: " + map.Texture.Name,
                new Vector2(20, 20),
                Color.White);
            spriteBatch.End();

            // bounding box stuff

            // animated sprites
            // npc stuff

            // debugging
        }


        #endregion
    }
}
