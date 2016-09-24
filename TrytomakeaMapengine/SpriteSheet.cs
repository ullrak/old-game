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
    public class SpriteSheet
    {
        public Texture2D levelImageSheet, playerMoveSheet;
        public Texture2D LevelBlockSheet;
        public Texture2D TileSprites;
        Vector2 spritePosition = Vector2.Zero;

        public Texture2D Texture
        {
            get { return playerMoveSheet; }
        }

        public int FrameWidth; //width of sprite

        public int FrameHeight
        {
            get { return playerMoveSheet.Height; }
        }

        float frameTime; //speed of looping
        public float FrameTime
        {
            get { return frameTime; }
        }

        public int FrameCount;

        bool isLooping;
        public bool IsLooping
        {
            get { return isLooping; }
        }


        public SpriteSheet(Texture2D newTexture, int newFrameWidth, float newFrameTime, bool newIsLooping)
        {
            playerMoveSheet = newTexture;
            FrameWidth= newFrameWidth;
            frameTime = newFrameTime;
            isLooping = newIsLooping;
            FrameCount = playerMoveSheet.Width / FrameWidth;
        }

        public void LoadContent(ContentManager Content)
        {
            levelImageSheet = Content.Load<Texture2D>("TestLevel2");
            //playerMoveSheet = Content.Load<Texture2D>("Run_Sheet");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
        }
    }

   

}
