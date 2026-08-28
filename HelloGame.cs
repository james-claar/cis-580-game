using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace cis_580_game;

public class HelloGame : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Texture2D _texture;
    private Vector2 _position;
    private Vector2 _direction;

    public HelloGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        base.Initialize();
    }

    protected override void LoadContent()
    {
        MathHelper.Random random = new ();

        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here

        _texture = Content.Load<Texture2D>("fireball_0");

        _position = new Vector2(
            random.NextFloat() * (GraphicsDevice.Viewport.Width),
            random.NextFloat() * (GraphicsDevice.Viewport.Height)
        );

        _direction = new Vector2(
            100 * random.NextFloat(),
            100 * random.NextFloat()
        );
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here
        _position += _direction * (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (_position.X < 0 || _position.X > GraphicsDevice.Viewport.Width) // Omitted "- _texture.Width" because texture is big
        {
            _direction.X *= -1;
        }

        if (_position.Y < 0 || _position.Y > GraphicsDevice.Viewport.Height) // Omitted "- _texture.Height" because texture is big
        {
            _direction.Y *= -1;
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Purple);

        // TODO: Add your drawing code here
        _spriteBatch.Begin();

        _spriteBatch.Draw(_texture, _position, Color.White);

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
