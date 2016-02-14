using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Engine;

namespace Parallel_Worlds
{
    public static class World
    {
        #region Fields


        private static Player player;
        private static Vector2 motion = Vector2.Zero;
        private static Vector2 direction = Vector2.Zero;
        private static Vector2 prevPos = Vector2.Zero;
        private static Vector2 changePos = Vector2.Zero;

        private static float interactRadius = 40f;

        private static float alpha;
        private static bool fadeIn = true;
        private static bool fadeOut = false;
        private static Texture2D fadeTexture;

        static SpriteFont font;
        private static string screenMessage = "";
        private static bool inputHandling = false;

        private static Map map = null;
        private static Map changeMap = null;
        public static ContentManager MapContentManager;

        private static bool setBoundingBoxes = true;
        private static Texture2D boxTexture;
        static Vector2 min, max;

        #endregion


        #region Properties


        /// <summary>
        /// Map being used by the tile engine
        /// </summary>
        public static Map Map
        {
            get { return map; }
        }

        public static Player Player
        {
            get { return player; }
        }


        #endregion


        #region Load


        public static void Load(ContentManager cm)
        {
            if (map != null)
                map.Reset();

            MapContentManager = cm;
            map = MapContentManager.Load<Map>("Maps/Start");
            font = MapContentManager.Load<SpriteFont>("Arial");
            boxTexture = MapContentManager.Load<Texture2D>("Textures/tile");
            Map.CollisionTexture = MapContentManager.Load<Texture2D>("Textures/collision");
            Map.BlockTexture = MapContentManager.Load<Texture2D>("Textures/block");
            Block b = new Block();
            b.Position = new Vector2(5, 4);
            b.Size = new Vector2(32, 32);
            b.IsMoveable = true;
            map.Blocks.Add(b);
            map.SetIndex(LayerType.Collision, (int)b.Position.X, (int)b.Position.Y, 1);

            player = new Player();
            Player.Load(MapContentManager, "Textures/Player");
            Player.Position = new Vector2(64, 64);

            fadeIn = true;

            fadeTexture = MapContentManager.Load<Texture2D>("Textures/fade");
        }


        #endregion


        #region Changing Maps


        /// <summary>
        /// Set the map in use by the tile engine.
        /// </summary>
        /// <param name="map">The new map for the tile engine.</param>
        /// <param name="portal">The portal the party is entering on, if any.</param>
        public static void SetMap(Map newMap, Portal portalEntry)
        {
            // check the parameter
            if (newMap == null)
            {
                throw new ArgumentNullException("newMap");
            }

            // assign the new map
            changeMap = newMap;

            // move the party to its initial position
            if (portalEntry == null)
            {
                // no portal - use the spawn position                
                Player.Position = new Vector2(changeMap.SpawnMapPosition.X * Map.TILE_WIDTH,
                                            changeMap.SpawnMapPosition.Y * Map.TILE_HEIGHT);
            }
            else
            {
                // use the portal provided, which may include automatic movement
                changePos = new Vector2(portalEntry.Position.X * Map.TILE_WIDTH,
                                            portalEntry.Position.Y * Map.TILE_HEIGHT);

                Player.SetAutoMovement(new Vector2(portalEntry.AutoMotion.X * Map.TILE_WIDTH,
                    portalEntry.AutoMotion.Y * Map.TILE_HEIGHT));

                motion = Vector2.Zero;
            }
        }

