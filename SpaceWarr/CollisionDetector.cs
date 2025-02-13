
using GameNamespace;
using Raylib_cs;
using SpaceWarr;
using System;
using System.Numerics;

namespace SpaceWarr
{
    public class CollisionDetector
    {
        public void CheckBulletCollision(List<Bullet> bullet, Enemy enemy)
        {
            foreach (var b in bullet)
            {
                if (Raylib.CheckCollisionCircles(enemy.HitboxEnemy.Position, enemy.HitboxEnemy.Radius, b.position, b.bullethitboxradius))
                {
                    b.OnHit();
                    enemy.TakeDamage(Bullet.damage);
                }
            }
        }

        public void CheckEnemyBulletCollision(List<BulletForEnemy> bullet, Spaceship spaceship)
        {
            foreach (var b in bullet)
            {
                if (Raylib.CheckCollisionCircles(spaceship.position, spaceship.spaceshipradius, b.position, b.bullethitboxradius))
                {
                    b.OnHit();
                    spaceship.TakeDamage(BulletForEnemy.damage);
                }
            }
        }
        public void CheckCollision(Spaceship spaceship, Enemy enemy)
        {
            if (Raylib.CheckCollisionCircles(enemy.HitboxEnemy.Position, enemy.HitboxEnemy.Radius, spaceship.position, spaceship.spaceshipradius))
            {
                spaceship.TakeDamage(enemy.damage);
            }
        }

        public void CheckCollectCollision(Spaceship spaceship, List<Collect> collect)
        {
            foreach (var c in collect)
            {
                if (Raylib.CheckCollisionCircles(spaceship.position, spaceship.spaceshipradius, c.position, c.collectradius))
                {
                    c.isCollected = true;
                    spaceship.PlayerCollected();
                }
            }

        }
    }

    public class Circle
    {
        public Vector2 Position { get; set; }
        public float Radius { get; set; }

        public Circle(Vector2 position, float radius)
        {
            Position = position;
            Radius = radius;

        }

        public void Draw()
        {
            Raylib.DrawCircleV(Position, Radius, Color.Blue);
        }
    }
}
