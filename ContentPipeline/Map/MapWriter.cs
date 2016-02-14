using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

// TODO: replace this with the type you want to write out.
using TWrite = System.String;

namespace ContentPipeline.Map
{
    /// <summary>
    /// This class will be instantiated by the XNA Framework Content Pipeline
    /// to write the specified data type into binary .xnb format.
    ///
    /// This should be part of a Content Pipeline Extension Library project.
    /// </summary>
    [ContentTypeWriter]
    public class MapWriter : ContentTypeWriter<MapContent>
    {
        protected override void Write(ContentWriter output, MapContent value)
        {
            // Map Name
            output.Write(value.Name);

            // dimensions
            output.Write(value.Layers[0].Layer.GetLength(0));
            output.Write(value.Layers[0].Layer.GetLength(1));

            // Map layers

            // base
            for (int y = 0; y < value.Layers[0].Layer.GetLength(0); y++)
                for (int x = 0; x < value.Layers[0].Layer.GetLength(1); x++)
                    output.Write(value.Layers[0].Layer[y, x]);

            // object
            for (int y = 0; y < value.Layers[1].Layer.GetLength(0); y++)
                for (int x = 0; x < value.Layers[1].Layer.GetLength(1); x++)
                    output.Write(value.Layers[1].Layer[y, x]);

            // collision
            for (int y = 0; y < value.Layers[2].Layer.GetLength(0); y++)
                for (int x = 0; x < value.Layers[2].Layer.GetLength(1); x++)
                    output.Write(value.Layers[2].Layer[y, x]);

            // map textures
            MapTextureContent mapTexture = value.Texture;
            output.WriteExternalReference<TextureContent>(mapTexture.Texture);

            // chest texture
            MapTextureContent chestTexture = value.ChestTexture;
            output.WriteExternalReference<TextureContent>(chestTexture.Texture);

            // chest open texture
            MapTextureContent chestOpenTexture = value.ChestOpenTexture;
            output.WriteExternalReference<TextureContent>(chestOpenTexture.Texture);

            // switch texture
            MapTextureContent switchTexture = value.SwitchTexture;
            output.WriteExternalReference<TextureContent>(switchTexture.Texture);

            // spawn position
            output.Write((int)value.SpawnPosition.X);
            output.Write((int)value.SpawnPosition.Y);

            // portals
            output.Write(value.Portals.Count);
            foreach (MapPortalContent portalContent in value.Portals)
            {
                output.Write(portalContent.ID);
                output.Write(portalContent.Location);
                output.Write(portalContent.DestinationMap);
                output.Write(portalContent.DestinationID);
                output.Write(portalContent.AutoMotion);
            }

            // chests
            output.Write(value.Chests.Count);
            foreach (MapChestContent chestContent in value.Chests)
            {
                output.Write(chestContent.Name);
                output.Write(chestContent.Position);
                output.Write(chestContent.Size);
                output.Write(chestContent.Gold);
            }

            // switches
            output.Write(value.Switches.Count);
            foreach (MapSwitchContent switchContent in value.Switches)
            {
                output.Write(switchContent.Name);
                output.Write(switchContent.Position);
                output.Write(switchContent.Size);
                output.Write(switchContent.Activated);
            }
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return "Engine.MapReader, Engine";
        }
    }
}
