using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace spaceShip
{
    class Controller
    {
        public List<Asteroid> asteroids=new List<Asteroid>();
        public double timer = 2d;
        public double maxTime = 2d;
        public int speed = 250;
        public int maxSpeed = 300;
        public bool inGame = false;
        public int score = 0;

        public float totalTime = 0f;

        public void conUpdate(GameTime gt)
        {
            if (inGame)
            {
                timer -= gt.ElapsedGameTime.TotalSeconds;
                totalTime+= (float)gt.ElapsedGameTime.TotalSeconds;
            } else
            {
                //
                 

                KeyboardState ksate = Keyboard.GetState();
                if (ksate.IsKeyDown(Keys.Enter))
                {
                    score = 0;
                    inGame = true;
                    totalTime = 0f;
                    speed = 250;
                    maxTime = 2d;
                    timer = 2d;
                }
            }

            if (timer <= 0)
            {
                asteroids.Add(new Asteroid(speed));
                timer = maxTime;
                if (maxTime > 0.5D)
                    maxTime -= 0.1D;
                if (speed < maxSpeed)
                    speed += 4;

            }
           

        }

    }
}
