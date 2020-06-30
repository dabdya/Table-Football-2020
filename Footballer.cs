using System;

namespace TableFootball
{
    class Footballer
    {
        public readonly Position Position;
        public double DirectionRadians;
        public Footballer(Position position, double directionRadians)
        {
            Position = position;
            DirectionRadians = directionRadians;
        }
      
        public int Speed { get; } = 5;
        public void Move(double directionRadians)
        {
            var dy = Math.Sin(directionRadians) * Speed;
            Position.Y += dy;
        }
    }
}
