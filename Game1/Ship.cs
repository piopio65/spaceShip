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
    class Ship
    {
        
        public static Vector2 defaultPosition = new Vector2(640, 360);
        public Vector2 position = defaultPosition;
        // 300 pixel /s
        public int speed = 300;
        private KeyboardState kStateOld;
        private int radius = 34;

        // move the ship
        public void shipUpdate(GameTime gametime, Controller con)
        {
            float dt = (float)gametime.ElapsedGameTime.TotalSeconds;

            KeyboardState kState = Keyboard.GetState();
            if (con.inGame)
            {
                if (kState.IsKeyDown(Keys.Right) && position.X < 1280)
                    position.X += speed * dt;
                if (kState.IsKeyDown(Keys.Left) && position.X > 0f)
                    position.X -= speed * dt;
                if (kState.IsKeyDown(Keys.Up) && position.Y > 0f)
                    position.Y -= speed * dt;
                if (kState.IsKeyDown(Keys.Down) && position.Y < 720)
                    position.Y += speed * dt;

                // weapons
                if (kState.IsKeyDown(Keys.Space) && kStateOld.IsKeyUp(Keys.Space))
                {
                    Weapon.Weapons.Add(new Fire(new Vector2(position.X + radius,position.Y)));
                }
                else if (kState.IsKeyDown(Keys.LeftControl) && kStateOld.IsKeyUp(Keys.LeftControl))
                {
                    Weapon.Weapons.Add(new Rocket(new Vector2(position.X + 32, position.Y - 32)));
                    Weapon.Weapons.Add(new Rocket(new Vector2(position.X + 32, position.Y + 32)));
                }
                kStateOld = kState;

            }
        }


    }
}