        /// <summary>
        /// Change the current map, arriving at the given portal if any.
        /// </summary>
        /// <param name="contentName">The asset name of the new map.</param>
        /// <param name="originalPortal">The portal from the previous map.</param>
        public static void ChangeMap(string contentName, Portal originalPortal)
        {
            // make sure the content name is valid
            string mapContentName = contentName;
            if (!mapContentName.StartsWith(@"Maps\"))
            {
                mapContentName = Path.Combine(@"Maps", mapContentName);
            }

            // check for trivial movement - typically intra-map portals
            if ((Map != null) && (Map.Name == mapContentName))
            {
                SetMap(Map, originalPortal == null ? null :
                    Map.GetPortal(originalPortal.DestinationMap));
            }

            // load the map
            changeMap = MapContentManager.Load<Map>(mapContentName);

            // modify the map based on the world changes (removed chests, etc.).
            //ModifyMap(map);

            // start playing the music for the new map
            //AudioManager.PlayMusic(map.MusicCueName);

            // set the new map into the tile engine
            SetMap(changeMap, originalPortal == null ? null :
                changeMap.GetPortal(originalPortal.DestinationPortal));
        }


        #endregion


        #region Portals


        private static bool EncounterPortal(Vector2 position)
        {
            // look for portals from the map
            Portal portalEntry = Map.GetPortal(position);

            if (portalEntry != null)
            {
                Vector2 portalDir = -portalEntry.AutoMotion;
                if (GetDirection() == portalDir)
                {
                    HandlePortal(portalEntry);
                    return true;
                }
            }
            return false;
        }

        private static void HandlePortal(Portal portalEntry)
        {
            fadeOut = true;
            fadeIn = false;

            // change to the new map
            ChangeMap(portalEntry.DestinationMap,
                portalEntry);

            Portal landingPortal = changeMap.GetPortal(portalEntry.DestinationPortal);

            changePos = new Vector2(landingPortal.Position.X * Map.TILE_WIDTH,
                landingPortal.Position.Y * Map.TILE_HEIGHT);
        }


        #endregion


        #region Handle Chests/Switches/Blocks


        private static void HandleChest(Chest c)
        {
            if (!c.IsOpen())
            {
                Player.Gold = c.Open();
            }
        }

        private static void HandleSwitch(Switch s)
        {
            s.Activate();
        }

        private static void HandleBlock(Block b)
        {
            if (b.IsMoveable)
            {
                Vector2 newPos = b.Position + GetDirection();
                if (!OutOfBounds(newPos))
                {
                    if (map.GetIndex(newPos) != 1)
                    {
                        b.Push(GetDirection());
                        map.SetIndex(LayerType.Collision, (int)b.Position.X, (int)b.Position.Y, 0);
                        map.SetIndex(LayerType.Collision, (int)(b.Position.X + GetDirection().X), (int)(b.Position.Y + GetDirection().Y), 1);
                    }
                }
            }
        }

        private static bool OutOfBounds(Vector2 pos)
        {
            if (pos.X >= map.Width ||
                pos.X < 0 ||
                pos.Y >= map.Height ||
                pos.Y < 0)
                return true;

            return false;
        }


        #endregion


        #region Collision


        private static void Move(GameTime gameTime)
        {
            Vector2 spriteCell = Map.PositionToCell(Player.Center);
            Rectangle bounds = new Rectangle(
                (int)(Player.Position.X + 2),
                (int)(Player.Position.Y + 2),
                Map.TILE_WIDTH - 4,
                Map.TILE_HEIGHT - 4);

            min = new Vector2(spriteCell.X - 1, spriteCell.Y - 1);
            max = new Vector2(spriteCell.X + 1, spriteCell.Y + 1);

            if (max.X > Map.Width - 1)
                max.X = Map.Width - 1;
            if (max.Y > Map.Height - 1)
                max.Y = Map.Height - 1;
            if (min.X < 0)
                min.X = 0;
            if (min.Y < 0)
                min.Y = 0;

            if (BlockCollision(min, max, bounds))
                Player.Position = prevPos;
        }

        private static bool BlockCollision(Vector2 min, Vector2 max, Rectangle bounds)
        {
            for (int y = (int)min.Y; y <= (int)max.Y; y++)
            {
                for (int x = (int)min.X; x <= (int)max.X; x++)
                {
                    Rectangle rect = new Rectangle(
                        x * Map.TILE_WIDTH, 
                        y * Map.TILE_HEIGHT, 
                        Map.TILE_WIDTH,
                        Map.TILE_HEIGHT);
                    if (map.GetIndex(new Vector2(x, y)) == 1)
                    {
                        if (bounds.Intersects(rect))
                            return true;
                    }
                }
            }
            return false;
        }


        #endregion


        #region Update


        public static void Update(GameTime gameTime)
        {
            if (!fadeIn && !fadeOut)
            {
                HandleInput();
                inputHandling = true;
            }
            else
                inputHandling = false;

            if (fadeIn)
            {
                FadeIn();
                if (map != null &&
                    changeMap != null &&
                    map != changeMap)
                {
                    map = changeMap;
                    Player.Position = changePos;
                }
                UpdateMap(gameTime);
            }
            else if (fadeOut)
            {
                FadeOut();
            }
            else if (!fadeOut && !fadeIn)
                UpdateMap(gameTime);
        }

        private static void FadeIn()
        {
            alpha -= 0.025f;
            if (alpha <= 0.0f)
            {
                alpha = 0f;
                fadeIn = false;
                fadeOut = false;
            }
        }

        private static void FadeOut()
        {
            alpha += 0.025f;
            if (alpha >= 1.0f)
            {
                alpha = 1f;
                fadeIn = true;
                fadeOut = false;
            }
        }

        private static void UpdateMap(GameTime gameTime)
        {
            // pass w, h as entire screen in pixels

            if (Player.GetAutoMovement() == Vector2.Zero)
            {
                // normalize motion vector and move sprite position
                if (motion != Vector2.Zero)
                {
                    // normalize
                    motion.Normalize();
                    direction = motion;

                    // update player sprite
                    prevPos = Player.Position;
                    Player.Update(gameTime, motion);

                    Move(gameTime);

                    EncounterPortal(Map.PositionToCell(Player.Center));
                }
            }
            else if (Player.GetAutoMovement() != Vector2.Zero)
            {
                Player.Update(gameTime, Vector2.Zero);
            }

            // get screen height/width in pixels
            int width = map.WidthInPixels;
            int height = map.HeightInPixels;

            // clamp player to map bounds
            Player.Clamp(width, height);

            // keep camera centered on player

            // keep camera from going past map bounds

            // add code to scroll camera

            // move blocks
            foreach (Block b in map.Blocks)
                b.Update(gameTime);
        }

        private static void UpdateChests()
        {
            foreach (Chest c in Map.Chests)
            {
                if (InRange(Player.Position, c.Position * 32, interactRadius))
                {
                    if (IsFacing(c.Position))
                        HandleChest(c);
                }
            }
        }

        private static void UpdateSwitches()
        {
            foreach (Switch s in Map.Switches)
            {
                if (InRange(Player.Position, s.Position * 32, interactRadius))
                {
                    if (IsFacing(s.Position))
                        HandleSwitch(s);
                }
            }
        }

        private static void UpdateBlocks()
        {
            foreach (Block b in Map.Blocks)
            {
                if (InRange(Player.Position, b.Position * 32, interactRadius))
                {
                    if (IsFacing(b.Position))
                        HandleBlock(b);
                }
            }
        }

        private static Vector2 GetDirection()
        {
            return direction;
        }

        private static Vector2 GetDistance(Vector2 pos1, Vector2 pos2)
        {
            return (pos1 - pos2);
        }

        private static bool IsFacing(Vector2 pos)
        {
            Vector2 dir = (Player.Position - (pos * 32));
            if (Math.Abs(dir.X) > Math.Abs(dir.Y))
            {
                if (dir.X > 0)
                    dir = -Vector2.UnitX;
                else
                    dir = Vector2.UnitX;
            }
            else
            {
                if (dir.Y > 0)
                    dir = -Vector2.UnitY;
                else
                    dir = Vector2.UnitY;
            }

            if (GetDirection() == dir)
                return true;

            return false;
        }

        public static bool InRange(Vector2 pos1, Vector2 pos2, float radius)
        {
            Vector2 distance = GetDistance(pos1, pos2);
            return (distance.Length() < radius);
        }


        #endregion


        #region Draw


        public static void Draw(SpriteBatch spriteBatch)
        {
            Map.Draw(spriteBatch);

            if (setBoundingBoxes)
            {
                spriteBatch.Begin();
                for (int y = 0; y < map.Height; y++)
                {
                    for (int x = 0; x < map.Width; x++)
                    {
                        if (Player.Rectangle.Intersects(new Rectangle(x*32, y*32, 32, 32)))
                        {
                            spriteBatch.Draw(
                                boxTexture,
                                new Rectangle(
                                    (int)x * 32,
                                    (int)y * 32,
                                    32,
                                    32),
                                Color.Red);
                        }
                    }
                }
                spriteBatch.End();
            }

            Player.Draw(spriteBatch);

            spriteBatch.Begin();

            spriteBatch.Draw(fadeTexture,
                new Rectangle(0,
                    0,
                    Map.WidthInPixels,
                    Map.HeightInPixels),
                    Color.White * alpha);

            spriteBatch.DrawString(font, "Gold: " + Player.Gold, new Vector2(500, 120), Color.Yellow);

            foreach (Block b in map.Blocks)
            {
                spriteBatch.DrawString(font, "Block moveable: " + b.IsMoveable, new Vector2(500, 150), Color.Red);
                spriteBatch.DrawString(font, "Block moving: " + b.IsMoving, new Vector2(500, 170), Color.Red);
            }

            spriteBatch.End();
        }


        #endregion


        #region Input


        public static void HandleInput()
        {
            motion = Vector2.Zero;

            if (InputManager.IsKeyTriggered(Keys.Space))
            {
                // update NPC

                // update chests
                UpdateChests();

                // update switches
                UpdateSwitches();

                // update blocks
                UpdateBlocks();
            }
            else if (InputManager.IsKeyTriggered(Keys.D))
            {
                const string message =
                    "Hello there! This is a dialog box. Wheee!";
                DialogBox dialog = new DialogBox(message, null);
                dialog.Accepted += DialogAccepted;
                ScreenManager.Push(dialog);
            }

            if (InputManager.IsKeyPressed(Keys.Up))
            {
                motion.Y--;
            }
            if (InputManager.IsKeyPressed(Keys.Down))
            {
                motion.Y++;
            }
            if (InputManager.IsKeyPressed(Keys.Left))
            {
                motion.X--;
            }
            if (InputManager.IsKeyPressed(Keys.Right))
            {
                motion.X++;
            }
        }

        static void DialogAccepted(object sender, EventArgs e)
        {
            //string message = "You successfully changed the screen message through a dialog. Yay!";
            //ExitScreen();
        }


        #endregion
    }
}
