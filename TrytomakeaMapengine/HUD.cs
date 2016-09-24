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
    public class HUD
    {
        public int playerScore, screenWidth, screenHeight;
        public SpriteFont playerScoreFont;
        public Vector2 playerScorePos;
        public bool showHUD;

        public HUD()
        {
            playerScore = 0;
            showHUD = true;
            screenHeight = 720;
            screenWidth = 1280;
            playerScoreFont = null;
            playerScorePos = new Vector2(screenWidth / 2, 50);
        }

        public void LoadContent(ContentManager Content)
        {
            playerScoreFont = Content.Load<SpriteFont>("georgia");
        }

        public void Update(GameTime gameTime)
        {

            KeyboardState keyState = Keyboard.GetState();
            if (keyState.IsKeyDown(Keys.Tab))
            {
                if (showHUD == true) { showHUD = false; }
                else if (showHUD == false) { showHUD = true; }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (showHUD == true)
            {
                spriteBatch.DrawString(playerScoreFont, "HE said, Let's get it on " + playerScore, playerScorePos, Color.Silver);
            }
        }
    }
}
