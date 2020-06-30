using System;
using System.Media;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;

namespace TableFootball
{
    class Game
    {
        public GameState State { get; private set; } = GameState.NotStarted;
        public SoundPlayer soundPlayer = new SoundPlayer(@"sounds\whistle.wav");
        public SoundPlayer hitPlayer = new SoundPlayer(@"sounds\hit.wav");
        public int Width { get; }
        public int Height { get; }
        public bool Sleep;
        public int SleepCount = 300;
        public int borderSize = 50;
        private readonly Dictionary<Action, System.Action> actions;
        public AI AI;
        public List<Bonus> Bonuses = new List<Bonus>();
        public Game(int width, int height)
        {
            Width = width;
            Height = height;
            actions = new Dictionary<Action, System.Action>()
            {
                { Action.MoveUp, MoveUp },
                { Action.MoveDown, MoveDown }
            };
            AI = new AI(this);
        }

        public Ball Ball { get; private set; }
        private Random rnd = new Random();
        private Dictionary<int, double> angles = new Dictionary<int, double>() {
            {0, Math.PI / 3}, {1, Math.PI / 6 }, {2, Math.PI / 4 }
        };

        public void Start()
        {
            State = GameState.Process;
            var ballAngle = rnd.Next(angles.Count);
            Ball = new Ball(new Position(
                Width / 2 - 16, Height / 2 - 16, 32, 32), angles[ballAngle]);
            InitializeTeams();
        }

        public int UserScore { get; private set; }
        public void UserGoal()
        {
            if (Ball.Position.X + Ball.Position.Width < borderSize)
            {
                AiScore++;
                Bonuses.Clear();
                State = GameState.Goal;
                Bonuses.Add(new Bonus(new Position(520, 400, 32, 32)));
                soundPlayer.Play();
                Start();
            }
             
            if (UserScore == 5) State = GameState.End;
        }

        public int AiScore { get; private set; }
        public void AiGoal()
        {
            if (Ball.Position.X > Width - borderSize)
            {
                UserScore++;
                Bonuses.Clear();
                Ball.smallIters = 0;
                Ball.speedIters = 0;
                BallCollisionIters = 0;
                SleepCount = 0;
                State = GameState.Goal;
                Bonuses.Add(new Bonus(new Position(660, 460, 16, 16)));
                soundPlayer.Play();
                Start();
            }

            if (AiScore == 5) State = GameState.End;
        }

        public void Bonus()
        {
            foreach (var bonus in Bonuses.ToArray())
            {
                if (IsBonusCollidedWithPlayers(bonus))
                {
                    var effect = bonus.GetBonus();
                    Bonuses.Remove(bonus);
                    EffectAct(effect);
                }
            }
        }

        private void EffectAct(Effect effect)
        {
            if (effect == Effect.Freeze)
            {
                AI.Sleep = true;
                Sleep = true;
                SleepCount = 300;
            }
            else if (effect == Effect.SpeedBoost)
            {
                Ball.SpeedEffect();
            }
            else if (effect == Effect.SmallBall)
            {
                Ball.SmallEffect();
            }
            else if (effect == Effect.GhostBall)
            {
                TurnOffBallsCollision = true;
                BallCollisionIters = 300;
                Ball.IsGhost = true;
            }
        }

        private bool IsBonusCollidedWithPlayers(Bonus bonus)
        {
            foreach (var footballer in userTeam.Concat(AI.AiFootballers))
            {
                if (Math.Max(bonus.Position.X, footballer.Position.X)
                <= Math.Min(bonus.Position.X + bonus.Position.Width,
                footballer.Position.X + footballer.Position.Width)
                && Math.Max(bonus.Position.Y, footballer.Position.Y)
                <= Math.Min(bonus.Position.Y + bonus.Position.Height,
                footballer.Position.Y + footballer.Position.Height)) return true;            
            }
            return false;
        }

        private List<Footballer> userTeam;
        public IEnumerable<Footballer> UserFootballers 
            => userTeam.AsEnumerable();

        public IEnumerable<Footballer> AiFootballers
            => AI.AiFootballers;

