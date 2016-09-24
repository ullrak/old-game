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
    class EnemyPirate
    {
        Texture2D texture, ballTexture, healthTexture;
        public Rectangle boundingBox, healthRectangle;
        Vector2 position;
        public int speed, current, reloadTime, health, currentDifficultyLevel;
        int handling;
        public bool isVisible, shotsFired;

        public List<CannonBall> ballList;

        public EnemyPirate(Texture2D newTexture, Vector2 newPosition, Texture2D newBalltexture)
        {
            ballList = new List<CannonBall>();
            texture = newTexture;
            ballTexture = newBalltexture;
            health = 5;
            position = newPosition;
            currentDifficultyLevel = 1;
            reloadTime = 80;
            speed = 1;
            isVisible = true;
            boundingBox = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
        }

        public void Update(GameTime gameTime)
        {
            boundingBox = new Rectangle((int)position.X - 123, (int)position.Y - texture.Height/2, texture.Width, texture.Height);

            position.X -= speed;

            if (position.X <= 150)
                position.X = 1000;

            //EnemyShoot();
            UpdateCannonBalls();
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, null, Color.White, 0f, new Vector2(123, 60), 1f, SpriteEffects.FlipHorizontally, 0f);

            foreach (CannonBall c in ballList)
            {
                c.Draw(spriteBatch);
            }
        }

        public void UpdateCannonBalls()
        {
            EnemyShoot();
           
            foreach (CannonBall c in ballList)
            {
                c.position.X -= (speed+1) * 5;
                //c.position.Y += speed;
                c.boundingBox.Location = new Point((int)c.position.X, (int)c.position.Y);
                if (c.position.X <= 10)
                    c.isVisible = false;
            }

            for (int i = 0; i < ballList.Count; i++)
            {
                if (!ballList[i].isVisible)
                {
                    ballList.RemoveAt(i);
                    i--;
                }
            }
        }

        public void EnemyShoot()
        {
            if (reloadTime >= 0)
                reloadTime--;

            if (reloadTime <= 0)
            {
                CannonBall newBall = new CannonBall(ballTexture);
                newBall.position = new Vector2(position.X, position.Y);
                newBall.isVisible = true;

                ballList.Add(newBall);
            }

            if (reloadTime <= 0)
                reloadTime = 80;
        }

    }
}
