using System;

namespace TableFootball
{
    class Bonus
    {
        public Position Position;
        public Bonus(Position position)
        {
            Position = position;
        }

        private Random rnd = new Random();
        public Effect GetBonus()
        {
            var number = rnd.Next(4);
            if (number == 0)
                return Effect.Freeze;
            if (number == 1)
                return Effect.SpeedBoost;
            if (number == 2)
                return Effect.SmallBall;
            else return Effect.GhostBall;
        }
    }
}
