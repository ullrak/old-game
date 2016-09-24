﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TrytomakeaMapengine
{
    struct AnimationPlayer
    {
        SpriteSheet animation;
        public SpriteSheet Animation
        {
            get { return animation; }
        }

        int frameIndex;
        public int FrameIndex
        {
            get { return frameIndex; }
            set { frameIndex = value; }
        }

        private float timer;

        public Vector2 Origin
        {
            get { return new Vector2(animation.FrameWidth / 2, animation.FrameHeight); }
        }

        public void PlayAnimation(SpriteSheet newAnimation)
        {
            if (animation == newAnimation)
                return;

            animation = newAnimation;
            frameIndex = 0;
            timer = 0;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 position, SpriteEffects spriteEffects)
        {
            if (Animation == null)
                throw new NotSupportedException("no animation selected");

            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            while (timer >= animation.FrameTime)
            {
                timer -= animation.FrameTime;

                if (animation.IsLooping)
                    frameIndex = (frameIndex + 1) % animation.FrameCount;
                else frameIndex = Math.Min(frameIndex + 1, animation.FrameCount - 1);
            }

            Rectangle rectangle = new Rectangle(frameIndex * Animation.FrameWidth, 0, Animation.FrameWidth, Animation.FrameHeight);

            spriteBatch.Draw(Animation.Texture, position, rectangle, Color.White, 0f, Origin, 1f, spriteEffects, 0f);
        }
    }
}
