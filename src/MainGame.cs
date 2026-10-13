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
        _resources.Input.Update(gameTime);
        _guiManager.Update(gameTime);

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

        // Update input when drawing, instead of game tick

        // Update camera position
        Vector2 newCameraPosition =
            _resources.PlayerShip.Position
            + Vector2.Transform(new(0f, -0.2f*VirtualScreen.Height), Matrix.Invert(Matrix.CreateRotationZ(-_resources.PlayerShip.Angle + MathHelper.PiOver2) * Matrix.CreateScale(_resources.Camera.Zoom,-_resources.Camera.Zoom,_resources.Camera.Zoom)))
            // + 0.3f * _resources.PlayerShip.Velocity / _resources.Camera.Zoom
            // + 0.25f * Vector2.Transform(_resources.Input.VirtualMousePosition - VirtualScreen.Center, _resources.Camera.RelativeTransformationMatrix) / (_resources.Camera.Zoom * _resources.Camera.Zoom)
        ;
        _resources.Camera.SetCameraPosition(newCameraPosition, _resources.PlayerShip.Angle - MathHelper.PiOver2, _resources.Camera.Zoom);
        _resources.Input.UpdateMousePosition();


        // Gameplay layer
        _resources.GameplayRenderer.Begin(SpriteSortMode.BackToFront, samplerState: SamplerState.PointClamp, transformMatrix: _resources.Camera.TransformationMatrix, rasterizerState: RasterizerState.CullClockwise);

        _resources.PlayerShip.Draw(gameTime, _resources.GameplayRenderer);

        // Debug: Render origin
        PrimitiveRenderer.DrawRectangle(gameTime, _resources.GameplayRenderer, new Rectangle(-10,-5,60,10), Color.Red, Layers.ForcedFront);
        PrimitiveRenderer.DrawRectangle(gameTime, _resources.GameplayRenderer, new Rectangle(-5,-10,10,60), Color.White, Layers.ForcedFront);
        int spacing = 200;
        int count = 50;
        int radius = 3;
        for (int i = -count; i < count; i++)
        {
            for (int j = -count; j < count; j++)
            {
                PrimitiveRenderer.DrawRectangle(gameTime, _resources.GameplayRenderer, new Rectangle(i*spacing-(radius-1),j*spacing-(radius-1),2*(radius-1)+1,2*(radius-1)+1), Color.LightGray, Layers.ForcedFront);
            }
        }

        // Debug: Render mouse cursor
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
