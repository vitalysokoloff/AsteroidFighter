using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace AsteroidFighter
{
    public class ButtonsController
    {
        private ScreenButton[] buttons;

        /// <summary>
        ///  
        /// </summary>
        /// <param name="buttons"></param>
        public ButtonsController(ScreenButton[] buttons)
        {
            this.buttons = buttons;
        }

        /// <summary>
        ///  0 - 1(a)/2(x)/3(b) / 1 - 1(left rotation)/2(right rotation) 
        /// </summary>
        /// <returns>  0/1(a)/2(x)/3(b) </returns>
        public int [] GetPressed()
        {
            int  [] answer = { 0, 0};

            if (GamePad.GetState(PlayerIndex.One).IsConnected) // для XBox геймпада
            {
                answer[0] = GamePad.GetState(PlayerIndex.One).Buttons.A != ButtonState.Pressed ? 0 : 1;
                answer[0] = GamePad.GetState(PlayerIndex.One).Buttons.X != ButtonState.Pressed ? (answer[0] != 0? 1 : 0) : 2;
                answer[0] = GamePad.GetState(PlayerIndex.One).Buttons.B != ButtonState.Pressed ? (answer[0] != 0? (answer[0] == 1 ? 1 : 2) : 0) : 3;

                answer[1] = GamePad.GetState(PlayerIndex.One).DPad.Left != ButtonState.Pressed ? (GamePad.GetState(PlayerIndex.One).DPad.Right != ButtonState.Pressed? 0 : 2) : 1;
            }
            else
            {

                TouchCollection touches = TouchPanel.GetState(); // Массив косаний к экрану /  *далее используем первые два
                if (touches.Count > 0) // Проверка что массив заполнен значениями
                {
                    for (int i = 0; i < touches.Count; i++) // Проверка пересечения касаний с кнопками
                    {
                        Point position = new Point((int)touches[i].Position.X, (int)touches[i].Position.Y);
                        if (buttons[0].Crossing(position))
                            answer[0] = 1;
                        if (buttons[1].Crossing(position))
                            answer[0] = 2;
                        if (buttons[2].Crossing(position))
                            answer[0] = 3;
                        if (buttons[3].Crossing(position))
                            answer[1] = 1;
                        if (buttons[4].Crossing(position))
                            answer[1] = 2;
                    }
                }
                else
                {
                    answer[0] = Keyboard.GetState().IsKeyDown(Keys.Space) != true ? 0 : 1;
                    answer[0] = Keyboard.GetState().IsKeyDown(Keys.C) != true ? (answer[0] != 0 ? 1 : 0) : 2;
                    answer[0] = Keyboard.GetState().IsKeyDown(Keys.X) != true ? (answer[0] != 0 ? (answer[0] == 1 ? 1 : 2) : 0) : 3;
                    answer[1] = Keyboard.GetState().IsKeyDown(Keys.Left) != true ? (Keyboard.GetState().IsKeyDown(Keys.Right) != true ? 0 : 2) : 1;
                }
            }

            /* установка значений состояния кнопок в зависимости от значений answer */

            switch (answer[0])
            {
                case 0:
                    buttons[0].SetUp();
                    buttons[1].SetUp();
                    buttons[2].SetUp();
                    break;
                case 1:
                    buttons[0].SetDown();
                    buttons[1].SetUp();
                    buttons[2].SetUp();
                    break;
                case 2:
                    buttons[0].SetUp();
                    buttons[1].SetDown();
                    buttons[2].SetUp();
                    break;
                case 3:
                    buttons[0].SetUp();
                    buttons[1].SetUp();
                    buttons[2].SetDown();
                    break;
            }

            switch (answer[1])
            {
                case 0:
                    buttons[3].SetUp();
                    buttons[4].SetUp();
                    break;
                case 1:
                    buttons[3].SetDown();
                    buttons[4].SetUp();
                    break;
                case 2:
                    buttons[3].SetUp();
                    buttons[4].SetDown();
                    break;
            }

            return answer;
        }

        /// <summary>
        ///  Возвращает сфокусированные кнопки
        /// </summary>
        /// <returns> Если вернёт длинну масива кнопок, значит сфокусированной нет</returns>
        public int GetFocused()
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                if (buttons[i].isFocused)
                    return i;
            }
            return buttons.Length;
        }

        public void SetFocused(int numberOfFocused)
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                if (buttons[i].isFocused)
                {
                    buttons[i].SetUp();
                    break;
                }
            }

            buttons[numberOfFocused].SetFocused();
        }

    }
}