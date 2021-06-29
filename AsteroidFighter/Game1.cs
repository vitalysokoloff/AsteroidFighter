using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AsteroidFighter
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Point screenSize;

        Color bgColor = new Color(9, 10, 12);
        Texture2D mask;

        // Buttons
        Texture2D buttonsSpriteSet;
        ScreenButton [] screenButtons = new ScreenButton[5];
        ButtonsController buttonsController;

        // GameObjects
        Texture2D GOspriteSet;
        GOController controller;

        SpriteFont font;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            var metric = new Android.Util.DisplayMetrics();
            Activity.WindowManager.DefaultDisplay.GetMetrics(metric);

            Content.RootDirectory = "Content";

            graphics.IsFullScreen = true;
            screenSize.X = graphics.PreferredBackBufferWidth = metric.WidthPixels;
            screenSize.Y = graphics.PreferredBackBufferHeight = metric.HeightPixels;
            graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;
            graphics.ApplyChanges();

            screenButtons[0] = new ScreenButton(new Rectangle(10, screenSize.Y / 2 + 30, 126, 138), new Rectangle(0, 0, 126, 138));
            screenButtons[0].SetPressedTexture(new Rectangle(126, 0, 126, 138));
            screenButtons[1] = new ScreenButton(new Rectangle(10, screenSize.Y / 2 - 156, 126, 138), new Rectangle(252, 0, 126, 138));
            screenButtons[1].SetPressedTexture(new Rectangle(380, 0, 126, 138));
            screenButtons[2] = new ScreenButton(new Rectangle(146, screenSize.Y / 2 - 78, 126, 138), new Rectangle(504, 0, 126, 138));
            screenButtons[2].SetPressedTexture(new Rectangle(630, 0, 126, 138));
            screenButtons[3] = new ScreenButton(new Rectangle(screenSize.X - 282, screenSize.Y / 2 - 78, 126, 138), new Rectangle(1008, 0, 126, 138));
            screenButtons[3].SetPressedTexture(new Rectangle(1134, 0, 126, 138));
            screenButtons[4] = new ScreenButton(new Rectangle(screenSize.X - 146, screenSize.Y / 2 + 30, 126, 138), new Rectangle(1008, 138, 126, 138));
            screenButtons[4].SetPressedTexture(new Rectangle(1134, 138, 126, 138));
            buttonsController = new ButtonsController(screenButtons);

            controller = new GOController(buttonsController, screenSize);

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            buttonsSpriteSet = Content.Load<Texture2D>("sprites/buttons");
            mask = Content.Load<Texture2D>("sprites/mask");
            GOspriteSet = Content.Load<Texture2D>("sprites/spritepack");

            font = Content.Load<SpriteFont>("font");

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {

            if (controller.gameObjects[0].IsAlive)
                controller.Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(bgColor);
            spriteBatch.Begin();
            if (controller.gameObjects[0].IsAlive)
                controller.Draw(spriteBatch, GOspriteSet);

            spriteBatch.Draw(mask, new Vector2(screenSize.X / 2 - 619, screenSize.Y / 2 - 614), Color.White);
            spriteBatch.DrawString(font, "Score: " + controller.Scores, new Vector2(screenSize.X / 2 - 100, 2), Color.White);
            spriteBatch.DrawString(font, "Iteration: " + controller.Iteration, new Vector2(screenSize.X / 2 - 100, 24), Color.White);
            spriteBatch.DrawString(font, "Lives: " + controller.Lives, new Vector2(screenSize.X / 2 - 100, 46), Color.White);
            spriteBatch.DrawString(font, "Explosions: " + controller.Explosions, new Vector2(screenSize.X / 2 - 100, 68), Color.White);

            foreach (ScreenButton sb in screenButtons)
                sb.Draw(spriteBatch, buttonsSpriteSet);

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
