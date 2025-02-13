using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Raylib_cs;
using System.Numerics;
using SpaceWarr;

namespace SpaceWarr
{
    class ScoreBoard
    {
        private const string SCORE_FILE = "highscores.txt";
        private const int MAX_SCORES = 10;
        private List<(string name, int score)> highScores;
        private bool isVisible;
        private Rectangle scoreboardButton;
        private Rectangle exitButton;

        public ScoreBoard()
        {
            highScores = new List<(string name, int score)>();
            LoadScores();


            scoreboardButton = new Rectangle(
                Game.screenWidth / 2 - 120,
                Game.screenHeight / 2 + 50,
                200,
                40
            );


            exitButton = new Rectangle(
                Game.screenWidth / 2 + 150,
                Game.screenHeight / 2 - 200,
                60,
                30
            );
        }

        private void LoadScores()
        {
            if (!File.Exists(SCORE_FILE))
            {
                File.Create(SCORE_FILE).Close();
                return;
            }

            highScores.Clear();
            string[] lines = File.ReadAllLines(SCORE_FILE);
            foreach (string line in lines)
            {
                string[] parts = line.Split(',');
                if (parts.Length == 2 && int.TryParse(parts[1], out int score))
                {
                    highScores.Add((parts[0], score));
                }
            }
        }

        public void SaveScore(string playerName, int score)
        {
            highScores.Add((playerName, score));
            highScores = highScores.OrderByDescending(x => x.score).Take(MAX_SCORES).ToList();

            // Save to file
            using (StreamWriter writer = new StreamWriter(SCORE_FILE, false))
            {
                foreach (var (name, playerScore) in highScores)
                {
                    writer.WriteLine($"{name},{playerScore}");
                }
            }
        }

        public void DrawScoreboardButton()
        {
            Raylib.DrawRectangleRec(scoreboardButton, Color.DarkGray);
            Raylib.DrawText("View Scoreboard",
                (int)scoreboardButton.X + 20,
                (int)scoreboardButton.Y + 10,
                20,
                Color.White);

            // Check for button click
            if (Raylib.IsMouseButtonPressed(MouseButton.Left))
            {
                Vector2 mousePoint = Raylib.GetMousePosition();
                if (Raylib.CheckCollisionPointRec(mousePoint, scoreboardButton))
                {
                    isVisible = true;
                }
            }
        }

        public void DrawScoreboardPopup()
        {
            if (!isVisible) return;


            Raylib.DrawRectangle(0, 0, Game.screenWidth, Game.screenHeight, new Color(0, 0, 0, 200));
            int popupWidth = 400;
            int popupHeight = 500;
            Rectangle popup = new Rectangle(
                Game.screenWidth / 2 - popupWidth / 2,
                Game.screenHeight / 2 - popupHeight / 2,
                popupWidth,
                popupHeight
            );
            Raylib.DrawRectangleRec(popup, Color.RayWhite);


            Raylib.DrawText("High Scores",
                (int)popup.X + 120,
                (int)popup.Y + 20,
                30,
                Color.Black);

            // Draw scores
            for (int i = 0; i < highScores.Count; i++)
            {
                string scoreText = $"{i + 1}. {highScores[i].name}: {highScores[i].score}";
                Raylib.DrawText(scoreText,
                    (int)popup.X + 50,
                    (int)popup.Y + 80 + (i * 30),
                    20,
                    Color.Black);
            }


            Raylib.DrawRectangleRec(exitButton, Color.Red);
            Raylib.DrawText("X",
                (int)exitButton.X + 25,
                (int)exitButton.Y + 5,
                20,
                Color.White);


            if (Raylib.IsMouseButtonPressed(MouseButton.Left))
            {
                Vector2 mousePoint = Raylib.GetMousePosition();
                if (Raylib.CheckCollisionPointRec(mousePoint, exitButton))
                {
                    isVisible = false;
                }
            }
        }
    }
}