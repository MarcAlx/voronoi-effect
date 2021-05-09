using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using plasmaeffect;
using plasmaeffect.Engine;

namespace plasma_effect
{
    public class VoronoiEffect : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteFont _defaultFont;

        private Texture2D _voronoi;
        private VoronoiEngine _engine;

        public VoronoiEffect()
        {
            this.Window.AllowUserResizing = true;
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            this.TargetElapsedTime = TimeSpan.FromSeconds(1d / Config.FPS_CAP);
        }

        protected override void Initialize()
        {
            this.Window.Title = Config.WINDOW_TITLE;

            this._engine = new VoronoiEngine(20, ColorRampEnum.RAINBOW, ApplyColorDirectionEnum.HORIZONTAL);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _defaultFont = Content.Load<SpriteFont>("Default");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //update voronoi
            this._voronoi = this._engine.UpdateVoronoi(
                GraphicsDevice,
                GraphicsDevice.Viewport.Bounds.Width,
                GraphicsDevice.Viewport.Bounds.Height,
                Config.PIXEL_RATIO
            );
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            this._spriteBatch.Begin();

            //draw voronoi
            this._spriteBatch.Draw(this._voronoi, new Vector2(0, 0), Color.White);

            //draw point
            if (Config.DRAW_POINTS)
            {
                foreach(var point in this._engine.Points)
                {
                    Texture2D rect = new Texture2D(GraphicsDevice, 3,3);
                    Color[] data = new Color[9];
                    for (var i = 0; i < data.Length; i++)
                    {
                        data[i] = Color.Black;
                    }
                    rect.SetData(data);
                    var p = point.GetAbsolutePoint(GraphicsDevice.Viewport.Bounds.Width,GraphicsDevice.Viewport.Bounds.Height);
                    this._spriteBatch.Draw(rect, new Vector2(p.X-1, p.Y-1), Color.White);
                }
            }

            //draw fps
            if (Config.DISPLAY_FPS)
            {
                Toolkit.DrawFPSAt(new Vector2(5, 5), _defaultFont, _spriteBatch, 1 / (float)gameTime.ElapsedGameTime.TotalSeconds);
            }

            this._spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
