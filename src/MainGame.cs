using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace cis_580_game;

public class MainGame : Game
{
    private Resources _resources;
    private GuiManager _guiManager;
    private MusicManager _musicManager;

    public MainGame()
    {
        _resources = new(this)
        {
            Graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width,
                PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height,
                SynchronizeWithVerticalRetrace = true
            }
        };
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        Window.AllowUserResizing = true;
    }

    protected override void Initialize()
    {
        _guiManager = new(_resources);
        _musicManager = new(_resources);

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _resources.BaseRenderer = new(GraphicsDevice);
        _resources.GuiRenderer = new(GraphicsDevice);
        _resources.GameplayRenderer = new(GraphicsDevice);

        _resources.VirtualScreen = new(_resources);
        _resources.Camera = new(_resources);

        _resources.LoadContent(Content);
        _guiManager.LoadContent(Content);
        _musicManager.LoadContent(Content);
    }

    protected override void Update(GameTime gameTime)
    {
        // Allow user to exit using buttons
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
        {
            Exit();
        }

        _resources.Update(gameTime);
        _guiManager.Update(gameTime);

        _resources.Camera.SetCameraPosition(_resources.PlayerShip.Position, _resources.PlayerShip.Angle - MathHelper.PiOver2, 0.5f);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(GameColors.BackgroundColor);

        if (!_resources.IsLoaded)
        {
            base.Draw(gameTime);
            return;
        }

        // Gameplay layer
        _resources.GameplayRenderer.Begin(SpriteSortMode.BackToFront, samplerState: SamplerState.PointClamp, transformMatrix: _resources.Camera.TransformationMatrix, rasterizerState: RasterizerState.CullClockwise);

        _resources.PlayerShip.Draw(gameTime, _resources.GameplayRenderer);
        PrimitiveRenderer.DrawRectangle(gameTime, _resources.GameplayRenderer, new Rectangle(-10,-5,60,10), Color.Red, Layers.ForcedFront);
        PrimitiveRenderer.DrawRectangle(gameTime, _resources.GameplayRenderer, new Rectangle(-5,-10,10,60), Color.White, Layers.ForcedFront);

        Rectangle mouseCursor = new((int)(_resources.Input.GameWorldMousePosition.X - 10f), (int)(_resources.Input.GameWorldMousePosition.Y - 10f), 20, 20);
        PrimitiveRenderer.DrawRectangle(gameTime, _resources.GameplayRenderer, mouseCursor, Color.Yellow, Layers.ForcedFront);

        _resources.GameplayRenderer.End();


        // GUI layer
        _resources.GuiRenderer.Begin(SpriteSortMode.BackToFront, samplerState: SamplerState.PointClamp, transformMatrix: _resources.VirtualScreen.TransformationMatrix);

        _guiManager.Draw(gameTime, _resources.GuiRenderer);

        _resources.GuiRenderer.End();


        // Front layer (border bars)
        _resources.BaseRenderer.Begin(SpriteSortMode.BackToFront, samplerState: SamplerState.PointClamp);

        _resources.VirtualScreen.DrawBorderBars(gameTime, _resources.BaseRenderer);

        _resources.BaseRenderer.End();




        base.Draw(gameTime);
    }
}
