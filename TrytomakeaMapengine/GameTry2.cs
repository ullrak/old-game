using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace TrytomakeaMapengine
{
    public class GameTry2 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Camera camera;
        Player player;
        DrawMap drawMap = new DrawMap();
        PlatformerHUD platformerHUD = new PlatformerHUD();
        Texture2D levelSheet;
        public List<EnemyKnight> enemyKnights;
        Color[,] playerWeaponData;
        Color[,] enemyBody;
        Color[,] playerBody;
        Color[,] enemyWeapon;

        #region vars from ship

        public enum State
        {
            Menu,
            Playing,
            Boarding,
            Gameover
        }

        Background backGround = new Background();
        PlayerShip playerShip = new PlayerShip();
        HUD HUD = new HUD();

        public float scale = 2;
        public int width, height;
        public Texture2D menuTexture;
        public int enemyBallDamage;
        Random random = new Random();

        List<EnemyPirate> enemyList = new List<EnemyPirate>();

        //Set first State
        State gameState = State.Menu;

        #endregion

        #region Main Game Block

        public GameTry2()
        {
            graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
            graphics.IsFullScreen = true;
            graphics.PreferredBackBufferHeight = 360 * 2;
            graphics.PreferredBackBufferWidth = 640 * 2;
            this.Window.Title = "Almost a Game?";
            menuTexture = null;
            enemyBallDamage = 5;
            enemyKnights = new List<EnemyKnight>();
        }

        protected override void Initialize()
        {
            player = new Player();
            EnemyKnight badiey = new EnemyKnight();
            enemyKnights.Add(badiey);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            player.LoadContent(Content, GraphicsDevice);
            playerWeaponData = player.playerWeaponData;

            foreach (EnemyKnight badie in enemyKnights)
                badie.LoadContent(Content);

            Tiles.Content = Content;
            levelSheet = Content.Load<Texture2D>("TestLevel2");
            Color[,] Colors = drawMap.TextureTo2DArray(levelSheet);
            drawMap.Generate(Colors, levelSheet);
            camera = new Camera(GraphicsDevice.Viewport);

            //From ship LoadContent
            HUD.LoadContent(Content);
            platformerHUD.LoadContent(Content);
            playerShip.LoadContent(Content);
            backGround.LoadContent(Content);
            menuTexture = Content.Load<Texture2D>("Menu_pirate_game_try1");
            LoadEnemy();

        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            //From Ship
            #region Ship stuff

            switch (gameState)
            {
                case State.Playing:
                    {
                        foreach (EnemyPirate e in enemyList)
                        {
                            //add collision fool
                            if (e.boundingBox.Intersects(playerShip.boundingBox))
                            {
                                //playerShip.health -= 40;

                                e.isVisible = false;

                                gameState = State.Boarding;
                            }

                            //bullet collision
                            for (int i = 0; i < e.ballList.Count; i++)
                            {
                                if (playerShip.boundingBox.Intersects(e.ballList[i].boundingBox))
                                {
                                    playerShip.health -= enemyBallDamage;
                                    e.ballList[i].isVisible = false;
                                }
                            }

                            for (int i = 0; i < playerShip.ballList.Count; i++)
                            {
                                if (playerShip.ballList[i].boundingBox.Intersects(e.boundingBox))
                                {
                                    playerShip.ballList[i].isVisible = false;
                                    e.speed = 0;
                                }
                            }

                            e.Update(gameTime);
                        }



                        backGround.speed = 2;
                        playerShip.Update(gameTime);
                        backGround.Updtae(gameTime);
                        HUD.Update(gameTime);

                        break;

                    }

                case State.Menu:
                    {
                        KeyboardState keyState = Keyboard.GetState();
                        if (keyState.IsKeyDown(Keys.Enter))
                            gameState = State.Playing;

                        backGround.Updtae(gameTime);
                        backGround.speed = 1;
                        break;
                    }
#endregion
                #region action stuff
                case State.Boarding:
                    {
                        //GameTry2 original update
                        if (Keyboard.GetState().IsKeyDown(Keys.Up))
                            camera.Zoom += 0.01f;
                        if (Keyboard.GetState().IsKeyDown(Keys.Down))
                            camera.Zoom -= 0.01f;
                        if (Keyboard.GetState().IsKeyDown(Keys.Left))
                            camera.Rotation += 0.01f;
                        if (Keyboard.GetState().IsKeyDown(Keys.Right))
                            camera.Rotation -= 0.01f;

                        player.Update(gameTime);
                        if (enemyKnights.Count == 0)
                        {
                            EnemyKnight badiey = new EnemyKnight();
                            enemyKnights.Add(badiey);
                        }
                        foreach (EnemyKnight badie in enemyKnights)
                            badie.Update(gameTime);

                        foreach (CollisionTiles tile in drawMap.CollisionTiles)
                        {
                            player.Collision(tile.Rectangle, levelSheet.Width * 64, levelSheet.Height * 64);
                            foreach (EnemyKnight badie in enemyKnights)
                                badie.Collision(tile.Rectangle, levelSheet.Width * 64, levelSheet.Height * 64);
                        }

               #region Enemy AI Updates
                        enemyHitFinal();

                        foreach (EnemyKnight badie in enemyKnights)
                        {
                            int enemySight = 1000;
                            int strikeRange = 100;
                            float xDistance = badie.Position.X - player.Position.X; 
                            float yDistance = player.Position.Y - badie.Position.Y;

                            if (badie.hitByPlayerPoint.X > -1)
                            {
                                badie.velocity.X = 0f;
                                badie.isSwinging = false;
                                badie.stunned = 60;
                                //badie.hitByPlayerPoint = new Vector2(-1, -1);
                            }
                            else
                            {

                                if (xDistance < enemySight && xDistance > strikeRange)
                                {
                                    badie.StepLeft(gameTime);
                                }
                                if (xDistance > -enemySight && xDistance < -strikeRange)
                                {
                                    badie.StepRight(gameTime);
                                }
                                if (Math.Abs(xDistance) < strikeRange)
                                {
                                    badie.Swing(gameTime);
                                }
                            }
                            
                        }
                        #endregion

                        platformerHUD.playerPos.X = player.position.X;
                        platformerHUD.playerPos.Y = player.position.Y;
                        platformerHUD.playerWeaponPos.X = player.weaponPosition.X;
                        platformerHUD.playerWeaponPos.Y = player.weaponPosition.Y;
                        platformerHUD.HUDweaponRotation = player.weaponRotation;
                        platformerHUD.enemyPos.X = enemyKnights[0].position.X;
                        platformerHUD.enemyPos.Y = enemyKnights[0].position.Y;
                        platformerHUD.Update(gameTime);
                        platformerHUD.colPointEnemy = enemyKnights[0].hitByPlayerPoint;
                        camera.Update(player.Position);
                        //end of that

                        break;
                    }

                case State.Gameover:
                    {

                        break;
                    }
            }

            #endregion

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();

            switch (gameState)
            {
                case State.Playing:
                    {
                        backGround.Draw(spriteBatch);

                        foreach (EnemyPirate e in enemyList)
                        {
                            e.Draw(spriteBatch);
                        }

                        playerShip.Draw(spriteBatch);
                        HUD.Draw(spriteBatch);
                        break;
                    }
                case State.Menu:
                    {
                        backGround.Draw(spriteBatch);
                        spriteBatch.Draw(menuTexture, new Vector2(0, 0), Color.White);
                        break;
                    }

                case State.Boarding:
                    {
                        //maybe end then begin
                        //end playing draw
                        spriteBatch.End();
                        //start with camera
                        spriteBatch.Begin(SpriteSortMode.Deferred,
                        BlendState.AlphaBlend,
                        null, null, null, null,
                        camera.Transform);
                        //maybe
                        
                        //original draw
                        drawMap.Draw(spriteBatch);
                        player.Draw(gameTime, spriteBatch);
                        foreach (EnemyKnight badie in enemyKnights)
                            badie.Draw(gameTime, spriteBatch);
                        // end that
                        platformerHUD.Draw(spriteBatch);
                        break;
                    }

                case State.Gameover:
                    {

                        break;
                    }
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }

        #endregion

        #region platformer action stuff

        protected bool enemyHit()
        {
            foreach (EnemyKnight badie in enemyKnights)
            {
                Rectangle enemyBody = badie.boundingBox;
                Rectangle playerWeaponBox = player.weaponBoundingBox;
                Color[] enemyBodyData = new Color[badie.texture.Width * badie.texture.Height];
                badie.texture.GetData(enemyBodyData);
                Color[] weaponData = new Color[player.weaponTexture.Width * player.weaponTexture.Height];
                player.weaponTexture.GetData(weaponData);
                //Need to rsize all the running textures to be 144 x x202 like the swing textures because when it switches to a larger
                //texture it gices array index out fo bounds exception
                //trying to use the largest texture to define rectangle first. swing1texture in place of texture, nope got a size issue
                if (enemyBody.Intersects(playerWeaponBox))
                {
                    if (IntersectPixels(playerWeaponBox, weaponData, enemyBody, enemyBodyData)) { return true; }
                }
    
            }
            return false;
        }

        protected Vector2 enemyHitTwoD()
        {
            Vector2 collPoint = new Vector2(0,0);
            foreach (EnemyKnight badie in enemyKnights)
            {
                Matrix enemyBodyMat = Matrix.Identity;
                Matrix playerWeaponMat = Matrix.CreateTranslation(13, 120, 0)
                    * Matrix.CreateRotationZ(player.weaponRotation)
                    * Matrix.CreateTranslation(player.weaponPosition.X, player.weaponPosition.Y, 0);
                Color[,] enemyBodyData = badie.enemyColorArray;

                Vector2 eneCollisionPoint = TexturesCollide(playerWeaponData, playerWeaponMat, enemyBodyData, enemyBodyMat);
                collPoint = eneCollisionPoint;
            }
            return collPoint;
                /**
                 * Let’s rewrite the correct way of transformations, this time in matrices. Note that we’ve already left away the Identity matrix as first step:
                    First: Matrix.CreateTranslation(rocketPosition.X, rocketPosition.Y, 0)
                    Then : Matrix.CreateScale(rocketScaling)
                    Then : Matrix.CreateRotationZ(rocketAngle)
                    Finally: Matrix.CreateTranslation(-42, -240, 0)
                    And since we need to combine all of them together, we multiply them. Note that ”*” means “after”:
                    Matrix rocketMat = Matrix.CreateTranslation(-42, -240, 0) * Matrix.CreateRotationZ(rocketAngle) * Matrix.CreateScale(rocketScaling) * Matrix.CreateTranslation(rocketPosition.X, rocketPosition.Y, 0);
                    This is the most general transformation you’ll find when creating any 2D game. As an example: the matrices of the foreground and carriages are simplifications of the line above. Based on your SpriteBatch.Draw line, you can create this line. The leftmost matrix contains the origin point you specified in the SpriteBatch.Draw method, then the angle, next the scaling and finally the position on the screen, which you specified as second argument in the SpriteBatch.Draw call.
                 **/
            /**
             * We create the transformation matrices for our rocket and foreground textures, and pass them together with their color arrays to the TexturesCollide method. This method will return (-1,-1) if no collision was detected, otherwise the screen coordinate of the collision will be returned. The result of this method will be returned by the CheckTerrainCollision method.
             **/
        }

        protected Vector2 enemyHitFinal()
        {
            Matrix playerWeaponMat = Matrix.CreateTranslation(13, 120, 0)
                    * Matrix.CreateRotationZ(player.weaponRotation)
                    * Matrix.CreateTranslation(player.weaponPosition.X, player.weaponPosition.Y, 0);

            for (int i = 0; i < enemyKnights.Count; i++)
            {
                EnemyKnight badie = enemyKnights[i];
                
                if (badie.isAlive)
                {
                    Color[,] enemyBodyData = badie.enemyColorArray;
                    int xPos = (int)badie.position.X;
                    int yPos = (int)badie.position.Y;

                    Matrix enemyBodyMat = Matrix.CreateTranslation(xPos, yPos, 0);
                    Vector2 enemyCollisionPoint = TexturesCollide(playerWeaponData, playerWeaponMat, enemyBodyData, enemyBodyMat);

                    //Code for what happens if there is a collision bwtween player weapon and enemy
                    if (enemyCollisionPoint.X > -1)
                    {
                        badie.hitByPlayerPoint = enemyCollisionPoint;
                        return enemyCollisionPoint;
                    }
                }
            }
            return new Vector2(-1, -1);
        }

        private Vector2 TexturesCollide(Color[,] tex1, Matrix mat1, Color[,] tex2, Matrix mat2)
        {
            Matrix mat1to2 = mat1 * Matrix.Invert(mat2);
            int width1 = tex1.GetLength(0);
            int height1 = tex1.GetLength(1);
            int width2 = tex2.GetLength(0);
            int height2 = tex2.GetLength(1);

            for (int x1 = 0; x1 < width1; x1++)
            {
                for (int y1 = 0; y1 < height1; y1++)
                {
                    Vector2 pos1 = new Vector2(x1, y1);
                    Vector2 pos2 = Vector2.Transform(pos1, mat1to2);

                    int x2 = (int)pos2.X;
                    int y2 = (int)pos2.Y;
                    if ((x2 >= 0) && (x2 < width2))
                    {
                        if ((y2 >= 0) && (y2 < height2))
                        {
                            if (tex1[x1, y1].A > 0)
                            {
                                if (tex2[x2, y2].A > 0)
                                {
                                    Vector2 screenPos = Vector2.Transform(pos1, mat1);
                                    return screenPos;
                                }
                            }
                        }
                    }
                }
            }

            return new Vector2(-1, -1);
        }


        static bool eIntersectPixel(Rectangle rect1, Color[] data1, Rectangle rect2, Color[] data2)
        {
            int top = Math.Max(rect1.Top, rect2.Top);
            int bottom = Math.Min(rect1.Bottom, rect2.Bottom);
            int left = Math.Max(rect1.Left, rect2.Left);
            int right = Math.Max(rect1.Right, rect2.Right);

            for (int y = top; y < bottom; y++)
            {
                for (int x = left; x < right; x++)
                {
                    Color color1 = data1[(x - rect1.Left) + (y - rect1.Top) * rect1.Width];
                    Color color2 = data2[(x - rect2.Left) + (y - rect2.Top) * rect2.Width];

                    if (color1.A != 0 && color2.A != 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        //potentially but not a garen
        private bool IntersectPixels(Rectangle rectangleA, Color[] dataA, Rectangle rectangleB, Color[] dataB)
        {
            int top = Math.Max(rectangleA.Top, rectangleB.Top);
            int bottom = Math.Min(rectangleA.Bottom, rectangleB.Bottom);
            int left = Math.Max(rectangleA.Left, rectangleB.Left);
            int right = Math.Min(rectangleA.Right, rectangleB.Right);

            for (int y = top; y < bottom; y++)
            {
                for (int x = left; x < right; x++)
                {
                    Color colorA = dataA[(x - rectangleA.Left) +
                                (y - rectangleA.Top) * rectangleA.Width];
                    Color colorB = dataB[(x - rectangleB.Left) +
                                (y - rectangleB.Top) * rectangleB.Width];

                    if (colorA.A != 0 && colorB.A != 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        #endregion

        #region ShipStuff
        public void LoadEnemy()
        {
            //int randY = random.Next(300, 400);
            //int randX = random.Next(650, 750);

            if (enemyList.Count() < 3)
            {
                enemyList.Add(new EnemyPirate(Content.Load<Texture2D>("shipMaybe"), new Vector2(1000, 320), Content.Load<Texture2D>("cannonBall")));
            }

            for (int i = 0; i < enemyList.Count; i++)
            {
                if (!enemyList[i].isVisible)
                {
                    enemyList.RemoveAt(i);
                    i--;
                }
            }
        }

        #endregion

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
    }
}
