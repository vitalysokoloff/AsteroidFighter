using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace AsteroidFighter
{
    public class GOController
    {
        private ButtonsController buttons;
        private Rectangle innerRect;
        private bool immortality = false; 

        private Helper.Metronome iterationStartingMetronome;
        private Helper.Metronome blinkMetronome;
        private Helper.Metronome bangMetronome;
        
        // пули / стрельба
        private bool FirePressed = false;
        private int bulletCount = 0;
        private int bulletLimit = 7;

        // Суперка
        private bool ExpPressed = false;

        private int asteroidCount = 0;

        private Point screenSize;

        public List<GameObject> gameObjects { get; private set; }
        public List<GameObject> bangs{ get; private set; }
        public int Iteration { get; private set; } = 1;
        public int Lives { get; private set; } = 3;
        public int Explosions { get; private set; } = 3;
        public int Scores { get; private set; } = 0;

        public GOController(ButtonsController buttons, Point screenSize)
        {
            this.buttons = buttons;
            this.screenSize = screenSize;
            iterationStartingMetronome = new Helper.Metronome(1000);
            blinkMetronome = new Helper.Metronome(100);
            bangMetronome = new Helper.Metronome(70);

            innerRect = new Rectangle(new Point(screenSize.X / 2 - 360, screenSize.Y / 2 - 355), new Point(720, 710));
            gameObjects = new List<GameObject>();
            bangs = new List<GameObject>();

            GameObject go = new GameObject(
                new Point(0, 0),
                new Point(50, 64),
                4,
                new Vector2(screenSize.X / 2, screenSize.Y / 2),
                new Point(50, 64),
                35,
                "Player"
                );
            go.ImpulseSpeed = 2;
            go.SetFrame(innerRect);

            gameObjects.Add(go);
        }

        public void Update(GameTime gameTime)
        {
            Blink(gameTime);                // Мерцание ракеты в режиме бессмертия
            IterationStarting(gameTime);    // Старт уровня сложности (Итерация)
            KeyListen();                    // Нажатие кнопок
            Colliding();                    // Проверка столкновений
            
            BangAnimate(gameTime);          // Анимация взрывов (blast и bang)
            Harvest();                      // Сборка "убитых" объектов
            

            foreach (GameObject go in gameObjects)
                go.Update();
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D spriteSet)
        {
            foreach (GameObject go in gameObjects)
                go.Draw(spriteBatch, spriteSet);
            foreach (GameObject b in bangs)
                b.Draw(spriteBatch, spriteSet);
        }

        private void ScoresAdd(int s)
        {
            Scores += s;
            if (Scores < 0)
                Scores = 0;
        }

        private void BangAnimate(GameTime gameTime)
        {
            if (bangs.Count > 0)
            {
                bangMetronome.Start();
                if (bangMetronome.Ticking(gameTime))
                    foreach (GameObject b in bangs)
                    {
                        b.Animate();
                        if (b.frameCur > 3)
                            b.IsAlive = false;
                    }
            }
            else
            {
                bangMetronome.Stop();
                bangMetronome.Reset();
            }
        }

        private void Harvest()
        {
            int j = 0;
            bool gotcha = false;

            for (int i = 1; i < gameObjects.Count; i++)
            {
                GameObject g = gameObjects[i];
                if (g.Type == "Big" && g.IsAlive == false)
                {
                    MakeMiddle(g);
                    MakeBlast(g);
                    ScoresAdd(10);
                    gotcha = true;
                    j = i;
                    break;
                }
                if (g.Type == "Middle" && g.IsAlive == false)
                {
                    MakeSmall(g);
                    MakeBlast(g);
                    ScoresAdd(20);
                    gotcha = true;
                    j = i;
                    break;
                }
                if (g.Type == "Small" && g.IsAlive == false)
                {
                    MakeBlast(g);
                    ScoresAdd(40);
                    gotcha = true;
                    j = i;
                    break;
                }
                if (g.Type == "Bullet" && g.IsAlive == false)
                {
                    gotcha = true;
                    MakeBang(g);
                    j = i;
                    bulletCount--;
                    break;
                }
            }

            if(gotcha)
            {
                gameObjects.RemoveAt(j);
                gotcha = false;
            }

            for (int i = 0; i < bangs.Count; i++)
            {
                if (!bangs[i].IsAlive)
                {
                    gotcha = true;
                    j = 0;
                }
            }

            if (gotcha)
            {
                bangs.RemoveAt(j);
                gotcha = false;
            }
        }

        private void Colliding()
        {
            asteroidCount = 0;
            for (int i = 1; i < gameObjects.Count; i++)
            {
                GameObject g = gameObjects[i];
                if (!immortality)
                    if (gameObjects[0].Collider.Crossing(g.Collider))
                    {
                        if (g.Type == "Big" || g.Type == "Middle" || g.Type == "Small")
                        {
                            g.IsAlive = false;
                            immortality = true;
                            Lives--;
                            ScoresAdd(-300);
                            break;
                        }
                    }

                if (g.Type == "Bullet")
                {
                    if (g.IsCollision)
                        g.IsAlive = false;

                    for (int j = 1; j < gameObjects.Count; j++ )
                    {
                        GameObject c = gameObjects[j];
                        if (g.Collider.Crossing(c.Collider))
                        {
                            if (c.Type == "Big" || c.Type == "Middle" || c.Type == "Small")
                            {
                                g.IsAlive = false;
                                c.IsAlive = false;
                            }
                        }
                    }
                    
                }
                if (g.Type == "Big" || g.Type == "Middle" || g.Type == "Small")
                    asteroidCount++;
            }
        }

        private void MakeBullet(GameObject p)
        {
            GameObject g = new GameObject(
                            new Point(200, 0),
                            new Point(8, 8),
                            1,
                            p.Position + p.Forward * Helper.PointToVector2(p.Size) / 2,
                            new Point(8, 8),
                            4,
                            "Bullet"
                            );
            g.ImpulseSpeed = 5;
            g.SetFrame(innerRect);
            g.Impulse = p.Forward;
            gameObjects.Add(g);
        }

        private void MakeBang(GameObject g)
        {
            GameObject b = new GameObject(
                            new Point(0, 92),
                            new Point(90, 90),
                            5,
                            g.Position,
                            new Point(90, 90),
                            45,
                            "Bang"
                            );
            b.ImpulseSpeed = 0;
            bangs.Add(b);
        }

        private void MakeBlast(GameObject g)
        {
            GameObject b = new GameObject(
                            new Point(0, 182),
                            new Point(90, 90),
                            5,
                            g.Position,
                            new Point(90, 90),
                            45,
                            "Blast"
                            );
            b.ImpulseSpeed = 0;
            bangs.Add(b);
        }

        private void MakeSmall(GameObject m)
        {
            GameObject g1 = new GameObject(
                            new Point(270, 488),
                            new Point(40, 37),
                            1,
                            m.Position,
                            new Point(40, 37),
                            17,
                            "Small"
                            );
            g1.ImpulseSpeed = 1;
            g1.SetFrame(innerRect);
            g1.DefRotateSpeed = Helper.RandomizeRotateSpeed();
            g1.SetImpulse(m.ImpulseAngle + 1.571f);
            gameObjects.Add(g1);

            GameObject g2 = new GameObject(
                            new Point(270, 488),
                            new Point(40, 37),
                            1,
                            m.Position,
                            new Point(40, 37),
                            17,
                            "Small"
                            );
            g2.ImpulseSpeed = 1;
            g2.SetFrame(innerRect);
            g2.DefRotateSpeed = Helper.RandomizeRotateSpeed();
            g1.SetImpulse(m.ImpulseAngle - 1.571f);
            gameObjects.Add(g2);
        }

        private void MakeMiddle(GameObject b)
        {
            GameObject g1 = new GameObject(
                            new Point(270, 416),
                            new Point(64, 72),
                            1,
                            b.Position,
                            new Point(64, 72),
                            29,
                            "Middle"
                            );
            g1.ImpulseSpeed = 1;
            g1.SetFrame(innerRect);
            g1.DefRotateSpeed = Helper.RandomizeRotateSpeed();
            g1.SetImpulse(b.ImpulseAngle + 1.571f);
            gameObjects.Add(g1);

            GameObject g2 = new GameObject(
                            new Point(270, 416),
                            new Point(64, 72),
                            1,
                            b.Position,
                            new Point(64, 72),
                            29,
                            "Middle"
                            );
            g2.ImpulseSpeed = 1;
            g2.SetFrame(innerRect);
            g2.DefRotateSpeed = Helper.RandomizeRotateSpeed();
            g1.SetImpulse(b.ImpulseAngle - 1.571f);
            gameObjects.Add(g2);
        }

        private void MakeBig()
        {
            GameObject g = new GameObject(
                            new Point(270, 306),
                            new Point(116, 111),
                            1,
                            Helper.RandomizePosition(innerRect, new Point(116, 111)),
                            new Point(116, 111),
                            52,
                            "Big"
                            );
            g.ImpulseSpeed = 2;
            g.SetFrame(innerRect);
            g.SetImpulse(Helper.RandomizeAngle());
            g.DefRotateSpeed = Helper.RandomizeRotateSpeed();
            gameObjects.Add(g);
        }        

        private void IterationStarting(GameTime gameTime)
        {
            if (asteroidCount == 0)
            {
                iterationStartingMetronome.Start();
                iterationStartingMetronome.Ticking(gameTime);

                if (iterationStartingMetronome.Count == 3)
                {
                    immortality = true;
                    for (int i = 0; i < Iteration; i++)
                    {
                        MakeBig();                                               
                        iterationStartingMetronome.Reset();
                    }
                    Iteration++;
                }

            }
            else if (asteroidCount > 0)
            {
                iterationStartingMetronome.Ticking(gameTime);

                if (iterationStartingMetronome.Count == 3)
                {
                    ScoresAdd(1000);
                    iterationStartingMetronome.Stop();
                    iterationStartingMetronome.Reset();
                }
            }
        }

        private void Blink(GameTime gameTime)
        {
            if (immortality)
            {
                blinkMetronome.Start();
                gameObjects[0].IsDrawn = !blinkMetronome.Ticking(gameTime);
                if (blinkMetronome.Count > 30)
                    immortality = false;
            }
            else
            {
                gameObjects[0].IsDrawn = true;
                blinkMetronome.Stop();
                blinkMetronome.Reset();
            }
        }

        private void ExplodeAll()
        {
            for (int i = 1; i < gameObjects.Count; i++)
            {
                GameObject g = gameObjects[i];
                if (g.Type == "Big" || g.Type == "Middle" || g.Type == "Small")
                    g.IsAlive = false;
            }
        }

        private void KeyListen()
        {
            int[] answer = buttons.GetPressed();

            if (answer[0] == 2)
            {
                if (!gameObjects[0].IsCollision)
                    gameObjects[0].MoveTo(3);
                gameObjects[0].Animate();
            }
            else if (answer[0] != 2)
            {
                gameObjects[0].AnimationReset();
            }

            if (answer[0] == 1)
            {
                if (!FirePressed && bulletCount < bulletLimit)
                {
                    MakeBullet(gameObjects[0]);
                    FirePressed = true;
                    ScoresAdd(-1);
                    bulletCount++;
                }
            }

            if (answer[0] != 1)
            {
                if (FirePressed)
                {
                    FirePressed = false;
                }
            }

            if (answer[0] == 3)
            {
                if (!ExpPressed && Explosions > 0)
                {
                    ExplodeAll();
                    Explosions--;
                    ExpPressed = true;
                }
            }

            if (answer[0] != 3)
            {
                if (ExpPressed)
                {                    
                    ExpPressed = false;
                }
            }

            if (answer[1] == 1)
            {
                gameObjects[0].Rotate(-0.1f);
            }

            if (answer[1] == 2)
            {
                gameObjects[0].Rotate(0.1f);
            }
        }
    }
}