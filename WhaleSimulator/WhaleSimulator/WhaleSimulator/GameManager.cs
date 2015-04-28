﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using RB_GameResources.Xna.Controls;

namespace WhaleSimulator
{
    public enum GameState
    {
        Undefined, MainMenu, PauseMenu, Playing
    }

    public class GameManager
    {
        private GameState gameState = GameState.Undefined;
        
        private MainMenu mainMenu;
        private Map currentMap;

        public GameManager()
        {
            ChunkGrid grid = new ChunkGrid("Data/MapData/Tutorial.xml");
            int x = 5;
        }

        public void Initialize()
        {
            LoadMainMenu();
        }

        private void LoadMainMenu()
        {
            mainMenu = new MainMenu();
            mainMenu.mapChooseEvent += LoadMap;
            gameState = GameState.MainMenu;
        }

        void LoadMap(string name)
        {
            //Load map
            currentMap = new Map(name);

        }

        public void Update(GameTime gameTime, InputStates inputStates)
        {
            switch (gameState)
            {
                case GameState.Undefined:
                    break;
                case GameState.MainMenu:
                    mainMenu.Update(gameTime, inputStates);
                    break;
                case GameState.PauseMenu:
                    break;
                case GameState.Playing:
                    break;
            }
        }

        /// <summary>
        /// Draws any 3D objects to the screen (3D objects are always drawn behind 2D sprites).
        /// </summary>
        /// <param name="gameTime">The GameTime object to use as reference.</param>
        public void Draw3D(GameTime gameTime)
        {

        }

        /// <summary>
        /// Draws any 2-dimensional sprites to the SpriteBatch (2D Sprites are always drawn above 3D material).
        /// </summary>
        /// <param name="gameTime">The GameTime object to use as reference.</param>
        /// <param name="spriteBatch">The SpriteBatch to draw to.</param>
        public void Draw2D(GameTime gameTime, SpriteBatch spriteBatch)
        {

        }
    }
}