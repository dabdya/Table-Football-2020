using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Windows.Forms;
using System.Media;
using System.Threading;

namespace TableFootball
{
    public partial class GameForm : Form
    {
        private readonly Game game;
        private readonly Controls controls = new Controls();
        private readonly Dictionary<string, Bitmap> bitmaps
            = new Dictionary<string, Bitmap>();
        private readonly HashSet<Keys> pressedKeys = new HashSet<Keys>();
        private Keys keyPressed;
        private System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer
        {
            Interval = 15
        };
        private System.Windows.Forms.Timer timerUi = new System.Windows.Forms.Timer
        {
            Interval = 15
        };

        public GameForm()
        {
            game = new Game(1200, 720);
            ClientSize = new Size(game.Width, game.Height);
            FormBorderStyle = FormBorderStyle.FixedDialog;

            var imagesDirectory = new DirectoryInfo("images");
            foreach (var file in imagesDirectory.GetFiles("*.png"))
                bitmaps[file.Name] = (Bitmap)Image.FromFile(file.FullName);
            foreach (var file in imagesDirectory.GetFiles("*.jpg"))
                bitmaps[file.Name] = (Bitmap)Image.FromFile(file.FullName);


            timer.Tick += TimerTick;
            timerUi.Tick += TimerTick1;
            game.Start();
            timer.Start();
            timerUi.Start();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Text = "TableFootball";
            DoubleBuffered = true;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            pressedKeys.Add(e.KeyCode);
            keyPressed = e.KeyCode;
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            pressedKeys.Remove(e.KeyCode);
            keyPressed = pressedKeys.Any() ? pressedKeys.Min() : Keys.None;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var boardImage = "board.jpg";
            var userFootballerImage = "user_footballer.png";
            var userFootballerTurned = "user_turned.png";
            var aiFootballerImage = "ai_footballer.png";
            var ballImage = "ball.png";
            var smallBall = "small_ball.png";
            var ghostBall = "ghost_ball.png";
            var bonus = "bonus.png";
            var freezeUser = "frezee_user.png";
            var freezeAi = "frezee_ai.png";

            e.Graphics.DrawImage(bitmaps[boardImage], 0, 0, game.Width, game.Height);

            foreach (var footballer in game.UserFootballers)
            {
                if (game.Sleep)
                    e.Graphics.DrawImage(bitmaps[freezeUser],
                    (float)footballer.Position.X, (float)footballer.Position.Y,
                    footballer.Position.Width, footballer.Position.Height);
                else
                    e.Graphics.DrawImage(bitmaps[userFootballerImage],
                    (float)footballer.Position.X, (float)footballer.Position.Y,
                    footballer.Position.Width, footballer.Position.Height);
            }

            foreach (var footballer in game.AiFootballers)
            {
                if(game.AI.Sleep)
                    e.Graphics.DrawImage(bitmaps[freezeAi],
                (float)footballer.Position.X, (float)footballer.Position.Y,
                footballer.Position.Width, footballer.Position.Height);
                else 
                    e.Graphics.DrawImage(bitmaps[aiFootballerImage],
                    (float)footballer.Position.X, (float)footballer.Position.Y,
                    footballer.Position.Width, footballer.Position.Height);
            }

            if (game.Ball.IsSmall)
                e.Graphics.DrawImage(bitmaps[smallBall],
                (float)game.Ball.Position.X, (float)game.Ball.Position.Y,
                game.Ball.Position.Width, game.Ball.Position.Height);
            else if(game.Ball.IsGhost)
                    e.Graphics.DrawImage(bitmaps[ghostBall],
                (float)game.Ball.Position.X, (float)game.Ball.Position.Y,
                game.Ball.Position.Width, game.Ball.Position.Height);
            else
                e.Graphics.DrawImage(bitmaps[ballImage],
                    (float)game.Ball.Position.X, (float)game.Ball.Position.Y,
                    game.Ball.Position.Width, game.Ball.Position.Height);

            foreach (var b in game.Bonuses)
                e.Graphics.DrawImage(bitmaps[bonus], (float)b.Position.X, (float)b.Position.Y,
                    b.Position.Width, b.Position.Height);

            e.Graphics.DrawRectangle(new Pen(Color.Black), 370, 0, 150, 45);
            e.Graphics.FillRectangle(Brushes.DarkOrange, 371, 0, 150, 45);
            e.Graphics.DrawRectangle(new Pen(Color.Black), 520, 0, 150, 45);
            e.Graphics.FillRectangle(Brushes.Gray, 520, 0, 150, 45);
            e.Graphics.DrawRectangle(new Pen(Color.Black), 670, 0, 150, 45);
            e.Graphics.FillRectangle(Brushes.ForestGreen, 670, 0, 150, 45);


            e.Graphics.DrawString(game.AiScore.ToString(), new Font("MV Boli", 20), Brushes.Black, 595, 5);
            e.Graphics.DrawString("User", new Font("MV Boli", 20), Brushes.Black, 410 , 5);
            e.Graphics.DrawString("AI", new Font("MV Boli", 20), Brushes.Black, 720, 5);
            e.Graphics.DrawString(game.UserScore.ToString() + ":", new Font("MV Boli", 20), Brushes.Black, 565, 5);           
        }

        private void TimerTick1(object sender, EventArgs args)
        {
            if (keyPressed == Keys.P)
            {
                timer.Stop();
            }
            if (keyPressed == Keys.R)
            {
                timer.Start();
            }

        }
        private void TimerTick(object sender, EventArgs args)
        {
            if (game.State == GameState.End)
                Application.Restart();
            if (keyPressed == Keys.Escape)
            {
                Application.Exit();
                return;
            }
            if (controls.ControlsKeys.ContainsKey(keyPressed))
                game.ActionsHandler(controls.ControlsKeys[keyPressed]);
            else 
                game.ActionsHandler();

            Invalidate();
        }

        private void GameForm_Load(object sender, EventArgs e)
        {

        }

        private void GameForm_Load_1(object sender, EventArgs e)
        {

        }
    }
}
