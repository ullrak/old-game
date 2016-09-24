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
    class Tiles
    {
        protected Texture2D texture;

        private Rectangle rectangle;
        public Rectangle Rectangle
        {
            get { return rectangle; }
            protected set { rectangle = value; }
        }

        private static ContentManager content;
        public static ContentManager Content
        {
            protected get { return content; }
            set { content = value; }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, rectangle, Color.White);
        }
           
    }

    class CollisionTiles : Tiles
    {
        public CollisionTiles(Color color, Rectangle newRectangle)
        {
            Color pixColor = color;
            Color grassColor = new Color(0, 255, 0, 255);
            Color woodColor = new Color(127, 127, 0, 255);
            Color stoneColor = new Color(255, 255, 0, 255);
            String path = "earthTile";

            if (color == grassColor) path = "grassonearthTile";
          //  if (color == woodColor) path = "woodTile";
            if (color == stoneColor) path = "stonewallTile";

            texture = Content.Load<Texture2D>(path);
            this.Rectangle = newRectangle;
        }
    }

    class NoCollisionTiles : Tiles
    {
        public NoCollisionTiles(Color color, Rectangle newRectangle)
        {
            Color pixColor = color;
            Color grassColor = new Color(0, 255, 0, 255);
            Color woodColor = new Color(127, 127, 0, 255);
            Color stoneColor = new Color(255, 255, 0, 255);
            Color dirtColor = new Color(127, 51, 0, 255);

            String path = "earthTIle";

         //   if (color == grassColor) path = "grassTile";
            if (color == woodColor) path = "wood2Tile";
            if (color == dirtColor) path = "earthTile";
          //  if (color == stoneColor) path = "stoneTile";

            texture = Content.Load<Texture2D>(path);
            this.Rectangle = newRectangle;
        }
    }
}
