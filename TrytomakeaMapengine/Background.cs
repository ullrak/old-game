using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

//namespace ShipGameTry1
namespace TrytomakeaMapengine
{
    // From ShipGameTry1
    public class Background
    {
        Texture2D background;
        Rectangle rectangle;
        Vector2 position1, position2;
        public int speed;

        public Background()
        {
            background = null;
            position1 = new Vector2(0, 0);
            position2 = new Vector2(640 * 2, 0);
            speed = 2;
        }

        public void LoadContent(ContentManager Content)
        {
            background = Content.Load<Texture2D>("Water_Background");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, position1, null, Color.White, 0f, new Vector2(0,0), 2f, SpriteEffects.None, 0f);
            spriteBatch.Draw(background, position2, null, Color.White, 0f, new Vector2(0, 0), 2f, SpriteEffects.None, 0f);
        }


        public void Updtae(GameTime gameTime)
        {
            position1.X = position1.X - speed;
            position2.X = position2.X - speed;

            if (position1.X <= -640 * 2)
            {
                position1.X = 0;
                position2.X = 640 * 2;
            }

        }

    }
}
