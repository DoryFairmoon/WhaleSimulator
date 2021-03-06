﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Linq;
using System.Collections;
using System.Threading;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using RB_GameResources.Xna.Controls;


namespace WhaleSimulator
{
    /// <summary>
    /// A data structure that stores map "chunks" and allows the traversing through them, while loading and unloading as needed.
    /// </summary>
    public class ChunkGrid : IEnumerable
    {
        private Chunk spawnChunk;
        private Chunk currentChunk;
        private Chunk rootChunk;

        private ContentManager mapContent;

        //private List<Graphics3D> globalTerrain;

        private Vector3 mapSize;
        private Vector3 playerChunkPosition;

        private bool initialized = false;
        //private bool loadingChunk = false;
        //private bool unloadingChunk = false;

        public Vector3 MapSize { get { return mapSize; } }
        public Chunk SpawnChunk { get { return spawnChunk; } set { spawnChunk = value; } }
        public Vector3 PlayerSpawn { get; set; }
        public Vector3 SpawnDirection { get; set; }
        public string PlayerSpecies { get; set; }
        public int WaterLevel { get; set; }
        public Chunk Root { get { return rootChunk; } set { rootChunk = value; } }

        public static Vector3 MapCenter { get; set; }
        public static List<Chunk> LoadedChunks { get; set; }
        public static Chunk CurrentChunk { get; set; }
        //public static List<Graphics3D> GlobalTerrain { get; set; }

        /// <summary>
        /// Creates a new ChunkGrid by loading data from the designated filepath.
        /// </summary>
        /// <param name="filepath">The path to the XML data file.</param>
        public ChunkGrid(string filepath, ContentManager content)
        {
            mapContent = content;
            LoadFromXML(filepath);
            MapCenter = new Vector3((mapSize.X * 1000) / 2, (mapSize.Y * 1000) / 2, (mapSize.Z * 1000) / 2);
            Map.WaterLevel = WaterLevel;
            LoadedChunks = new List<Chunk>();
            playerChunkPosition = new Vector3(currentChunk.Position.X, currentChunk.Position.Y, currentChunk.Position.Z);
        }

        public void LoadAssets()
        {
            LoadAssets(currentChunk);
            List<Chunk> loadedChunks = new List<Chunk>();
            foreach (Chunk chunk in this)
            {
                if (chunk.IsLoaded)
                    loadedChunks.Add(chunk);
            }
            LoadedChunks = loadedChunks;
        }

        public void LoadMap()
        {
            foreach (Chunk chunk in this)
            {
                chunk.LoadAssets();
                LoadedChunks.Add(chunk);
            }
        }

        public void LoadAssets(Vector3 centerChunk)
        {
            LoadAssets(this[(int)centerChunk.X, (int)centerChunk.Y, (int)centerChunk.Z]);
        }

