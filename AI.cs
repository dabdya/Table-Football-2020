using System;
using System.Collections.Generic;
using System.Linq;

namespace TableFootball
{
    class AI
    {
        private Game game;
        public bool Sleep;
        public int SleepCount = 300;

        private List<Footballer> aiTeam;
        public IEnumerable<Footballer> AiFootballers
            => aiTeam.AsEnumerable();

        public AI(Game game)
        {
            this.game = game;
        }

        public void InitializeTeam()
        {
            aiTeam = new List<Footballer>()
            {
                new Footballer(new Position(
                1080, game.Height / 2, 21, 31), Math.PI),
                new Footballer(new Position(
                940, 300, 21, 31), Math.PI),
                new Footballer(new Position(
                940, 420, 21, 31), Math.PI),
                new Footballer(new Position(
                660, 180, 21, 31), Math.PI),
                new Footballer(new Position(
                660, 300, 21, 31), Math.PI),
                new Footballer(new Position(
                660, 420, 21, 31), Math.PI),
                new Footballer(new Position(
                660, 540, 21, 31), Math.PI),
                new Footballer(new Position(
                380, 150, 21, 31), Math.PI),
                new Footballer(new Position(
                380, 360, 21, 31), Math.PI),
                new Footballer(new Position(
                380, 570, 21, 31), Math.PI)
            };
        }

        private Random rnd = new Random();
        public void Act()
        {
            if (Sleep && SleepCount == 0)
            {
                Sleep = false;
                SleepCount = 300;
                return;
            }
            else if (Sleep)
            {
                SleepCount--;
                return;
            }
            if (!Sleep)
                if (ShortestDistanceToBall() < 0)
                    MoveDown();
                else MoveUp();
        }

        private void MoveUp()
        {
            foreach (var footballer in aiTeam)
                if (footballer.Position.Y - footballer.Speed < game.borderSize + 10)
                    return;

            foreach (var footballer in aiTeam)
                footballer.Move(-Math.PI / 2);
        }

        private void MoveDown()
        {
            foreach (var footballer in aiTeam)
                if (footballer.Position.Y + footballer.Position.Height + footballer.Speed >= game.Height - game.borderSize - 10)
                    return;

            foreach (var footballer in aiTeam)
                footballer.Move(Math.PI / 2);
        }

        private double ShortestDistanceToBall()
        {
            var shortest = double.MaxValue;
            foreach (var footballer in aiTeam)
            {
                var distance = footballer.Position.Y - game.Ball.Position.Y;
                if (distance < shortest)
                    shortest = distance;
            }
            return shortest;
        }
    }
}
