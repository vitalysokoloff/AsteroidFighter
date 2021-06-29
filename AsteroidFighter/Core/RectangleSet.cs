using Microsoft.Xna.Framework;

namespace AsteroidFighter
{
    public class RectangleSet
    {
        public Rectangle Top { get; private set; }
        public Rectangle Left { get; private set; }
        public Rectangle Bottom { get; private set; }
        public Rectangle Right { get; private set; }

        public Point Location { get; private set; }
        public Point Size { get; private set; }

        public RectangleSet(Rectangle innerRectangle)
        {
            Top = new Rectangle(innerRectangle.Location - new Point(0, 20), new Point(innerRectangle.Width, 20));
            Left = new Rectangle(innerRectangle.Location - new Point(20, 0), new Point(20, innerRectangle.Height));
            Bottom = new Rectangle(innerRectangle.Location + new Point(0, innerRectangle.Height), new Point(innerRectangle.Width, 20));
            Right = new Rectangle(innerRectangle.Location + new Point(innerRectangle.Width, 0), new Point(20, innerRectangle.Height));

            Location = innerRectangle.Location;
            Size = innerRectangle.Size;
        }
    }
}