        public void LoadAssets(Chunk centerChunk)
        {
            if (centerChunk != null)
            {
                if (!centerChunk.IsLoaded)
                    centerChunk.LoadAssets();
                if (centerChunk.North != null)
                {
                    if (!centerChunk.North.IsLoaded)
                        centerChunk.North.LoadAssets();
                    if (centerChunk.North.East != null)
                    {
                        if (!centerChunk.North.East.IsLoaded)
                            centerChunk.North.East.LoadAssets();
                    }
                    if (centerChunk.North.West != null)
                    {
                        if (!centerChunk.North.West.IsLoaded)
                            centerChunk.North.West.LoadAssets();
                    }
                }
                if (centerChunk.South != null)
                {
                    if (!centerChunk.South.IsLoaded)
                        centerChunk.South.LoadAssets();
                    if (centerChunk.South.East != null)
                    {
                        if (!centerChunk.South.East.IsLoaded)
                            centerChunk.South.East.LoadAssets();
                    }
                    if (centerChunk.South.West != null)
                    {
                        if (!centerChunk.South.West.IsLoaded)
                            centerChunk.South.West.LoadAssets();
                    }
                }

                if (centerChunk.East != null)
                {
                    if (!centerChunk.East.IsLoaded)
                    centerChunk.East.LoadAssets();
                }
                if (centerChunk.West != null)
                {
                    if (!centerChunk.West.IsLoaded)
                    centerChunk.West.LoadAssets();
                }

                if (centerChunk.Up != null)
                {
                    if (!centerChunk.Up.IsLoaded)
                        centerChunk.Up.LoadAssets();
                    if (centerChunk.Up.North != null)
                    {
                        if (!centerChunk.Up.North.IsLoaded)
                            centerChunk.Up.North.LoadAssets();
                        if (centerChunk.Up.North.East != null)
                        {
                            if (!centerChunk.Up.North.East.IsLoaded)
                                centerChunk.Up.North.East.LoadAssets();
                        }
                        if (centerChunk.Up.North.West != null)
                        {
                            if (!centerChunk.Up.North.West.IsLoaded)
                                centerChunk.Up.North.West.LoadAssets();
                        }
                    }
                    if (centerChunk.Up.South != null)
                    {
                        if (!centerChunk.Up.South.IsLoaded)
                            centerChunk.Up.South.LoadAssets();
                        if (centerChunk.Up.South.East != null)
                        {
                            if (!centerChunk.Up.South.East.IsLoaded)
                                centerChunk.Up.South.East.LoadAssets();
                        }
                        if (centerChunk.Up.South.West != null)
                        {
                            if (!centerChunk.Up.South.West.IsLoaded)
                                centerChunk.Up.South.West.LoadAssets();
                        }
                    }

                    if (centerChunk.Up.East != null)
                    {
                        if (!centerChunk.Up.East.IsLoaded)
                            centerChunk.Up.East.LoadAssets();
                    }
                    if (centerChunk.Up.West != null)
                    {
                        if (!centerChunk.Up.West.IsLoaded)
                            centerChunk.Up.West.LoadAssets();
                    }
                }

                if (centerChunk.Down != null)
                {
                    if (!centerChunk.Down.IsLoaded)
                        centerChunk.Down.LoadAssets();
                    if (centerChunk.Down.North != null)
                    {
                        if (!centerChunk.Down.North.IsLoaded)
                            centerChunk.Down.North.LoadAssets();
                        if (centerChunk.Down.North.East != null)
                        {
                            if (!centerChunk.Down.North.East.IsLoaded)
                                centerChunk.Down.North.East.LoadAssets();
                        }
                        if (centerChunk.Down.North.West != null)
                        {
                            if (!centerChunk.Down.North.West.IsLoaded)
                                centerChunk.Down.North.West.LoadAssets();
                        }
                    }
                    if (centerChunk.Down.South != null)
                    {
                        if (!centerChunk.Down.South.IsLoaded)
                            centerChunk.Down.South.LoadAssets();
                        if (centerChunk.Down.South.East != null)
                        {
                            if (!centerChunk.Down.South.East.IsLoaded)
                                centerChunk.Down.South.East.LoadAssets();
                        }
                        if (centerChunk.Down.South.West != null)
                        {
                            if (!centerChunk.Down.South.West.IsLoaded)
                                centerChunk.Down.South.West.LoadAssets();
                        }
                    }

                    if (centerChunk.Down.East != null)
                    {
                        if (!centerChunk.Down.East.IsLoaded)
                            centerChunk.Down.East.LoadAssets();
                    }
                    if (centerChunk.Down.West != null)
                    {
                        if (!centerChunk.Down.West.IsLoaded)
                            centerChunk.Down.West.LoadAssets();
                    }
                }
            }

            List<Chunk> loadedChunks = new List<Chunk>();
            foreach (Chunk chunk in this)
            {
                if (chunk.IsLoaded)
                    loadedChunks.Add(chunk);
            }
            LoadedChunks = loadedChunks;
        }

