using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace Trabalho_Prático
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        bool rotate = false;
        float rotangle = MathHelper.Pi / 100;
        Terrain Ground;
        ClsCamera camera;
        Vector3 cameraorigin = new Vector3(60, 20, -80);
        Lighting lighting;
        Texture2D cursor;
        Vector2 cursorpos;
        ClsTank myTank;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }
        
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();
        }
        
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            //cursor = Content.Load<Texture2D>("cursor");
            
            Ground = new Terrain(GraphicsDevice, Content);
            camera = new ClsCamera(cameraorigin, GraphicsDevice, Ground);
            lighting = new Lighting(Ground.effect);
            lighting.CustomLighting();
            myTank = new ClsTank(GraphicsDevice, Content, Ground);
        }
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }
        
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            
            MouseState mouse = Mouse.GetState();
            cursorpos = mouse.Position.ToVector2();
            myTank.Update();

            if (rotate) Ground.worldMatrix *= Matrix.CreateRotationY(rotangle);

            camera.Update();

            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            
            Ground.Draw(GraphicsDevice,camera);
            myTank.Draw(camera);
            
            spriteBatch.Begin();
            //spriteBatch.Draw(cursor, cursorpos, Color.White);
            spriteBatch.End();
            
            GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            base.Draw(gameTime);
        }
    }
}
