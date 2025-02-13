using Raylib_cs;
using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace SpaceWarr
{
    public class BulletForEnemy
    {
        private static Texture2D bulletTexture = Raylib.LoadTexture("bulletenemy.png");
        public Vector2 position;
        public static float speed = 3f;
        public static float damage = 15;
        float direction;
        float timerForDestroyBullet;
        public float bullethitboxradius = 15;
        public bool Destroyed { get; private set; }
        public BulletForEnemy(Vector2 enemyshipPosition, float enemyshiprotation)
        {
            position = enemyshipPosition;
            direction = enemyshiprotation - 90f;
            Destroyed = false;

        }

        public void Update()
        {

            float deltaTime = Raylib.GetFrameTime();
            timerForDestroyBullet += deltaTime;
            if (timerForDestroyBullet > 6)
            {
                Destroyed = true;
            }
            Move();
        }
        public void Draw()
        {

            float scale = 0.07f;
            Raylib.DrawTexturePro(
                bulletTexture,
                new Rectangle(0, 0, bulletTexture.Width, bulletTexture.Height),
                new Rectangle(position.X, position.Y, bulletTexture.Width * scale, bulletTexture.Height * scale),
                new Vector2((bulletTexture.Width * scale) / 2.0f, (bulletTexture.Height * scale) / 2.0f),
                direction,
                Color.White
            );
            Circle bullethitbox = new Circle(position, bullethitboxradius);
            bullethitbox.Draw();

        }
        void Move()
        {
            if (!Destroyed)
            {
                position += new Vector2(
                    (float)Math.Cos(direction * Math.PI / 180f) * speed,
                    (float)Math.Sin(direction * Math.PI / 180f) * speed
                );
            }
        }

        public void OnHit()
        {
            if (!Destroyed)
            {
                Destroyed = true;
            }


        }
    }
}