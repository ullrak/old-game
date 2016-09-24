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
    class Player
    {
        public Texture2D texture, run1texture, run2texture, run3texture, run4texture, run5texture, run6texture, run7texture, run8texture, run9texture, run10texture, run11texture, run12texture, idletexture;
        public Texture2D swing1texture, swing2texture, swing3texture, swing4texture, swing5texture, swing6texture, swing7texture, swing8texture, swing9texture, swing10texture, swing11texture, swing12texture, swing13texture, swing14texture, swing15texture, swing16texture;
        public Texture2D weaponTexture;
        public Vector2 position;
        public Rectangle boundingBox, weaponBoundingBox;
        public Vector2 velocity, origin, weaponPosition;
        public bool hasJumped, isSwinging, isAlive;
        public Color[,] playerColorArray, playerWeaponData;
        public float timer, weaponRotation;
        GraphicsDevice graphics;
        public Color[] textureData;

        public Vector2 Position
        {
            get { return position; }
        }

        public Player() 
        {
            position = new Vector2(3250, 550);
            hasJumped = false;
            isSwinging = false;
            isAlive = true;
            
        }

        public void LoadContent(ContentManager Content, GraphicsDevice newGraphics)
        {
            /**
            walkAnimation = new SpriteSheet(Content.Load<Texture2D>("Run_Sheet"), 50, 0.1f, true);
            idleAnimation = new SpriteSheet(Content.Load<Texture2D>("Mount_Blade_Warband72_guy1_try1"), 50, 0.1f, true);
             * **/
            weaponTexture = Content.Load<Texture2D>("Long_sword_Overhead_1");
            playerWeaponData = TextureTo2DArray(weaponTexture);

            texture = Content.Load<Texture2D>("chainmail/Standing_guy_chainmail");
            idletexture = Content.Load<Texture2D>("chainmail/Standing_guy_chainmail");
            run1texture = Content.Load<Texture2D>("chainmail/run0");
            run2texture = Content.Load<Texture2D>("chainmail/run1");
            run3texture = Content.Load<Texture2D>("chainmail/run2");
            run4texture = Content.Load<Texture2D>("chainmail/run3");
            run5texture = Content.Load<Texture2D>("chainmail/run4");
            run6texture = Content.Load<Texture2D>("chainmail/run5");
            run7texture = Content.Load<Texture2D>("chainmail/run6");
            run8texture = Content.Load<Texture2D>("chainmail/run7");
            run9texture = Content.Load<Texture2D>("chainmail/run8");
            run10texture = Content.Load<Texture2D>("chainmail/run9");
            run11texture = Content.Load<Texture2D>("chainmail/run10");
            //run12texture = Content.Load<Texture2D>("RunningFrame11");

            swing1texture = Content.Load<Texture2D>("chainmail/swing0");
            swing2texture = Content.Load<Texture2D>("chainmail/swing1");
            swing3texture = Content.Load<Texture2D>("chainmail/swing2");
            swing4texture = Content.Load<Texture2D>("chainmail/swing3");
            swing5texture = Content.Load<Texture2D>("chainmail/swing4");
            swing6texture = Content.Load<Texture2D>("chainmail/swing5");
            swing7texture = Content.Load<Texture2D>("chainmail/swing6");
            swing8texture = Content.Load<Texture2D>("chainmail/swing7");
            swing9texture = Content.Load<Texture2D>("chainmail/swing8");
            swing10texture = Content.Load<Texture2D>("chainmail/swing9");
            swing11texture = Content.Load<Texture2D>("chainmail/swing10");
            swing12texture = Content.Load<Texture2D>("chainmail/swing11");
            swing13texture = Content.Load<Texture2D>("chainmail/swing12");
            swing14texture = Content.Load<Texture2D>("chainmail/swing13");
            swing15texture = Content.Load<Texture2D>("chainmail/swing14");
            swing16texture = Content.Load<Texture2D>("chainmail/swing15");

            graphics = newGraphics;

            

            /**
            textureData = new Color[walkAnimation.FrameWidth * walkAnimation.FrameHeight];
            walkAnimation.Texture.GetData(textureData);
             * **/

        }

        public void Update(GameTime gameTime)
        {
            position += velocity;
            boundingBox = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height); //walkAnimation.FrameWidth, walkAnimation.FrameHeight);//texture.Width, texture.Height);
            weaponBoundingBox = new Rectangle((int)weaponPosition.X, (int)weaponPosition.Y, weaponTexture.Width, weaponTexture.Height);
            origin = new Vector2(boundingBox.Width / 2, boundingBox.Height / 2);

            Input(gameTime);
            if (velocity.X != 0 && isSwinging == false)
                animateWalk(gameTime);
            else if (velocity.X == 0 && isSwinging == false)
                texture = idletexture;

            if (isSwinging == true) animateSwing(gameTime);

            /**
            if (velocity.X != 0)
                animationPlayer.PlayAnimation(walkAnimation);
            else if (velocity.X == 0)
                animationPlayer.PlayAnimation(idleAnimation);
             * **/

            if (velocity.Y < 10)
                velocity.Y += 0.4f;

            weaponPosition = drawWeaponHere(texture);
            weaponRotation = rotateWeapon(texture);

            //if (position.Y >= graphics.Viewport.Height - texture.Height)
           // {
            //    position.Y = graphics.Viewport.Height - texture.Height;
           //     velocity.Y = 0f;
           //     hasJumped = false;
           // }

           // velocity.Y += 0.15f * 0.75f;
        }

        private void Input(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.A) && isSwinging == false)
                velocity.X = -(float)gameTime.ElapsedGameTime.TotalMilliseconds / 3;
            else if (Keyboard.GetState().IsKeyDown(Keys.D) && isSwinging == false)
                velocity.X = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 3;
            else velocity.X = 0f;

            if (Keyboard.GetState().IsKeyDown(Keys.W) && hasJumped == false)
            {
                position.Y -= 5f;
                velocity.Y = -9f;
                hasJumped = true;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Space) && isSwinging == false)
            {
                isSwinging = true;
            }
        }

        public void Collision(Rectangle newRectangle, int xOffset, int yOffset)
        {
            if(boundingBox.TouchTopOf(newRectangle))
            {
                boundingBox.Y = newRectangle.Y - boundingBox.Height;
                velocity.Y = 0f;
                hasJumped = false;
            }

            if(boundingBox.TouchLeftOf(newRectangle))
            {
                position.X = newRectangle.X - boundingBox.Width - 2;  
            }
            if (boundingBox.TouchRightOf(newRectangle))
            {
                position.X = newRectangle.X + newRectangle.Width + 2;
            }
            if (boundingBox.TouchBottomOf(newRectangle))
            {
                velocity.Y = 1f;
            }

            if (position.X < 0) position.X = 0;
            if (position.X > xOffset - boundingBox.Width) position.X = xOffset - boundingBox.Width;
            if (position.Y < 0) velocity.Y = 1f;
            if (position.Y > yOffset - boundingBox.Height) position.Y = yOffset - boundingBox.Height;

        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            SpriteEffects flip = SpriteEffects.None;
            if (velocity.X >= 0)
                flip = SpriteEffects.None;
            else if (velocity.X < 0)
            {
                flip = SpriteEffects.FlipHorizontally;
                weaponPosition.X = position.X + ((position.X + texture.Width) - weaponPosition.X);
            }
            

            
            //animation tut
            /**
             * SpriteEffects flip = SpriteEffects.None;
            if (velocity.X >= 0)
                flip = SpriteEffects.None;
            else if (velocity.X < 0)
                flip = SpriteEffects.FlipHorizontally;

            animationPlayer.Draw(gameTime, spriteBatch, position, flip);
            **/
            //Vector2 wp = drawWeaponHere(texture);
            //float rot = rotateWeapon(texture);

            spriteBatch.Draw(weaponTexture, weaponPosition, null, Color.White, weaponRotation, new Vector2(13, 120), 1f, flip, 0f);
            spriteBatch.Draw(texture, position, null, Color.White, 0f, new Vector2(0,0), 1f, flip, 0f);
        }

        public void animateWalk(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            while (timer >= 0.1f)
            {
                timer -= 0.06f;

                if (texture == run11texture) texture = run1texture;
                else if (texture == run1texture) texture = run2texture;
                else if (texture == run2texture) texture = run3texture;
                else if (texture == run3texture) texture = run4texture;
                else if (texture == run4texture) texture = run5texture;
                else if (texture == run5texture) texture = run6texture;
                else if (texture == run6texture) texture = run7texture;
                else if (texture == run7texture) texture = run8texture;
                else if (texture == run8texture) texture = run9texture;
                else if (texture == run9texture) texture = run10texture;
                else texture = run11texture;

            }

            textureData = new Color[texture.Width * texture.Height];
            texture.GetData(textureData);
        }

        public void animateSwing(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            //texture = swing1texture;
            while (timer >= 0.1f)
            {
                //original 0.07f
                timer -= 0.08f;

                if (texture == swing16texture) texture = swing1texture;
                else if (texture == swing1texture) texture = swing2texture;
                else if (texture == swing2texture) texture = swing3texture;
                else if (texture == swing3texture) texture = swing4texture;
                else if (texture == swing4texture) texture = swing5texture;
                else if (texture == swing5texture) texture = swing6texture;
                else if (texture == swing6texture) texture = swing7texture;
                else if (texture == swing7texture) texture = swing8texture;
                else if (texture == swing8texture) texture = swing9texture;
                else if (texture == swing9texture) texture = swing10texture;
                else if (texture == swing10texture) texture = swing11texture;
                else if (texture == swing11texture) texture = swing12texture;
                else if (texture == swing12texture) texture = swing13texture;
                else if (texture == swing13texture) texture = swing14texture;
                else if (texture == swing14texture) texture = swing15texture;
                else
                {
                    texture = swing16texture;
                    isSwinging = false;
                }
                

            }

            textureData = new Color[texture.Width * texture.Height];
            texture.GetData(textureData);
        }

        public Color[,] TextureTo2DArray(Texture2D texture)
        {

            Color[] colors1D = new Color[texture.Width * texture.Height]; //The hard to read,1D array
            texture.GetData(colors1D); //Get the colors and add them to the array

            Color[,] colors2D = new Color[texture.Width, texture.Height]; //The new, easy to read 2D array
            for (int x = 0; x < texture.Width; x++) //Convert!
                for (int y = 0; y < texture.Height; y++)
                    colors2D[x, y] = colors1D[x + y * texture.Width];

            return colors2D;
        }

        public Vector2 drawWeaponHere(Texture2D texture)
        {
            playerColorArray = TextureTo2DArray(texture);
            Vector2 weapPos = new Vector2(3250, 550);
            
            for (int y = 0; y < texture.Height; y++)
            {
                for (int x = 0; x < texture.Width; x++)
                {
                    Color color = playerColorArray[x, y];
                    Color weapColor = new Color(57, 43, 31, 255);
                    if (color == weapColor) { weapPos.X = x + position.X; weapPos.Y = y + position.Y; }
                }
            }

            return weapPos;
        }

        public float rotateWeapon(Texture2D texture)
        {
            playerColorArray = TextureTo2DArray(texture);
            double weapRot = new double();
            weapRot = 0;
            Vector2 pos1 = new Vector2(0, 0);
            Vector2 pos2 = new Vector2(0, 0);
            Vector2 pos3 = new Vector2(0, 0);
            Vector2 eyePos = new Vector2(0, 0);

            for (int y = 0; y < texture.Height; y++)
            {
                for (int x = 0; x < texture.Width; x++)
                {
                    Color color = playerColorArray[x, y];

                    Color weapBottomColor = new Color(57, 44, 32, 255);
                    Color weapMidColor = new Color(57, 43, 31, 255);
                    Color weapTopColor = new Color(57, 44, 31, 255);
                    Color eyeColor = new Color(25, 84, 15, 255);

                    if (color == weapTopColor) { pos1.X = x + position.X; pos1.Y = y + position.Y; }
                    if (color == weapMidColor) { pos2.X = x + position.X; pos2.Y = y + position.Y; }
                    if (color == weapBottomColor) { pos3.X = x + position.X; pos3.Y = y + position.Y; }
                    if (color == eyeColor) { eyePos.X = x + position.X; eyePos.Y = y + position.Y; }

                    double opp = pos2.Y - eyePos.Y;
                    double adj = pos2.X - eyePos.X;

                    //Standing guy eye position is 75, 21.  Glove color pixel position is 113, 55
                    //When the hands are below the starting pos by 5 lets tweak the rotation down to mimic moving the wrist
                    //When the hands are above the eye pos by the height of the head start keeping the rotation at perpendicular
                    double correction = -0.9;
                    if (opp > 45) { correction = 0.5; }
                    if (opp < 0) { correction = -0.25; }
                    weapRot = (Math.Atan2(opp, adj)) + correction;

                    //1.57 is pointing to the right
                    //-1.57 is pointing to the left
                    //0.7854 is pointing NE
                    //0 is north
                    //3.142 is pointing south

                    if (opp < 0 && weapRot < -1.57) { weapRot = -1.57; }
                    //weapRot = (Math.Atan2(pos1.Y - pos2.Y, pos1.X - pos2.X)) - 1.5;
                }
            }
            
            return (float)weapRot;
        }
    }

}
