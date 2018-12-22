using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace spaceShip
{
    class Asteroid
    {
        public Vector2 position;
        public int speed = 220;
        public int radius=59;
        public int resistance = 10;

        public bool offscreen = false;
        
        static Random rand=new Random();

        public Asteroid()
        {

        }

        public Asteroid(int speed)
        {
            this.speed = speed;
            position = new Vector2(1280 + radius, rand.Next(0,721));
        }

        //public static void initRand()
        //{
        //    rand = new Random();
        //}
        
        public void asteroidUpdate(GameTime gt)
        {
            float dt = (float)gt.ElapsedGameTime.TotalSeconds;
            position.X -= speed * dt;
        }


        
        public int getRadiusY (Texture2D asteroid)
        {
            return  asteroid.Height / 2;
        }

        public int getRadiusX(Texture2D asteroid)
        {
            return asteroid.Width / 2;
        }

    }
}
