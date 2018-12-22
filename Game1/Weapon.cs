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
    class Weapon
    {
        private Vector2 position;
        protected int speed;
        protected int damage;
        protected int radius;
        protected bool isdestroyed = false;
        // temps mis avant explosion lors d'un impact
        protected float timeBeforeExplosion;
        private bool IsDetonatorActivated;
        private static Random rand = new Random();

        private static List<Weapon> weapons = new List<Weapon>();


        // liste de toutes les munitions
        public static List<Weapon> Weapons
        {
            get { return weapons; }
        }


        public bool IsDestroyed
        {
            get { return isdestroyed; }
            set { isdestroyed = value; }
        }

        public Vector2 Position
        {
            get { return position; }
        }

        public int Speed
        {
            get { return speed; }
        }

        public int Damage
        {
            get { return damage; }
        }

        public int Radius
        {
            get { return radius; }
        }


        public Weapon(Vector2 newPos)
        {
            position = newPos;
            IsDetonatorActivated = false;
            
        }

        // quand on tire
        public void Update(GameTime gt)
        {
            float dt = (float)gt.ElapsedGameTime.TotalSeconds;
            position.X += speed * dt;
            if (IsDetonatorActivated)
            {
                timeBeforeExplosion -= dt;
                if (timeBeforeExplosion <= 0f)
                    isdestroyed = true;

            }

        }
        // activation du delai avant destruction d'un tir , lorsqu'on touche 1 asteroid
        // 2 parametres , le premier est le radius de l'objet touché, 
        // le second est la vitesse de l'objet touché.
        public void DelayBeforeExplosion(Asteroid a)
        {
            if (!IsDetonatorActivated)
            {
                IsDetonatorActivated = true;
                // calcul du temps avant explosion
                int sumspeed = speed + a.speed;
                // calcul du temps maxi avant que les 2 objets se separent apres impact
                float maxtime = (float)(a.radius * 2) / (float)sumspeed;
                int max = (int)(maxtime * 1000);
                timeBeforeExplosion = (float)rand.Next(0, max+1)/1000.0f;
                a.resistance -= damage;
            }

        }



    }

    class Fire : Weapon
    {
        public Fire(Vector2 newPos) : base(newPos)
        {
            speed = 700;
            radius = 15;
            damage = 3;

        }
    }

    class Rocket : Weapon
    {
        public Rocket(Vector2 newPos) : base(newPos)
        {
            speed = 800;
            radius = 24;
            damage = 1;
        }
    }



}
