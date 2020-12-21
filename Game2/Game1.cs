using Game2.Figures;
using Game2.GUI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Myra;

namespace Game2
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Figure _figure;

        private CoordinateGrid _grid;

        private Gui _gui;
        
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void LoadContent()
        {
            MyraEnvironment.Game = this;
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _grid = new CoordinateGrid(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, 12);

            _figure = new Figure(_grid.Center, _grid.Unit);

            _gui = new Gui(_figure, _grid);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            _spriteBatch.Begin();
            
            _grid.Draw(_spriteBatch);
            _figure.Draw(_spriteBatch);
            
            _spriteBatch.End();
            
            _gui.Render();
            
            base.Draw(gameTime);
        }
    }
}
