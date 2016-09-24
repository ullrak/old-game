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
    class PlayerShip
    {
        Texture2D texture, ballTexture, healthTexture;
        public Rectangle boundingBox, healthRectangle;
        Vector2 position;
        public int speed = 4, current = 0, reloadTime = 10, health = 200;
        int handling = 1;
        bool turningUp, turningDown, shotsFired;
        float rot;

        public List<CannonBall> ballList;

        public PlayerShip()
        {
            ballList = new List<CannonBall>();
            texture = null;
            position = new Vector2(100, 360);
            rot = 0;
            shotsFired = false;
            
        }

        public void LoadContent(ContentManager Content)
        {
            texture = Content.Load<Texture2D>("shipMaybe");
            ballTexture = Content.Load<Texture2D>("cannonBall");
            healthTexture = Content.Load<Texture2D>("ShipHealthHeart");
        }

        public void Update(GameTime gameTime)
        {
            KeyboardState keyState = Keyboard.GetState();

            boundingBox = new Rectangle((int)position.X - 123, (int)position.Y - 60, texture.Width, texture.Height);

            turningUp = false;
            turningDown = false;
            //rot = 0;

            if (keyState.IsKeyDown(Keys.E))
            {
                position.Y += handling;
                rot += 0.017f;
                turningDown = true;
            }
            //if(turningDown == true) { rot = 0; }

            if (keyState.IsKeyDown(Keys.Q))
            {
                position.Y -= handling;
                rot += 6.2657f;
                turningUp = true;
            }
           // if(turningUp == true) { rot = 0; }

            if (keyState.IsKeyDown(Keys.A))
                position.X -= handling;

            if (keyState.IsKeyDown(Keys.D))
                position.X += speed;

            if (keyState.IsKeyDown(Keys.W))
                position.Y -= handling;

            if (keyState.IsKeyDown(Keys.S))
                position.Y += handling;

            if (keyState.IsKeyDown(Keys.Space))
                shotsFired = true;


            UpdateCannonBalls();

            healthRectangle = new Rectangle(50, 50, health, 25);
            position.X -= current;

        }

        public void Draw(SpriteBatch spriteBatch)
        {

            spriteBatch.Draw(texture, position, null, Color.White, rot, new Vector2(123, 60), 1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(healthTexture, healthRectangle, Color.White);

            foreach (CannonBall c in ballList)
                c.Draw(spriteBatch);
        }

        public void Shoot()
        {
            if (reloadTime >= 0) { reloadTime--; }

            if (shotsFired == true && reloadTime <= 0)
            {
                CannonBall newBall = new CannonBall(ballTexture);
                //try to start at cannon
                newBall.position = new Vector2(position.X, position.Y);
                newBall.isVisible = true;

                ballList.Add(newBall);
 
            }

            if (shotsFired == true && reloadTime <= 0)
            {
                shotsFired = false;
                reloadTime = 30;
            }
        }

        public void UpdateCannonBalls()
        {
            Shoot();
            foreach (CannonBall c in ballList)
            {
                c.position.X += speed;
                //c.position.Y += speed;
                c.boundingBox.Location = new Point((int)c.position.X, (int)c.position.Y);
                if (c.position.X >= 1000)
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
    }
}
