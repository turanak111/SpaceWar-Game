using Raylib_cs;

using SpaceWarr;
using System;
using System.Numerics;

namespace GameNamespace
{
    public abstract class Enemy
    {
        protected Spaceship targetSpaceship;

        public Vector2 position;
        public float health;
        public float damage;
        public float speed;
        public bool Dead;
        public List<Collect> collect;
        Game game;




        public abstract Texture2D Texture { get; }
        public abstract Circle HitboxEnemy { get; }

        public Enemy(Vector2 spawnPosition, float maxHealth, Spaceship spaceship, List<Collect> collect)
        {
            position = spawnPosition;
            health = maxHealth;
            targetSpaceship = spaceship;
            this.collect = collect;
        }


        public void TakeDamage(float damage)
        {
            health -= damage;


            if (health <= 0)
                Destroy();
        }
        public Vector2 find_shortest_path(Vector2 target_position)
        {

            float distance = MathF.Sqrt(MathF.Pow(target_position.X - position.X, 2) + MathF.Pow(target_position.Y - position.Y, 2));
            Vector2 direction = (target_position - position) / distance;
            return direction;


        }
        public void Destroy()
        {

            Collect newCollect = new Collect();
            collect.Add(newCollect);
            newCollect.FindPositionofCollect(position);
            Dead = true;
            Console.WriteLine("Enemy destroyed.");
        }

        public void Draw()
        {
            float scale = 2f;
            Raylib.DrawTexturePro(
                Texture,
                new Rectangle(0, 0, Texture.Width, Texture.Height),
                new Rectangle(position.X, position.Y, Texture.Width * scale, Texture.Height * scale),
                new Vector2(Texture.Width * scale / 2.0f, Texture.Height * scale / 2.0f),
                MathF.Atan2(targetSpaceship.position.Y - position.Y, targetSpaceship.position.X - position.X) * (180 / MathF.PI) + 90,
                Color.White
            );
        }

        // soyut hareket ve saldırı metotları
        public abstract void Move();
        public abstract void Attack();


    }

    public class BasicEnemy : Enemy
    {
        private static Texture2D BasicEnemyTexture = Raylib.LoadTexture("basicenemy.png");

        public override Texture2D Texture => BasicEnemyTexture;

        Circle BasicEnemyHitbox;

        public override Circle HitboxEnemy => BasicEnemyHitbox;
        public BasicEnemy(Vector2 startPosition, float startHealth, Spaceship spaceship, List<Collect> collect)
            : base(startPosition, startHealth, spaceship, collect)
        {
            speed = 2f;
            damage = 10f;


        }


        public override void Move()
        {

            Vector2 direction = find_shortest_path(targetSpaceship.position);
            position += direction * speed;
            BasicEnemyHitbox = new Circle(position, 38);


        }

        public override void Attack()
        {

        }
    }

    public class FastEnemy : Enemy
    {
        private static Texture2D FastEnemyTexture = Raylib.LoadTexture("fastenemy.png");
        public override Texture2D Texture => FastEnemyTexture;
        Circle FastEnemyHitbox;
        public override Circle HitboxEnemy => FastEnemyHitbox;

        Spaceship spaceship = new Spaceship();

        public FastEnemy(Vector2 startPosition, float startHealth, Spaceship spaceship, List<Collect> collect)
            : base(startPosition, startHealth, spaceship, collect)
        {

            speed = 3f;
            damage = 5f;
        }

        public override void Move()
        {
            Vector2 direction = find_shortest_path(targetSpaceship.position);
            position += direction * speed;
            FastEnemyHitbox = new Circle(position, 38);

        }

        public override void Attack()
        {

        }
    }

    public class StrongEnemy : Enemy
    {
        Spaceship spaceship = new Spaceship();
        private static Texture2D StrongEnemyTexture = Raylib.LoadTexture("strongenemy.png");
        public override Texture2D Texture => StrongEnemyTexture;
        Circle StrongEnemyHitbox;
        public override Circle HitboxEnemy => StrongEnemyHitbox;

        public StrongEnemy(Vector2 startPosition, float startHealth, Spaceship spaceship, List<Collect> collect)
            : base(startPosition, startHealth, spaceship, collect)
        {
            speed = 1.5f;
            damage = 20f;
        }

        public override void Move()
        {
            Vector2 direction = find_shortest_path(targetSpaceship.position);
            position += direction * speed;
            StrongEnemyHitbox = new Circle(position, 38);

        }

        public override void Attack()
        {

        }
    }

    public class BossEnemy : Enemy
    {

        Spaceship spaceship = new Spaceship();
        private static Texture2D BossEnemyTexture = Raylib.LoadTexture("bossenemy.png");
        public override Texture2D Texture => BossEnemyTexture;
        Circle BossEnemyHitbox;
        public List<BulletForEnemy> bulletForEnemies = new List<BulletForEnemy>();
        public override Circle HitboxEnemy => BossEnemyHitbox;

        float deltatime = Raylib.GetFrameTime();
        float timer = 0;
        float bullethaste = 1;
        public BossEnemy(Vector2 startPosition, float startHealth, Spaceship spaceship, List<Collect> collect)
            : base(startPosition, startHealth, spaceship, collect)
        {
            Vector2 direction = find_shortest_path(targetSpaceship.position);
            position += direction * speed;
        }
        public void EnemyShoot()
        {
            timer += deltatime;
            if (timer > bullethaste)
            {
                BulletForEnemy newEnemyBullet = new BulletForEnemy(position, MathF.Atan2(targetSpaceship.position.Y - position.Y, targetSpaceship.position.X - position.X) * (180 / MathF.PI) + 90);

                bulletForEnemies.Add(newEnemyBullet);
                timer = 0;
            }
            bulletForEnemies.RemoveAll(bulletforenemies => bulletforenemies.Destroyed);
            foreach (var bullet in bulletForEnemies)
            {
                bullet.Draw();
                bullet.Update();
            }
        }


        public override void Move()
        {
            BossEnemyHitbox = new Circle(position, 100);

        }

        public override void Attack()
        {

        }
    }
}
