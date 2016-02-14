using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using System.Xml;
using System.IO;

namespace ContentPipeline
{
    /// <summary>
    /// This class will be instantiated by the XNA Framework Content Pipeline
    /// to apply custom processing to content data, converting an object of
    /// type TInput to TOutput. The input and output types may be the same if
    /// the processor wishes to alter data without changing its type.
    ///
    /// This should be part of a Content Pipeline Extension Library project.
    ///
    /// TODO: change the ContentProcessor attribute to specify the correct
    /// display name for this processor.
    /// </summary>
    [ContentProcessor(DisplayName = "MapProcessor")]
    public class MapProcessor : ContentProcessor<XmlDocument, MapContent>
    {
        public override MapContent Process(XmlDocument input, ContentProcessorContext context)
        {
            MapContent map = new MapContent();
            int width = 0, height = 0;
            MapLayerContent layer1 = new MapLayerContent();
            MapLayerContent layer2 = new MapLayerContent();
            MapLayerContent layer3 = new MapLayerContent();

            foreach (XmlNode node in input.DocumentElement.ChildNodes)
            {
                if (node.Name == "Name")
                {
                    map.Name = node.InnerText;
                }
                else if (node.Name == "Dimensions")
                {
                    string rawDim = node.InnerText;
                    string[] dim = rawDim.Split(',');
                    width = int.Parse(dim[0]);
                    height = int.Parse(dim[1]);
                }
                else if (node.Name == "Texture")
                {
                    string file = node.Attributes["File"].Value;

                    MapTextureContent textureContent = new MapTextureContent();

                    OpaqueDataDictionary data = new OpaqueDataDictionary();
                    data.Add("GenerateMipmaps", true);
                    textureContent.Texture = context.BuildAsset<TextureContent, TextureContent>(
                        new ExternalReference<TextureContent>(file),
                        "TextureProcessor",
                        data,
                        "TextureImporter",
                        file);

                    map.Texture = textureContent;
                }
                else if (node.Name == "ChestTexture")
                {
                    string file = node.Attributes["File"].Value;

                    MapTextureContent textureContent = new MapTextureContent();

                    OpaqueDataDictionary data = new OpaqueDataDictionary();
                    data.Add("GenerateMipmaps", true);
                    textureContent.Texture = context.BuildAsset<TextureContent, TextureContent>(
                            new ExternalReference<TextureContent>(file),
                            "TextureProcessor",
                            data,
                            "TextureImporter",
                            file);

                    map.ChestTexture = textureContent;
                }
                else if (node.Name == "ChestOpenTexture")
                {
                    string file = node.Attributes["File"].Value;

                    MapTextureContent textureContent = new MapTextureContent();

                    OpaqueDataDictionary data = new OpaqueDataDictionary();
                    data.Add("GenerateMipmaps", true);
                    textureContent.Texture = context.BuildAsset<TextureContent, TextureContent>(
                            new ExternalReference<TextureContent>(file),
                            "TextureProcessor",
                            data,
                            "TextureImporter",
                            file);

                    map.ChestOpenTexture = textureContent;
                }
                else if (node.Name == "SwitchTexture")
                {
                    string file = node.Attributes["File"].Value;

                    MapTextureContent textureContent = new MapTextureContent();

                    OpaqueDataDictionary data = new OpaqueDataDictionary();
                    data.Add("GenerateMipmaps", true);
                    textureContent.Texture = context.BuildAsset<TextureContent, TextureContent>(
                            new ExternalReference<TextureContent>(file),
                            "TextureProcessor",
                            data,
                            "TextureImporter",
                            file);

                    map.SwitchTexture = textureContent;
                }
                else if (node.Name == "Spawn")
                {
                    string rawPos = node.InnerText;
                    string[] pos = rawPos.Split(',');
                    map.SpawnPosition = new Vector2(
                        int.Parse(pos[0]),
                        int.Parse(pos[1]));
                }
                else if (node.Name == "Portals")
                {
                    foreach (XmlNode portalNode in node.ChildNodes)
                    {
                        int portalID = int.Parse(portalNode.Attributes["ID"].Value);
                        string portalDestination = portalNode.Attributes["Destination"].Value;
                        int portalDestinationID = int.Parse(portalNode.Attributes["DestinationID"].Value);

                        Vector2 portalLocation = new Vector2();
                        string loc = portalNode.Attributes["Location"].Value;
                        string[] location = loc.Split(',');
                        portalLocation.X = int.Parse(location[0]);
                        portalLocation.Y = int.Parse(location[1]);

                        Vector2 autoMovement = new Vector2();
                        string rawMovement = portalNode.Attributes["Motion"].Value;
                        string[] movement = rawMovement.Split(',');
                        autoMovement.X = int.Parse(movement[0]);
                        autoMovement.Y = int.Parse(movement[1]);

                        MapPortalContent p = new MapPortalContent();
                        p.ID = portalID;
                        p.Location = portalLocation;
                        p.DestinationMap = portalDestination;
                        p.DestinationID = portalDestinationID;
                        p.AutoMotion = autoMovement;

                        map.Portals.Add(p);
                    }
                }
                else if (node.Name == "Chests")
                {
                    foreach (XmlNode chestNode in node.ChildNodes)
                    {
                        string name = chestNode.Attributes["Name"].Value;
                        
                        Vector2 chestPos = new Vector2();
                        string rawPos = chestNode.Attributes["Location"].Value;
                        string[] pos = rawPos.Split(',');
                        chestPos.X = int.Parse(pos[0]);
                        chestPos.Y = int.Parse(pos[1]);

                        Vector2 chestSize = new Vector2();
                        string rawSize = chestNode.Attributes["Size"].Value;
                        string[] size = rawSize.Split(',');
                        chestSize.X = int.Parse(size[0]);
                        chestSize.Y = int.Parse(size[1]);

                        int gold = int.Parse(chestNode.Attributes["Gold"].Value);


                        MapChestContent c = new MapChestContent();
                        c.Name = name;
                        c.Position = chestPos;
                        c.Size = chestSize;
                        c.Gold = gold;

                        map.Chests.Add(c);
                    }
                }
                else if (node.Name == "Switches")
                {
                    foreach (XmlNode switchNode in node.ChildNodes)
                    {
                        string name = switchNode.Attributes["Name"].Value;

                        Vector2 switchPos = new Vector2();
                        string rawPos = switchNode.Attributes["Location"].Value;
                        string[] pos = rawPos.Split(',');
                        switchPos.X = int.Parse(pos[0]);
                        switchPos.Y = int.Parse(pos[1]);

                        Vector2 switchSize = new Vector2();
                        string rawSize = switchNode.Attributes["Size"].Value;
                        string[] size = rawSize.Split(',');
                        switchSize.X = int.Parse(size[0]);
                        switchSize.Y = int.Parse(size[1]);

                        int rotation = int.Parse(switchNode.Attributes["Rotation"].Value);

                        string rawActive = switchNode.Attributes["Active"].Value;
                        bool active = false;
                        if (rawActive == "True")
                            active = true;

                        MapSwitchContent s = new MapSwitchContent();
                        s.Name = name;
                        s.Position = switchPos;
                        s.Size = switchSize;
                        s.Activated = active;

                        map.Switches.Add(s);
                    }
                }
                else if (node.Name == "Base")
                {
                    layer1.Layer = new int[height, width];

                    string layout = node.InnerText;

                    string[] lines = layout.Split('\r', '\n');

                    int row = 0;

                    foreach (string line in lines)
                    {
                        string realLine = line.Trim();

                        if (string.IsNullOrEmpty(realLine))
                            continue;

                        string[] cells = realLine.Split(' ');

                        for (int x = 0; x < width; x++)
                        {
                            int cellIndex = int.Parse(cells[x]);

                            layer1.Layer[row, x] = cellIndex;
                        }

                        row++;
                    }

                    map.Layers.Add(layer1);
                }
                else if (node.Name == "Object")
                {
                    layer2.Layer = new int[height, width];

                    string obj = node.InnerText;

                    string[] lines = obj.Split('\r', '\n');

                    int row = 0;

                    foreach (string line in lines)
                    {
                        string realLine = line.Trim();

                        if (string.IsNullOrEmpty(realLine))
                            continue;

                        string[] cells = realLine.Split(' ');

                        for (int x = 0; x < width; x++)
                        {
                            int cellIndex = int.Parse(cells[x]);

                            layer2.Layer[row, x] = cellIndex;
                        }

                        row++;
                    }

                    map.Layers.Add(layer2);
                }
                else if (node.Name == "Collision")
                {
                    layer3.Layer = new int[height, width];

                    string collision = node.InnerText;

                    string[] lines = collision.Split('\r', '\n');

                    int row = 0;

                    foreach (string line in lines)
                    {
                        string realLine = line.Trim();

                        if (string.IsNullOrEmpty(realLine))
                            continue;

                        string[] cells = realLine.Split(' ');

                        for (int x = 0; x < width; x++)
                        {
                            int cellIndex = int.Parse(cells[x]);

                            layer3.Layer[row, x] = cellIndex;
                        }

                        row++;
                    }

                    map.Layers.Add(layer3);
                }
            }

            return map;
        }
    }
}