        /// <summary>
        /// Opens an XML data file from the filepath and creates the ChunkGrid full of Chunks.
        /// </summary>
        /// <param name="filepath">The path to the XML data file.</param>
        private void LoadFromXML(string filepath)
        {
            XDocument doc;
            XElement element;

            try
            {
                using (Stream stream = TitleContainer.OpenStream(filepath))
                {
                    doc = XDocument.Load(stream);
                    stream.Close();
                }

                element = doc.Root.Element("PlayerSpecies");
                if (element != null)
                    PlayerSpecies = element.Value;
                else
                    throw new Exception("Player Species not found in XML Data.");
                
                element = doc.Root.Element("Size");
                if (element != null)
                    mapSize = Utilities.Parse(element.Value);
                else
                    throw new Exception("Size not found in XML Data.");


                WaterLevel = int.Parse(doc.Root.Element("WaterLevel").Value);

                //IEnumerable<XElement> elements = doc.Root.Elements("GlobalTerrain").Elements("Terrain");
                //globalTerrain = new List<Graphics3D>();

                //foreach (XElement e in elements)
                //{
                //    globalTerrain.Add(new Graphics3D(mapContent.Load<Model>("Terrain/" + e.Element("Name").Value)));
                //}

                PlayerSpawn = Utilities.Parse(doc.Root.Element("PlayerSpawn").Value);

                SpawnDirection = Utilities.Parse(doc.Root.Element("SpawnDirection").Value);

                Vector3 SpawnChunkPosition = Utilities.Parse(doc.Root.Element("SpawnChunk").Value);

                IEnumerable<XElement> elements = doc.Root.Elements("ChunkGrid").Elements("Chunk");

                
                /*
                foreach (XElement e in elements)
                {
                    chunkList.Add(Chunk.FromXML(e));
                }
                 */

                List<Chunk> chunkList = new List<Chunk>();
                
                for (int i = 0; i < mapSize.X; i++)
                {
                    for (int j = 0; j < mapSize.Y; j++)
                    {
                        for (int k = 0; k < mapSize.Z; k++)
                        {
                            Chunk chunk = new Chunk(new Vector3(i, j, k));
                            chunk.CreatureList = new List<CreatureInfo>();
                            chunk.TerrainList = new List<CreatureInfo>();
                            chunk.Creatures = new List<Creature>();
                            chunk.Terrain = new List<Creature>();
                            chunkList.Add(chunk);
                        }
                    }
                }

                if (chunkList.Count == 0)
                    throw new Exception("No chunks found in Map data.");

                foreach (Chunk chunk in chunkList)
                {
                    foreach (Chunk c in chunkList)
                    {
                        if ((c.Position.X == chunk.Position.X - 1) && (c.Position.Y == chunk.Position.Y) && (c.Position.Z == chunk.Position.Z))
                            chunk.West = c;
                        if ((c.Position.X == chunk.Position.X + 1) && (c.Position.Y == chunk.Position.Y) && (c.Position.Z == chunk.Position.Z))
                            chunk.East = c;

                        if ((c.Position.X == chunk.Position.X) && (c.Position.Y == chunk.Position.Y) && (c.Position.Z == chunk.Position.Z - 1))
                            chunk.North = c;
                        if ((c.Position.X == chunk.Position.X) && (c.Position.Y == chunk.Position.Y) && (c.Position.Z == chunk.Position.Z + 1))
                            chunk.South = c;

                        if ((c.Position.X == chunk.Position.X) && (c.Position.Y == chunk.Position.Y - 1) && (c.Position.Z == chunk.Position.Z))
                            chunk.Down = c;
                        if ((c.Position.X == chunk.Position.X) && (c.Position.Y == chunk.Position.Y + 1) && (c.Position.Z == chunk.Position.Z))
                            chunk.Up = c;


                        if (chunk.Position.X == 0 && c.Position.X == mapSize.X - 1 && c.Position.Y == chunk.Position.Y && c.Position.Z == chunk.Position.Z)
                        {
                            chunk.West = c;
                            c.East = chunk;
                        }
                        
                        if (chunk.Position.Z == 0 && c.Position.X == chunk.Position.X && c.Position.Y == chunk.Position.Y && c.Position.Z == mapSize.Z - 1)
                        {
                            chunk.North = c;
                            c.South = chunk;
                        }

                    }
                }

                rootChunk = chunkList[0];
                SpawnChunk = this[(int)SpawnChunkPosition.X, (int)SpawnChunkPosition.Y, (int)SpawnChunkPosition.Z];
                currentChunk = spawnChunk;

                element = doc.Root.Element("CreatureList");
                //List<CreatureInfo> creatureList = new List<CreatureInfo>();
                if (element != null)
                {
                    elements = element.Elements("Creature");
                    foreach (XElement e in elements)
                    {
                        string species = e.Element("Species").Value;
                        string family = e.Element("Family").Value;
                        Vector3 spawn = Utilities.Parse(e.Element("Position").Value);
                        spawn.X += mapSize.X * 500;
                        spawn.Z += mapSize.Z * 500;

                        Vector3 direction = Utilities.Parse(e.Element("Direction").Value);

                        if (direction.X == -99)
                            direction.X = (float)Map.Randomizer.NextDouble() * 2 - 1;
                        if (direction.Y == -99)
                            direction.Y = (float)Map.Randomizer.NextDouble() * 2 - 1;
                        if (direction.Z == -99)
                            direction.Z = (float)Map.Randomizer.NextDouble() * 2 - 1;

                        bool swims;
                        element = e.Element("Swims");
                        if (element != null)
                            swims = bool.Parse(element.Value);
                        else
                            swims = false;

                        Chunk chunk = this[(int)Math.Floor(spawn.X/1000), 0, (int)Math.Floor(spawn.Z/1000)];
                        if (family == "Terrain")
                            chunk.TerrainList.Add(new CreatureInfo(species, family, spawn, direction, true, swims));
                        else
                            chunk.CreatureList.Add(new CreatureInfo(species, family, spawn, direction, true, swims));
                    }
                }


            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }

            initialized = true;
        }

