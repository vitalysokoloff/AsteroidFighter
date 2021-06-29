using Microsoft.Xna.Framework;
using System;

namespace AsteroidFighter
{
    public static class Helper
    {
        private static Random random = new Random();

        public static Vector2 PointToVector2(Point point)
        {
            return new Vector2(point.X, point.Y);
        }

        public static Point Vector2ToPoint(Vector2 vector)
        {
            return new Point((int)vector.X, (int)vector.Y);
        }

        public static Vector2 Vector2FromAngle(float angle)
        {
            return new Vector2((float)Math.Sin(angle), -(float)Math.Cos(angle));
        }

        public static Vector2 RandomizePosition(Rectangle rectangle, Point size)
        {
            int x = random.Next(rectangle.X + size.X, rectangle.X + rectangle.Width - size.X);
            int y = random.Next(rectangle.Y + size.Y, rectangle.Y + rectangle.Height - size.Y);
            return new Vector2(x, y);
        }

        public static float RandomizeAngle()
        {
            return random.Next(-4712, 1571) / 1000f;
        }

        public static float RandomizeRotateSpeed()
        {
            return random.Next(-15, 15) / 100f;
        }


        public class Metronome
        {
            private int curTime;
            private int period;
            private bool isStart = false;
            public int Count { get; private set; }

            public Metronome(int period)
            {
                curTime = 0;
                Count = 0;
                this.period = period;
            }

            public bool Ticking(GameTime gameTime)
            {
                if (isStart)
                {
                    curTime += gameTime.ElapsedGameTime.Milliseconds;
                    if (curTime > period) // Анимация
                    {
                        curTime = 0;
                        Count++;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }

            public void Reset()
            {
                curTime = 0;
                Count = 0;
            }

            public void Stop()
            {
                if (isStart)
                    isStart = false;
            }

            public void Start()
            {
                if (!isStart)
                    isStart = true;
            }
        }
    }
}