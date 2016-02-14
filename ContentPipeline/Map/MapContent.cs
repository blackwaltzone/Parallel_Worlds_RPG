using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework;


namespace ContentPipeline
{
    public class MapContent
    {
        public string Name;

        public MapTextureContent Texture =
            new MapTextureContent();

        public MapTextureContent ChestTexture =
            new MapTextureContent();

        public MapTextureContent ChestOpenTexture =
            new MapTextureContent();

        public MapTextureContent SwitchTexture =
            new MapTextureContent();

        public Vector2 SpawnPosition = new Vector2();

        public Collection<MapLayerContent> Layers =
            new Collection<MapLayerContent>();

        public Collection<MapPortalContent> Portals =
            new Collection<MapPortalContent>();

        public Collection<MapChestContent> Chests =
            new Collection<MapChestContent>();

        public Collection<MapSwitchContent> Switches =
            new Collection<MapSwitchContent>();
    }

    public class MapTextureContent
    {
        public ExternalReference<TextureContent> Texture;
    }

    public class MapLayerContent
    {
        public int[,] Layer;
    }

    public class MapPortalContent
    {
        public int ID;
        public Vector2 Location;
        public string DestinationMap;
        public int DestinationID;
        public Vector2 AutoMotion;
    }

    public class MapChestContent
    {
        public string Name;
        public int Gold;
        public Vector2 Position;
        public Vector2 Size;
    }

    public class MapSwitchContent
    {
        public string Name;
        public Vector2 Position;
        public Vector2 Size;
        public bool Activated;
    }
}