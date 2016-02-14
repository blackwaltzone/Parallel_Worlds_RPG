using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using System.Reflection;

namespace Engine
{
    /// <summary>
    /// This class will be instantiated by the XNA Framework Content
    /// Pipeline to read the specified data type from binary .xnb format.
    /// 
    /// Unlike the other Content Pipeline support classes, this should
    /// be a part of your main game project, and not the Content Pipeline
    /// Extension Library project.
    /// </summary>
    public class MapReader : ContentTypeReader<Map>
    {
        protected override Map Read(ContentReader input, Map existingInstance)
        {
            string mapName = input.ReadString();

            int height = input.ReadInt32();
            int width = input.ReadInt32();

            Map map = new Map(width, height);

            int[,] baseLayer = new int[height, width];
            int[,] objectLayer = new int[height, width];
            int[,] collisionLayer = new int[height, width];

            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    map.SetIndex(LayerType.Base, x, y, input.ReadInt32());

            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    map.SetIndex(LayerType.Object, x, y, input.ReadInt32());

            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    map.SetIndex(LayerType.Collision, x, y, input.ReadInt32());

            map.Name = mapName;
            
            Texture2D texture = null;
            Texture2D chestTexture = null;
            Texture2D chestOpenTexture = null;
            Texture2D switchTexture = null;
            
            TempTexture t = new TempTexture();
            t.Texture = input.ReadExternalReference<Texture2D>();
            texture = t.Texture;
            map.Texture = texture;

            TempTexture cT = new TempTexture();
            cT.Texture = input.ReadExternalReference<Texture2D>();
            chestTexture = cT.Texture;
            map.ChestTexture = chestTexture;

            TempTexture cOT = new TempTexture();
            cOT.Texture = input.ReadExternalReference<Texture2D>();
            chestOpenTexture = cOT.Texture;
            map.ChestOpenTexture = chestOpenTexture;

            TempTexture sT = new TempTexture();
            sT.Texture = input.ReadExternalReference<Texture2D>();
            switchTexture = sT.Texture;
            map.SwitchTexture = switchTexture;

            //map.TileSize = tileSize;

            // spawn position                     
            Vector2 spawnPosition = new Vector2(input.ReadInt32(), input.ReadInt32());
            map.SpawnMapPosition = spawnPosition;

            // portals
            int numPortals = input.ReadInt32();
            for (int i = 0; i < numPortals; i++)
            {
                int portalID = input.ReadInt32();
                Vector2 portalLocation = input.ReadVector2();
                string portalDestination = input.ReadString();
                int portalDestinationID = input.ReadInt32();
                Vector2 autoMovement = input.ReadVector2();

                Portal p = new Portal();
                p.ID = portalID;
                p.Position = new Vector2((int)portalLocation.X, (int)portalLocation.Y);
                p.DestinationMap = portalDestination;
                p.DestinationPortal = portalDestinationID;
                p.AutoMotion = autoMovement;

                map.Portals.Add(p);
            }

            // chests
            int numChests = input.ReadInt32();
            for (int i = 0; i < numChests; i++)
            {
                string chestName = input.ReadString();
                Vector2 chestLoc = input.ReadVector2();
                Vector2 chestSize = input.ReadVector2();
                int gold = input.ReadInt32();

                Chest c = new Chest();
                c.Name = chestName;
                c.Position = chestLoc;
                c.Size = chestSize;
                c.SetSpoils(gold);

                map.Chests.Add(c);
            }

            // switches
            int numSwitches = input.ReadInt32();
            for (int i = 0; i < numSwitches; i++)
            {
                string switchName = input.ReadString();
                Vector2 switchLoc = input.ReadVector2();
                Vector2 switchSize = input.ReadVector2();
                bool active = input.ReadBoolean();

                Switch s = new Switch();
                s.Name = switchName;
                s.Position = switchLoc;
                s.Size = switchSize;
                if (active)
                    s.Activate();

                map.Switches.Add(s);
            }

            return map;
        }
    }

    class TempTexture
    {
        public Texture2D Texture;
    }
}
