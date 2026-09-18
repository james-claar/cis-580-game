using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace cis_580_game;

public class MainGame : Game
{
    private Resources _resources;
    private GuiManager _guiManager;

    public MainGame()
    {
        _resources = new()
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
        Window.ClientSizeChanged += new EventHandler<EventArgs>(Window_ClientSizeChanged);
    }

    /// <summary>
    /// Event triggered when the user resizes the game window
    /// </summary>
    /// <param name="sender">The sender of the event</param>
    /// <param name="e">The event arguments</param>
    protected void Window_ClientSizeChanged(object sender, EventArgs e)
    {
        Window.ClientSizeChanged -= Window_ClientSizeChanged;

        _resources.ScaledRenderer?.SetWindowSize(Window.ClientBounds.Width, Window.ClientBounds.Height);

        Window.ClientSizeChanged += Window_ClientSizeChanged;
    }

    protected override void Initialize()
    {
        _guiManager = new(_resources);

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _resources.SpriteBatch = new(GraphicsDevice);

        _resources.Game = this;
        _resources.ScaledRenderer = new(_resources.Graphics.PreferredBackBufferWidth, _resources.Graphics.PreferredBackBufferHeight, _resources.SpriteBatch);
        _resources.LoadContent(Content);
        _guiManager.LoadContent(Content);
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

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        if (_resources.IsLoaded)
        {
            _resources.SpriteBatch.Begin(SpriteSortMode.BackToFront);

            _guiManager.Draw(gameTime, _resources.SpriteBatch);
            _resources.ScaledRenderer.DrawScreenBorderBars(gameTime, GameColors.WindowBorderColor, Layers.ForcedFront);

            _resources.SpriteBatch.End();
        }

        base.Draw(gameTime);
    }
}