        /// <summary>
        /// Returns the Chunk at the given Grid coordinates.
        /// </summary>
        /// <param name="x">The X-Coordinate of the Chunk to get.</param>
        /// <param name="y">The Y-Coordinate of the Chunk to get.</param>
        /// <param name="z">The Z-Coordinate of the Chunk to get.</param>
        /// <returns>The Chunk at the given Grid coordinates.</returns>
        public Chunk this[int x, int y, int z]
        {
            get
            {
                Chunk temp = rootChunk;

                for (int i = 0; i < x; i++)
                {
                    if (temp == null)
                        return null;
                    else
                        temp = temp.East;
                }

                for (int i = 0; i < y; i++)
                {
                        
                    if (temp == null)
                        return null;
                    else
                        temp = temp.Up;
                }

                for (int i = 0; i < z; i++)
                {
                    
                    if (temp == null)
                        return null;
                    else
                        temp = temp.South;
                }

                return temp;
            }
            //set { }
        }

        /// <summary>
        /// Updates each of the Chunks in the ChunkGrid.
        /// </summary>
        /// <param name="gameTime">The GameTime object to use as reference.</param>
        /// <param name="inputStates">The InputStates object to use when checking player input.</param>
        public void Update(GameTime gameTime, InputStates inputStates, ref Player player)
        {
            if (initialized)
            {
                Vector3 playerPosition = player.Position;

                if (playerPosition.X < Map.MapSize.X * 500 - 500)
                {
                    playerPosition.X += 1000;
                    foreach (Chunk chunk in this)
                    {
                        chunk.ShiftAssets(Chunk.Directions.East, 1000);
                        chunk.ShiftPosition(Chunk.Directions.East);
                    }
                    rootChunk = rootChunk.West;
                    Chunk movingChunk = this[0, 0, 0];
                    for (int i = 0; i < Map.MapSize.Z; i++)
                    {
                        movingChunk.ShiftAssets(Chunk.Directions.West, (int)Map.MapSize.X * 1000);
                        movingChunk = movingChunk.South;
                    }
                }
                else if (playerPosition.X > Map.MapSize.X * 500 + 500)
                {
                    playerPosition.X -= 1000;
                    foreach (Chunk chunk in this)
                    {
                        chunk.ShiftAssets(Chunk.Directions.West, 1000);
                        chunk.ShiftPosition(Chunk.Directions.West);
                    }
                    rootChunk = rootChunk.East;
                    Chunk movingChunk = this[(int)Map.MapSize.X - 1, 0, 0];
                    
                    for (int i = 0; i < Map.MapSize.Z; i++)
                    {
                        movingChunk.ShiftAssets(Chunk.Directions.East, (int)Map.MapSize.X * 1000);
                        movingChunk = movingChunk.South;
                    }
                    
                }

                if (playerPosition.Z < Map.MapSize.Z * 500 - 500)
                {
                    playerPosition.Z += 1000;
                    foreach (Chunk chunk in this)
                    {
                        chunk.ShiftAssets(Chunk.Directions.South, 1000);
                        chunk.ShiftPosition(Chunk.Directions.South);
                    }
                    rootChunk = rootChunk.North;
                    Chunk movingChunk = this[0, 0, 0];
                    for (int i = 0; i < Map.MapSize.X; i++)
                    {
                        movingChunk.ShiftAssets(Chunk.Directions.North, (int)Map.MapSize.Z * 1000);
                        movingChunk = movingChunk.East;
                    }
                }
                else if (playerPosition.Z > Map.MapSize.Z * 500 + 500)
                {
                    int k = 0;
                    playerPosition.Z -= 1000;
                    foreach (Chunk chunk in this)
                    {
                        chunk.ShiftAssets(Chunk.Directions.North, 1000);
                        chunk.ShiftPosition(Chunk.Directions.North);
                        k++;
                    }
                    rootChunk = rootChunk.South;
                    Chunk movingChunk = this[0, 0, (int)Map.MapSize.Z - 1];

                    for (int i = 0; i < Map.MapSize.X; i++)
                    {
                        movingChunk.ShiftAssets(Chunk.Directions.South, (int)Map.MapSize.Z * 1000);
                        movingChunk = movingChunk.East;
                    }

                }

                currentChunk = this[2, 0, 2];
                CurrentChunk = currentChunk;
                player.Position = playerPosition;
                
                foreach (Chunk chunk in this)
                {
                    chunk.Update(gameTime, inputStates);
                }

                //foreach (Graphics3D g in globalTerrain)
                //{
                //    g.Update(gameTime);
                //}
            }
        }

