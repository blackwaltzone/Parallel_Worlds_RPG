using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Engine
{
    public enum CollisionType
    {
        Empty = 0,
        Unwalkable,
        Portal,
        Spawn,
        Object,
        Slow,
        Fast,
    }

    public enum LayerType
    {
        Base,
        Object,
        Collision,
    }


    public class Map
    {
        #region Fields


        public const int TILE_WIDTH = 32;
        public const int TILE_HEIGHT = 32;
        int numLayers = 3;
        int[,] baseLayer;
        int[,] objectLayer;
        int[,] collisionLayer;
        Texture2D texture;
        Texture2D chestTexture;
        Texture2D chestOpenTexture;
        Texture2D switchTexture;
        Texture2D collisionTexture;
        Block block;
        Texture2D blockTexture;
        string name;
        int width, height;
        int pixelWidth, pixelHeight;
        Point min, max;


        #endregion


        #region Properties


        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }

        public Texture2D ChestTexture
        {
            get { return chestTexture; }
            set { chestTexture = value; }
        }

        public Texture2D ChestOpenTexture
        {
            get { return chestOpenTexture; }
            set { chestOpenTexture = value; }
        }

        public Texture2D SwitchTexture
        {
            get { return switchTexture; }
            set { switchTexture = value; }
        }

        public Texture2D CollisionTexture
        {
            get { return collisionTexture; }
            set { collisionTexture = value; }
        }

        public Texture2D BlockTexture
        {
            get { return blockTexture; }
            set { blockTexture = value; }
        }

        public int Width
        {
            get { return width; }
        }

        public int Height
        {
            get { return height; }
        }

        public int WidthInPixels
        {
            get { return pixelWidth; }
        }

        public int HeightInPixels
        {
            get { return pixelHeight; }
        }
        
        /// <summary>
        /// A valid spawn position for this map. 
        /// </summary>
        private Vector2 spawnMapPosition;

        /// <summary>
        /// A valid spawn position for this map. 
        /// </summary>
        public Vector2 SpawnMapPosition
        {
            get { return spawnMapPosition; }
            set { spawnMapPosition = value; }
        }


        #endregion


        #region Methods


        public int GetIndex(Vector2 cell)
        {
            if ((cell.X < 0) || (cell.X > width) ||
                (cell.Y < 0) || (cell.Y > height))
            {
                throw new ArgumentOutOfRangeException("Index out of bounds");
            }

            return collisionLayer[(int)cell.Y, (int)cell.X];
        }

        public int GetIndex(int x, int y)
        {
            if ((x < 0) || (x > width) ||
                (y < 0) || (y > height))
            {
                throw new ArgumentOutOfRangeException("Index out of bounds");
            }

            return collisionLayer[y, x];
        }

        public Vector2 PositionToCell(int x, int y)
        {
            return new Vector2(
                (int)(x / TILE_WIDTH),
                (int)(y / TILE_HEIGHT));
        }

        public Vector2 PositionToCell(Vector2 pos)
        {
            return new Vector2(
                (int)(pos.X / TILE_WIDTH),
                (int)(pos.Y / TILE_HEIGHT));
        }

        public void SetIndex(LayerType type, int x, int y, int index)
        {
            if (type == LayerType.Base)
                baseLayer[y, x] = index;
            else if (type == LayerType.Object)
                objectLayer[y, x] = index;
            else if (type == LayerType.Collision)
                collisionLayer[y, x] = index;
        }

        public void SetMap(int[,] layer, LayerType type)
        {
            if (type == LayerType.Base)
                baseLayer = layer;
            else if (type == LayerType.Object)
                objectLayer = layer;
            else if (type == LayerType.Collision)
                collisionLayer = layer;
        }


        #endregion


        #region Constructor / Initialization


        public Map()
        {
        }

        public Map(int w, int h)
        {
            setSize(w, h);
            SetEmptyMap(w, h);
        }

        public Map(int w, int h, Texture2D tex)
        {
            setSize(w, h);
            SetEmptyMap(w, h);
            texture = tex;
        }

        public Map(int[,] b, int[,] o, int[,] c, Texture2D tex)
        {
            baseLayer = b;
            objectLayer = o;
            collisionLayer = c;
            texture = tex;
            setSize(b.GetLength(1), b.GetLength(0));
            // getlength(0) = rows (y)
            // getlength(1) = cols (x)
        }

        private void setSize(int w, int h)
        {
            baseLayer = new int[h, w];
            objectLayer = new int[h, w];
            collisionLayer = new int[h, w];
            width = w;
            height = h;
            pixelWidth = w * TILE_WIDTH;
            pixelHeight = h * TILE_HEIGHT;
            min = new Point(0, 0);
            max = new Point(width, height);
        }

        private void SetEmptyMap(int w, int h)
        {
            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    baseLayer[y, x] = -1;
                    objectLayer[y, x] = -1;
                    collisionLayer[y, x] = -1;
                }
            }

        }

        public void Reset()
        {
            foreach (Chest c in Chests)
                c.Reset();

            foreach (Switch s in Switches)
                s.Reset();

            //baseLayer = null;
            //objectLayer = null;
            //collisionLayer = null;
            //collisionTexture = null;
            //chestTexture = null;
            //chestOpenTexture = null;
            //switchTexture = null;
            //texture = null;
            //name = null;
        }


        #endregion


        #region Portals


        private List<Portal> portals = new List<Portal>();

        /// <summary>
        /// Portals to other maps.
        /// </summary>
        public List<Portal> Portals
        {
            get { return portals; }
            set { portals = value; }
        }

        // Find
        public Portal GetPortal(String name)
        {
            foreach (Portal p in portals)
            {
                if (p.Name == name)
                    return p;
            }
            return null;
        }

        public Portal GetPortal(int id)
        {
            foreach (Portal p in portals)
            {
                if (p.ID == id)
                    return p;
            }
            return null;
        }

        public Portal GetPortal(Vector2 pos)
        {
            foreach (Portal p in portals)
            {
                if (p.Position == pos)
                    return p;
            }
            return null;
        }


        #endregion


        #region Chests


        private List<Chest> chests = new List<Chest>();
        public List<Chest> Chests
        {
            get { return chests; }
            set { chests = value; }
        }

        public Chest GetChest(string name)
        {
            foreach (Chest c in chests)
            {
                if (c.Name == name)
                    return c;
            }
            return null;
        }

        public Chest GetChest(Vector2 pos)
        {
            foreach (Chest c in chests)
            {
                if (c.Position == pos)
                    return c;
            }
            return null;
        }


        #endregion


        #region Switches


        private List<Switch> switches = new List<Switch>();
        public List<Switch> Switches
        {
            get { return switches; }
            set { switches = value; }
        }

        public Switch GetSwitch(string name)
        {
            foreach (Switch s in switches)
            {
                if (s.Name == name)
                    return s;
            }
            return null;
        }

        public Switch GetSwitch(Vector2 pos)
        {
            foreach (Switch s in switches)
            {
                if (s.Position == pos)
                    return s;
            }
            return null;
        }


        #endregion


        #region Blocks


        private List<Block> blocks = new List<Block>();
        public List<Block> Blocks
        {
            get { return blocks; }
            set { blocks = value; }
        }

        public Block GetBlock(Vector2 pos)
        {
            foreach (Block b in blocks)
            {
                if (b.Position == pos)
                    return b;
            }
            return null;
        }


        #endregion


        #region Draw


        public void Draw(SpriteBatch spriteBatch)
        {
            // ***************************************
            // need to figure out how not to hard code
            for (int i = 0; i < numLayers; i++)
            {
                if (i == 0)
                    DrawLayer(spriteBatch, baseLayer);
                else if (i == 1)
                {
                    DrawLayer(spriteBatch, objectLayer);
                    foreach (Chest c in chests)
                    {
                        if (c.IsOpen())
                            c.Draw(spriteBatch, chestOpenTexture);
                        else
                            c.Draw(spriteBatch, chestTexture);
                    }
                    foreach (Switch s in switches)
                        s.Draw(spriteBatch, switchTexture);
                    foreach (Block b in blocks)
                        b.Draw(spriteBatch, blockTexture);
                }
                else if (i == 2)
                {
                    //spriteBatch.Begin();
                    //// collision layer
                    //for (int y = 0; y < Height; y++)
                    //{
                    //    for (int x = 0; x < width; x++)
                    //    {
                    //        if (GetIndex(new Vector2(x, y)) == 1)
                    //            spriteBatch.Draw(collisionTexture, new Rectangle(x * 32, y * 32, 32, 32), Color.White);
                    //    }
                    //}
                    //spriteBatch.End();
                    continue;
                }
            }
        }

        public void DrawLayer(SpriteBatch spriteBatch, int[,] layer)
        {
            min.X = (int)Math.Max(min.X, 0);
            min.Y = (int)Math.Max(min.Y, 0);
            max.X = (int)Math.Min(max.X, width);
            max.Y = (int)Math.Min(max.Y, height);

            spriteBatch.Begin();

            // cycle through each cell
            for (int x = min.X; x < max.X; x++)
            {
                for (int y = min.Y; y < max.Y; y++)
                {
                    // get index
                    int index = layer[y, x];

                    // if index is -1, leave blank
                    if (index == -1)
                        continue;

                    // get tile on texturemap
                    int row = index / (texture.Width / TILE_WIDTH);
                    int col = index % (texture.Width / TILE_HEIGHT);

                    // draw! draw! draw!
                    spriteBatch.Draw(
                        texture,
                        new Rectangle(
                            x * TILE_WIDTH,
                            y * TILE_HEIGHT,
                            TILE_WIDTH,
                            TILE_HEIGHT),
                        new Rectangle(
                            col * TILE_WIDTH,
                            row * TILE_HEIGHT,
                            TILE_WIDTH,
                            TILE_HEIGHT),
                        new Color(new Vector4(1f, 1f, 1f, 0f)));
                }
            }

            spriteBatch.End();
        }

        #endregion
    }
}
