using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

//namespace ShipGameTry1
namespace TrytomakeaMapengine
{
    // From ShipHameTry1
    class CannonBall
    {
        Texture2D ballTexture;
        public Rectangle boundingBox;
        public Vector2 position, origin;
        int speed;
        public bool isVisible;

        public CannonBall(Texture2D texture)
        {
            speed = 10;
            ballTexture = texture;
            isVisible = false;
            //boundingBox = new Rectangle((int)position.X, (int)position.Y, 25, 25); //ballTexture.Width, ballTexture.Height);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(ballTexture, position, Color.White);
        }

    }
}