        /// <summary>
        /// Draws any 3D objects to the screen (3D objects are always drawn behind 2D sprites).
        /// </summary>
        /// <param name="gameTime">The GameTime object to use as reference.</param>
        public void Draw3D(GameTime gameTime)
        {
            if (initialized)
            {
                foreach (Chunk chunk in this)
                {
                    chunk.Draw3D(gameTime);
                }

                //foreach (Graphics3D g in globalTerrain)
                //{
                //    g.Draw3D(gameTime);
                //}
            }
        }

        /// <summary>
        /// Gets an Enumerator to enumerate through the Chunks in the grid.
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)GetEnumerator();
        }

        /// <summary>
        /// Gets an Enumerator to enumerate through the Chunks in the grid.
        /// </summary>
        /// <returns></returns>
        public ChunkGridEnumerator GetEnumerator()
        {
            return new ChunkGridEnumerator(this, mapSize);
        }
    }

    /// <summary>
    /// An Enumerator to be used with a ChunkGrid that enumerates through the grid.
    /// </summary>
    public class ChunkGridEnumerator : IEnumerator
    {
        private Chunk chunk = null;
        private ChunkGrid grid; //bottom tier, top-left
        private Vector3 mapSize;

        int i = 0;
        int k = 0;
        int j = 0;

        /// <summary>
        /// Creates a new Enumerator.
        /// </summary>
        /// <param name="root">The root Chunk in the grid.</param>
        public ChunkGridEnumerator(ChunkGrid grid, Vector3 size)
        {
            this.grid = grid;
            this.mapSize = size;
        }

        public bool MoveNext()
        {
            if (chunk == null)
            {
                chunk = grid.Root;
                return true;
            }


            if (i == grid.MapSize.X - 1 && j == grid.MapSize.Y - 1 && k == grid.MapSize.Z - 1)
                return false;

            if (i < grid.MapSize.X - 1)
                i++;
            else
            {
                i = 0;
                if (j < grid.MapSize.Y - 1)
                    j++;
                else
                {
                    j = 0;
                    if (k < grid.MapSize.Z - 1)
                        k++;
                    else
                    {
                        k = 0;
                    }
                }
            }

            chunk = grid[i, j, k];
            return true;
        }

        /// <summary>
        /// Resets the enumerator to the beginning.
        /// </summary>
        public void Reset()
        {
            chunk = null;
        }

        /// <summary>
        /// Returns the Chunk being pointed to by the enumerator.
        /// </summary>
        object IEnumerator.Current
        {
            get { return Current; }
        }

        /// <summary>
        /// Returns the Chunk being pointed to by the enumerator.
        /// </summary>
        public Chunk Current
        {
            get { return chunk; }
        }
    }
}