        private void InitializeTeams()
        {
            AI.InitializeTeam();
            userTeam = new List<Footballer>()
            {
                new Footballer(new Position(
                100, Height / 2, 21, 31), 0),
                new Footballer(new Position(
                240, 300, 21, 31), 0),
                new Footballer(new Position(
                240, 420, 21, 31), 0),
                new Footballer(new Position(
                520, 180, 21, 31), 0),
                new Footballer(new Position(
                520, 300, 21, 31), 0),
                new Footballer(new Position(
                520, 420, 21, 31), 0),
                new Footballer(new Position(
                520, 540, 21, 31), 0),
                new Footballer(new Position(
                800, 150, 21, 31), 0),
                new Footballer(new Position(
                800, 360, 21, 31), 0),
                new Footballer(new Position(
                800, 570, 21, 31), 0),
        };
        }

        public void ActionsHandler(params Action[] actions)
        {
            //State = GameState.Process;
            AiGoal();
            UserGoal();          
            MoveBall();
            Bonus();
            AI.Act();
            if (Sleep && SleepCount == 0)
            {
                Sleep = false;
                SleepCount = 300;
            }
            else if (Sleep)
                SleepCount--;
            if (!Sleep)
            {
                foreach (var action in actions)
                    this.actions[action]();
            }                   
        }

        private bool TurnOffBallsCollision;
        private int BallCollisionIters = 300;

        public void MoveBall()
        {
            Ball.Move(Ball.DirectionRadians);
            if (Ball.Position.X + Ball.Position.Width >= Width - borderSize + 5 && !IsGateCollided())
            {
                Ball.Move((Ball.DirectionRadians - Math.PI) * 0.9);
                return;
            }

            if (Ball.Position.X <= borderSize - 5 && !IsGateCollided())
            {
                Ball.Move((Ball.DirectionRadians - Math.PI) * 0.9);
                return;
            }

            if (Ball.Position.Y < borderSize - 5
                || Ball.Position.Y + Ball.Position.Height >= Height - borderSize + 5)
            {
                Ball.Move((Ball.DirectionRadians - Math.PI) * 0.9);
                return;
            }

            if (TurnOffBallsCollision && BallCollisionIters == 0)
            {
                TurnOffBallsCollision = false;
                Ball.IsGhost = false;
                BallCollisionIters = 300;
            }
            else if (TurnOffBallsCollision)
            {
                BallCollisionIters--;
            }
            if (!TurnOffBallsCollision)
            {
                foreach (var footballer in userTeam.Concat(AiFootballers))
                    if (IsBallCollidedWithFootballer(footballer))
                    {
                        hitPlayer.Play();
                        Ball.Move((Ball.DirectionRadians - Math.PI) * 0.8);
                    }
            }


            var rightCondition = Ball.Position.X + Ball.Position.Width > Width - borderSize 
                ? !IsGateCollided() : Ball.Position.X + Ball.Position.Width > Width;
            if (rightCondition) Ball.Bounce(Math.PI / 2);

            var leftCondition = Ball.Position.X < borderSize 
                ? !IsGateCollided() : Ball.Position.X < 0;
            if (leftCondition) Ball.Bounce(Math.PI / 2);

            if (Ball.Position.Y < borderSize
                || Ball.Position.Y + Ball.Position.Height > Height - borderSize)
                Ball.Bounce(Math.PI);
        }

        private bool IsGateCollided()
        {
            return Ball.Position.Y > 225 && Ball.Position.Y < 460;
        }

        public bool IsBallCollidedWithFootballer(Footballer footballer)
        {
            return Math.Max(Ball.Position.X, footballer.Position.X)
                <= Math.Min(Ball.Position.X + Ball.Position.Width,
                footballer.Position.X + footballer.Position.Width)
                && Math.Max(Ball.Position.Y, footballer.Position.Y)
                <= Math.Min(Ball.Position.Y + Ball.Position.Height, 
                footballer.Position.Y + footballer.Position.Height);
        }

        private void MoveUp()
        {
                foreach (var footballer in userTeam)
                    if (footballer.Position.Y - footballer.Speed < borderSize + 10)
                        return;

                foreach (var footballer in userTeam)
                    footballer.Move(-Math.PI / 2);
        }

        private void MoveDown()
        {
            foreach (var footballer in userTeam)
                if (footballer.Position.Y + footballer.Position.Height 
                    + footballer.Speed >= Height - borderSize - 10)
                    return;

            foreach (var footballer in userTeam)
                footballer.Move(Math.PI / 2);
        }


    }
}
