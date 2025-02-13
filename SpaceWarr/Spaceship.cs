using Raylib_cs;
using SpaceWarr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SpaceWarr
{
    public class Spaceship
    {
        public Vector2 position;
        public static float rotation = 0.0f;
        float speed = 3f;
        float health = 100;
        float maxhealth = 100;
        float bullethaste = 0.2f;
        float timer = 0f;
        public float spaceshipradius = 15;
        int level = 1;
        public float exp = 0;
        float exptolevelup = 100;
        Game game = new Game();
        float timertakedamage = 0;
        public List<Collect> collect = new List<Collect>();
        public float score = 0;

        Texture2D playerSpaceship;
        public List<Bullet> bullets = new List<Bullet>();

        public Spaceship()
        {
            position = new Vector2(Game.screenWidth / 2.0f, Game.screenHeight / 2.0f);


        }

        public void LoadSpaceshipTexture()
        {
            playerSpaceship = Raylib.LoadTexture("shipimage.png");

        }
        public void PlayerSpaceShipMove()
        {
            DrawPlayerSpaceShip();
            if (Raylib.IsKeyDown(KeyboardKey.Q)) rotation -= 3f;
            if (Raylib.IsKeyDown(KeyboardKey.E)) rotation += 3f;
            if (Raylib.IsKeyDown(KeyboardKey.W) && position.Y > 10) position.Y -= speed;
            if (Raylib.IsKeyDown(KeyboardKey.S) && position.Y < Game.screenHeight - 10)
                position.Y += speed;
            if (Raylib.IsKeyDown(KeyboardKey.A) && position.X > 10) position.X -= speed;
            if (Raylib.IsKeyDown(KeyboardKey.D) && position.X < Game.screenWidth - 10)
                position.X += speed;
        }
        void DrawPlayerSpaceShip()
        {
            float scale = 1.5f;
            Raylib.DrawTexturePro(
             playerSpaceship,
              new Rectangle(0, 0, playerSpaceship.Width, playerSpaceship.Height),
              new Rectangle(position.X, position.Y, playerSpaceship.Width
              * scale, playerSpaceship.Height * scale),
              new Vector2(playerSpaceship.Width * scale
              / 2.0f, playerSpaceship.Height * scale / 2.0f),
              rotation,
              Color.White

          );



            Circle spaceshiphitbox = new Circle(position, spaceshipradius);



        }
        public void TakeDamage(float amount)
        {
            if (timertakedamage > 0.25f)
            {
                health -= amount;
                timertakedamage = 0;
                Console.WriteLine(health);
            }

            if (health <= 0)
            {
                Game.GameOver();
                Console.WriteLine("öldün");
            }
        }
        public void Shoot()
        {
            float deltatime = Raylib.GetFrameTime();
            timertakedamage += deltatime;
            timer += deltatime;
            if (Raylib.IsKeyDown(KeyboardKey.Space) && timer > bullethaste)
            {
                Bullet newBullet = new Bullet(position, rotation);
                bullets.Add(newBullet);
                timer = 0;
            }
            bullets.RemoveAll(bullet => bullet.Destroyed);
            foreach (var bullet in bullets)
            {
                bullet.Draw();
                bullet.Update();
            }
        }
        public void Unload()
        {
            Raylib.UnloadTexture(playerSpaceship);
        }
        public void DrawHealthAndLevel()
        {
            Raylib.DrawText("Health: " + health, 20, +20, 30, Color.Red);
            Raylib.DrawText("Level : " + level, 20, +Game.screenHeight - 55, 30, Color.Blue);
            Raylib.DrawText("Exp: " + exp + "/" + (int)exptolevelup, 20, +Game.screenHeight - 30, 25, Color.Blue);
        }
        public void PlayerCollected()
        {
            if (health < maxhealth)
            {
                health += 5;
            }
            if (health > maxhealth) { health = maxhealth; }
            exp += 20;
            score += 50;
            if (exp >= exptolevelup)
            {
                LevelUp();

            }

        }

        void IncreaseBulletSpeed()
        {
            bullethaste *= 0.85f;
        }
        void IncreaseBulletDamage()
        {
            Bullet.damage += 2f;
        }

        void IncreaseMovementSpeed()
        {
            speed += 0.875f;
        }
        void LevelUp()
        {

            exp = 0;
            level++;
            exptolevelup *= 1.25f;

            Rectangle button1 = new Rectangle(400, 150, 700, 50);
            Rectangle button2 = new Rectangle(400, 220, 700, 50);
            Rectangle button3 = new Rectangle(400, 290, 700, 50);

            bool choiceMade = false;

            while (!choiceMade)
            {
                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.DarkBlue);
                Raylib.DrawText("LEVEL UP!", 400, 50, 70, Color.White);


                Raylib.DrawRectangleRec(button1, Color.SkyBlue);
                Raylib.DrawText("Increase Bullet Speed", (int)button1.X + 10, (int)button1.Y + 15, 20, Color.Black);

                Raylib.DrawRectangleRec(button2, Color.SkyBlue);
                Raylib.DrawText("Increase Bullet Damage", (int)button2.X + 10, (int)button2.Y + 15, 20, Color.Black);

                Raylib.DrawRectangleRec(button3, Color.SkyBlue);
                Raylib.DrawText("Increase Movement Speed", (int)button3.X + 10, (int)button3.Y + 15, 20, Color.Black);


                if (Raylib.IsMouseButtonPressed(MouseButton.Left))
                {
                    Vector2 mousePos = Raylib.GetMousePosition();

                    if (Raylib.CheckCollisionPointRec(mousePos, button1))
                    {
                        IncreaseBulletSpeed();
                        choiceMade = true;
                    }
                    else if (Raylib.CheckCollisionPointRec(mousePos, button2))
                    {
                        IncreaseBulletDamage();
                        choiceMade = true;
                    }
                    else if (Raylib.CheckCollisionPointRec(mousePos, button3))
                    {
                        IncreaseMovementSpeed();
                        choiceMade = true;
                    }
                }

                Raylib.EndDrawing();
            }
        }




    }

}
