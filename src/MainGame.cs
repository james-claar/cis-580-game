using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace cis_580_game;

public class MainGame : Game
{
    public Resources Resources;
    private GuiManager _guiManager;

    public MainGame()
    {
        Resources = new()
        {
            Graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width,
                PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height
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

        Resources.ScaledRenderer?.SetWindowSize(Window.ClientBounds.Width, Window.ClientBounds.Height);

        Window.ClientSizeChanged += Window_ClientSizeChanged;
    }

    protected override void Initialize()
    {
        Resources.Initialize();

        _guiManager = new(Resources);

        base.Initialize();
    }

    protected override void LoadContent()
    {
        Resources.SpriteBatch = new(GraphicsDevice);

        Resources.ScaledRenderer = new(Resources.Graphics.PreferredBackBufferWidth, Resources.Graphics.PreferredBackBufferHeight, Resources.SpriteBatch);
        Resources.LoadContent(Content);
        _guiManager.LoadContent(Content);
    }

    protected override void Update(GameTime gameTime)
    {
        // Allow user to exit using buttons
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        _guiManager.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        Resources.SpriteBatch.Begin(SpriteSortMode.BackToFront);

        _guiManager.Draw(gameTime, Resources.SpriteBatch);

        Resources.ScaledRenderer.DrawScreenBorderBars(gameTime, GameColors.WindowBorderColor, Layers.ForcedFront);

        PrimitiveRenderer.DrawRectangle(gameTime, Resources.SpriteBatch, (Rectangle)Resources.ScaledRenderer._gameplayBounds, GameColors.BackgroundColor, Layers.ForcedBack);

        Resources.SpriteBatch.End();

        base.Draw(gameTime);
    }
}
