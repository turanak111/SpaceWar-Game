
using GameNamespace;
using Raylib_cs;
using SpaceWarr;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SpaceWarr
{
    internal class Game
    {
        public enum GameState
        {
            Playing,
            GameOver,
            MainMenu,
            Scoreboard
        }
        ScoreBoard scoreBoard = new ScoreBoard();
        List<Enemy> enemies;
        CollisionDetector collisionDetector;
        Spaceship spaceship;
        public const int screenWidth = 1366;
        public const int screenHeight = 768;
        float timer = 0;
        double startTime;
        float spawntimer = 3.5f;
        private Music backgroundMusic;
        private bool isMusicPlaying;
        private float musicVolume = 0.5f;
        double elapsedTime;
        Texture2D gamebackground;
        Texture2D mainmenubackground;
        bool bossadded = false;
        BossEnemy boss;
        Vector2 spawn = new Vector2(screenWidth / 2, 50);
        bool savescorebool = true;
        static GameState currentGameState = GameState.MainMenu;
        private string playerName = "";
        private bool isNameEntered = false;
        private readonly int MAX_NAME_LENGTH = 15;
        private Rectangle nameInputBox;
        private bool isInputActive = false;
        private float inputBoxBlink = 0;
        bool calculatetime = true;
        bool winbool = false;

        public void StartGame()
        {
            Raylib.SetTargetFPS(60);
            Raylib.InitWindow(Game.screenWidth, Game.screenHeight, "SpaceWar");
            enemies = new List<Enemy>();
            Raylib.InitAudioDevice();
            backgroundMusic = Raylib.LoadMusicStream("music.mp3");
            Raylib.SetMusicVolume(backgroundMusic, musicVolume);
            Raylib.PlayMusicStream(backgroundMusic);
            isMusicPlaying = true;
            collisionDetector = new CollisionDetector();
            spaceship = new Spaceship();
            spaceship.LoadSpaceshipTexture();
            gamebackground = Raylib.LoadTexture("GameBackground.png");
            mainmenubackground = Raylib.LoadTexture("MainMenuBackground.png");
            nameInputBox = new Rectangle(
            screenWidth / 2 - 100,
              screenHeight / 2 + 50,
                  200,
                  30
                      );

            UpdateGame();

        }
        void UpdateGame()
        {

            while (!Raylib.WindowShouldClose())
            {
                
                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.RayWhite);
                UpdateMusic();
                if (currentGameState == GameState.Playing)
                {

                    Raylib.DrawTexturePro(
                    gamebackground,
                    new Rectangle(0, 0, gamebackground.Width, gamebackground.Height),
                  new Rectangle(0, 0, screenWidth, screenHeight),
                  new Vector2(0, 0),
                    0,
                    Color.White
                   );

                    if (currentGameState == GameState.Playing)

                        spaceship.PlayerSpaceShipMove();
                    spaceship.Shoot();
                    CreateEnemies();
                    enemies.RemoveAll(Enemy => Enemy.Dead);
                    spaceship.DrawHealthAndLevel();
                    CreateCollects();
                    spaceship.collect.RemoveAll(Collect => Collect.isCollected);
                    collisionDetector.CheckCollectCollision(spaceship, spaceship.collect);
                    CalculateTime();
                    if (bossadded && !boss.Dead) EnemyBullet(boss);
                    if (winbool) GameOver();
                }
                else if (currentGameState == GameState.GameOver)
                {

                    Raylib.DrawTexturePro(
                    gamebackground,
                    new Rectangle(0, 0, gamebackground.Width, gamebackground.Height),
                  new Rectangle(0, 0, screenWidth, screenHeight),
                  new Vector2(0, 0),
                    0,
                    Color.White
                   );
                    if (winbool)
                    {
                        Raylib.DrawText("YOU WIN", screenWidth / 2 - 120, screenHeight / 2 - 50, 50, Color.Green);
                    }
                    else
                    {
                        Raylib.DrawText("GAME OVER", screenWidth / 2 - 170, screenHeight / 2 - 50, 50, Color.Red);
                    }

                    Raylib.DrawText(playerName + " YOUR SCORE: " + (int)(spaceship.score / elapsedTime * 100), screenWidth / 2 - 300, screenHeight / 2 - 100, 50, Color.Yellow);
                    Raylib.DrawText("Press ENTER to return to the Main Menu", screenWidth / 2 - 200, screenHeight / 2, 20, Color.White);


                    if (savescorebool)
                    {
                        if (boss.Dead) spaceship.score += 1000;
                        scoreBoard.SaveScore(playerName, (int)(spaceship.score / elapsedTime * 100));
                        savescorebool = false;
                    }
                    if (Raylib.IsKeyPressed(KeyboardKey.Enter))
                    {
                        ResetGame();
                        currentGameState = GameState.MainMenu;
                    }
                    scoreBoard.DrawScoreboardButton();
                    scoreBoard.DrawScoreboardPopup();

                }
                else if (currentGameState == GameState.MainMenu)
                {

                    DrawMainMenu();
                    UpdateMainMenu();
                }

                Raylib.EndDrawing();
            }

            spaceship.Unload();
            Raylib.CloseWindow();
        }
        private void UpdateMusic()
        {

            Raylib.UpdateMusicStream(backgroundMusic);


            if (Raylib.IsKeyPressed(KeyboardKey.M))
            {
                if (isMusicPlaying)
                {
                    Raylib.PauseMusicStream(backgroundMusic);
                    isMusicPlaying = false;
                }
                else
                {
                    Raylib.ResumeMusicStream(backgroundMusic);
                    isMusicPlaying = true;
                }
            }

            // Opsiyonel: Ses seviyesi kontrolü
            if (Raylib.IsKeyDown(KeyboardKey.Up) && musicVolume < 1.0f)
            {
                musicVolume += 0.01f;
                Raylib.SetMusicVolume(backgroundMusic, musicVolume);
            }
            else if (Raylib.IsKeyDown(KeyboardKey.Down) && musicVolume > 0.0f)
            {
                musicVolume -= 0.01f;
                Raylib.SetMusicVolume(backgroundMusic, musicVolume);
            }
        }
        private void DrawMainMenu()
        {
            Raylib.DrawTexturePro(
                mainmenubackground,
                new Rectangle(0, 0, mainmenubackground.Width, mainmenubackground.Height),
                new Rectangle(0, 0, screenWidth, screenHeight),
                new Vector2(0, 0),
                0,
                Color.White
            );

            Raylib.DrawText("SPACE WAR", screenWidth / 2 - 250, screenHeight / 2 - 100, 80, Color.Yellow);

            if (!isNameEntered)
            {

                Raylib.DrawRectangleRec(nameInputBox, Color.DarkGray);
                Raylib.DrawRectangleLinesEx(nameInputBox, 2, isInputActive ? Color.Yellow : Color.White);

                Raylib.DrawText("Enter Your Name:",
                    (int)(nameInputBox.X),
                    (int)(nameInputBox.Y - 20),
                    20,
                    Color.White);


                Raylib.DrawText(playerName,
                    (int)(nameInputBox.X + 10),
                    (int)(nameInputBox.Y + 8),
                    20,
                    Color.White);

                if (isInputActive)
                {
                    inputBoxBlink += Raylib.GetFrameTime();
                    if ((inputBoxBlink % 1.0f) < 0.5f)
                    {
                        float cursorX = nameInputBox.X + 10 + Raylib.MeasureText(playerName, 20);
                        Raylib.DrawRectangle(
                            (int)cursorX,
                            (int)(nameInputBox.Y + 5),
                            2,
                            20,
                            Color.White
                        );
                    }
                }
            }
            else
            {

                Raylib.DrawText($"Welcome, {playerName}!",
                    screenWidth / 2 - 120,
                    screenHeight / 2,
                    20,
                    Color.White);

                Raylib.DrawText("Press ENTER to Start",
                    screenWidth / 2 - 120,
                    screenHeight / 2 + 30,
                    20,
                    Color.White);
            }
        }
        private void UpdateMainMenu()
        {
            if (currentGameState == GameState.MainMenu)
            {

                if (Raylib.IsMouseButtonPressed(MouseButton.Left))
                {
                    Vector2 mousePoint = Raylib.GetMousePosition();
                    isInputActive = Raylib.CheckCollisionPointRec(mousePoint, nameInputBox);
                }


                if (isInputActive && !isNameEntered)
                {
                    int key = Raylib.GetCharPressed();

                    while (key > 0)
                    {
                        if ((key >= 32) && (key <= 125) && (playerName.Length < MAX_NAME_LENGTH))
                        {
                            playerName += (char)key;
                        }
                        key = Raylib.GetCharPressed();
                    }


                    if (Raylib.IsKeyPressed(KeyboardKey.Backspace) && playerName.Length > 0)
                    {
                        playerName = playerName.Remove(playerName.Length - 1);
                    }


                    if (Raylib.IsKeyPressed(KeyboardKey.Enter) && playerName.Length > 0)
                    {
                        isNameEntered = true;
                        isInputActive = false;
                    }
                }

                else if (isNameEntered && Raylib.IsKeyPressed(KeyboardKey.Enter))
                {
                    currentGameState = GameState.Playing;
                }
            }
        }

        void CalculateTime()
        {
            if (calculatetime)
            {
                startTime = Raylib.GetTime();
                calculatetime = false;
            }
            elapsedTime = Raylib.GetTime() - startTime;
            double elapsedTimeforboss = Raylib.GetTime() - startTime;
            int minutes = (int)elapsedTime / 60;
            int seconds = (int)elapsedTime % 60;
            if (elapsedTimeforboss > 10 && bossadded == false)
            {
                bossadded = true;
                CallBoss();

            }
            if (elapsedTime > 180 && winbool == false)
            {
                winbool = true;
            }



            string timerText = string.Format("{0:D2}:{1:D2}", minutes, seconds);


            Raylib.DrawText(timerText, screenWidth / 2 - Raylib.MeasureText(timerText, 20) / 2, 20, 30, Color.White);
        }



        void CreateEnemies()
        {
            Random random = new Random();
            float deltatime = Raylib.GetFrameTime();
            timer += deltatime;

            if (timer > spawntimer)
            {
                int enemyType = random.Next(0, 3);
                int randomx = Raylib.GetRandomValue(0, 800);
                int randomy;
                int randoma = Raylib.GetRandomValue(0, 1);
                if (randoma == 0)
                { randomy = Raylib.GetRandomValue(0, -100); }
                else
                { randomy = Raylib.GetRandomValue(screenHeight, screenHeight + 100); }
                Vector2 spawnPosition = new Vector2(randomx, randomy);
                if (spawntimer > 1)
                { spawntimer -= 0.05f; }


                Enemy newEnemy;
                switch (enemyType)
                {
                    case 1: newEnemy = new FastEnemy(spawnPosition, 80, spaceship, spaceship.collect); break;
                    case 2: newEnemy = new StrongEnemy(spawnPosition, 150, spaceship, spaceship.collect); break;
                    default: newEnemy = new BasicEnemy(spawnPosition, 100, spaceship, spaceship.collect); break;
                }

                enemies.Add(newEnemy);
                timer = 0;
            }

            foreach (var enemy in enemies)
            {
                enemy.Move();
                enemy.Draw();
                CheckCollisions(enemy);



            }

        }
        void CheckCollisions(Enemy enemy)
        {
            collisionDetector.CheckBulletCollision(spaceship.bullets, enemy);
            collisionDetector.CheckCollision(spaceship, enemy);


        }
        void EnemyBullet(BossEnemy boss)
        {

            collisionDetector.CheckEnemyBulletCollision(boss.bulletForEnemies, spaceship);
            boss.EnemyShoot();
        }
        public static void GameOver()
        {
            currentGameState = GameState.GameOver;

        }
        void CreateCollects()
        {
            foreach (var c in spaceship.collect)
            {
                c.CreateCollect();
            }
        }
        void CallBoss()
        {

            boss = new BossEnemy(spawn, 500, spaceship, spaceship.collect);
            enemies.Add(boss);
        }
        void ResetGame()
        {
            calculatetime = true;
            startTime = Raylib.GetTime();
            spaceship = new Spaceship();
            spaceship.LoadSpaceshipTexture();
            enemies.Clear();
            timer = 0;
            spawntimer = 3.5f;
            bossadded = false;
            winbool = false;
        }







    }


    internal class Program
    {
        public static void Main(string[] args)
        {
            Game game = new Game();
            game.StartGame();

        }
    }
}