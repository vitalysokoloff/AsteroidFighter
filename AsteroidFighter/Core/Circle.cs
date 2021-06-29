using Microsoft.Xna.Framework;
using System;

namespace AsteroidFighter
{
    public class Circle
    {
        public int radius;
        private Point _position;

        public Point Position
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;
                Left = _position.X - radius;
                Top = _position.Y - radius;
                Bottom = _position.Y + radius;
                Right = _position.X + radius;
            }
        }

        public int Left { get; private set; }
        public int Top { get; private set; }
        public int Bottom { get; private set; }
        public int Right { get; private set; }

        public Circle(Point position, int radius)
        {
            this.radius = radius;
            Position = position;            
        }

        public Circle(int x, int y, int radius)
        {
            _position = new Point(x, y);
            this.radius = radius;
        }
        
        public bool Crossing(Point position)
        {
            
            if ((Math.Pow(position.X - _position.X, 2) + Math.Pow(position.Y - _position.Y, 2)) <= radius * radius)
                return true;
            else
                return false;
        }

        public bool Crossing(Vector2 position)
        {
            if ((Math.Pow(position.X - _position.X, 2) + Math.Pow(position.Y - _position.Y, 2)) <= radius * radius)
                return true;
            else
                return false;
        }

        public bool Crossing(Circle circle)
        {
            if (Math.Pow(_position.X - circle.Position.X, 2) + Math.Pow(_position.Y - circle.Position.Y, 2) < Math.Pow(radius + circle.radius, 2))
                return true;
            else
                return false;
        }

        public bool Crossing(Rectangle rect)
        {
            int x = _position.X;
            int y = _position.Y;

            if (_position.X < rect.X)
                x = rect.X;
            else if (_position.X > (rect.X + rect.Width))
                x = rect.X + rect.Width;

            if (_position.Y < rect.Y)
                y = rect.Y;
            else if (_position.Y > (rect.Y + rect.Height))
                y = rect.Y + rect.Height;

            if (Math.Pow(_position.X - x, 2) + Math.Pow(_position.Y - y, 2) <= (radius * radius))
                return true;
            else
                return false;
        }

        public string CrossingRectangleSet(RectangleSet set)
        {
            if (Crossing(set.Top))
                return "Top";
            if (Crossing(set.Bottom))
                return "Bottom";
            if (Crossing(set.Left))
                return "Left";
            if (Crossing(set.Right))
                return "Right";
            return "None";
        }

        /// <summary>
        /// Вернёт под каким углом относительно центра было пересечение
        /// </summary>
        /// <param name="position">координаты точки</param>
        /// <returns>радианы наверное</returns>
        public float CrossingAngle(Point position)
        {
            int legY = _position.Y - position.Y; // длина противолежащего катета
            int legX = _position.X - position.X; // длина прилежащего катета

            return (float) Math.Atan2(legY, legX);
        }

        /// <summary>
        /// Вернёт под каким углом относительно центра было пересечение
        /// </summary>
        /// <param name="position">координаты точки</param>
        /// <returns>радианы наверное</returns>
        public float CrossingAngle(Vector2 position)
        {
            float legY = _position.Y - position.Y; // длина противолежащего катета
            float legX = _position.X - position.X; // длина прилежащего катета

            return (float) Math.Atan2(legY, legX);
        }
    }
}