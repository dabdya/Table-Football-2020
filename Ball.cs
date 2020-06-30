using System;

namespace TableFootball
{
    class Ball
    {
        public bool IsSmall;
        public bool IsGhost;
        public Position Position { get; private set; }
        public Ball(Position position, double angle)
        {
            Position = position;
            DirectionRadians = angle;
        }

        public double DirectionRadians { get; private set; }
        public int Acceleration { get; } = 10;
        private int speed;
        public void Move(double directionRadians, int force = 1)
        {
            if (directionRadians == Math.PI || directionRadians == 0 
                || directionRadians == Math.PI / 2 || directionRadians == 3 * Math.PI / 2)
                directionRadians *= 0.9;
            DirectionRadians = directionRadians;
            if (speed > Acceleration) speed -= Acceleration;
            speed = Math.Max(speed, Acceleration) * force;
            if (speedIters != 0)
            {
                speedIters--;
                speed = 20;
            }
            if (smallIters != 0)
            {
                smallIters--;
                Position = new Position(Position.X, Position.Y, 18, 18);
            }
            else
            {
                Position = new Position(Position.X, Position.Y, 32, 32);
                IsSmall = false;
            }
            var dx = Math.Cos(directionRadians) * speed;
            var dy = Math.Sin(directionRadians) * speed;
            Position.X += dx;
            Position.Y += dy;
        }

        public void Bounce(double inclinationRadians)
        {
            DirectionRadians =  2 * inclinationRadians - DirectionRadians;
        }

        public double GetBounce(double inclinationRadians)
        {
            return 2 * inclinationRadians - DirectionRadians;
        }

        public int speedIters;
        public void SpeedEffect(int speedIters = 300)
        {
            this.speedIters = speedIters;
        }

        public int smallIters;
        public void SmallEffect(int smallIters = 300)
        {
            IsSmall = true;
            this.smallIters = smallIters;
        }
    }
}
