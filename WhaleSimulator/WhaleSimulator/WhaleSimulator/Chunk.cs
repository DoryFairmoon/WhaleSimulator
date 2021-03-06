﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using RB_GameResources.Xna.Controls;

namespace WhaleSimulator
{
    public class Chunk
    {
        public enum Directions
        {
            North, South, East, West
        }

        /// <summary>
        /// Get or set the Chunk immediately to the north of this one.
        /// </summary>
        public Chunk North { get; set; }
        /// <summary>
        /// Get or set the Chunk immediately to the south of this one.
        /// </summary>
        public Chunk South { get; set; }
        /// <summary>
        /// Get or set the Chunk immediately to the east of this one.
        /// </summary>
        public Chunk East { get; set; }
        /// <summary>
        /// Get or set the Chunk immediately to the west of this one.
        /// </summary>
        public Chunk West { get; set; }
        /// <summary>
        /// Get or set the Chunk immediately above this one.
        /// </summary>
        public Chunk Up { get; set; }
        /// <summary>
        /// Get or set the Chunk immediately below this one.
        /// </summary>
        public Chunk Down { get; set; }

        /// <summary>
        /// Get the position of this Chunk in the ChunkGrid.
        /// </summary>
        public Vector3 Position { get { return position; } private set { position = value; } }
        /// <summary>
        /// The ContentManager responsible for this Chunk's assets.
        /// </summary>
        //public ContentManager Content { get; set; }
        /// <summary>
        /// Whether or not the assets in this Chunk are loaded in memory.
        /// </summary>
        public bool IsLoaded { get; private set; }

        public List<CreatureInfo> CreatureList { get; set; }
        public List<Creature> Creatures { get; set; }
        public List<CreatureInfo> TerrainList { get; set; }
        public List<Creature> Terrain { get; set; }

        private Vector3 position;

        /// <summary>
        /// Creates a new Chunk object with a given grid position.  Does not link to other Chunks in the grid.
        /// </summary>
        /// <param name="position"></param>
        public Chunk(Vector3 position)
        {
            Position = position;
            IsLoaded = false;
        }
         
        /// <summary>
        /// Update all the objects in this Chunk.
        /// </summary>
        /// <param name="gameTime">The GameTime object to use as reference.</param>
        /// <param name="inputStates">The InputStates object to use when checking player input.</param>
        public void Update(GameTime gameTime, InputStates inputStates)
        {
            //Update
            if (Creatures != null)
            {
                foreach (Creature c in Creatures)
                    c.Update(gameTime, inputStates);
            }

            if (Terrain != null)
            {
                foreach (Creature t in Terrain)
                    t.Update(gameTime, inputStates);
            }
        }

        /// <summary>
        /// Draws any 3D objects to the screen (3D objects are always drawn behind 2D sprites).
        /// </summary>
        /// <param name="gameTime">The GameTime object to use as reference.</param>
        public void Draw3D(GameTime gameTime)
        {
            //Draw
            if ((position.X > 0 && position.Y < 4) && (position.Z > 0 && position.Z < 4))
            {
                if (Creatures != null)
                {
                    foreach (Creature c in Creatures)
                        c.Draw3D(gameTime);
                }
            }
            

            if (Terrain != null)
            {
                foreach (Creature t in Terrain)
                    t.Draw3D(gameTime);
            }

            //if (StaticTerrain != null)
            //{
            //    foreach (Graphics3D t in StaticTerrain)
            //        t.Draw3D(gameTime);
            //}
        }

        /// <summary>
        /// Loads the assets associated with this Chunk into memory.
        /// </summary>
        public void LoadAssets()
        {
            if (!IsLoaded)
            {
                foreach (CreatureInfo info in CreatureList)
                {
                    if (info.IsAlive)
                        Creatures.Add(new Creature(info));
                }
                foreach (CreatureInfo info in TerrainList)
                {
                    Terrain.Add(new Creature(info));
                }
                IsLoaded = true;
            }
        }

        public void ShiftPosition(Directions direction)
        {
            switch (direction)
            {
                case Directions.East:
                    if (Position.X < Map.MapSize.X - 1)
                        position.X++;
                    else
                        position.X = 0;
                    break;

                case Directions.West:
                    if (Position.X > 0)
                        position.X--;
                    else
                        position.X = Map.MapSize.X - 1;
                    break;

                case Directions.South:
                    if (Position.Z < Map.MapSize.Z - 1)
                        position.Z++;
                    else
                        position.Z = 0;
                    break;

                case Directions.North:
                    if (Position.Z > 0)
                        position.Z--;
                    else
                        position.Z = Map.MapSize.Z - 1;
                    break;
            }
        }

        public void ShiftAssets(Directions direction, int distance)
        {
            switch (direction)
            {
                case Directions.East:
                    foreach (Creature creature in Creatures)
                    {
                        Vector3 newPosition = creature.Position;
                        newPosition.X += distance;
                        creature.Position = newPosition;
                    }
                    break;

                case Directions.West:
                    foreach (Creature creature in Creatures)
                    {
                        Vector3 newPosition = creature.Position;
                        newPosition.X -= distance;
                        creature.Position = newPosition;
                    }
                    break;

                case Directions.North:
                    foreach (Creature creature in Creatures)
                    {
                        Vector3 newPosition = creature.Position;
                        newPosition.Z -= distance;
                        creature.Position = newPosition;
                    }
                    break;

                case Directions.South:
                    foreach (Creature creature in Creatures)
                    {
                        Vector3 newPosition = creature.Position;
                        newPosition.Z += distance;
                        creature.Position = newPosition;
                    }
                    break;
            }
        }

        /*
        /// <summary>
        /// Unloads this Chunk's ContentManager,  removing all assets associated with it from memory.
        /// </summary>
        public void UnloadAssets()
        {
            if (Content != null)
            {
                Content.Unload();
                Content.Dispose();
                Content = null;
            }

            if (Creatures.Count > 0)
            {
                foreach (Creature c in Creatures)
                {
                    c.Dispose();
                }
                Creatures.Clear();
                Creatures.TrimExcess();
            }

            if (StaticTerrain.Count > 0)
            {
                StaticTerrain.Clear();
                StaticTerrain.TrimExcess();
            }

            IsLoaded = false;
        }
         * */

    }
}
