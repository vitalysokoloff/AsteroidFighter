using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AsteroidFighter
{
    public class ScreenButton
    {       
        public bool isPressed { private set; get; } = false;
        public bool isFocused { private set; get; } = false;

        private Rectangle spriteRectangleCur;
        private Rectangle spriteRectangleDef;
        private Rectangle spriteRectangleFocused;
        private Rectangle spriteRectanglePressed;

        public Rectangle collider;

        /// <summary>
        /// По умолчанию для всех состояний кнопки дефолтная текстура, используй SetFocusedTexture и SetPressedTexture для изменения.
        /// </summary>
        /// <param name="colleder"> Расположение и размеры кнопки</param>
        /// <param name="spriteRectangle"> Положение текстуры на спрайте и её размеры</param>
        public ScreenButton(Rectangle collider, Rectangle spriteRectangle)
        {
            this.collider = collider;
            spriteRectangleCur = spriteRectangle;
            spriteRectangleDef = spriteRectangle;
            spriteRectangleFocused = spriteRectangle;
            spriteRectanglePressed = spriteRectangle;
        }

        public void SetDown()
        {
            isPressed = true;
            isFocused = false;
            spriteRectangleCur = spriteRectanglePressed;
        }

        public void SetUp()
        {
            isPressed = false;
            isFocused = false;
            spriteRectangleCur = spriteRectangleDef;
        }

        public void SetFocused()
        {
            isFocused = true;
            spriteRectangleCur = spriteRectangleFocused;
        }

        public void SetFocusedTexture(Rectangle rectangle)
        {
            spriteRectangleFocused = rectangle;
        }

        public void SetPressedTexture(Rectangle rectangle)
        {
            spriteRectanglePressed = rectangle;
        }

        public void SetPosition(Point position)
        {
            collider.Location = position;
        }

        public Point GetPosition()
        {
            return collider.Location;
        }

        /// <summary>
        /// Возвращает размеры коллайдера
        /// </summary>
        /// <returns> X - ширина, Y - Высота</returns>
        public Point GetSizes()
        {
            return new Point(collider.Width, collider.Height);
        }

        /// <summary>
        /// Проверяет касания
        /// </summary>
        /// <param name="position"> (int) Место касания экрана</param>
        /// <returns></returns>
        public bool Crossing(Point position)
        {
            if (position.X > collider.Left && position.X < collider.Right && position.Y > collider.Top && position.Y < collider.Bottom)
                return true;
            else
                return false;
        }        

        public void Draw(SpriteBatch spriteBatch, Texture2D spriteSet)
        {
            spriteBatch.Draw(spriteSet, new Vector2(collider.X, collider.Y), spriteRectangleCur, Color.White );
        }
    }
}