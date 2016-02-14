using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace Engine
{
    /// <summary>
    /// A transition point from one map to another.  
    /// </summary>
    public class Portal// : MapObject
    {
        private int id;

        /// <summary>
        /// The ID of the portal.
        /// </summary>
        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        private Vector2 position;

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public string Name;

        private Vector2 autoMotion;

        /// <summary>
        /// Movement after exiting portal on new map
        /// </summary>
        public Vector2 AutoMotion
        {
            get { return autoMotion; }
            set { autoMotion = value; }
        }

        /// <summary>
        /// The content name of the map that the portal links to.
        /// </summary>
        private string destinationMap;

        /// <summary>
        /// The content name of the map that the portal links to.
        /// </summary>
        public string DestinationMap
        {
            get { return destinationMap; }
            set { destinationMap = value; }
        }


        /// <summary>
        /// The name of the portal that the party spawns at on the destination map.
        /// </summary>
        private int destinationPortal;

        /// <summary>
        /// The name of the portal that the party spawns at on the destination map.
        /// </summary>
        public int DestinationPortal
        {
            get { return destinationPortal; }
            set { destinationPortal = value; }
        }

    }
}
