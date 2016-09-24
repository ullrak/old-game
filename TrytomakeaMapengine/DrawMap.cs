using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace TrytomakeaMapengine
{
    class DrawMap
    {
        int TileWidth = 64, TileHeight = 64;

        //Creating a tilemap part 1
        private List<CollisionTiles> collisionTiles = new List<CollisionTiles>();
        public List<CollisionTiles> CollisionTiles
        {
            get { return collisionTiles; }
        }

        private List<NoCollisionTiles> noCollisionTiles = new List<NoCollisionTiles>();
        public List<NoCollisionTiles> NoCollisionTiles
        {
            get { return noCollisionTiles; }
        }

        public DrawMap() { }

        public void LoadContent(ContentManager Content)
        {
 
        }

        public Color[,] TextureTo2DArray(Texture2D texture)
        {

            Color[] colors1D = new Color[texture.Width * texture.Height]; //one d array
            texture.GetData(colors1D); //Get the colors and add them to the array

            Color[,] colors2D = new Color[texture.Width, texture.Height]; //two d array
            for (int x = 0; x < texture.Width; x++) //Convert!
                for (int y = 0; y < texture.Height; y++)
                    colors2D[x, y] = colors1D[x + y * texture.Width];

            return colors2D; 
        }

        public void Generate(Color[,] map, Texture2D theMap)
        {
            for (int y = 0; y < theMap.Height; y++)
            {
                for (int x = 0; x < theMap.Width; x++)
                {
                    Vector2 position = new Vector2(x * TileWidth, y * TileHeight);

                    Color grassColor = new Color(0, 255, 0, 255);
                    Color woodColor = new Color(127, 127, 0, 255);
                    Color stoneColor = new Color(255, 255, 0, 255);
                    Color dirtColor = new Color(127, 51, 0, 255);

                    Color color = map[x, y];
                    Color voidColor = new Color(255,255,255,255);
                    if (color != voidColor && color != woodColor && color != dirtColor)
                    {
                        collisionTiles.Add(new CollisionTiles(color, new Rectangle(x * TileWidth, y * TileHeight, TileWidth, TileHeight)));
                    }
                    if (color != voidColor && color != stoneColor && color != grassColor)
                    {
                        noCollisionTiles.Add(new NoCollisionTiles(color, new Rectangle(x * TileWidth, y * TileHeight, TileWidth, TileHeight)));
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (CollisionTiles tile in collisionTiles)
                tile.Draw(spriteBatch);
            foreach (NoCollisionTiles tile in noCollisionTiles)
                tile.Draw(spriteBatch);
        }
    }
}
