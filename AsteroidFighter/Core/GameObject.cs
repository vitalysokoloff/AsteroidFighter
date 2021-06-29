using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace AsteroidFighter
{
    public class GameObject
    {
        private Point imagePosition;
        private Point imageSize;
        private Vector2 _position;        
        private RectangleSet rectangleSet;
        public Point Size;
        private Vector2 axis;
        private int speed;

        public int frameCur { get; private set; } = 0;
        public int FrameQuantity { get; private set; }
        public float ImpulseAngle { get; private set; } = 0;

        // Движение
        private Vector2 _forward = Vector2.Zero;        
        private Vector2 target = Vector2.Zero;
        public Vector2 _impulse;
        public int ImpulseSpeed { get; set; } = 0;
        public float Rotation { get; private set; } = 0f;
        public Vector2 Forward
        {
            get
            {
                return _forward;
            }
        }
        public Vector2 Impulse
        {
            get
            {
                return _impulse;
            }
            set
            {
                _impulse = value;
            }
        } 
        public Vector2 Position
        {
            get
            {
                return _position;
            }

            set
            {
                _position = value;
                Collider.Position = Helper.Vector2ToPoint(_position);
            }
        }

        public Circle Collider { get; private set; }
        public bool IsCollision { get; private set; } = false;
        public bool IsAlive { get; set; } = true;        
        public bool IsDrawn { get; set; } = true;
        public string Type { get; private set; }
        public float DefRotateSpeed { get; set; } = 0;

        public GameObject(Point imagePosition, Point imageSize, int frameQuantity, Vector2 position, Point size, int radius, string type)
        {
            this.imagePosition = imagePosition;
            this.imageSize = imageSize;
            FrameQuantity = frameQuantity;
            Size = size;
            axis = new Vector2(size.X / 2, size.Y / 2);
            Collider = new Circle(Helper.Vector2ToPoint(axis), radius);
            Position = position;
            _impulse = Helper.Vector2FromAngle(ImpulseAngle);
            Type = type;
        }

        public Circle GetCollider()
        {
            return Collider;
        }

        public void SetImpulse(float angle)
        {
            if (angle < -4.712f)
                angle = 1.571f + angle + 4.712f;
            if (angle > 1.571f)
                angle = -4.712f - angle - 1.571f;
            _impulse = Helper.Vector2FromAngle(angle);
        }

        public void SetFrame(Rectangle rectangle)
        {
            rectangleSet = new RectangleSet(rectangle);
        }

        public void MoveTo(int speed)
        {
            this.speed = speed;
            if (!IsCollision)
            {
                Position += _forward * speed;
                _impulse = Helper.Vector2FromAngle(Rotation);
            }
        }

        public void Rotate(float speed)
        {
            Rotation += speed;
            if (Rotation < -4.712f)
                Rotation = 1.571f;
            if (Rotation > 1.571f)
                Rotation = -4.712f;
            _forward = Helper.Vector2FromAngle(Rotation);
        }

        public void Update()
        {
            Position += _impulse * ImpulseSpeed;
            Rotate(DefRotateSpeed);

            switch (Collider.CrossingRectangleSet(rectangleSet))
            {
                case "Top":
                    IsCollision = true;
                    _impulse.Y *= -1;
                    Position += _impulse * speed * 3;                   
                    break;
                case "Bottom":
                    IsCollision = true;
                    _impulse.Y *= -1;
                    Position += _impulse * speed * 3;
                    break;
                case "Left":
                    IsCollision = true;
                    _impulse.X *= -1;
                    Position += _impulse * speed * 3;                    
                    break;
                case "Right":
                    IsCollision = true;
                    _impulse.X *= -1;
                    Position += _impulse * speed * 3;                    
                    break;
                case "None":                                       
                    IsCollision = false;
                    break;
            }

        }

        public void Animate()
        {
            if (frameCur < FrameQuantity)
                frameCur++;
            if (frameCur >= FrameQuantity)
                frameCur = 0;
        }

        public void AnimationReset()
        {
            frameCur = 0;
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D spriteSet)
        {
            if (IsDrawn)
            spriteBatch.Draw(
                spriteSet,
                new Rectangle(Helper.Vector2ToPoint(Position), Size),
                new Rectangle(imagePosition + new Point(imageSize.X * frameCur, 0), imageSize),
                Color.White,
                Rotation,
                axis,
                SpriteEffects.None,
                1f
                );
        }
    }
}