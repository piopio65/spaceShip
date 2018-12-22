using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using System;

namespace spaceShip
{
    
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D ship_Sprite;
        Texture2D asteroid_Sprite;
        Texture2D space_Sprite;

        // tirs
        Texture2D simpleFire_Sprite;
        Texture2D rocket_Sprite;

        SpriteFont gameFont;
        SpriteFont timerFont;

        // musique de fond
        Song background_music;

        

        // player
        Ship player = new Ship();
        
        // Controller
        //AsteroidRand();
        Controller controller = new Controller();


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            
            // center the screen 
            // topic here :
            // http://community.monogame.net/t/how-to-center-gamewindow/9518/4

            Window.Position = new Point(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 2 - graphics.PreferredBackBufferWidth / 2, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 2 - graphics.PreferredBackBufferHeight /2);


            Content.RootDirectory = "Content";
        }

       
        protected override void Initialize()
        {

           

            // permet d'accelerer considerablement l'affichage si les 2 valeurs suivantes
            // sont false
            this.IsFixedTimeStep = true;
            graphics.SynchronizeWithVerticalRetrace = true;
            graphics.ApplyChanges();
           

            base.Initialize();
        }

        
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            // sprites
            ship_Sprite = Content.Load<Texture2D>("ship");
            space_Sprite = Content.Load<Texture2D>("space");
            asteroid_Sprite = Content.Load<Texture2D>("asteroid");

            // Weapons
            simpleFire_Sprite = Content.Load<Texture2D>("simple_fire");
            rocket_Sprite = Content.Load<Texture2D>("rocket");

            // fonts
            gameFont = Content.Load<SpriteFont>("spaceFont");
            timerFont = Content.Load<SpriteFont>("timerFont");

            // music
            background_music = Content.Load<Song>("110-bpm");
            
            
        }

        
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (!controller.inGame)
                 MediaPlayer.Stop();
             else
            {
                if (MediaPlayer.State == MediaState.Stopped)
                    MediaPlayer.Play(background_music);
            }

            player.shipUpdate(gameTime,controller);
            //testAsteroid.asteroidUpdate(gameTime);
            controller.conUpdate(gameTime);
           // for (int i = 0; i < controller.asteroids.Count; i++)
           // {
           //     controller.asteroids[i].asteroidUpdate(gameTime);
           // }
            foreach (Asteroid asteroid in controller.asteroids)
            {
               asteroid.asteroidUpdate(gameTime);
                if (asteroid.position.X < -asteroid.radius)
                {
                    controller.score += 10;
                    asteroid.offscreen = true;
                }
                // radius of the selected asteroid + radius player
                int sum = asteroid.radius + 30;
                if (Vector2.Distance(asteroid.position, player.position) <= sum)
                {
                    controller.inGame = false;
                    player.position = Ship.defaultPosition;
                    controller.asteroids.Clear();
                    break;
                    
                }
            }

            // on regarde les projectiles
            foreach(Weapon weapon in Weapon.Weapons)
            {
                weapon.Update(gameTime);
                // on verifie si un projectile touche 1 asteroid
                foreach (Asteroid asteroid in controller.asteroids)
                {
                  
                        int sum = asteroid.radius + weapon.Radius;
                        if (Vector2.Distance(asteroid.position, weapon.Position) <= sum)
                            // activer le detonateur 
                            weapon.DelayBeforeExplosion(asteroid);

                        if (asteroid.resistance <= 0)
                            asteroid.offscreen = true;
                }
                 
            }
            
            // voir definition sur les prédicats
            try
            {
                controller.asteroids.RemoveAll(ast => ast.offscreen);
                Weapon.Weapons.RemoveAll(weapon => (weapon.IsDestroyed || weapon.Position.X > (1280 +weapon.Radius)));
            }
            catch (Exception)
            { }
            base.Update(gameTime);
        }

        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
                spriteBatch.Draw(space_Sprite, new Vector2(0, 0), Color.White);
                spriteBatch.Draw(ship_Sprite, new Vector2(player.position.X - ship_Sprite.Width/2 ,player.position.Y - ship_Sprite.Height/2), Color.White);
            
            if (!controller.inGame)
            {
                string menuMessage = "Press enter to Begin";
                Vector2 sizeOfText=gameFont.MeasureString(menuMessage);
                spriteBatch.DrawString(gameFont, menuMessage, new Vector2(640 - sizeOfText.X / 2, 360 - sizeOfText.Y / 2), Color.White);

            }
            //spriteBatch.Draw(asteroid_Sprite, new Vector2(testAsteroid.position.X - testAsteroid.getRadiusX(asteroid_Sprite), testAsteroid.position.Y - testAsteroid.getRadiusY(asteroid_Sprite)), Color.White);
            foreach (Asteroid asteroid in controller.asteroids)
            {
                Vector2 tempPos = asteroid.position;
                int tempRadius = asteroid.radius;
                spriteBatch.Draw(asteroid_Sprite, new Vector2(tempPos.X - tempRadius, tempPos.Y - tempRadius), Color.White);
            }

            // affichage des projectiles
            foreach(Weapon w in Weapon.Weapons)
            {
                Texture2D tmpText=null;
                Vector2 tempPos=w.Position;
                
                int xc=0, yc=0;
                if (w.GetType() == typeof(Fire))
                {
                    tmpText = simpleFire_Sprite;
                    xc = 16;
                    yc = 16;
                }
                else if (w.GetType() == typeof(Rocket))
                {
                    tmpText = rocket_Sprite;
                    xc = 32;
                    yc = 16;
                }
                spriteBatch.Draw(tmpText, new Vector2(tempPos.X - xc, tempPos.Y - yc), Color.White);
            }

            // test
            string infos = "Total Time in Game : " + (int)controller.totalTime + "\nTotal asteroids : " + controller.asteroids.Count + "\nTotal Weapons : " + Weapon.Weapons.Count + "\nScore : " + controller.score;
            Vector2 sizeinfo = timerFont.MeasureString(infos);
            spriteBatch.DrawString(timerFont, infos, new Vector2(3, 3), Color.White);
            spriteBatch.DrawString(timerFont, "measure w= " + sizeinfo.X + "  measure h= " + sizeinfo.Y, new Vector2(3, 3 + sizeinfo.Y), Color.White);
            /*
            for (int i=0;i<controller.asteroids.Count;i++)
            {
                Vector2 temppos = controller.asteroids[i].position;
                int tempradius = controller.asteroids[i].radius;
                spriteBatch.Draw(asteroid_Sprite, new Vector2(temppos.X - tempradius, temppos.Y - tempradius), Color.White);
            }
             */   
            
            //spriteBatch.DrawString(timerFont, ((float)gameTime.ElapsedGameTime.TotalSeconds).ToString(), new Vector2(3, 100), Color.White);
            spriteBatch.End();
            
            base.Draw(gameTime);
        }
    }
}
