namespace TableFootball
{
    class Position
    {
        public readonly int Width;
        public readonly int Height;
        public double X;
        public double Y;           
        public Position(double x, double y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }
    }
}
