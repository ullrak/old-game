using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace TrytomakeaMapengine
{
    public class PlatformerHUD
    {
        Texture2D healthTexture;
        public int playerPosInfo, screenWidth, screenHeight, playerHealth, enemyHealth;
        public SpriteFont playerHUDFont;
        public Vector2 MainHUDpos, HUDposWeaponPosition, HUDposWeaponRotation, HUDposEnemy, HUDposCollision;
        public Vector2 playerPos, playerWeaponPos, enemyPos, colPointEnemy;
        public bool showHUD;
        public float HUDweaponRotation;
        Rectangle playerHealthRectangle, enemyHealthRectangle;

        public PlatformerHUD()
        {
            healthTexture = null;
            
            showHUD = true;
            screenHeight = 720;
            screenWidth = 1280;
            playerHUDFont = null;
            MainHUDpos = new Vector2(0, 0);
            HUDposWeaponRotation = new Vector2(0, 0);
            HUDposWeaponPosition = new Vector2(0, 0);
            HUDposEnemy = new Vector2(0, 0);
            playerPosInfo = 0;
            playerPos = new Vector2(0, 0);
            playerWeaponPos = new Vector2(0, 0);
            playerHealth = 200;
            enemyPos = new Vector2(0, 0);
            enemyHealth = 100;
            colPointEnemy = new Vector2(0, 0);
            HUDposCollision = new Vector2(0, 0);
        }

        public void LoadContent(ContentManager Content)
        {
            playerHUDFont = Content.Load<SpriteFont>("georgia");
            healthTexture = Content.Load<Texture2D>("healthTexture1");
        }

        public void Update(GameTime gameTime)
        {

            KeyboardState keyState = Keyboard.GetState();
            if (keyState.IsKeyDown(Keys.Tab))
            {
                if (showHUD == true) { showHUD = false; }
                else if (showHUD == false) { showHUD = true; }
            }
            MainHUDpos.X = playerPos.X;
            MainHUDpos.Y = playerPos.Y - 300;
            HUDposWeaponPosition.X = playerPos.X + 200;
            HUDposWeaponPosition.Y = playerPos.Y -200;
            HUDposWeaponRotation.X = playerPos.X + 200;
            HUDposWeaponRotation.Y = playerPos.Y - 150;
            HUDposEnemy.X = playerPos.X - 500;
            HUDposEnemy.Y = playerPos.Y - 200;
            HUDposCollision.X = playerPos.X - 500;
            HUDposCollision.Y = playerPos.Y - 150;
            playerHealthRectangle = new Rectangle((int)playerPos.X - (screenWidth/2 - 10), (int)playerPos.Y - (screenHeight / 2) + 10, playerHealth, 25);
            enemyHealthRectangle = new Rectangle((int)playerPos.X + (screenWidth/2 - (enemyHealth + 10)), (int)playerPos.Y - (screenHeight / 2) + 10, enemyHealth, 25);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (showHUD == true)
            {
                spriteBatch.DrawString(playerHUDFont, "Put useful info here,  " + playerPosInfo, MainHUDpos, Color.Green);
                spriteBatch.DrawString(playerHUDFont, "Weapon Rotation,  " + HUDweaponRotation, HUDposWeaponRotation, Color.Green);
                spriteBatch.DrawString(playerHUDFont, "Weapon Position, X = " + (playerWeaponPos.X) +" Y = "+ (playerWeaponPos.Y), HUDposWeaponPosition, Color.Green);

                spriteBatch.DrawString(playerHUDFont, "Enemy Position, X = " + (enemyPos.X) + " Y = " + (enemyPos.Y), HUDposEnemy, Color.Green);

                spriteBatch.DrawString(playerHUDFont, "Point of Collision, X = " + (colPointEnemy.X) + " Y = " + (colPointEnemy.Y), HUDposCollision, Color.Green);

                spriteBatch.Draw(healthTexture, playerHealthRectangle, Color.White);
                spriteBatch.Draw(healthTexture, enemyHealthRectangle, Color.White);
            }
        }
    }
}
